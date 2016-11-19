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
    
    var _updateMemberRole = function (memberId, role) {

        var deferred = $q.defer();
        $http.post(serviceBase + 'api/companies/UpdateCompanyMemberRole?memberId=' + memberId + '&role=' + role).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };
    var _getCompanyMembers = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/GetCurrentCompanyMembers').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _getCompanies = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/Companies/getcompanies').success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };

    var _searchMemberForJoinCompany = function (searchText) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/SearchMemberForJoinCompany?searchText=' + searchText).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;

    };



    var _getMemberById = function (memberId) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/companies/GetCurrentCompanyMember?memberId=' + memberId).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _deleteMemberById = function (memberId) {

        var deferred = $q.defer();


        //start from here
        $http.delete(serviceBase + 'api/companies/RemoveMember?memberId=' + memberId).success(function (response) {//here we just delete the join between company and member, we never delete user.
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };



    var _addNewMemberToCompany = function (userInfo) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/account/registerCompanyUser', userInfo).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
         return deferred.promise;

    };



    var _addExistingMemberToCompany = function (addMemberModel) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/companies/AddExistingMemberToCompany', addMemberModel).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
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
    //var _updateCompanyMember = function (memberInfo) {
    //    var deferred = $q.defer();
    //    $http.post(serviceBase + 'api/account/updatecompanyUser', memberInfo).success(function (response) {//broken
    //        deferred.resolve(response);
    //    }).error(function (err, status) {
    //        deferred.reject(err);
    //    });
    //     return deferred.promise;

    //};
 

   

  
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
    

    
  
 
    

    return companyServiceFactory;
}]);