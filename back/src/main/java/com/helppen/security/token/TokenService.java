package com.helppen.security.token;

import org.springframework.security.core.userdetails.User;

public interface TokenService {
    String createTokenFrom(User userDetails);
    User getUserFromToken(String token);
}
