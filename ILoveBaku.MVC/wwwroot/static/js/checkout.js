$(document).ready(function () {
    let addresses = document.querySelectorAll('.single-address');
    addresses.forEach(item => {
        item.addEventListener('click', (e) => {
            addresses.forEach(item => {
                item.classList.remove('selected-address');
            })
            e.currentTarget.classList.add('selected-address')
        })
    });


    var counter = 0;

    $("#submit-payment").click(function (e) {
        counter++;
        $(".custom-modal-wrapper.payment-modal").remove();
        e.preventDefault();

        var formData = new FormData();

        formData.append("Name", $("#name").val());
        formData.append("Surname", $("#surname").val());
        formData.append("Phone", $("#phone").val());
        formData.append("PaymentType", $("[name='payment-option']:checked").attr("data-id") == undefined ? "" : $("[name='payment-option']:checked").attr("data-id"));
        formData.append("ShipmentOptions", $("[name='shipment-option']:checked").attr("data-id") == undefined ? "" : $("[name='shipment-option']:checked").attr("data-id"));
        formData.append("AddressId", $(".single-address.selected-address").attr("data-id") == undefined ? "" : $(".single-address.selected-address").attr("data-id"));
        formData.append("ShipmentDate", $(".shipping-option-item-date").val());

        $.ajax({
            url: "/payment/pay",
            data:formData,
            type: "post",
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.status == 400) {
                    var errorModal = `<div class="custom-modal-wrapper payment-modal active" data-id="${counter}">
                                         <div class="custom-modal success-modal">
                                             <div class="modal-container">
                                                 <div class="modal-img">
                                                     <img src="/static/img/x-mark.png" alt="">
                                                 </div>
                                                 <div class="modal-title-wrapper">
                                                     <p class="modal-title">${res.title}</p>
                                                     <p class="modal-subtitle">${res.error}</p>
                                                 </div>
                                                 <div class="modal-btn-wrapper">
                                                     <button class="modal-btn" data-target="${counter}">
                                                         OK
                                                     </button>
                                                 </div>
                                             </div>
                                         </div>
                                        </div>`
                    $('body').append(errorModal);
                }
                else {
                    if (res.withCard)
                        window.location.href = res.data;
                    else
                        window.location.href = res.data + "#orders";
                }
            }
        });
    });
    $("[name='shipment-option']").change(function () {
        if ($(this).data("id") == 10) {
            AddShipping(today);
        }
        else {
            RemoveShipping();
        }
    });
    function AddShipping(today) {
        let element = `<div class="shipping-amount d-flex align-items-center justify-content-between">
                        <label for="total">Shipping:</label>
                        <span>${today} <sup><i class="azn">M</i></sup> </span>
                    </div>`;
        $(element).insertAfter(".total-amount");
    }
    function RemoveShipping() {
        $(".shipping-amount").remove();
    }
});