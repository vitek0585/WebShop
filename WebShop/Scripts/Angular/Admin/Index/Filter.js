angular.module("adminApp")
    .filter("sizeFilter",["$filter", function(filter) {

        return function (data) {
       
            var value = data / 1024;
         
            return filter("number")(value, 2) + " KB";
        }

    }]);
