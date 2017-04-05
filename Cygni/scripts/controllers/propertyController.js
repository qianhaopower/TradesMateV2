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
            || (item.address && (item.address.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
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

    $scope.goBack = function () {
        $state.go('base.clients');
    }

    $scope.deleteProperty = function (property) {
        var propertyRef = property;
        ModalService.showModal({
            templateUrl: 'deletePropertymodal.html',
            controller: "deletePropertyModalController"
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