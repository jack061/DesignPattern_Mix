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
            url: '/Bus/StockManage/StockEntityHandler.ashx?action=stockoutlist2',
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'CHECKID', title: '关联id', width: '80px', hidden: true },
                { field: 'STATUS', title: '状态', width: '80px' },
                { field: 'OUTDOCNO', title: '出库单号', width: '120px' },
                { field: 'WNAME', title: '仓库名称', width: '100px' },
                { field: 'CONTRACTNO', title: '合同号', width: '100px' },
                { field: 'SELLER', title: '卖方', width: '150px' },
                { field: 'BUYER', title: '买方', width: '150px' },
                { field: 'CHECKNOTICENUMBER', title: '订舱申请号', width: '100px' },
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
//                { field: 'outstatus', title: '状态', width: '40px',
//                    formatter: function (value, row, index) {
//                        if ("" == value) {
//                            return "未出库";
//                        } else {
//                            return value;
//                        }
//                    }
//                },
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


//确认
function confirm() {
    var no = '';
    var contractno = ''
    var checkid = ''
    var row = $("#tt").datagrid('getSelected');

    if (row != undefined) {
        no = row.OUTDOCNO;
        contractno = row.SALECONTRACT;
        chekid = row.CHECKID;
        window.top.addNewTab("库管出库确认-确认", '/Bus/StockManage/StockEntityOutAdd.aspx?action=edit&outdocno=' + no + '&chekid=' + chekid);
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//查看
function browse() {
    var no = '';
    var checkId = ''
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.OUTDOCNO;
        checkId = row.CHECKID;
        window.top.addNewTab("库管出库确认-查看", '/Bus/StockManage/StockEntityOutAdd.aspx?action=browse&outdocno=' + no);
    } else {
        $.messager.alert('提示', '请选择要查看的数据！', 'info');
    }
}
function checkView() {
    var checkId = '';

    var row = $("#tt").datagrid('getSelected');

    if (row != undefined) {
        checkId = row.CHECKID;

        window.top.addNewTab("海运订舱-查看", "/Bus/CheckOutNotice/checkNoticeView.aspx?action=add&checkNoticeNumber=" + row.CHECKNOTICENUMBER + "&contractNo=" + row.CONTRACTNO + "&isBrowse=true");
    } else {
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
//        no = row.outdocno;
//        submitbill(row.outstatus, '提交', no, '出库');
//    }
//}

//var review = function () {
//    var no = '';
//    var row = $("#tt").datagrid('getSelected');
//    if (row != undefined) {
//        no = row.outdocno;
//        submitbill(row.outstatus, '审核', no, '出库');
//    }
//}

//var confirm = function () {
//    var no = '';
//    var row = $("#tt").datagrid('getSelected');
//    if (row != undefined) {
//        no = row.outdocno;
//        submitbill(row.outstatus, '库管员确认', no, '出库');
//    }
//}