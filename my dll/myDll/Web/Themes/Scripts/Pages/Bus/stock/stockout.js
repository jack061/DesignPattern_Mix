//子表处理
var editRow = undefined;
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
            idField: 'outdocno',
            url: '/Bus/StockManage/StockHandler.ashx?action=stockoutlist',
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'status', title: '状态', width: '40px' },
                { field: 'outdocno', title: '出库单号', width: '120px', hidden: true },
                { field: 'cuscode', title: '客户编码', width: '80px', hidden: true },
                { field: 'seller', title: '供货单位', width: '190px' },
                { field: 'buyer', title: '收货单位', width: '190px' },
                { field: 'contractNo', title: '合同号', width: '90px' },
                { field: 'mname', title: '产品名称', width: '150px' },
                { field: 'outquantity', title: '出库数量', width: '70px' },
                { field: 'amount', title: '出库金额', width: '90px' },
                { field: 'outdate', title: '出库时间', width: '80px', formatter: formatDatebox },
                { field: 'createmanname', title: '库管员', width: '80px' },
                { field: 'remark', title: '备注', width: '150px' }
            ]],
            pagination: true
        });

        $("#dd").dialog({
            title: '新增出库',
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
});



//查询操作
function SearchData() {
    indocno = $("#outdocno").val();
    man = $("#man").val();
    beginTime = $("#beginTime").datebox('getValue');
    endTime = $("#endTime").datebox('getValue');
   
    para = {};
    para.outdocno = indocno;
    para.man = man;

    para.beginTime = beginTime;
    para.endTime = endTime;

    $("#tt").datagrid('load', para);
}


//添加
function add() {
    window.top.addNewTab("产品出库单-新增", '/Bus/StockManage/StockOutAdd.aspx?action=add');
}
//修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');

    if (row != undefined) {
        no = row.outdocno;
        window.top.addNewTab("产品出库单-修改", '/Bus/StockManage/StockOutAdd.aspx?action=edit&outdocno=' + no);
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//删除
function del() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined && row.status == '新建') {
        no = row.outdocno;
        $.messager.confirm('系统提示', '确认要删除么?</br></br>',
        function (r) {
            if (r) {
                $.post('/Bus/StockManage/StockHandler.ashx?action=delout', { outdocno: no },
	                function (msg) {
	                    if (msg.length > 0) {
	                        alert(msg);
	                    }
	                    else {
	                        SearchData();
	                    }
	                });
            }
        });
    } 
    else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//添加

////提交form表单
//function submitForm() {
//    alert('123');
//    $("#ddframe")[0].contentWindow.getSubTable();
//    var frame = $(window.frames["ddframe"].document).find("#form1");
//    var form = window.frames["ddframe"].document.getElementById("form1");
//    frame.form('submit', {
//        url: "/Bus/StockManage/StockHandler.ashx?action=saveout",
//        onSubmit: function () {
//            //进行表单验证 
//            return window.frames["ddframe"].valiForm();
//        },
//        success: function (data) {
//            alert(data)
//        }
//    });

//}

////校验form
//function valiForm() {
//    $('#cuscodevalue').attr('value', $('#cuscode').combobox('getValue'));
//    return $("#form1").form('validate');
//}
////获取子表数据
//function getSubTable() {
//    var datagrid = $("#tt_subTable").datagrid("getRows");
//    var datagridjson = JSON.stringify(datagrid);
//    $("#datagrid").val(datagridjson);
//}

var RefreshUI = function () {
    SearchData();
}

var submit = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.outdocno;
        submitbill(row.status, '提交', no, '出库');
    }
}

var review = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.outdocno;
        submitbill(row.status, '审核', no, '出库');
    }
}

var confirm = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.outdocno;
        submitbill(row.status, '确认', no, '出库');
    }
}