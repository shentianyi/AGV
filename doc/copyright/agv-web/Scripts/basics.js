$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})

Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

function FutureDate(day) {
    var now = new Date();
    var FutureDate = new Date(now.getTime() - day * 24 * 60 * 60 * 1000).Format("yyyy/MM/dd");
    return FutureDate;
}

var Basic = {};

Basic.datepicker = function (date_picker) {
    $(date_picker).datetimepicker({
        format: 'Y-m-d',
        //scrollInput: false,
        timepicker: false
    });
}

Basic.timepicker = function (time_picker) {
    $(time_picker).datetimepicker({
        format: 'H:i',
        defaultTime: '00:00',
        datepicker: false
    });
}

Basic.datetimepicker = function (date_time_picker) {
    $(date_time_picker).datetimepicker({
        //scrollInput: false
    });
}

Basic.rangedatepicker = function (date_picker_start, date_picker_end) {
    $(date_picker_start).datetimepicker({
        format: 'Y-m-d',
        onShow: function (ct) {
            this.setOptions({
                maxDate: $(date_picker_end).val() ? $(date_picker_end).val() : false
            })
        },
        scrollInput: false,
        timepicker: false
    });

    $(date_picker_end).datetimepicker({
        format: 'Y-m-d',
        onShow: function (ct) {
            this.setOptions({
                minDate: $(date_picker_start).val() ? $(date_picker_start).val() : false
            })
        },
        //scrollInput: false,
        timepicker: false
    });
}
