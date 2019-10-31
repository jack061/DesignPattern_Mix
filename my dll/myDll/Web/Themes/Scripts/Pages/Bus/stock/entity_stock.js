$(document).ready(function () {
    $("#contentDiv2").css("min-height", document.body.clientHeight - 20);
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //----初始化datagrid-----
        //结余列表
        $('#tt').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'indocno',
            url: '/Bus/StockManage/StockEntityHandler.ashx?action=getsotcklist',
            columns: [[
                { field: 'ck', checkbox: true },
                { field: 'WCODE', title: '仓库编号', width: '80px', hidden: true },
                { field: 'POSITION', title: '库位', width: '80px', hidden: true },
                { field: 'WNAME', title: '仓库', width: '80px'},
                { field: 'OWNER', title: '货权所属方', width: '160px' },
                { field: 'BATCHNO', title: '批次', width: '100px' },
                { field: 'MCODE', title: '产品编号', width: '100px' },
                { field: 'MNAME', title: '产品名称', width: '200px' },
                { field: 'MSPEC', title: '规格', width: '150px' },
                { field: 'UNIT', title: '单位', width: '70px' },
                { field: 'SUMCARNUMBER', title: '车皮数', width: '70px' },
                { field: 'NUMBER', title: '件数', width: '100px' },
                { field: 'QUANTITY', title: '数量', width: '120px' },
                { field: 'CREATEDATE', title: '入库时间', width: '100px' },
                { field: 'REMARK', title: '备注', width: '120px' }
            ]],
            pagination: true
        });
        addViewButton();
    }

    //----初始化datagrid-----
    //库存明细信息
    $('#stockdetail').datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        idField: 'contractno',
        url: '/Bus/StockManage/StockEntityHandler.ashx?action=getstocklog&mcode=' + mcode + '&batchno=' + batchno,
        columns: [[
            { field: 'doctype', title: '交易类型', width: '60px' },
            { field: 'trandate', title: '交易日期', width: '100px' },
            { field: 'docno', title: '单据号', width: '90px' },
            { field: 'wname', title: '仓库', width: '100px' },
            { field: 'owner', title: '货权所属方', width: '150px' },
            { field: 'batchno', title: '批次', width: '100px' },
            { field: 'mcode', title: '产品编号', width: '100px' },
            { field: 'mname', title: '产品名称', width: '120px' },
            { field: 'mspec', title: '规格', width: '100px' },
            { field: 'unit', title: '单位', width: '80px' },
            { field: 'carnumber', title: '车皮号', width: '100px' },
            { field: 'number', title: '件数', width: '100px' },
            { field: 'quantity', title: '数量', width: '120px' },
            { field: 'contractno', title: '合同号', width: '120px' },
            { field: 'seller', title: '卖方', width: '150px' },
            { field: 'buyer', title: '买方', width: '150px' },
            { field: 'remark', title: '备注', width: '200px' }
        ]],
        pagination: true
    });
});

//查询操作
function SearchData() {
    mcode = $("#mcode").val();
    mname = $("#mname").val();
    wname = $("#wname").val();
    owner = $("#owner").val();

    para = {};
    para.mcode = mcode;
    para.mname = mname;
    para.wname = wname;
    para.owner = owner;

    $("#tt").datagrid('load', para);
}

function addViewButton() {
    $("div[class='btabs']").append('<a href="javascript:void(0)" onclick="showdetail()"><span class="icon icon-nav">&nbsp;</span>查看明细信息</a>');
}

var showdetail = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        window.top.addNewTab("出入库详细信息", '/Bus/StockManage/StockDetail.aspx?mcode=' + row.MCODE + '&batchno=' + row.BATCHNO);
    }
}

