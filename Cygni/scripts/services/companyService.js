﻿'use strict';
app.factory('companyService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', function ($http, $q, localStorageService, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var companyServiceFactory = {};

  

    var _updateCompany = function (companyInfo) {

        return $http.put(serviceBase + 'api/companies' , companyInfo).then(function (response) {
            return response.data;
        },function (err, status) {
            deferred.reject(err);
        });

    };
    

    var _getCurrentCompany = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies').then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };
    
    var _updateMemberRole = function (memberId, role) {

        var deferred = $q.defer();
        $http.put(serviceBase + 'api/companies/member/' + memberId + '?role=' + role).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };
    var _getCompanyMembers = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/member').then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _getCompanies = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/all').then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };

    var _searchMemberForJoinCompany = function (searchText) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/member/search?searchText=' + searchText).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };
    var _searchClientForCompanyInvite = function (searchText) {
        var deferred = $q.defer();
        $http.get(serviceBase + 'api/companies/client/search?searchText=' + searchText).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };




    var _getMemberById = function (memberId) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/member/' + memberId).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _deleteMemberById = function (memberId) {

        var deferred = $q.defer();


        //start from here
        $http.delete(serviceBase + 'api/companies/member/' + memberId).then(function (response) {//here we just delete the join between company and member, we never delete user.
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };



    var _addNewMemberToCompany = function (userInfo) {
        var deferred = $q.defer();
        userInfo.userType =1;//trade
        $http.post(serviceBase + 'api/account/register/company', userInfo).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
         return deferred.promise;

    };



    var _addExistingMemberToCompany = function (addMemberModel) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/companies/member/add', addMemberModel).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };
    var _addNewClientToCompany = function (userInfo) {
        var deferred = $q.defer();
        userInfo.userType =0;//client
        $http.post(serviceBase + 'api/clients', userInfo).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
         return deferred.promise;

    };
    var _addBulkClientToCompany = function (userInfos) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/clients/bulk', userInfos).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
         return deferred.promise;
    };



    var _addExistingClientToCompany = function (addMemberModel) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/companies/client/add', addMemberModel).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };
    // public  enum TradeType
    //{
    //    Electrician,
    //    Handyman,
    //    Plumber,
    //    Builder,
    //    AirConditioning,
    //    }
    var _getDefaultServices = function () {
        var result =  [
    { icon: "<i class=\"fa fa-bolt\" ></i>", name: "Electrician", ticked: true, enumValue: 0 },
    { icon: "<i class=\"fa fa-wrench\" ></i>", name: "Handyman", ticked: false, enumValue: 1 },
    { icon: "<i class=\"fa fa-tint\" ></i>", name: "Plumber", ticked: false, enumValue: 2 },
    { icon: "<i class=\"fa fa-home\" ></i>", name: "Builder", ticked: false, enumValue: 3 },
    { icon: "<i class=\"fa fa-snowflake-o\" ></i>", name: "Air Conditioning", ticked: false, enumValue: 4 }
        ];
        return result;
    }
 

    var _updateMemberServiceTypes = function (memberId, selectedService) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/companies/memeber/update' ,{ memberId: memberId, selectedTypes: selectedService}).then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

  
    companyServiceFactory.updateCompany = _updateCompany;
    companyServiceFactory.getCurrentCompany = _getCurrentCompany;

    companyServiceFactory.getCompanyMembers = _getCompanyMembers;
   
    companyServiceFactory.getMemberById = _getMemberById;
    companyServiceFactory.deleteMemberById = _deleteMemberById;
    companyServiceFactory.updateMemberRole = _updateMemberRole;
    companyServiceFactory.searchMemberForJoinCompany = _searchMemberForJoinCompany;
    companyServiceFactory.addExistingMemberToCompany = _addExistingMemberToCompany;
    companyServiceFactory.addNewMemberToCompany = _addNewMemberToCompany;

    companyServiceFactory.getDefaultServices = _getDefaultServices;
    companyServiceFactory.getCompanies = _getCompanies;
    companyServiceFactory.updateMemberServiceTypes = _updateMemberServiceTypes;

    companyServiceFactory.searchClientForCompanyInvite = _searchClientForCompanyInvite;
    companyServiceFactory.addExistingClientToCompany = _addExistingClientToCompany;
    companyServiceFactory.addNewClientToCompany = _addNewClientToCompany;
    companyServiceFactory.addBulkClientToCompany = _addBulkClientToCompany;

    return companyServiceFactory;
}]);