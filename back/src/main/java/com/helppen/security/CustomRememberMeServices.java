package com.helppen.security;


import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.web.authentication.RememberMeServices;
import org.springframework.security.web.authentication.rememberme.TokenBasedRememberMeServices;
import org.springframework.util.StringUtils;

import javax.servlet.http.HttpServletRequest;

public class CustomRememberMeServices extends TokenBasedRememberMeServices {

    private final String HEADER_SECURITY_TOKEN = "authToken";

    public CustomRememberMeServices(String key, UserDetailsService userDetailsService) {
        super(key, userDetailsService);
    }

    @Override
    protected String extractRememberMeCookie(HttpServletRequest request) {
        String token = request.getHeader(HEADER_SECURITY_TOKEN);
        return StringUtils.isEmpty(token) ? null : token;
    }
}
