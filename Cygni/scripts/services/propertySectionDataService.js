'use strict';
var app = angular.module('sbAdminApp');
app.factory('propertySectionDataService', [ '$q', '$http', '$window', 'urls',function ( $q, $http, $window, urls) {

    
  
    return {
        //$scope.url = urls.apiUrl;
        getPropertyAllSections: function (propertyId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/properties/' + propertyId + '/section';//entity property should be lower case, as it is formatted already
            var error = 'Error happened when getting property\'s sections with id ' + propertyId;
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

        getSectionById: function (sectionId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/sections/' + sectionId ;
            var error = 'Error happened when getting section with id ' + sectionId;
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
        editSection: function (section) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/sections';
            var error = 'Error happened when saving section with id ' + section.id;
            $http({
                method: 'PUT',
                url: path,
                data: section,
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
        
        createSection: function (newSection) {

            //find out a way allow passing extra to server. 
            //Or find a pattern to remove all extra property
            newSection.isNew = undefined;

            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/sections';
            var error = 'Error happened when creating section';
            $http({
                method: 'POST',
                url: path,
                data: newSection,
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

        deleteSection: function (sectionId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/sections/' + sectionId ;
            var error = 'Error happened when deleting section';
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


