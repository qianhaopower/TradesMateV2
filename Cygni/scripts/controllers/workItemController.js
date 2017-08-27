'use strict';

angular.module('sbAdminApp')
  .controller('workItemController', ['$scope', 'workItemDataService', 'Notification',
      '$state', 'ModalService', '$stateParams', 'propertySectionDataService', 'propertyDataService','companyService',
      function ($scope, workItemDataService, Notification, $state, ModalService, $stateParams, propertySectionDataService, propertyDataService, companyService) {



    $scope.outputSelectedStatus = [];
    $scope.outputSelectedServiceTypes = [];
    $scope.availableStatus = workItemDataService.getDefaultWorkItemStatuses();
    $scope.availableServiceTypes = companyService.getDefaultServices();


    _.each($scope.availableStatus, function (status) {
        status.ticked = true;
    });

    $scope.filterTextModel = {
        searchText: undefined,
    };

    $scope.propertyId = $stateParams.propertyId;

    $scope.search = function (item) {
        if (!$scope.filterTextModel.searchText
            || (item.name && (item.name.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            || (item.description && (item.description.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
        ) {

            var selectedStatus = _.pluck($scope.outputSelectedStatus, 'enumValue');
            if (_.contains(selectedStatus, item.status)) {
                var selectedServices = _.pluck($scope.outputSelectedServiceTypes, 'enumValue');
                if (_.contains(selectedServices, item.tradeWorkType)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        return false;
    };

  

    $scope.openWorkItemDetail = function (workItem, readonly) {
        if (readonly)
            $state.go('base.viewWorkItem', {
                workItemId: workItem.id,
                propertyId: $stateParams.propertyId,
                sectionId: $stateParams.sectionId,
            });
        else
            $state.go('base.editWorkItem', {
                workItemId: workItem.id,
                propertyId: $stateParams.propertyId,
                sectionId: $stateParams.sectionId,
            });

    };
    $scope.addNewWorkItem = function () {
        $state.go('base.createWorkItem', {
            propertyId: $stateParams.propertyId,
            sectionId: $stateParams.sectionId,
        });
    }

    $scope.goBack = function () {
        $state.go('base.propertySections', {
            propertyId: $stateParams.propertyId
           
        });
    }
    $scope.deleteWorkItem = function (workItem) {
        var workItemRef = workItem;
        ModalService.showModal({
            templateUrl: 'deleteWorkItemmodal.html',
            controller: "deleteWorkItemModalController"
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result) {
                    $scope.proceedDelete(workItemRef.id);
                }
            });
        });
    }

    $scope.proceedDelete = function (workItemId) {
        workItemDataService.deleteWorkItem(workItemId).then(function (result) {
            Notification.success({ message: 'Deleted', delay: 2000 });
            init();// fetch data again
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    $scope.viewWorkItems = function (workItem) {
        $state.go('base.workItems', {
            workItemId: workItem.id,
            propertyId: $stateParams.propertyId,
            sectionId: $stateParams.sectionId,
        });

    }



    $scope.viewWorkItemAttachments = function (workItem) {
        $state.go('base.workItemAttachments', {
            workItemId: workItem.id,
            propertyIdForWorkItem: $stateParams.propertyId,
            sectionId: $stateParams.sectionId, });
    }


    //work item status
    $scope.workItemStatusList = workItemDataService.getDefaultWorkItemStatuses();
    

    $scope.getWorkItemStatusDisplay = function (enumValue) {

        var displayName = undefined;
        //set up the workItem status
        angular.forEach($scope.workItemStatusList, function (value, key) {
            /* do your stuff here */
            if (enumValue == value.enumValue) {
                displayName =  value.name;
            } 
        });
        return displayName;
    }

    $scope.getWorkItemStatusClass = function (enumValue) {
        var displayName = undefined;
        //set up the workItem status
        angular.forEach($scope.workItemStatusList, function (value, key) {
            /* do your stuff here */
            if (enumValue == value.enumValue) {
                displayName = value.class;
            }
        });
        return displayName;
    }

    var init = function () {
        workItemDataService.getSectionAllWorkItems($stateParams.sectionId).then(function (result) {
            $scope.workItemList = result;

            $scope.availableServiceTypes = _.filter($scope.availableServiceTypes, function (item) {
                return _.some($scope.workItemList, function (aWorkItem) {
                    return aWorkItem.tradeWorkType == item.enumValue;
                });
            })



        }, function (error) { Notification.error({ message: error, delay: 2000 }); });

        propertySectionDataService.getSectionById($stateParams.sectionId).then(function (result) {
            $scope.sectionName = result.name;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });

        propertyDataService.getPropertyById($stateParams.propertyId).then(function (result) {
            $scope.propertyName = result.name;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });

    }


    init();

}]);



angular.module('sbAdminApp').controller('deleteWorkItemModalController', function ($scope, close) {

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
});