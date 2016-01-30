(function() {
    'use strict';
    var app = angular.module("globalApp");
    app.injectRequires(['lazyLoadSimpleApp', 'ui.bootstrap']);
    app.controller("historyUserCtrl", historyUserCtrl);
    app.controller("exclusiveCtrl", actionExclusiveWidget);


    actionExclusiveWidget.$inject = ["$scope", 'httpService'];
    historyUserCtrl.$inject = ["$scope","lazyLoadSvc"];

    function historyUserCtrl($scope,lazy) {
        var vm = this;
        vm.items = [];
        vm.items = lazy.setupLazy('/User/History','history-orders');
      

    }
    function actionExclusiveWidget(scope, http) {
        scope.exclusiveGoods = [];
        scope.isWaiter = true;
        http.getRequest({ count: 10 }, "/api/Good/RandomGood").then(function (d) {
            scope.exclusiveGoods = d.data;
            scope.isWaiter = false;
        });
    }
})();