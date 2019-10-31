$(function () {
    doSearch();
})
//获取价格有效期
function getValidityDate(row, value) {
    if (value == "" || value == null) {
        return row.pvalidity;
    }
    else {
        return value;
    }
}
//新增
function add() {
    window.top.addNewTab("进口合同-新增", '/Bus/ContractCategory/chooseImportContract.aspx', '');
}
//编辑
function edit() {
    var row = $("#maingrid").datagrid('getSelected');

    var isContact = "";
    var isContactEdit = "";
    var isFrameAttachEdit = "";
    if (!(row.status == status1 || row.status == status2)) {//新建和退回
     
        if (isConManage != "True") {
            $.messager.alert("提醒", "非" + status1 + status2 + "状态下不可修改");
            return;
        }

    }
    if (row.purchaseCode != null && row.purchaseCode != "") {
        isContactEdit = "true";
    }
    if (row.frameAttachContractNo != null && row.frameAttachContractNo != "") {
        isFrameAttachEdit = "true";
    }
    if (row != undefined && row.outTag == outTag) {
        window.top.addNewTab("外部文本合同-修改", '/Bus/ContractCategory/exportOutsideContractForm.aspx?contractNo=' + row.contractNo + '&isEdit=true&isConManage=' + isConManage, '');
    }
    else if (row != undefined && (row.contractTag == contractTag2 || row.contractTag == contractTag3)) {//发货通知
        window.top.addNewTab("数量确认函-修改", '/Bus/ContractCategory/addImportSendNotice.aspx?contractNo=' + row.contractNo + '&isEdit=true&isConManage=' + isConManage, '');
    }
    else {
        window.top.addNewTab("进口合同-修改", '/Bus/ContractCategory/importContractTest.aspx?contractNo=' + row.contractNo + "&isContactEdit=" + isContactEdit + "&isFrameAttachEdit=" + isFrameAttachEdit + '&frameAttachContractNo=' + row.frameAttachContractNo + '&isEdit=true&isConManage=' + isConManage, '');
    }
}
//查看
function browse() {
    var row = $("#maingrid").datagrid('getSelected');

    var isContact = "";
    var isContactEdit = "";
    var isFrameAttachEdit = "";
    if (row.purchaseCode != null && row.purchaseCode != "") {
        isContactEdit = "true";
    }
    if (row.frameAttachContractNo != null && row.frameAttachContractNo != "") {
        isFrameAttachEdit = "true";
    }
    if (row != undefined && row.outTag == outTag) {
        window.top.addNewTab("外部文本合同-查看", '/Bus/ContractCategory/exportOutsideContractForm.aspx?contractNo=' + row.contractNo + '&isBrowse=true', '');
    }
    else if (row != undefined && (row.contractTag == contractTag2 || row.contractTag == contractTag3)) {//发货通知
        window.top.addNewTab("数量确认函-查看", '/Bus/ContractCategory/addImportSendNotice.aspx?contractNo=' + row.contractNo + '&isBrowse=true', '');
    }
    else {
        window.top.addNewTab("进口合同-查看", '/Bus/ContractCategory/importContractTest.aspx?contractNo=' + row.contractNo + "&isContactEdit=" + isContactEdit + "&isFrameAttachEdit=" + isFrameAttachEdit + "&isBrowse=true", '');
    }
}
//预览
function preview() {
    var no = '';
    var row = $("#maingrid").datagrid('getSelected');
    no = row.contractNo;
    if (row != undefined && (row.contractTag == contractTag2 || row.contractTag == contractTag3)) {
        window.top.addNewTab("数量确认函-预览", '/Bus/ContractCategory/sendOutNoticeTemplate.aspx?language=' + row.language + '&contractNo=' + no + '&tableName=Econtract', '');
    }
    else if (row != undefined) {
        window.top.addNewTab("进口合同-预览", '/Bus/ContractCategory/contractTemplate.aspx?language=' + row.language + '&contractno=' + no + '&tableName=Econtract', '');
    }
}
//查找
function doSearch() {
    var queryData = {};
    queryData.contractNo = $('#contractNo').textbox('getValue');
    queryData.signedtime_begin = $('#signedtime_begin').datebox('getValue');
    queryData.signedtime_end = $('#signedtime_end').datebox('getValue');
    queryData.businessclass = $('#businessclass').textbox('getValue');
    $('#maingrid').datagrid({ queryParams: queryData });
}
//删除
function del() {
    var row = $("#maingrid").datagrid('getSelected');
    no = row.contractNo;
    if (!(row.status == status1 || row.status == status2)) {//新建和退回
        $.messager.alert("提醒", "非" + status1 + status2 + "状态下不可删除");
        return;
    }
    $.messager.confirm('系统提示', '确认要删除么？', function (r) {
        if (r) {
            $.ajax({
                cache: true,
                type: "POST",
                url: '/ashx/Contract/contractOperater.ashx?module=deletecontract&contractNo=' + no,
                data: {},
                async: false,
                error: function (data) {
                    $.messager.alert('系统提示', '后台操作失败!', 'info');
                },
                success: function (data) {
                    if (data.sucess == 1) {
                        //$.messager.alert('系统提示', '删除成功!', 'info');
                        $("#maingrid").datagrid("reload", {});
                    }
                    else {
                        $.messager.alert('系统提示', '删除失败!' + data.errormsg, 'info');
                    }
                }
            });
        }
    });
}

