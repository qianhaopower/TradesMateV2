'use strict';


angular.module('sbAdminApp').controller('userProfileController', ['$scope', '$location', '$timeout', 'Notification', 'authService',
    function ($scope, $location, $timeout, Notification, authService) {

    $scope.userInfo = {
        userName: undefined,
        emailAddress: undefined,
        firstName: undefined,
        lastName: undefined,
    };
   
    $scope.updateUser = function () {

        if ($scope.userInfoForm.$invalid) return;

        authService.updateUser($scope.userInfo).then(function (response) {

            Notification.success({ message: "Saved", delay: 2000 });
           
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    var getUserDetail = function () {
        authService.getCurrentUser().then(function (currentUser) {
            $scope.userInfo.firstName = currentUser.firstName;
            $scope.userInfo.emailAddress = currentUser.emailAddress;
            $scope.userInfo.userName = currentUser.userName;
            $scope.userInfo.lastName = currentUser.lastName;
        },function (error) { Notification.error({ message: error, delay: 2000 }); });
    }
    getUserDetail();
    }

]);