package com.helppen.security.token;

import org.springframework.security.core.userdetails.User;
import org.springframework.security.crypto.codec.Base64;
import org.springframework.stereotype.Component;

import java.util.Collections;

@Component
public class Base64TokenService implements TokenService {

    @Override
    public String createTokenFrom(User user) {
        byte[] bytes = (user.getUsername() + ":"+user.getPassword()).getBytes();
        byte[] encoded = Base64.encode(bytes);
        return new String(encoded);
    }

    @Override
    public User getUserFromToken(String token) {
        byte[] decoded = Base64.decode(token.getBytes());
        String decodedToken = new String(decoded);
        String[] userInfo = decodedToken.split(":");
        return new User(userInfo[0], userInfo[1], Collections.emptyList());
    }
}
