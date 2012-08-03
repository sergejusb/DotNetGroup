(function (global, undefined) {
    global.Stream = {
        attachActions: function () {
            $(".item").live({
                mouseenter:
                    function () {
                        $(this).find(".item-hover").css("display", "inline-block");
                    },
                mouseleave:
                    function () {
                        $(this).find(".item-hover").css("display", "none");
                    }
            });
        },
        attachLoadMore: function () {
            $("#load-more, #load-more a").live("click", function () {
                var isCompleted = false;
                setTimeout(function () {
                    if (!isCompleted) {
                        $("#spinner").spin();
                    }
                }, 500);

                var url = $("#load-more a").attr("href");
                $("#load-more").remove();
                $.get(url).success(function(html) {
                    $("#stream").append(html);
                    $("#spinner").spin(false);
                    isCompleted = true;
                });

                return false;
            });
        }
    };
}(this));