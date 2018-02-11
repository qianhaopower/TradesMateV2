'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */

angular.module('sbAdminApp')
  .directive('sidebar', ['$interval', 'authService', 'messageService', function ($interval, authService, messageService) {
    return {
      templateUrl:'scripts/directives/sidebar/sidebar.html',
      restrict: 'E',
      replace: true,
      scope: {
      },
      controller:function($scope){
        $scope.selectedMenu = 'dashboard';
        $scope.collapseVar = 0;
        $scope.multiCollapseVar = 0;
        
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
        
        $scope.multiCheck = function(y){
          
          if(y==$scope.multiCollapseVar)
            $scope.multiCollapseVar = 0;
          else
            $scope.multiCollapseVar = y;
        };
      }
    }
  }]);
