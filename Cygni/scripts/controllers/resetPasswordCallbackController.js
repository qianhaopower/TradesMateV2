'use strict';
app.controller('resetPasswordCallbackController', ['$scope', '$location', '$timeout', '$state', 'authService', 'constants','$stateParams',
    function ($scope, $location, $timeout, $state, authService, constants,$stateParams ) {

        $scope.message = '';
        $scope.savedSuccessfully = true;
        var userId = $location.search().userId;
        var code = $location.search().code;
        $scope.resetpassword =undefined;
        $scope.resetpasswordRepeat = undefined;
        $scope.passwordNotSame = $scope.resetpassword != $scope.resetpasswordRepeat;
        $scope.$watch('resetpassword', function(newValue, oldValue) {
            $scope.passwordNotSame = $scope.resetpassword != $scope.resetpasswordRepeat;
          });
          $scope.$watch('resetpasswordRepeat', function(newValue, oldValue) {
            $scope.passwordNotSame = $scope.resetpassword != $scope.resetpasswordRepeat;
          });
         

        $scope.submit = function () {

            if ($scope.resetForm.$invalid) return;
            if(!userId || !code) return;
            if($scope.resetpassword != $scope.resetpasswordRepeat){
                $scope.passwordNotSame= true;
                return;
            }
            $scope.passwordNotSame= false;


            authService.sendResetPasswordCallBack(userId, code, $scope.resetpassword).then(function (response) {
                $scope.message = "Your password has been reset successfully.";
                $scope.savedSuccessfully = true;
            },
                function (response) {
                    $scope.message = "Failed to reset password. " +( (response.data.exceptionMessage )? response.data.exceptionMessage: '');
                    $scope.savedSuccessfully = false;
                });
        };

    }]);