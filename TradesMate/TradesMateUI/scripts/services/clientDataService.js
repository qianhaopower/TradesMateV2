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
            var path = baseURL + '/Clients';//Odata is case sensitive
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


        //getFakeClientList: function () {
        //    //call get
        //    return dataModel;
        //},
        //getPerson: function (personId) {
        //    return getPerson(personId);
        //},
        //getPersonComponent: function (personId, componentId) {
        //    for (var i = 0 ; i < dataModel.length; i++) {
        //        if (dataModel[i].id == personId) {

        //            if (dataModel[i].componentList) {
        //                for (var j = 0 ; j < dataModel[i].componentList.length; j++) {
        //                    if (dataModel[i].componentList[j].id == componentId) {
        //                        return dataModel[i].componentList[j];
        //                    }
        //                }
        //            }

        //        }
        //    }
        //},
        //setClientPropertyComponent: function (personId, componentList) {
        //    // add the componentList to the dataModel
        //    for (var i = 0 ; i < dataModel.length; i++) {
        //        if (dataModel[i].id == personId) {
        //            dataModel[i].componentList = componentList;
        //        }
        //    }

        //},
        //getClientPropertyComponent: function (personId) {
        //    for (var i = 0 ; i < dataModel.length; i++) {
        //        if (dataModel[i].id == personId) {
        //            return dataModel[i].componentList;
        //        }
        //    }

        //},
        //setClientPropertyComponentItem: function (personId, componentId, items) {

        //    for (var i = 0 ; i < dataModel.length; i++) {
        //        if (dataModel[i].id == personId) {

        //            if (dataModel[i].componentList) {
        //                for (var j = 0 ; j < dataModel[i].componentList.length; j++) {
        //                    if (dataModel[i].componentList[j].id == componentId) {
        //                        dataModel[i].componentList[j].items = items;
        //                    }
        //                }
        //            }

        //        }
        //    }

        //},
        //getClientPropertyComponentItem: function (personId, componentId) {

        //    for (var i = 0 ; i < dataModel.length; i++) {
        //        if (dataModel[i].id == personId) {

        //            if (dataModel[i].componentList) {
        //                for (var j = 0 ; j < dataModel[i].componentList.length; j++) {
        //                    if (dataModel[i].componentList[j].id == componentId) {
        //                        return dataModel[i].componentList[j].items
        //                    }
        //                }
        //            }

        //        }
        //    }

        //},

        //generateReport: function (personId) {
        //    var personDataModel = getPerson(personId);
        //    if (!personDataModel.componentList || personDataModel.componentList.length == 0) {
        //        $window.alert('Please at least insert one item to generate a report.');
        //    } else {
        //        //do the save;
        //        $http({
        //            method: 'POST',
        //            url: 'http://tradedata.azurewebsites.net/api/reports',
        //            data: { Id: personId, JsonContent: JSON.stringify(personDataModel) }
        //        }).then(function successCallback(response) {
        //            $window.alert('Report generated');
        //        }, function errorCallback(response) {
        //            $window.alert('Error happend' + response);
        //        });

        //    }
        //},
        //geReportList: function (callback) {

        //    //do the save;
        //    $http({
        //        method: 'GET',
        //        url: 'http://tradedata.azurewebsites.net/api/reports'

        //    }).then(function successCallback(response) {
        //        callback(response.data)
        //    }, function errorCallback(response) {
        //        $window.alert('Error happend' + response);
        //    });


        //},
        //geReportById: function (id, callback) {
        //    $window.open('http://tradedata.azurewebsites.net/api/reports/' + id);
        //    //do the save;
        //    //$http({
        //    //    method: 'GET',
        //    //    url: 'http://tradedata.azurewebsites.net/api/reports/'+id

        //    //}).then(function successCallback(response) {
        //    //    if(callback)
        //    //    callback(response.data)
        //    //}, function errorCallback(response) {
        //    //    $window.alert('Error happend' + response);
        //    //});


        //}


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
