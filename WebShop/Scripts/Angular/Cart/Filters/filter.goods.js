(function() {
    'use strict';

    angular.module('globalApp').filter('filterBy', filterBy);
    angular.module('globalApp').filter('correctBy', correctBy);
    angular.module('globalApp').filter('correctByCount', correctByCount);
    filterBy.$inject = ['$filter'];
    correctBy.$inject = ['$filter'];
    correctByCount.$inject = ['$filter'];
    function filterBy($filter) {
        return function (data, current, name) {

            var w = $filter('where')(data, {
                sizeId: current.sizeId,
                colorId: current.colorId,
               // goodId: current.goodId
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
    function correctByCount($filter) {
        return function (data, current, name, count) {

            return (count > $filter('filterBy')(data, current, name));
        }
    }
})();