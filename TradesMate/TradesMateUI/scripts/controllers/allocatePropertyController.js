'use strict';

angular.module('sbAdminApp')
  .controller('allocatePropertyController', ['$scope', 'companyService', 'authService',
      'Notification', '$state', '$stateParams', 'propertyDataService',
function ($scope, companyService, authService, Notification, $state, $stateParams, propertyDataService) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.memberId = undefined;
    $scope.member = undefined;
    $scope.allocations = [];
   
    $scope.goBack = function () {
        $state.go('base.companyMember');
    }
    $scope.updateAllocation = function (info) {
        if ($scope.memberId) {

            //  var clientCopy= angular.copy($scope.client);
            propertyDataService.updateMemberAllocation(info.propertyId, $scope.memberId, info.allocated).then(function (result) {
                Notification.success({ message: "Updated", delay: 2000 });
                init();
            }, function (error) {
                Notification.error({ message: error.message, delay: 2000 });
            });
        }
      
    
    }
    

    var init = function () {
        $scope.memberId = $stateParams.memberId;
        if ($scope.memberId ) {
            companyService.getMemberById($scope.memberId).then(function (result) {
                $scope.member = result;
            }, function (error) {
                Notification.error({ message: error, delay: 2000 });
            });

            //  var clientCopy= angular.copy($scope.client);
            propertyDataService.getMemberAllocation($scope.memberId).then(function (result) {
                $scope.allocations = result;
            }, function (error) {
                Notification.error({ message: error.message, delay: 2000 });
            });
        } 
    }


    init();

}]);
