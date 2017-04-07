'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */

angular.module('sbAdminApp')
  .directive('propertyDefaultSection', [function () {
      return {
          templateUrl: 'scripts/directives/propertyDefaultSecion/propertyDefaultSecion.html',
          restrict: 'E',
          replace: true,
          //scope: {


          //},//user shared scope here
          link: function (scope, element, attrs) {
              //nothing to link
          },

      }
  }]);
