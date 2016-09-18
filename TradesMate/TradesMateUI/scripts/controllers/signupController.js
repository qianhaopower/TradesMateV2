'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', '$state', 'authService','constants',
function ($scope, $location, $timeout, $state, authService,constants) {

    $scope.savedSuccessfully = false;
    $scope.message = "";



    $scope.registration = {
        userName: "",
        password: "",
        confirmPassword: "",
        firstName:undefined,
        lastName:undefined,

        //to do: put all enum into global constant.
        userType: constants.userType.client , //by defual 0 is client,
        companyName: undefined,
        isTrade: false,
    };

    $scope.signUp = function () {

        if ($scope.registerForm.$invalid) return;

        $scope.registration.userType = $scope.registration.isTrade ? constants.userType.trade : constants.userType.client  ;

        authService.saveRegistration($scope.registration).then(function (response) {

            $scope.savedSuccessfully = true;
            $scope.message = "Sign up successfully";
            startTimer();

        },
         function (response) {
             var errors = [];
             for (var key in response.data.modelState) {
                 for (var i = 0; i < response.data.modelState[key].length; i++) {
                     errors.push(response.data.modelState[key][i]);
                 }
             }
             $scope.message = "Failed to register user due to:" + errors.join(' ');
         });
    };

    var startTimer = function () {
        var timer = $timeout(function () {
            $timeout.cancel(timer);
            // $location.path('/login');
            // then automatically log the user in.

            authService.login({
                userName: $scope.registration.userName,
                password: $scope.registration.password,
                useRefreshTokens: false,
            }).then(function (response) {

                //$location.path('/orders');
                $state.go('base.home');

            },
        function (err) {
            $scope.message = err.error_description;
        });



        }, 2000);
    }

}]);