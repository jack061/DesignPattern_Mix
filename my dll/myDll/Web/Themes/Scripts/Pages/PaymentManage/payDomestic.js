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
            url: '/ashx/PaymentManage/PaymentDomesticHandler.ashx?type=getList',
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
            title: '境内到款-新增',
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
        var editRow = undefined;
        var toolbar = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
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
        //----初始化datagrid-----
        var payno = $("input[name='payNo']").val();
        $('#tt_subTable').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'contractNo',
            url: '/ashx/PaymentManage/PaymentDomesticHandler.ashx?type=getSubList&payNo=' + payno,
            columns: [[
            { field: 'PAYNO', title: '到款单号', width: '100px', editor: 'text' },
            { field: 'CONTRACTNO', title: '合同编号', width: '100px', editor: 'text' },
            { field: 'ATTACHMENTNO', title: '附件编号', width: '100px', editor: 'text' },
            { field: 'RECEIVER', title: '收款方', width: '100px', editor: 'text' },
            { field: 'PAYER', title: '付款方', width: '100px', editor: 'text' },
            { field: 'PAYAMOUNT', title: '付款金额', width: '100px', editor: 'text' },
            { field: 'APPLYNO', title: '发货申请单号', width: '100px', editor: 'text' }
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
    }


});

    //添加
    function add () {
        $("#ddframe").attr('src', '/PaymentManage/DomesticPaymentForm1.aspx?action=add');
        $("#dd").dialog().title = "境内到款-添加";
        $("#dd").dialog('open');
    }
    //修改
    function edit() {
        var no = '';
        var row = $("#tt").datagrid('getSelected');
        if (row != undefined) {
            no = row.PAYNO;
            $("#ddframe").attr('src', '/PaymentManage/DomesticPaymentForm1.aspx?action=edit&payNo=' + no);
            $("#dd").dialog().title = "境内到款-修改";
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
        frame.form('submit', {
            url: "/ashx/PaymentManage/PaymentDomesticHandler.ashx?type=add",
            onSubmit: function(){ 
                //进行表单验证 
                //如果返回false阻止提交 
            }, 
            success:function(data){ 
                alert(data) 
            } 
        });

    }
    //获取子表数据
    function getSubTable() {
        var datagrid = $("#tt_subTable").datagrid("getRows");
        var datagridjson = JSON.stringify(datagrid);
        $("#datagrid").val(datagridjson);
      }