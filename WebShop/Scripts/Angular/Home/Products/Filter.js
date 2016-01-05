angular.module("globalApp").
filter("countPrice", function () {

    return function (data, from, to) {
        var result = data.filter(function (e) {

            if (e.priceUsd > from && e.priceUsd < to) {
                return true;
            }
            return false;
        });
        return result;

    }
}).
filter("filterProduct", ["$filter", function ($filter) {

    return function (data, name, priceFrom, priceTo) {

        var result = data;
        if (name) {
            result = result.filter(function (e) {
                var isMatch = e.goodName.toLowerCase().match(name, "i");
                return isMatch != null;
            });
        }

        result = $filter("countPrice")(result,priceFrom, priceTo);
     
        return result;

    }

}]);