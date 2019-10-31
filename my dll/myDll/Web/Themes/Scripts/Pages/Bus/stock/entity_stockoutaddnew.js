var pageTag = "form";
var editRow = undefined;
var calcRow = function (rowIndex, rowData) {
    //完成行计算
//    var quan = rowData.outquantity;
//    if (rowData.outquantity > rowData.oldoutquantity) {
//        quan = rowData.oldoutquantity;
//    }
//    var newamount = quan * rowData.price;
//    console.log("当前编辑的行：" + rowIndex);
//    $("#tt_subTable").datagrid('updateRow', { index: rowIndex, row: { amount: newamount} });
}
if (pageTag == "form") {
    //组件初始化
    //付款人
    $('#cuscode').combobox({
        url: '/ashx/Basedata/CustomerHandler.ashx?action=getComboList',
        valueField: 'id',
        textField: 'text'
    });
    $('#cuscode').combobox('setValue', $("#cuscodevalue").val());

    //加载仓库
    $("#wcode").combogrid('grid').datagrid('loadData', wcode);

    //加载所属单位
    $("#ownercode").combogrid('grid').datagrid('loadData', ownercode);

    //加载买方
    $("#buyercode").combogrid('grid').datagrid('loadData', buyercode);

    var toolbar;
    var ckid = $('#checkid').val();
    if (typeof (ckid) == "undefined" || ckid == null || ckid == '')
    {//无合同
        toolbar = [
            {
                text: '添加',
                iconCls: 'icon-add',
                handler: function () {
                    var newurl = '/Bus/StockManage/StockEntityHandler.ashx?action=stockswiftlist&wcode=' + $('#wcode').combogrid('getValue') + '&ownercode=' + $('#ownercode').combogrid('getValue');
                    $('#abroadlist').datagrid('options').url = newurl;
                    $('#abroadlist').datagrid('reload');
                    $('#dd').window('open');
                    console.log("wcode=" + $('#wcode').combogrid('getValue'));
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
                    } else {
                        $.messager.alert('提示', '请选择一行数据！', 'info');
                    }
                }
            }];
    }else
    {
        toolbar = [
             {
                text: '删除',
                iconCls: 'icon-remove',
                handler: function () {
                    var row = $("#tt_subTable").datagrid('getSelected');
                    if (row != undefined) {
                        var editIndex = $("#tt_subTable").datagrid('getRowIndex', row);
                        $('#tt_subTable').datagrid('cancelEdit', editIndex)
                            .datagrid('deleteRow', editIndex);
                        editIndex = undefined;
                    } else {
                        $.messager.alert('提示', '请选择一行数据！', 'info');
                    }
                }
            }];

            initSubTable(productD);
    }
     
    //选择产品
    $('#dd').dialog({
        title: '请选择产品',
        width: 600,
        height: document.documentElement.clientHeight * 0.8,
        closed: true,
        cache: false,
        modal: true,
        buttons: [{
            text: '选择',
            handler: function () {
                $('#dd').dialog('close');
                var outdocno = $("#outdocno").val();
                //把选择的产品加载到当前页面
                var rows = $('#abroadlist').datagrid('getSelections');

                var rows = $('#abroadlist').datagrid('getSelections');
                for (var i in rows) {
                    var row = rows[i];
                    var newrow = {};
                    newrow.outdocno = outdocno;
                    newrow.batchno = row.batchno;
                    newrow.mcode = row.mcode;
                    newrow.mname = row.mname;
                    newrow.mspec = row.mspec;
                    newrow.pack = row.pack;
                    newrow.packunit = row.packunit;
                    newrow.packdes = row.packdes;
                    newrow.number = row.number; ;
                    newrow.outquantity = row.quantity;
                    newrow.unit = row.unit;
                    newrow.indate = row.createdate;
                    newrow.remark = "";

                    $('#tt_subTable').datagrid('appendRow', newrow);
                }
                $('#abroadlist').datagrid('clearSelections');
            }
            //选择按钮handle结束
        }, {
            text: '取消',
            handler: function () {
                $("#dd").dialog('close');
            }
        }]
    });

    //----初始化datagrid-----
    $('#tt_subTable').datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        idField: 'mcode',
        url: '/Bus/StockManage/StockEntityHandler.ashx?action=getckmatrlist&outdocno=' + $('#outdocno').val(),
        columns: [[
            { field: 'outdocno', title: '出库单号', width: '100px', hidden: true },
            { field: 'mcode', title: '产品编号', width: '100px' },
            { field: 'mname', title: '产品名称', width: '150px' },
            { field: 'mspec', title: '规格', width: '100px' },
            { field: 'unit', title: '单位', width: '100px' },
            { field: 'pack', title: '最小包装', width: '60px' },
            { field: 'packunit', title: '包装单位', width: '60px' },
            { field: 'packdes', title: '包装', width: '80px' },
            { field: 'batchno', title: '批次号', width: '100px' },
            { field: 'number', title: '通知件数', width: '100px', editor: 'numberbox' },
            { field: 'outquantity', title: '通知数量', width: '100px', editor: { type: 'numberbox', options: { precision: 3 } } },
            
            { field: 'indate', title: '入库时间', width: '80px' },
            { field: 'remark', title: '备注', width: '150px', editor: 'text' }
            ]],
        pagination: true,
        toolbar: toolbar,
        onAfterEdit: function (rowIndex, rowData, changes) {
            calcRow(rowIndex, rowData);
            editRow = undefined;
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);
            }

            if (editRow == undefined) {
                $("#tt_subTable").datagrid('beginEdit', rowIndex);
                editRow = rowIndex;
            }
        },
        onClickRow: function (rowIndex, rowData) {
            console.log("开始onclickrow");
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);
            }
            console.log("结束onclickrow");
        }
    });
    //初始化库存列表
    $('#abroadlist').datagrid({
        pagination: true,
        rownumbers: true,
        sortName: 'wname',
        sortOrder: 'asc',
        url: '/Bus/StockManage/StockEntityHandler.ashx?action=stockswiftlist&wcode=' + $('#wcode').combogrid('getValue') + '&ownercode=' + $('#ownercode').combogrid('getValue'),
        columns: [[
                    { field: 'wname', title: '仓库', width: 60 },
                    { field: 'owner', title: '货权所属方', width: 120 },
                    { field: 'batchno', title: '批次', width: 80 },
                    { field: 'mcode', title: 'SAP编码', width: 80 },
                    { field: 'mname', title: 'SAP名称', width: 120 },
                    { field: 'mspec', title: '规格', width: 80 },
                    { field: 'unit', title: '计量单位', width: 60 },
                    { field: 'number', title: '库存件数', width: 80 },
                    { field: 'quantity', title: '库存数量', width: 80 },
                    { field: 'pack', title: '最小包装', width: '60px' },
                { field: 'packunit', title: '包装单位', width: '60px' },
                { field: 'packdes', title: '包装', width: '80px' },
                { field: 'createdate', title: '入库时间', width: '80px' },
            ]]
    });
}
//添加

