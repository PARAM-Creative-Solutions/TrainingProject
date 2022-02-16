//*****this date validation script give problem when it opened in firefox
//$(function () {
//    $.validator.methods.date = function (value, element) {
//        var dateParts = value.split('-');        
//        var dateStr = dateParts[1] + '-' + dateParts[0] + '-' + dateParts[2];
//        return this.optional(element) || !/Invalid|NaN/.test(new Date(dateStr));
//    };
//});

//**********below script resolves the problem of date picker and validation for firefox and ie and all updated browesers
$.validator.methods.date = function (value, element) {
    return this.optional(element) || (/^\d{1,2}[\/-]\d{1,2}[\/-]\d{4}(\s\d{2}:\d{2}(:\d{2})?)?$/.test(value)
            && !/Invalid|NaN/.test(new Date(value.substr(6, 4) + '-' + value.substr(3, 2) + '-' + value.substr(0, 2))));
}

