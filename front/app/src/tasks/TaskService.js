(function(){
   'use strict';

   angular.module('tasks')
          .service('userService', ['$q', TaskService]);


   function TaskService($q){
      var tasks = [
         {
            id: '1',
            title: 'Task #1',
            desc: 'Task #1 Description',
            isDone: false,
         },
         {
            id: '2',
            title: 'Task #2',
            desc: 'Task #2 Description',
            isDone: false,
         },
         {
            id: '3',
            title: 'Task #3',
            desc: 'Task #3 Description',
            isDone: false,
         }
      ];
      return {
         loadAllTasks: function() {
            return $q.when(tasks);
         }
      };     
   }
})();

