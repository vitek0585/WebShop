//var app = angular.module("detailApp", ["httpApp", "notifyApp", 'slick', 'imageZoomApp', 'lazyLoadApp']);

var m = angular.module("globalApp");
var arr = ["httpApp", "notifyApp", 'slick', 'imageZoomApp', 'angular.filter', 'selectBotstrApp'];
Array.prototype.push.apply(m.requires, arr);

m.controller("ctrlDetail", [
    "$scope", "$timeout", "httpService", "notifyWindow",
    function (scope, timeout, http, notifyWindow) {
        var url = "/Cart/Add";
        var contains = function (arr, id, sizeId, colorId) {
            return arr.filter(function (elem) {
                return elem.goodId == id &&
                    elem.size.sizeId == sizeId &&
                    elem.color.colorId == colorId;
            }).length > 0;
        }
        scope.item = {};
        scope.randomItem = {};
        scope.current = {};
        scope.initCurrent = function (item) {
            scope.current.colorId = item.colorId;
            scope.current.sizeId = item.sizeId;


        };
        scope.initialize = function (item) {
            scope.item = item;

            if (angular.isDefined(scope.item.types) && scope.item.types.length > 0) {
                scope.current.colorId = scope.item.types[0].colorId;
                scope.current.sizeId = scope.item.types[0].sizeId;
            }
            scope.current.goodId = scope.item.goodId;
            //http.getRequest({ id: item }, "/api/Good/GetGood").then(function (d) {

            //    scope.item = JSON.parse(d.data);
            //    if (angular.isDefined(scope.item.types) && scope.item.types.length>0) {
            //        scope.current.colorId = scope.item.types[0].colorId;
            //        scope.current.sizeId = scope.item.types[0].sizeId;
            //    }
            //    scope.current.goodId = scope.item.goodId;
            //});
            http.getRequest({count:10}, "/api/Good/RandomGood").then(function (d) {

                scope.randomItem = d.data;

            });
            //http.postRequest({}, "/Cart/UserCart").then(function (d) {

            //    scope.cart = d.data;
            //});
        };
        scope.add = function (count) {
            var item = scope.current;
            var tmp = {};
            angular.copy(item, tmp);
            tmp.countGood = count;
            http.postRequest(tmp, url).then(function (d) {
                notifyWindow.notifySuccess(d.statusText, "notifyWin");

                if (!contains(scope.cart, tmp.goodId, tmp.sizeId, tmp.colorId)) {
                    tmp.size = { sizeId: tmp.sizeId };
                    tmp.color = { colorId: tmp.colorId };
                    scope.cart.push(tmp);
                    
                }

            }, function (d) {
                notifyWindow.notifyError(d.statusText, "notifyWin");
            }).finally(function () {
                item.isActive = true;
            });
        }

    }]);