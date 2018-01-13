'use strict';
app.controller('resetPasswordController', ['$scope', '$location', '$timeout', '$state', 'authService', 'constants',
    function ($scope, $location, $timeout, $state, authService, constants, ) {

        $scope.savedSuccessfully = true;
        $scope.message = '';
        $scope.resetEmail = undefined;
        $scope.goback = function () {
            $state.go('login');
        }

        $scope.submit = function () {

            if ($scope.resetPasswordForm.$invalid) return;
            authService.sendResetPasswordRequest($scope.resetEmail).then(function (response) {
                $scope.message = "An email has been sent. Please follow the instructions to reset your password.";
                $scope.savedSuccessfully = true;
            },
                function (response) {
                    $scope.message = "Failed to reset password. " + response.data.exceptionMessage;
                    $scope.savedSuccessfully = false;
                });
        };

    }]);