'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp')
  .controller('baseController',['$scope','$state','authService', function ($scope, $state,authService) {

      //$scope.allowRequests = authService.authentication.userType == 'Client'; // 0 client , 1 trade

      //$scope.goToWorkRequest = function(){
      //    $state.go('base.workRequest');
      //}

     
  }]);
