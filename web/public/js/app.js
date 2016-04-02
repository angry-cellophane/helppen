'use strict';

angular.module('helppen', [
  'ngRoute',
  'helppen.loginForm',
  'helppen.tasks',
  'ngMaterial'
])
  .config(['$routeProvider', function($routeProvider) {
    $routeProvider.otherwise({redirectTo: '/loginForm'});
  }])
  .controller('MainCtrl', function($scope, $timeout) {
   
  });

