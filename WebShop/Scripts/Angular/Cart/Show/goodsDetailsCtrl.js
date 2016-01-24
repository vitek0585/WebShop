(function () {
    'use strict';
    angular.module('globalApp')
        .controller('goodsDetailsCtrl', goodsDetailsCtrl);
    angular.module('globalApp').filter('filterBy', filterBy);
    angular.module('globalApp').filter('correctBy', correctBy);
    angular.module('globalApp').injectRequires(['selectBotstrApp']);

    goodsDetailsCtrl.$inject = ['cartSvc', '$filter'];
    filterBy.$inject = ['$filter'];
    correctBy.$inject = ['$filter'];

    function goodsDetailsCtrl(cart, filter) {
        var VIEW_DET = 'det';
        var VIEW_DET_CORR = 'det-corr';
        var tmpData = {};
        var vm = this;
        vm.isWaiter = false;
        vm.details = [];
        vm.current = {};
        vm.view = VIEW_DET;

        vm.initCurrent = initCurrent;
        vm.getDetails = getDetails;
        vm.saveDetail = save;
        vm.cancel = cancel;
        vm.select = selectPicker;
        vm.isCorrectCount = isCorrectCount;

        //functions
        function initCurrent(item) {
            vm.current = item;
        }

        function isCorrectCount(count) {
            return vm.current[count] > filter('filterBy')(vm.details, vm.current, count);
        }
        function getDetails(id) {
            vm.isWaiter = true;

            cart.getDetails(id).then(function (d) {
                vm.details = d;
                if (vm.details[0]) {

                    vm.view = VIEW_DET_CORR;
                    tmpData = clone(vm.current);
                }
            }).finally(dispose);
        }

        function save(size, color) {
            vm.isWaiter = true;
            vm.current[size] = filter('filterBy')(vm.details, vm.current, size);
            vm.current[color] = filter('filterBy')(vm.details, vm.current, color);
            vm.view = VIEW_DET;
            cart.update(vm.current).then(function (d) {
            }).finally(dispose);
        }

        function selectPicker(item) {
            for (var i = 1; i < arguments.length; i++) {
                vm.current[arguments[i]] = item[arguments[i]];
            }

        }
        function cancel() {
            vm.view = VIEW_DET;
            vm.current = clone(tmpData);
        }

        function clone(item) {
            return JSON.parse(JSON.stringify(item));
        }
        function dispose(d) {
            vm.isWaiter = false;
        }

    }

    function filterBy($filter) {
        return function (data, current, name) {

            var w = $filter('where')(data, {
                sizeId: current.sizeId,
                colorId: current.colorId,
                goodId: current.goodId
            });

            var f = $filter('first')(w);

            return f[name];
        }
    }
    function correctBy($filter) {
        return function (data, current, name) {
            
            return (current[name] > $filter('filterBy')(data, current, name));
        }
    }
})();