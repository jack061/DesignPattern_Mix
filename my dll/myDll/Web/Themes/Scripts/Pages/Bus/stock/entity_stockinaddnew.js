//添加
var calcRow = function (rowIndex, rowData) {
    //完成行计算
    //alert(rowData.inquantity);
//    var quan = rowData.inquantity;
//    if (rowData.inquantity > rowData.oldinquantity) {
//        quan = rowData.oldinquantity;
//    }
//    var newamount = rowData.inquantity * rowData.price;
    //alert(rowData.amount);
//    console.log("当前编辑的行：" + rowIndex);
//    $("#tt_subTable").datagrid('updateRow', { index: rowIndex, row: { amount: newamount, inquantity: quan} });
}
//提交form表单
function save() {
    getSubTable();
    $("#owner").attr("value", $("#ownercode").combobox("getText"));
    $("#wname").attr("value", $("#wcode").combobox("getText"));

    console.log("已经获取子表json，开始提交form：");
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/StockManage/StockEntityHandler.ashx?action=save&status=新建",
        onSubmit: function () {
            //进行表单验证 
            console.log("正在表单验证！");
            valiForm();
        },
        success: function (data) {
            alert(data);
//            $.messager.alert('提示', data, 'info');
            //打开指定模板页面
            if (data.indexOf('保存成功') >= 0) {
//                $.messager.alert("提醒", data);
                window.top.selectTab('入库通知单');
            }
        }
    });
}

//校验form
function valiForm() {
    //$('#sellercode').attr('value', $('#sellercode').combobox('getValue'));
    return $("#form1").form('validate');
}
//获取子表数据
function getSubTable() {
    //提交正在编辑的行
    //if (editRow != undefined) {
    //    var tmprow = editRow;
    //    $("#tt_subTable").datagrid('endEdit', editRow);
    //    var rowdata = $("#tt_subTable").datagrid('getSelected', tmprow);
    //    if (rowdata != null) {
    //        calcRow(tmprow, rowdata);
    //    }
    //}
    $("#tt_subTable").datagrid("acceptChanges");
    var datagrid = $("#tt_subTable").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}

//查询操作
function SearchData() {
    var code = $("#mcode").val();
    var name = $("#mname").val();
    var productCategory = $("#productCategory").val();

    para = {};
    para.code = code;
    para.name = name;
    para.productCategory = productCategory;

    $("#abroadlist").datagrid('load', para);
}

$(document).ready(function () {
    //加载仓库
    $("#wcode").combogrid('grid').datagrid('loadData', wcode);

    //加载所属单位
    $("#ownercode").combogrid('grid').datagrid('loadData', ownercode);

    //加载发站
    $("#sendport").combogrid('grid').datagrid('loadData', sendports);

    //加载出战
    $("#destinationport").combogrid('grid').datagrid('loadData', desports);

    //上传
    $("#dlg").dialog("close"); //init
    $("#downloadTemp").click(function () {
        downloadTemp();
    });
    $("#btnInfoUpload").click(function () {
        uploadFile();
    });
});
//提交
var submit = function () {
    getSubTable();
    $("#owner").attr("value", $("#ownercode").combobox("getText"));
    $("#wname").attr("value", $("#wcode").combobox("getText"));

    console.log("已经获取子表json，开始提交form：");
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/StockManage/StockEntityHandler.ashx?action=save&status=待收货",
        onSubmit: function () {
            //进行表单验证 
            console.log("正在表单验证！");
            valiForm();
        },
        success: function (data) {
            //            alert(data);
            $.messager.alert('提示', data, 'info');
            //打开指定模板页面
            if (data.indexOf('提交成功') >= 0) {
                window.top.selectTab('入库通知单');
            }
        }
    });
//    var no = $("#indocno").val();
//    var status = $("#status").val();
//    console.log(no);
//    console.log(status);
//    if (no != undefined) {
//        submitbill2(status, '提交', no, '入库', function (msg) {
//            if (msg.length > 0) {
//                alert(msg);
//            }
//            else {
//                //刷新页面
//                window.top.selectTab('入库管理');
//            }
//        });
    //    }
}
//确认
//var confirm = function () {
//    var no = $("#indocno").val();
//    var status = $("#status").val();
//    console.log(no);
//    console.log(status);
//    if (no != undefined) {
//        submitbill2(status, '确认', no, '入库', function (msg) {
//            if (msg.length > 0) {
//                alert(msg);
//            }
//            else {
//                //刷新页面
//                window.top.selectTab('入库管理');
//            }
//        });
//    }
//}

//打开合同列表
//var OpenContractList = function () {
//    var newurl = "/Bus/StockManage/StockEntityHandler.ashx?action=getselectedhtlist&group=" + $('#bgroup').combobox('getValue');
//    $('#abroadlist').datagrid('options').url = newurl;
//    $('#abroadlist').datagrid('loadData', []);
//    var p = {};
//    p.seller = $("#sellercode").combobox("getValue");
//    p.buyer = $("#buyercode").combobox("getValue");
//    p.busman = $("#busmancode").combobox("getValue");
//    console.log("seller:" + p.seller);
//    console.log("buyer:" + p.buyer);
//    console.log("busman:" + p.busman);
//    $('#abroadlist').datagrid('reload', p);
//    $('#dd').window('open');
//}

