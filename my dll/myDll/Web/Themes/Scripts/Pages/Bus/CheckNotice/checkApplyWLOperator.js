$(function () {

    initData();
    initSubList();
    if (isBrowse == 'true') {
        $("#send_span").hide();
        $("#btnOperate").hide();
        $("input").attr("disabled", "disabled");
        $("#btabs").html('选择的货代：<input class="easyui-textbox" style="width: 100px;height:25px;" type="text" name="saleContract" id="saleContract" value="' + applyInfo.distributeMan_ED + '" readonly="readonly" disabled="disabled"/>');
    } else {
        $("#Fieldset1").hide();
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
    var customMan = $("#customMan").combogrid('getValue');
    var sendEmail = $("#sendEmail").attr("checked");
    var sendEmail1 = $("#sendEmail");
    var check = sendEmail1[0].checked;

    var checkNoticeNumber = $('#checkNoticeNumber').textbox('getValue');
    if ('已分配' == applyInfo.distributeStatus) {
        alert("已经分配");
    } else {
        $.messager.confirm("确认", "请检查所填信息正确无误，提交后不可修改！", function (r) {
            if (r) {
                if ("" != customMan) {
                    $.ajax({
                        cache: true,
                        type: "POST",
                        url: "/ashx/CheckNotice/CheckData.ashx?module=receiveSubmitApply&checkNoticeNumber=" + checkNoticeNumber + "&customMan=" + customMan + "&sendEmail=" + check,
                        async: false,
                        error: function (data) {
                            alert("操作失败");
                        },
                        success: function (data) {
                            var result = JSON.parse(data);
                            if (result.status === 'T') {
                                alert("操作成功");
                                $.messager.alert('提示', result.msg, 'info');
                                top.selectAndRefreshTab("海运订舱分配");
                            } else {
                                //alert(result.msg);
                                $.messager.alert('提示', result.msg, 'info');
                            }
                        }
                    });
                } else {
                    alert("请选择报关行");
                }
            }
        });
     }
    
}

//退回
function back() {
    if ('已分配' == applyInfo.distributeStatus) {
        alert("已经分配，不能退回！");
    } else {
        var checkNoticeNumber = $('#checkNoticeNumber').textbox('getValue');
        $.messager.confirm("确认", "确定退回该订舱申请吗？", function (r) {
            if (r) {
                $.ajax({
                    cache: true,
                    type: "POST",
                    url: "/ashx/CheckNotice/CheckData.ashx?module=backApply&checkNoticeNumber=" + checkNoticeNumber,
                    async: false,
                    error: function (data) {
                        alert("操作失败");
                    },
                    success: function (data) {
                        var result = JSON.parse(data);
                        if (result.status === 'T') {
                            alert("操作成功");
                            top.selectAndRefreshTab("海运订舱分配");
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