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
            idField: 'checkId',
            sortName: 'checkId',
            sortOrder: 'desc',
            url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=ckpagelist',
            columns: [[
                { field: 'ck', checkbox: true, width: 60 },
                { field: 'STATUS', title: '状态', width: 60 },
                { field: 'businessGroup', title: '销售组', width: 80 },
                { field: 'saleContract', title: '销售合同号', width: 120 },
                { field: 'purchaseContract', title: '采购合同号', width: 80 },
                { field: 'buyer', title: '买方', width: 80 },
                { field: 'seller', title: '卖方', width: 80 },
                { field: 'isConsignor', title: '是否作为发货人', width: 80 },
                { field: 'isConsignee', title: '是否作为收货人', width: 100 },
                { field: 'Consignor', title: '发货人', width: 100 },
                { field: 'Consignee', title: '收货人', width: 100 },
                { field: 'ConsignorReport', title: '发货人声明', width: 100 },
                { field: 'fromStation', title: '发站', width: 100 },
                { field: 'fromStationCode', title: '发站代码', width: 100 },
                { field: 'destination', title: '到站', width: 100 },
                { field: 'destinationCode', title: '到站代码', width: 100 },
                { field: 'transitPayCode', title: '过境付费代码', width: 80 }
                ]],

            pagination: true
        });
    }

});

function setReadOnly() { //设置为只读
    $('input').attr("readonly", true);
}

//显示消息
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

//查看
var browse = function () {
    var checkId = '';

    var row = $("#tt").datagrid('getSelected');

    if (row != undefined) {
        checkId = row.checkId;

        window.top.addNewTab("铁路发货出库指令-查看", "/Bus/TrainCheckOut/trainCheckForm.aspx?checkId=" + checkId + "&action=view");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//预览
function preview() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.checkId;
        window.open('/TrainApply/PrintTrainApply.aspx?checkId=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}