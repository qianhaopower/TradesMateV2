'use strict';
var app = angular.module('sbAdminApp');
app.factory('workItemDataService', ['$q', '$http', '$window', 'urls', function ($q, $http, $window, urls) {

    
  
    return {

        //#region work item
       
        getSectionAllWorkItems: function (sectionId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Sections(' + sectionId + ')/workItemList';//entity property should be lower case, as it is formatted already
            var error = 'Error happened when getting section\'s workItems with id ' + sectionId;
            $http({
                method: 'GET',
                url: path,
            }).then(function successCallback(response) {
                if (response.data && response.status >= 200 && response.status <= 299) {
                    deferred.resolve(response.data.value);
                } else {
                    deferred.reject(error);
                }

            }, function errorCallback(response) {
                deferred.reject(error);
            });
            return deferred.promise;
        },

        getWorkItemById: function (workItemId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/WorkItems(' + workItemId + ')';
            var error = 'Error happened when getting workItem with id ' + workItemId;
            $http({
                method: 'GET',
                url: path,
            }).then(function successCallback(response) {
                if (response.data && response.status >= 200 && response.status <= 299) {
                    deferred.resolve(response.data);
                } else {
                    deferred.reject(error);
                }

            }, function errorCallback(response) {
                deferred.reject(error);
            });
            return deferred.promise;
        },
        editWorkItem: function (workItem) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/WorkItems(' + workItem.id + ')';
            var error = 'Error happened when saving workItem with id ' + workItem.id;
            $http({
                method: 'PATCH',
                url: path,
                data: workItem,
            }).then(function successCallback(response) {
                if ( response.status >= 200 && response.status <= 299) {
                    deferred.resolve(response.data);
                } else {
                    deferred.reject(error);
                }

            }, function errorCallback(response) {
                deferred.reject(error);
            });
            return deferred.promise;
        },
        
        createWorkItem: function (newWorkItem) {

            //find out a way allow passing extra to server. 
            //Or find a pattern to remove all extra property
            newWorkItem.isNew = undefined;

            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/WorkItems';
            var error = 'Error happened when creating workItem';
            $http({
                method: 'POST',
                url: path,
                data: newWorkItem,
            }).then(function successCallback(response) {
                if (response.status >= 200 && response.status <= 299) {
                    deferred.resolve(response.data);
                } else {
                    deferred.reject(error);
                }

            }, function errorCallback(response) {
                deferred.reject(error);
            });
            return deferred.promise;
        },

        deleteWorkItem: function (workItemId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/WorkItems(' + workItemId + ')';
            var error = 'Error happened when deleting workItem';
            $http({
                method: 'DELETE',
                url: path,
               
            }).then(function successCallback(response) {
                if (response.status >= 200 && response.status <= 299) {
                    deferred.resolve();
                } else {
                    deferred.reject(error);
                }

            }, function errorCallback(response) {
                deferred.reject(error);
            });
            return deferred.promise;
        },
        //#endregion

        getAllWorkItemTemplates: function () {//need to filter by type later
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/WorkItemTemplates';//entity property should be lower case, as it is formatted already
            var error = 'Error happened when getting work item templates';
            $http({
                method: 'GET',
                url: path,
            }).then(function successCallback(response) {
                if (response.data && response.status >= 200 && response.status <= 299) {
                    deferred.resolve(response.data.value);
                } else {
                    deferred.reject(error);
                }

            }, function errorCallback(response) {
                deferred.reject(error);
            });
            return deferred.promise;
        },
    };
}]);


