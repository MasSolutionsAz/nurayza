function Filter() {
    var url;
    var type;
    var error;
    var success;
    this._constructor = function (_url,_type,_success,_error) {
        url = _url;
        type = _type;
        success = _success;
        error = _error;
    }

    this._filter = function () {
        var ajax = new AJAX();
        ajax._constructor(url, type, success, error);

        if (type.toLowerCase() == "get") {
            ajax._getAsync();
        }
        else {
            alert("Filter get sorgu ile aparilmalidir.");
        }
    }

    this.makeQuery = function (obj) {

    }
}