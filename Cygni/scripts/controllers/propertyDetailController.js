'use strict';

angular.module('sbAdminApp')
    .controller('propertyDetailController', ['$scope', 'propertyDataService', 'clientDataService','storageService',
      'Notification', '$state', '$stateParams',
      function ($scope, propertyDataService, clientDataService, storageService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.propertyId = undefined;
    $scope.property = undefined;
    $scope.clientId = undefined;
    $scope.client = undefined;
    $scope.status = {};


    $scope.readOnly = $state.current.name == 'base.viewProperty';

    $scope.goBack = function () {
        $state.go('base.properties');
    }

    $scope.gotoViewPropertyCompany = function () {
        $state.go('base.propertyCompanies', { propertyId: $scope.propertyId });
    }
    

    $scope.save = function () {
      //  $scope.property.address = undefined;

        if ($scope.property.isNew) {
            var propertyCopy = angular.copy($scope.property);
            propertyCopy.clientId = $stateParams.clientId;
            propertyCopy.defaultSections = $scope.defaultSections;
            propertyDataService.createProperty(propertyCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 2000 });
                $scope.goToPropertyIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else {
            propertyDataService.editProperty($scope.property).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToPropertyIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }
    
    }
    $scope.goToPropertyIndex = function () {
        if ($scope.property.clientId)
            $state.go('base.clientProperties', { param: $scope.property.clientId });
        else
            $state.go('base.properties');
    };


    var init = function () {

        if ($state.current.name == 'base.createProperty') {
            $scope.clientId = $stateParams.clientId;
        } else {
            $scope.propertyId = $stateParams.propertyId;
        }
       
        if ($state.current.name == 'base.editProperty' || $state.current.name == 'base.viewProperty') {
            propertyDataService.getPropertyById($scope.propertyId).then(function (result) {
                $scope.property = result;
                $scope.clientId = result.clientId;
                if (result.address)
                {
                    $scope.property.addressDisplay = "{0} {1} {2} {3} {4} {5}".format(
                        result.address.line1,
                           result.address.line2,
                              '',//result.address.line3,
                                 result.address.suburb,
                                    result.address. state,
                                       result.address.postCode
                        )
                }

                if ($scope.clientId) {
                    clientDataService.getClientById($scope.clientId).then(function (result) {
                        $scope.client = result

                    }, function (error) { Notification.error({ message: error, delay: 2000 }); });
                }

            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'base.createProperty') {
            //create new property
            $scope.property = {
                condition:'Normal',//default
                isNew: true,
                address: {},
            }
            $scope.defaultSections = {
                bedroomNumber: 0,
                livingRoomNumber: 0,
                bathroomNumber: 0,
                kitchenNumber: 0,
                laundryRoomNumber: 0,
                hallWayNumber: 0,
                deckNumber: 0,
                basementNumber: 0,
                gardenNumber: 0,
                garageNumber: 0,
            };
        }
        //propertyDataService.getAllPropertys().then(function (result) {
        //    $scope.propertylist = result;
        //}, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);
