'use strict';

angular.module('helppen.loginForm', ['ngRoute', 'ngMaterial', 'ngCookies'])
  .config(['$routeProvider', '$locationProvider', function($routeProvider, $locationProvider) {
    $routeProvider.when('/loginForm', {
      templateUrl: 'views/loginForm.html',
      controller: 'LoginFormCtrl'
    });
  }])

  .controller('LoginFormCtrl', ['$scope', '$http', '$cookies', '$cookieStore', '$location', function($scope, $http, $cookie, $cookieStore, $location) {
    $scope.formData = {};
    $scope.isCredentialsWrong = false;

    $scope.submitCredentials = function() {
      $http({
        method: 'POST',
        url: 'auth/token',
        data: $scope.formData,
        headers: {'Content-Type': 'application/json'}
      }).then(function success(res) {
        $scope.isCredentialsWrong = false;
        $cookie.put('authToken', res.data.token);
        console.log($location);
        $location.path('/tasks');
      }, function failure(res) {
        $scope.isCredentialsWrong = true;
      });
    }
  }]); 

