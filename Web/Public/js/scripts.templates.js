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
                            return "rss";
                        case 2:
                            return "twitter";
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