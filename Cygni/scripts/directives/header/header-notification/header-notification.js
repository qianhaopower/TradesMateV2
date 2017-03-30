'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */
//angular.module('sbAdminApp')
//	.directive('headerNotification',function(){
//	    return {
//	        templateUrl: 'scripts/directives/header/header-notification/header-notification.html',
//	        restrict: 'E',
//	        replace: true,
//	        link: function ($scope, element, attrs) {
//	            $scope.logOut = function () {
//	                authService.logOut();
//	                $location.path('/home');
//	            }

//	            $scope.authentication = authService.authentication;
//	        }
//	    }
//	});

angular.module('sbAdminApp')
app.directive('headerNotification', ['$state', 'authService', function ($state, authService) {
    return {
        templateUrl: 'scripts/directives/header/header-notification/header-notification.html',
        restrict: 'E',
        replace: true,
        link: function ($scope, element, attrs) {
            $scope.logOut = function () {
                authService.logOut();
                $state.go('login');
            }

            $scope.authentication = authService.authentication;
        }
    }
}]);