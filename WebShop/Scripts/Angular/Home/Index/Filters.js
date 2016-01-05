angular.module("filterApp", []).
    filter("filterProduct", function() {

        return function(data,name) {

            var result = data.filter(function(e) {

                if (name) {
                    var isMatch = e.goodName.toLowerCase().match(name, "i");
                    return isMatch!=null;
                }

                return true;
            });
            

            return result;

        }

    });