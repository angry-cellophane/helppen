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
      this.isDone = this.state === 'COMPLITED';
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

    $scope.moveUp = function(task) {
      if ($scope.tasks.length === 0 || $scope.tasks[0] === task) return;

      var dto = new Task(task);
      dto.orderNumber = findMaxNumber($scope.tasks) + 1;

      Tasks.update(dto, function(newToUp) {
        $scope.tasks.unshift(dto);
        $scope.tasks.splice($scope.tasks.indexOf(task), 1);
      });

    };

    $scope.update = function(task, tasks) {
      for (var i in tasks) {
        if (tasks[i] !== task) continue;

        var newState = task.isDone ? 'COMPLITED' : 'NOT_COMPLITED';

        var dto = new Task(task);
        dto.state = newState;
        Tasks.update(dto, function(rowData) {
          task.state = newState;
        });

        return;
      }
      console.log('Task ' + task.id + ' not found');
    };

    $scope.remove = function(task, tasks) {
      for (var i in tasks) {
        if (tasks[i] !== task) continue;

        Tasks.delete(task, function(data) {
          tasks.splice(i, 1);
        });

        return;
      };

      console.log('the task ' + task + ' is not found');
    };

    $scope.moveToStash = function(task) {
      for (var i in $scope.tasks) {
        if ($scope.tasks[i] !== task) continue;

        dto.state = 'STASH';
        dto.orderNumber = findMaxNumber($scope.stash) + 1;

        Tasks.update(dto, function(res) {
          $scope.stash.unshift(dto);
          var dto = new Task(task);
          $scope.tasks.splice(i, 1);
        });
        return;
      }
    };

    $scope.moveFromStash = function(task) {
      for (var i in $scope.stash) {
        if ($scope.stash[i] !== task) continue;

        var dto = new Task(task);
        dto.state = 'NOT_COMPLITED';
        dto.orderNumber = findMaxNumber($scope.tasks) + 1;

        Tasks.update(dto, function(res) {
          $scope.tasks.unshift(dto);
          $scope.stash.splice(i, 1);
        });
        return;
      }
    };

    Tasks.query().$promise.then(function(tasks) {
      var actualTasks = [];
      var stashTasks = [];

      angular.forEach(tasks, function(task) {
        if (task.state === 'STASH') {
          stashTasks.push(task);
        } else {
          task.isDone = task.state === 'COMPLITED';
          actualTasks.push(task);
        }
      });

      $scope.tasks = actualTasks;
      $scope.stash = stashTasks;
    });

  }]);
