(function (global, undefined) {
    global.Stream = {
        get: function (url, callback) {
            var isCompleted = false;

            setTimeout(function () {
                if (!isCompleted) {
                    $("#spinner").spin();
                }
            }, 500);

            $.getJSON(url, function (items) {
                callback(items);

                $("#spinner").spin(false);
                isCompleted = true;
            });
        }
    };
} (this));