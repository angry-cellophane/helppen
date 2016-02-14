package com.helppen.service;


import com.helppen.model.Task;
import com.helppen.model.TaskState;
import org.springframework.stereotype.Component;

import javax.annotation.PostConstruct;
import java.util.*;

@Component
public class DummyMemoryTaskService implements TaskService {

    private final Map<String, List<Task>> tasksByUserName;
    private final Map<String, Task> tasksById;

    public DummyMemoryTaskService() {
        tasksByUserName = new HashMap<>();
        tasksById = new HashMap<>();
    }

    @PostConstruct
    public void init() {
        for (int i = 0; i < 10; i++) {
            create("Alex", "Task #" + i);
        }
    }

    @Override
    public synchronized List<Task> getTasksForUser(String userName) {
        List<Task> tasks = tasksByUserName.get(userName);
        return tasks == null? Collections.emptyList() : tasks;
    }

    @Override
    public synchronized Task update(Task task) {
        Task persistTask = tasksById.get(task.getId());

        persistTask.setText(task.getText());
        persistTask.setState(task.getState());

        return task;
    }

    @Override
    public synchronized Task create(String ownerName, String text) {
        Task task = new Task();

        task.setId(UUID.randomUUID().toString());
        task.setText(text);
        task.setState(TaskState.NOT_COMPLETED);
        task.setCreatedBy(ownerName);

        tasksById.put(task.getId(), task);
        List<Task> tasks = tasksByUserName.get(ownerName);
        if (tasks == null) {
            tasks = new ArrayList<>();
            tasksByUserName.put(ownerName, tasks);
        }
        tasks.add(task);

        return task;
    }

    @Override
    public synchronized boolean delete(String taskId) {
        Task task = tasksById.remove(taskId);
        if (task == null) return false;

        List<Task> tasks = tasksByUserName.get(task.getCreatedBy());
        return tasks.remove(task);
    }
}
