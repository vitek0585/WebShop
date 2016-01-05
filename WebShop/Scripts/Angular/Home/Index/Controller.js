//var m = angular.module("app", ["httpApp", "notifyApp", "filterApp", 'slick', 'imageZoomApp', 'lazyLoadApp']);

var m = angular.module("globalApp");
var arr = ["httpApp", "notifyApp",  'slick'];
Array.prototype.push.apply(m.requires,arr);


m.controller("ctrl", ["$scope", "$timeout", "httpService", "notifyWindow", 
    function (scope, timeout, http, notifyWindow) {


    var url = "/Cart/Add";
    scope.add = function (item) {

        http.postRequest(item, url).then(function (d) {
            notifyWindow.notifySuccess(d.statusText, "notifyWin");
        }, function (d) {
            notifyWindow.notifyError(d.statusText, "notifyWin");
        }).finally(function () {
            item.isActive = true;
        });
    }

    scope.items = [];
    http.getRequest(null, "/api/Good/NewGoods").then(function (d) {
        scope.items = d.data;
    });

    //lazyLoad.setupElement("/api/Good/GetByPage", "loading", "up", scope.items);
    //$("#carousel-base").on("beforeChange", function(slick, currentSlide, nextSlide) {
    //    console.log(slick);
    //    console.log(currentSlide);
    //    console.log(nextSlide);
    //    return false;
    //});


    



}]);
