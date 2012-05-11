package net.link.safeonline.sdk.auth.protocol.oauth2;

import static net.link.safeonline.sdk.configuration.SDKConfigHolder.config;

import com.google.common.base.Function;
import com.google.gson.*;
import com.google.gson.reflect.TypeToken;
import com.lyndir.lhunath.opal.system.logging.Logger;
import com.lyndir.lhunath.opal.system.logging.exception.InternalInconsistencyException;
import java.io.*;
import java.lang.reflect.Type;
import java.net.*;
import java.security.*;
import java.util.*;
import javax.net.ssl.*;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import net.link.safeonline.sdk.api.attribute.AttributeSDK;
import net.link.safeonline.sdk.auth.protocol.*;
import net.link.safeonline.sdk.auth.protocol.oauth2.lib.OAuth2Message;
import net.link.safeonline.sdk.auth.protocol.oauth2.lib.exceptions.OauthInvalidMessageException;
import net.link.safeonline.sdk.auth.protocol.oauth2.lib.messages.*;
import net.link.safeonline.sdk.configuration.*;
import net.link.util.error.ValidationFailedException;


/**
 * TODO description
 * <p/>
 * Date: 09/05/12
 * Time: 14:46
 *
 * @author: sgdesmet
 */
public class OAuth2ProtocolHandler implements ProtocolHandler {

    private static final Logger logger = Logger.get( OAuth2ProtocolHandler.class );

    private static final Gson gson;

    static {
        GsonBuilder gsonBuilder = new GsonBuilder().disableHtmlEscaping()
                                                   .generateNonExecutableJson()
                                                   .setFieldNamingPolicy( FieldNamingPolicy.LOWER_CASE_WITH_UNDERSCORES )
                                                   .setPrettyPrinting();
        gson = gsonBuilder.create();
    }

    private AuthenticationContext authnContext;

    @Override
    public AuthnProtocolRequestContext sendAuthnRequest(final HttpServletResponse response, final AuthenticationContext context)
            throws IOException {

        logger.dbg( "send oauth authn request" );

        authnContext = context;

        String targetURL = getTargetUrl();
        String landingURL = getLandingUrl();

        String authnService = ConfigUtils.getLinkIDAuthURLFromPath( config().proto().oauth2().authorizationPath() );

        // create oauht2 authorization request ( authorization grant code flow)
        AuthorizationRequest authorizationRequest = new AuthorizationRequest( OAuth2Message.ResponseType.CODE, context.getApplicationName());
        authorizationRequest.setRedirectUri( landingURL );
        String state = UUID.randomUUID().toString();
        authorizationRequest.setState( state );
        // TODO set scope
        boolean paramsInBody = config().proto().oauth2().binding().contains( "POST" );
        MessageUtils.sendRedirectMessage( authnService,authorizationRequest,response, paramsInBody );

        return new AuthnProtocolRequestContext( authorizationRequest.getState(), authorizationRequest.getClientId() , this, targetURL );
    }

    @Override
    public AuthnProtocolResponseContext findAndValidateAuthnResponse(final HttpServletRequest request)
            throws ValidationFailedException {

        logger.dbg( "Find and validate OAuth2 response" );

        if (authnContext == null)
            // This protocol handler has not sent an authentication request.
            return null;

        boolean success = false;
        // get the message
        ResponseMessage responseMessage = null;
        try {
            responseMessage = MessageUtils.getAuthorizationCodeResponse( request );
        }
        catch (OauthInvalidMessageException e) {
            throw new ValidationFailedException( "Unexpected response message: ", e );
        }

        // get state from message
        String state = null;
        if (responseMessage instanceof ErrorResponse) {
            state = ((ErrorResponse) responseMessage).getState();
        } else if (responseMessage instanceof AuthorizationCodeResponse) {
            state = ((AuthorizationCodeResponse) responseMessage).getState();
        }
        if (null == state) {
            throw new ValidationFailedException( "Oauth2 response did not contain a state." );
        }

        //does the state match messages we've sent?
        AuthnProtocolRequestContext authnRequest = ProtocolContext.findContext( request.getSession(), state );
        if (authnRequest == null || !state.equals( authnRequest.getId() ))
            throw new ValidationFailedException( "Request's OAuth2 response state does not match that of any active requests." );
        logger.dbg( "OAuth2 response matches request: " + authnRequest );

        String userId = "";
        String applicationName = authnRequest.getIssuer();
        Map<String, List<AttributeSDK<?>>> attributes = new HashMap<String, List<AttributeSDK<?>>>(  );
        if (!(responseMessage instanceof ErrorResponse)) {
            try{
                //ok, got a response, and it's valid, now for the rest of the OAuth flow
                //fetch access + refresh token using the code (include credentials)
                AccessTokenResponse tokenResponse = getAccessToken( ((AuthorizationCodeResponse) responseMessage).getCode(), authnRequest );
                String accessToken = tokenResponse.getAccessToken();

                //validate access token (we need to get the linkid userid (and verify application name), this is provided here)
                ValidationResponse validationResponse = validateToken( accessToken, authnRequest );
                userId = validationResponse.getUserId();

                //call attribute service with our token
                attributes = getAttributes( accessToken );
                success = true;
            }catch (Exception e){
                throw new InternalInconsistencyException( e );
            }
        } else {
            success = userRefusedAccess( (ErrorResponse) responseMessage );
        }

        return new AuthnProtocolResponseContext( authnRequest, state, userId, applicationName, null, attributes, success, null );
    }

