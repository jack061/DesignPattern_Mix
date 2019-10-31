
$(document).ready(function () {

    $('#dd').dialog('close');
    //----初始化datagrid-----
    //待商检确认合同列表
    $('#tt').datagrid({
        rownumbers: false,
        sortName: 'applydate',
        sortOrder: 'desc',
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        url: '/ashx/Contract/contractData.ashx?module=htpagelistByHK&transport=海运', //获取要创建关联的合同
        columns: [[
            { field: 'ck', checkbox: true },
            {
                field: 'contactStatus', title: '状态', width: '60px', formatter: function (value, row, index) {
                    return value == "" ? '未处理' : value
                }
            },
            { field: 'createDateTag', title: '申请单号', width: '100px' },
            { field: 'applyman', title: '申请人', width: '60px' },
            { field: 'applydate', title: '申请时间', width: '80px' },
            { field: 'contractNo', title: '合同编号', width: '120px' },
            { field: 'purchaseCode', title: '关联合同号', width: '120px' },
            { field: 'iscustoms', title: '是否为报关合同', width: '100px' },
            { field: 'seller', title: '卖方', width: '150px' },
            { field: 'buyer', title: '买方', width: '150px' },
            { field: 'transport', title: '运输方式', width: '80px' },
            { field: 'pcode', title: '产品编号', width: '120px' },
            { field: 'pname', title: '产品名称', width: '100px' },
            { field: 'spec', title: '规格', width: '100px' },
            { field: 'quantity', title: '合同数量', width: '100px' },
            { field: 'sendQuantity', title: '申请发货数量', width: '100px' },
            { field: 'qunit', title: '数量单位', width: '100px' },
            { field: 'pallet', title: '最小包装', width: '100px' },
            { field: 'unit', title: '包装单位', width: '100px' },
            { field: 'packdes', title: '包装', width: '100px' },
        ]],
            toolbar: [
        {
                 text: '合同预览',
                 iconCls: 'icon-add',
                 handler: function () {
                     var row = $("#tt").datagrid('getSelected');
                     if (row != undefined) {
                         no = row.contractNo;
                         window.top.addNewTab("合同-预览", '/Bus/ContractCategory/contractTemplate.aspx?language=' + row.language + '&contractno=' + no + '&tableName=Econtract', '');
                     } else {
                         $.messager.alert('提示', '请选择一行数据！', 'info');
                     }
                 }
             },
             {
                 text: '创建关联合同',
                 iconCls: 'icon-add',
                 handler: function () {
                     var row = $("#tt").datagrid('getSelected');
                     if (row != undefined) {
                         if (row.contactStatus == '已创关联合同') { $.messager.alert("提示", "已创关联合同，不能再次创建！"); return; }
                         if (row.contactStatus == '已直接发货') { $.messager.alert("提示", "已直接发货，不能再次创建！"); return; }
                         no = row.contractNo;
                         window.top.addNewTab('生成关联合同', '/Bus/ContactContract/contactTrainForm.aspx?contractNo=' + no + '&createDateTag=' + row.createDateTag + '&transport=海运'+'&ifcheck='+row.ifcheck, "");
                     } else {
                         $.messager.alert('提示', '请选择一行数据！', 'info');
                     }
                 }
             }, {
                 text: '直接发货',
                 iconCls: 'icon-add',
                 handler: function () {
                     var row = $("#tt").datagrid('getSelected');
                     var contractNo = row.contractNo;
                     if (row != undefined) {
                         if (row.contactStatus == '已创关联合同' || row.contactStatus == '已直接发货') { $.messager.alert("提示", "请选择未处理合同！"); return; }
                         bindCombobox();
                         //checkbox 单选
                         $('input[type=checkbox]').bind('click', function () {
                             $('input[name=createContract]').not(this).attr("checked", false);
                         });
                         
                         $("#dd").dialog('open');
                         //校验商检合同
                         initInpsectInfo(contractNo);//初始化商检合同信息
                         //$.post("/ashx/Contract/contractOperater.ashx?module=sendimmediate",{ createDateTag: row.createDateTag }, function (data) {
                         //    if (data.sucess == 1) {
                         //        $.messager.alert("提示", "直接发货成功");
                         //        $('#tt').datagrid('reload');
                         //    }
                         //}, 'json')
                     }
                 }
             }, {
                 text: '查看关联合同',
                 iconCls: 'icon-add',
                 handler: function () {
                     var row = $("#tt").datagrid('getSelected');
                     if (row != undefined) {
                         no = row.contractNo;
                         if (row.contactStatus != '已创关联合同') { $.messager.alert("提示", "请选择已关联合同！"); return; }
                         //获取其关联合同的合同号
                         $.post("/ashx/Contract/contractData.ashx?module=getPurchaseCode", { contractNo: row.contractNo }, function (data) {
                             window.top.addNewTab("查看关联合同", '/Bus/ContractCategory/exportContractTest.aspx?contractNo=' + data + '&isBrowse=true', '');
                         }, 'text')

                     } else {
                         $.messager.alert('提示', '请选择一行数据！', 'info');
                     }
                 }
             }, {
                 text: '退回',
                 iconCls: 'icon-add',
                 handler: function () {
                     var row = $("#tt").datagrid('getSelected');
                     if (row != undefined) {
                         no = row.contractNo;
                         if (row.contactStatus == '已创关联合同'||row.contactStatus=='已直接发货') { $.messager.alert("提示", "请选择未处理合同！"); return; }
                         //获取其关联合同的合同号
                         $.post("/ashx/Contract/contractData.ashx?module=CancelApply", { contractNo: row.contractNo, createDateTag: row.createDateTag }, function (data) {
                           
                             if (data.result=="ok") {
                                 $.messager.alert('提示', '操作成功！', 'info');
                                 $("#tt").datagrid('reload');
                             }
                         }, 'json')

                     } else {
                         $.messager.alert('提示', '请选择一行数据！', 'info');
                     }
                 }
             }
        //{
        //    text: '作废申请',
        //    iconCls: 'icon-remove',
        //    handler: function () {
        //        var row = $("#tt").datagrid('getSelected');


        //        //只有未作废且没有关联合同的申请才能作废
        //        if (row.isCancel != 1 && row.purchaseCode != null) {
        //            $.messager.confirm("提示", "确认要作废该申请吗？", function (result) {
        //                if (result) {
        //                    $.ajax({
        //                        url: "/ashx/Contract/contractData.ashx?module=CancelApply&contractNo=" + row.contractNo,
        //                        dataType: 'json',
        //                        async: false,
        //                        cache: false,
        //                        success: function (data) {
        //                            if (data.result == 'ok') {
        //                                $.messager.alert("Info", "作废成功！");
        //                                $("#tt").datagrid("reload");
        //                            } else {
        //                                $.messager.alert("error", data.msg);
        //                            }
        //                        }
        //                    });
        //                }
        //            });
        //        } else {
        //            $.messager.alert("警告", "只有未作废且没有关联合同的申请才能作废！");
        //        }
        //    }
        //}
        ],
        pagination: true
    });

});

