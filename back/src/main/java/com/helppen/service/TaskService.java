package com.helppen.service;

import com.helppen.model.Task;

import java.util.List;
import java.util.Optional;

public interface TaskService {

    Optional<Task> get(String id);

    List<Task> getTasksForUser(String userName);

    Task update(Task task);

    Task create(String ownerName, String text);

    void delete(String taskId);
}
