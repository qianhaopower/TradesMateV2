'use strict';

angular.module('sbAdminApp')
  .controller('messageDetailController', ['$scope',  '$stateParams','$state',
function ($scope, $stateParams, $state) {

   
   
    $scope.goBack = function () {
        $state.go('base.message');
    };

    $scope.messageId = $stateParams.messageId;
    

}]);





