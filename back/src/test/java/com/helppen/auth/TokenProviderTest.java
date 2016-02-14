package com.helppen.auth;


import com.helppen.model.Task;
import com.helppen.rest.v10.controller.TaskController;
import feign.*;

import java.util.List;
import java.util.Objects;

public class TokenProviderTest {

    interface TokenProvider {
        @RequestLine("POST auth/login")
        @Headers("Content-type: application/json")
        String newToken(String content);
    }

    interface Tasks {
        @RequestLine("GET /api/v1.0/tasks")
        @Headers("Content-type: application/json")
        List<Task> get();
    }

    public static void main(String[] args) {
        TokenProvider tokenClient = Feign
                .builder()
                .target(TokenProvider.class, "http://localhost:8080");

        String input = "{\"username\":\"Alex\", \"password\":\"password\"}";
        System.out.println(tokenClient.newToken(input));

        String invalidInput = "{\"username\":\"Alex123\", \"password\":\"password123\"}";
        System.out.println(tokenClient.newToken(invalidInput));

        Tasks tasksClient = Feign.builder()
                .target(Tasks.class, "http://localhost:8080");

        System.out.println(tasksClient.get());
    }
}
