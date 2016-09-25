'use strict';


angular.module('sbAdminApp').controller('companyUserController', ['$scope', '$location', '$timeout', '$state', 'Notification', 'companyService',
    function ($scope, $location, $timeout, $state, Notification, companyService) {

        $scope.editMode = false;

        $scope.gotoEditCompanyUser = function () {
            $state.go('base.editCompanyUser');
        }


        $scope.filterTextModel = {
            searchText: undefined,
        };

        $scope.search = function (item) {
            if (!$scope.filterTextModel.searchText
                || (item.firstName && (item.firstName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
                || (item.lastName && (item.lastName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
                ) {
                return true;
            }
            return false;
        };

        $scope.userList = [];
    
        $scope.companyInfo = {
            companyName: "Company",
            description: undefined,
            creditCard: undefined,
           // companyId: undefined,

        };

        $scope.addNewUser = function () {
            
            $state.go("editCompanyUser");

        }

   

    var getUsersInCompany = function () {
        companyService.getCompanyUsers().then(function (users) {

            $scope.userList = users;
        },function (error) { Notification.error({ message: error, delay: 2000 }); });
    }
    var getCompanyDetail = function () {
        companyService.getCurrentCompany().then(function (company) {
            $scope.companyInfo.companyName = company.companyName;
            $scope.companyInfo.description = company.description;
            $scope.companyInfo.creditCard = company.creditCard;
            //$scope.companyInfo.companyId = company.companyId;

        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    // get the company info for display
    getCompanyDetail();
        
    //get user list for the current company
    getUsersInCompany();


    }

]);