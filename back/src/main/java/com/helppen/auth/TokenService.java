package com.helppen.auth;

import java.time.ZonedDateTime;

public interface TokenService {
    String encode(String username, long creationTime);
    UserInfoFromToken decode(String token);
}
