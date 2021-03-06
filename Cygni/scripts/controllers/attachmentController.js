'use strict';

angular.module('sbAdminApp')
    .controller('attachmentController', ['$scope', 'propertyDataService', 'workItemDataService', 'workItemTemplateService','Notification', '$state', '$stateParams', 'storageService', 'Lightbox',
        function ($scope, propertyDataService, workItemDataService,workItemTemplateService, Notification, $state, $stateParams, storageService, Lightbox) {

            $scope.filterTextModel = {
                searchText: undefined,
            };
            $scope.attachmentType = undefined;
            $scope.attachmentList = [];

            $scope.search = function (item) {
                if (!$scope.filterTextModel.searchText ||
                    (item.name && (item.name.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
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
                        Notification.success({
                            message: "Deleted",
                            delay: 2000
                        });
                        storageService.downloadAttachmentForEntity($scope.entityId, $scope.attachmentType).then(function (result) {
                            markAttachmentsAsReadOnly(result)
                            $scope.attachmentList = result;
                            updateGallary($scope.attachmentList);
                        }, function (error) {
                            Notification.error({
                                message: error,
                                delay: 2000
                            });
                        });


                    }, function (error) {
                        Notification.error({
                            message: error,
                            delay: 2000
                        });
                    });
            };


            $scope.uploadedFile = function (element) {
                $scope.$apply(function ($scope) {
                    $scope.files = element.files;

                    storageService.uploadFile($scope.files[0], $scope.entityId, $scope.attachmentType)
                        .then(function () {
                            Notification.success({
                                message: "Upload successful",
                                delay: 2000
                            });

                            storageService.downloadAttachmentForEntity($scope.entityId, $scope.attachmentType).then(function (result) {
                                 markAttachmentsAsReadOnly(result)
                                $scope.attachmentList = result;
                                updateGallary($scope.attachmentList);
                            }, function (error) {
                                Notification.error({
                                    message: error,
                                    delay: 2000
                                });
                            });


                        }, function (error) {
                            Notification.error({
                                message: error,
                                delay: 2000
                            });
                        });
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
                else if ($stateParams.workItemTemplateId) {
                    $state.go('base.companyWorkItemTemplate');
                }
            }
            $scope.trunc = Math.trunc;




            var updateGallary = function (attachmentList) {
                $scope.images = [];
                var imageList = _.filter(attachmentList, function (item) {
                    return item.type == 0;
                }); //0 is image;

                _.each(imageList, function (item) {
                    $scope.images.push({
                        'url': item.url,
                        //'caption': 'Optional caption',
                        'thumbUrl': item.url // used only for this example
                    });
                });

            }


            $scope.openLightboxModal = function (index) {
                Lightbox.openModal($scope.images, index);
            };

            var init = function () {

                var attachmentType = undefined;
                if ($stateParams.propertyId) {
                    $scope.attachmentType = "Property";
                    $scope.propertyId = $stateParams.propertyId;
                    propertyDataService.getPropertyById($scope.propertyId).then(function (result) {
                        $scope.property = result;
                    }, function (error) {
                        Notification.error({
                            message: error,
                            delay: 2000
                        });
                    });

                } else if ($stateParams.workItemId) {
                    $scope.attachmentType = "WorkItem";
                    $scope.workItemId = $stateParams.workItemId;
                    workItemDataService.getWorkItemById($scope.workItemId).then(function (result) {
                        $scope.workItem = result;
                    }, function (error) {
                        Notification.error({
                            message: error,
                            delay: 2000
                        });
                    });
                } 
                else if ($stateParams.workItemTemplateId) {
                    $scope.attachmentType = 'WorkItemTemplate';
                    $scope.workItemTemplateId = $stateParams.workItemTemplateId;
                    workItemTemplateService.getTemplateById($scope.workItemTemplateId).then(function (result) {
                        $scope.workItemTemplate = result;
                    }, function (error) {
                        Notification.error({
                            message: error,
                            delay: 2000
                        });
                    });
                } 

                $scope.entityId = $stateParams.propertyId || $stateParams.workItemId || $stateParams.workItemTemplateId;
                storageService.downloadAttachmentForEntity($scope.entityId, $scope.attachmentType).then(function (result) {
                    markAttachmentsAsReadOnly(result)
                    $scope.attachmentList = result;
                    updateGallary($scope.attachmentList);
                }, function (error) {
                    Notification.error({
                        message: error,
                        delay: 2000
                    });
                });


            }
            
            var markAttachmentsAsReadOnly  = function(attachments){
                if($scope.attachmentType == 'WorkItem')
                _.each(attachments, function (item) {
                    //mark all workitem templates as not deletable
                   if(item.entityType == 3)//workItemTemplate
                   {
                       item.fromTemplate = true;
                   }

                });
            }


            init();

        }
    ]);