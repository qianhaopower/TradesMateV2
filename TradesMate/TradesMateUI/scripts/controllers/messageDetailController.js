'use strict';

angular.module('sbAdminApp')
  .controller('messageDetailController', ['$scope',  '$stateParams',
function ($scope,$stateParams) {

   
   
    $scope.goBack = function () {
        $state.go('base.message');
    };

    $scope.messageId = $stateParams.messageId;
    

}]);





