$(document).ready(() => {
    $('#addToCartForm').submit((e) => {
        e.preventDefault();
        if ($(".size-radiobox input").length == 0) {
            $.ajax({
                url: '/cart/add',
                type: 'POST',
                data: $(e.target).serialize(),
                success: (response) => {
                    if (response.hasOwnProperty('status')) {
                        switch (response.status) {
                            case 200:
                                const image = $('.carousel-indicators li:first-of-type img').attr('src');
                                const name = $('.product-title').text();
                                const cartId = response.cartId;
                                const count = $('input[name=count]').val();
                                const price = Number($('.product-price').text().replace(',', '.').trim());
                                const totalPrice = round(price * count, 2);
                                $('#carts tbody').prepend(NewCartElement(image, name, cartId, price, $('input[name=count]').data('max'), count, totalPrice));
                                $('#carts tbody .count-input:first-of-type').data('max', $('#carts tbody .count-input:first-of-type').attr('max'));
                                $('#totalAmount').text(round(Number($('#totalAmount').text().replace(',', '.').trim()) + totalPrice, 2));
                                $('.basket-count').show();
                                $('.basket-count').text(Number($('.basket-count').text().trim()) + 1);
                                $('.show-basket-box').click();
                                break;
                            case 404:
                                alert(response.error); break;
                            case 406:
                                alert(response.error); break;
                            case 208:
                                alert(response.error); break;
                        }
                    }
                }
            });
        }
        else {
            if ($(".size-radiobox input:checked").length == 1) {
                $.ajax({
                    url: '/cart/add',
                    type: 'POST',
                    data: $(e.target).serialize(),
                    success: (response) => {
                        if (response.hasOwnProperty('status')) {
                            switch (response.status) {
                                case 200:
                                    const image = $('.carousel-indicators li:first-of-type img').attr('src');
                                    const name = $('.product-title').text();
                                    const cartId = response.cartId;
                                    const count = $('input[name=count]').val();
                                    const price = Number($('.product-price').text().replace(',', '.').trim());
                                    const totalPrice = round(price * count, 2);
                                    $('#carts tbody').prepend(NewCartElement(image, name, cartId, price, $('input[name=count]').data('max'), count, totalPrice));
                                    $('#carts tbody .count-input:first-of-type').data('max', $('#carts tbody .count-input:first-of-type').attr('max'));
                                    $('#totalAmount').text(round(Number($('#totalAmount').text().replace(',', '.').trim()) + totalPrice, 2));
                                    $('.basket-count').show();
                                    $('.basket-count').text(Number($('.basket-count').text().trim()) + 1);
                                    $('.show-basket-box').click();
                                    break;
                                case 404:
                                    alert(response.error); break;
                                case 406:
                                    alert(response.error); break;
                                case 208:
                                    alert(response.error); break;
                            }
                        }
                    }
                });
            }
            else {
                alert("Ölçü seçin");
            }
        }
        
    });
});