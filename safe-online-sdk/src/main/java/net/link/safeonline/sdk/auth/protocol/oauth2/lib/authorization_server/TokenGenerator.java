package net.link.safeonline.sdk.auth.protocol.oauth2.lib.authorization_server;

import net.link.safeonline.sdk.auth.protocol.oauth2.lib.data.objects.*;


/**
 * TODO description
 * <p/>
 * Date: 22/03/12
 * Time: 17:15
 *
 * @author: sgdesmet
 */
public interface TokenGenerator {
    
    public CodeToken createCode(ClientAccessRequest accessRequest);

    public AccessToken createAccessToken(ClientAccessRequest accessRequest);

    public RefreshToken createRefreshToken(ClientAccessRequest accessRequest);

}
