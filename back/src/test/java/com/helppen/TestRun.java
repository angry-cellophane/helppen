package com.helppen;

import org.springframework.boot.Banner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.util.Collections;

@SpringBootApplication
public class TestRun {

    public static void main(String[] args) {
        SpringApplication app = new SpringApplication();
        app.setBannerMode(Banner.Mode.OFF);
        app.setMainApplicationClass(TestRun.class);
        app.setSources(Collections.singleton(TestRun.class));
        app.run(args);
    }
}
