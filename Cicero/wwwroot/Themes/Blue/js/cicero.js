// masthead scroll adding class
        $(window).scroll(function () {
            var scroll = $(window).scrollTop();

            if (scroll >= 10) {
                $(".masthead").addClass("sticky");
            } else {
                $(".masthead").removeClass("sticky");
            }
            if (scroll >= 600) {
                $(".masthead").addClass("small-hd");
            } else {
                $(".masthead").removeClass("small-hd");
            }
        });