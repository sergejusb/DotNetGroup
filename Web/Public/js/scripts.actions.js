(function (global, undefined) {
    var toItemTypeEnum = function (itemType) {
        switch (itemType) {
            case 1:
                return "rss";
            case 2:
                return "twitter";
            default:
                return "";
        }
    };
    var filter = function (items, predicate) {
        if (!predicate) return items;
        var filteredItems = [];
        for (var i = 0; i < items.length; i++) {
            if (predicate(items[i])) {
                filteredItems.push(items[i]);
            }
        }
        return filteredItems;
    };
    global.Model = {
        getVisibleItems: function (items, itemType) {
            if (!itemType) return items;
            return filter(items, function (item) {
                return toItemTypeEnum(item.ItemType) == itemType;
            });
        }
    };
    global.Model.Items = {};

    global.View = {
        setRendering: function (render) {
            $(window).on("hashchange", function () {
                render(Model.getVisibleItems(Model.Items, View.getActiveItem()));
            });
        },
        getActiveItem: function () {
            return $.query.load(window.location.hash).get("type");
        },
        setActiveMenu: function (menuItem) {
            var nav = $(".nav");
            var li = nav.find("a[data-menu-item=" + menuItem + "]").parent();
            nav.find("li.active").removeClass("active");
            (li.length ? li : nav.find("li:first")).addClass("active");

            var query = $.query.load(window.location.hash);
            query = menuItem ? query.set("type", menuItem) : query.remove("type");
            window.location.hash = decodeURIComponent(query.toString());
        },
        bindMenuClick: function () {
            $(".nav:first a").click(function (e) {
                e.preventDefault();

                var menuItem = $(this).data("menu-item");
                View.setActiveMenu(menuItem);
            });
        },
        bindLoadMore: function (baseUrl) {
            $(".load-more a").live("click", function (e) {
                e.preventDefault();
                var to = $.query.load(window.location.hash).get("from");
                if (!to) to = $("tr.item:last .item-date").text();
                to = to ? Date.parse(to) : new Date();
                var from = to.clone().add(-7).days();

                var query = Api.buildQuery(/*type*/null, $.format.date(from, "yyyy-MM-dd"));
                var apiQuery = query.set("to", $.format.date(to, "yyyy-MM-dd HH:mm:ss"));
                var url = Api.getUrl(baseUrl, apiQuery);
                Stream.get(url, function (items) {
                    for (var i = 0; i < items.length; i++) {
                        Model.Items.push(items[i]);
                    }
                    window.location.hash = decodeURIComponent(query.toString());
                });
            });
        }
    };
} (this));