//////////////////////////////////////////////
// Function
/////////////////////////////////////////////
function backLoginPage() {
    window.location.href = "/Home/Login";
}

function showError(e) {
    var systemNotification = $("#systemNotification").kendoNotification({
        stacking: "down",
        show: onSystemNotificationShow,
        button: true,
        autoHideAfter: 5000
        //width:500
    }).data("kendoNotification");
    var message = e.responseJSON;
    if (message) {
        systemNotification.show(message, "error");
        console.log("error json");
        console.log(message);
        if (e.status == 404 || message.indexOf("Login") >= 0 || message.indexOf("登入") >= 0)
            backLoginPage();
        if (message.indexOf("Execution Timeout Expired.  The timeout period elapsed prior to completion of the operation or the server is not responding.") >= 0) {
            alert("Oops....篩選結果筆數過多，請減少日期範圍或增加條件再搜尋");
        }
        if (message.indexOf("執行逾時到期。在作業完成之前超過逾時等待的時間，或是伺服器未回應") >= 0) {
            alert("Oops....篩選結果筆數過多，請減少日期範圍或增加條件再搜尋");
        }
    } else if (e.responseText) {
        console.log("error responseText");
        console.log(e);
        systemNotification.show(e.responseText, "error");
    } else if (typeof e === "string") {
        console.log("error string");
        console.log(e);
        systemNotification.show(e, "error");
    }
}

function showMessage(msg) {
    var systemNotification = $("#systemNotification").kendoNotification({
        stacking: "down",
        show: onSystemNotificationShow,
        button: true,
        autoHideAfter: 5000
        //width:500
    }).data("kendoNotification");
    systemNotification.show(" " + msg, "success");
}

function onSystemNotificationShow(e) {
    if (!$("." + e.sender._guid)[1]) {
        var element = e.element.parent(),
            eWidth = element.width(),
            eHeight = element.height(),
            wWidth = $(window).width(),
            wHeight = $(window).height(),
            newTop, newLeft;

        newLeft = Math.floor(wWidth / 2 - eWidth / 2);
        newTop = Math.floor(wHeight / 2 - eHeight / 2);

        e.element.parent().css({
            top: newTop,
            left: newLeft,
            zIndex: 22222
        });
    }
}

function showLoading(element, msg) {
    kendo.ui.progress.messages = {
        loading: msg
    };
    kendo.ui.progress(element, true);
    $(".k-loading-image").html('<div class="loader"><div class="dot"></div> <div class="dot"></div> <div class="dot"></div><div class="dot"></div></div>');
}

function hideLoading(element) {
    kendo.ui.progress(element, false);
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}

function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

// IE not support find function
// custom pollyfill
if (!Array.prototype.find) {
    Object.defineProperty(Array.prototype, 'find', {
        value: function (predicate) {
            // 1. Let O be ? ToObject(this value).
            if (this == null) {
                throw new TypeError('"this" is null or not defined');
            }

            var o = Object(this);

            // 2. Let len be ? ToLength(? Get(O, "length")).
            var len = o.length >>> 0;

            // 3. If IsCallable(predicate) is false, throw a TypeError exception.
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }

            // 4. If thisArg was supplied, let T be thisArg; else let T be undefined.
            var thisArg = arguments[1];

            // 5. Let k be 0.
            var k = 0;

            // 6. Repeat, while k < len
            while (k < len) {
                // a. Let Pk be ! ToString(k).
                // b. Let kValue be ? Get(O, Pk).
                // c. Let testResult be ToBoolean(? Call(predicate, T, « kValue, k, O »)).
                // d. If testResult is true, return kValue.
                var kValue = o[k];
                if (predicate.call(thisArg, kValue, k, o)) {
                    return kValue;
                }
                // e. Increase k by 1.
                k++;
            }

            // 7. Return undefined.
            return undefined;
        },
        configurable: true,
        writable: true
    });
}



//this will expand all the column sizes within kendo grid if
//  after autofit, there is empty space left
kendo.ui.Grid.fn.expandToFit = function () {
    if (this.thead == null)
        return;

    var $gridHeaderTable = this.thead.closest('table');

    var gridDataWidth = $gridHeaderTable.width();

    var gridWrapperWidth = $gridHeaderTable.closest('.k-grid-header-wrap').innerWidth();

    // Since this is called after column auto-fit, reducing size

    // of columns would overflow data.

    if (gridDataWidth >= gridWrapperWidth) {

        return;

    }


    var $headerCols = $gridHeaderTable.find('colgroup > col');
    var $tableCols = this.table.find('colgroup > col');
    var sizeFactor = (gridWrapperWidth / gridDataWidth);

    $headerCols.add($tableCols).not('.k-group-col').each(function () {
        var currentWidth = $(this).width() - 2;
        var newWidth = (currentWidth * sizeFactor);

        $(this).css({
            width: newWidth
        });

    });

    var footerCols = this.footer.find('table').find('colgroup > col');
    if (footerCols.length > 0) {
        footerCols.each(function () {
            var currentWidth = $(this).width() - 2;
            var newWidth = (currentWidth * sizeFactor);

            $(this).css({
                width: newWidth
            });
        });
    }

    this.addLastColumnRightBorder();

}//expandToFit
kendo.ui.Grid.fn.addLastColumnRightBorder = function () {
    var last_column_field;
    $.each(this.columns, function (i, v) {
        if (v.hidden != true)
            last_column_field = v.field;
    });

    // don't delete this console log
    // 以便以後查詢此處加上css
    console.log("add last_column" + last_column_field + "border-right");
    this.thead.find("th[data-field='" + last_column_field + "']").css("border-right", " 1px solid rgb(193, 193, 193)");
}

// customize the _show method to call options.beforeShow 
// to allow preventing the tooltip from being shown..
kendo.ui.Tooltip.fn._show = function (show) {
    return function (target) {
        var e = {
            sender: this,
            target: target,
            preventDefault: function () {
                this.isDefaultPrevented = true;
            }
        };

        if (typeof this.options.beforeShow === "function") {
            this.options.beforeShow.call(this, e);
        }
        if (!e.isDefaultPrevented) {
            // only show the tooltip if preventDefault() wasn't called..
            show.call(this, target);
        }
    };
}(kendo.ui.Tooltip.fn._show);


function getTextFromKendoMultiSelect(name, textFieldName) {
    var str = [];
    var multiSelect = $("#" + name).data("kendoMultiSelect");
    $.each(multiSelect.dataItems(), function (i, d) {
        str.push(d[textFieldName]);
    });
    return str.join(", ");
}
