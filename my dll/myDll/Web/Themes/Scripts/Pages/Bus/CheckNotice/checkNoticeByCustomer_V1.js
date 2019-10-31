
var fileColumName = "";
$(function () {

    initData();
    initSubList();
    if (isBrowse == 'true') {
        $("#btabs").hide();
        $("input").attr("disabled", "disabled");
    }

    //查看图片
    $("#dlg").dialog("close"); //init
    $("#btnInfoUpload").click(function () {
        uploadFile();
    });

    $('#boxorderName').textbox({
        onClickButton: function () {
            var url = $('#boxorderUrl').val();
            if (url != '' && url != '&nbsp;') {
                window.open(url);
            } else {
                alert('请先上传文件！');
            }
        }
    });
    $('#bhorderName').textbox({
        onClickButton: function () {
            var url = $('#bhorderUrl').val();
            if (url != '' && url != '&nbsp;') {
                window.open(url);
            } else {
                alert('请先上传文件！');
            }
        }
        
    });
    $('#noticeorderName').textbox({
        onClickButton: function () {
            var url = $('#noticeorderUrl').val();
            if (url != '' && url != '&nbsp;') {
                window.open(url);
            } else {
                alert('请先上传文件！');
            }
        }
    });



});

function initData() {
    $('#form1').form('load', applyInfo);
  }
function initSubList() {
    $('#htcplist').datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        sortName: 'checkNoticeNumber',
        sortOrder: 'desc',
        url: '/ashx/CheckNotice/CheckData.ashx?module=getCheckNoticeSubList&checkNoticeNumber=' + applyNo,
        columns: [[
            { field: 'mass', title: '箱唛', width: '150px' },
            { field: 'pname', title: '货物名称', width: '200px' },
            { field: 'quantity', title: '数量', width: '150px' },
            { field: 'qunit', title: '单位', width: '150px' },
            { field: 'packing', title: '包装', width: '200px' },
            { field: 'packspec', title: '重量', width: '100px' },
            { field: 'volume', title: '体积', width: '120px' }
            ]],
        pagination: true
    });
}

//保存
function save() {
    $.messager.confirm("确认", "请检查所填信息正确无误，提交后不可修改！", function (r) {
        if (r) {
            if ($("#form1").form('validate')) {
                var rrdata = SaveDataToDB();

                var result = JSON.parse(rrdata);
                if ("T" == result.status) {
                    $.messager.alert("提醒", "保存成功");
                    top.selectAndRefreshTab("海运订舱通知");
                    window.top.closeTab();
                } else {
                    $.messager.alert("提醒", result.msg);
                }
            } else {
                return false;
            }
        }
    });


}
//保存方法
var SaveDataToDB = function () {
    var shipProduct = $("#htcplist").datagrid("getRows");
    var shipProductJson = JSON.stringify(shipProduct);
    $('#shipProduct').attr('value', shipProductJson);

    var retdata = {};
    var action = "updateNotice";
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/CheckNotice/CheckData.ashx?module=' + action + '&applyNo=' + applyNo + '&contractNo=' + contractNo + "&createDateTag=" + createDateTag,
        data: $('#form1').serialize(),

        async: false,
        error: function (data) {
            retdata = data;
        },
        success: function (data) {

            retdata = data;
        }
    });
    return retdata;
}

//取消
function cancel() {

    //关闭当前tab
    window.top.closeTab();
}

//打开长传界面
function uploadFile_open(name) {
    fileColumName = name;
    $("#dlg").dialog("open"); //打开
}

//文件上传
function uploadFile() {
    $("#form_up").ajaxSubmit({
        url: "/ashx/CheckNotice/CheckData.ashx?module=upload",
        type: "post",
        success: function (data) {
            if (data == "error") {
                $.messager.alert("错误：", "上传失败");
            }
            else {
                $.messager.alert("提醒：", "上传成功");
                var str = data.split(':');
                $("#" + fileColumName + "Url").val(str[0]);//str[0]-文件路径
                $("#" + fileColumName + "Name").textbox('setText', str[1]); //str[1]-文件名称
                $("input[name='" + fileColumName + "Name']").val(str[1]);

            }
        }
    })
}