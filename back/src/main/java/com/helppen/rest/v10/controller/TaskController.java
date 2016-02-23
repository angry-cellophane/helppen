package com.helppen.rest.v10.controller;

import com.helppen.auth.user.UserService;
import com.helppen.model.Task;
import com.helppen.service.TaskService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/api/v1.0/tasks")
public class TaskController {

    private final UserService userService;
    private final TaskService taskService;

    @Autowired
    public TaskController(UserService userService, TaskService taskService) {
        this.userService = userService;
        this.taskService = taskService;
    }


    @RequestMapping(method = RequestMethod.GET)
    public List<Task> getTasks() {
        return taskService.getTasksForUser(userService.getUserName());
    }

    @RequestMapping(method = RequestMethod.POST)
    public Task createTask(@RequestBody Task task) {
        return taskService.create(userService.getUserName(), task.getText());
    }

    @RequestMapping(value = "/{taskId}", method = RequestMethod.PUT)
    public Task update(@PathVariable String taskId, @RequestBody Task newTask) {
        Optional<Task> taskOp = taskService.get(taskId);
        if (!taskOp.isPresent()) return null;

        Task task = taskOp.get();
        task.setText(newTask.getText());
        task.setState(newTask.getState());
        task.setOrderNumber(newTask.getOrderNumber());

        return taskService.update(task);
    }

    @RequestMapping(value = "/{taskId}", method = RequestMethod.DELETE)
    public void delete(@PathVariable String taskId) {
        taskService.delete(taskId);
    }
}
