'use strict';

angular.module('sbAdminApp')
    .controller('companyWorkItemTemplateController', ['$scope', 'workItemTemplateService', 'Notification', '$state', 'ModalService',  '$stateParams',
        function ($scope, workItemTemplateService, Notification, $state, ModalService, $stateParams) {

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

    //open template ..

    $scope.openTemplateDetail = function (template, readonly) {
        if (readonly)
            $state.go('base.viewCompanyWorkItemTemplate', { templateId: template.id });
        else
            $state.go('base.editCompanyWorkItemTemplate', { templateId: template.id });
    };

    $scope.addNewTemplate = function () {
        $state.go('base.createCompanyWorkItemTemplate');
    }

    $scope.goBack = function () {
        $state.go('base.manageCompany');
    }

    $scope.deleteTemplate = function (template) {
        var templateRef = template;
        ModalService.showModal({
            templateUrl: 'deleteTemplateModal.html',
            controller: "deleteTemplateModalController"
        }).then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result) {
                    $scope.proceedDelete(templateRef.id);
                }
            });
        });
    }



    $scope.proceedDelete = function (templateId) {
        workItemTemplateService.deleteTemplate(templateId).then(function (result) {
            Notification.success({ message: 'Deleted', delay: 2000 });
            init();// fetch data again
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    };

    $scope.viewWorkItemTemplateAttachments = function (workItemTemplate) {
        $state.go('base.workItemTemplateAttachments', {
            workItemTemplateId: workItemTemplate.id });
    }


    var init = function () {
        workItemTemplateService.getAllTemplates().then(function (result) {
            $scope.templateList = result;
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }
    init();

}]);



angular.module('sbAdminApp').controller('deleteTemplateModalController', function ($scope, close) {

    $scope.close = function (result) {
        close(result, 500); // close, but give 500ms for bootstrap to animate
    };
});