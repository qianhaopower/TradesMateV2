'use strict';
app.controller('signupController', ['$scope', '$location', '$timeout', '$state', 'authService', 'constants', 'companyService',
function ($scope, $location, $timeout, $state, authService,constants, companyService) {

    $scope.savedSuccessfully = false;
    $scope.message = "";

   

    $scope.serviceTypes = companyService.getDefaultServices();

    //angular.forEach($scope.outputBrowsers, function (value, key) {
    //    /* do your stuff here */
    //});

    $scope.registration = {
        userName: "",
        password: "",
        confirmPassword: "",
        firstName:undefined,
        lastName:undefined,
        email:undefined,
        //to do: put all enum into global constant.
        userType: constants.userType.client , //by defual 0 is client,
        companyName: undefined,
        isTrade: false,
    };
    $scope.goback = function () {
        $state.go('login');
    }

    $scope.signUp = function () {

        if ($scope.registerForm.$invalid) return;

        $scope.registration.userType = $scope.registration.isTrade ? constants.userType.trade : constants.userType.client;
        if ($scope.registration.isTrade) {
            $scope.registration.tradeTypes = _.pluck($scope.outputServiceTypes, 'enumValue');
        }

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
            $scope.message = err.data.error_description;
        });



        }, 2000);
    }

}]);