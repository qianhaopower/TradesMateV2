'use strict';

/**
 * @ngdoc directive
 * @name izzyposWebApp.directive:adminPosHeader
 * @description
 * # adminPosHeader
 */

angular.module('sbAdminApp')
  .directive('messageDetailDirective', ['messageService', 'Notification', '$sce', 'ModalService', function (messageService, Notification, $sce,modalService) {
      return {
          templateUrl: 'scripts/directives/messageDetail/messageDetailDirective.html',
          restrict: 'E',
          replace: true,
          scope: {
              messageId: '@',// read the property id from outside
              
          },
          link: function (scope, element, attrs) {
              attrs.$observe('messageId', function (value) {
                  scope.messageId = value;
                  if (scope.messageId) {
                      init();
                  }
              });


              scope.message = {};
              scope.messageTitle = undefined;
              scope.showRespond = false;

              //Accept = 0,
              //Reject = 1,
              scope.accept = function () {
                  handle(0);
              }

              scope.reject = function () {
                  handle(1);
              }
              scope.status = {
                  isRespondOpen: true,
                  isMessageOpen: true,

              };
              scope.renderHtml = function (html_code) {
                  return $sce.trustAsHtml(html_code);
              };

              var handle = function (action) {
                  messageService.handleMessageResponse(scope.message.id, action).then(function (result) {
                      Notification.success({ message: action ? 'Message rejected' : 'Message accepted', delay: 2000 });
                      init();
                  }, function (error) { Notification.error({ message: error, delay: 2000 }); });
              }

              var init = function () {
                  messageService.getMessageById(scope.messageId).then(function (result) {

                      scope.message = result;
                      scope.showRespond = result.messageResponse;
                      scope.respondText = result.messageResponse
                          ? result.name + ' has ' + result.messageResponse.responseAction.toLocaleLowerCase() + ' this request.'
                          : undefined;
                      scope.messageTitle = messageService.getMessageTitleForType(scope.message.messageType);
                      //if(!result.isRead)//mark as read

                      //server will handle should we proceed this.
                      messageService.markMessageAsRead(scope.message.id);

                      //if (result.messageResponse && !result.messageResponse.isRead)//mark response as read, we always mark it, server handles if this is the right person reading it and decide if we mark it.
                      //    messageService.markResponseAsRead(scope.message.id);//message and respond share the same key.


                  }, function (error) { Notification.error({ message: error, delay: 2000 }); });
              }

              scope.createProperty = function () {
                  modalService.showModal({
                      templateUrl: 'scripts/directives/messageDetail/createPropertyFromRequestModal.html',
                      controller: "createPropertyFromRequestController",
                      inputs: {
                          addressFormatted: scope.message.propertyAddress,

                      }
                  }).then(function (modal) {
                      modal.element.modal();
                      modal.close.then(function (property) {
                          if (property) {
                              //property should be created, reload this message detail

                              messageService.createPropertyForWorkRequest(scope.messageId, property).then(function (result) {
                                  init();
                              }, function (error) { Notification.error({ message: error, delay: 2000 }); });

                          }
                      });
                  });
              }             

          },
         
      }
  }]);



angular.module('sbAdminApp').controller('createPropertyFromRequestController', function ($scope, addressFormatted, close) {
    $scope.property = {
        name: undefined,
        description: undefined,
        address: {
            line1: undefined,
        },
    };
    $scope.addressFormatted = addressFormatted;
    $scope.cancel = function () {
        close(undefined, 500); // close, but give 500ms for bootstrap to animate
    };
    $scope.save = function () {
        //fire the save
        $scope.property.defaultSections = $scope.defaultSections;
        close($scope.property, 500);
    }
    $scope.defaultSections = {
        bedroomNumber: 0,
        livingRoomNumber: 0,
        bathroomNumber: 0,
        kitchenNumber: 0,
        laundryRoomNumber: 0,
        hallWayNumber: 0,
        deckNumber: 0,
        basementNumber: 0,
        gardenNumber: 0,
        garageNumber: 0,
    };
});