var toolbar = [{
    text: '选择',
    iconCls: 'icon-add',
    handler: function () {
        $('#abroadlist').datagrid('reload');
        $('#dd').window('open');
    }
}, {
    text: '复制',
    iconCls: 'icon-add',
    handler: function () {
        copySub();

    }
}, {
    text: '删除',
    iconCls: 'icon-remove',
    handler: function () {
        var row = $("#tt_subTable").datagrid('getSelected');
        if (row != undefined) {
            var editIndex = $("#tt_subTable").datagrid('getRowIndex', row);
            $('#tt_subTable').datagrid('cancelEdit', editIndex)
                    .datagrid('deleteRow', editIndex);
            editIndex = undefined;
        }
    }
}, {
    text: 'excel导入',
    iconCls: 'icon-remove',
    handler: function () {
        uploadFile_open();
    }
}, {
    text: 'excel模板',
    iconCls: 'icon-remove',
    handler: function () {
        downloadTemp();
    }
}];
//选择产品
$('#dd').dialog({
    title: '请选择入库通知产品',
    width: 700,
    height: document.documentElement.clientHeight * 0.8,
    closed: true,
    cache: false,
    //href: 'Abroad_Product_Select.aspx',
    modal: true,
    buttons: [{
        text: '选择',
        handler: function () {
            $('#dd').dialog('close');
            var indocno = $("#indocno").val();
            //把选择的产品加载到当前页面
            var rows = $('#abroadlist').datagrid('getSelections');
            for (var i in rows) {
                var row = rows[i];
                var newrow = {};
                newrow.INDOCNO = indocno;
                newrow.MCODE = row.PCODE;
                newrow.MNAME = row.PNAME;
                newrow.MSPEC = row.SPEC;
                newrow.UNIT = row.UNIT;
                newrow.PACK = row.PALLET;
                newrow.PACKUNIT = row.PACKAGEUNIT;
                newrow.PACKDES = row.PACKDES;
                newrow.NUMBER = "0";
                newrow.INQUANTITY = "0";
                newrow.LOSSNUMBER = "0";
                newrow.REALNUMBER = "0";
                newrow.REALINQUANTITY = "0";
                newrow.UNIT = row.UNIT;
                newrow.REMARK = "";

                $('#tt_subTable').datagrid('appendRow', newrow);
                setSubTableEdit();
            }
            $('#abroadlist').datagrid('clearSelections');
        }
    }, {
        text: '取消',
        handler: function () {
            $("#dd").dialog('close');
        }
    }]
});

//----初始化datagrid-----
//入库产品列表
var editRow = undefined;
$('#tt_subTable').datagrid({
    nowrap: true,
    fitColumns: true,
    striped: true,
    collapsible: true,
    pageList: [10, 15, 30],
    singleSelect: true,
    idField: 'MCODE',
    url: '/Bus/StockManage/StockEntityHandler.ashx?action=getrkmatrlist&indocno=' + $('#indocno').val(),
    columns: [[
                { field: 'MCODE', title: 'SAP产品编号', width: '90px'},
                { field: 'MNAME', title: '产品名称', width: '120px' },
                { field: 'MSPEC', title: '规格', width: '80px' },
                { field: 'UNIT', title: '单位', width: '60px' },
                { field: 'PACK', title: '最小包装', width: '60px' },
                { field: 'PACKUNIT', title: '包装单位', width: '60px' },
                { field: 'PACKDES', title: '包装', width: '80px' },
                { field: 'CARNUMBER', title: '车皮号', width: '100px', editor: 'textbox' },
                { field: 'TICKETDATE', title: '收票日期', width: '150px', editor: 'datebox' },//datetimebox
                { field: 'NUMBER', title: '通知件数', width: '100px', editor: 'numberbox' },
                { field: 'INQUANTITY', title: '通知数量', width: '100px', editor: { type: 'numberbox', options: { precision: 3}} },
//                { field: 'lossnumber', title: '接收少件', width: '100px', editor: 'numberbox' },
//                { field: 'realnumber', title: '接收件数', width: '100px', editor: 'numberbox' },
//                { field: 'realinquantity', title: '接收数量', width: '100px', editor: 'numberbox' },
                { field: 'REMARK', title: '备注', width: '200px', editor: 'text' }
            ]],
    pagination: true,
    toolbar: toolbar,
    onAfterEdit: function (rowIndex, rowData, changes) {
        editRow = undefined;
        calcRow(rowIndex, rowData);
    },
//    onDblClickRow: function (rowIndex, rowData) {
//        if (editRow != undefined) {
//            $("#tt_subTable").datagrid('endEdit', editRow);
//        }

//        if (editRow == undefined) {
//            $("#tt_subTable").datagrid('beginEdit', rowIndex);
////                                var ed = $(this).datagrid('getEditor', { index: rowIndex, field: "inquantity" });
////                                $(ed.target)[0].focus();
////                                alert($(ed.target).innerHTML);
//            editRow = rowIndex;

//            //把数量单元格获取焦点
//        }
//    },
    onClickRow: function (rowIndex, rowData) {
        console.log("开始onclickrow");
        if (editRow != undefined) {
            $("#tt_subTable").datagrid('endEdit', editRow);

        }
        if (editRow == undefined) {
            
        }
        console.log("结束onclickrow");
    }
});
//设置子表处于编辑状态
function setSubTableEdit()
{
    //$('#tt_subTable').datagrid('selectAll');
    var rows = $('#tt_subTable').datagrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        $('#tt_subTable').datagrid('beginEdit', i);
    }
}

