'use strict';

angular.module('sbAdminApp')
  .controller('companyMemberDetailController', ['$scope', 'companyService','authService',
      'Notification', '$state', '$stateParams',
function ($scope, companyService, authService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.memberId = undefined;
    $scope.member = undefined;
    $scope.readOnly = $state.current.name == 'base.viewCompanyMember';

    
    $scope.goBack = function () {
        $state.go('base.companyMember');
    }
    $scope.save = function () {

        $scope.member.confirmPassword = $scope.member.password;
        if ($scope.member.isNew) {
           
          //  var clientCopy= angular.copy($scope.client);
            companyService.createCompanyUser($scope.member).then(function (result) {
                Notification.success({ message: 'Member Created', delay: 2000 });
                $scope.goBack();
            }, function (error) {
                Notification.error({ message: error.message, delay: 2000 });
            });
        } else {
            companyService.updateCompanyMember($scope.member).then(function (result) {
                Notification.success({ message: 'Member saved', delay: 2000 });
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
        $scope.memberId = $stateParams.memberId;
        if ($scope.memberId &&
            ($state.current.name == 'base.editCompanyMember' || $state.current.name == 'base.viewCompanyMember')) {
            companyService.getMemberById($scope.memberId).then(function (result) {
                $scope.member = result;
            }, function (error) {
                Notification.error({ message: error, delay: 2000 });
            });
        } else if ($state.current.name == 'base.createCompanyMember') {
            //create new member
            $scope.member = {
                firstName: undefined,
                lastName: undefined,
                password:undefined,
                email: undefined,
                isNew: true,
                userType: 1,//trade

            }
        }
     
    }


    init();

}]);
