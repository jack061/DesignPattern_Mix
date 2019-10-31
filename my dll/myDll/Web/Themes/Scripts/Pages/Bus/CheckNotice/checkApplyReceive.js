$(function () {

    initData();
    initSubList();
    if (isBrowse == 'true') {
        $("#btabs").hide();
        $("input").attr("disabled", "disabled");
    }

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

//接收提交
function receiveSubmit() {
     var checkNoticeNumber = $('#checkNoticeNumber').textbox('getValue');
     if ('已接收' == applyInfo.confirmStatus) {
         alert("已经接收！");
     } else {
         var checkNoticeNumber = $('#checkNoticeNumber').textbox('getValue');
         $.messager.confirm("确认", "确定接收该订舱申请吗？", function (r) {
             if (r) {
                 $.ajax({
                     cache: true,
                     type: "POST",
                     url: "/ashx/CheckNotice/CheckData.ashx?module=receiveApply&checkNoticeNumber=" + checkNoticeNumber,
                     async: false,
                     error: function (data) {
                         alert("操作失败");
                     },
                     success: function (data) {
                         var result = JSON.parse(data);
                         if (result.status === 'T') {
                             alert("操作成功");
                             top.selectAndRefreshTab("海运订舱通知");
                             window.top.closeTab();
                         } else {
                             alert(result.msg);
                         }
                     }
                 });
             }
         });
     }
}

//退回
function back() {
    if ('已确认' == applyInfo.confirmStatus) {
        alert("已经确认，不能退回！");
    } else {
        var checkNoticeNumber = $('#checkNoticeNumber').textbox('getValue');
        $.messager.confirm("确认", "确定退回该订舱申请吗？", function (r) {
            if (r) {
                $.ajax({
                    cache: true,
                    type: "POST",
                    url: "/ashx/CheckNotice/CheckData.ashx?module=backConfirm&checkNoticeNumber=" + checkNoticeNumber,
                    async: false,
                    error: function (data) {
                        alert("操作失败");
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.status === 'T') {
                            alert("操作成功");
                            top.selectAndRefreshTab("海运订舱通知");
                            window.top.closeTab();
                        } else {
                            alert(result.msg);
                        }
                    }
                });
            }
        });
    }
}

//取消
function cancel() {

    //关闭当前tab
    window.top.closeTab();
}