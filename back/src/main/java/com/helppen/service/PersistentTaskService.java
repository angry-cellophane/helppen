package com.helppen.service;

import com.helppen.model.Task;
import com.helppen.model.TaskState;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

import java.util.List;
import java.util.Optional;
import java.util.UUID;

@Component
public class PersistentTaskService implements TaskService {

    private final TaskRepository taskRepository;

    @Autowired
    public PersistentTaskService(TaskRepository taskRepository) {
        this.taskRepository = taskRepository;
    }


    @Override
    public Optional<Task> get(String id) {
        return  Optional.ofNullable(taskRepository.findOne(id));
    }

    @Override
    public List<Task> getTasksForUser(String userName) {
        return taskRepository.findByOwner(userName);
    }

    @Override
    public Task update(Task task) {
        return taskRepository.save(task);
    }

    @Override
    public Task create(String ownerName, String text) {
        int order = taskRepository.findMaxOrder(ownerName).map(x -> x.getOrderNumber() + 1).orElse(0);

        Task task = new Task();
        task.setId(UUID.randomUUID().toString());
        task.setText(text);
        task.setOrderNumber(order);
        task.setState(TaskState.NOT_COMPLETED);
        task.setOwner(ownerName);

        return taskRepository.save(task);
    }

    @Override
    public void delete(String taskId) {
        taskRepository.delete(taskId);
    }
}
