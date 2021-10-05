function AJAX() {
    //old
    //this.success;
    //this.error;
    //old end

    this._url;
    this.successCallback;
    this.errorCallback;
    this.type;
    this.formData;

    this._constructor = function (url, type, successCallback, errorCallback) {
        this._url = url;
        this.successCallback = successCallback;
        this.errorCallback = errorCallback;
        this.type = type;
    };

    this._getAsync = async function () {
        var loader = new Loader("preloader.svg");
        loader.show($(document.body));
        this._ajaxGet(loader, this.successCallback, this.errorCallback);
    };
    this._postAsync = async function (data,isReady=false) {
        var loader = new Loader("preloader.svg");
        loader.show($(document.body));

        var formData;
        if (!isReady)
            formData = this.makeFormData(data);

        this._ajaxPost(!isReady?formData:data, this.type, loader, this.successCallback, this.errorCallback);
    };
    this._ajaxGet = function (loader, successCallback, errorCallback) {
        $.ajax({
            url: this._url,
            dataType: "json",
            type: "get",
            success: function (res) {
                if (res.status == 200)
                    successCallback(res, loader);
                else
                    errorCallback(res, loader);
            },
            error: function () {

                errorCallback(loader);
            }
        });
    };
    this._ajaxPost = function (formData, type, loader, successCallback, errorCallback) {
        $.ajax({
            url: this._url,
            data: formData,
            dataType: "json",
            type: type,
            cache: false,
            processData: false,
            contentType: false,
            success: function (res) {
                if (res.status == 200)
                    successCallback(res, loader);
                else
                    errorCallback(res,loader);
            },
            error: function () {
                errorCallback(loader);
            }
        });

    };
    this.makeFormData = function (array) {
        var formData = new FormData();
        for (var data of array) {
            formData.append(data.name, data.value);
        }
        this.formData = formData;
        return formData;
    };

    this.makeQuery = function (formArray) {
        var query = "";
        for (var formElement of formArray) {
            if (query == "") {
                query += "?" + formElement.name + "=" + formElement.value;
            }
            else {
                query += "&" + formElement.name + "=" + formElement.value;
            }
        }

        return query;
    };

    this.getData = function (prop) {
        this.formData.get(prop);
    };

    this.getData = function (data, prop) {
        for (var d of data) {
            if (d.name == prop)
                return d.value;
        }

        return null;
    };
}






