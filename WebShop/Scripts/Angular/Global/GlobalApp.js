var global = angular.module("globalApp", ["lazyLoadApp"]);
global.value("culture", {
    symbol: ""
});
global.value("kindsRate", {
    ukr: {
        en:"grn.",
        ru:"грн."
    }
});
global.config([
    "$locationProvider", function ($locationProvider) {


    }]);
global.controller("globalCtrl", ["$scope", "culture", 'lazyService', function (scope, culture, lazyLoad) {

    lazyLoad.setupElement("up");
    scope.initSymbol = function (value) {
        culture.symbol = value;
    };
}]);
global.controller('collapseCtrl', [
    "$scope", function (scope) {
        scope.collapse = {
            lang: true,
            curr: true
        };
        scope.collapseLang = function (e) {

            scope.collapse.lang = !scope.collapse.lang;
            e.stopPropagation();

        }
        scope.collapseCurr = function (e) {

            scope.collapse.curr = !scope.collapse.curr;
            e.stopPropagation();

        }
    }
]);
global.filter("currencyExtend", ["$filter", "culture", function (filter, culture) {

    return function (data, num) {

        var res = filter("number")(data, num);
        return res + " " + culture.symbol;
    }
}]);

global.filter("currencyExtendConvertToGrn", ["$filter", "culture", "kindsRate", function (filter, culture, rate) {

    return function (data, curs, num) {
        if (culture.symbol == rate.ukr.en || culture.symbol == rate.ukr.ru)
            data = data * curs;

        var res = filter("number")(data, num);
        return res + " " + culture.symbol;
    }
}]);