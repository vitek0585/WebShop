//var prod = angular.module("prodApp", ["httpApp", "notifyApp", 'slick', 'lazyLoadApp']);

var prod = angular.module("globalApp");
var arr = ["httpApp","httpRouteApp", "pagingApp",  "notifyApp", 'slick'];
Array.prototype.push.apply(prod.requires, arr);

prod.controller("prodCtrl", ["$scope", "$timeout", "httpService", "notifyWindow",
    function (scope, timeout, http, notifyWindow) {
        var url = "/Cart/Add";
        var contains = function (arr, id) {
            return arr.filter(function (elem) { return elem.goodId == id }).length > 0;
        }
     
        scope.cart = [];
        //scope.filter = {
        //    name: "",
        //    priceFrom: 0,
        //    priceTo: Number.MAX_VALUE,
        //    isUpdate: true

        //}
        //lazyLoad.setupElementLazy("/api/Good/GetByPage", "loading", "up", scope.items, 300);

        var getCart = function () {
            http.postRequest({}, "/Cart/UserCart").then(function (d) {
                scope.cart = d.data;
            });
        };
        scope.updateSlick = function () {
            scope.filter.isUpdate = false;
            timeout(function () { scope.filter.isUpdate = true; });
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
        }
        //getCart();

    }
]);

//paging goods
prod.controller("pageCtrl", ["$scope", "routeService", "httpService", "$timeout",  function (s, route, http, timeout) {
    s.items = [];
    s.isWait = false;
    s.info = {

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
        http.getRequest({ page: page, category: s.category }, "/api/Good/GetByPage").then(function (d) {
            timeout(function () {
                s.info.totalPages = d.data.totalPagesCount;
                s.items.length=0;
                s.isWait = false;
                s.items = d.data.items;
              
            }, 0, true);
        });
    };

}
]);