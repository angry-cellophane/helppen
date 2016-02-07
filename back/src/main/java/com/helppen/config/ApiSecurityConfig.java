package com.helppen.config;


import com.helppen.security.CustomRememberMeServices;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.core.annotation.Order;
import org.springframework.security.authentication.RememberMeAuthenticationProvider;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.config.http.SessionCreationPolicy;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.web.authentication.Http403ForbiddenEntryPoint;
import org.springframework.security.web.authentication.rememberme.RememberMeAuthenticationFilter;
import org.springframework.security.web.authentication.www.BasicAuthenticationFilter;

import java.util.concurrent.CompletableFuture;

//@Configuration
//@EnableWebSecurity
//@Order(1)
public class ApiSecurityConfig extends WebSecurityConfigurerAdapter {

    private static final String tokenKey = "authToken";

    @Autowired
    private UserDetailsService userDetailsService;

    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http
                .antMatcher("/api/**")
                .csrf()
                .disable()
                .authorizeRequests().anyRequest().authenticated()
                .and()
                .addFilterBefore(rememberMeAuthenticationFilter(), BasicAuthenticationFilter.class)
                .sessionManagement().sessionCreationPolicy(SessionCreationPolicy.STATELESS).and()
                .exceptionHandling().authenticationEntryPoint(new Http403ForbiddenEntryPoint());
    }

    @Bean
    public RememberMeAuthenticationFilter rememberMeAuthenticationFilter() throws Exception {
        return new RememberMeAuthenticationFilter(authenticationManager(), tokenBasedRememberMeServices());
    }

    @Bean
    public CustomRememberMeServices tokenBasedRememberMeServices() throws Exception {
        CustomRememberMeServices rememberMeServices = new CustomRememberMeServices(tokenKey, userDetailsService);
        rememberMeServices.setAlwaysRemember(true);
        rememberMeServices.setCookieName("at");
        return rememberMeServices;
    }

    @Bean
    public RememberMeAuthenticationProvider rememberMeAuthenticationProvider() {
        return new RememberMeAuthenticationProvider(tokenKey);
    }


}
