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
        }
    };
}(this));