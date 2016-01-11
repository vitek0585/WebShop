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
        scope.colors = [{ name: 'red' }, { name: 'blue' }, { name: 'green' }, { name: 'red' }, { name: 'blue' }, { name: 'green' }, { name: 'red' }, { name: 'blue' }, { name: 'green' }];
        scope.sizes = [{ name: '14' }, { name: '24' }, { name: '12' }, { name: '3' }, { name: '4' }, { name: '5' }, { name: '34' }, { name: '5' }, { name: '7' }];

        scope.filter = {
            colorsSelect: [],
            sizesSelect: [],
            sortBySelect: []
        };

        var getCart = function () {
            http.postRequest({}, "/Cart/UserCart").then(function (d) {
                scope.cart = d.data;
            });
        };

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
                c: scope.filter.colorsSelect.map(function (e) { return e.name; }),
                s: scope.filter.sizesSelect.map(function (e) { return e.name; })

            });
        }
        scope.$watch('filter.sortBySelect', function () {

            scope.$broadcast("filterSortByEvent",
                {
                    sortBy: scope.filter.sortBySelect.map(function (e) { return e.name; })
                });

        },true);
        //getCart();

    }
]);

//paging goods
prod.controller("pageCtrl", ["$scope", "routeService", "httpService", "$timeout", function (s, route, http, timeout) {
    s.items = [];
    //filter
    var colorsSelect = [];
    var sizesSelect = [];
    var sortBySelect = [];
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

    s.filterFormatter = function () {
        console.log(colorsSelect);
        console.log(sizesSelect);
        console.log(sortBySelect);
    }
    var path = location.href.split('/').filter(function (e, i) { if (i > 2 && i < 7) return e; }).join('/');

    s.clickPage = function (page) {

        if (!angular.isDefined(s.info.currentPage)) {
            s.info.currentPage = page;
        }
        var url = "/" + path + "/" + page;

        route.route(url, false);
        s.isWait = true;

        s.filterFormatter();

        http.getRequest({ page: page, category: s.category, colors: colorsSelect.toString(), sizes: sizesSelect, sort: sortBySelect },
            "/api/Good/GetByPage").then(function (d) {
            timeout(function () {
                s.info.totalPages = d.data.totalPagesCount;

                s.isWait = false;
                s.items = d.data.items;

            }, 0, true);
        }, function () {
            timeout(function () {
                s.info.totalPages = 0;
                s.items.length = 0;
                s.isWait = false;


            }, 0, true);
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

        refreshPage();
    });

}
]);