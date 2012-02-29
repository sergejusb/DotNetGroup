(function (global, undefined) {
    var setActiveMenu = function () {
        var type = $.query.load(window.location.hash).get("type");
        var nav = $(".nav");
        var li = nav.find("a[data-item-type=" + type + "]");
        nav.find("li.active").removeClass("active");
        (li.length ? li.parent() : nav.find("li:first")).addClass("active");
    };
    var load = function(baseUrl, callback) {
        var url = Api.getUrl(baseUrl);
        Items.get(url, function(data) {
            callback(data);
            setActiveMenu();
        });
    };
    global.Nav = {
        bindNavigation: function (baseUrl, callback) {
            $(window).bind("hashchange", function () {
                load(baseUrl, callback);
            });
        },
        bindMenu: function () {
            $(".nav:first a").click(function (e) {
                e.preventDefault();
                var type = $(this).data("item-type");
                var query = Api.buildQuery(type);
                if (!type) query = query.remove("type");
                window.location.hash = query.toString();
            });

            $(window).trigger("hashchange");
        },
        loadMore: function (baseUrl, callback) {
            debugger;
            var oldest = $("tr.item:last .item-date").text();
            var to = oldest ? new Date(oldest) : new Date();
            var from = to.setMonth(to.getMonth() - 1);
            to = $.format.date(to, "yyyy-MM-dd hh:mm:ss");
            from = $.format.date(from, "yyyy-MM-dd hh:mm:ss");
            var limit = 10;
            var query = Api.buildQuery(null, from, to, limit);
            var url = Api.getUrl(baseUrl, query);

            Items.get(url, function (data) {
                callback(data);
            });
        }
    };
} (this));