package com.helppen.model;


public class Task {
    private String id;
    private String text;
    private TaskState state;
    private String createdBy;

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

    public String getCreatedBy() {
        return createdBy;
    }

    public void setCreatedBy(String userName) {
        this.createdBy = createdBy;
    }

    @Override
    public String toString() {
        return "Task{" +
                "id='" + id + '\'' +
                ", text='" + text + '\'' +
                ", state=" + state +
                ", createdBy='" + createdBy + '\'' +
                '}';
    }
}
