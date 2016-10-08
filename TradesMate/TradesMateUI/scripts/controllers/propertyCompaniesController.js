'use strict';

angular.module('sbAdminApp')
  .controller('propertyCompaniesController', ['$scope', 'propertyDataService', 'clientDataService',
      'Notification', '$state', '$stateParams',
function ($scope, propertyDataService, clientDataService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.propertyId = undefined;
    $scope.property = undefined;
    
    $scope.companies = [];

    $scope.goBack = function () {
        $state.go('base.viewProperty', { propertyId: $scope.propertyId });
    }

    
    var init = function () {

        $scope.propertyId = $stateParams.propertyId;
        propertyDataService.getPropertyById($scope.propertyId).then(function (result) {
            $scope.property = result;
            $scope.clientId = result.clientId;

            if ($scope.clientId) {
                clientDataService.getClientById($scope.clientId).then(function (result) {
                    $scope.client = result

                }, function (error) { Notification.error({ message: error, delay: 2000 }); });
            }

        }, function (error) { Notification.error({ message: error, delay: 2000 }); });


        propertyDataService.getPropertyCompanies($scope.propertyId).then(function (result) {
            $scope.companies = result;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);
