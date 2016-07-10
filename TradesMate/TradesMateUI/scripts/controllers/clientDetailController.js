'use strict';

angular.module('sbAdminApp')
  .controller('clientDetailController', ['$scope', 'clientDataService',
      'Notification', '$state', '$stateParams',
function ($scope, clientDataService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };

    //$scope.search = function (item) {
    //    if (!$scope.filterTextModel.searchText
    //        || (item.firstName && (item.firstName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
    //        || (item.surName && (item.surName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
    //        ) {
    //        return true;
    //    }
    //    return false;
    //};


    $scope.goToClientIndex = function () {

        $state.go('dashboard.clients');
    };


    var init = function () {
        $scope.client = $stateParams.myParam;
        //clientDataService.getAllClients().then(function (result) {
        //    $scope.clientlist = result;
        //}, function (error) { Notification.error({ message: error, delay: 1000 }); });
    }


    init();

}]);
