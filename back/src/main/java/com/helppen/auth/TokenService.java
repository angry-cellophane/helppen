package com.helppen.auth;

public interface TokenService {
    String encode(String username);
    String decode(String token);
}
