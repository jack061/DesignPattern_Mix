$(document).ready(function () {
    var toolbar = [{
        text: '报关发票',
        iconCls: 'icon-add',
        handler: function () {
            var row = $("#tt").datagrid('getSelected');
            if (row != undefined) {
                no = row.contractNo;
                window.top.closeAddTab('报关发票', '/InvoiceAndPacking/InvoiceOrPacking.aspx?type=invoice&no=' + no, "");
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }, {
        text: '报关箱单',
        iconCls: 'icon-add',
        handler: function () {
            var row = $("#tt").datagrid('getSelected');
            if (row != undefined) {
                no = row.contractNo;
                window.top.closeAddTab('报关箱单', '/InvoiceAndPacking/InvoiceOrPacking.aspx?type=packing&no=' + no, "");
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }, {
        text: '税务发票',
        iconCls: 'icon-add',
        handler: function () {
            var row = $("#tt").datagrid('getSelected');
            if (row != undefined) {
                no = row.contractNo;
                window.top.closeAddTab('税务发票', '/Bus/StockManage/CustomsDeclarationPrintForm.aspx?type=other&no=' + no, "");
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }];
    $('#tt').datagrid({
        sortName: 'contractNo',
        sortOrder: 'asc',
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        url: 'CustomsDeclaration.ashx?action=GetContractListNew',
        columns: [[
            { field: 'ck', checkbox: 'true' },
            { field: 'seqno', title: '税务流水号', width: '80px' },
            { field: 'customId', title: '报关单号', width: '120px' },
            { field: 'contractNo', title: '合同号', width: '150px' },
            { field: 'purchaseCode', title: '关联合同号', width: '150px' },
            { field: 'amount', title: '报关总金额', width: '100px' },
            { field: 'signedtime', title: '合同签订时间', width: '100px',formatter: formatDatebox },
            { field: 'buyer', title: '买方', width: '200px' },
            { field: 'seller', title: '卖方', width: '200px' },
            { field: 'harborarrive', title: '发货港口', width: '80px' },
            { field: 'harborout', title: '到货港口', width: '80px' },
            { field: 'transport', title: '运输方式', width: '80px' },
            { field: 'createMan', title: '创建人', width: '80px' },
            { field: 'createDate', title: '创建日期', width: '80px', formatter: formatDatebox }            
            ]],
        toolbar: [{
            text: '税务发票',
            iconCls: 'icon-add',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != undefined) {

                    var language = "zheg"; //语言
                    no = row.contractNo; //合同号
                    if (row.transport === '铁路')
                        window.top.addTab('铁路税务发票', '/Bus/StockManage/CustomsDeclarationRail.aspx?type=CustomsDeclarationPrint&contractNo=' + row.contractNo + "&isBrowse=true");
                    else
                        window.top.addNewTab('税务发票', '/Bus/StockManage/CustomsDeclarationPrintForm.aspx?type=other&no=' + no + "&language=" + language + "&choice=" + language + "&seqno=" + row.seqno, "");
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }],
        pagination: true
    });
});

//查询操作
function SearchData() {
    cdno = $("#cdno").textbox("getText");
    contractno = $("#contractno").textbox("getText");
    buyer = $('#buyer').textbox("getText");
    seller = $('#seller').textbox("getText");

    para = {};
    para.cdno = cdno;
    para.contractno = contractno;
    para.buyer = buyer;
    para.seller = seller;
    $("#tt").datagrid('load', para);
}