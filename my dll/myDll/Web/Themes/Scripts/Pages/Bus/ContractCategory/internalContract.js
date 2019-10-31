var endEditing = function () {
    if (editIndex == undefined) { return true }
    if ($('#htcplist').datagrid('validateRow', editIndex)) {
        $('#htcplist').datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
////返回
var back = function () {

    window.top.closeAddTab("合同-新增", "/Bus/Contract/ContractTest.aspx");
}
//保存
var save = function () {


    var rrdata = SaveDataToDB(0);

    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        window.top.selectAndRefreshTab('内部结算单');
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        $.messager.alert("提醒", rrdata.sucdata.errormsg);
    }
}
//取消
var undo = function () {

    //关闭当前tab
    window.top.closeTab();
}
//提交
var submit = function () {

    var rrdata = SaveDataToDB(1);
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        $.messager.alert('系统提示', '提交成功!', 'info', function () {
            //打开指定模板页面
            window.top.selectAndRefreshTab('内部结算单');
        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
};

//返回填写信息
function backWrite() {

    $('#tt').tabs("select", "手动变量");
}
var SaveDataToDB = function (status) {
  
    if (endEditing() != true) {
        return;
    }

    var retdata = {};
    //获取list数据到htcplistStr
    var cplist = $("#htcplist").datagrid("getRows");

    var datagridjson = JSON.stringify(cplist);
    $('#htcplistStr').attr('value', datagridjson);
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/Contract/contractOperater.ashx?module=addeditInternalContract&category=internal&contactCode=' + contactContractNo,
        data: $('#form1').serialize(), // 你的formid

        async: false,
        error: function (data) {
            retdata.errdata = data;
        },
        success: function (data) {

            retdata.sucdata = data;
        }
    });
    return retdata;
}
console.time('初始化');
$(function () {
 
    bindUI();
   
    loadProdunt();
    //tabs选中事件
    selectTabs();
});


var bindUI = function () {
    //为table填充数据

    $('#form1').form('load', htdata);

    if (isnew == "true") {
        $('#contractNo').textbox('setValue', '自动编号');
        $('#contractNo').textbox('readonly', true);
    }
    ////默认签订地点
    $("#signedplace").textbox('setValue', '乌鲁木齐');
    $("#buyercode").val(buyercode);
    $("#sellercode").val(sellercode);
 
    if (contactSeller != "") {
        $('#seller').textbox('setValue', contactSeller);
        $('#seller').textbox('readonly',true);
    }
    else {
    $('#seller').combogrid({
        panelWidth: 450,
        //value:htdata.seller,
        idField: 'name',
        textField: 'name',
        data: comSeller,
        editable: false,
        disable:true,
        columns: [[
                    { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#sellercode").val(rowdata.code);
        }
    });
    $("#seller").combogrid("setValue", htdata.seller);
    }
    if (contactBuyer != "") {
       
        $('#buyer').textbox('setValue', contactBuyer);
        $('#buyer').textbox('readonly',true);
    }
    else {
        $('#buyer').combogrid({
            panelWidth: 450,
            //value:htdata.buyer,
            idField: 'name',
            textField: 'name',
            data: comBuyer,
            columns: [[
                        { field: 'code', title: '客户编码', width: 60 },
                        { field: 'shortname', title: '简称', width: 80 },
                        { field: 'name', title: '客户名称', width: 100 },
                        { field: 'address', title: '地址', width: 150 }
            ]],
            onSelect: function (index, rowdata) {
                $("#buyercode").val(rowdata.code);
            }
          
        });
        $("#buyer").combogrid("setValue", htdata.buyer);

    }

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
     
            var contractNo = $("#contractNo").textbox('getValue');
            var signedTime = $("#signedtime").datebox('getValue');
            var signedplace = $("#signedplace").textbox('getValue');
            var buyer = $("#buyer").combobox('getValue');
            var seller = $("#seller").combobox('getValue');
            var remark = $("#batchRemark").textbox('getValue');
            $('#htcplist').datagrid('acceptChanges');
      
            //获取list数据到htcplistStr
            var cplist = $("#htcplist").datagrid("getRows");
            var datagridjson = JSON.stringify(cplist);
            if (title == "查看结算单文本") {
            
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/contractData.ashx?module=GetInternalPreview",
                    dataType: "text",
                    data: {
                        buyer: buyer, seller: seller,
                        signedTime: signedTime,
                        signedplace: signedplace,
                        datagridjson: datagridjson,
                        remark:remark
                    },
                    success: function (data) {
                        $("#realTimeContractText").val(data);
                    }
                });
                $("#previewFormFrame").attr('src', '/Bus/ContractCategory/internalClearingPreviewTemplate.aspx');
                //$("#previewFormFrame").attr('src', '/Bus/ContractCategory/internalClearingPreviewTemplate.aspx?buyer=' + buyer + '&seller='
                //    + seller + '&signedTime=' + signedTime + '&signedplace=' + signedplace + '&contractNo=' + contractNo + '&datagridjson=' + datagridjson);
            }
        }
    })
}

console.timeEnd('初始化');
//加载产品列表
function loadProdunt() {
    console.time('产品列表');
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + productContractNo,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox' } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100, editor: { type: 'textbox' } },
                    { field: 'amount', title: '金额', width: 100, },
                    { field: 'packspec', title: '包装规格', width: 100, editor: 'textbox' },
                    { field: 'packing', title: '包装描述', width: 100, editor: 'textbox' },
                    { field: 'pallet', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
                    { field: 'ifplace', title: '是否产地证', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' }
        ]],

        onClickCell: function (index, row, changes) {
            var row = $("#htcplist").datagrid('getSelected');

            if (editIndex != index) {
                var mygrid = $('#htcplist');
                if (endEditing()) {
                    $(this).datagrid('beginEdit', index);
                    var ed = $(this).datagrid('getEditor', { index: index, field: field });
                    if ($(ed.target) != undefined) {
                        $(ed.target).focus();
                    }
                    editIndex = index;
                } else {
                    setTimeout(function () {
                        mygrid.datagrid('selectRow', editIndex);
                    }, 0);
                }
            }

        },
        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
    console.timeEnd('产品列表');
}