'use strict';

angular.module('sbAdminApp')
  .controller('propertySectionDetailController', ['$scope', 'propertySectionDataService',
      'Notification', '$state', '$stateParams',
function ($scope, propertySectionDataService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.sectionId = undefined;
    $scope.section = undefined;
    $scope.readOnly = $state.current.name == 'dashboard.viewPropertySection';

    

    $scope.save = function () {

        if ($scope.section.isNew) {
            var sectionCopy= angular.copy($scope.section);
            propertySectionDataService.createSection(sectionCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 2000 });
                $scope.goToSectionIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else {
            propertySectionDataService.editSection($scope.section).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToSectionIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }
    
    }
    $scope.goToSectionIndex = function () {
        $state.go('dashboard.propertySections', { propertyId: $stateParams.propertyId });
    };


    var init = function () {
        $scope.sectionId = $stateParams.sectionId;
        if ($scope.sectionId &&
            ($state.current.name == 'dashboard.editPropertySection'
            || $state.current.name == 'dashboard.viewPropertySection')) {
            propertySectionDataService.getSectionById($scope.sectionId).then(function (result) {
                $scope.section = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'dashboard.createPropertySection') {
            //create new section
            $scope.section = {
                name: undefined,
                propertyId: $stateParams.propertyId,
                isNew: true,
            }
        }
        //propertySectionDataService.getAllSections().then(function (result) {
        //    $scope.sectionlist = result;
        //}, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);
