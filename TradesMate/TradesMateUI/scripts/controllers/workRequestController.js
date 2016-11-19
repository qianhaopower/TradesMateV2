'use strict';


angular.module('sbAdminApp').controller('workRequestController', ['$scope', '$location', '$timeout', '$state', 'Notification',
    'companyService', 'propertyDataService','addressService',
    function ($scope, $location, $timeout, $state, Notification, companyService, propertyDataService, addressService) {



        $scope.discard = function () {
            $state.go('base.dashboard');
        }
        $scope.serviceTypes = companyService.getDefaultServices();
        $scope.companyList = [];
        $scope.clientId = undefined;
        $scope.propertyList = [];
        $scope.isNewProperty = false;
        $scope.selectedProperty = undefined;
        $scope.newPropertyAddress = undefined;
        $scope.outputServiceType = undefined;
        $scope.selectedCompany = undefined;

    
        $scope.requestInfo = {
            propertyId: undefined,
            propertyAddress: undefined,//propertyId and propertyAddress, must provide one
            companyId: undefined,
            tradeType: undefined, // must select one and only one 
            sectionId: undefined, //must select one
            description: undefined,
            isNewProperty:false

        };

        $scope.companyFilterFunc = function () {

            return function (item) {
                var found = _.some(item.tradeTypes, function (companyType) {
                    return companyType == $scope.outputServiceType[0].enumValue;
                });
                if (found) {
                    return true;
                }
                return false;
            };
          
        }


        $scope.findAddress = function (search) {
            return addressService.findAddress(search).then(function (results) {
               
                return results;
                //$scope.searchResult = members;
            }, function (error) {
                // no need to display no results
                //Notification.error({ message: error, delay: 2000 });
            });
        }

        var init = function () {
            //need get companyList, for now we can get all companies as there are not too many.
            companyService.getCompanies().then(function (result) {
                $scope.companyList = result;
        
                if (result.length > 0) {
                    $scope.selectedCompany = result[0];
                }
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });

            //need get all the property for the current user has
          
          
            propertyDataService.getAllProperties().then(function (result) {
                $scope.propertyList = result;
                if (result.length > 0) {
                    $scope.selectedProperty = result[0];
                }
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });


        }

        init();

    }

]);