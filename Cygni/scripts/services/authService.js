'use strict';
app.factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings','Notification', function ($http, $q, localStorageService, ngAuthSettings, Notification) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        userName: "",
        useRefreshTokens: false
    };

    var _externalAuthData = {
        provider: "",
        userName: "",
        externalAccessToken: "" 
    };

    var _saveRegistration = function (registration) {

        _logOut();

        return $http.post(serviceBase + 'api/account/register', registration).then(function (response) {
            return response;
        });

    };

    var _updateUser = function (userInfo) {

        return $http.post(serviceBase + 'api/account/updateuser', userInfo).then(function (response) {
            return response;
        });

    };
    

    var _getCurrentUser = function () {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/getcurrentuser').then(function (response) {
            deferred.resolve(response.data);
        }, function (err, status) {
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password;

        if (loginData.useRefreshTokens) {
            data = data + "&client_id=" + ngAuthSettings.clientId;
        }

        var deferred = $q.defer();

        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {

            if (loginData.useRefreshTokens) {
                localStorageService.set('authorizationData', {
                    token: response.data.access_token, userName: loginData.userName,
                    refreshToken: response.data.refresh_token, useRefreshTokens: true, userRole: response.data.userRole, userType: response.data.userType
                });
            }
            else {
                localStorageService.set('authorizationData', {
                    token: response.data.access_token, userName: loginData.userName,
                    refreshToken: "", useRefreshTokens: false, userRole: response.data.userRole, userType: response.data.userType
                });
            }
            _authentication.isAuth = true;
            _authentication.userName = loginData.userName;  
            _authentication.useRefreshTokens = loginData.useRefreshTokens;
            _authentication.userRole = response.data.userRole;
            _authentication.userType = response.data.userType;


            _getCurrentUser().then(function (currentUser) {
                _authentication.userInfo = currentUser;
                deferred.resolve(response.data);

            }, function (error) { Notification.error({ message: error, delay: 2000 }); });


        },function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.userName = "";
        _authentication.useRefreshTokens = false;
        _authentication.userRole = undefined;
        _authentication.userType = undefined;
        _authentication.userInfo = undefined ;

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.userName = authData.userName;
            _authentication.useRefreshTokens = authData.useRefreshTokens;
            _authentication.userRole = authData.userRole;
            _authentication.userType = authData.userType;
        }

    };

    var _refreshToken = function () {
        var deferred = $q.defer();

        var authData = localStorageService.get('authorizationData');

        if (authData) {

            if (authData.useRefreshTokens) {

                var data = "grant_type=refresh_token&refresh_token=" + authData.refreshToken + "&client_id=" + ngAuthSettings.clientId;

                localStorageService.remove('authorizationData');

                $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).then(function (response) {

                    localStorageService.set('authorizationData', { token: response.data.access_token, userName: response.data.userName, refreshToken: response.data.refresh_token, useRefreshTokens: true });

                    deferred.resolve(response.data);

                },function (err, status) {
                    _logOut();
                    deferred.reject(err);
                });
            }
        }

        return deferred.promise;
    };

    var _obtainAccessToken = function (externalData) {

        var deferred = $q.defer();

        $http.get(serviceBase + 'api/account/ObtainLocalAccessToken', { params: { provider: externalData.provider, externalAccessToken: externalData.externalAccessToken } }).then(function (response) {

            localStorageService.set('authorizationData', { token: response.data.access_token, userName: response.data.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.data.userName;
            _authentication.useRefreshTokens = false;
            _authentication.userRole = response.data.userRole;
            _authentication.userType = response.data.userType;
          
            deferred.resolve(response.data);

        },function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _registerExternal = function (registerExternalData) {

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/account/registerexternal', registerExternalData).then(function (response) {

            localStorageService.set('authorizationData', { token: response.data.access_token, userName: response.data.userName, refreshToken: "", useRefreshTokens: false });

            _authentication.isAuth = true;
            _authentication.userName = response.data.userName;
            _authentication.useRefreshTokens = false;
            _authentication.userRole = response.data.userRole;
            _authentication.userType = response.data.userType;

            deferred.resolve(response.data);

        },function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };
    var _sendResetPasswordRequest = function (email) {

        var deferred = $q.defer();

        $http.post(serviceBase + 'api/account/resetpassword?email='+ email  ).then(function (response) {
            deferred.resolve(response.data);

        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };
    var _sendResetPasswordCallBack = function (userId, code, password) {
        var deferred = $q.defer();
        var data =  {userId:userId,code:code,password:password};
        $http.post(serviceBase + 'api/account/resetpassword/callback',data).then(function (response) {
            deferred.resolve(response.data);
        },function (err, status) {
            deferred.reject(err);
        });
        return deferred.promise;
    };

    authServiceFactory.saveRegistration = _saveRegistration;
    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;
    authServiceFactory.refreshToken = _refreshToken;

    authServiceFactory.obtainAccessToken = _obtainAccessToken;
    authServiceFactory.externalAuthData = _externalAuthData;
    authServiceFactory.registerExternal = _registerExternal;
    authServiceFactory.updateUser = _updateUser;
    authServiceFactory.getCurrentUser = _getCurrentUser;
    authServiceFactory.sendResetPasswordRequest =_sendResetPasswordRequest;
    authServiceFactory.sendResetPasswordCallBack =_sendResetPasswordCallBack;

    return authServiceFactory;
}]);