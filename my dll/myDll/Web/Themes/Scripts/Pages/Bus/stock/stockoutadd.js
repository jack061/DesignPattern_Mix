var pageTag = "form";
var calcRow = function (rowIndex, rowData) {
    //完成行计算
    var quan = rowData.outquantity;
    if (rowData.outquantity > rowData.oldoutquantity) {
        quan = rowData.oldoutquantity;
    }
    var newamount = quan * rowData.price;
    console.log("当前编辑的行：" + rowIndex);
    $("#tt_subTable").datagrid('updateRow', { index: rowIndex, row: { amount: newamount} });
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
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
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
        modal: true,
        buttons: [{
            text: '选择',
            handler: function () {
                $('#dd').dialog('close');
                var outdocno = $("#outdocno").val();
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
                dto.action = "getselectedhtcp_xs";

                for (i = 0; i < rows.length; i++) {
                    if (i == 0) {
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
                        newrow.outdocno = outdocno;
                        newrow.mcode = row.pcode;
                        newrow.mname = row.pname;
                        newrow.price = row.price;
                        newrow.outquantity = row.quantity;
                        newrow.oldoutquantity = row.quantity;
                        newrow.amount = row.amount;
                        newrow.remark = "";
                        newrow.mspec = row.packspec;
                        newrow.unit = row.qunit;
                        newrow.batchno = '0';
                        $('#tt_subTable').datagrid('appendRow', newrow);
                    }
                    $('#abroadlist').datagrid('clearSelections');
                });
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
        idField: 'mname',
        url: '/Bus/StockManage/StockHandler.ashx?action=getckmatrlist&outdocno=' + $('#outdocno').val(),
        columns: [[
            { field: 'mcode', title: '编号', width: '100px', hidden: true },
            { field: 'mname', title: '名称', width: '100px' },
            { field: 'batchno', title: '批次号', width: '100px', hidden: true },
            { field: 'mspec', title: '规格', width: '100px' },
            { field: 'unit', title: '单位', width: '100px' },
            { field: 'outquantity', title: '数量', width: '100px', editor: 'numberbox' },
            { field: 'oldoutquantity', title: '数量2', width: '100px', hidden: true },
            { field: 'price', title: '单价', width: '100px' },
            { field: 'amount', title: '金额', width: '100px' },
            { field: 'remark', title: '备注', width: '100px', editor: 'textarea' }
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
    //初始化合同选择列表
    $('#abroadlist').datagrid({
        pagination: true,
        rownumbers: true,
        sortName: 'contractno',
        singleSelect: true,
        sortOrder: 'asc',
        url: '/Bus/StockManage/StockHandler.ashx?action=getselectedhtlist_xs',
        columns: [[
                   { field: 'contractNo', title: '合同号', width: 120 },
                    { field: 'seller', title: '卖方', width: 150 },
                    { field: 'pname', title: '产品', width: '120px' },
                    { field: 'amount', title: '金额', width: '120px' }
            ]]
    });
}
//添加

//提交form表单
function save() {
    getSubTable();

    $("#seller").attr("value", $("#sellercode").combobox("getText"));
    $("#buyer").attr("value", $("#buyercode").combobox("getText"));
    $("#busman").attr("value", $("#busmancode").combobox("getText"));
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/StockManage/StockHandler.ashx?action=saveout",
        onSubmit: function () {
            //进行表单验证 
            return valiForm();
        },
        success: function (data) {
            alert(data);
            //打开指定模板页面
            if (data.indexOf('保存成功') >= 0) {
                window.top.selectTab('出库管理');
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
                window.top.selectTab('出库管理');
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
                window.top.selectTab('出库管理');
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
                window.top.selectTab('出库管理');
            }
        });
    }
}

var OpenContractList = function () {
    var newurl = "/Bus/StockManage/StockHandler.ashx?action=getselectedhtlist_xs&group=" + $('#bgroup').combobox('getValue');
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