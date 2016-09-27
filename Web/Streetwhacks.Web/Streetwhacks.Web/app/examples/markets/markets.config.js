(function() {
    'use strict';

    angular
        .module('app.examples.markets')
        .config(moduleConfig);

    /* @ngInject */
    function moduleConfig($stateProvider, triMenuProvider) {

        $stateProvider
        .state('triangular.markets-layout', {
            abstract: true,
            views: {
                sidebarLeft: {
                    templateUrl: 'app/triangular/components/menu/menu.tmpl.html',
                    controller: 'MenuController',
                    controllerAs: 'vm'
                },
                content: {
                    template: '<div id="admin-panel-content-view" flex ui-view></div>'
                },
                belowContent: {
                    template: '<div ui-view="belowContent"></div>'
                }
            }
        });
       
        triMenuProvider.addMenu({
            name: 'Commodities',
            icon: 'zmdi',
            type: 'dropdown',
            priority: 1.1,
            children: [{
                name: 'Intra-day',
                state: 'triangular.dashboard-analytics',
                type: 'link'
            },{
                name: 'Positional',
                state: '',
                type: 'link'
            }]
        });

    }
})();
