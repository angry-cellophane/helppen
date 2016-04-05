'use strict';

angular.module('helppen.loginForm', ['ngMaterial', 'ngCookies'])
  .config(['$mdThemingProvider', function($mdThemingProvider) {
    $mdThemingProvider.theme('default')
      .primaryPalette('teal')
      .accentPalette('deep-orange');
  }])
.controller('LoginFormCtrl', ['$scope', '$http', '$cookies', function($scope, $http, $cookies) {
  $scope.formData = {};
  $scope.isCredentialsWrong = false;

  $scope.submitCredentials = function() {
    $http({
      method: 'POST',
      data: $scope.formData,
      url: 'auth/token',
      headers: {
        'Content-Type': 'application/json'
      }
    }).then(function success(res) {
      $scope.isCredentialsWrong = false;
      $cookies.put('authToken', res.data.token);
      // window.location.replace(window.location.href);
      window.location = '/';
      // console.log($location);
    }, function failure(res) {
      $scope.isCredentialsWrong = true;
    });
  }
}]);
