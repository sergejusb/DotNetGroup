function SiteModule(window, viewData) {
    var that = this;

    String.prototype.format = function () {
        var formatted = this;
        for (var i = 0; i < arguments.length; i++) {
            var regexp = new RegExp('\\{' + i + '\\}', 'gi');
            formatted = formatted.replace(regexp, arguments[i]);
        }
        return formatted;
    };

    this.Home_Index = function () {
        $(document).on("click", "li.more-items:visible", function () {
            var url = $(this).attr("data-url");
            $(this).remove();
            $.get(url, function (data) {
                $('ul.stream-item-list:visible').append(data);
                $('ul.stream-item-list:visible').listview('refresh');
            });
        });
    };

    $(".js-module").each(function () {
        var module = $(this).attr('data-module');
        try {
            return new that[module](window, viewData);
        } catch (e) {
            throw new Error('Unable to create module "' + module + '"');
        }
    });
}

var _viewData = {};
var _site = null;

$(function () {
    _site = SiteModule(window, _viewData);
});

