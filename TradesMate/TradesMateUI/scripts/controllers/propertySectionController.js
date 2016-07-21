'use strict';

angular.module('sbAdminApp')
  .controller('propertySectionController', ['$scope', 'propertySectionDataService', 'Notification', '$state', 'ModalService',  '$stateParams',
function ($scope, propertySectionDataService, Notification, $state, ModalService, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };

    $scope.search = function (item) {
        if (!$scope.filterTextModel.searchText
            || (item.name && (item.name.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            || (item.description && (item.description.toLowerCase().indexOf($scope.filterTextModel.searchText.toLowerCase()) != -1))
            ) {
            return true;
        }
        return false;
    };

    //open section ..

    $scope.openSectionDetail = function (section, readonly) {
        if (readonly)
            $state.go('dashboard.viewPropertySection', { sectionId: section.id, propertyId: $stateParams.propertyId });
        else
            $state.go('dashboard.editPropertySection', { sectionId: section.id, propertyId: $stateParams.propertyId });

    };
    $scope.addNewSection = function () {
        $state.go('dashboard.createPropertySection', {  propertyId: $stateParams.propertyId });
    }

    $scope.goBack = function () {
        $state.go('dashboard.properties');
    }

    $scope.deleteSection = function (section) {
        var sectionRef = section;
        ModalService.showModal({
            templateUrl: 'deleteSectionmodal.html',
            controller: "deleteSectionModalController"
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result) {
                    $scope.proceedDelete(sectionRef.id);
                }
            });
        });
    }



    $scope.proceedDelete = function (sectionId) {
        propertySectionDataService.deleteSection(sectionId).then(function (result) {
            Notification.success({ message: 'Deleted', delay: 2000 });
            init();// fetch data again
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    $scope.viewWorkItems = function (section) {
        $state.go('dashboard.workItems', { sectionId: section.id, propertyId: $stateParams.propertyId });

    }


    var init = function () {
        propertySectionDataService.getPropertyAllSections($stateParams.propertyId).then(function (result) {
            $scope.sectionlist = result;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }


    init();

}]);



angular.module('sbAdminApp').controller('deleteSectionModalController', function ($scope, close) {

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
});