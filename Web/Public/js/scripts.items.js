(function (global, undefined) {
    global.Items = {
        get: function (url, callback) {
            $.getJSON(url, function (items) {
                var result = {};
                result.Items = items;
                callback(result);
            });
        }
    };
} (this));