    protected boolean userRefusedAccess(ErrorResponse errorResponse) throws ValidationFailedException{
        if (OAuth2Message.ErrorType.ACCESS_DENIED == errorResponse.getErrorType())
            return true;
        else{
            logger.err( "Received error response for OAuth authorization request: " + errorResponse.toString() );
            throw new ValidationFailedException( "Error response returned for OAuth authorization request: " +errorResponse.getErrorDescription() );
        }
    }

    protected AccessTokenResponse getAccessToken(String code, AuthnProtocolRequestContext authnRequest)
            throws IOException, NoSuchAlgorithmException, KeyManagementException, KeyStoreException, ValidationFailedException {

        AccessTokenRequest tokenRequest = new AccessTokenRequest();
        tokenRequest.setCode( code );
        tokenRequest.setGrantType( OAuth2Message.GrantType.AUTHORIZATION_CODE );
        tokenRequest.setRedirectUri( getLandingUrl() );

        String endpoint = ConfigUtils.getLinkIDAuthURLFromPath( config().proto().oauth2().tokenPath() );

        ResponseMessage tokenResponse = MessageUtils.sendRequestMessage( endpoint, tokenRequest,
                authnContext.getOauth2().getSslCertificate(), authnRequest.getIssuer(), config().proto().oauth2().clientSecret() );

        if (tokenResponse instanceof ErrorResponse) {
            logger.err( "Received error response for OAuth authorization request: " + tokenResponse.toString() );
            throw new ValidationFailedException( "Error response returned for OAuth authorization request: "
                                                 + ((ErrorResponse) tokenResponse).getErrorDescription() );
        }

        return (AccessTokenResponse) tokenResponse;
    }

    protected String getTargetUrl(){
        String targetURL = authnContext.getTarget();
        if (targetURL == null || !URI.create( targetURL ).isAbsolute())
            targetURL = ConfigUtils.getApplicationURLForPath( targetURL );
        logger.dbg( "target url: %s", targetURL );
        return targetURL;
    }

    protected String getLandingUrl(){
        String targetURL = getTargetUrl();
        String landingURL = null;
        if (config().web().landingPath() != null)
            landingURL = ConfigUtils.getApplicationConfidentialURLFromPath( config().web().landingPath() );
        logger.dbg( "landing url: %s", landingURL );

        if (landingURL == null) {
            // If no landing URL is configured, land on target.
            landingURL = targetURL;
        }
        return landingURL;
    }

    protected ValidationResponse validateToken(String accessToken, AuthnProtocolRequestContext authnRequest)
            throws IOException, NoSuchAlgorithmException, KeyManagementException, KeyStoreException, ValidationFailedException {

        ValidationRequest validationRequest = new ValidationRequest();
        validationRequest.setAccessToken( accessToken );
        String endpoint = ConfigUtils.getLinkIDAuthURLFromPath( config().proto().oauth2().validationPath() );
        ResponseMessage validationResponse = MessageUtils.sendRequestMessage( endpoint, validationRequest,
                authnContext.getOauth2().getSslCertificate(), authnRequest.getIssuer(), config().proto().oauth2().clientSecret() );

        if (validationResponse instanceof ErrorResponse) {
            logger.err( "Received error response for OAuth authorization request: " + validationResponse.toString() );
            throw new ValidationFailedException( "Error response returned for OAuth authorization request: "
                                                 + ((ErrorResponse) validationResponse).getErrorDescription() );
        }
        if ( !authnRequest.getIssuer().equals( ((ValidationResponse) validationResponse).getAudience() ) ){
            throw new ValidationFailedException( "OAuth token is not for this application" );
        }
        Long expiresIn = ((ValidationResponse)validationResponse).getExpiresIn();
        if ( null == expiresIn || expiresIn <= 0 ){
            throw new ValidationFailedException( "OAuth access token has expired" );
        }

        return (ValidationResponse) validationResponse;
    }

