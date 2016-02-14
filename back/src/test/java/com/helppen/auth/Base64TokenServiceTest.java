package com.helppen.auth;

import org.junit.Assert;
import org.junit.Test;

import static org.junit.Assert.*;

public class Base64TokenServiceTest {

    private final Base64TokenService service = new Base64TokenService();

    @Test
    public void testService() throws Exception {
        String userName = "Alex";
        String encode = service.encode(userName);
        String decode = service.decode(encode);
        String[] parts = decode.split(":");
        Assert.assertEquals(userName, parts[0]);
    }
}