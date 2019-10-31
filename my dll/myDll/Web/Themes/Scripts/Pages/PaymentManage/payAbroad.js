$(document).ready(function () {
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //----初始化datagrid-----
        $('#tt').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'PAYNO',
            url: '/ashx/PaymentManage/PaymentAbroadHandler.ashx?action=getList',
            columns: [[
            { field: 'PAYNO', title: '到款单号', width: '100px' },
            { field: 'STATUS', title: '状态', width: '50px' },
            { field: 'PAYER', title: '付款方', width: '80px' },
            { field: 'PAYDATE', title: '付款时间', width: '100px' },
            { field: 'PAYAMOUNT', title: '付款金额', width: '80px' },
            { field: 'CURRENCY', title: '货币', width: '80px' },
            { field: 'PAYRATE', title: '付款汇率', width: '80px' },
            { field: 'REMARK', title: '备注', width: '150px' },
            { field: 'CREATEMAN', title: '创建人', width: '100px' },
            { field: 'CREATEDATE', title: '创建日期', width: '100px' },
            { field: 'LASTMOD', title: '最后修改人', width: '100px' },
            { field: 'LASTMODDATE', title: '最后修改日期', width: '100px' }
            ]],
            pagination: true
        });

        $("#dd").dialog({
            title: '境外到款-新增',
            width: document.documentElement.clientWidth,
            height: document.documentElement.clientHeight,
            closed: true,
            cache: false,
            modal: true,
            maximizable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    submitForm();
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#dd").dialog('close');
                }
            }]
        });
    }

    if (pageTag == "form") {
        //组件初始化
        //付款人
        $('#payer').combobox({
            url: '/ashx/Basedata/CustomerHandler.ashx?action=getComboList',
            valueField: 'text',
            textField: 'text'
        });
        $('#payer').combobox('setValue',$("#payer_").val());
        //货币
        $('#currency').combobox({
            url: '/ashx/Basedata/DictronaryHandler.ashx?action=getCurrencyList',
            valueField: 'id',
            textField: 'text'
        });
        $('#currency').combobox('setValue', $("#currency_").val());
        //子表处理
        var editRow = undefined;
        var toolbar = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
            /*
                if (editRow != undefined) {
                    $("#tt_subTable").datagrid('endEdit', editRow);
                }
                if (editRow == undefined) {
                    $("#tt_subTable").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });

                    $("#tt_subTable").datagrid('beginEdit', 0);
                    editRow = 0;
                }
                */
                //弹出
                $('#dd').window('open');
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#tt_subTable").datagrid('getSelected');
                if (row != null) {
                    if (editRow != undefined) {
                        $("#tt_subTable").datagrid('endEdit', editRow);
                    }

                    if (editRow == undefined) {
                        var index = $("#tt_subTable").datagrid('getRowIndex', row);
                        $("#tt_subTable").datagrid('beginEdit', index);
                        editRow = index;
                        $("#tt_subTable").datagrid('unselectAll');
                    }
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }

            }
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != undefined) {
                    var editIndex = $("#tt").datagrid('getRowIndex', row);
                    $('#dg').datagrid('cancelEdit', editIndex)
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
            height: document.documentElement.clientHeight,
            closed: true,
            cache: false,
            //href: 'Abroad_Product_Select.aspx',
            modal: true,
            buttons: [{
                text: '选择',
                handler: function () {
                    $('#dd').dialog('close');
                    var payNo = $("#payNo").val();
                    //把选择的产品加载到当前页面
                    var rows = $('#abroadlist').datagrid('getSelections');
                    var oldrows = $('#tt_subTable').datagrid('getRows');
                    for (var i = 0; i < rows.length; i++) {
                        //判断当前表格里面是否有pcode
                        var isexists = false;
                        for (var j = 0; j < oldrows.length; j++) {
                            if (oldrows[j].CONTRACTNO == rows[i].CONTRACTNO && oldrows[j].ATTACHMENTNO == rows[i].ATTACHMENTNO) {
                                isexists = true;
                                break;
                            }
                        }
                        if (isexists == false) {
                            var row = rows[i];
                            var newrow = {};
                            newrow.PAYNO = payNo;
                            newrow.CONTRACTNO = row.CONTRACTNO;
                            newrow.ATTACHMENTNO = row.ATTACHMENTNO;
                            newrow.RECEIVER = row.SELLER;
                            newrow.PAYER = row.BUYER;
                            newrow.PAYAMOUNT = 0;
                            newrow.ICONTRACTNO = '';

                            $('#tt_subTable').datagrid('appendRow', newrow);
                        }
                    }
                    $('#abroadlist').datagrid('clearSelections');
                    if (endEditing()) {
                        editIndex = $('#tt_subTable').datagrid('getRows').length - 1;
                        $('#tt_subTable').datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
                    }
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
            idField: 'CONTRACTNO',
            url: '/ashx/PaymentManage/PaymentAbroadHandler.ashx?action=getSubList',
            columns: [[
            { field: 'PAYNO', title: '到款单号', width: '100px' },
            { field: 'CONTRACTNO', title: '合同编号', width: '100px' },
            { field: 'ATTACHMENTNO', title: '附件编号', width: '100px' },
            { field: 'RECEIVER', title: '收款方', width: '100px' },
            { field: 'PAYER', title: '付款方', width: '100px' },
            { field: 'PAYAMOUNT', title: '付款金额', width: '100px', editor: 'text' },
            { field: 'ICONTRACTNO', title: '境内合同编号', width: '100px', editor: 'text' }
            ]],
            pagination: true,
            toolbar: toolbar,
            onAfterEdit: function (rowIndex, rowData, changes) {
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
                if (editRow != undefined) {
                    $("#tt_subTable").datagrid('endEdit', editRow);

                }
            }
        });
        //初始化合同选择列表
        $('#abroadlist').datagrid({
            url: '/ashx/PaymentManage/PaymentAbroadHandler.ashx?action=getHTList',
            pagination: true,
            rownumbers: true,
            sortName: 'CONTRACTNO',
            sortOrder: 'asc',
            columns: [[
		            { field: 'CONTRACTNO', title: '合同编号', width: 100 },
		            { field: 'ATTACHMENTNO', title: '附件编号', width: 100, align: 'center' },
                    { field: 'SELLER', title: '收款方', width: '100px', editor: 'text' },
                    { field: 'BUYER', title: '付款方', width: '100px', editor: 'text' }
                ]]
        });
    }


});

