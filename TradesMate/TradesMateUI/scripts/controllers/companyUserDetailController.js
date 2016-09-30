'use strict';

angular.module('sbAdminApp')
  .controller('companyUserDetailController', ['$scope', 'companyService','authService',
      'Notification', '$state', '$stateParams',
function ($scope, companyService, authService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.userId = undefined;
    $scope.user = undefined;
    $scope.readOnly = $state.current.name == 'base.viewCompanyUser';

    
    $scope.goBack = function () {
        $state.go('base.companyUser');
    }
    $scope.save = function () {

        $scope.user.confirmPassword = $scope.user.password;
        if ($scope.user.isNew) {
           
          //  var clientCopy= angular.copy($scope.client);
            companyService.createCompanyUser($scope.user).then(function (result) {
                Notification.success({ message: 'User Created', delay: 2000 });
                $scope.goBack();
            }, function (error) {
                Notification.error({ message: error.message, delay: 2000 });
            });
        } else {
            companyService.updateCompanyUser($scope.user).then(function (result) {
                Notification.success({ message: 'User saved', delay: 2000 });
                $scope.goBack();
            }, function (error) {
                Notification.error({ message: error.message, delay: 2000 });
            });
        }
    
    }
    //$scope.goToClientIndex = function () {

    //    $state.go('base.clients');
    //};

    var init = function () {
        $scope.userId = $stateParams.userId;
        if ($scope.userId &&
            ($state.current.name == 'base.editCompanyUser' || $state.current.name == 'base.viewCompanyUser')) {
            companyService.getUserById($scope.userId).then(function (result) {
                $scope.user = result;
            }, function (error) {
                Notification.error({ message: error, delay: 2000 });
            });
        } else if ($state.current.name == 'base.createCompanyUser') {
            //create new user
            $scope.user = {
                firstName: undefined,
                lastName: undefined,
                password:undefined,
                email: undefined,
                isNew: true,
            }
        }
     
    }


    init();

}]);