//提交form表单
function save() {
    getSubTable();
    $("#owner").attr("value", $("#ownercode").combobox("getText"));
    $("#wname").attr("value", $("#wcode").combobox("getText"));

    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/StockManage/StockEntityHandler.ashx?action=saveout&status=新建",
        onSubmit: function () {
            //进行表单验证 
            return valiForm();
        },
        success: function (data) {
            alert(data);
//            $.messager.alert('提示', data, 'info');
            //打开指定模板页面
            if (data.indexOf('保存成功') >= 0) {
                window.top.selectTab('出库通知单');
            }
        }
    });

}

//提交
var submit = function () {
    getSubTable();
    $("#owner").attr("value", $("#ownercode").combobox("getText"));
    $("#wname").attr("value", $("#wcode").combobox("getText"));

    console.log("已经获取子表json，开始提交form：");
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/StockManage/StockEntityHandler.ashx?action=saveout&status=待发货",
        onSubmit: function () {
            //进行表单验证 
            console.log("正在表单验证！");
            valiForm();
        },
        success: function (data) {
            $.messager.alert('提示', data, 'info');
            //打开指定模板页面
            if (data.indexOf('提交成功') >= 0) {
                window.top.selectTab('出库通知单');
            }
        }
    });

}

//校验form
function valiForm() {
    //$('#cuscodevalue').attr('value', $('#cuscode').combobox('getValue'));
    return $("#form1").form('validate');
}
//获取子表数据
function getSubTable() {
    //提交正在编辑的行
    if (editRow != undefined) {
        var tmprow = editRow;
        $("#tt_subTable").datagrid('endEdit', editRow);
        var rowdata = $("#tt_subTable").datagrid('getSelected', tmprow);
        if (rowdata != null) {
            calcRow(tmprow, rowdata);
        }
    }
    var datagrid = $("#tt_subTable").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}

