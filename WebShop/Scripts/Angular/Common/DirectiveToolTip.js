var app = angular.module("ToolTipApp", ["httpApp"]);
app.factory("slickService", function () {

    return {
        setup: function() {
            $('.slider-for:not(.slick-slider)').slick({
                slidesToShow: 1,
                slidesToScroll: 1,
                arrows: false,
                fade: true,
                asNavFor: '.slider'
            });
            $('.slider:not(.slick-slider)').slick({
                centerMode: true, //главная картинка переносится в центр и тогда по краям видно остальные картинки
                dots: true, //ставит точки для выбора фото
                centerPadding: '50px', //отступы, если 0 то поместяться ровно сколько указано slidesToShow (по умолчанию 50px)
                //vertical: true,
                //slidesToScroll: 1,
                slidesToShow: 3,
                cssEase: 'ease',
                focusOnSelect: true,
                //  variableWidth: true,//ширина берется по элементу (картинки будут разной ширины)
                asNavFor: '.slider-for',
                // adaptiveWidth: true,
                arrows: false,
            });
        }
    }
});
app.directive("toolTipDirect", ["$timeout", "httpService", "slickService", function (timeout, http,slick) {
        var timer;
        var init = function (d) {
            console.log(d.data);
        };
        
        var onLeave = function (e) {
           timeout.cancel(timer);
        }
        return {
            link: function (scope, element, attr) {
                
                var config = {
                    params: { id: scope.id } 
                };
                var onOver = function (e) {
                  
                    timeout.cancel(timer);
                    timer = timeout(function () {
                        http.requestWithConfig(undefined, "/Home/GetDescription", config).
                        then(function (d) {
                            element.append(angular.element(d.data));
                            $(element).parent().off("mouseenter mouseleave");
                                slick.setup();
                            });
                    }, 1000, true);
                }
                $(element).parent().on("mouseenter", onOver);
                $(element).parent().on("mouseleave", onLeave);

            },
            
            restrict: "A",
            scope:{
                id: "=eId"
            }
        }


    }]);

