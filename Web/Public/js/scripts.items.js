(function (global, undefined) {
    global.Items = {
        get: function (url, callback) {
            var isCompleted = false;         

            setTimeout(function () {
                if (!isCompleted) {
                    $("#spinner").spin();
                }
            }, 500);

            $.getJSON(url, function (items) {
                var result = {};
                result.Items = items;
                callback(result);

                $("#spinner").spin(false);
                isCompleted = true;
            });
        }
    };
} (this));