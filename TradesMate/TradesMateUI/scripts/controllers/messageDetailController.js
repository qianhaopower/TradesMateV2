'use strict';

angular.module('sbAdminApp')
  .controller('messageDetailController', ['$scope', 'messageService', 'Notification', '$state', '$stateParams',
function ($scope, messageService, Notification, $state, $stateParams) {

   
    $scope.message = {};
    $scope.goBack = function () {
        $state.go('base.message');
    };
    $scope.messageTitle = undefined;
    var init = function () {
        messageService.getMessageById($stateParams.messageId).then(function (result) {
            $scope.message = result;
            $scope.messageTitle = messageService.getMessageTitleForType($scope.message.messageType);
            if(!result.isRead)//mark as read
            messageService.markMessageAsRead($scope.message.id);
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);



