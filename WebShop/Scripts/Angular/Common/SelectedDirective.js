angular.module("selectedApp", [])
.directive('selectPicker', ['$compile', '$timeout', '$interpolate', selectpickerDirective]);

function selectpickerDirective(compile, timeout, interpolate) {
    var tmp = "<select multiple>"
        + " <option ng-repeat='role in item.roles' ng-selected='item.userRole.indexOf(role.id)>-1' value='{{role.name}}'>{{role.name}}</option> " +
        "</select>";

    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
           
            scope.$watch('item',function refresh() {
                var elem = angular.element(tmp);
                
                compile(elem)(scope);
                element = $(element);
                element.css('opacity',0);
                element.html(compile(elem)(scope));
                //$(element).hide(0);
                timeout(function () {
                    $(elem).selectpicker()
                        .change(function (e) {
                            scope.select = $(elem).val();
                            scope.$apply();
                        });
                    scope.select = $(elem).val();
                });
                $(element).animate({ opacity: '1' }, 200);
                
            });

        },
        scope: {
            item: "=bind",
            select: "="

        }
    };
}