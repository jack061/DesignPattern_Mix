
$(function () {
    bindUI();
    initReviewInfo();
    initContractInfo();//初始化合同信息
    selectTabs();
    checkAttachUp();
});
//取消
var undo = function () {

    //关闭当前tab
    window.top.closeTab();
}

var bindUI = function () {
    $('input').attr("disabled", true);
}

//查询产品
function SearchContract() {
    pcode = $('#pcode').textbox('getText');

    name = $('#pname').textbox('getText');
    para = {};
    para.pcode = pcode;
    para.name = name;
    $('#productlist').datagrid('load', para);

}

//tabs选中事件
function selectTabs() {
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
            $("#isMeetReviewTr").hide();//是否开会审核
            //隐藏开会审核列表
            $("#meetReviewTable").hide();
            var contractNo = $("#contractNo").textbox('getValue');
            switch (title) {
                case "货物条款":
                    $('input').attr('disabled', true);
                    break;
                case "查看结算单文本":
                    //获取审核状态，如果为业务直线审核说明是合同管理员审核，显示开会审核节点
                    $('input').attr('disabled', false);
                    if (status == status5) {//待合同管理员审核
                        $("#isMeetReviewTr").show();//是否开会审核
                    }
                    if (status != status1 && status != status4 && status != status5) {//新建,待直线经理审核,待合同管理员审核
                        
                        var isMeet = $("#isMeet").val();
                        if (isMeet == "1") {//已经开会审核
                            $("#meetReviewTable").show();//显示开会审核详细信息
                            $("#reviewmeetsubTd").hide();
                            $("#meetReviewTable input").attr('disabled', true);
                        }
                    }
                  
                    $("#previewFormFrame").attr('src', '/Bus/ContractCategory/internalClearingTemplate.aspx?contractNo=' + contractNo);
                    break;
                case "审核日志":
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
                        url: '/ashx/Contract/contractData.ashx?module=GetServiceReviewList&contractNo=' + $("#contractNo").val(),
                        columns: [[
                        { field: 'reviewstatus', title: '审核节点', width: '100px', editor: 'text' },
                        { field: 'status', title: '状态', width: '100px', editor: 'text' },
                        { field: 'reviewdate', title: '审核时间', width: '100px', editor: 'text' },
                        { field: 'reviewlog', title: '审核日志', width: '100px', editor: 'text' },
                         { field: 'reviewman', title: '审核人', width: '100px', editor: 'text' },
                        ]],
                        pagination: false,
                    })
                    break;

                default:
                    $.messager.alert("提醒","未找到页面");
                    break;
                 

            }

        }
    })
}
//加载内结产品表
function loadInternalProdunt() {
    console.time('产品列表');
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=GetInternalProductList&contractNo=' + contractInfo.contractNo,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		          { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '合同数量', width: 100 },
                    { field: 'interingQuantity', title: '已内结数量', width: 100 },
                    { field: 'unInterQuantity', title: '未内结数量', width: 100 },
                    { field: 'interquantity', title: '本次内结数量', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100, },
                    { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'rate', title: '税率', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'SAPNumber', title: 'SAP订单类型和编号', width: 100, editor: { type: 'textbox', } }
           
        ]],
        onClickCell: function (index, row, changes) {
            $(this).datagrid('beginEdit', index);
            var editors = $(this).datagrid('getEditors', index);
            var quantity = editors[0];
            var price = editors[1];
            var amount = editors[2];
            $(price.target).numberbox({
                onChange: function (n, o) {
                    var finamount = parseFloat(quantity.target.val()) * (parseFloat(price.target.val()));//计算后总金额
                    $(amount.target).numberbox('setValue', finamount);
                }
            })
        },
        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
    console.timeEnd('产品列表');
}
//初始化合同信息
function initContractInfo() {
    $("#contractNo").textbox('setValue', contractInfo.contractNo);
    $("#signedplace").textbox('setValue', contractInfo.signedplace);
    $("#signedtime").datebox('setValue', contractInfo.signedtime);
    $("#buyer").combobox('setValue', contractInfo.buyer);
    $("#buyercode").val(contractInfo.buyercode);
    $("#sellercode").val(contractInfo.sellercode);
    $("#seller").combobox('setValue', contractInfo.seller);
    $("#simpleSeller").textbox('setValue', contractInfo.simpleSeller);
    $("#simpleBuyer").textbox('setValue', contractInfo.simpleBuyer);
    $("#purchaseCode").textbox('setValue', contractInfo.purchaseCode);
    $("#adminReview").combobox('setValue', contractInfo.adminReview);
    $("#salesmanCode").combobox('setValue', contractInfo.salesmanCode);
    $("#businessclass").combobox('setValue', contractInfo.businessclass);
    $("#itemProName").textbox('setValue', contractInfo.itemProName);
    $("#createTableName").datebox('setValue', contractInfo.createTableName);
    $("#Organizer").textbox('setValue', contractInfo.Organizer);
    $("#startDate").datebox('setValue', contractInfo.startDate);
    $("#endDate").datebox('setValue', contractInfo.endDate);
    $("#status").val(contractInfo.status);
    $("#adminReviewNumber").val(contractInfo.adminReviewNumber);
    loadInternalProdunt();

}
//初始化审核信息
function initReviewInfo() {
 
    //$("#reviewlog").textbox('setValue', reviewInfo.reviewlog);
    $("#meetlog").textbox('setValue', reviewInfo.meetlog);
    $("#meettime").datetimebox('setValue', reviewInfo.meettime);
    $("#meetReviewFile").val(reviewInfo.filepath);
    $("#isMeet").val(reviewInfo.isMeetReview);
    $("#upMationDetails").textbox('setText', reviewInfo.filepath);
    $("#upMationDetails").text('setValue', reviewInfo.filepath);
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
//审核提交
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
    var status = status;
    $.post("/ashx/Contract/reviewContractOperater.ashx?module=reviewInternalData" + "&isapprove=" + data + "&log=" + log + "&contractNo=" + $("#contractNo").val() + "&contractStatus=" + status, function (data) {
        if (data.sucess == "1") {
            $.messager.alert("提醒", "审核成功");
            window.top.selectAndRefreshTab('管理合同审批');
        }
        else {
            $.messager.alert("提醒", data.warnmsg);
        }
    });
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
    var meettime = $("#meettime").datetimebox('getValue');
    //获取是否开会审核
    var isMeetReview = $("input[name='isMeetReview']:checked").val();
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
    $.post("/ashx/Contract/reviewContractOperater.ashx?module=reviewInternalMeetData" + "&isapprove=" + meetreviewstatus, {
        meetlog: meetlog, contractStatus: status, filepath: filepath, meettime: meettime, isMeetReview: isMeetReview
        , contractNo: contractNo
    }, function (data) {
        if (data.sucess == "1") {
            $.messager.alert("提醒", "审核成功");
            window.top.selectAndRefreshTab('内部结算单审批');
            //$("#reviewTable").datagrid("reload");
        }
        else {
            $.messager.alert("提醒", data.warnmsg);
        }
    });
}
//文件上传
function btnInfoUpload() {
    $("#form1").ajaxSubmit({
        url: "/ashx/Contract/reviewContractData.ashx?module=uploadFile",
        type: "post",
        success: function (data) {
            if (data == "error") {
                $.messager.alert("错误：", "上传失败");
            }
            else {
                alert("上传成功");
                var str = data.split(':');
                $("#upMationDetails").textbox('setText', str[0]);
                $("#upMationDetails").text('setValue', str[1]);
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
//点击查看文件
function checkAttachUp() {
    $('#upMationDetails').textbox({
        onClickButton: function () {
            var filepath = $("#upMationDetails").textbox('getValue');
            window.top.addNewTab("查看", filepath, '');
        }
    });
}