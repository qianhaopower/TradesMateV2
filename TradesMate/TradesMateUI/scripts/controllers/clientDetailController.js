'use strict';

angular.module('sbAdminApp')
  .controller('clientDetailController', ['$scope', 'clientDataService',
      'Notification', '$state', '$stateParams',
function ($scope, clientDataService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.clientId = undefined;
    $scope.client = undefined;
    $scope.readOnly = $state.current.name == 'dashboard.viewClient';

    

    $scope.save = function () {
        $scope.client.address = undefined;

        if ($scope.client.isNew) {
            var clientCopy= angular.copy($scope.client);
            clientDataService.createClient(clientCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 1000 });
                $scope.goToClientIndex();
            }, function (error) { Notification.error({ message: error, delay: 1000 }); });
        } else {
            clientDataService.editClient($scope.client).then(function (result) {
                Notification.success({ message: 'Saved', delay: 1000 });
                $scope.goToClientIndex();
            }, function (error) { Notification.error({ message: error, delay: 1000 }); });
        }
    
    }
    $scope.goToClientIndex = function () {

        $state.go('dashboard.clients');
    };


    var init = function () {
        $scope.clientId = $stateParams.param;
        if ($scope.clientId && $scope.clientId != '0') {
            clientDataService.getClientById($scope.clientId).then(function (result) {
                $scope.client = result;
            }, function (error) { Notification.error({ message: error, delay: 1000 }); });
        } else if ($scope.clientId == '0') {
            //create new client
            $scope.client = {
                firstName: undefined,
                surName: undefined,
                mobileNumber: undefined,
                email: undefined,
                isNew: true,
            }
        }
        //clientDataService.getAllClients().then(function (result) {
        //    $scope.clientlist = result;
        //}, function (error) { Notification.error({ message: error, delay: 1000 }); });
    }


    init();

}]);
