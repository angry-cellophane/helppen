package com.helppen.auth;

import org.springframework.security.crypto.codec.Base64;
import org.springframework.stereotype.Component;

import java.time.ZonedDateTime;

@Component
public class Base64TokenService implements TokenService {
    @Override
    public String token(String username) {
        long time  = ZonedDateTime.now().toEpochSecond();

        byte[] encode = Base64.encode((username + ":" + time).getBytes());
        return new String(encode);
    }
}
