'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */

angular.module('sbAdminApp')
  .directive('clientReadOnlyInfo', ['clientDataService', 'Notification', function (clientDataService, Notification) {
      return {
          templateUrl: 'scripts/directives/clientReadOnlyInfo/clientReadOnlyInfo.html',
          restrict: 'E',
          replace: true,
          scope: {
              clientId: '@',// read the client id from outside
          },
          link: function (scope, element, attrs) {
              attrs.$observe('clientId', function (value) {
                  scope.clientId = value;
                  if (scope.clientId) {
                      clientDataService.getClientById(scope.clientId).then(function (result) {
                          scope.client = result;
                      }, function (error) { Notification.error({ message: error, delay: 2000 }); });
                  }
              });
             // scope.clientId = $attrs.clientId; //assign attribute to the scope

          },
          //controller: ['$scope', '$attrs', function ($scope, $attrs) {
          //    //  console.log($attrs.text); // just call to the $attrs instead $scope and i got the actual value
          //    $scope.clientId = $attrs.clientId; //assign attribute to the scope

          //    clientDataService.getClientById($scope.clientId).then(function (result) {
          //        $scope.client = result;
          //    }, function (error) { Notification.error({ message: error, delay: 2000 }); });
          //}]
      }
  }]);
