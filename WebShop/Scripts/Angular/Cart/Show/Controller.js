//var module = angular.module("cartApp", ['httpApp']);
var module = angular.module("globalApp");
var arr = ["httpApp"];
Array.prototype.push.apply(module.requires, arr);

module.controller("ctrl", ["$scope", "$timeout", "httpService", function (scope, timeout, http) {
    var messages = {
        remove: 'Do you want to remove products?',
        buy: 'Do you want to buy products?'
    }
    var actions = {};

    scope.currentItem = { Id: undefined };
    scope.isActive = true;
    scope.items = { count: 0, totalAmount: 0 };
    scope.modal = {
        message: '',
        isModal: false,
        action: function () { }
    };
    scope.showModal = function (m) {
        scope.isActive = false;
        scope.modal.isModal = true;
        scope.modal.action = actions[m];
        scope.modal.message = messages[m];
    };
    scope.hideModal = function () {
        scope.isActive = true;
        scope.modal.isModal = false;
    };
    scope.result = {
        message: '',
        css: ''
    };
    var time;
    var clearResult = function () {
        scope.isActive = true;
        timeout.cancel(time);
        time = timeout(function () {
            scope.result.message = '';
            scope.result.css = '';
        }, 5000, true);
    }
    var successRemove = function (d) {
        scope.result.css = 'success';
        scope.result.message = d.statusText;
        scope.items.count = parseInt(d.data);

        var elem = angular.element(document.querySelector("#item-" + scope.currentItem.Id));
        elem.scope().$destroy();
        elem.remove();
        scope.items.totalAmount = 0;
        scope.$broadcast("calculate");
        clearResult();
    }
    var success = function (d) {
        scope.result.css = 'success';
        scope.result.message = d.statusText;
        clearResult();
    }
    var error = function (d) {
        scope.result.css = 'label label-danger';
        scope.result.message = d.statusText;
        clearResult();
    }
    var remove = function () {
        
        http.postRequest({ id: scope.currentItem.Id }, "/Cart/Remove", null).then(successRemove, error);
    };
    
    var buy = function () {
        var data = $("form").serialize();
        var headers = { 'Content-Type': 'application/x-www-form-urlencoded' }
        http.postRequest(data, "/Cart/Buy", headers).then(success, error);
    };

    actions = {
        remove: remove,
        buy: buy
    };
   
    
    
}]);
module.controller("views", [
    "$scope", "httpService", function (scope,http) {
        
        scope.view = {
            current: 'cart',
            history: [],
            userInfo:{}
        };
        
        var selectView = function(e) {

            var elem = $(e.target);
            var data = elem.data("view");
            
            scope.view.current = data;
            scope.$apply();

        };
        scope.getHistory = function (name) {

            http.requestOneDataApi("/api/Sale/GetHistory", name).
            then(function (d) {
                scope.view.history = JSON.parse(d.data);
                
            });
        };
        scope.getUserInfo = function (name) {

            http.requestFormSimpleData("/Account/UserInfo", { name: name }).
            then(function (d) {
                scope.view.userInfo = d.data;
                });
        };
        $(".menu-user").on("click", "a", selectView);
    }
]);
module.controller('order', [
    "$scope", "$rootScope", function (scope, root) {
        scope.obj = {
            count: 0,
            price: 0
        };

        scope.$on("calculate", function () {
            console.log("calc");
            scope.items.totalAmount = scope.items.totalAmount + scope.obj.price * scope.obj.count;
        });
        scope.$watch("obj.count", function () {
            scope.items.totalAmount = 0;
            root.$broadcast("calculate");
        });
    }
]);