//弹出框编辑操作
var endEditing = function () {
    if (editIndex == undefined) { return true }
    else { return false; }
}

//查询操作
function SearchData() {
    contractNo = $("#contract").val();
    //attachmentno = $("#attachmentno").val();
    signedtime_begin = $('#signedtime_begin').datebox('getValue');
    signedtime_end = $('#signedtime_end').datebox('getValue');
    buyer_sch = $('#buyer_sch').textbox('getValue');
    seller_sch = $('#seller_sch').textbox('getValue');
    //ifchecked = $("input:radio[name='partInfo']:checked").val();

    para = {};
    para.contractNo = contractNo;
    //para.attachmentno = attachmentno;
    para.signedtime_begin = signedtime_begin;
    para.signedtime_end = signedtime_end;
    para.buyer_sch = buyer_sch;
    para.seller_sch = seller_sch;
    //para.ifchecked = ifchecked;
    $("#tt").datagrid('load', para);
}
//初始化商检合同信息
function initInpsectInfo(contractNo) {
    $.post("/ashx/contract/contractData.ashx?module=initInpectInfo", { contractNo:contractNo }, function (data) {
        //初始化商检合同信息
        //if (data.sucess == "0") {
            $("#inspectTab").css("display", "none");
            $("input[name='isInspect'][value='0']").attr("checked", true);//商检默认选中
            $("input[name='isInspect']").attr("disabled", true);
            $("#inspectStatus").val("否");//是否商检状态
        //}
        //else {
        //    $("#inspectTab").css("display", "block");
        //    $("input[name='isInspect'][value='1']").attr("checked", true);//商检默认选中
        //    $("#inspectStatus").val("是");//是否商检状态
        //    $("input[name='isInspect']").attr("disabled", true);
        //    $("#inspectApplyNo").textbox('setValue', data.inspectApplyNo);
        //    $("#inspectStyle").textbox('setValue', data.inspectStyle);
        //    $("#buyer").textbox('setValue', data.buyer);
        //    $("#seller").textbox('setValue', data.seller);
        //    $("#inspectSendMan").textbox('setValue', data.sendMan);
        //    $("#inspectSendFactory").textbox('setValue', data.sendFactory);
        //    $("#sendMan").textbox('setValue', data.sendMan);
        //    $("#sendFactory").textbox('setValue', data.sendFactory);
        //}
    }, 'json')
}
//绑定combobox
function bindCombobox() {
    //绑定发货人
    $('#sendMan').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        idField: 'name',
        textField: 'name',
        editable: false,
        columns: [[
                    { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#sendManCode").val(rowdata.code);
           
        }

    });
    //绑定发货工厂
    $('#sendFactory').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        idField: 'name',
        textField: 'name',
        editable: false,
        columns: [[
                    { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#sendFactoryCode").val(rowdata.code);
        }

    });

}
//添加商检合同
function saveCheck() {
    var row = $("#tt").datagrid('getSelected');
    var ifcheck = row.ifcheck;//合同产品是否商检
    var inspectStatus = $("#inspectStatus").val();//是否已生成商检
    //校验选择直接发货的合同创建报关关联和同时必须是非报关的合同
  
    if (($("input[name=createContract]").is(':checked'))) {//选中创建关联报关合同
        if (row.iscustoms == '是') {
            $.messager.alert("提醒", "请选择不为报关合同的合同");
            return;
        }
    } 
    
    //var createConVal = $("input[name=createContract]:checked").val();
    //alert(createConVal);
    //return;
    //if (ifcheck!=inspectStatus) {
    //    $.messager.alert("提醒", "合同产品需商检");
    //    return;
    //}
    $.ajax({
        cache: true,
        type: "POST",
        dataType: 'json',
        url: '/ashx/contract/contractOperater.ashx?module=sendimmediate&createDateTag=' + row.createDateTag + '&contractNo=' + row.contractNo
        + '&pcode=' + row.pcode + '&pname=' + row.pname + '&quantity=' + row.quantity + '&qunit=' + row.qunit + '&ifcheck=' + row.ifcheck + '&transport=' + row.transport,
        data: $('#form2').serialize(), // 你的formid
        async: false,
        error: function (data) {
            alert(data);
        },
        success: function (data) {
            if (data != undefined && data.sucess == '1') {
                alert('保存成功');
                $.messager.alert("提醒", "保存成功");
                $('#dd').dialog('close');
                window.top.refreshTab();
            }
            if (data != undefined && data.sucess == '0') {
                //像用户提示错误
                $.messager.alert("提醒", data.errormsg);
            }
        }
    });


}
//关闭直接发货窗口
function closeDialog() {
    $('#dd').dialog('close');
}
