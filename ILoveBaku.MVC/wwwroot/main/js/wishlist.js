$(document).ready(() => {
    $(document).on('click', '.wishlist', (e) => {
        e.preventDefault();
        $.ajax({
            url: '/wishlist/process',
            type: 'POST',
            data: { productStockId: $(e.currentTarget).data('productstockid') },
            success: (response) => {
                if (response.hasOwnProperty('status')) {
                    switch (response.status) {
                        case 200:
                            if (response.process) {
                                $(e.currentTarget.activeElement).find('img').attr('src', '/static/img/heart-red.png');
                            }
                            else {
                                $(e.currentTarget.activeElement).find('img').attr('src', '/static/img/heart.png');
                                if (location.pathname == '/wishlist/list') {
                                    $(e.currentTarget.activeElement).parents('.clothing-item').parent().remove()
                                }
                            }
                            break;
                        case 400:
                            alert(response.error); break;
                        case 404:
                            alert(response.error); break;
                    }
                }
            }
        });
    });
});