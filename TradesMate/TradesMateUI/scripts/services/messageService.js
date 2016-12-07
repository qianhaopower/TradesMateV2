'use strict';
app.factory('messageService', ['$http', '$q', 'ngAuthSettings', function ($http, $q, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var messageServiceFactory = {};

  

    

    var _getMessages = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/messages/GetMessages').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _getMessageById = function (messageId) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/messages/GetMessage?messageId=' + messageId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };


    var _getPendingMessagesCount = function () {

        var deferred = $q.defer();
        $http.get(serviceBase + 'api/messages/GetUnReadMessagesCount').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };
    

    var _markMessageAsRead = function (messageId) {

        var deferred = $q.defer();
        $http.post(serviceBase + 'api/messages/MarkMessageAsRead?messageId=' + messageId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };


    var _generateClientWorkRequest = function (data) {

        var deferred = $q.defer();
        $http.post(serviceBase + 'api/messages/GenerateClientWorkRequest', data).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };



    var _createPropertyForWorkRequest = function (messageId, propertyModel) {

        //var data = { property: propertyModel, messageId: messageId };
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/messages/CreatePropertyForWorkRequest?messageId=' + messageId, propertyModel).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };


    //var _markResponseAsRead = function (responseId) {

    //    var deferred = $q.defer();
    //    $http.post(serviceBase + 'api/messages/MarkMessageAsRead?responseId=' + responseId).success(function (response) {
    //        deferred.resolve(response);
    //    }).error(function (err, status) {
    //        deferred.reject(err);
    //    });
    //    return deferred.promise;

    //};

    

    var _handleMessageResponse = function (messageId, action) {

        var deferred = $q.defer();
        $http.post(serviceBase + 'api/messages/HandleMessageResponse?messageId=' + messageId +'&action=' + action).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;
    };

    var _getMessageTitleForType = function (type) {
       //AssignDefaultRole = 0,
       //AssignDefaultRoleRequest = 1,
       //AssignContractorRole = 2,
       //InviteJoinCompanyRequest = 3,
       //WorkRequest = 4,
        //AddPropertyCoOwner = 5,
        var title = 'You have a new message';
        switch(type) {
            case 0:
                title = 'You are assigned default role';
                break;
            case 1:
                title = 'New role request';
                break;
            case 2:
                title = 'You are assigned contractor role';
                break;
            case 3:
                title = 'You are invited to join a company';
                break;
            case 4:
                title = 'New work request';
                break;
            case 5:
                title = 'New property request';
                break;
            default:
            
        }
        return title;

    };


    var _isMessageWaitingForRespond = function (message) {
        //AssignDefaultRole = 0,
        //AssignDefaultRoleRequest = 1,
        //AssignContractorRole = 2,
        //InviteJoinCompanyRequest = 3,
        //WorkRequest = 4,
        //AddPropertyCoOwner = 5,
    
        var isWait = false;
        switch (message.messageType) {
            //no response should even happen for theses three
            case 0:
            case 2:
            case 5:
                break;

            case 1:
            case 3:
            case 4:
                isWait =  message.isWaitingForResponse;
            default:
        }
        return isWait;

    };


    messageServiceFactory.getMessages = _getMessages;
    messageServiceFactory.getPendingMessagesCount = _getPendingMessagesCount;
    messageServiceFactory.markMessageAsRead = _markMessageAsRead;
    messageServiceFactory.getMessageTitleForType = _getMessageTitleForType;
    messageServiceFactory.getMessageById = _getMessageById;
    messageServiceFactory.handleMessageResponse = _handleMessageResponse;
    messageServiceFactory.generateClientWorkRequest = _generateClientWorkRequest;
    messageServiceFactory.createPropertyForWorkRequest = _createPropertyForWorkRequest;
    
    
    //messageServiceFactory.markResponseAsRead = _markResponseAsRead;
    
    
  
 
    

    return messageServiceFactory;
}]);