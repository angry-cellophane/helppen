package com.helppen.auth.controller;

import com.helppen.auth.token.AuthTokenUserDetailsProvider;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.*;

@RestController
public class TokenProviderController {

    static class TokenInput {
        private String username;
        private String password;

        public String getUsername() {
            return username;
        }

        public void setUsername(String username) {
            this.username = username;
        }

        public String getPassword() {
            return password;
        }

        public void setPassword(String password) {
            this.password = password;
        }
    }

    static class TokenOutput {
        private final String token;

        TokenOutput(String token) {
            this.token = token;
        }

        public String getToken() {
            return token;
        }
    }

    private static Logger LOGGER = Logger.getLogger(TokenProviderController.class);

    @Autowired
    private AuthTokenUserDetailsProvider userDetailsProvider;

    @RequestMapping(value = "/auth/login", method = RequestMethod.POST)
    public ResponseEntity<TokenOutput> login(@RequestBody TokenInput input) {
        if (StringUtils.isEmpty(input.getUsername()) || StringUtils.isEmpty(input.getPassword())) {
            return ResponseEntity.badRequest().body(null);
        }

        LOGGER.info("username = "+input.getUsername());
        LOGGER.info("password = "+input.getPassword());

        TokenOutput output = new TokenOutput(userDetailsProvider.tokenForUserName(input.getUsername()));
        return ResponseEntity.ok(output);
    }
}
