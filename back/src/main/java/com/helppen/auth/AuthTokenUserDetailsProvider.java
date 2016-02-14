package com.helppen.auth;

import org.springframework.security.core.userdetails.UserDetails;

import java.util.Optional;

public interface AuthTokenUserDetailsProvider {
    Optional<UserDetails> loadUserDetailsByToken(String token);
    String tokenForUserName(String userName);
}
