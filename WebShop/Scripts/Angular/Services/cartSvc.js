(function () {
    'use strict';
    angular.module("cartApp", [])
        .provider("cartSvc", cartExecute);

    function cartExecute() {

        var cart = [];
        var urlAdd = '';
        var urlCart = '';
        var urlUpdate = '';
        var urlGetTypes = '';
        return {
            initUrl: initUrl,
            $get: ["$q", "toaster", "httpService", get]
        }
        //-------------------functions $get
        function get($q, toaster, http) {

            return {
                cart: cart,
                getCart: getCart,
                add: add,
                update: update,
                getDetails: getDetails
            }
            //-------------------functions $get
            function getCart() {
                var dfd = $q.defer();

                if (urlCart == undefined)
                    end();
                else remoteCart();

                return dfd.promise;
                //func
                function remoteCart() {
                    try {

                        http.getRequest({}, urlCart).then(function (d) {
                            cart = d.data;
                        }).finally(end);

                    } catch (e) {
                        end();
                    }
                }
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
            function update(item) {
                var dfd = $q.defer();
                error({});


                return dfd.promise;
                function error(d) {
                    dfd.reject(d);
                    toaster.pop('error', '', d.data);
                }
            }

            function getDetails(id) {
                var dfd = $q.defer();
                http.getByCache({ id: id }, urlGetTypes)
                    .then(function (d) {
                        dfd.resolve(d.data);
                    }).catch(error);
                return dfd.promise;

                function error(d) {
                    dfd.reject(d);
                    toaster.pop('error', '', d.data);
                }
            }

        }
        //-------------------functions for provider
        function initUrl(pathToAdd, pathToCart, pathUpdate, pathGetTypes) {
            if (pathToAdd != null)
                urlAdd = pathToAdd;
            if (pathToCart != null)
                urlCart = pathToCart;
            if (pathUpdate != null)
                urlUpdate = pathUpdate;
            if (pathGetTypes != null)
                urlGetTypes = pathGetTypes;
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