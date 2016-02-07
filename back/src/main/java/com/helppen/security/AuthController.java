package com.helppen.security;

import com.helppen.security.token.TokenConstants;
import com.helppen.security.token.TokenService;
import org.apache.log4j.Logger;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.User;
import org.springframework.stereotype.Controller;
import org.springframework.util.StringUtils;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;

import javax.servlet.http.HttpServletResponse;
import java.util.Collections;

@Controller
public class AuthController {

    private static final Logger LOGGER = Logger.getLogger(AuthController.class);

    @Autowired
    private TokenService tokenService;

    @RequestMapping(value = "/auth/login", method = RequestMethod.POST)
    public void login(@RequestParam String username,@RequestParam String password, HttpServletResponse response) {
        if (StringUtils.isEmpty(username) || StringUtils.isEmpty(password)) {
            response.setStatus(HttpServletResponse.SC_UNAUTHORIZED);
            LOGGER.info("Attempt to login: "+username +": "+password+" ;");
            return;
        }

        User userDetails = new User(username, password, Collections.emptyList());
        String token = tokenService.createTokenFrom(userDetails);

        response.addHeader(TokenConstants.AUTH_HEADER_NAME, token);
    }

}
