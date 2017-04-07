'use strict';

angular.module('sbAdminApp')
    .controller('companyWorkItemTemplateDetailController', ['$scope', 'workItemTemplateService', 'companyService',
      'Notification', '$state', '$stateParams',
        function ($scope, workItemTemplateService, companyService, Notification, $state, $stateParams) {

    $scope.filterTextModel = {
        searchText: undefined,
    };
    $scope.templateId = undefined;
    $scope.template = undefined;
    $scope.readOnly = $state.current.name == 'base.viewCompanyWorkItemTemplate';

    $scope.companyInfo = {
        companyName: undefined,
        description: undefined,
        creditCard: undefined,
        companyId: undefined,
        tradeTypes: $scope.serviceTypes,
    };
    $scope.outputTemplateType = undefined;
    var getCompanyDetail = function () {
        companyService.getCurrentCompany().then(function (company) {
            $scope.companyInfo.companyName = company.companyName;
            $scope.companyInfo.description = company.description;
            $scope.companyInfo.creditCard = company.creditCard;
            $scope.companyInfo.companyId = company.companyId;

            //grab all of the default
            //only show type that company has
            $scope.companyInfo.tradeTypes = _.filter($scope.serviceTypes, function (item) { return company.tradeTypes.indexOf(item.enumValue) > -1 });
            if ($scope.template)//editing
            {   angular.forEach($scope.companyInfo.tradeTypes, function (value, key) {
                /* do your stuff here */
                if ($scope.template.tradeWorkType == value.enumValue) {
                    value.ticked = true;
                } else {
                    value.ticked = false;
                }
               
            });
            }else{
                $scope.companyInfo.tradeTypes[0].ticked = true;//get the default one
            }
            //    $scope.outputTemplateType = _.filter($scope.serviceTypes, function (item) { return $scope.template.tradeWorkType == item.enumValue });

            //else//creating new 
            //    $scope.outputTemplateType = $scope.serviceTypes[0];//get the default one
        }, function (error) { Notification.error({ message: error, delay: 2000 }); });
    }
    $scope.save = function () {

        if ($scope.template.isNew) {
            var templateCopy = angular.copy($scope.template);
            //get the type from selection
            templateCopy.tradeWorkType = $scope.outputTemplateType[0].enumValue;
            workItemTemplateService.createTemplate(templateCopy).then(function (result) {
                Notification.success({ message: 'Created', delay: 2000 });
                $scope.goToTemplateIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else {
            $scope.template.tradeWorkType = $scope.outputTemplateType[0].enumValue;
            workItemTemplateService.editTemplate($scope.template.id, $scope.template).then(function (result) {
                Notification.success({ message: 'Saved', delay: 2000 });
                $scope.goToTemplateIndex();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        }
    
    }
    $scope.goToTemplateIndex = function () {
        $state.go('base.companyWorkItemTemplate');
    };

    $scope.serviceTypes = companyService.getDefaultServices();
    var init = function () {
        $scope.templateId = $stateParams.templateId;
        if ($scope.templateId &&
            ($state.current.name == 'base.editCompanyWorkItemTemplate'
            || $state.current.name == 'base.viewCompanyWorkItemTemplate')) {
            workItemTemplateService.getTemplateById($scope.templateId).then(function (result) {
                $scope.template = result;
                getCompanyDetail();
            }, function (error) { Notification.error({ message: error, delay: 2000 }); });
        } else if ($state.current.name == 'base.createCompanyWorkItemTemplate') {
            //create new template
            $scope.template = {
                name: undefined,
                description: undefined,
                isNew: true,
            }
            getCompanyDetail();
        }
        
        
    }

    init();

}]);
