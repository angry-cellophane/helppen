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

    $scope.newTaskText = '';
    $scope.tasks = [];

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

    Tasks.query().$promise.then(function(tasks) {
      angular.forEach(tasks, function(task) {
        task.isDone = task.state === 'COMPLITED';
      });
      $scope.tasks = tasks;
    });

  }]);
