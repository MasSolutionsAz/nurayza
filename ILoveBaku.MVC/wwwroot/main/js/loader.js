function Loader(_src) {
    this.src = _src;
    this.show = function (appendTo) {
        appendTo.append($(`<div class="overlay-preloader">
                        <img src="/main/img/${this.src}" />    
                        </div>`));
    };

    this.remove = function () {
        $(document.body).find(".overlay-preloader").remove();
    };
}