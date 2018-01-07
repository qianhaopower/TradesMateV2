'use strict';
var app = angular.module('sbAdminApp');
app.factory('propertyDataService', ['$q', '$http', '$window', 'urls', 'ngAuthSettings', function ($q, $http, $window, urls, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
  
    return {
        //$scope.url = urls.apiUrl;    
        getAllProperties: function () {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/properties';
            var error = 'Error happened when getting properties';  
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
        getPropertyCompanies: function (propertyId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/properties/' + propertyId + '/company' ; 
            var error = 'Error happened when getting property companies';
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
        getMemberAllocation: function (memberId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/' + memberId + '/allocation';
            var error = 'Error happened when getting property allocations';
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
        updateMemberAllocation: function (propertyId , memberId, allocated) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/' + propertyId + '/allocation/' + memberId + '/' +allocated ;

            var error = 'Error happened when updating property allocations';
            $http({
                method: 'POST',
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
           
        getPropertyById: function (propertyId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/properties/' + propertyId ;
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
            property.address = undefined;
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/properties';
            var error = 'Error happened when saving property with id ' + property.id;
            $http({
                method: 'PUT',
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
            //var data = { property: propertyModel, messageId: messageId };
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            $http.post(baseURL + '/properties', newProperty).then(function (response) {
                deferred.resolve(response.data);
            },function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        },
        getPropertyReportItems: function (propertyId) {
          
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            $http.get(baseURL + '/properties/'+ propertyId + '/report').then(function (response) {
                deferred.resolve(response.data);
            }, function (err, status) {
                deferred.reject(err);
            });
            return deferred.promise;
        },

        deleteProperty: function (propertyId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/properties/' + propertyId;
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


