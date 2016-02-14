package com.helppen.auth;

import org.junit.Assert;
import org.junit.Test;

import java.time.Instant;
import java.time.ZoneOffset;
import java.time.ZonedDateTime;
import java.util.Date;

import static org.junit.Assert.*;

public class Base64TokenServiceTest {

    private final Base64TokenService service = new Base64TokenService();

    @Test
    public void testService() throws Exception {
        String userName = "Alex";
        long creationTime = ZonedDateTime.now().toEpochSecond();
        String encode = service.encode(userName, creationTime);
        UserInfoFromToken userDetails = service.decode(encode);

        System.out.println(userDetails.getCreationDate());

        Assert.assertEquals(userName, userDetails.getUserName());
        Assert.assertEquals(creationTime, userDetails.getCreationDate().toEpochSecond());

    }
}