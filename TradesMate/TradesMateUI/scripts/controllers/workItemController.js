'use strict';

angular.module('sbAdminApp')
  .controller('workItemController', ['$scope', 'workItemDataService', 'Notification',
      '$state', 'ModalService', '$stateParams', 'propertySectionDataService', 'propertyDataService',
function ($scope, workItemDataService, Notification, $state, ModalService, $stateParams, propertySectionDataService, propertyDataService) {




    $scope.filterTextModel = {
        searchText: undefined,
    };

    $scope.search = function (item) {
        if (!$scope.filterTextModel.searchText
            || (item.name && (item.name.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            || (item.description && (item.description.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            ) {
            return true;
        }
        return false;
    };

  

    $scope.openWorkItemDetail = function (workItem, readonly) {
        if (readonly)
            $state.go('dashboard.viewWorkItem', {
                workItemId: workItem.id,
                propertyId: $stateParams.propertyId,
                sectionId: $stateParams.sectionId,
            });
        else
            $state.go('dashboard.editWorkItem', {
                workItemId: workItem.id,
                propertyId: $stateParams.propertyId,
                sectionId: $stateParams.sectionId,
            });

    };
    $scope.addNewWorkItem = function () {
        $state.go('dashboard.createWorkItem', {
          
            propertyId: $stateParams.propertyId,
            sectionId: $stateParams.sectionId,
        });
    }

    $scope.goBack = function () {
        $state.go('dashboard.propertySections', {
          
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
        $state.go('dashboard.workItems', {
            workItemId: workItem.id,
            propertyId: $stateParams.propertyId,
            sectionId: $stateParams.sectionId,
        });

    }


    var init = function () {
        workItemDataService.getSectionAllWorkItems($stateParams.sectionId).then(function (result) {
            $scope.workItemList = result;
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