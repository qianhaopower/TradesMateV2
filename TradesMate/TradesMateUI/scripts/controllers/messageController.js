'use strict';

angular.module('sbAdminApp')
  .controller('messageController', ['$scope', 'messageService', 'Notification', '$state',
function ($scope, messageService, Notification, $state) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.messageList = [];

    $scope.search = function (item) {
        //if (!$scope.filterTextModel.searchText
        //   // || (item.firstName && (item.firstName.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            
        //    ) {
        //    return true;
        //}
        // return false;
        return true;
        
    };

    $scope.openMessageDetail = function (message) {
            $state.go('base.messageDetail', { messageId: message.id });
    };

    $scope.getMessageTitleForType = function (message) {
        if (message.title  ) {
            return message.title;
        }
        return messageService.getMessageTitleForType(message.messageType);
    };
    


   

    var init = function () {
        messageService.getMessages().then(function (result) {
            $scope.messageList = result;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);



