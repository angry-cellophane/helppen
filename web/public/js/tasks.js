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
    return $resource('/api/tasks/:id', { id: '@id' }, {
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
      console.log('typeof tasks == undefined ' + (typeof tasks == 'undefined'));
      if (typeof tasks == 'undefined') return 0;
      console.log('tasks.length < 1 ' + (tasks.length < 1));
      if (tasks.length < 1) return 0;
      console.log('typeof tasks[0].orderNumber == undefined ' + (typeof tasks[0].orderNumber == 'undefined'));
      if (typeof tasks[0].orderNumber == 'undefined') return 0;

      console.log('max number: ' + tasks[0].orderNumber);
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
      if ($scope.tasks[0] === task) return;

      for (var i in $scope.tasks) {
        if ($scope.tasks[i] !== task) continue;

        var toUp = new Task(task);
        var toDown = new Task($scope.tasks[i - 1]);

        var temp = toUp.orderNumber;

        toUp.orderNumber = toDown.orderNumber;
        toDown.orderNumber = temp;

        Tasks.update(toUp, function(newToUp) {
          Tasks.update(toDown, function(newToDown) {
            toUp = newToUp.orderNumber;
            toDown = newToDown.orderNumber;
            $scope.tasks.splice(i, 1);
            $scope.tasks.splice(i - 1, 0, task);
          });
        });

        return;
      }
      console.log('Task ' + task.id + ' not found');
    };

    $scope.update = function(task) {
      for (var i in $scope.tasks) {
        if ($scope.tasks[i] !== task) continue;

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

    $scope.remove = function(task) {
      for (var i in $scope.tasks) {
        if ($scope.tasks[i] !== task) continue;

        Tasks.delete(task, function(data) {
          $scope.tasks.splice(i, 1);
        });

        return;
      };

      console.log('the task ' + task + ' is not found');
    };

    $scope.moveToStash = function (task) {
      for (var i in $scope.tasks) {
        if ($scope.tasks[i] !== task) continue;

        var dto = new Task(task);
        dto.state = 'STASH';
        dto.orderNumber = findMaxNumber($scope.stash) + 1;

        Tasks.update(dto, function (res) {
          $scope.stash.unshift(dto);
          $scope.tasks.splice(i, 1);
        });
        return;
      }
    };

    $scope.moveFromStash = function (task) {
      for (var i in $scope.stash) {
        if ($scope.stash[i] !== task) continue;

        var dto = new Task(task);
        dto.state = 'NOT_COMPLITED';
        dto.orderNumber = findMaxNumber($scope.tasks) + 1;

        Tasks.update(dto, function (res) {
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
