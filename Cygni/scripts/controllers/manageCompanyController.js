'use strict';


angular.module('sbAdminApp').controller('manageCompanyController', ['$scope', '$location', '$timeout', '$state', 'Notification', 'companyService','storageService',
    function ($scope, $location, $timeout, $state, Notification, companyService,storageService) {

        $scope.editMode = false;

        $scope.gotoManagerCompanyUser = function () {
            $state.go('base.companyMember');
        }
        $scope.gotoManageWorkItemTemplate = function () {
            $state.go('base.companyWorkItemTemplate');
        }
        $scope.serviceTypes = companyService.getDefaultServices();
       

    $scope.companyInfo = {
        tradeTypes: $scope.serviceTypes,
    };
    $scope.companyInfoClone = {};
   
    $scope.updateCompany = function () {
        $scope.editMode = false;
        if ($scope.companyInfoForm.$invalid) return;

        $scope.companyInfo.tradeTypes = _.pluck($scope.outputServiceTypes, 'enumValue');
        companyService.updateCompany($scope.companyInfo).then(function (response) {
            getCompanyDetail();
            Notification.success({ message: "Saved", delay: 2000 });
           
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    $scope.cancelEdit = function () {
        $scope.editMode = false;
        //change it back to original.
        $scope.companyInfo = $scope.companyInfoClone;
    }
    $scope.attachmentType  = 'CompanyLogo';
    $scope.uploadedFile = function (element) {
        $scope.$apply(function ($scope) {
            $scope.files = element.files;

            storageService.uploadFile($scope.files[0],  $scope.companyInfo.companyId,$scope.attachmentType)
                .then(function () {
                    Notification.success({ message: "Upload successful", delay: 2000 });
                    getCompanyDetail();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        });
    }
    $scope.hasType = function(enumNumber){
        return _.any($scope.companyInfo.tradeTypes, function(item){return item.enumValue == enumNumber && item.ticked})
    }
    var getCompanyDetail = function () {
        companyService.getCurrentCompany().then(function (company) {
            $scope.companyInfo.companyName = company.companyName;
            $scope.companyInfo.description = company.description;
            $scope.companyInfo.creditCard = company.creditCard;
            $scope.companyInfo.companyId = company.companyId;
            $scope.companyInfo.abn = company.abn;
            $scope.companyInfo.address = company.address;
            $scope.companyInfo.website = company.website;
            $scope.companyInfo.companyLogoUrl = company.companyLogoUrl;
            $scope.companyInfo.electricianLicense = company.electricianLicense;
            $scope.companyInfo.plumberLicense = company.plumberLicense;
            $scope.companyInfo.handymanLicense = company.handymanLicense;
            $scope.companyInfo.builderLicense = company.builderLicense;
            $scope.companyInfo.airconditioningLicense = company.airconditioningLicense;


            //grab all of the default
            $scope.companyInfo.tradeTypes = $scope.serviceTypes;
            
            angular.forEach($scope.companyInfo.tradeTypes, function (value, key) {
                /* do your stuff here */
                if (company.tradeTypes.indexOf(value.enumValue) > -1) {
                    value.ticked = true;
                } else {
                    value.ticked = false;
                }
               
            });

            //$scope.companyInfo.serviceCurrentSelected = _.filter($scope.serviceTypes, function (item) {
            //    return company.tradeTypes.indexOf(item.enumValue) > -1;
            //});

            $scope.companyInfoClone = JSON.parse(JSON.stringify($scope.companyInfo));
          


           
        },function (error) { Notification.error({ message: error, delay: 2000 }); });
    }
    getCompanyDetail();
    }

]);