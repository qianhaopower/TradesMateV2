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
            || (item.surName && (item.surName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            ) {
            return true;
        }
        return false;
    };

    $scope.openClientDetail = function (client, readonly) {
        if (readonly)
            $state.go('dashboard.viewClient', { param: client.id });
        else
            $state.go('dashboard.editClient', { param: client.id });

    };
    $scope.addNewClient = function () {
        $state.go('dashboard.createClient');
    }


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

    $scope.ViewClientProperties = function (client) {
        $state.go('dashboard.clientProperties', { param: client.id });

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