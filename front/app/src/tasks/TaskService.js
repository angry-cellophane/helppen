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
         for (var i in tasks) {
            if (tasks[i].id > maxId) maxId = tasks[i].id;
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
         },
         moveUp: function (id) {
            var i  = 0;
            for (var t in tasks) {
               if (tasks[t].id == id) {
                  var task = tasks[t];
                  tasks.slice(i, 1);
                  tasks.splice(i -1, 0, task);
                  return;
               }
               i++;
            }
         }   
      };     
   }
})();
