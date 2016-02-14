package com.helppen.auth;

import org.junit.Assert;
import org.junit.Test;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;

import java.util.Collections;
import java.util.Optional;
import java.util.concurrent.TimeUnit;


public class AuthTokenUserDetailsProviderImplTest {

    @Test
    public void testTokenNotExpired() {
        AuthTokenUserDetailsProviderImpl authProvider = new AuthTokenUserDetailsProviderImpl();

        authProvider.setTokenService(new Base64TokenService());
        authProvider.setUserService(username -> new User(username, "", Collections.emptyList()));

        String userName = "Alex";
        String token = authProvider.tokenForUserName(userName);
        Optional<UserDetails> userDetails = authProvider.loadUserDetailsByToken(token);
        Assert.assertTrue(userDetails.isPresent());
        Assert.assertEquals(userDetails.get().getUsername(), userName);
    }

    @Test
    public void testTokenExpired() throws InterruptedException {
        AuthTokenUserDetailsProviderImpl authProvider = new AuthTokenUserDetailsProviderImpl();

        authProvider.setTokenService(new Base64TokenService());
        authProvider.setUserService(username -> new User(username, "", Collections.emptyList()));
        authProvider.setExpirationDurationInSec(3);

        String userName = "Alex";
        String token = authProvider.tokenForUserName(userName);

        TimeUnit.SECONDS.sleep(4);

        Optional<UserDetails> userDetails = authProvider.loadUserDetailsByToken(token);
        Assert.assertFalse(userDetails.isPresent());
    }

}