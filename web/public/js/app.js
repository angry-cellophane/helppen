'use strict';

angular.module('helppen', [
  'ngRoute',
  'helppen.tasks',
  'ngMaterial',
  'ngCookies'
])
  .config(['$routeProvider', function($routeProvider) {
    $routeProvider.otherwise({redirectTo: '/tasks'});
  }])
  .controller('MainCtrl', ['$scope', '$cookies', '$cookieStore', function($scope, $cookie, $cookieStore) {
    $scope.logout = function() {
      $cookie.remove('authToken');
      window.location = '/';
    };
  }]);