    /**
     * Get attributes from the OAuth attribte service. This service is not part of the oauth spec, so
     * this method is not present in {@code MessageUtils} from the OAuth lib
     * @return
     */
    protected Map<String, List<AttributeSDK<?>>> getAttributes(String accessToken)
            throws IOException, NoSuchAlgorithmException, KeyManagementException, KeyStoreException, ValidationFailedException {

        URL endpointURL = new URL( ConfigUtils.getLinkIDAuthURLFromPath( config().proto().oauth2().attributesPath() ));
        HttpsURLConnection connection = (HttpsURLConnection)endpointURL.openConnection();
        SSLContext sslContext = SSLContext.getInstance( "SSL" );
        TrustManager trustManager = new MessageUtils.OAuthCustomTrustManager( authnContext.getOauth2().getSslCertificate() );
        TrustManager[] trustManagers = { trustManager };
        sslContext.init( null, trustManagers, null );
        connection.setSSLSocketFactory( sslContext.getSocketFactory() );
        if ( null == authnContext.getOauth2().getSslCertificate()){
            connection.setHostnameVerifier( new HostnameVerifier() {
                @Override
                public boolean verify(final String s, final SSLSession sslSession) {

                    logger.wrn( "Warning: URL Host: " + s + " vs. " + sslSession.getPeerHost() );
                    return true;
                }
            } );
        }
        connection.setDoOutput( true );
        connection.setDoInput( true );
        connection.setAllowUserInteraction( false );
        connection.setUseCaches( false );
        connection.setInstanceFollowRedirects( false );
        connection.setRequestMethod( MessageUtils.HttpMethod.GET.toString() );
        connection.setRequestProperty( "Authorization", "Bearer " + accessToken );
        PrintWriter contentWriter = new PrintWriter( connection.getOutputStream() );
        contentWriter.close();

        InputStreamReader reader = new InputStreamReader( connection.getInputStream() );
        Type type = new TypeToken<HashMap<String, ArrayList<AttributeSDK<Serializable>>>>(){}.getType();
        Map<String, List<AttributeSDK<?>>> attributes = gson.fromJson( reader, type );
        logger.dbg( "attributes: " + attributes );
        reader.close();
        return attributes;
    }

    @Override
    public AuthnProtocolResponseContext findAndValidateAuthnAssertion(final HttpServletRequest request,
                                                                      final Function<AuthnProtocolResponseContext, AuthenticationContext> responseToContext)
            throws ValidationFailedException {

        return null;  //To change body of implemented methods use File | Settings | File Templates.
    }

    @Override
    public LogoutProtocolRequestContext sendLogoutRequest(final HttpServletResponse response, final String userId,
                                                          final LogoutContext context)
            throws IOException {

        return null;  //To change body of implemented methods use File | Settings | File Templates.
    }

    @Override
    public LogoutProtocolResponseContext findAndValidateLogoutResponse(final HttpServletRequest request)
            throws ValidationFailedException {

        return null;  //To change body of implemented methods use File | Settings | File Templates.
    }

    @Override
    public LogoutProtocolRequestContext findAndValidateLogoutRequest(final HttpServletRequest request,
                                                                     final Function<LogoutProtocolRequestContext, LogoutContext> requestToContext)
            throws ValidationFailedException {

        return null;  //To change body of implemented methods use File | Settings | File Templates.
    }

    @Override
    public LogoutProtocolResponseContext sendLogoutResponse(final HttpServletResponse response,
                                                            final LogoutProtocolRequestContext logoutRequestContext,
                                                            final boolean partialLogout)
            throws IOException {

        return null;  //To change body of implemented methods use File | Settings | File Templates.
    }
}
