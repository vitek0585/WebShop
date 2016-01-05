var reg = angular.module("regApp", ["httpApp", "confirmApp", "spinnerApp", "serializeApp"]);
reg.controller("regCtrl", [
    "$scope", "httpService", "formToObject", function (scope, http, ser) {

        
        scope.responseHandler = {
            message: '',
            isBusy: false,
            isSuccededRegister:false
        }

        var clear = function () {
            scope.responseHandler.message = '';
        };
        var error = function (d) {
          
            var data = d.data;
          
            clear();
            var newLine = '\n';
            for (var i=0;i<data.length;i++) {
                for (var j = 0; j < data[i].length; j++)
                    scope.responseHandler.message += data[i][j].errorMessage + newLine;
            }
        };
        scope.submit = function () {
            scope.responseHandler.isBusy = true;
            var form = ser.serializeObject(registerForm);
            var url = "/Account/Register";
            http.formRequest(url, form).then(function (d) {
                    scope.responseHandler.isSuccededRegister = true;
                }, error)
                .finally(function() {
                    scope.responseHandler.isBusy = false;
                });
        };
    }
]);