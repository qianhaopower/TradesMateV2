'use strict';
var app = angular.module('sbAdminApp');
app.factory('propertyDataService', [ '$q', '$http', '$window', 'urls',function ( $q, $http, $window, urls) {

    
  
    return {
        //$scope.url = urls.apiUrl;    
        getAllProperties: function () {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Properties';
            var error = 'Error happened when getting properties';  
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
        getPropertyCompanies: function (propertyId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Properties(' + propertyId + ')/GetCompanies' ; 
            var error = 'Error happened when getting property companies';
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
        getPropertyById: function (propertyId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Properties(' + propertyId + ')?$expand=address';
            var error = 'Error happened when getting property with id ' + propertyId;
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
        editProperty: function (property) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Properties(' + property.id + ')';
            var error = 'Error happened when saving property with id ' + property.id;
            $http({
                method: 'PATCH',
                url: path,
                data:property,
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
        createProperty: function (newProperty) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Properties';
            var error = 'Error happened when creating property';
            $http({
                method: 'POST',
                url: path,
                data: newProperty,
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
        createProperty: function (newProperty) {

            //find out a way allow passing extra to server. 
            //Or find a pattern to remove all extra property
            newProperty.isNew = undefined;

            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Properties';
            var error = 'Error happened when creating property';
            $http({
                method: 'POST',
                url: path,
                data: newProperty,
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

        deleteProperty: function (propertyId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Properties(' + propertyId + ')';
            var error = 'Error happened when deleting property';
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


    };
}]);


