$(document).ready(function () {
    $("#contentDiv2").css("min-height", document.body.clientHeight - 20);
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
            url: '/Bus/StockManage/StockHandler.ashx?action=getsotcklist',
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'wcode', title: '仓库', width: '80px', hidden: true },
                { field: 'position', title: '库位', width: '80px', hidden: true },
                { field: 'contractno', title: '合同号', width: '80px' },
                { field: 'buyer', title: '买方', width: '160px' },
                { field: 'seller', title: '卖方', width: '160px' },
                { field: 'mname', title: '产品名称', width: '100px' },
                { field: 'quantity', title: '数量', width: '80px' },
                { field: 'price', title: '单价', width: '80px' },
                { field: 'amount', title: '金额', width: '90px' },
                { field: 'unit', title: '单位', width: '70px' },
                { field: 'mspec', title: '规格', width: '70px' },
                { field: 'remark', title: '备注', width: '120px' }
            ]],
            pagination: true
        });
        addViewButton();
    }

    //----初始化datagrid-----
    $('#stockdetail').datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        idField: 'contractno',
        url: '/Bus/StockManage/StockHandler.ashx?action=getstocklog&mcode=' + mcode,
        columns: [[
            { field: 'stocktype', title: '类型', width: '50px' },
            { field: 'confirmdate', title: '日期', width: '120px' },
            { field: 'contractno', title: '合同号', width: '90px' },
            { field: 'seller', title: '卖方', width: '190px' },
            { field: 'buyer', title: '买方', width: '190px' },
            { field: 'quantity', title: '数量', width: '90px' },
            { field: 'amount', title: '金额', width: '90px' },
            { field: 'unit', title: '单位', width: '90px' },
            { field: 'mspec', title: '规格', width: '90px' },
            { field: 'remark', title: '备注', width: '100px' }
        ]],
        pagination: true
    });
});

//查询操作
function SearchData() {
    mcode = $("#mcode").val();
    mname = $("#mname").val();
    beginTime = $("#beginTime").val();
    endTime = $("#endTime").val();

    para = {};
    para.mcode = mcode;
    para.mname = mname;
    para.beginTime = beginTime;
    para.endTime = endTime;

    $("#tt").datagrid('load', para);
}

function addViewButton() {
    $("div[class='btabs']").append('<a href="javascript:void(0)" onclick="showdetail()"><span class="icon icon-nav">&nbsp;</span>查看明细</a>');
}

var showdetail = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        window.top.addNewTab("出入库详细", '/Bus/StockManage/StockDetail.aspx?mcode=' + row.mcode);
    }
}

