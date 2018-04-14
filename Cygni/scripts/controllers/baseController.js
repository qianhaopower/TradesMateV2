'use strict';
/**
 * @ngdoc function
 * @name sbAdminApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the sbAdminApp
 */
angular.module('sbAdminApp')
  .controller('baseController',['$scope','$state','authService','$interval', 'messageService', 
  function ($scope, $state,authService,$interval, messageService) {

      //$scope.allowRequests = authService.authentication.userType == 'Client'; // 0 client , 1 trade

      //$scope.goToWorkRequest = function(){
      //    $state.go('base.workRequest');
      //}

      $scope.currentUser = authService.authentication;
      $scope.showManage = authService.authentication.userRole == 'Admin';
      $scope.showClients = authService.authentication.userType == 'Trade' && authService.authentication.userRole == 'Admin'; // 0 client , 1 trade
      $scope.unReadMessageCount = 0;
    
      var checkCount = function () {
          messageService.getPendingMessagesCount().then(function (count) {
              $scope.unReadMessageCount = count;
          }, function (error) {
              // Get message count fail do nothing.
          });
      } 

      $scope.checkEverySecond = 60 *60;
      $interval(checkCount, 1000 * $scope.checkEverySecond);


      $scope.checkCount = function(x){
        
        if(x==$scope.collapseVar)
          $scope.collapseVar = 0;
        else
          $scope.collapseVar = x;
      };

      $scope.logOut = function () {
        authService.logOut();
        $state.go('login');
    }

    $scope.authentication = authService.authentication;

     
  }]);