//生成发票箱单方法
function other() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.contractNo;
        var language = row.language;
        var choice = "";
        if (language == "中文") {
            choice = "zh"
        }
        if (language == "中文-英文") {
            choice = "zheg"
        }
        if (language == "中文-俄文") {
            choice = "zhrs"
        }

        window.top.addTab('报关发票', '/InvoiceAndPacking/InvoiceOrPacking.aspx?type=invoice&no=' + no + "&language=" + choice + "&choice=" + choice, "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

function packing() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.contractNo;
        var language = row.language;
        var choice = "";
        if (language == "中文") {
            choice = "zh"
        }
        if (language == "中文-英文") {
            choice = "zheg"
        }
        if (language == "中文-俄文") {
            choice = "zhrs"
        }
        window.top.addTab('报关箱单', '/InvoiceAndPacking/InvoiceOrPacking.aspx?type=packing&no=' + no + "&language=" + choice + "&choice=" + choice, "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//查看合同审批表
function checkContractApproval() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.contractNo;
        window.top.addTab('查看合同审批表', '/Bus/Contract/ContractApproval.aspx?type=export&contractNo=' + no, "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//是否打印
function isPrint() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        $.post("/ashx/Contract/contractOperater.ashx?module=updatePrintStatus&isPrint=true&tableName=Econtract", { contractNo: row.contractNo }, function (data) {

            if (data.sucess == "1") {
                $.messager.alert('提示', '已打印', 'info');
                $("#maingrid").datagrid('reload');
            }
        });
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//是否下载
function isDownload() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        $.post("/ashx/Contract/contractOperater.ashx?module=updatePrintStatus&isDownload=true&tableName=Econtract", { contractNo: row.contractNo }, function (data) {

            if (data.sucess == "1") {
                $.messager.alert('提示', '已下载', 'info');
                $("#maingrid").datagrid('reload');
            }
        });
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//中止，执行一部分的合同，已经做了一部分的发货申请
function discontinue() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row.status != status10) {//审批通过
        $.messager.alert("提醒", "非" + status10 + "状态，不可中止");
        return;
    }
    if (row.sendOutStatus == null || row.sendOutStatus == 0) {//说明合同未发货执行，不能中止
        $.messager.alert("提醒", "合同未执行，不可中止");
        return;
    }
    $('#disContinueDiv').dialog({
        title: '中止原因',
        width: 400,
        height: 200,
        closed: false,
        cache: false,
        modal: true
    });
    $('#disContinueDiv').dialog('open');
   

}
//中止提交
function disContinueSub() {
    $.messager.confirm('系统提示', '确认要中止么？', function (r) {
        if (r) {
            var row = $("#maingrid").datagrid('getSelected');
            var disContinueArea = $("#disContinueArea").val();
            $.post("/ashx/Contract/loadOther.ashx?module=changeConDisContinue", { contractNo: row.contractNo, disContinueArea: disContinueArea }, function (data) {
                if (data == "ok") {
                    $('#disContinueDiv').dialog('close');
                    $("#maingrid").datagrid('reload');
                }
            }, 'text')
        }
    })
}
//中止取消
function closeDisContinue() {
    $('#disContinueDiv').dialog('close');
}
//废弃,没有执行的合同,即没有开始发货申请的合同
function abandon() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row.status != status10) {//审批通过
        $.messager.alert("提醒", "非" + status10 + "状态，不可废弃");
        return;
    }
    if (row.sendOutStatus == 1 || row.sendOutStatus == 2) {//说明合同已经发货一部分，只能中止，不能废弃
        $.messager.alert("提醒", "合同已执行，不可废弃");
        return;
    }
    $('#abandonDiv').dialog({
        title: '废弃原因',
        width: 400,
        height: 200,
        closed: false,
        cache: false,
        modal: true
    });
    $('#abandonDiv').dialog('open');


}
//废弃提交
function abandonSub() {
    $.messager.confirm('系统提示', '确认要废弃么？', function (r) {
        if (r) {
            var row = $("#maingrid").datagrid('getSelected'); 
            var abandonArea = $("#abandonArea").val();
            $.post("/ashx/Contract/loadOther.ashx?module=changeConAbandon", { contractNo: row.contractNo, abandonArea: abandonArea }, function (data) {
                if (data == "ok") {
                    $('#abandonDiv').dialog('close');
                    $("#maingrid").datagrid('reload');
                }
            }, 'text')
        }
    })
}
//废弃取消
function closeAbandon() {
    $('#abandonDiv').dialog('close');
}