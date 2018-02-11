'use strict';
app.controller('associateController', ['$scope', '$location', '$timeout', 'authService', 'constants', function ($scope, $location, $timeout, authService, constants) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

    $scope.registerData = {
        userName: authService.externalAuthData.userName.replace(/\s/g, '').toLowerCase(),
        provider: authService.externalAuthData.provider,
        externalAccessToken: authService.externalAuthData.externalAccessToken
    };
    if(authService.externalAuthData.userName.indexOf(' ' > 0))
    {
        //set the user first name and last name
        $scope.registerData.firstName = authService.externalAuthData.userName.split(' ')[0];
        $scope.registerData.lastName = authService.externalAuthData.userName.split(' ')[1];
    }

    $scope.registerExternal = function () {
        $scope.registerData.userType = $scope.registerData.isTrade ? constants.userType.trade : constants.userType.client;

        authService.registerExternal($scope.registerData).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "User has been registered successfully, you will be redicted to home page in 2 seconds.";
            startTimer();

        },
          function (response) {
              var errors = [];
              for (var key in response.modelState) {
                  errors.push(response.modelState[key]);
              }
              $scope.message = "Failed to register user due to:" + errors.join(' ');
          });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            $location.path('/orders');
        }, 2000);
    }

}]);