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
            idField: 'indocno',
            url: '/Bus/StockManage/StockHandler.ashx?action=stockinlist',
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'status', title: '状态', width: '40px' },
                { field: 'indocno', title: '入库单号', width: '120px',hidden:true },
                { field: 'supcode', title: '供应商编码', width: '80px', hidden: true },
                { field: 'seller', title: '供货单位', width: '190px' },
                { field: 'buyer', title: '收货单位', width: '190px' },
                { field: 'contractno', title: '合同号', width: '90px' },
                { field: 'mname', title: '产品名称', width: '130px' },
                { field: 'inquantity', title: '入库数量', width: '70px' },
                { field: 'amount', title: '入库金额', width: '90px' },
                { field: 'indate', title: '入库时间', width: '80px', formatter: formatDatebox },
                { field: 'createmanname', title: '库管员', width: '90px' },
                { field: 'remark', title: '备注', width: '150px' }
            ]],
            pagination: true
        });

        $("#dd").dialog({
            title: '新增入库',
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
        
    }
});
//查询操作
function SearchData() {
    indocno = $("#indocno").val();
    man = $("#man").val();
    beginTime = $("#beginTime").datebox('getValue');
    endTime = $("#endTime").datebox('getValue');

    para = {};
    para.indocno = indocno;
    para.man = man;
    para.beginTime = beginTime;
    para.endTime = endTime;


    $("#tt").datagrid('load', para);
}
//添加
function add() {
    window.top.addNewTab("产品入库单-新增", '/Bus/StockManage/StockInAdd.aspx?action=add');
}
//修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined && row.status == '新建') {
        no = row.indocno;
        window.top.addNewTab("产品入库单-修改", '/Bus/StockManage/StockInAdd.aspx?action=edit&indocno=' + no);
    } else if (row == undefined) {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//查看
function browse() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined && row.status != '新建') {
        no = row.indocno;
        window.top.addNewTab("产品入库单-查看", '/Bus/StockManage/StockInAdd.aspx?action=edit&indocno=' + no);
    } 
}
//删除
function del() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined && row.status == '新建') {
        no = row.indocno;
        $.messager.confirm('系统提示', '确认要删除么?</br></br>',
        function (r) {
            if (r) {
                $.post('/Bus/StockManage/StockHandler.ashx?action=delin', { indocno: no },
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
    else if(row == undefined){
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

var RefreshUI = function () {
    SearchData();
}

var submit = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.indocno;
        submitbill(row.status, '提交', no, '入库');
    }
}

var review = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.indocno;
        submitbill(row.status, '审核', no, '入库');
    }
}

var confirm = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.indocno;
        submitbill(row.status, '确认', no, '入库');
    }
}
