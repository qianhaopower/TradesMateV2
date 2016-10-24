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



    var _createCompanyUser = function (userInfo) {
        var deferred = $q.defer();
        $http.post(serviceBase + 'api/account/registerCompanyUser', userInfo).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            deferred.reject(err);
        });
         return deferred.promise;

    };


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
    companyServiceFactory.createCompanyUser = _createCompanyUser;

    //companyServiceFactory.updateCompanyMember = _updateCompanyMember;
    companyServiceFactory.getMemberById = _getMemberById;
    companyServiceFactory.deleteMemberById = _deleteMemberById;
    companyServiceFactory.updateMemberRole = _updateMemberRole;
  
 
    

    return companyServiceFactory;
}]);