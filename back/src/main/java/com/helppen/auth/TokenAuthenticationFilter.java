package com.helppen.auth;

import org.apache.log4j.Logger;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.util.AntPathMatcher;
import org.springframework.util.StringUtils;

import javax.servlet.*;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;

public class TokenAuthenticationFilter implements Filter {

    private static final Logger LOGGER = Logger.getLogger(TokenAuthenticationFilter.class);

    private final TokenService tokenService;

    private final String filterUrlPattern;
    private final AntPathMatcher pathMatcher;

    public TokenAuthenticationFilter(TokenService tokenService, String filterUrlPattern) {
        this.tokenService = tokenService;
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
            if (StringUtils.isEmpty(authToken)) {
                httpResponse.setStatus(HttpServletResponse.SC_UNAUTHORIZED);
                return;
            }

            String username = tokenService.decode(authToken).split(":")[0];
            SecurityContextHolder.getContext().setAuthentication(new UsernamePasswordAuthenticationToken(username, authToken));
        }

        chain.doFilter(request, response);
    }

    @Override
    public void destroy() {

    }
}