//查询操作
function SearchData() {
    payno = $("#payno").val();
    payer = $("#payer").val();
    beginTime = $("#beginTime").val();
    endTime = $("#endTime").val();

    para = {};
    para.payno = payno;
    para.payer = payer;
    para.beginTime = beginTime;
    para.endTime = endTime;

    $("#tt").datagrid('load', para);
}


//添加
function add() {
    $("#ddframe").attr('src', '/PaymentManage/PaymentAbroadForm1.aspx?action=add');
    $("#dd").dialog().title = "境外到款-添加";
    $("#dd").dialog('open');
}
//修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.PAYNO;
        $("#ddframe").attr('src', '/PaymentManage/PaymentAbroadForm1.aspx?action=edit&payNo=' + no);
        $("#dd").dialog().title = "境外到款-修改";
        $("#dd").dialog('open');
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//删除
function del() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.PAYNO;

    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//添加

//提交form表单
function submitForm() {
    $("#ddframe")[0].contentWindow.getSubTable();
    var frame = $(window.frames["ddframe"].document).find("#form1");
    var form = window.frames["ddframe"].document.getElementById("form1");
    frame.form('submit', {
        url: "/ashx/PaymentManage/PaymentAbroadHandler.ashx?action=add",
        onSubmit: function () {
            //进行表单验证 
            return window.frames["ddframe"].valiForm();
        },
        success: function (data) {
            alert(data)
        }
    });

}

//校验form
function valiForm() {
    return $("#form1").form('validate');
  }
//获取子表数据
function getSubTable() {
    var datagrid = $("#tt_subTable").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}