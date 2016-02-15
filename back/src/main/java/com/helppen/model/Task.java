package com.helppen.model;

import javax.persistence.Entity;
import javax.persistence.Id;

@Entity
public class Task {

    @Id
    private String id;
    private String text;
    private TaskState state;
    private String owner;
    private int orderNumber;

    public Task() {}

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getText() {
        return text;
    }

    public void setText(String text) {
        this.text = text;
    }

    public TaskState getState() {
        return state;
    }

    public void setState(TaskState state) {
        this.state = state;
    }

    public String getOwner() {
        return owner;
    }

    public void setOwner(String userName) {
        this.owner = userName;
    }

    public int getOrderNumber() {
        return orderNumber;
    }

    public void setOrderNumber(int orderNumber) {
        this.orderNumber = orderNumber;
    }

    @Override
    public String toString() {
        return "Task{" +
                "id='" + id + '\'' +
                ", text='" + text + '\'' +
                ", state=" + state +
                ", owner='" + owner + '\'' +
                ", orderNumber=" + orderNumber +
                '}';
    }
}
