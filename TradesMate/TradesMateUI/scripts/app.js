'use strict';
/**
 * @ngdoc overview
 * @name sbAdminApp
 * @description
 * # sbAdminApp
 *
 * Main module of the application.
 */
angular
  .module('sbAdminApp', [
    'oc.lazyLoad',
    'ui.router',
    'ui.bootstrap',
    'angular-loading-bar',
    'ui-notification',
    'ngMessages',
    'angularModalService',
  ])
  .constant('domain', 'http://localhost')
  .constant('api', '/DataService/odata')
  .service('urls', function (domain, api) { this.apiUrl = domain + api; })
  .config(['$stateProvider', '$urlRouterProvider', '$ocLazyLoadProvider', 'NotificationProvider',
      function ($stateProvider, $urlRouterProvider, $ocLazyLoadProvider, NotificationProvider) {

          $ocLazyLoadProvider.config({
              debug: false,
              events: true,
          });

          $urlRouterProvider.otherwise('/dashboard/home');

          $stateProvider
            .state('dashboard', {
                url: '/dashboard',
                templateUrl: 'views/dashboard/main.html',
                resolve: {
                    loadMyDirectives: function ($ocLazyLoad) {
                        return $ocLazyLoad.load(
                        {
                            name: 'sbAdminApp',
                            files: [
                            'scripts/directives/header/header.js',
                            'scripts/directives/header/header-notification/header-notification.js',
                            'scripts/directives/sidebar/sidebar.js',
                            'scripts/directives/sidebar/sidebar-search/sidebar-search.js'
                            ]
                        }),
                        $ocLazyLoad.load(
                        {
                            name: 'toggle-switch',
                            files: ["bower_components/angular-toggle-switch/angular-toggle-switch.min.js",
                                   "bower_components/angular-toggle-switch/angular-toggle-switch.css"
                            ]
                        }),
                        $ocLazyLoad.load(
                        {
                            name: 'ngAnimate',
                            files: ['bower_components/angular-animate/angular-animate.js']
                        })
                        $ocLazyLoad.load(
                        {
                            name: 'ngCookies',
                            files: ['bower_components/angular-cookies/angular-cookies.js']
                        })
                        $ocLazyLoad.load(
                        {
                            name: 'ngResource',
                            files: ['bower_components/angular-resource/angular-resource.js']
                        })
                        $ocLazyLoad.load(
                        {
                            name: 'ngSanitize',
                            files: ['bower_components/angular-sanitize/angular-sanitize.js']
                        })
                        $ocLazyLoad.load(
                        {
                            name: 'ngTouch',
                            files: ['bower_components/angular-touch/angular-touch.js']
                        })
                    }
                }
            })
            .state('dashboard.home', {
                url: '/home',
                controller: 'MainCtrl',
                templateUrl: 'views/dashboard/home.html',
                resolve: {
                    loadMyFiles: function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: [
                            //controllers
                            'scripts/controllers/main.js',
                            'scripts/controllers/clientController.js',
                            'scripts/controllers/propertyController.js',
                            'scripts/controllers/propertySectionController.js',
                            'scripts/controllers/workItemController.js',

                             //services
                            'scripts/services/clientDataService.js',


                            //directives
                            'scripts/directives/timeline/timeline.js',
                            'scripts/directives/notifications/notifications.js',
                            'scripts/directives/chat/chat.js',
                            'scripts/directives/dashboard/stats/stats.js'


                            ]
                        })
                    }
                }
            })
            .state('dashboard.form', {
                templateUrl: 'views/form.html',
                url: '/form'
            })
            .state('dashboard.blank', {
                templateUrl: 'views/pages/blank.html',
                url: '/blank'
            })
            .state('login', {
                templateUrl: 'views/pages/login.html',
                url: '/login'
            })
            .state('dashboard.chart', {
                templateUrl: 'views/chart.html',
                url: '/chart',
                controller: 'ChartCtrl',
                resolve: {
                    loadMyFile: function ($ocLazyLoad) {
                        return $ocLazyLoad.load({
                            name: 'chart.js',
                            files: [
                              'bower_components/angular-chart.js/dist/angular-chart.min.js',
                              'bower_components/angular-chart.js/dist/angular-chart.css'
                            ]
                        }),
                        $ocLazyLoad.load({
                            name: 'sbAdminApp',
                            files: ['scripts/controllers/chartContoller.js']
                        })
                    }
                }
            })
            .state('dashboard.table', {
                templateUrl: 'views/table.html',
                url: '/table'
            })
            .state('dashboard.panels-wells', {
                templateUrl: 'views/ui-elements/panels-wells.html',
                url: '/panels-wells'
            })
            .state('dashboard.buttons', {
                templateUrl: 'views/ui-elements/buttons.html',
                url: '/buttons'
            })
            .state('dashboard.notifications', {
                templateUrl: 'views/ui-elements/notifications.html',
                url: '/notifications'
            })
            .state('dashboard.typography', {
                templateUrl: 'views/ui-elements/typography.html',
                url: '/typography'
            })
            .state('dashboard.icons', {
                templateUrl: 'views/ui-elements/icons.html',
                url: '/icons'
            })
            .state('dashboard.grid', {
                templateUrl: 'views/ui-elements/grid.html',
                url: '/grid'
            })

         //start from here

              //client
            .state('dashboard.clients', {
                templateUrl: 'views/clients.html',
                controller: 'clientController',
                url: '/clients',
                //resolve: {
                //    loadMyFiles: function ($ocLazyLoad) {
                //        return $ocLazyLoad.load({
                //            name: 'sbAdminApp',
                //            files: [
                //            'scripts/controllers/clientController.js',
                //            'scripts/services/clientDataService.js',
                //            ]
                //        })
                //    }
                //}
            })
               .state('dashboard.editClient', {
                   templateUrl: 'views/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/edit/:param',
               })
               .state('dashboard.createClient', {
                   templateUrl: 'views/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/create',
               })
               .state('dashboard.viewClient', {
                   templateUrl: 'views/clientDetail.html',
                   controller: 'clientDetailController',
                   url: '/client/view/:param',
               })



              //property
             .state('dashboard.properties', {
                 templateUrl: 'views/properties.html',
                 controller: 'propertyController',
                 url: '/properties'
             })
              .state('dashboard.createProperty', {
                  templateUrl: 'views/propertyDetail.html',
                  controller: 'propertyDetailController',
                  url: '/client/:clientId/property/create',
              })
               .state('dashboard.editProperty', {
                   templateUrl: 'views/propertyDetail.html',
                   controller: 'propertyDetailController',
                   url: '/property/edit/:propertyId',
               })
               .state('dashboard.viewProperty', {
                   templateUrl: 'views/propertyDetail.html',
                   controller: 'propertyDetailController',
                   url: '/property/view/:propertyId',
               })
             .state('dashboard.clientProperties', {
                  templateUrl: 'views/properties.html',
                  controller: 'propertyController',
                  url: '/client/:param/properties'
             })



              //propertySection
             .state('dashboard.property-sections', {
                 templateUrl: 'views/property-sections.html',
                 controller: 'propertySectionController',
                 url: '/property-sections'
             })
             .state('dashboard.workitems', {
                 templateUrl: 'views/workitems.html',
                 controller: 'workItemController',
                 url: '/workitems'
             });


          //NotificationProvider.setOptions({
          //    delay: 10000,
          //    startTop: 20,
          //    startRight: 10,
          //    verticalSpacing: 20,
          //    horizontalSpacing: 20,
          //    positionX: 'left',
          //    positionY: 'bottom'
          //});
      }]);



