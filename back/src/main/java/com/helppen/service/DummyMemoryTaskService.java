package com.helppen.service;


import com.helppen.model.Task;
import com.helppen.model.TaskState;

import javax.annotation.PostConstruct;
import java.util.*;
import java.util.stream.Collectors;

//@Component
public class DummyMemoryTaskService implements TaskService {

    private final Map<String, List<Task>> tasksByUserName;
    private final Map<String, Task> tasksById;

    private int lastOrder;

    public DummyMemoryTaskService() {
        tasksByUserName = new HashMap<>();
        tasksById = new HashMap<>();
        lastOrder = 0;
    }

    @PostConstruct
    public void init() {
        for (int i = 0; i < 10; i++) {
            create("Alex", "Task #" + i);
        }
    }

    @Override
    public Optional<Task> get(String id) {
        return Optional.ofNullable(tasksById.get(id));
    }

    @Override
    public synchronized List<Task> getTasksForUser(String userName) {
        List<Task> tasks = tasksByUserName.get(userName);
        return Optional.ofNullable(tasks)
                .map(t -> t.stream()
                            .sorted((o1, o2) -> Integer.compare(o1.getOrderNumber(), o2.getOrderNumber()))
                            .collect(Collectors.toList()))
                .orElse(Collections.emptyList());
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
        task.setOwner(ownerName);
        task.setOrderNumber(lastOrder++);

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
    public synchronized void delete(String taskId) {
        Task task = tasksById.remove(taskId);
        if (task == null) return;

        List<Task> tasks = tasksByUserName.get(task.getOwner());
        tasks.remove(task);
    }
}
