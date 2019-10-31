//主页加载js
$(function () {
    $("#BeautifulGreetings").text(BeautifulGreetings());
    writeDateInfo();
})

//换验证码
function ToggleCode(obj, codeurl) {
    $("#txtCode").val("");
    $("#" + obj).attr("src", codeurl + "?time=" + Math.random());
}


//当前日期
function writeDateInfo() {
    var day = "";
    var month = "";
    var ampm = "";
    var ampmhour = "";
    var myweekday = "";
    var year = "";
    mydate = new Date();
    myweekday = mydate.getDay();
    mymonth = mydate.getMonth() + 1;
    myday = mydate.getDate();
    myyear = mydate.getYear();
    year = (myyear > 200) ? myyear : 1900 + myyear;
    if (myweekday == 0)
        weekday = " 星期日";
    else if (myweekday == 1)
        weekday = " 星期一";
    else if (myweekday == 2)
        weekday = " 星期二";
    else if (myweekday == 3)
        weekday = " 星期三";
    else if (myweekday == 4)
        weekday = " 星期四";
    else if (myweekday == 5)
        weekday = " 星期五";
    else if (myweekday == 6)
        weekday = " 星期六";
    $("#datetime").text(year + "年" + mymonth + "月" + myday + "日 " + weekday);
}

//温馨提示
function BeautifulGreetings() {
    var now = new Date();
    var hour = now.getHours();
    if (hour < 3) { return ("夜深了,早点休息吧！") }
    else if (hour < 9) { return ("早上好！") }
    else if (hour < 12) { return ("上午好！") }
    else if (hour < 14) { return ("中午好！") }
    else if (hour < 18) { return ("下午好！") }
    else if (hour < 23) { return ("晚上好！") }
    else { return ("夜深了,早点休息吧！") }
}