package net.link.safeonline.client.sdk.device.qr;

public interface QRDevice {

    String NAME = "qr";

    // WS_Authentication
    String WS_REGISTRATION_ID = "urn:net:lin-k:safe-online:qr:ws:auth:registrationId";
    String WS_SESSION_ID      = "urn:net:lin-k:safe-online:qr:ws:auth:sessionId";
    String WS_QR_CODE         = "urn:net:lin-k:safe-online:qr:ws:auth:qrCode";
}
