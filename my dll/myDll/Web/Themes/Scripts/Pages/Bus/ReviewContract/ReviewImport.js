$(function () {
    //绑定数据
    bindUI();
    //绑定产品列表
    if (isContactEdit == "true") {
        popContactProductSelect();
    }
    else {
        //绑定产品
        bindProduct();
    }
    //tabs选中事件
    selectTabs();
    //查看上传附件
    checkAttachUp();
    setTimeout(function () {
        var str = $('#upMationDetails').textbox('getValue');
        if (str.length > 0) {
            $('#upMationDetails').textbox('setValue', str);
            $('#upMationDetails').textbox('setText', str.split('/')[6]);
        }
    }, 1000);
})
//查看上传附件
function checkAttachUp() {
    $('#upMationDetails').textbox({
        onClickButton: function () {
            var filepath = $("#upMationDetails").textbox('getValue');

            window.top.addNewTab("查看", filepath, '');
        }
    });
}
//绑定数据
function bindUI() {
    //控制废弃中止合同原因显示
    $("#abandonResonTr").hide();
    if (isAbandon == "true") {
        $("#abandonResonTr").show();
    }
    initFile(); //初始化文件路径
    checkAttachUp1();//查看上传文件
    $("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadReviewApp&contractNo=' + $("#contractNo").val());
    $('input').attr('disabled', true);
    $("#outsideTr").hide();
    if (isoutside == "true") {
        $("#outsideTr").show();
    }
}
//绑定产品
function bindProduct() {
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + $("#contractNo").val(),
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100 },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, },
                    { field: 'priceUnit', title: '价格单位', width: 100 },
                    { field: 'amount', title: '金额', width: 100, },
                    { field: 'spec', title: '产品规格', width: 100 },
                    { field: 'pallet', title: '最小包装', width: 100 },
                    { field: 'unit', title: '包装单位', width: 100 },
                    { field: 'packdes', title: '包装', width: 100 },
                    { field: 'palletrequire', title: '托盘要求', width: 100, },
                    { field: 'ifcheck', title: '是否商检', width: 100, },
                    { field: 'ifplace', title: '是否产地证', width: 100, }
        ]]
    });
}

//tabs选中事件
function selectTabs() {
    $("#isMeetReviewTr").hide();//是否开会审核
    //隐藏开会审核列表
    $("#meetReviewTable").hide();
    
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
            if (title == "货物条款") {
                $('input').attr('disabled', true);
            }
            if (title == "查看合同文本") {
                $('input').attr('disabled', false);
                //获取审核状态，如果为业务直线审核说明是合同管理员审核，显示开会审核节点
                if (status == status5) {//待合同管理员审核

                    $("#isMeetReviewTr").show();//是否开会审核
                }
                if (status != status1 && status != status4 && status != status5) {//新建,待直线经理审核,待合同管理员审核
                    var isMeet = $("#isMeet").val();
                    if (isMeet == "1") {//已经开会审核
                        $("#meetReviewTable").show();//显示开会审核详细信息
                        $("#reviewmeetsubTd").hide();
                        $('#meetReviewTable input').attr('disabled', true);
                    }
                }
                //根据radio选择控制显示开会审核详情
                //selectRadio();
             
                var language = $("#language").textbox('getValue');
                var templateno = $("#templateno").val();
                $("#previewFormFrame").attr('src', '/Bus/ContractCategory/contractTemplate.aspx?language=' + language + '&contractNo=' + $("#contractNo").val() + '&tableName=Econtract');
            }
            if (title == "审核日志") {
                $("#reviewTable").datagrid({
                    height: 300,
                    width: 700,
                    nowrap: true,
                    fitColumns: true,
                    striped: true,
                    collapsible: true,
                    pageList: [10, 15, 30],
                    singleSelect: true,
                    idField: 'contractNo',
                    url: '/ashx/Contract/contractData.ashx?module=getReviewData&contractNo=' + $("#contractNo").val(),
                    columns: [[
                    { field: 'reviewstatus', title: '审核节点', width: '100px', editor: 'text' },
                    { field: 'status', title: '状态', width: '100px', editor: 'text' },
                    { field: 'reviewdate', title: '审核时间', width: '100px', editor: 'text' },
                    { field: 'reviewlog', title: '审核日志', width: '100px', editor: 'text' },
                     { field: 'reviewman', title: '审核人', width: '100px', editor: 'text' },
                    ]],
                    pagination: false,
                })
            }
            if (title == "合同箱单") {

                $("#packinglist").attr('src', '/InvoiceAndPacking/InvoiceOrPacking.aspx?type=other&no=' + $("#contractNo").val());
            }
            if (title == "合同发票") {

                $("#Invoice").attr('src', '/InvoiceAndPacking/InvoiceOrPacking.aspx?type=other&no=' + $("#contractNo").val());

            }
        }
    })
}
//出口进口审核提交
function reviewSub() {
    var data = $("input[name='reviewstatus']:checked").val();
    var log = $("#reviewlog").textbox('getValue');
    if (data == null) {
        $.messager.alert("提醒", "审核不能为空");
        return;
    }
    if (log == "") {
        $.messager.alert("提醒", "审核日志不能为空");
        return;
    }
    var status = $("#status").val();

    //废弃中止合同审核提交
    if (isAbandon == "true") {
        $.post("/ashx/Contract/reviewContractOperater.ashx?module=reviewAbandonData" + "&isapprove=" + data + "&log=" + log + "&contractNo=" + $("#contractNo").val() + "&contractStatus=" + status, function (data) {
            if (data.sucess == "1") {
                $.messager.alert("提醒", "审核成功");
                window.top.selectAndRefreshTab('废弃出口合同审批');
                //$("#reviewTable").datagrid("reload");
            }
            else {
                $.messager.alert("提醒", data.warnmsg);
            }
        });
    }
    else {
        $.post("/ashx/Contract/reviewContractOperater.ashx?module=reviewData" + "&isapprove=" + data + "&log=" + log + "&contractNo=" + $("#contractNo").val() + "&contractStatus=" + status, function (data) {
            if (data.sucess == "1") {
                $.messager.alert("提醒", "审核成功");
                window.top.selectAndRefreshTab('进口合同审批');
                //$("#reviewTable").datagrid("reload");
            }
            else {
                $.messager.alert("提醒", data.warnmsg);
            }
        });
    }
}
//开会审核提交
function reviewMeetSub() {

    //获取是否通过
    var meetreviewstatus = $("input[name='reviewstatus']:checked").val();
    //获取会议记录
    var meetlog = $("#meetlog").textbox('getValue');
    //获取合同现所处审核状态
    var status = status;
    //获取附件文件路径
    var filepath = $("#meetReviewFile").val();
    //获取审核日志
    //var reviewmeetlog = $("#reviewmeetlog").textbox('getValue');
    //获取审核时间
    //获取是否开会审核
    var isMeetReview = $("input[name='isMeetReview']:checked").val();
    var meettime = $("#meettime").datetimebox('getValue');
    if (meetreviewstatus == null) {
        $.messager.alert("提醒", "审核不能为空");
        return;
    }
    if (meetlog == "") {
        $.messager.alert("提醒", "会议记录不能为空");
        return;
    }
    //if (reviewmeetlog == "") {
    //    $.messager.alert("提醒", "审核日志不能为空");
    //    return;
    //}
    if (meettime == "" || meettime == null) {
        $.messager.alert("提醒", "请选择开会时间");
        return;
    }
    var contractNo = $("#contractNo").textbox('getValue');
    $.post("/ashx/Contract/reviewContractOperater.ashx?module=reviewMeetData" + "&isapprove=" + meetreviewstatus, {
        meetlog: meetlog, contractStatus: status, filepath: filepath, meettime: meettime, isMeetReview: isMeetReview
        , contractNo: contractNo
    }, function (data) {
        if (data.sucess == "1") {
            $.messager.alert("提醒", "审核成功");
            window.top.selectAndRefreshTab('进口合同审批');
            //$("#reviewTable").datagrid("reload");
        }
        else {
            $.messager.alert("提醒", data.warnmsg);
        }
    });

}

