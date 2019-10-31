
$(document).ready(function () {

    var action = $("#action").val();
    
    if (action == 'browse') {
        $("#midbtns").hide();
//        $('input').attr('disabled', true);

    }
});
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
//    $("#owner").attr("value", $("#ownercode").combobox("getText"));
//    $("#wname").attr("value", $("#wcode").combobox("getText"));

    console.log("已经获取子表json，开始提交form：");
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/StockManage/StockEntityHandler.ashx?action=receive&status=待收货",
        onSubmit: function () {
            //进行表单验证 
            console.log("正在表单验证！");
            valiForm();
        },
        success: function (data) {
            alert(data);
            //打开指定模板页面
            if (data.indexOf('保存成功') >= 0) {
                window.top.selectTab('库管入库确认');
            }
        }
    });
}
//提交
var submit = function () {
    getSubTable();
//    $("#owner").attr("value", $("#ownercode").combobox("getText"));
//    $("#wname").attr("value", $("#wcode").combobox("getText"));

    console.log("已经获取子表json，开始提交form：");
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/StockManage/StockEntityHandler.ashx?action=receive&status=已收货",
        onSubmit: function () {
            //进行表单验证 
            console.log("正在表单验证！");
            valiForm();
        },
        success: function (data) {
            alert(data);
            //打开指定模板页面
            if (data.indexOf('提交成功') >= 0) {
                window.top.selectTab('库管入库确认');
            }
        }
    });
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
//校验form
function valiForm() {
    //$('#sellercode').attr('value', $('#sellercode').combobox('getValue'));
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

//查询操作
function SearchData() {
    var code = $("#mcode").val();
    var name = $("#mname").val();

    para = {};
    para.code = code;
    para.name = name;

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
    idField: 'mname',
    url: '/Bus/StockManage/StockEntityHandler.ashx?action=getrkmatrlist&indocno=' + $('#indocno').val(),
    columns: [[
                { field: 'MCODE', title: 'SAP产品编号', width: '80px' },
                { field: 'MNAME', title: '产品名称', width: '120px' },
                { field: 'MSPEC', title: '规格', width: '80px' },
                { field: 'UNIT', title: '单位', width: '60px' },
                { field: 'PACK', title: '最小包装', width: '60px' },
                { field: 'PACKUNIT', title: '包装单位', width: '60px' },
                { field: 'PACKDES', title: '包装', width: '80px' },
                { field: 'CARNUMBER', title: '车皮号', width: '100px' },
                { field: 'TICKETDATE', title: '收票日期', width: '150px'},
                { field: 'NUMBER', title: '通知件数', width: '80px' },
                { field: 'INQUANTITY', title: '通知数量', width: '80px' },
                { field: 'LOSSNUMBER', title: '接收少件', width: '80px', editor: 'numberbox' },
                { field: 'REALNUMBER', title: '接收件数', width: '80px', editor: 'numberbox' },
                { field: 'REALINQUANTITY', title: '接收数量', width: '80px', editor: { type: 'numberbox', options: { precision: 3}} },
                {field: 'REMARK', title: '备注', width: '200px', editor: 'text' }
            ]],
    pagination: true,
    toolbar: toolbar,
    onAfterEdit: function (rowIndex, rowData, changes) {
        editRow = undefined;
        calcRow(rowIndex, rowData);
    },
    onDblClickRow: function (rowIndex, rowData) {
        if (editRow != undefined) {
            $("#tt_subTable").datagrid('endEdit', editRow);
        }

        if (editRow == undefined) {
            $("#tt_subTable").datagrid('beginEdit', rowIndex);
//                                var ed = $(this).datagrid('getEditor', { index: rowIndex, field: "inquantity" });
//                                $(ed.target)[0].focus();
//                                alert($(ed.target).innerHTML);
            editRow = rowIndex;

            //把数量单元格获取焦点
        }
    },
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

function closetab() {
    window.top.closeTab(); //关闭标签
}


