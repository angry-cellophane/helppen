package com.helppen;


import org.springframework.boot.Banner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.util.Collections;

@SpringBootApplication
public class App {

    public static void main(String[] args) {
        SpringApplication app = new SpringApplication();
        app.setBannerMode(Banner.Mode.OFF);
        app.setMainApplicationClass(App.class);
        app.setSources(Collections.singleton(App.class));
        app.run(args);
    }
}
