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

    

    $scope.save = function () {
        $scope.client.address = undefined;
        clientDataService.editClient($scope.client).then(function (result) {
            Notification.success({ message: 'Saved', delay: 1000 });
            $scope.goToClientIndex();
        }, function (error) { Notification.error({ message: error, delay: 1000 }); });
    }
    $scope.goToClientIndex = function () {

        $state.go('dashboard.clients');
    };


    var init = function () {
        $scope.clientId = $stateParams.param;
        if ($scope.clientId) {
            clientDataService.getClientById($scope.clientId).then(function (result) {
                $scope.client = result;
            }, function (error) { Notification.error({ message: error, delay: 1000 }); });
        }
        //clientDataService.getAllClients().then(function (result) {
        //    $scope.clientlist = result;
        //}, function (error) { Notification.error({ message: error, delay: 1000 }); });
    }


    init();

}]);
