'use strict';


angular.module('sbAdminApp').controller('manageCompanyController', ['$scope', '$location', '$timeout', '$state', 'Notification', 'companyService',
    function ($scope, $location, $timeout, $state, Notification, companyService) {

        $scope.editMode = false;

        $scope.gotoManagerCompanyUser = function () {
            $state.go('base.companyMember');
        }


    $scope.companyInfo = {
        companyName: undefined,
        description: undefined,
        creditCard: undefined,
        companyId: undefined,
      
    };
    $scope.companyInfoClone = {};
   
    $scope.updateCompany = function () {
        $scope.editMode = false;
        if ($scope.companyInfoForm.$invalid) return;

        companyService.updateCompany($scope.companyInfo).then(function (response) {

            Notification.success({ message: "Saved", delay: 2000 });
           
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    $scope.cancelEdit = function () {
        $scope.editMode = false;
        //change it back to original.
        $scope.companyInfo = $scope.companyInfoClone;
    }

    var getCompanyDetail = function () {
        companyService.getCurrentCompany().then(function (company) {
            $scope.companyInfo.companyName = company.companyName;
            $scope.companyInfo.description = company.description;
            $scope.companyInfo.creditCard = company.creditCard;
            $scope.companyInfo.companyId = company.companyId;
           
            $scope.companyInfoClone = JSON.parse(JSON.stringify($scope.companyInfo));
           
        },function (error) { Notification.error({ message: error, delay: 2000 }); });
    }
    getCompanyDetail();
    }

]);