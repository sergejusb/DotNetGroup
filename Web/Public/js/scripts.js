(function (global, undefined) {
    var apiBase = "http://api.dotnetgroup.lt";

    global.Items = {
        setApiUrl: function (apiUrl) {
            apiBase = apiUrl;
        },
        load: function (container) {
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

            var url = apiBase + decodeURIComponent(apiQuery.toString());
            $.getJSON(url, function (items) {
                var result = {};
                result.Items = items;
                container.html($("#containerTemplate").render(result));
            });
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