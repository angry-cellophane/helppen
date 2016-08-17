'use strict';

angular.module('helppen.tasks', ['ngRoute', 'ngMaterial', 'ngCookies', 'ngResource'])
  .config(['$routeProvider', '$resourceProvider', function($routeProvider, $resourceProvider) {
    $routeProvider.when('/tasks', {
      templateUrl: 'views/tasks.html',
      controller: 'TasksCtrl'
    });
    $resourceProvider.defaults.stripTrailingSlashes = false;
  }])
  .factory('Tasks', function($resource) {
    return $resource('/api/tasks/:id', {
      id: '@id'
    }, {
      update: {
        method: 'PUT'
      }
    });
  })
  .controller('TasksCtrl', ['$scope', '$http', 'Tasks', function($scope, $http, Tasks) {

    var Task = function(rowData) {
      this.id = rowData.id;
      this.text = rowData.text;
      this.state = rowData.state;
      this.orderNumber = rowData.orderNumber;
      this.ownerId = rowData.ownerId;
      this.isDone = this.state === 'COMPLETED';
    }

    var findMaxNumber = function(tasks) {
      if (typeof tasks == 'undefined') return 0;
      if (tasks.length < 1) return 0;
      if (typeof tasks[0].orderNumber == 'undefined') return 0;

      return tasks[0].orderNumber;
    }

    $scope.newTaskText = '';
    $scope.tasks = [];
    $scope.stash = [];

    $scope.addNewTask = function() {
      if (!$scope.newTaskText) return;

      var newTask = new Task({
        text: $scope.newTaskText
      });
      Tasks.save(newTask).$promise.then(function(rowData) {
        $scope.tasks.unshift(new Task(rowData));
        $scope.newTaskText = '';
      });
    };

    $scope.moveUp = function(task, tasks) {
      if (tasks.length === 0 || tasks[0] === task) return;

      var dto = new Task(task);
      dto.orderNumber = findMaxNumber(tasks) + 1;

      Tasks.update(dto, function(newToUp) {
        tasks.unshift(dto);
        tasks.splice(tasks.indexOf(task), 1);
      });
    };

    $scope.update = function(task, tasks) {
      var i = tasks.indexOf(task);

      if (i === -1) {
        console.log('Task ' + task.id + ' not found');
      } else {
        var newState = task.isDone ? 'COMPLETED' : 'NOT_COMPLETED';
        var dto = new Task(task);
        dto.state = newState;
        Tasks.update(dto, function(rowData) {
          task.state = newState;
        });
      }
    };

    $scope.remove = function(task, tasks) {
      var i = tasks.indexOf(task);

      if (i === -1) {
        console.log('the task ' + task + ' is not found');
      } else {
        Tasks.delete(task, function(data) {
          tasks.splice(i, 1);
        });
      }
    };

    $scope.moveToStash = function(task) {
      var dto = new Task(task);
      dto.state = 'STASH';
      dto.orderNumber = findMaxNumber($scope.stash) + 1;

      Tasks.update(dto, function(res) {
        $scope.stash.unshift(dto);
        $scope.tasks.splice($scope.tasks.indexOf(task), 1);
      });
    };

    $scope.moveFromStash = function(task) {
      var dto = new Task(task);
      dto.state = 'NOT_COMPLETED';
      dto.orderNumber = findMaxNumber($scope.tasks) + 1;

      Tasks.update(dto, function(res) {
        $scope.tasks.unshift(dto);
        $scope.stash.splice($scope.tasks.indexOf(task), 1);
      });
    };

    Tasks.query().$promise.then(function(tasks) {
      var actualTasks = [];
      var stashTasks = [];

      angular.forEach(tasks, function(task) {
        if (task.state === 'STASH') {
          stashTasks.push(task);
        } else {
          task.isDone = task.state === 'COMPLETED';
          actualTasks.push(task);
        }
      });

      $scope.tasks = actualTasks;
      $scope.stash = stashTasks;
    });

  }]);
