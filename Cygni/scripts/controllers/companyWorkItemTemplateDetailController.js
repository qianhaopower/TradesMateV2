'use strict';

angular.module('sbAdminApp')
    .controller('companyWorkItemTemplateDetailController', ['$scope', 'workItemDataService',
      'Notification', '$state', '$stateParams',
        function ($scope, workItemDataService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.templateId = undefined;
    $scope.template = undefined;
    $scope.readOnly = $state.current.name == 'base.viewCompanyWorkItemTemplate';

    
    $scope.save = function () {

        if ($scope.template.isNew) {
            var templateCopy= angular.copy($scope.template);
            workItemDataService.createTemplate(templateCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 2000 });
                $scope.goToTemplateIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else {
            workItemDataService.editTemplate($scope.template).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToTemplateIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }
    
    }
    $scope.goToTemplateIndex = function () {
        $state.go('base.companyWorkItemTemplate');
    };


    var init = function () {
        $scope.templateId = $stateParams.templateId;
        if ($scope.templateId &&
            ($state.current.name == 'base.editCompanyWorkItemTemplate'
            || $state.current.name == 'base.viewCompanyWorkItemTemplate')) {
            workItemDataService.getTemplateById($scope.templateId).then(function (result) {
                $scope.template = result;
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'base.createCompanyWorkItemTemplate') {
            //create new template
            $scope.template = {
                name: undefined,
                description: undefined,
                isNew: true,
            }
        }
        
    }

    init();

}]);
