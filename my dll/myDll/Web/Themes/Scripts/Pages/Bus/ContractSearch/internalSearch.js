$(function () {
    $('#maingrid').datagrid({
        border: 2,
        nowrap: false,
        fit: true,
        url: '/ashx/Contract/contractData.ashx?module=getInternalListByRoles',
        frozenColumns: [[
            { field: 'createmanname', title: '创建人', width: 100 },
            { field: 'createdate', title: '创建日期', width: 100 },
            { field: 'status', title: '状态', width: 100 },
            {
                field: 'printStatus', title: '打印状态', width: 100, formatter: function (value, row, index) {
                    if (value == '1') {
                        return '已下载';
                    }
                    else if (value == '2') {
                        return '已打印';
                    }
                    else {
                        return '未打印';
                    }
                }
            },
            { field: 'contractNo', title: '合同编号', width: 100 },
            { field: 'buyer', title: '买方', width: 100 },
            { field: 'seller', title: '卖方', width: 100 },

        ]],
        columns: [[

         { "title": "合同金额", "colspan": 3 }],
        [{ "field": "contractAmount", "title": "总金额", "rowspan": 1 },
         { "field": "paidAmount", "title": "已付金额", "rowspan": 1 },
         { "field": "unpaidAmount", "title": "未付金额额", "rowspan": 1 },

        ]],
        rownumbers: true
    });
    doSearch();
})
//查看
function browse() {
    var no = '';
    var row = $("#maingrid").datagrid('getSelected');
    no = row.contractNo;
    if (row != undefined) {
        window.top.addNewTab("内部结算单-查看", '/Bus/ContractCategory/InternalClearingForm.aspx?isBrowse=true&contractNo=' + no, '');
    }
}

//预览
function preview() {

    var no = '';
    var templateno = "";
    var language = "";
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.contractNo;
        templateno = row.templateno;
        language = row.language;

        window.top.addNewTab("内部清算单-预览", '/Bus/ContractCategory/internalClearingTemplate.aspx?contractNo=' + no, '');

    }
};

var doSearch = function (data) {

    var queryData = {};
    queryData.contractNo = $('#contractNo').textbox('getValue');
    queryData.signedtime_begin = $('#signedtime_begin').datebox('getValue');
    queryData.signedtime_end = $('#signedtime_end').datebox('getValue');
    queryData.businessclass = $('#businessclass').textbox('getValue');
    $('#maingrid').datagrid({ queryParams: queryData });
}