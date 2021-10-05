let timeout = null;
$('#searchFilter').on('input', SearchFilter);
var total = 0;
$(document).on('input', '.checkbox-items input[type=checkbox]', () => {
    $('.page-item').removeClass('active');
    $('.page-item').removeClass('disabled');

    $('.page-item:first-child').addClass('active');
    $('.page-item:first-child').addClass('disabled');
    SearchFilter(() => {
        $("#pagination-container div").remove();
        MakePagination(total, 1);
    });
});

$(document).on('input', '.gender-clothing input[type=checkbox]', () => {
    $('.page-item').removeClass('active');
    $('.page-item').removeClass('disabled');

    $('.page-item:first-child').addClass('active');
    $('.page-item:first-child').addClass('disabled');
    SearchFilter(() => {
        $("#pagination-container div").remove();
        MakePagination(total, 1);
    });
});

$(document).on('change', '.min-price, .max-price', () => {
    $('.page-item').removeClass('active');
    $('.page-item').removeClass('disabled');

    $('.page-item:first-child').addClass('active');
    $('.page-item:first-child').addClass('disabled');

    SearchFilter(() => {
        $("#pagination-container div").remove();
        MakePagination(total, 1);
    });
});

//$(document).on('click', '.paginationjs-page.J-paginationjs-page.page-item:not(.active)', (e) => {
//    e.preventDefault();
//    $('.page-item').removeClass('active');
//    $('.page-item').removeClass('disabled');
//    $(e.currentTarget).addClass('active');
//    $(e.currentTarget).addClass('disabled');


//    SearchFilter();
//});


function GenerateSearchFilterQuery() {
    let searchFilterUrl = '';
    let separator = '|';
    let key = $('#searchFilter').val();
    if (key) searchFilterUrl += `key=${key}`;
    let categories = $('#categoryFilter .checkbox-items input[type=checkbox]:checked');
    if (categories.length > 0) {
        if (searchFilterUrl.length > 0) searchFilterUrl += separator;
        searchFilterUrl += 'categories=';
        for (let category of categories) {
            searchFilterUrl += `${$(category).data('route-name')},`;
        }
        searchFilterUrl = searchFilterUrl.slice(0, -1);
    }
    let filters = $('.filter-checkbox:not(#categoryFilter):not(#priceFilter)');
    for (var filter of filters) {
        let values = $(filter).find('input[type=checkbox]:checked');
        if (values.length > 0) {
            if (searchFilterUrl.length > 0) searchFilterUrl += separator;
            searchFilterUrl += `${$(filter).data('route-name')}=`;
            for (let value of values) {
                searchFilterUrl += `${$(value).data('route-value')},`;
            }
            searchFilterUrl = searchFilterUrl.slice(0, -1);
        }
    }
    if ($('#priceFilter')) {
        let min = $('.min-price');
        let max = $('.max-price');
        if (!(min.val() == min.data('min') && max.val() == max.data('max'))) {
            if (searchFilterUrl.length > 0) searchFilterUrl += separator;
            searchFilterUrl += `price=${$('.min-price').val()}-${$('.max-price').val()}`;
        }
    }
    if ($('.page-item.active').length > 0) {
        if (searchFilterUrl.length > 0) searchFilterUrl += separator;
        var pageNumber = isNaN($('.page-item.active a').text())
        console.log(pageNumber);
        searchFilterUrl += `page=${pageNumber ? "1" : $('.page-item.active a').text()}`;
    }
   
    return searchFilterUrl;
}

function GenerateSearchFilterUrl() {
    let URL = '/products/searchfilter';

    let categoryName = $('#categoryName').data('route-name');

    if (categoryName) URL += `/${categoryName}`;

    let filters = GenerateSearchFilterQuery();

    if (filters.length > 0) URL += `${(categoryName) ? '/' : '?filters='}${filters}`;

    return URL;
}

function SearchFilter(callback) {
    //clearTimeout(timeout);
    //timeout = setTimeout(() => {
        let URL = GenerateSearchFilterUrl();
        //history.pushState({}, '', URL.replace('/searchfilter', ''));
        $.ajax({
            url: URL,
            type: 'GET',
            success: (response) => {
                $('#products').html(response.data.result);
                total = response.total;

            }
        }).then(callback);
    //}, 100);
}

function MakePagination(total, current) {
    $('#pagination-container').pagination({
        dataSource: function (done) {
            var result = [];
            for (var i = 1; i <= total; i++) {
                result.push(i);
            }
            done(result);
        },
        pageRange: 2,
        pageSize: 20,
        showPrevious: true,
        showNext: true,
        autoHidePrevious: true,
        autoHideNext: true,
        pageNumber: current,
        activeClassName: "disabled active",
        ulClassName: "pagination pagination-custom d-flex justify-content-center",
        afterPageOnClick: function (e) {
            e.preventDefault();
            $('.page-item').removeClass('active');
            $('.page-item').removeClass('disabled');
            //$(e.currentTarget).addClass('active');
            //$(e.currentTarget).addClass('disabled');

            $(".paginationjs-pages li").addClass("page-item");
            $(".paginationjs-pages li a").addClass("page-link");

            SearchFilter();

           

            var scrollTo = $("#products").offset().top - 200;
            $('html, body').animate({
                scrollTop: scrollTo
            }, 500);
        },
        afterPreviousOnClick: function (e) {
            e.preventDefault();
            $('.page-item').removeClass('active');
            $('.page-item').removeClass('disabled');


            $(".paginationjs-pages li").addClass("page-item");
            $(".paginationjs-pages li a").addClass("page-link");
            SearchFilter();

            

            var scrollTo = $("#products").offset().top - 200;
            $('html, body').animate({
                scrollTop: scrollTo
            }, 500);
        },
        afterNextOnClick: function (e) {
            e.preventDefault();
            $('.page-item').removeClass('active');
            $('.page-item').removeClass('disabled');

            $(".paginationjs-pages li").addClass("page-item");
            $(".paginationjs-pages li a").addClass("page-link");

            SearchFilter();

      

            var scrollTo = $("#products").offset().top - 200;
            $('html, body').animate({
                scrollTop: scrollTo
            }, 500);
        }
    });

    $(".paginationjs-pages li").addClass("page-item");
    $(".paginationjs-pages li a").addClass("page-link");
}
function GenerateFormData(formData, data, parentKey) {
    if (data && typeof (data) === "object" && !(data instanceof Date) && !(data instanceof File)) {
        for (var key in data) {
            GenerateFormData(formData, data[key], parentKey ? `${parentKey}[${key}]` : key)
        }
    }
    else {
        const value = data ?? '';

        formData.append(parentKey, value);
    }
}

function ToFormData(data) {
    const formData = new FormData();

    GenerateFormData(formData, data);

    return formData;
}