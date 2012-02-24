(function (global, undefined) {
    var apiBase = "http://api.dotnetgroup.lt";
    var buildApiUrl = function () {
        var apiQuery = $.query.load(apiBase);
        var type = $.query.get("type");
        if (type) {
            apiQuery.SET("type", type);
        }
        var from = $.query.get("from");
        if (from) {
            apiQuery.SET("from", from);
        }
        var to = $.query.get("to");
        if (to) {
            apiQuery.SET("to", to);
        }
        var limit = $.query.get("limit");
        if (limit) {
            apiQuery.SET("limit", limit);
        }
        apiQuery.SET("callback", "?");

        return apiBase + decodeURIComponent(apiQuery.toString());
    };

    global.Items = {
        setApiUrl: function (apiUrl) {
            apiBase = apiUrl;
        },
        load: function (container) {
            var url = buildApiUrl();
            $.getJSON(url, function (items) {
                var result = {};
                result.Items = items;
                container.html($("#containerTemplate").render(result));
            });
        },
        bind: function (nav, container) {
            nav.find("a").click(function (e) {
                var el = $(this);
                el.data("item-type") ? $.query.SET("type", el.data("item-type")) : $.query.REMOVE("type");
                $("li.active").removeClass();
                el.parent().addClass("active");

                e.preventDefault();
                Items.load(container);
            });
            this.load(container);            
        }
    };
} (this));

(function (global, undefined) {
    global.Templates = {
        registerJsRenderHelpers: function () {
            $.views.registerTags({
                get: function (value) {
                    return value || this.defaultValue;
                },
                toItemType: function (value) {
                    switch (value) {
                        case 1:
                            return "RSS";
                        case 2:
                            return "Twitter";
                        default:
                            return "";
                    }
                },
                toDate: function (value) {
                    var date = new Date(parseInt(value.substr(6)));
                    return $.format.date(date, "yyyy-MM-dd hh:mm:ss");
                }
            });
        }
    };
} (this));