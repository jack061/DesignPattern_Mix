//添加
var calcRow = function (rowIndex, rowData) {
    //完成行计算
    //alert(rowData.inquantity);
    var quan = rowData.inquantity;
    if (rowData.inquantity > rowData.oldinquantity) {
        quan = rowData.oldinquantity;
    }
    var newamount = rowData.inquantity * rowData.price;
    //alert(rowData.amount);
    console.log("当前编辑的行：" + rowIndex);
    $("#tt_subTable").datagrid('updateRow', { index: rowIndex, row: { amount: newamount, inquantity: quan} });
}
//提交form表单
function save() {
    getSubTable();

    $("#seller").attr("value", $("#sellercode").combobox("getText"));
    $("#buyer").attr("value", $("#buyercode").combobox("getText"));
    $("#busman").attr("value", $("#busmancode").combobox("getText"));

    console.log("已经获取子表json，开始提交form：");
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/StockManage/StockHandler.ashx?action=save",
        onSubmit: function () {
            //进行表单验证 
            console.log("正在表单验证！");
            valiForm();
        },
        success: function (data) {
            alert(data);
            //打开指定模板页面
            if (data.indexOf('保存成功') >= 0) {
                window.top.selectTab('入库管理');
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
    para.buyer = $("#buyercode").combobox("getValue");
    para.busman = $("#busmancode").combobox("getValue");

    console.log("传入参数：");
    console.log("seller:" + para.seller);
    console.log("buyer:" + para.buyer);
    console.log("busman:" + para.busman);

    $("#abroadlist").datagrid('load', para);
}

$(document).ready(function () {
    //加载业务员下拉列表
    $("#busmancode").combobox('loadData', busman);

    //加载供货单位
    $("#sellercode").combogrid('grid').datagrid('loadData', ghdw);

    //加载收货单位
    $("#buyercode").combogrid('grid').datagrid('loadData', shdw);
});

var submit = function () {
    var no = $("#outdocno").val();
    var status = $("#status").val();
    console.log(no);
    console.log(status);
    if (no != undefined) {
        submitbill2(status, '提交', no, '出库', function (msg) {
            if (msg.length > 0) {
                alert(msg);
            }
            else {
                //刷新页面
                window.top.selectTab('入库管理');
            }
        });
    }
}

var review = function () {
    var no = $("#outdocno").val();
    var status = $("#status").val();
    if (no != undefined) {
        submitbill2(status, '审核', no, '出库', function (msg) {
            if (msg.length > 0) {
                alert(msg);
            }
            else {
                //刷新页面
                window.top.selectTab('入库管理');
            }
        });
    }
}

var confirm = function () {
    var no = $("#outdocno").val();
    var status = $("#status").val();
    console.log(no);
    console.log(status);
    if (no != undefined) {
        submitbill2(status, '确认', no, '出库', function (msg) {
            if (msg.length > 0) {
                alert(msg);
            }
            else {
                //刷新页面
                window.top.selectTab('入库管理');
            }
        });
    }
}

var OpenContractList = function () {
    var newurl = "/Bus/StockManage/StockHandler.ashx?action=getselectedhtlist&group=" + $('#bgroup').combobox('getValue');
    $('#abroadlist').datagrid('options').url = newurl;
    $('#abroadlist').datagrid('loadData', []);
    var p = {};
    p.seller = $("#sellercode").combobox("getValue");
    p.buyer = $("#buyercode").combobox("getValue");
    p.busman = $("#busmancode").combobox("getValue");
    console.log("seller:" + p.seller);
    console.log("buyer:" + p.buyer);
    console.log("busman:" + p.busman);
    $('#abroadlist').datagrid('reload', p);
    $('#dd').window('open');
}

var OpenSHDW = function () {

}

var OpenGHDW = function () {

}


//组件初始化
//付款人
$('#supcode').combobox({
    url: '/ashx/Basedata/CustomerHandler.ashx?action=getComboList',
    valueField: 'id',
    textField: 'text'
});
$('#supcode').combobox('setValue', $("#supcodevalue").val());

var toolbar = [{
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
}];
//选择境外合同
$('#dd').dialog({
    title: '请选择合同',
    width: 600,
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

            if (rows.length > 0) {
                $('#currency').val(rows[0].currency);
                //加载汇率
                $(rate).each(function (index) {
                    if (rate[index].name == rows[0].currency) {
                        $('#rate').val(rate[index].rate);
                        return;
                    }
                });
            }

            var isexists = false;
            //从选择的合同里面，获取产品信息
            var dto = {};
            dto.action = "getselectedhtcp";

            for (i = 0; i < rows.length; i++) {
                if (i == 0) {
                    //alert(rows[i].contractNo);
                    $('#contractno').textbox('setValue', rows[i].contractNo);
                    dto.contractno = rows[i].contractNo;
                    dto.attachmentno = rows[i].attachmentno;
                    break;
                }
            }
            var cprows;
            //发起后台请求，获取产品列表
            $.post('/Bus/StockManage/StockHandler.ashx', dto, function (result) {
                cprows = jQuery.parseJSON(result).rows;

                //清空原来的grid
                $('#tt_subTable').datagrid('loadData', []);

                for (var i in cprows) {
                    var row = cprows[i];
                    var newrow = {};
                    newrow.indocno = indocno;
                    newrow.mcode = row.pcode;
                    newrow.mname = row.pname;
                    newrow.price = row.price;
                    newrow.inquantity = row.quantity;
                    newrow.oldinquantity = row.quantity;
                    newrow.amount = row.amount;
                    newrow.remark = "";
                    newrow.mspec = row.packspec;
                    newrow.unit = row.qunit;
                    newrow.batchno = '0';

                    $('#tt_subTable').datagrid('appendRow', newrow);
                }
                $('#abroadlist').datagrid('clearSelections');
                //                        if (endEditing()) {
                //                            editIndex = $('#tt_subTable').datagrid('getRows').length - 1;
                //                            $('#tt_subTable').datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
                //                        }
            });
        }
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
    idField: 'mname',
    url: '/Bus/StockManage/StockHandler.ashx?action=getrkmatrlist&indocno=' + $('#indocno').val(),
    columns: [[
                { field: 'mcode', title: '产品编号', width: '100px', hidden: true },
                { field: 'mname', title: '名称', width: '100px' },
                { field: 'batchno', title: '批次号', width: '100px', hidden: true },
                { field: 'mspec', title: '规格', width: '100px' },
                { field: 'unit', title: '单位', width: '100px' },
                { field: 'inquantity', title: '数量', width: '100px', editor: 'numberbox' },
                { field: 'oldinquantity', title: '数量2', width: '100px', hidden: true },
                { field: 'price', title: '单价', width: '100px' },
                { field: 'amount', title: '金额', width: '100px' },
                { field: 'remark', title: '备注', width: '100px', editor: 'textarea' }
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
            //                    var ed = $(this).datagrid('getEditor', { index: rowIndex, field: "inquantity" });
            //                    $(ed.target)[0].focus();
            //                    alert($(ed.target).innerHTML);
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
//初始化合同选择列表
$('#abroadlist').datagrid({
    url: '/Bus/StockManage/StockHandler.ashx?action=getselectedhtlist',
    pagination: true,
    rownumbers: true,
    singleSelect: true,
    sortName: 'contractno',
    sortOrder: 'asc',
    columns: [[
                    { field: 'contractNo', title: '合同号', width: 120 },
                    { field: 'seller', title: '卖方', width: 150 },
                    { field: 'pname', title: '产品', width: '120px' },
                    { field: 'amount', title: '金额', width: '120px' }
                ]]
});

