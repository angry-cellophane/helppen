package com.helppen.auth.controller;

import com.helppen.auth.Consts;
import com.helppen.auth.token.AuthTokenUserDetailsProvider;
import org.apache.log4j.Logger;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.util.AntPathMatcher;

import javax.servlet.*;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.Optional;

public class TokenAuthenticationFilter implements Filter {

    private static final Logger LOGGER = Logger.getLogger(TokenAuthenticationFilter.class);

    private final AuthTokenUserDetailsProvider userDetailsProvider;

    private final String filterUrlPattern;
    private final AntPathMatcher pathMatcher;

    public TokenAuthenticationFilter(AuthTokenUserDetailsProvider userDetailsProvider, String filterUrlPattern) {
        this.userDetailsProvider = userDetailsProvider;
        this.filterUrlPattern = filterUrlPattern;
        this.pathMatcher = new AntPathMatcher();
    }

    @Override
    public void init(FilterConfig filterConfig) throws ServletException {

    }

    @Override
    public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain) throws IOException, ServletException {
        HttpServletRequest httpRequest = (HttpServletRequest) request;
        HttpServletResponse httpResponse = (HttpServletResponse) response;

        String requestURI = httpRequest.getRequestURI();
        boolean isFiltered = pathMatcher.match(filterUrlPattern, requestURI);

        if (isFiltered) {
            String authToken = httpRequest.getHeader(Consts.AUTH_TOKEN_NAME);
            Optional<UserDetails> userDetailsOptional = userDetailsProvider.loadUserDetailsByToken(authToken);
            if (!userDetailsOptional.isPresent()) {
                httpResponse.setStatus(HttpServletResponse.SC_UNAUTHORIZED);
                return;
            }

            UserDetails userDetails = userDetailsOptional.get();
            UsernamePasswordAuthenticationToken authentication =
                    new UsernamePasswordAuthenticationToken(
                            userDetails.getUsername(),
                            userDetails.getPassword(),
                            userDetails.getAuthorities());
            SecurityContextHolder
                    .getContext()
                    .setAuthentication(authentication);
        }

        chain.doFilter(request, response);
    }

    @Override
    public void destroy() {

    }
}
