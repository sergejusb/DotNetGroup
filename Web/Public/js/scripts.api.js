(function (global, undefined) {
    var apiUrl = "http://api.dotnetgroup.lt";
    global.Api = {
        buildQuery: function (type, from, to, limit) {
            var query = $.query.load(window.location.hash);
            if (!type) type = query.get("type");
            if (!from) from = query.get("from");
            if (!to) to = query.get("to");
            if (!limit) limit = query.get("limit");
            query = type ? query.set("type", type) : query.remove("type");
            query = from ? query.set("from", from) : query.remove("from");
            query = to ? query.set("to", to) : query.remove("to");
            query = limit ? query.set("limit", limit) : query.remove("limit");
            return query;
        },
        getUrl: function (baseUrl, query) {
            if (!baseUrl) baseUrl = apiUrl;
            if (!query) query = this.buildQuery();
            query = query.set("callback", "?");
            return baseUrl + decodeURIComponent(query.toString());
        }
    };
} (this));