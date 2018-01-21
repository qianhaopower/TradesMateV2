'use strict';

angular.module('sbAdminApp')
  .controller('propertyController', ['$scope', 'clientDataService', 'propertyDataService', 'Notification', '$state', 'ModalService','$stateParams',
function ($scope, clientDataService, propertyDataService, Notification, $state, ModalService, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };

    $scope.clientName = undefined;
    $scope.client = undefined;
    $scope.clientId = $stateParams.param;

    $scope.search = function (item) {
        if (!$scope.filterTextModel.searchText
            || (item.name && (item.name.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            || (item.description && (item.description.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            || (item.addressDisplay && (item.addressDisplay.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            ) {
            return true;
        }
        return false;
    };

    $scope.openPropertyDetail = function (property, readonly) {
        if (readonly)
            $state.go('base.viewProperty', { propertyId: property.id });
        else
            $state.go('base.editProperty', { propertyId: property.id });

    };
    $scope.addNewProperty = function () {
        $state.go('base.createProperty', { clientId:$scope.clientId });
    }

    $scope.viewPropertySections = function (property) {
        $state.go('base.propertySections', { propertyId: property.id });
    }


    $scope.viewPropertyAttachments = function (property) {
        $state.go('base.propertyAttachments', { propertyId: property.id });
    }


    $scope.viewPropertyReports = function (property) {
        var propertyRef = property;
        ModalService.showModal({
            templateUrl: 'propertyReportsModal.html',
            controller: "propertyReportsModalController",
            bodyClass: 'report-modal',
            inputs: {
                propertyId: propertyRef.id,
            }
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result) {
                   //just close, nothing todo
                }
            });
        });
    }


    $scope.goBack = function () {
        $state.go('base.clients');
    }

    $scope.deleteProperty = function (property) {
        var propertyRef = property;
        ModalService.showModal({
            templateUrl: 'deletePropertymodal.html',
            controller: "deletePropertyModalController",
            
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result) {
                    $scope.proceedDelete(propertyRef.id);
                }
            });
        });
    }

    $scope.proceedDelete = function (propertyId) {
        propertyDataService.deleteProperty(propertyId).then(function (result) {
            Notification.success({ message: 'Deleted', delay: 2000 });
            init();
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };


    var init = function () {
        if ($scope.clientId) {
            //get client 

            clientDataService.getClientById($scope.clientId).then(function (result) {
                $scope.client = result;
                $scope.clientName = $scope.client.firstName + ' ' + $scope.client.lastName;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
            //get property for client
            clientDataService.getClientProperties($scope.clientId).then(function (result) {
                $scope.propertyList = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });

        } else {
            //get all properties
            propertyDataService.getAllProperties().then(function (result) {
                $scope.propertyList = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }
        
    }


    init();

}]);



angular.module('sbAdminApp').controller('deletePropertyModalController', function ($scope, close) {
   
    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
});

angular.module('sbAdminApp').controller('propertyReportsModalController', function ($scope, propertyDataService, Notification, propertyId, close) {


        $scope.propertyId = propertyId;

        $scope.close = function (result) {
            close(result, 500); // close, but give 500ms for bootstrap to animate
        };

        $scope.hasImage = function (workItem, sequence) {
            if (!workItem.imageUrls)
                return false;

            if (!sequence)
                return workItem.imageUrls[0];//has first one.

            if (sequence == 1) 
                return workItem.imageUrls[0];//has first one.
            if (sequence == 2)
                return workItem.imageUrls[1];//has second one.
            if (sequence == 3)
                return workItem.imageUrls[2];//has third one.
            if (sequence == 4)
                return workItem.imageUrls[3];//has fourth one.

        }

        var init = function () {
            if ($scope.propertyId) {
                //get client 

                propertyDataService.getPropertyReportItems($scope.propertyId).then(function (result) {

                    if (!result) return;
                    $scope.reportItems = result;

                    $scope.reportItemsFlatten = [];

                    _.each($scope.reportItems, function (group) {
                        _.each(group.workItems, function (aItem) {
                            aItem.sectionName = group.sectionName;
                            $scope.reportItemsFlatten.push(aItem);
                        });
                    });
                }, function (error) { Notification.error({ message: error, delay: 2000 }); });

               

            } 

        }


        init();

       


});