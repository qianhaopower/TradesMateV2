'use strict';
app.factory('companyService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', function ($http, $q, localStorageService, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var companyServiceFactory = {};

  

    var _updateCompany = function (companyInfo) {

        return $http.put(serviceBase + 'api/companies/ModifyCompany' , companyInfo).then(function (response) {
            return response;
        });

    };
    

    var _getCurrentCompany = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/GetCompanyForCurrentUser').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _getCompanyUsers = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/GetCurrentCompanyMembers').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };



    var _getUserById = function (userId) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/getUserById/' + userId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _deleteUserById = function (userId) {

        var deferred = $q.defer();

        $http.delete(serviceBase + 'api/account/deleteUserById/' + userId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };



    var _createCompanyUser = function (userInfo) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/account/registerCompanyUser', userInfo).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
         return deferred.promise;

    };


    var _updateCompanyUser = function (userInfo) {
        var deferred = $q.defer();
         $http.post(serviceBase + 'api/account/updatecompanyuser', userInfo).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
         return deferred.promise;

    };
 

   

  
    companyServiceFactory.updateCompany = _updateCompany;
    companyServiceFactory.getCurrentCompany = _getCurrentCompany;

    companyServiceFactory.getCompanyUsers = _getCompanyUsers;
    companyServiceFactory.createCompanyUser = _createCompanyUser;

    companyServiceFactory.updateCompanyUser = _updateCompanyUser;
    companyServiceFactory.getUserById = _getUserById;
    companyServiceFactory.deleteUserById = _deleteUserById;
 
    

    return companyServiceFactory;
}]);