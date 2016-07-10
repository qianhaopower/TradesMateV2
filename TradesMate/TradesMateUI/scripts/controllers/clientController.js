'use strict';

angular.module('sbAdminApp')
  .controller('clientController', ['$scope', 'clientDataService','Notification',
      function ($scope, clientDataService, Notification) {


  

      var init = function () {
          clientDataService.getAllClients().then(function (result) {
              $scope.clientlist = result;
          }, function (error) { Notification.error({ message: error, delay: 1000 }); });
      }

      init();

  }]);
