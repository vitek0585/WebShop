var app = angular.module('globalApp');
app.injectRequires();
var confirmRegister = function (scope, http, ser, compile, toaster, t) {
    var tmpl = "<div >" +
        "<div class='label-error' ng-repeat='mess in responseHandler.errorMessages'>{{::mess}}</div>" +
        "</div>";

    scope.responseHandler = {
        errorMessages: [],
        succesMessage: undefined,
        isBusy: false,
        isSuccededRegister: false
    }

    var blursArray = [];

    scope.blurs = function (e) {
        scope[e] = false;
        blursArray.push(e);
    }
    scope.submit = function () {
        scope.responseHandler.isBusy = true;
        var form = ser.serializeObject(confirmForm);
        var url = "/Account/ExternalLoginConfirmation";

        http.formRequest(url, form).then(function (d) {
            window.location(d);
        }, function (d) {

            scope.responseHandler.errorMessages = d.data;
            var element = angular.element(tmpl);
            var fn = compile(element);
            fn(scope);
            t(function () {
                toaster.pop('error', '', element[0].innerHTML, 5000, 'trustedHtml');
            }, 0, true);

            angular.forEach(blursArray, function (value) {
                scope[value] = true;
            });
            })
            .finally(function () {
                scope.responseHandler.isBusy = false;
            });
    };
}
app.controller('extConfCtrl', ["$scope", "httpService", "formToObject", "$compile", "toaster", "$timeout", confirmRegister]);