//复制子表
function copySub()
{
    //把选择的产品加载到当前页面
    var rows = $('#tt_subTable').datagrid('getSelections');
    if (rows.length > 0) {
        for (var i in rows) {
            var row = rows[i];
            var newrow = {};
            newrow.INDOCNO = row.INDOCNO;
            newrow.MCODE = row.MCODE;
            newrow.MNAME = row.MNAME;
            newrow.MSPEC = row.MSPEC;
            newrow.UNIT = row.UNIT;
            newrow.PACK = row.PACK;
            newrow.PACKUNIT = row.PACKUNIT;
            newrow.PACKDES = row.PACKDES;
            newrow.NUMBER = "0";
            newrow.INQUANTITY = "0";
            newrow.LOSSNUMBER = "0";
            newrow.REALNUMBER = "0";
            newrow.REALINQUANTITY = "0";
            newrow.UNIT = row.UNIT;
            newrow.REMARK = "";

            $('#tt_subTable').datagrid('appendRow', newrow);
            setSubTableEdit();
        }
    } else
    {
        $.messager.alert('提示', '请选择需要复制的记录！', 'info');
    }
   
}

//初始化产品选择列表
$('#abroadlist').datagrid({
    url: '/ashx/Basedata/ProductListHandler.ashx?type=getList',
    nowrap: false,
    striped: false,
    collapsible: false,
    sortName: 'pcode',
    sortOrder: 'desc',
    idField: 'pcode',
    columns: [[
            { field: 'PRODUCTCATEGORY', title: '产品大类', width: 60 },
            { field: 'PCODE', title: 'SAP编码', width: 60 },
            { field: 'PNAME', title: 'SAP名称', width: 120 },
            { field: 'SPEC', title: '规格', width: 80 },
            { field: 'UNIT', title: '单位', width: 60 },
            { field: 'PALLET', title: '最小包装', width: 80 },
            { field: 'PACKAGEUNIT', title: '包装单位', width: 70 },
            { field: 'PACKDES', title: '包装', width: 100 },
            { field: 'HSSCODE', title: 'HS编码', width: 80, hidden: true },
            { field: 'IFINSPECTION', title: '是否商检', width: 80, hidden: true },
            { field: 'STATUS', title: '状态', width: 60 }
            ]],

    pagination: true
});


function closetab() {
    window.top.closeTab(); //关闭标签
}



//下载模板
function downloadTemp() {
    //window.top.addNewTab("入库产品模板下载", '/Bus/BaseData/file/入库产品模板.xls', '');
    window.open("/Bus/BaseData/file/入库产品模板.xls");
};
//打开长传界面
function uploadFile_open(name) {
    fileColumName = name;
    $("#dlg").dialog("open"); //打开
}

//文件上传
function uploadFile() {
    $("#form_up").ajaxSubmit({
        url: "/ashx/Contract/chosseContractData.ashx?module=acceptUploadFile",
        type: "post",
        dataType: 'json',
        success: function (data) {
            if (data == "error") {
                $.messager.alert("错误：", "上传失败");
            }
            else {
                if (data.rows.length > 0) {
                    for (var i in data.rows) {
                        var row = data.rows[i];
                        var newrow = {};
                        newrow.INDOCNO = row.INDOCNO;
                        newrow.MCODE = row.MCODE;
                        newrow.MNAME = row.MNAME;
                        newrow.MSPEC = row.MSPEC;
                        newrow.UNIT = row.UNIT;
                        newrow.PACK = row.PACK;
                        newrow.PACKUNIT = row.PACKUNIT;
                        newrow.PACKDES = row.PACKDES;
                        newrow.CARNUMBER = row.CARNUMBER;
                        newrow.TICKETDATE = row.TICKETDATE;
                        newrow.NUMBER = "0";
                        newrow.INQUANTITY = "0";
                        newrow.LOSSNUMBER = "0";
                        newrow.REALNUMBER = "0";
                        newrow.REALINQUANTITY = "0";
                        newrow.UNIT = row.UNIT;
                        newrow.REMARK = "";

                        $('#tt_subTable').datagrid('appendRow', newrow);
                        setSubTableEdit();
                        $.messager.alert("提示", "加载成功！");
                    }
                }
            }
        }
    });
}
