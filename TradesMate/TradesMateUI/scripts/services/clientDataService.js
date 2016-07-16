'use strict';
var app = angular.module('sbAdminApp');
app.factory('clientDataService', [ '$q', '$http', '$window', 'urls',function ( $q, $http, $window, urls) {
    // var resource = $resource('/api/Personnel/:section/:id', { section: '@section', id: '@id' });
    var dataModel = getFakeClientList();// universal data model
    //var getPerson = function (personId) {
    //    for (var i = 0 ; i < dataModel.length; i++) {
    //        if (dataModel[i].id == personId) {
    //            return dataModel[i];
    //        }
    //    }
    //};
    return {
        //$scope.url = urls.apiUrl;
        getAllClients: function () {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Clients?$expand=address';//Odata is case sensitive
            var error = 'Error happened when getting clients';  
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
        getClientById: function (clientId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Clients(' + clientId + ')?$expand=address';
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
            var path = baseURL + '/Clients(' + client.id + ')';
            var error = 'Error happened when saving client with id ' + client.id;
            $http({
                method: 'PATCH',
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
        
        createClient: function (newClient) {

            //find out a way allow passing extra to server. 
            //Or find a pattern to remove all extra property
            newClient.isNew = undefined;

            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Clients';
            var error = 'Error happened when creating client';
            $http({
                method: 'POST',
                url: path,
                data: newClient,
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

        deleteClient: function (clientId) {
            var deferred = $q.defer();
            var baseURL = urls.apiUrl;
            var path = baseURL + '/Clients(' + clientId + ')';
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
            var path = baseURL + '/Clients(' + clientId + ')/properties';// property should be lower case, as it is formatted already
            var error = 'Error happened when getting client\'s properties with id ' + clientId;
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
        }




    };
}]);



var getFakeClientList = function () {

    var list = [{
        "id": 1,
        "gender": "Male",
        "first_name": "Peter",
        "last_name": "Williams",
        "email": "pwilliams0@creativecommons.org",
        "Address": "66 Nobel Alley"
    }, {
        "id": 2,
        "first_name": "Helen",
        "last_name": "Martin",
        "email": "hmartin1@arstechnica.com",
        "Address": "88457 Texas Parkway"
    }, {
        "id": 3,
        "gender": "Male",
        "first_name": "Todd",
        "last_name": "Johnston",
        "email": "tjohnston2@symantec.com",
        "Address": "0993 Darwin Place"
    }, {
        "id": 4,
        "gender": "Female",
        "first_name": "Phyllis",
        "last_name": "Payne",
        "email": "ppayne3@hhs.gov",
        "Address": "03020 Bluestem Alley"
    }, {
        "id": 5,
        "gender": "Male",
        "first_name": "Gary",
        "last_name": "Jackson",
        "email": "gjackson4@loc.gov",
        "Address": "8 Hanson Terrace"
    }, {
        "id": 6,
        "gender": "Female",
        "first_name": "Sarah",
        "last_name": "Harris",
        "email": "sharris5@wikipedia.org",
        "Address": "2 Lakewood Gardens Center"
    }, {
        "id": 7,
        "gender": "Male",
        "first_name": "Jerry",
        "last_name": "Fernandez",
        "email": "jfernandez6@sciencedaily.com",
        "Address": "850 Lotheville Court"
    }, {
        "id": 8,
        "gender": "Female",
        "first_name": "Patricia",
        "last_name": "Fisher",
        "email": "pfisher7@ox.ac.uk",
        "Address": "87720 Veith Point"
    }, {
        "id": 9,
        "gender": "Female",
        "first_name": "Karen",
        "last_name": "Carpenter",
        "email": "kcarpenter8@ox.ac.uk",
        "Address": "21 Magdeline Avenue"
    }, {
        "id": 10,
        "gender": "Female",
        "first_name": "Heather",
        "last_name": "Carter",
        "email": "hcarter9@mtv.com",
        "Address": "99 Spenser Park"
    }, {
        "id": 11,
        "gender": "Male",
        "first_name": "Samuel",
        "last_name": "Sanchez",
        "email": "ssancheza@canalblog.com",
        "Address": "67 Esch Street"
    }, {
        "id": 12,
        "gender": "Female",
        "first_name": "Frances",
        "last_name": "Brown",
        "email": "fbrownb@drupal.org",
        "Address": "2 Waywood Alley"
    }, {
        "id": 13,
        "gender": "Male",
        "first_name": "Carl",
        "last_name": "Ferguson",
        "email": "cfergusonc@webmd.com",
        "Address": "902 Oriole Alley"
    }, {
        "id": 14,
        "gender": "Female",
        "first_name": "Janice",
        "last_name": "Smith",
        "email": "jsmithd@noaa.gov",
        "Address": "70821 Prentice Terrace"
    }, {
        "id": 15,
        "gender": "Female",
        "first_name": "Carol",
        "last_name": "Garza",
        "email": "cgarzae@pagesperso-orange.fr",
        "Address": "769 Sutteridge Trail"
    }, {
        "id": 16,
        "gender": "Male",
        "first_name": "Joshua",
        "last_name": "Bowman",
        "email": "jbowmanf@diigo.com",
        "Address": "25 Birchwood Parkway"
    }, {
        "id": 17,
        "gender": "Male",
        "first_name": "Kevin",
        "last_name": "Burton",
        "email": "kburtong@dion.ne.jp",
        "Address": "947 Merry Avenue"
    }, {
        "id": 18,
        "gender": "Female",
        "first_name": "Gloria",
        "last_name": "Bishop",
        "email": "gbishoph@mayoclinic.com",
        "Address": "6981 Kenwood Street"
    }, {
        "id": 19,
        "first_name": "Gerald",
        "last_name": "Bowman",
        "email": "gbowmani@pagesperso-orange.fr",
        "Address": "5053 Scoville Hill"
    }, {
        "id": 20,
        "gender": "Female",
        "first_name": "Brenda",
        "last_name": "Spencer",
        "email": "bspencerj@mayoclinic.com",
        "Address": "1 Cascade Park"
    }];
    return list;

};
