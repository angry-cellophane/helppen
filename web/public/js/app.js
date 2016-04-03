'use strict';

angular.module('helppen', [
    'ngRoute',
    'helppen.tasks',
    'ngMaterial',
    'ngCookies'
  ])
  .config(['$routeProvider', '$mdThemingProvider', function($routeProvider, $mdThemingProvider) {
    $routeProvider.otherwise({
      redirectTo: '/tasks'
    });
    $mdThemingProvider.theme('default')
      .primaryPalette('teal')
      .accentPalette('deep-orange');
  }])
  .controller('MainCtrl', ['$scope', '$cookies', '$cookieStore', function($scope, $cookie, $cookieStore) {
    $scope.logout = function() {
      $cookie.remove('authToken');
      window.location = '/';
    };
  }]);
