package com.helppen.auth.token;

import java.time.ZonedDateTime;

public class UserInfoFromToken {

    private final String userName;
    private final ZonedDateTime expirationDate;

    public UserInfoFromToken(String userName, ZonedDateTime expirationDate) {
        this.userName = userName;
        this.expirationDate = expirationDate;
    }

    public String getUserName() {
        return userName;
    }

    public ZonedDateTime getCreationDate() {
        return expirationDate;
    }
}
