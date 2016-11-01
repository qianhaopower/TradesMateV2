'use strict';

angular.module('sbAdminApp')
  .controller('messageDetailController', ['$scope', 'messageService', 'Notification', '$state', '$stateParams',
function ($scope, messageService, Notification, $state, $stateParams) {

   
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
            if(!result.isRead)//mark as read
            messageService.markMessageAsRead($scope.message.id);
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);



