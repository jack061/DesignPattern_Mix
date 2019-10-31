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
            idField: 'OUTDOCNO',
            url: '/Bus/StockManage/StockEntityHandler.ashx?action=stockoutlist1',
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'CHECKID', title: '关联id', width: '80px', hidden: true },
                { field: 'STATUS', title: '状态', width: '80px' },
                { field: 'OUTDOCNO', title: '出库单号', width: '120px'},
                { field: 'WNAME', title: '仓库名称', width: '100px' },
                { field: 'SELLER', title: '卖方', width: '150px' },
                { field: 'BUYER', title: '买方', width: '150px' },
                { field: 'SALECONTRACT', title: '合同号', width: '100px' },
                { field: 'MNAME', title: '产品名称', width: '120px' },
                { field: 'NUMBER', title: '通知件数', width: '90px' },
                { field: 'OUTQUANTITY', title: '通知数量', width: '70px' },
                { field: 'REALNUMBER', title: '实出件数', width: '90px' },
                { field: 'REALOUTQUANTITY', title: '实出数量', width: '70px' },
                { field: 'CREATEMANNAME', title: '制单人', width: '80px' },
                { field: 'CREATEDATE', title: '制单时间', width: '120px' },
                { field: 'STOCKMANAGER', title: '库管员', width: '80px' },
                { field: 'STOCKMANAGERCONDATE', title: '出库时间', width: '120px' },
                { field: 'REMARK', title: '备注', width: '150px' }

            ]],
            pagination: true
        });

    }
});


//查询操作
function SearchData() {
    wname = $("#wname").val();
    seller = $("#seller").val();
    buyer = $("#buyer").val();
    beginTime = $("#beginTime").datebox('getValue');
    endTime = $("#endTime").datebox('getValue');
   
    para = {};
    para.wname = wname;
    para.seller = seller;
    para.buyer = buyer;
    para.beginTime = beginTime;
    para.endTime = endTime;

    $("#tt").datagrid('load', para);
}


//添加
function add() {
    window.top.addNewTab("出库通知单-新增", '/Bus/StockManage/StockEntityOutNewAdd.aspx?action=add');
//    var no = '';
//    var row = $("#tt").datagrid('getSelected');

//    if (row != undefined) {
//        if ("" == row.outstatus) {
//            no = row.checkId;
//            window.top.addNewTab("产品出库单-新增", '/Bus/StockManage/StockEntityOutAdd.aspx?action=add&checkid=' + no);
//        } else {
//            $.messager.alert('提示', '请选择未出库的单据！', 'info');
//         }
//        
//    } else {
//        $.messager.alert('提示', '请选择一行数据！', 'info');
//    }
}
//修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');

    if (row != undefined) {
        if ("新建" == row.STATUS) {
            no = row.OUTDOCNO;
            window.top.addNewTab("出库通知单-修改", '/Bus/StockManage/StockEntityOutNewAdd.aspx?action=edit&outdocno=' + no );
        }
        else {
            $.messager.alert('提示', '请选择新建状态的出库通知单！', 'info');
         }
    } else {
        $.messager.alert('提示', '请选择要修改的数据！', 'info');
    }
}
//查看
function browse() {
    var no = '';
    var checkId = ''
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.OUTDOCNO;
        window.top.addNewTab("出库通知单-查看", '/Bus/StockManage/StockEntityOutAdd.aspx?action=browse&outdocno=' + no);
    } else {
        $.messager.alert('提示', '请选择要查看的数据！', 'info');
    }
}

//删除
function del() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined && row.STATUS == '新建') {
        no = row.OUTDOCNO;
        $.messager.confirm('提示', '确认删除出库通知单?</br></br>',
        function (r) {
            if (r) {
                $.post('/Bus/StockManage/StockEntityHandler.ashx?action=delout', { outdocno: no },
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
	    else if (row == undefined) {
	        $.messager.alert('提示', '请选择要删除的数据！', 'info');
	    }
	    else {
	        $.messager.alert('提示', '数据不能删除，请选择新建状态数据！', 'info');
	    }
}


var RefreshUI = function () {
    SearchData();
}

var submit = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.outdocno;
        submitbill(row.outstatus, '提交', no, '出库');
    }
}

var review = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.outdocno;
        submitbill(row.outstatus, '审核', no, '出库');
    }
}

var confirm = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.outdocno;
        submitbill(row.outstatus, '确认', no, '出库');
    }
}