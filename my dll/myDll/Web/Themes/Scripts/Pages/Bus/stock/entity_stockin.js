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
            idField: 'INDOCNO',
            url: '/Bus/StockManage/StockEntityHandler.ashx?action=stockinlist&listflag=2',
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'STATUS', title: '状态', width: '60px' },
                { field: 'INDOCNO', title: '入库单号', width: '100px' },
                { field: 'WNAME', title: '仓库名称', width: '80px' },
                { field: 'OWNER', title: '货权所属方', width: '120px' },
                { field: 'MNAME', title: '产品名称', width: '120px' },
                { field: 'MSPEC', title: '规格', width: '80px' },
                { field: 'UNIT', title: '单位', width: '60px' },
                { field: 'PACK', title: '最小包装', width: '60px' },
                { field: 'PACKUNIT', title: '包装单位', width: '60px' },
                { field: 'PACKDES', title: '包装', width: '80px' },
                { field: 'SUMCARNUMBER', title: '车皮数', width: '70px' },
                { field: 'NUMBER', title: '通知件数', width: '80px' },
                { field: 'INQUANTITY', title: '通知数量', width: '80px' },
                { field: 'LOSSNUMBER', title: '接收少件', width: '80px' },
                { field: 'REALNUMBER', title: '接收件数', width: '80px' },
                { field: 'REALINQUANTITY', title: '接收数量', width: '80px' },
                { field: 'CREATEMANNAME', title: '制单人', width: '80px' },
                { field: 'CREATEDATE', title: '制单时间', width: '120px' },
                { field: 'STOCKMANAGER', title: '库管员', width: '80px' },
                { field: 'STOCKMANAGERCONDATE', title: '入库时间', width: '120px' },
            //                { field: 'BUSMAN', title: '业务确认', width: '80px' },
            //                { field: 'BUSMANCONDATE', title: '确认时间', width: '120px' },
                {field: 'REMARK', title: '备注', width: '150px' }
            ]],
            pagination: true
        });
    }

    if (pageTag == "form") {
        
    }
});



//查询操作
function SearchData() {
    var listflag = $("#listflag").val();
    var wname = $("#wname").val();
    var owner = $("#owner").val();
    var status = $('input:radio[name="status"]:checked').val();
    var beginTime = $("#beginTime").datebox('getValue');
    var endTime = $("#endTime").datebox('getValue');

    para = {};
    para.listflag = listflag;
    para.wname = wname;
    para.owner = owner;
    para.status = status;
    para.beginTime = beginTime;
    para.endTime = endTime;


    $("#tt").datagrid('load', para);
}

//收货确认
function receive() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined &&row.STATUS == '收货') {
        $.messager.alert('提示', '请选择待收货数据！', 'info');
    } else if (row != undefined &&row.STATUS == '确认') {
        $.messager.alert('提示', '请选择待收货数据！', 'info');
    } else if (row != undefined) {
        no = row.INDOCNO;
        window.top.addNewTab("产品入库单-收货", '/Bus/StockManage/StockEntityInAdd.aspx?action=receive&indocno=' + no);
    }  
    else if (row == undefined) {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//查看
function browse() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined && row.STATUS != '新建') {
        no = row.INDOCNO;
        window.top.addNewTab("产品入库单-查看", '/Bus/StockManage/StockEntityInAdd.aspx?action=browse&indocno=' + no);
    } else if (row == undefined) {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}


var RefreshUI = function () {
    SearchData();
}

//var submit = function () {
//    var no = '';
//    var row = $("#tt").datagrid('getSelected');
//    if (row != undefined) {
//        no = row.INDOCNO;
//        submitbill(row.STATUS, '接收', no, '入库');
//    }
//}

//var review = function () {
//    var no = '';
//    var row = $("#tt").datagrid('getSelected');
//    if (row != undefined) {
//        no = row.INDOCNO;
//        submitbill(row.STATUS, '审核', no, '入库');
//    }
//}

//var confirm = function () {
//    var no = '';
//    var row = $("#tt").datagrid('getSelected');
//    if (row != undefined) {
//        no = row.INDOCNO;
//        submitbill(row.STATUS, '接收', no, '入库');
//    }
//}

//var sendback = function () {
//    var no = '';
//    var row = $("#tt").datagrid('getSelected');
//    if (row != undefined) {
//        no = row.INDOCNO;
//        submitbill(row.STATUS, '退回', no, '入库');
//    }
// }