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

            $scope.savedSuccessfully = true;
            $scope.message = "User profile updated successfully";
           
        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
            // $scope.message = "Failed to update user due to:" + errors.join(' ');
         });
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