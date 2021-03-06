'use strict';

angular.module('sbAdminApp')
  .controller('workItemDetailController', ['$scope', 'workItemDataService', 'workItemTemplateService', 'companyService',
      'Notification', '$state', '$stateParams',
function ($scope, workItemDataService, workItemTemplateService, companyService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };

    $scope.readOnly = $state.current.name == 'base.viewWorkItem';

    $scope.creatingNew = $state.current.name == 'base.createWorkItem';


    $scope.propertyId = $stateParams.propertyId;

    $scope.save = function () {
        $scope.workItem.address = undefined;
        $scope.workItem.status = _.pluck($scope.outputWorkItemStatus, 'enumValue')[0];
        if ($scope.workItem.isNew) {
            var workItemCopy = angular.copy($scope.workItem);

            workItemCopy.sectionId = $stateParams.sectionId;

            //change this workItem TemplateId from UI
            //workItemCopy.workItemTemplateId = 1;

         
            workItemDataService.createWorkItem(workItemCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 2000 });
                $scope.goToWorkItemIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else {

        //set the work item status value
          
            workItemDataService.editWorkItem($scope.workItem).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToWorkItemIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }

    }
    $scope.goToWorkItemIndex = function () {
        $state.go('base.workItems', {
            sectionId: $stateParams.sectionId,
            propertyId: $stateParams.propertyId
        });
    };
    $scope.cloneTemplate = function (templateItem) {
        $scope.workItem.name = templateItem.name;
        $scope.workItem.description = templateItem.description;
        $scope.workItem.workItemTemplateId = templateItem.id;
        $scope.workItem.quantity = 1;
        $scope.workItem.tradeWorkType = getDefaultWorkItemStatuses( templateItem.tradeWorkType);
    };

    var getDefaultWorkItemStatuses = function (tradeWorkTypeNumber) {

        var type = undefined;
        switch (tradeWorkTypeNumber) {
            case 0:
                type = "Electrician";
                break;
            case 1:
                type = "Handyman";
                break;
            case 2:
                type = "Plumber";
                break;
            case 3:
                type = "Builder";
                break;
            case 4:
                type = "AirConditioning";
                break;
        }
       
        return type;
    }




    $scope.shouldHideOption = function (value) {
        if ($scope.companyInfo && $scope.companyInfo.tradeTypes.indexOf(value) == -1) {
            return true;
        }
        return false;
    }

    //Electrician 0,
    //  Handyman 1,
    //  Plumber 2,
    $scope.userProfile = {
        tradeType: 0,
    };

    $scope.search = function (item) {
        if (item.tradeWorkType == $scope.userProfile.tradeType) {
            return true;
        }
        return false;
    };
    var getCompanyDetail = function () {
        companyService.getCurrentCompany().then(function (company) {
            $scope.companyInfo = company;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    } 

    //work item status
    $scope.workItemStatusList = workItemDataService.getDefaultWorkItemStatuses();
    $scope.outputWorkItemStatus = undefined

    var init = function () {

       
        if ($state.current.name != 'base.createWorkItem') {
            $scope.workItemId = $stateParams.workItemId;
        }

        if ($state.current.name == 'base.editWorkItem'
            || $state.current.name == 'base.viewWorkItem') {
            workItemDataService.getWorkItemById($scope.workItemId).then(function (workItem) {
                $scope.workItem = workItem;
                $scope.propertyId = workItem.propertyId;

                //set up the workItem status
                angular.forEach($scope.workItemStatusList, function (value, key) {
                    /* do your stuff here */
                    if (workItem.status == value.enumValue) {
                        value.ticked = true;
                    } else {
                        value.ticked = false;
                    }
                });



                if ($scope.propertyId) {
                    propertyDataService.getPropertyById($scope.propertyId).then(function (result) {
                        $scope.property = result;
                    }, function (error) { Notification.error({ message: error, delay: 2000 }); });
                }
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'base.createWorkItem') {
            //create new workItem
            $scope.workItem = {
                quantity: 0,//default
                isNew: true,
            }
        }

        workItemTemplateService.getAllTemplates().then(function (result) {
            $scope.templates = result;

        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        getCompanyDetail();
    }


    init();

}]);
