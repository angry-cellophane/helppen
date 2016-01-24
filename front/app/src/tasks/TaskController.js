(function() {

   angular
          .module('tasks')
          .controller('TaskController', [
             'taskService', '$log', '$q',
              TaskController
          ]);


   function TaskController(taskService, $log, $q) {
      var self = this;

      taskService.loadAllTasks()
		.then(function(tasks) {
			self.tasks = [].concat(tasks);
		});

   }
})();

      
