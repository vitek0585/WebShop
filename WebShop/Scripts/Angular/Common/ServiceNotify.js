//<div id="notifyWin" class="success-notify notify"></div>
angular.module("notifyApp", []).
    factory('notifyWindow', function () {
        var prevCss;
        var start = function (msg, nameNotify, css) {
            var elem = $("#" + nameNotify);
            elem.stop(true, true);
            if (prevCss != null)
                elem.removeClass(prevCss);

            elem.addClass(css);
            elem.text(msg);
            elem.animate({ height: '3em' }, 400).delay(5000).animate({ height: '0' }, 400, function () {
                $(this).removeClass(css);
            });
            prevCss = css;
        }

        return {
            notifySuccess: function (msg, nameNotify) {

                start(msg, nameNotify, 'success-notify');
            },
            notifyError: function (msg, nameNotify) {
                start(msg, nameNotify, 'error-notify');
            }

        }
    });

//.notify {
//    position: fixed;
//    top: 3em;
//    right: 5em;
//    width: 20em;
//    height: 0;
//    display: flex;
//    justify-content: center;
//    align-items: center;
//    z-index: 1000;
//    color: whitesmoke;
//    overflow: hidden;
//}

//.success-notify {
//    background: rgb(97, 143, 99);
//}

//.error-notify {
//    background: #db7093;
//}