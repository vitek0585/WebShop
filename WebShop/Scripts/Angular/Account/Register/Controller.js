var global = angular.module("globalApp");
global.injectRequires(["confirmApp", "spinnerApp", "serializeApp"]);

global.controller("registerModalCtrl", ["$scope", '$fancyModal', function (scope, modal) {

    scope.openRegisterModal = function () {
        modal.open({
            template: angular.element(document.querySelector("#registerTmpl")).html(),
            //openingClass: 'animated zoomIn',
            //closingClass: 'animated hinge',
            //openingOverlayClass: 'animated fadeIn',
            //closingOverlayClass: 'animated fadeOut',
            //plain: true,
            //closeByEscape: false,
            controller: 'regCtrl'
        });
    }
}]);
global.controller("regCtrl", ["$scope", "httpService", "formToObject", function (scope, http, ser) {


    scope.responseHandler = {
        errorMessages: [],
        succesMessage: undefined,
        isBusy: false,
        isSuccededRegister: false
    }

    scope.submit = function () {
        scope.responseHandler.isBusy = true;
        var form = ser.serializeObject(registerForm);
        var url = "/Account/Register";
        http.formRequest(url, form).then(function (d) {
            scope.responseHandler.isSuccededRegister = true;
            scope.responseHandler.succesMessage = d.data;
        }, function (d) {
            scope.responseHandler.errorMessages = d.data;
        })
            .finally(function () {
                scope.responseHandler.isBusy = false;
            });
    };
}
]);