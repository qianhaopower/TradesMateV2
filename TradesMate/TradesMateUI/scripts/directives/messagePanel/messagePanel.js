'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */

angular.module('sbAdminApp')
  .directive('messagePanel', [ 'Notification', function ( Notification) {
      return {
          templateUrl: 'scripts/directives/messagePanel/messagePanel.html',
          restrict: 'E',
          replace: true,
          //scope: {
          //    propertyId: '@',// read the property id from outside
            
          //},
          link: function (scope, element, attrs) {
              attrs.$observe('propertyId', function (value) {
                  scope.propertyId = value;
                  if (scope.propertyId) {
                      propertyDataService.getPropertyById(scope.propertyId).then(function (result) {
                          scope.property = result;
                      }, function (error) { Notification.error({ message: error, delay: 2000 }); });
                  }
              });
              scope.checked = false;
              scope.toggle = function () {
                  scope.checked = !scope.checked;
              }

          },
         
      }
  }]);
