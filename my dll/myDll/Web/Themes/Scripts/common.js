/*关于页面的操作
----------------------------------------------*/
//回调
function windowload() {
    rePage();
}
/**
刷新页面
**/
function rePage() {
    //Loading(true);
    window.location.href = window.location.href.replace('#', '');
    return false;
}
/**
* 返回上一级
*/
function back() {
    window.history.back(-1);
    //Loading(true)
}
//跳转页面
function Urlhref(url) {
    //Loading(true);
    window.location.href = url;
    return false;
}
/*打开新的页面
url:链接地址
name: _blank - URL加载到一个新的窗口。这是默认
_parent - URL加载到父框架
_self - URL替换当前页面
_top - URL替换任何可加载的框架集
name - 窗口名称
-----------------------------------*/
function newPage(url, name) {
    window.open(url, name, "location=no,menubar=no,toolbar=no,status=no,directories=no,scrollbars=yes,resizable=yes");
}

/*easyui 页面自动填充
url:返回json数据
*/
//easyui 页面自动填充
function loadData(url) {
    $.post(url, function (msg) {
        var result = JSON.parse(msg);
        $.each(result, function (name, value) {
            var object = document.getElementsByName(name);
            if (object.length > 0) {
                var type = object[0].parentElement.className;
                if ("textbox combo datebox" == type || "textbox textbox-readonly combo datebox" == type) {
                    //时间框处理
                    if (value != '&nbsp;') {
                        var value_ = value.replace(/\//g, '-');
                        $("#" + name).datetimebox('setValue', value_);
                    }
                } else {
                    var type = $("input[name='" + name + "']").attr('type');
                    if ("radio" == type) {
                        //单选按钮处理
                        $("input[name='" + name + "']").each(function () {
                            if ($(this).attr("value") == value) {
                                $(this).attr("checked", "checked");
                            }
                        });
                    } else {
                        if (value != '&nbsp;') {
                            $("input[name=\"" + name + "\"]").val(value);
                            $("textarea[name=\"" + name + "\"]").val(value);
                            $("select[name=\"" + name + "\"]").val(value);
                            
                        }
                    }
                }
            }
        });
    });
}


/********
接收地址栏参数
key:参数名称
**********/
function GetQuery(key) {
    var search = location.search.slice(1); //得到get方式提交的查询字符串
    var arr = search.split("&");
    for (var i = 0; i < arr.length; i++) {
        var ar = arr[i].split("=");
        if (ar[0] == key) {
            return ar[1];
        }
    }
}

/*弹出框（artDialog.js）
----------------------------------------------*/
function showFaceMsg(msg) {
    art.dialog({
        id: 'faceId',
        title: '温馨提醒',
        content: msg,
        icon: 'face-smile',
        time: 10,
        background: '#000',
        opacity: 0.1,
        lock: true,
        okVal: '关闭',
        ok: true
    });
}
function showWarningMsg(msg) {
    art.dialog({
        id: 'warningId',
        title: '系统提示',
        content: msg,
        icon: 'warning',
        time: 10,
        background: '#000',
        opacity: 0.1,
        lock: true,
        okVal: '关闭',
        ok: true
    });
}

/**
警告提示
msg: 显示消息
callBack：函数
**/
function showConfirmMsg(msg, callBack) {
    art.dialog({
        id: 'confirmId',
        title: '系统提示',
        content: msg,
        icon: 'warning',
        background: '#000000',
        opacity: 0.1,
        lock: true,
        button: [{
            name: '确定',
            callback: function () {
                callBack(true);
            },
            focus: true
        }, {
            name: '取消',
            callback: function () {
                this.close();
                return false;
            }
        }]
    });
}
/*弹出网页
/*url:          表示请求路径
/*_id:          ID
/*_title:       标题名称
/*width:        宽度
/*height:       高度
---------------------------------------------------*/
function openDialog(url, _id, _title, _width, _height, left, top) {
    art.dialog.open(url, {
        id: _id,
        title: _title,
        width: _width,
        height: _height,
        left: left + '%',
        top: top + '%',
        background: '#000000',
        opacity: 0.1,
        lock: true,
        resize: false,
        close: function () { }
    }, false);
}
//窗口关闭
function OpenClose() {
    art.dialog.close();
}

/*弹出网页（不指定起始位置坐标）
/*url:          表示请求路径
/*_id:          ID
/*_title:       标题名称
/*width:        宽度
/*height:       高度
---------------------------------------------------*/
function openDialog(url, _id, _title, _width, _height) {
    art.dialog.open(url, {
        id: _id,
        title: _title,
        width: _width,
        height: _height,
        background: '#000000',
        opacity: 0.1,
        lock: true,
        resize: true,
        close: function () { }
    }, false);
}

/*弹出网页（不指定高度与宽度）
/*url:          表示请求路径
/*_id:          ID
/*_title:       标题名称
/*width:        宽度
/*height:       高度
---------------------------------------------------*/
function openDialog(url, _id, _title) {
    art.dialog.open(url, {
        id: _id,
        title: _title,
        height: $(this).height() - 10,
        width: $(this).width() - 10,
        background: '#000000',
        opacity: 0.1,
        lock: true,
        resize: true,
        close: function () { }
    }, false);
}



/*关于日期的操作start
----------------------------------------------*/
//获取月份差值
function getMonthCha(begintime, endtime) {
    //默认格式为"2003-03-03",根据自己需要改格式和方法
    var year1 = begintime.substr(0, 4);
    var year2 = endtime.substr(0, 4);
    var month1 = begintime.substr(5, 2);
    var month2 = endtime.substr(5, 2);
    var len = (year2 - year1) * 12 + (month2 - month1);
    return len;
}
//获取周差值
function getZhouCha(begintime, endtime) {
    daysCha = Math.floor((Date.parse(endtime) - Date.parse(begintime)) / (24 * 3600 * 1000));
    return (daysCha + 1) / 7;
}

//获取当前日期在当前年第几周函数封装，例如2013-08-15 是当前年的第32周
function theWeek(zhoutime) {
    var totalDays = 0;
    myyears = zhoutime.substr(0, 4);
    mymonth = zhoutime.substr(5, 2);
    myday = zhoutime.substr(8, 2);
    alert(myyears + "-" + mymonth + "-" + myday);
    if (myyears < 1000)
        myyears += 1900
    var days = new Array(12);
    days[0] = 31;
    days[2] = 31;
    days[3] = 30;
    days[4] = 31;
    days[5] = 30;
    days[6] = 31;
    days[7] = 31;
    days[8] = 30;
    days[9] = 31;
    days[10] = 30;
    days[11] = 31;

    //判断是否为闰年，针对2月的天数进行计算
    if (Math.round(myyears / 4) == myyears / 4) {
        days[1] = 29
    } else {
        days[1] = 28
    }

    if (mymonth == 1) {
        totalDays = totalDays + myday;
    } else {
        var curMonth = mymonth;
        for (var count = 1; count <= curMonth; count++) {
            totalDays = totalDays + days[count - 1];
        }
        totalDays = totalDays + myday;
    }
    //得到第几周
    var week = Math.round(totalDays / 7);
    return week;
}


var Common = {

    /**

    * 格式化日期（不含时间）

    */

    formatterDate: function (date) {

        if (date == undefined) {

            return "";

        }

        /*json格式时间转js时间格式*/

        date = date.substr(0, date.length - 1);

        var obj = eval('(' + "{Date: new " + date + "}" + ')');

        var date = obj["Date"];

        if (date.getFullYear() < 1900) {

            return "";

        }



        var datetime = date.getFullYear()

                + "-"// "年"

                + ((date.getMonth() + 1) > 10 ? (date.getMonth() + 1) : "0"

                        + (date.getMonth() + 1))

                + "-"// "月"

                + (date.getDate() < 10 ? "0" + date.getDate() : date

                        .getDate());

        return datetime;

    },

    /**

    * 格式化日期（含时间"00:00:00"）

    */

    formatterDate2: function (date) {

        if (date == undefined) {

            return "";

        }

        /*json格式时间转js时间格式*/

        date = date.substr(1, date.length - 2);

        var obj = eval('(' + "{Date: new " + date + "}" + ')');

        var date = obj["Date"];

        if (date.getFullYear() < 1900) {

            return "";

        }



        /*把日期格式化*/

        var datetime = date.getFullYear()

                + "-"// "年"

                + ((date.getMonth() + 1) > 10 ? (date.getMonth() + 1) : "0"

                        + (date.getMonth() + 1))

                + "-"// "月"

                + (date.getDate() < 10 ? "0" + date.getDate() : date

                        .getDate()) + " " + "00:00:00";

        return datetime;

    },

    /**

    * 格式化去日期（含时间）

    */

    formatterDateTime: function (date) {

        if (date == undefined) {

            return "";

        }

        /*json格式时间转js时间格式*/

        date = date.substr(1, date.length - 2);

        var obj = eval('(' + "{Date: new " + date + "}" + ')');

        var date = obj["Date"];

        if (date.getFullYear() < 1900) {

            return "";

        }



        var datetime = date.getFullYear()

                + "-"// "年"

                + ((date.getMonth() + 1) > 10 ? (date.getMonth() + 1) : "0"

                        + (date.getMonth() + 1))

                + "-"// "月"

                + (date.getDate() < 10 ? "0" + date.getDate() : date

                        .getDate())

                + " "

                + (date.getHours() < 10 ? "0" + date.getHours() : date

                        .getHours())

                + ":"

                + (date.getMinutes() < 10 ? "0" + date.getMinutes() : date

                        .getMinutes())

                + ":"

                + (date.getSeconds() < 10 ? "0" + date.getSeconds() : date

                        .getSeconds());

        return datetime;

    }

};
