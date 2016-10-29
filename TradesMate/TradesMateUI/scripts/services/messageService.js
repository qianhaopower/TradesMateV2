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

    var _getPendingMessagesCount = function () {

        var deferred = $q.defer();
        $http.get(serviceBase + 'api/messages/GetUnReadMessagesCount').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };
    

    var _markMessageAsRead = function (messageOrResponseId) {

        var deferred = $q.defer();
        $http.post(serviceBase + 'api/messages/MarkMessageAsRead?messageOrResponseId=' + messageOrResponseId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };
    
    messageServiceFactory.getMessages = _getMessages;
    messageServiceFactory.getPendingMessagesCount = _getPendingMessagesCount;
    messageServiceFactory.markMessageAsRead = _markMessageAsRead;
  
 
    

    return messageServiceFactory;
}]);