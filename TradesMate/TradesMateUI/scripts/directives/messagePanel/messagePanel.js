'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */

angular.module('sbAdminApp')
  .directive('messagePanel', ['Notification', 'messageService', function (Notification, messageService) {
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
                      messageService.getWorkRequestMessageForProperty(scope.propertyId).then(function (result) {
                          scope.messageList = result;
                          //select the latest one
                          if (scope.messageList.length > 0) {
                              scope.selectedMessage = scope.messageList[0];
                          }


                      }, function (error) { Notification.error({ message: error, delay: 2000 }); });
                  }
                  
              });
              scope.messageList = [];
              scope.checked = false;
              scope.toggle = function () {
                  scope.checked = !scope.checked;
              }

          },
         
      }
  }]);
