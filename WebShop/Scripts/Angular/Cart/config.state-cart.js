(function () {
    'use strict';
    var app = angular.module("globalApp");
    app.injectRequires(['ui.router']);
    app.config(config);
    config.$inject = ['$stateProvider'];

    function config($stateProvider) {
        $stateProvider.state('details', {
            templateUrl: "details.html" 
        })
            .state('correct-details', {
                templateUrl: 'correct-details.html'

            });
    }

})();