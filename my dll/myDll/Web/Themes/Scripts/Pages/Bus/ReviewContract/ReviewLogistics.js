$(function () {
    //绑定数据
    bindUI();
  
    //tabs选中事件
    selectTabs();
  
})
//绑定数据
function bindUI() {
    //为table填充数据
    $('#Table1').form('load', htdata);
    $('#templateTextTable').form('load', htdata);
    $('#tableNameTable').form('load', htdata);
    $('input').attr('disabled', true);
    loadTemplateItesByEdit();
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
            if (title == "合同预览") {
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
                $('input').attr('disabled', false);
                var language = "中文";
                $("#previewFormFrame").attr('src', '/Bus/ContractCategory/previewLogisticsContract.aspx?language=' + language + '&logisticsContractNo=' + $("#logisticsContractNo").val());
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
                    url: '/ashx/Contract/contractData.ashx?module=getReviewData&contractNo=' + $("#logisticsContractNo").val(),
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
    
        }
    })
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
    $.post("/ashx/Contract/reviewContractOperater.ashx?module=reviewLogisticsData" + "&isapprove=" + data + "&log=" + log + "&contractNo=" + $("#logisticsContractNo").val() + "&contractStatus=" + status, function (data) {
        if (data.sucess == "1") {
            $.messager.alert("提醒", "审核成功");
            $("#reviewTable").datagrid("reload");
        }
        else {
            $.messager.alert("提醒", data.warnmsg);
        }
    });
}
//开会审核提交
function reviewMeetSub() {
    //获取是否通过
    var meetreviewstatus = $("input[name='meetreviewstatus']:checked").val();
    //获取会议记录
    var meetlog = $("#meetlog").textbox('getValue');
    //获取合同现所处审核状态
    var status = $("#status").textbox('getValue');
    //获取附件文件路径
    var filepath = $("#meetReviewFile").val();
    //获取审核日志
    var reviewmeetlog = $("#reviewmeetlog").textbox('getValue');
    //获取审核时间
    var meettime = $("#reviewtime").datebox('getValue');
    var contractNo = $("#contractNo").textbox('getValue');
    $.post("/ashx/Contract/reviewContractOperater.ashx?module=reviewMeetData" + "&isapprove=" + meetreviewstatus, {
        meetlog: meetlog, contractStatus: status, filepath: filepath, reviewmeetlog: reviewmeetlog, meettime: meettime
        , contractNo: contractNo
    }, function (data) {
        if (data.sucess == "1") {
            $.messager.alert("提醒", "审核成功");
            $("#reviewTable").datagrid("reload");
        }
        else {
            $.messager.alert("提醒", data.warnmsg);
        }
    });

}
//radio选中事件
function selectRadio() {
    $(":radio").click(function () {
        if ($("input[name='reviewstatus']:checked").val() == "开会审核") {
            //隐藏出口进口审核提交行
            $("#eximportReviewLogTr").hide();
            //显示开会审核列表
            $("#meetReviewTable").show();
        }
        else {
            //显示出口进口审核提交行
            $("#eximportReviewLogTr").show();
            //隐藏开会审核列表
            $("#meetReviewTable").hide();
        }

    })
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
                $("#upMationDetails").val(str[0]);
                alert(str[0]);
                $("#upMationDetails").text(str[1]);
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
//修改时根据合同号填充条款表格
function loadTemplateItesByEdit() {
    $("#templateItemDiv").css('display', 'block');
    var item1 = $("#item1").textbox('getValue');
    var item2 = $("#item2").textbox('getValue');
    var item3 = $("#item3").textbox('getValue');
    var item4 = $("#item4").textbox('getValue');
    var item5 = $("#item5").textbox('getValue');
    var item6 = $("#item6").textbox('getValue');
    var item7 = $("#item7").textbox('getValue');
    var item8 = $("#item8").textbox('getValue');
    $('#templateItem').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=logisticsItems&logisticsContractNo=' + $("#logisticsContractNo").val(),
        singleSelect: true,
        pagination: false,
        sortName: 'id',
        sortOrder: 'asc',
        columns: [[
                    { field: 'item1', title: item1, width: 150, editor: 'textbox' },
                    { field: 'item2', title: item2, width: 150, editor: 'textbox' },
                    { field: 'item3', title: item3, width: 150, editor: 'textbox' },
                    { field: 'item4', title: item4, width: 150, editor: 'textbox' },
                     { field: 'item5', title: item5, width: 150, editor: 'textbox' },
                    { field: 'item6', title: item6, width: 150, editor: 'textbox' },
                    { field: 'item7', title: item7, width: 150, editor: 'textbox' },
                    { field: 'item8', title: item8, width: 150, editor: 'textbox' }
        ]],


    });

}
