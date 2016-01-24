(function(){
   'use strict';

   angular.module('tasks')
          .service('taskService', ['$q', TaskService]);


   function TaskService($q){
      var tasks = [
         {
            id: '1',
            text: 'Task #1',
            isDone: false,
         },
         {
            id: '2',
            text: 'Task #2',
            isDone: false,
         },
         {
            id: '3',
            text: 'Task #3',
            isDone: false,
         }
      ];

      var maxId = function() {
         if (typeof tasks === 'undefined' || tasks.length == 0) return 0;

         var maxId = tasks[0].id;
         for (var task in tasks) {
            if (task.id > maxId) maxId = taskId;
         }
         return maxId;
      }

      return {
         loadAllTasks: function() {
            return $q.when(tasks);
         },
         createNewTask: function(newText) {
            var newId = maxId() + 1;
            var newTask = {id: newId, text: newText, isDone: false};  
            tasks.unshift(newTask);            
            return newTask; 
         }
      };     
   }
})();

