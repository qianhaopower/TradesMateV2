'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp')
  .controller('dashboardController', function ($scope, $state) {

      $scope.goToWorkRequest = function(){
          $state.go('base.workRequest');
      }
  });
