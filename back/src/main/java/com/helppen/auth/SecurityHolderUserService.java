package com.helppen.auth;

import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.User;
import org.springframework.stereotype.Component;

@Component
public class SecurityHolderUserService implements UserService {

    @Override
    public String getUserName() {
        return SecurityContextHolder.getContext().getAuthentication().getName();
    }
}
