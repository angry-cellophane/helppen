(function() {

   angular
          .module('tasks')
          .controller('TaskController', [
             'taskService', '$scope', '$log', '$q', 
              TaskController
          ]);


   function TaskController(taskService, $scope, $log, $q) {
      var self = this;
      self.scope = $scope;
      self.addNewTask = addNewTask;
      self.taskService = taskService;
      self.moveUp = moveUp;
      self.remove = remove;

      taskService.loadAllTasks()
		.then(function(tasks) {
			self.tasks = [].concat(tasks);
		});

   }

   function addNewTask() {
      var newText = this.scope.newTaskText;
      var newTask = this.taskService.createNewTask(newText);
      this.tasks.unshift(newTask);
   }

   function moveUp(id) {
      this.taskService.moveUp(id); 
            var i  = 0;
            for (var t in this.tasks) {
               if (this.tasks[t].id == id) {
                  var task = this.tasks[t];
                  this.tasks.splice(i, 1);
                  this.tasks.splice(i -1, 0, task);
                  return;
               }
               i++;
            }

   }
   
   function remove(id) {
      this.taskService.moveUp(id); 
            var i  = 0;
            for (var t in this.tasks) {
               if (this.tasks[t].id == id) {
                  var task = this.tasks[t];
                  this.tasks.splice(i, 1);
                  return;
               }
               i++;
            }

   }

})();

      
