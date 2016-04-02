'use strict';

angular.module('helppen.loginForm', ['ngMaterial', 'ngCookies'])
  .controller('LoginFormCtrl', ['$scope', '$http', '$cookies', function($scope, $http, $cookies) {
    $scope.formData = {};
    $scope.isCredentialsWrong = false;

    $scope.submitCredentials = function() {
      $http({
        method: 'POST',
        data: $scope.formData,
        url: 'auth/token',
        headers: {'Content-Type': 'application/json'}
      }).then(function success(res) {
        $scope.isCredentialsWrong = false;
        $cookies.put('authToken', res.data.token);
        window.location.replace(window.location.href);
        // console.log($location);
      }, function failure(res) {
        $scope.isCredentialsWrong = true;
      });
    }
  }]);
