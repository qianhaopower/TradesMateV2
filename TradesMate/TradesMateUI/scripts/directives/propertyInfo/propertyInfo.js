'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */

angular.module('sbAdminApp')
  .directive('propertyInfo', ['propertyDataService', 'Notification', function (propertyDataService, Notification) {
      return {
          templateUrl: 'scripts/directives/propertyInfo/propertyInfo.html',
          restrict: 'E',
          replace: true,
          scope: {
              propertyId: '@',// read the property id from outside
          },
          link: function (scope, element, attrs) {
              attrs.$observe('propertyId', function (value) {
                  scope.propertyId = value;
                  if (scope.propertyId) {
                      propertyDataService.getPropertyById(scope.propertyId).then(function (result) {
                          scope.property = result;
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
