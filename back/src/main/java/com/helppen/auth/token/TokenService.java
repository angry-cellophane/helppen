package com.helppen.auth.token;

public interface TokenService {
    String encode(String username, long creationTime);
    UserInfoFromToken decode(String token);
}
