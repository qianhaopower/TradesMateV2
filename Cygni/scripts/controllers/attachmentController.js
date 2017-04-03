'use strict';

angular.module('sbAdminApp')
    .controller('attachmentController', ['$scope', 'propertyDataService','workItemDataService', 'Notification', '$state', '$stateParams', 'uploadImageService',
        function ($scope, propertyDataService, workItemDataService, Notification, $state, $stateParams, uploadImageService) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.attachmentType = undefined;
  
    $scope.search = function (item) {
        if (!$scope.filterTextModel.searchText
            || (item.name && (item.name.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            ) { // find the attachment according to the name
            return true;
        }
        return false;
    };

    $scope.openUrl = function (attachment) {
        if (attachment.id)
            uploadImageService.downloadFile($scope.propertyId, attachmentType, attachment.id);
    };
    $scope.addNewAttachment = function () {
        
    }

    $scope.goBack = function () {
        if ($stateParams.propertyId) {
            $state.go('base.properties');

        } else if ($stateParams.workItemId) {
            $state.go('base.workItems');
        }
    }

    var init = function () {

        var attachmentType = undefined;
        if ($stateParams.propertyId) {
            $scope.attachmentType = "Property";
            $scope.propertyId = $stateParams.propertyId;
            propertyDataService.getPropertyById($scope.propertyId).then(function (result) {
                $scope.property = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });

        } else if ($stateParams.workItemId) {
            $scope.attachmentType = "WorkItem";
            $scope.workItemId = $stateParams.workItemId;
            workItemDataService.getWorkItemById($scope.workItemId).then(function (result) {
                $scope.workItem = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }

        $scope.entityId = $stateParams.propertyId || $stateParams.workItemId;

    }


    init();

}]);
