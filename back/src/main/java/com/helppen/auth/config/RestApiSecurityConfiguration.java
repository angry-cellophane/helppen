package com.helppen.auth.config;

import com.helppen.auth.token.AuthTokenUserDetailsProvider;
import com.helppen.auth.controller.TokenAuthenticationFilter;
import com.helppen.auth.user.FakeUserDetailService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;

@Configuration
@EnableWebSecurity
public class RestApiSecurityConfiguration extends WebSecurityConfigurerAdapter {

    @Autowired
    private AuthTokenUserDetailsProvider authTokenUserDetailsProvider;

    @Override
    protected void configure(AuthenticationManagerBuilder auth) throws Exception {
        auth.inMemoryAuthentication()
                .withUser("Alex").password("password").roles("ADMIN");
    }

    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http.authorizeRequests()
                .antMatchers("/api/**").authenticated();
        http.addFilterBefore(
                new TokenAuthenticationFilter(authTokenUserDetailsProvider, "/api/**"),
                UsernamePasswordAuthenticationFilter.class);
        http.csrf().disable();
    }

    @Bean
    public UserDetailsService userDetailsService() {
        return new FakeUserDetailService();
    }
}
