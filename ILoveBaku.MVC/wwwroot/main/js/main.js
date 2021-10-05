$(document).ready(() => {
    const basketCount = $('table#carts tbody tr').length;
    if (basketCount > 0)
        $('.basket-count').text(basketCount);
    else
        $('.basket-count').hide();

    inputFilter('.count-input', inputValidate);

    $('.count-input').data('max', $('.count-input').attr('max'));

    let timeout = null;
    if (document.getElementById('search') != null) {
        document.getElementById('search').addEventListener('input', (e) => {
            clearTimeout(timeout);
            timeout = setTimeout(SearchProduct(e.target.value), 100);
        });

        $('.search-categories a').click((e) => {
            $(e.currentTarget).toggleClass("active");
            SearchProduct($('#search').val());
           
        });
    }
   

    $(document).on('click', '.count-box .increase', (e) => {
        e.preventDefault();
        let input = e.currentTarget.previousElementSibling;
        if (Number(input.value) < $(input).data('max') && inputValidate(input)) {
            $(e.currentTarget).prev().val(Number($(e.currentTarget).prev().val()) + 1);
            $(e.currentTarget).prev().trigger('input');
            $(e.currentTarget).prev().trigger('change');
            $(e.currentTarget).siblings(".decrease").removeClass("dis");
        }
    });

    $(document).on('click', '.count-box .decrease', (e) => {
        e.preventDefault();
        let input = e.currentTarget.nextElementSibling;
        if ($(e.currentTarget).next().val() <= 1) {
            $(e.currentTarget).addClass("dis");
            return;
        }
        if (inputValidate(input)) {
            $(e.currentTarget).next().val(Number($(e.currentTarget).next().val()) - 1);
            $(e.currentTarget).next().trigger('input');
            $(e.currentTarget).next().trigger('change');
        }
    });

    $(document).on('change', '#carts .count-input', (e) => {
        $.ajax({
            url: '/cart/update',
            type: 'PUT',
            data: { cartDetailId: $(e.target).data('cartid'), count: $(e.target).val() },
            success: (response) => {
                if (response.hasOwnProperty('status')) {
                    switch (response.status) {
                        case 200:
                            let price = Number(String($(e.target).data('price')).replace(',', '.').trim());
                            const totalPrice = round(($(e.target).val() * price), 2);
                            let totalPriceElement = $(e.target).parents('tr').find('td.orders-product-price small');
                            price = totalPrice - Number(totalPriceElement.text().replace(',', '.').trim());
                            totalPriceElement.text(`${totalPrice} `);
                            $('#totalAmount').text(round((Number($('#totalAmount').text().replace(',', '.')) + price), 2));
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

    $(document).on('click', '#carts .delete-product', (e) => {
        let deleteIcon = $(e.currentTarget);
        $.ajax({
            url: '/cart/delete',
            type: 'DELETE',
            data: { cartDetaildId: $(e.currentTarget).data('cartid') },
            success: (response) => {
                if (response.hasOwnProperty('status')) {
                    switch (response.status) {
                        case 200:
                            const totalPrice = Number(deleteIcon.parents('tr')
                                .find('td.orders-product-price small').text()
                                .replace(',', '.').trim());
                            deleteIcon.parents("tr").fadeOut();
                            setTimeout(function () {
                                deleteIcon.parents("tr").remove();
                            }, 500);
                            $('#totalAmount').text(round((Number($('#totalAmount').text().replace(',', '.')) - totalPrice), 2));
                            $('.basket-count').text(Number($('.basket-count').text().trim()) - 1);
                            break;
                        case 404:
                            alert(response.error); break;
                    }
                }
            }
        });
    });
});

function SearchProduct(key) {
    let categories = [];
    $(".search-categories a.active").each((index, element) => {
        categories.push($(element).data("categoryname"));
    });
   
    $.ajax({
        url: '/products/search',
        type: 'GET',
        traditional: true,
        data: { key: key, categories: categories },
        success: (response) => {
            $('#searchedProducts').empty();
            if (response.hasOwnProperty('status')) {
                switch (response.status) {
                    case 200:
                        for (const product of response.products) {
                            const productElement = `<div class="search-item">
                                                        <a href="/product/${product.routeName}">
                                                            <div class="img-scale img-scale-search">
                                                                <img src="${product.image}" alt="product">
                                                            </div>
                                                            <div class="d-flex justify-content-between align-items-center pt-2">
                                                                <p>${product.name}</p>
                                                                <p><b>${product.discountedPrice} <sup><i class="azn">M</i></sup></b></p>
                                                            </div>
                                                        </a>
                                                    </div>`;
                            $('#searchedProducts').append(productElement);
                        }
                        break;
                }
            }
        }
    });
}

function round(number, decimals) {
    return Number(number.toFixed(decimals));
}

function inputFilter(selector, filter) {
    ['input', 'change', 'keydown', 'keyup', 'mousedown', 'mouseup', 'select', 'contextmenu', 'drop'].forEach((event) => {
        document.addEventListener(event, (e) => {
            if (e.target.getAttribute('id') == selector.replace('#', '') || e.target.classList.contains(selector.replace('.', ''))) {
                if (!filter(e.target)) {
                    if (e.target.hasOwnProperty('oldValue'))
                        e.target.value = e.target.oldValue;
                    else
                        e.target.value = e.target.defaultValue;
                }
                else
                    e.target.oldValue = e.target.value;
            }
        });
    });
}

function inputValidate(input) {
    let value = Number(input.value);
    return new RegExp('^[0-9]+$').test(value) && value !== 0 && value <= Number($(input).data('max'));
}

function operatorValidate(input) {
    let value = input.value.replace('+', '');
    let phoneNumber = takeOriginalPhoneNumber(value);
    const operators = ['010', '050', '051', '055', '070', '077'];
    const operatorMaxLength = longestString(operators);
    const countryCodeMaxLength = countryCodeValidate(phoneNumber);
    const hasCountryCode = countryCodeMaxLength > 0;
    let operatorOnPhoneNumber = phoneNumber.substr(countryCodeMaxLength, (hasCountryCode) ?
        operatorMaxLength - 1 : operatorMaxLength);
    for (var operator of operators) {
        if (hasCountryCode && operator.substr(1).startsWith(operatorOnPhoneNumber)) {
            if (phoneNumber.length > countryCodeMaxLength + operatorMaxLength + 6) return false;
            input.value = ShowPhoneNumber(phoneNumber, hasCountryCode, input.isDelete, value.length);
            return true;
        }
        else if (!hasCountryCode && operator.startsWith(operatorOnPhoneNumber)) {
            if (phoneNumber.length > operatorMaxLength + 7) return false;
            input.value = ShowPhoneNumber(phoneNumber, hasCountryCode, input.isDelete, value.length);
            return true;
        }
    }
    input.nextElementSibling.innerText = "Operatoru düzgün daxil edin.";
    return false;
}

function countryCodeValidate(phoneNumber) {
    const countryCodes = ['994'];
    const maxCount = longestString(countryCodes);
    for (var countryCode of countryCodes) {
        if (phoneNumber != '' && countryCode.startsWith(phoneNumber.substr(0, maxCount))) {
            return maxCount;
        }
    }
    return 0;
}

function takeOriginalPhoneNumber(phoneNumber) {
    let result = '';
    for (const number of phoneNumber) {
        if (!['(', ')', ' '].includes(number))
            result += number;
    }
    return result;
}

function ShowPhoneNumber(phoneNumber, hasCountryCode, isDelete, inputValueLength) {
    let result = '';
    for (var i = 0; i < phoneNumber.length; i++) {
        if (!hasCountryCode && i == 0) result = '(';
        result += phoneNumber[i];
        if (hasCountryCode) {
            if ((i == 2 || i == 4 || i == 7 || i == 9) && (!isDelete || inputValueLength > result.length)) result += ' ';
        }
        else {
            if (i == 2) {
                if (!isDelete || inputValueLength > result.length) result += ')';
                if (!isDelete || inputValueLength > result.length) result += ' ';
            }
            if ((i == 5 || i == 7) && (!isDelete || inputValueLength > result.length)) result += ' ';
        }
    }
    return (hasCountryCode) ? `+${result}` : result;
}

function longestString(array) {
    let max = 0;
    for (let item of array) {
        if (item.length > max) {
            max = item.length;
        }
    }
    return max;
}

function NewCartElement(image, name, cartId, price, stockCount, count, totalPrice) {
    return `<tr>
                <td>
                    <div class="orders-product-detail align-items-center clearfix d-flex">
                        <img src="${image}">
                        <p class="orders-product-name">${name}</p>
                    </div>
                </td>
                <td class="text-center">
                    <div class="form-group mb-0 form-group-count d-inline-block">
                        <div class="d-flex">
                            <div class="count-box mr-lg-4 d-flex align-items-center">
                                <button class="decrease">-</button>
                                <input type="text" class="count-input" data-cartid="${cartId}" data-price="${price}" max="${stockCount}" value="${count}">
                                <button class="increase">+</button>
                            </div>
                        </div>
                    </div>
                </td>
                <td class="orders-product-price">
                    <small>${totalPrice} </small><sup><i class="azn">M</i></sup>
                </td>
                <td>
                    <a class="delete-product" data-cartid="${cartId}"><img src="/static/img/cross.png" alt="icon"></a>
                </td>
            </tr>`;
}