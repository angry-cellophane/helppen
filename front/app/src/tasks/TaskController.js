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

})();

      
