package com.helppen.service;

import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Component;

@Component
public class ContextBasedUserService  implements UserService {

    private static final String ANONYMOUS = "anonymous";

    @Override
    public String getUserName() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (!authentication.isAuthenticated()) return ANONYMOUS;

        return authentication.getName();
    }
}
