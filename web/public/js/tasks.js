'use strict';

angular.module('helppen.tasks', ['ngRoute', 'ngMaterial', 'ngCookies'])
  .config(['$routeProvider', function($routeProvider) {
    $routeProvider.when('/tasks', {
      templateUrl: 'views/tasks.html',
      controller: 'TasksCtrl'
    });
  }])

.controller('TasksCtrl', ['$scope', '$http', function($scope, $http) {
  $scope.newTaskText = '';
  $scope.tasks = [];

  $scope.addNewTask = function() {
    if (!$scope.newTaskText) return;

    $http({
      method: 'POST',
      url: 'api/tasks',
      headers: {
        'Content-Type': 'application/json'
      },
      data: {
        text: $scope.newTaskText
      }
    }).then(function success(res) {
      var newTask = res.data;
      newTask.isDone = false;
      $scope.tasks.unshift(newTask);
      $scope.newTaskText = '';
    }, function failure(res) {
      console.log(res);
    });
  };

  $scope.moveUp = function(task) {
    if ($scope.tasks[0] === task) return;

    for (var i in $scope.tasks) {
      if ($scope.tasks[i] !== task) continue;

      var toUp = task;
      var toDown = $scope.tasks[i - 1];

      $http({
        method: 'PUT',
        url: 'api/tasks/' + toUp.id,
        headers: {
          'Content-Type': 'application/json'
        },
        data: {
          id: toUp.id,
          text: toUp.text,
          state: toUp.state,
          orderNumber: toDown.orderNumber,
          ownerId: toUp.ownerId
        }
      }).then(function success(res) {
        $http({
          method: 'PUT',
          url: 'api/tasks/' + toDown.id,
          headers: {
            'Content-Type': 'application/json'
          },
          data: {
            id: toDown.id,
            text: toDown.text,
            state: toDown.state,
            orderNumber: toUp.orderNumber,
            ownerId: toDown.ownerId
          }
        }).then(function success(res) {
          var temp = toUp.orderNumber;
          toUp.orderNumber = toDown.orderNumber;
          toDown.orderNumber = temp;
          $scope.tasks.splice(i, 1);
          $scope.tasks.splice(i - 1, 0, task);
        }, function failure(res) {
          console.log(res);
        });
      }, function failure(res) {
        console.log(res);
      });

      return;
    }
    console.log('Task ' + task.id + ' not found');
  };

  $scope.update = function(task) {
    for (var i in $scope.tasks) {
      if ($scope.tasks[i] !== task) continue;

      var newState = task.isDone ? 'COMPLITED' : 'NOT_COMPLITED';

      $http({
        method: 'PUT',
        url: 'api/tasks/' + task.id,
        headers: {
          'Content-Type': 'application/json'
        },
        data: {
          id: task.id,
          text: task.text,
          state: newState,
          orderNumber: task.orderNumber,
          ownerId: task.ownerId
        }
      }).then(function success(res) {
        task.state = newState;
      }, function failure(res) {
        console.log(res);
      });

      return;
    }
    console.log('Task ' + task.id + ' not found');
  };

  $scope.remove = function(task) {
    for (var i in $scope.tasks) {
      if ($scope.tasks[i] !== task) continue;

      $http({
        method: 'DELETE',
        url: 'api/tasks/' + task.id,
        data: task,
        headers: {
          'Content-Type': 'application/json'
        }
      }).then(function success(res) {
        $scope.tasks.splice(i, 1);
      }, function failure(res) {
        console.log(res);
      });
      return;
    };

    console.log('the task ' + task + ' is not found');
  };

  $http({
    method: 'GET',
    url: 'api/tasks',
    headers: {
      'Content-Type': 'application/json'
    }
  }).then(function success(res) {
    $scope.tasks = res.data;
    for (var i in $scope.tasks) {
      var task = $scope.tasks[i];
      task.isDone = task.state === 'COMPLITED';
    }
  }, function failure(res) {
    console.log(res);
  });

}]);
