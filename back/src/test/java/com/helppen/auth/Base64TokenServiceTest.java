package com.helppen.auth;

import com.helppen.auth.token.Base64TokenService;
import com.helppen.auth.token.UserInfoFromToken;
import org.junit.Assert;
import org.junit.Test;

import java.time.ZonedDateTime;

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