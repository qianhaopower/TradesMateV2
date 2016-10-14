'use strict';

angular.module('sbAdminApp')
  .controller('clientController', ['$scope', 'clientDataService', 'Notification', '$state', 'ModalService',
function ($scope, clientDataService, Notification, $state, ModalService) {

    $scope.filterTextModel = {
        searchText: undefined,
    };

    $scope.search = function (item) {
        if (!$scope.filterTextModel.searchText
            || (item.firstName && (item.firstName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            || (item.lastName && (item.lastName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            ) {
            return true;
        }
        return false;
    };

    $scope.openClientDetail = function (client, readonly) {
        if (readonly)
            $state.go('base.viewClient', { param: client.id });
        else
            $state.go('base.editClient', { param: client.id });

    };
    $scope.addNewClient = function () {
        $state.go('base.createClient');
    }
    //$scope.addNewClient = function () {
    //    $state.go('login');
    //}


    $scope.deleteClient = function (client) {
        var clientRef = client;
        ModalService.showModal({
            templateUrl: 'deleteClientmodal.html',
            controller: "deleteClientModalController"
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result) {
                    $scope.proceedDelete(clientRef.id);
                }
            });
        });
    }

    

    $scope.proceedDelete = function (clientId) {
        clientDataService.deleteClient(clientId).then(function (result) {
            Notification.success({ message: 'Deleted', delay: 2000 });
            init();
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    $scope.viewClientProperties = function (client) {
        $state.go('base.clientProperties', { param: client.id });

    }


    var init = function () {
        clientDataService.getAllClients().then(function (result) {
            $scope.clientlist = result;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);



angular.module('sbAdminApp').controller('deleteClientModalController', function ($scope, close) {

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
});