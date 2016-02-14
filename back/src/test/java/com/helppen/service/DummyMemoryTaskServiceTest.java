package com.helppen.service;

import com.helppen.model.Task;
import com.helppen.model.TaskState;
import org.junit.Assert;
import org.junit.Test;

import java.util.List;

import static org.junit.Assert.*;

public class DummyMemoryTaskServiceTest {

    @Test
    public void test() {
        DummyMemoryTaskService taskService = new DummyMemoryTaskService();
        String username = "Bob";
        String text = "dummy test";
        Task task = taskService.create(username, text);

        Assert.assertEquals(TaskState.NOT_COMPLETED,task.getState());
        Assert.assertEquals(username,task.getCreatedBy());
        Assert.assertEquals(text,task.getText());

        List<Task> tasks = taskService.getTasksForUser(username);
        Assert.assertEquals(1, tasks.size());
        Assert.assertEquals(task,tasks.get(0));

        String newText = text + " 2";
        task.setText(newText);
        taskService.update(task);
        List<Task> newTasks = taskService.getTasksForUser(username);
        Assert.assertEquals(1, tasks.size());

        Assert.assertEquals(task,newTasks.get(0));

        taskService.delete(task.getId());
        Assert.assertEquals(0, taskService.getTasksForUser(username).size());
    }

}