//radio选中事件
function selectRadio() {
    //$(":radio").click(function () {
    //    if ($("input[name='reviewstatus']:checked").val() == "开会审核") {
    //        //隐藏出口进口审核提交行
    //        $("#eximportReviewLogTr").hide();
    //        //显示开会审核列表
    //        $("#meetReviewTable").show();
    //    }
    //    else {
    //        //显示出口进口审核提交行
    //        $("#eximportReviewLogTr").show();
    //        //隐藏开会审核列表
    //        $("#meetReviewTable").hide();
    //    }

    //})
}

//文件上传
function btnInfoUpload() {
    $("#form1").ajaxSubmit({
        url: "/ashx/Contract/reviewContractData.ashx?module=uploadFile",
        type: "text",
        success: function (data) {
            if (data == "error") {
                $.messager.alert("错误：", "上传失败");
            }
            else {
                alert("上传成功");
                var str = data.split(':');
                $("#upMationDetails").textbox('setText',str[0]);
                $("#upMationDetails").text('setValue',str[1]);
                $("#meetReviewFile").val(str[0]);
            }
        }
    });
}
//a标签点击查看文件
function viewFile() {

    var filepath = $("#meetReviewFile").val();
    window.top.addNewTab("查看", filepath, '');
}
//是否开会审核checkbox点击事件
function isMeetRevCheck() {
    if ($("input[name='isMeetReview']:checked").val() == "1") {
        //隐藏出口进口审核提交行
        $("#subTr").hide();
        //显示开会审核列表
        $("#meetReviewTable").show();
    }
    else {
        //显示出口进口审核提交行
        $("#isMeetReviewTr").show();
        $("#subTr").show();
        //隐藏开会审核列表
        $("#meetReviewTable").hide();
    }
}
//显示开会审核详情
function showMeetRevDetails(contractNo) {

}
//弹出关联产品选择框,根据合同号加载产品
function popContactProductSelect() {

    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + $("#contractNo").val(),
        //width: document.documentElement.clientWidth,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        fitColumns: true,
        columns: [[

		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox' } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'amountfloat', title: '价格增减', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceAdd', title: '增减后价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100 },
                    { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'spec', title: '产品规格', width: 100 },
                    { field: 'pallet', title: '最小包装', width: 100 },
                    { field: 'unit', title: '包装单位', width: 100 },
                    { field: 'packdes', title: '包装', width: 100 },
                    { field: 'skinWeight', title: '皮重', width: 100 },
                    { field: 'packagesNumber', title: '件数', width: 100, editor: { type: 'numberbox', options: { precision: 0 } } },
                    { field: 'palletrequire', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
                    { field: 'ifplace', title: '是否产地证', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },


        ]]
    });
}
//点击查看文件
function checkAttachUp() {
    $('#upMationDetails').textbox({
        onClickButton: function () {
            var filepath = $("#upMationDetails").textbox('getValue');
            window.top.addNewTab("查看", filepath, '');
        }
    });
}
function checkAttachUp1() {
    $('#upMationDetails1').textbox({
        onClickButton: function () {
            var filepath = $("#upMationDetails1").textbox('getValue');
            window.top.addNewTab("查看", filepath, '');
        }
    });
}
//初始化文件路径
function initFile() {
    $("#upMationDetails1").textbox('setText', infoText);
    $("#upMationDetails1").textbox('setValue', infoValue);
}

