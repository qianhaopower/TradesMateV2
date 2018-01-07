'use strict';
var app = angular.module('sbAdminApp');
app.factory('clientDataService', [ '$q', '$http', '$window', 'urls',function ( $q, $http, $window, urls) {
   
    return {
        //$scope.url = urls.apiUrl;
        getAllClients: function () {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/clients';//Odata is case sensitive
            var error = 'Error happened when getting clients';  
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
        getClientById: function (clientId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/clients/' + clientId;
            var error = 'Error happened when getting client with id ' + clientId;
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
        editClient: function (client) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/clients';
            var error = 'Error happened when saving client with id ' + client.id;
            $http({
                method: 'PUT',
                url: path,
                data:client,
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
        
        // createClient: function (newClient) {

        //     //find out a way allow passing extra to server. 
        //     //Or find a pattern to remove all extra property
        //     newClient.isNew = undefined;

        //     var deferred = $q.defer();
        //     var baseURL = urls.apiUrl;
        //     var path = baseURL + '/clients';
        //     var error = 'Error happened when creating client';
        //     $http({
        //         method: 'POST',
        //         url: path,
        //         data: newClient,
        //     }).then(function successCallback(response) {
        //         if (response.status >= 200 && response.status <= 299) {
        //             deferred.resolve(response.data);
        //         } else {
        //             deferred.reject(error);
        //         }

        //     }, function errorCallback(response) {
        //         deferred.reject(error);
        //     });
        //     return deferred.promise;
        // },

        deleteClient: function (clientId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/clients/' + clientId ;
            var error = 'Error happened when deleting client';
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
        getClientProperties: function (clientId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/clients/' + clientId + '/properties';// property should be lower case, as it is formatted already
            var error = 'Error happened when getting client\'s properties with id ' + clientId;
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
        }
    };
}]);
