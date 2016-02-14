package com.helppen.rest.v10.controller;

import com.helppen.auth.UserService;
import com.helppen.model.Task;
import com.helppen.model.TaskState;
import com.helppen.service.TaskService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/api/v1.0/tasks")
public class TaskController {

    @Autowired
    private UserService userService;

    @Autowired
    private TaskService taskService;


    @RequestMapping(method = RequestMethod.GET)
    public List<Task> getTasks() {
        return taskService.getTasksForUser(userService.getUserName());
    }

    @RequestMapping(method = RequestMethod.POST)
    public Task createTask(@RequestParam("text") String text) {
        return taskService.create("Alex", text);
    }

    @RequestMapping(value = "/{taskId}", method = RequestMethod.POST)
    public Task update(@PathVariable String taskId, @RequestParam("text") String text, @RequestParam("state") String stateString) {
        TaskState state = TaskState.of(stateString);

        Task task = new Task();
        task.setText(text);
        task.setId(taskId);
        task.setState(state);

        return taskService.update(task);
    }

    @RequestMapping(value = "/{taskId}", method = RequestMethod.DELETE)
    public void delete(@PathVariable String taskId) {
        taskService.delete(taskId);
    }
}
