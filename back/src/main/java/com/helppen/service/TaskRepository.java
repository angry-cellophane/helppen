package com.helppen.service;


import com.helppen.model.Task;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.CrudRepository;
import org.springframework.data.rest.core.annotation.RepositoryRestResource;

import java.util.List;
import java.util.Optional;

@RepositoryRestResource(path = "api/v1.0/tasks", collectionResourceRel = "tasks")
public interface TaskRepository extends CrudRepository<Task, String> {
    List<Task> findByOwner(String owner);

    @Query("select max(t) from Task t where t.owner = ?")
    Optional<Task> findMaxOrder(String createdBy);
}
