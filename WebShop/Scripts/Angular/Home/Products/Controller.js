//var prod = angular.module("prodApp", ["httpApp", "notifyApp", 'slick', 'lazyLoadApp']);

var prod = angular.module("globalApp");
var arr = ["httpApp", "httpRouteApp", "pagingApp", "notifyApp", 'slick', 'ngAnimate', 'ui.bootstrap', 'angularRangeSlider'
, 'azSuggestBox', 'ngTooltips'];
Array.prototype.push.apply(prod.requires, arr);

prod.controller("prodCtrl", ["$scope", "$timeout", "httpService", "notifyWindow",
    function (scope, timeout, http, notifyWindow) {
        var url = "/Cart/Add";
        var contains = function (arr, id) {
            return arr.filter(function (elem) { return elem.goodId == id }).length > 0;
        }

        scope.cart = [];
        scope.colors = [];
        scope.sizes = [];
        scope.filter = {
            colorsSelect: [],
            sizesSelect: [],
            sortBySelect: [],
            priceMin: null,
            priceMax: null
        };

        var getCart = function () {
            http.postRequest({}, "/Cart/UserCart").then(function (d) {
                scope.cart = d.data;
            });
        };
        scope.initFilterData = function (min, max, colors, sizes) {
            scope.colors = colors;
            scope.sizes = sizes;
            scope.filter.priceMin = min;
            scope.filter.priceMax = max;
        }
        scope.add = function (item) {

            http.postRequest(item, url).then(function (d) {
                notifyWindow.notifySuccess(d.statusText, "notifyWin");

                if (!contains(scope.cart, item.goodId))
                    scope.cart.push(item);

            }, function (d) {
                notifyWindow.notifyError(d.statusText, "notifyWin");
            }).finally(function () {
                item.isActive = true;
            });
        };
        scope.filterAccept = function () {

            scope.$broadcast("acceptFilterEvent",
            {
                c: scope.filter.colorsSelect.map(function (e) { return e.id; }),
                s: scope.filter.sizesSelect.map(function (e) { return e.id; }),
                min: scope.filter.priceMin,
                max: scope.filter.priceMax

            });
        }
        scope.$watch('filter.sortBySelect', function () {

            scope.$broadcast("filterSortByEvent",
                {
                    sortBy: scope.filter.sortBySelect.map(function (e) { return e.sort; })
                });

        }, true);
        //getCart();

    }
]);
var actionExclusiveWidget = function (scope,http) {
    scope.exclusiveGoods=[];
    http.getRequest({ count: 10 }, "/api/Good/RandomGood").then(function (d) {
        scope.exclusiveGoods = d.data;
    });
}
prod.controller("exclusiveCtrl", ["$scope", 'httpService', actionExclusiveWidget]);
//paging goods
prod.controller("pageCtrl", ["$scope", "routeService", "httpService", "$timeout", function (s, route, http, timeout) {
    s.items = [];
    //filter
    var colorsSelect = [];
    var sizesSelect = [];
    var sortBySelect = [];
    var price = {
        min: '',
        max: ''
    };
    //wait load
    s.isWait = false;
    //for paging
    s.info = {
        refresh: function () { },
        currentPage: undefined,
        totalPages: 0,
        css: 'btn btn-pagging',
        cssActive: 'btn btn-primary active',
        rightPrev: '>>',
        leftPrev: '<<',
    };


    var path = location.href.split('/').filter(function (e, i) { if (i > 2 && i < 7) return e; }).join('/');

    s.clickPage = function (page) {

        if (!angular.isDefined(s.info.currentPage)) {
            s.info.currentPage = page;
        }
        var url = "/" + path + "/" + page;

        route.route(url, false);
        s.isWait = true;


        http.getRequest(
            {
                page: page,
                category: s.category,
                colors: colorsSelect.toString(),
                sizes: sizesSelect.toString(),
                sort: sortBySelect.toString(),
                priceMin: price.min,
                priceMax: price.max
            }, "/api/Good/GetByPage").then(function (d) {

                s.items = d.data.items;
                s.info.totalPages = d.data.totalPagesCount;
                s.isWait = false;


            }, function () {

                s.info.totalPages = 0;
                s.items.length = 0;
                s.isWait = false;

            });
    };
    var refreshPage = function () {

        if (!s.isWait) {
            s.info.currentPage = 1;
            s.clickPage(s.info.currentPage);
            s.info.refresh();
        }
    };
    s.$on("filterSortByEvent", function (event, args) {
        sortBySelect = args.sortBy;

        refreshPage();
    });

    s.$on("acceptFilterEvent", function (event, args) {
        colorsSelect = args.c;
        sizesSelect = args.s;
        price.min = args.min;
        price.max = args.max;

        refreshPage();
    });

}
]);
