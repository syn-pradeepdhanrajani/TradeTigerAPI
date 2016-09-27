(function() {
    'use strict';

    angular
        .module('app.examples.dashboards')
        .directive('loadDataWidget', loadDataWidget);

    /* @ngInject */
    function loadDataWidget($parse, $http, $mdDialog,$interval,$timeout) {
        // Usage:
        //
        // <tri-widget load-data-widget="{ variableName: urlOfJSONData }"></tri-widget>
        // Creates:
        //
        var directive = {
            require: 'triWidget',
            link: link,
            restrict: 'A'
        };
        return directive;

        $http.defaults.headers.put = {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
            'Access-Control-Allow-Headers': 'Content-Type, X-Requested-With'
        };
        $http.defaults.useXDomain = true;


        function link($scope, $element, attrs, widgetCtrl) {
            widgetCtrl.setLoading(true);
            var loadData = $parse(attrs.loadDataWidget)($scope);

            widgetCtrl.setMenu({
                icon: 'zmdi zmdi-more-vert',
                items: [{
                    icon: 'zmdi zmdi-search',
                    title: 'Details',
                    click: function($event) {
                        var data = [];
                        angular.forEach(loadData, function(url, variable) {
                            data = $scope[variable];
                        });
                        $mdDialog.show({
                            controller: LoadDataDialogController,
                            templateUrl: 'app/examples/dashboards/widgets/widget-load-data-dialog.tmpl.html',
                            targetEvent: $event,
                            locals: {
                                data: data
                            },
                            clickOutsideToClose: true
                        })
                        .then(function(answer) {
                            $scope.alert = 'You said the information was "' + answer + '".';
                        }, cancelDialog);
                    }
                },{
                    icon: 'zmdi zmdi-refresh',
                    title: 'Refresh',
                    click: function () {
                        $interval.cancel($scope.intervalPromise);
                        widgetCtrl.setLoading(true);
                        angular.forEach(loadData, function (url, variable) {
                            $http.get(url).
                            success(function (data) {
                                var header = {};
                                widgetCtrl.setLoading(true);
                                $scope[variable] = {
                                    header: header,
                                    data: data
                                };
                                $scope.$apply();
                            });
                        });
                    }
                },{
                    icon: 'zmdi zmdi-share',
                    title: 'Share'
                },{
                    icon: 'zmdi zmdi-print',
                    title: 'Print'
                }]
            });

            function cancelDialog() {
                $scope.alert = 'You cancelled the dialog.';
            }

            ///////////////////

            /* @ngInject */
            function LoadDataDialogController($scope, $mdDialog, data) {
                $scope.data = data;

                $scope.closeDialog = function() {
                    $mdDialog.cancel();
                };
            }

            //var headerConfig = {
            //    headers: {}
            //};

            $interval(function () {
                delete $http.defaults.headers.common['X-Requested-With'];
                angular.forEach(loadData, function (url, variable) {
                    $http.get(url).
                    success(function (data) {
                        var header = {};
                        widgetCtrl.setLoading(false);
                        $scope[variable] = {
                            header: header,
                            data: data
                        };
                    });
                });
            }, 1000);
        }
    }
})();
