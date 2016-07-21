'use strict';

angular.module('sbAdminApp')
  .controller('propertyDetailController', ['$scope', 'propertyDataService', 'clientDataService',
      'Notification', '$state', '$stateParams',
function ($scope, propertyDataService, clientDataService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.propertyId = undefined;
    $scope.property = undefined;
    $scope.clientId = undefined;
    $scope.client = undefined;

    $scope.readOnly = $state.current.name == 'dashboard.viewProperty';

    

    $scope.save = function () {
        $scope.property.address = undefined;

        if ($scope.property.isNew) {
            var propertyCopy = angular.copy($scope.property);
            propertyCopy.clientId = $stateParams.clientId;
            propertyDataService.createProperty(propertyCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 2000 });
                $scope.goToPropertyIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else {
            propertyDataService.editProperty($scope.property).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToPropertyIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }
    
    }
    $scope.goToPropertyIndex = function () {
        if ($scope.property.clientId)
            $state.go('dashboard.clientProperties', { param: $scope.property.clientId });
        else
            $state.go('dashboard.properties');
    };


    $scope.myInterval = 5000;
    var slides = $scope.slides = [];
    $scope.addSlide = function () {
        var newWidth = 600 + slides.length + 1;
        slides.push({
            image: 'http://placekitten.com/' + newWidth + '/300',
            text: ['More', 'Extra', 'Lots of', 'Surplus'][slides.length % 4] + ' ' +
              ['Cats', 'Kittys', 'Felines', 'Cutes'][slides.length % 4]
        });
    };
    for (var i = 0; i < 4; i++) {
        $scope.addSlide();
    }

    var init = function () {

        if ($state.current.name == 'dashboard.createProperty') {
            $scope.clientId = $stateParams.clientId;
        } else {
            $scope.propertyId = $stateParams.propertyId;
        }
       
        if ($state.current.name == 'dashboard.editProperty' || $state.current.name == 'dashboard.viewProperty') {
            propertyDataService.getPropertyById($scope.propertyId).then(function (result) {
                $scope.property = result;
                $scope.clientId = result.clientId;

                if ($scope.clientId) {
                    clientDataService.getClientById($scope.clientId).then(function (result) {
                        $scope.client = result

                    }, function (error) { Notification.error({ message: error, delay: 2000 }); });
                }

            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'dashboard.createProperty') {
            //create new property
            $scope.property = {
                condition:'Normal',//default
                isNew: true,
            }
        }
        //propertyDataService.getAllPropertys().then(function (result) {
        //    $scope.propertylist = result;
        //}, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);
