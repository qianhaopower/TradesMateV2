'use strict';

angular.module('sbAdminApp')
  .controller('messageDetailController', ['$scope', 'messageService', 'Notification', '$state', '$stateParams', '$sce', 'ModalService',
function ($scope, messageService, Notification, $state, $stateParams, $sce, modalService) {

   
    $scope.message = {};
    $scope.goBack = function () {
        $state.go('base.message');
    };

    

    $scope.messageTitle = undefined;
    $scope.showRespond = false;

    //Accept = 0,
    //Reject = 1,
    $scope.accept = function () {
        handle(0);
    }

    $scope.reject = function () {
        handle(1);
    }
    $scope.status = {
        isRespondOpen: true,
        isMessageOpen: true,

    };
    $scope.renderHtml = function (html_code) {
        return $sce.trustAsHtml(html_code);
    };

    $scope.createProperty = function () {
        modalService.showModal({
            templateUrl: 'createPropertyFromRequest.html',
            controller: "createPropertyFromRequestController",
            inputs: {
                addressFormatted: $scope.message.propertyAddress,
                
            }
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result) {
                    //property should be created, reload this message detail
                    init();
                }
            });
        });
    }


    var handle = function (action) {
        messageService.handleMessageResponse($scope.message.id, action).then(function (result) {
            Notification.success({ message: action ? 'Message rejected' : 'Message accepted', delay: 2000 });
            init();
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    var init = function () {
        messageService.getMessageById($stateParams.messageId).then(function (result) {

            $scope.message = result;
            $scope.showRespond = result.messageResponse;
            $scope.respondText = result.messageResponse
                ? result.memberName + ' has ' + result.messageResponse.responseAction.toLocaleLowerCase() + ' this request.'
                : undefined;
            $scope.messageTitle = messageService.getMessageTitleForType($scope.message.messageType);
            //if(!result.isRead)//mark as read

            //server will handle should we proceed this.
            messageService.markMessageAsRead($scope.message.id);

            //if (result.messageResponse && !result.messageResponse.isRead)//mark response as read, we always mark it, server handles if this is the right person reading it and decide if we mark it.
            //    messageService.markResponseAsRead($scope.message.id);//message and respond share the same key.


        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);





angular.module('sbAdminApp').controller('createPropertyFromRequestController', function ($scope, addressFormatted, close) {
    $scope.addressFormatted = addressFormatted;
    $scope.cancel = function () {
        close(false, 500); // close, but give 500ms for bootstrap to animate
    };
    $scope.save = function () {
        //fire the save
        close(true, 500);
    }
});