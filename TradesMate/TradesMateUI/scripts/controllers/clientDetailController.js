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
    $scope.readOnly = $state.current.name == 'base.viewClient';

    
    $scope.goBack = function () {
        $state.go('base.clients');
    }
    $scope.save = function () {
        $scope.client.address = undefined;

        if ($scope.client.isNew) {
            var clientCopy= angular.copy($scope.client);
            clientDataService.createClient(clientCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 2000 });
                $scope.goToClientIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else {
            clientDataService.editClient($scope.client).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToClientIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }
    
    }
    $scope.goToClientIndex = function () {

        $state.go('base.clients');
    };


    var init = function () {
        $scope.clientId = $stateParams.param;
        if ($scope.clientId &&
            ($state.current.name == 'base.editClient'
            || $state.current.name == 'base.viewClient')) {
            clientDataService.getClientById($scope.clientId).then(function (result) {
                $scope.client = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'base.createClient') {
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
        //}, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);
