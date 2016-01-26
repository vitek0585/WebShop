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
        var urlRemove = '';
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
                getDetails: getDetails,
                remove: remove
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

                }).catch(error).finally(function () {
                    item.isActive = true;
                });
                function error(d) {
                    notify(d);
                }

            };
            function update(item) {
                var dfd = $q.defer();

                http.postRequest({}, item, urlUpdate).then(function (d) {
                    dfd.resolve(d.data);
                }).catch(error);

                return dfd.promise;
                function error(d) {
                    dfd.reject(d);
                    notify(d);
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
                    notify(d);
                }
            }
            function remove(id) {
                var dfd = $q.defer();

                http.postRequest({ id: id }, {}, urlRemove).then(function (d) {
                    dfd.resolve(d.data);
                }).catch(error);

                return dfd.promise;
                function error(d) {
                    dfd.reject(d);
                    notify(d);
                }

            }
            function notify(d) {
                if (d === undefined)
                    return;
                if (typeof (d.data) == 'object') {
                    d.data = d.data.join(' ');
              
                }
                toaster.pop('error', '', d.data);
            }

        }
        //-------------------functions for provider
        function initUrl(pathToAdd, pathToCart, pathUpdate, pathGetTypes, pathRemove) {
            if (pathToAdd != null)
                urlAdd = pathToAdd;
            if (pathToCart != null || pathToCart === undefined)
                urlCart = pathToCart;
            if (pathUpdate != null)
                urlUpdate = pathUpdate;
            if (pathGetTypes != null)
                urlGetTypes = pathGetTypes;
            if (pathRemove != null)
                urlRemove = pathRemove;
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