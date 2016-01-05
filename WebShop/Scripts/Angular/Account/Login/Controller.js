//angular.module("loginApp", ["httpApp", "spinnerApp","serializeApp"]).
var m = angular.module("globalApp");
var arr = ["httpApp", "spinnerApp", "serializeApp"];
Array.prototype.push.apply(m.requires, arr);
m.controller("loginCtrl", ["$scope", "httpService", "formToObject", function (scope, http, ser) {
        scope.responseHandler = {
            isBusy: false,
            message:'',
            returnUrl: angular.isDefined(document.referrer)?document.referrer : ''
        };
        var clear = function () {
            scope.responseHandler.message = '';
        };
        var error = function (d) {

            var data = d.data;
            clear();
            var newLine = '\n';
            for (var i = 0; i < data.length; i++) {
                for (var j = 0; j < data[i].length; j++)
                    scope.responseHandler.message += data[i][j].errorMessage + newLine;
            }
        };
        scope.submit = function () {
            scope.responseHandler.isBusy = true;
            var form = ser.serializeObject(loginForm);
            form.returnUrl = scope.responseHandler.returnUrl;
            var url = "/Account/Login";
            http.formRequest(url, form).then(function (d) {
                    document.location = d.data.url;
                }, error)
                .finally(function () {
                    scope.responseHandler.isBusy = false;
                });
        };
    }]);