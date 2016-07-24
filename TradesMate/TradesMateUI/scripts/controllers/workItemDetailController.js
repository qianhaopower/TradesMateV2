'use strict';

angular.module('sbAdminApp')
  .controller('workItemDetailController', ['$scope', 'workItemDataService',
      'Notification', '$state', '$stateParams',
function ($scope, workItemDataService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };



    $scope.readOnly = $state.current.name == 'dashboard.viewWorkItem';

    $scope.creatingNew = $state.current.name == 'dashboard.createWorkItem';

    $scope.save = function () {
        $scope.workItem.address = undefined;

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
            workItemDataService.editWorkItem($scope.workItem).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToWorkItemIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }

    }
    $scope.goToWorkItemIndex = function () {
        $state.go('dashboard.workItems', {
            sectionId: $stateParams.sectionId,
            propertyId: $stateParams.propertyId
        });
    };
    $scope.cloneTemplate = function (templateItem) {
        $scope.workItem.name = templateItem.name;
        $scope.workItem.description = templateItem.description;
        $scope.workItem.workItemTemplateId = templateItem.id;
        $scope.workItem.quantity = 1;
    };

    //Electrician 0,
    //  Handyman 1,
    //  Plumber 2,
    $scope.userProfile = {
        tradeType: 'Electrician',
    };

    $scope.search = function (item) {
        if (item.tradeWorkType == $scope.userProfile.tradeType) {
            return true;
        }
        return false;
    };


    var init = function () {

        if ($state.current.name != 'dashboard.createWorkItem') {
            $scope.workItemId = $stateParams.workItemId;
        }

        if ($state.current.name == 'dashboard.editWorkItem'
            || $state.current.name == 'dashboard.viewWorkItem') {
            workItemDataService.getWorkItemById($scope.workItemId).then(function (result) {
                $scope.workItem = result;
                $scope.propertyId = result.propertyId;

                if ($scope.propertyId) {
                    propertyDataService.getPropertyById($scope.propertyId).then(function (result) {
                        $scope.property = result;

                    }, function (error) { Notification.error({ message: error, delay: 2000 }); });
                }

            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'dashboard.createWorkItem') {
            //create new workItem
            $scope.workItem = {
                quantity: 0,//default
                isNew: true,
            }
        }

        workItemDataService.getAllWorkItemTemplates().then(function (result) {
            $scope.templates = result;

        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        
    }


    init();

}]);
