'use strict';


angular.module('sbAdminApp').controller('userProfileController', ['$scope', '$location', '$timeout', 'Notification', 'authService',
    function ($scope, $location, $timeout, Notification, authService) {

        $scope.editMode = false;

    $scope.userInfo = {
        userName: undefined,
        emailAddress: undefined,
        firstName: undefined,
        lastName: undefined,
    };
    $scope.userInfoClone = {};
   
    $scope.updateUser = function () {
        $scope.editMode = false;
        if ($scope.userInfoForm.$invalid) return;

        authService.updateUser($scope.userInfo).then(function (response) {

            Notification.success({ message: "Saved", delay: 2000 });
           
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    $scope.cancelEdit = function () {
        $scope.editMode = false;
        //change it back to original.
        $scope.userInfo = $scope.userInfoClone;
    }

    var getUserDetail = function () {
        authService.getCurrentUser().then(function (currentUser) {
            $scope.userInfo.firstName = currentUser.firstName;
            $scope.userInfo.emailAddress = currentUser.emailAddress;
            $scope.userInfo.userName = currentUser.userName;
            $scope.userInfo.lastName = currentUser.lastName;
            $scope.userInfoClone =  JSON.parse(JSON.stringify($scope.userInfo));
           
        },function (error) { Notification.error({ message: error, delay: 2000 }); });
    }
    getUserDetail();
    }

]);