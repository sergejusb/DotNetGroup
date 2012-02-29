(function (global, undefined) {
    var setActiveMenu = function () {
        var type = $.query.load(window.location.hash).get("type");
        var nav = $(".nav");
        var li = nav.find("a[data-item-type=" + type + "]");
        nav.find("li.active").removeClass("active");
        (li.length ? li.parent() : nav.find("li:first")).addClass("active");
    };
    var load = function (baseUrl, callback) {
        var url = Api.getUrl(baseUrl);
        Items.get(url, function (data) {
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
                
                window.location.hash = decodeURIComponent(query.toString());
            });

            $(window).trigger("hashchange");
        },
        bindLoadMore: function () {
            $(".load-more a").live("click", function (e) {
                e.preventDefault();
                var from = $.query.load(window.location.hash).get("from");
                if (!from) from = $("tr.item:last .item-date").text();
                from = from ? Date.parse(from) : new Date();
                from = from.add(-7).days();
                var query = Api.buildQuery(/*type*/null, $.format.date(from, "yyyy-MM-dd"));

                window.location.hash = decodeURIComponent(query.toString());
            });
        }
    };
} (this));