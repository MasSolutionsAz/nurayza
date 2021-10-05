function Specification(categoryId) {
    this.categoryId = categoryId;
    this.getFullDto = async function (doneCallback) {
        var ajax = new AJAX();
        ajax._constructor("/Admin/CategorySpecification/Get/" + categoryId, "get",
            (res, loader) => {
                console.log(res);
                for (var group of res.data) {
                    if (group.categorySpecifications.length > 0) {
                        $(".group-wrapper").remove();
                        var groupWrapper = $(`<div style='background:white;margin:30px 0px;padding:10px 20px;' class='group-wrapper col-6' data-name='${group.name}'>
<span class="mb-3 badge badge-danger">Bu məlumatlar dilə uyğun olaraq dəyişilməlidir.</span>
                    </div>`);
                        var groupName = $(`<h3>${group.name}</h3>`);
                        groupWrapper.append(groupName);

                        for (var specification of group.categorySpecifications) {
                            var specificationWrapper = $(`<div class='personal-info-field specification-wrapper' data-name='${specification.name}'></div>`);
                            var specificationName = $(`<p class='info-field-title'>${specification.name}</p>`);
                            specificationWrapper.append(specificationName);
                            //make controller
                            var controller = document.createElement(specification.type.controller.name);
                            var controllerAsJquery = $(controller);

                            if (specification.type.name == "string") {
                                controllerAsJquery.attr("data-process","multilang")
                            }
                            controllerAsJquery.attr("data-table", specification.type.tableName);
                            controllerAsJquery.attr("data-specification", specification.id);
                            controllerAsJquery.attr("data-type", specification.type.name);
                            controllerAsJquery.addClass("specification-element");

                            if (specification.type.id == 40) {
                                controllerAsJquery.attr("id", "barcode");
                            }
                            for (var controllerSpecification of specification.type.controller.specifications) {
                                var value = "";
                                for (var controllerSpecificationValue of controllerSpecification.values) {
                                    value += controllerSpecificationValue.value + " ";
                                }
                                value = value.slice(0, -1);
                                controllerAsJquery.attr(controllerSpecification.name, value);
                            }
                            //end controller
                            if (specification.type.controller.name == "select") {
                                controllerAsJquery.attr("data-multi", "true");
                                controllerAsJquery.append(`<option value='0'>--</option`);

                                for (var property of specification.properties) {
                                    let option = $(`<option value='${property.id}'>${property.name}</option>`);
                                    controllerAsJquery.append(option);
                                }
                            }
                            else {
                                controllerAsJquery.attr("data-multi", "false");
                                controllerAsJquery.attr("data-property", specification.properties[0].id);
                            }
                            //controllerAsJquery.addClass("info-field-input");
                            controllerAsJquery.addClass("form-element");
                            specificationWrapper.append(controllerAsJquery);
                            groupWrapper.append(specificationWrapper);
                        }
                        groupWrapper.insertBefore($(".submit-button-wrapper"));
                        //$(".main-body").append(groupWrapper);
                    }
                }

                if (doneCallback != null || doneCallback != undefined) {
                    doneCallback();
                }
                loader.remove();
            },
            (res, loader) => {
                loader.remove();
                alert("Xeta bas verdi!");
            });
        await ajax._getAsync();
    };
}