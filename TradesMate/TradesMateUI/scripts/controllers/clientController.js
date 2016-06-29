'use strict';

angular.module('sbAdminApp')
  .controller('clientController', ['$scope', 'clientDataService', function ($scope, clientDataService) {
      $scope.personnelList = clientDataService.getFakeClientList();

  }]);
