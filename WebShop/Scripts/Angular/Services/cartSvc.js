(function () {
    'use strict';
    angular.module("cartApp", [])
        .provider("cartSvc", cartExecute);

    function cartExecute() {

        var cart = [];
        var urlAdd = '';
        var urlCart = '';

        return {
            initUrl: initUrl,
            $get: ["$q","toaster", "httpService", get]
        }
        //-------------------functions $get
        function get($q,toaster, http) {
            
            return {
                cart: cart,
                getCart: getCart,
                add: add
            }
            //-------------------functions $get
            function getCart() {
                var dfd = $q.defer();
                http.getRequest({}, urlCart).then(function (d) {
                    cart = d.data;
                }).finally(end);
                return dfd.promise;

                function end() {
                    dfd.resolve(cart);
                };
            };
            function add(item) {
                item.isActive = false;
                http.postRequest({}, item, urlAdd).then(function (d) {
                    toaster.pop('success', '', d.data);

                    if (!contains(cart, item))
                        cart.push(item);

                }, function (d) {
                    console.log(d.data);
                    toaster.pop('error', '', d.data);
                }).finally(function () {
                    item.isActive = true;
                });

            };


        }
        //-------------------functions for provider
        function initUrl(pathToAdd, pathToCart) {
            urlAdd = pathToAdd;
            urlCart = pathToCart;
        };
        //-------------------functions for additional
        function contains(arr, item) {
            return arr.filter(function (elem) {
                return elem.goodId == item.goodId &&
                    elem.sizeId == item.sizeId &&
                    elem.colorId == item.colorId;
            }).length > 0;
        }

    }
})();