package com.helppen.service;

import com.helppen.model.Task;

import java.util.List;

public interface TaskService {

    List<Task> getTasksForUser(String userName);

    Task update(Task task);

    Task create(String ownerName, String text);

    boolean delete(String taskId);


}
