'use strict';

angular.module('sbAdminApp')
  .controller('clientController', ['$scope', 'clientDataService', 'Notification', '$state',
function ($scope, clientDataService, Notification, $state) {

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

    $scope.openClientDetail = function (client) {
        $state.go('dashboard.createClient', { myParam: client });
    };


    var init = function () {
        clientDataService.getAllClients().then(function (result) {
            $scope.clientlist = result;
        }, function (error) { Notification.error({ message: error, delay: 1000 }); });
    }


    init();

}]);
