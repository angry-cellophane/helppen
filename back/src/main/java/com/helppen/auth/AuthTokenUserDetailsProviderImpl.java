package com.helppen.auth;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.stereotype.Component;
import org.springframework.util.StringUtils;

import java.time.ZoneOffset;
import java.time.ZonedDateTime;
import java.util.Optional;
import java.util.concurrent.TimeUnit;

@Component
public class AuthTokenUserDetailsProviderImpl implements AuthTokenUserDetailsProvider {

    @Autowired
    private TokenService tokenService;

    @Autowired
    private UserDetailsService userService;

    private long expirationDurationInSec = TimeUnit.HOURS.toSeconds(4);

    @Override
    public Optional<UserDetails> loadUserDetailsByToken(String token) {
        if (StringUtils.isEmpty(token)) return Optional.empty();

        UserInfoFromToken user = tokenService.decode(token);

        ZonedDateTime now = ZonedDateTime.now(ZoneOffset.UTC);
        if (now.toEpochSecond() - user.getCreationDate().toEpochSecond() > expirationDurationInSec) return Optional.empty();

        UserDetails userDetails = userService.loadUserByUsername(user.getUserName());
        return Optional.ofNullable(userDetails);
    }

    @Override
    public String tokenForUserName(String userName) {
        long now = ZonedDateTime.now(ZoneOffset.UTC).toEpochSecond();
        return tokenService.encode(userName, now);
    }

    public void setTokenService(TokenService tokenService) {
        this.tokenService = tokenService;
    }

    public void setUserService(UserDetailsService userService) {
        this.userService = userService;
    }

    public void setExpirationDurationInSec(long expirationDurationInSec) {
        this.expirationDurationInSec = expirationDurationInSec;
    }
}