var RefreshUI = function () {
    doSearch();
}

//查询操作
function SearchData() {
    var contractNo = $("#contractNo").val();
    var attachmentno = $("#attachmentno").val();
    var beginTime = $("#beginTime").datebox('getValue');
    var endTime = $("#endTime").datebox('getValue');

    para = {};
    para.contractNo = contractNo;
    para.attachmentno = attachmentno;
    para.beginTime = beginTime;
    para.endTime = endTime;
    para.seller = $("#sellercode").combobox("getValue");
    para.buyer = $("#sellercode").combobox("getValue");
    para.busman = $("#sellercode").combobox("getValue");

    $("#abroadlist").datagrid('load', para);
}

//var submit = function () {
//    var no = $("#outdocno").val();
//    var status = $("#status").val();
//    console.log(no);
//    console.log(status);
//    if (no != undefined) {
//        submitbill2(status, '提交', no, '出库', function (msg) {
//            if (msg.length > 0) {
//                alert(msg);
//            }
//            else {
//                //刷新页面
//                window.top.selectTab('出库管理');
//            }
//        });
//    }
//}

//var review = function () {
//    var no = $("#outdocno").val();
//    var status = $("#status").val();
//    if (no != undefined) {
//        submitbill2(status, '审核', no, '出库', function (msg) {
//            if (msg.length > 0) {
//                alert(msg);
//            }
//            else {
//                //刷新页面
//                window.top.selectTab('出库管理');
//            }
//        });
//    }
//}

//var confirm = function () {
//    var no = $("#outdocno").val();
//    var status = $("#status").val();
//    console.log(no);
//    console.log(status);
//    if (no != undefined) {
//        submitbill2(status, '确认', no, '出库', function (msg) {
//            if (msg.length > 0) {
//                alert(msg);
//            }
//            else {
//                //刷新页面
//                window.top.selectTab('出库管理');
//            }
//        });
//    }
//}

var OpenContractList = function () {
    var newurl = "/Bus/StockManage/StockEntityHandler.ashx?action=getselectedhtlist_xs&group=" + $('#bgroup').combobox('getValue');
    $('#abroadlist').datagrid('options').url = newurl;
    $('#abroadlist').datagrid('loadData', []);

    var p = {};
    p.seller = $("#sellercode").combobox("getValue");
    p.buyer = $("#buyercode").combobox("getValue");
    p.busman = $("#busmancode").combobox("getValue");

    $('#abroadlist').datagrid('reload', p);
    $('#dd').window('open');
}

$(document).ready(function () {
    //加载业务员下拉列表
    $("#busmancode").combobox('loadData', busman);
    //加载供货单位
    $("#sellercode").combogrid('grid').datagrid('loadData', ghdw);

    //加载收货单位
    $("#buyercode").combogrid('grid').datagrid('loadData', shdw);
});

function initSubTable(obj) {
    for (var i in obj) {
        var row = obj[i];
        var newrow = {};
        newrow.outdocno = outdocno;
        newrow.mcode = row.pcode;
        newrow.mname = row.pname;
        newrow.mspec = row.spec;
        newrow.number = row.packages;
        newrow.outquantity = row.mass;
        newrow.unit = row.unit;
        newrow.batchno = '0';
        newrow.remark = "";

        $('#tt_subTable').datagrid('appendRow', newrow);
    }
}

function closetab() {
    window.top.closeTab(); //关闭标签
}
