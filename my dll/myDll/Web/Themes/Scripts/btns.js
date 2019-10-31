$(function () {
    var url = window.location.href;
    var str = url.split('/');
    //    var name = str[str.length - 1];
    //    var index = name.indexOf('?', 0);
    //    if (index > 0) {
    //        name = name.substring(0, index);
    //    }
    var name = '';
    if (str.length >= 3) {
        for (var i = 3; i < str.length; i++) {
            name = name + '/' + str[i];
        }
    }


    $.getJSON('/ashx/GetButton.ashx?pageName=' + name, function (msg) {
        if (msg.flag) {
            alert("Session超时，请重新登录！");
            window.top.location.href = "/Frame/login.html"; return;
        }
        var str = '';
        for (var i = 0; i < msg.length; i++) {
            str += '&nbsp;&nbsp;<a href="javascript:void(0)" onclick="' + msg[i].BtnCode + '()"><span class="' + msg[i].Icon + '">&nbsp;</span>' + msg[i].ButtonName + '</a>';
        }
        $('.btabs').append(str);
        //预检部分Tabs操作
        if ($('.btabs_product').html() && $('.btabs_pack').html()) {
            var str = '';
            for (var i = 0; i < msg.length; i++) {
                str += '&nbsp;&nbsp;<a href="javascript:void(0)" onclick="' + (msg[i].BtnCode + "Product") + '()"><span class="' + msg[i].Icon + '">&nbsp;</span>' + msg[i].ButtonName + '</a>';
            }
            $('.btabs_product').append(str);

            var str = '';
            for (var i = 0; i < msg.length; i++) {
                str += '&nbsp;&nbsp;<a href="javascript:void(0)" onclick="' + (msg[i].BtnCode + "Pack") + '()"><span class="' + msg[i].Icon + '">&nbsp;</span>' + msg[i].ButtonName + '</a>';
            }
            $('.btabs_pack').append(str);
        }
    })
});