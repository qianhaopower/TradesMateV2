'use strict';


angular.module('sbAdminApp').controller('workRequestController', ['$scope', '$location', '$timeout', '$state', 'Notification', 'companyService', 'propertyDataService',
    function ($scope, $location, $timeout, $state, Notification, companyService, propertyDataService) {



        $scope.discard = function () {
            $state.go('base.dashboard');
        }
        $scope.serviceTypes = companyService.getDefaultServices();
        $scope.companyList = [];
        $scope.clientId = undefined;
        $scope.propertyList = [];

        $scope.requestInfo = {
            propertyId: undefined,
            propertyAddress: undefined,//propertyId and propertyAddress, must provide one
            companyId: undefined,
            tradeType: undefined, // must select one and only one 
            sectionId: undefined, //must select one
            description: undefined,


        };


        var init = function () {
            //need get companyList, for now we can get all companies as there are not too many.
            companyService.getCompanies().then(function (result) {
                $scope.companyList = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });

            //need get all the property for the current user has
          
          
            propertyDataService.getAllProperties().then(function (result) {
                $scope.propertyList = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });


        }

        init();

    }

]);