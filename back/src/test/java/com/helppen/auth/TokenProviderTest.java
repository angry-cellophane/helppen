package com.helppen.auth;


import com.helppen.model.Task;
import com.helppen.rest.v10.controller.TaskController;
import feign.*;
import feign.jackson.JacksonDecoder;
import org.springframework.boot.json.JacksonJsonParser;

import java.util.List;
import java.util.Objects;

public class TokenProviderTest {

    static class TokenWrapper {
        String token;

        public String getToken() {
            return token;
        }

        public void setToken(String token) {
            this.token = token;
        }
    }

    interface TokenProvider {
        @RequestLine("POST auth/login")
        @Headers("Content-type: application/json")
        TokenWrapper newToken(String content);
    }

    interface Tasks {
        @RequestLine("GET /api/v1.0/tasks")
        @Headers("Content-type: application/json")
        List<Task> get();
    }

    public static void main(String[] args) {
        TokenProvider tokenClient = Feign
                .builder()
                .decoder(new JacksonDecoder())
                .target(TokenProvider.class, "http://localhost:8080");

        String input = "{\"username\":\"Alex\", \"password\":\"password\"}";
        TokenWrapper alexToken = tokenClient.newToken(input);
        System.out.println(alexToken.getToken());

        RequestInterceptor authTokenSupplier = new RequestInterceptor() {
            @Override
            public void apply(RequestTemplate template) {
                template.header(Consts.AUTH_TOKEN_NAME, alexToken.getToken());
            }
        };
        Tasks tasksClient = Feign.builder()
                .decoder(new JacksonDecoder())
                .requestInterceptor(authTokenSupplier)
                .target(Tasks.class, "http://localhost:8080");

        System.out.println(tasksClient.get());
    }
}
