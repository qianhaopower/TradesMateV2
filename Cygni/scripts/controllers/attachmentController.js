'use strict';

angular.module('sbAdminApp')
    .controller('attachmentController', ['$scope', 'propertyDataService', 'workItemDataService', 'Notification', '$state', '$stateParams', 'storageService',
        function ($scope, propertyDataService, workItemDataService, Notification, $state, $stateParams, storageService) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.attachmentType = undefined;
    $scope.attachmentList = [];
  
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
            storageService.downloadFile($scope.entityId, $scope.attachmentType, attachment.id);

    };
    $scope.deleteFile = function (attachment) {
        if (attachment.id)
            storageService.deleteFile($scope.entityId, $scope.attachmentType, attachment.id)
                .then(function () {
                    Notification.success({ message: "Deleted", delay: 2000 });
                    storageService.downloadAttachmentForEntity($scope.entityId, $scope.attachmentType).then(function (result) {
                        $scope.attachmentList = result;
                    }, function (error) { Notification.error({ message: error, delay: 2000 }); });


                }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };
   

    $scope.uploadedFile = function (element) {
        $scope.$apply(function ($scope) {
            $scope.files = element.files;

            storageService.uploadFile($scope.files[0], $scope.entityId, $scope.attachmentType)
                .then(function () {
                    Notification.success({ message: "Upload successful", delay: 2000 });

                    storageService.downloadAttachmentForEntity($scope.entityId, $scope.attachmentType).then(function (result) {
                        $scope.attachmentList = result;
                    }, function (error) { Notification.error({ message: error, delay: 2000 }); });


            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        });
    }


    $scope.goBack = function () {
        if ($stateParams.propertyId) {
            $state.go('base.properties');

        } else if ($stateParams.workItemId) {
            $state.go('base.workItems', {
                sectionId: $stateParams.sectionId,
                propertyId: $stateParams.propertyIdForWorkItem
            });
        }
    }
    $scope.trunc = Math.trunc;

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
        storageService.downloadAttachmentForEntity($scope.entityId, $scope.attachmentType).then(function (result) {
            $scope.attachmentList = result;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
      

    }


    init();

}]);
