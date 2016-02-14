package com.helppen.auth.token;

import org.springframework.security.crypto.codec.Base64;
import org.springframework.stereotype.Component;

import java.time.Instant;
import java.time.ZoneOffset;
import java.time.ZonedDateTime;

@Component
public class Base64TokenService implements TokenService {
    @Override
    public String encode(String username, long creationTime) {
        byte[] encode = Base64.encode((username + ":" + creationTime).getBytes());
        return new String(encode);
    }

    @Override
    public UserInfoFromToken decode(String token) {
        String decoded = new String(Base64.decode(token.getBytes()));
        String[] parts = decoded.split(":");
        String username = parts[0];
        Long creationTime  = Long.parseLong(parts[1]);

        return new UserInfoFromToken(username, ZonedDateTime.ofInstant(Instant.ofEpochSecond(creationTime), ZoneOffset.UTC));
    }
}
