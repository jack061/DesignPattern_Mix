var editIndex = undefined;

$(document).ready(function () {
    $('#tt').datagrid({

        sortName: 'contractNo',
        sortOrder: 'asc',
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        url: 'ashx/ImportCustomsDec.ashx?action=GetContractList',
        columns: [[
            { field: 'ck', checkbox: 'true' },
             { field: 'customId', title: '报关单号', width: '100px' },
            { field: 'customMan', title: '报关人', width: '100px' },
            { field: 'cdcheck', title: '校对状态', width: '70px', formatter: function (val) { if (val == 1) return "已校对"; else return "未校对"; } },
            { field: 'businessclass', title: '业务组', width: '80px' },
            { field: 'iscustoms', title: '是否为报关合同', width: '100px',align:'center' },
            { field: 'createmanname', title: '创建人', width: '60px' },
            { field: 'createdate', title: '创建日期', width: '100px', formatter: formatDatebox },
            { field: 'contractNo', title: '合同编号', width: '120px' },
            { field: 'purchaseCode', title: '关联合同号', width: '120px' },
            { field: 'seller', title: '卖方', width: '150px' },
            { field: 'buyer', title: '买方', width: '150px' },
            { field: 'transport', title: '运输方式', width: '80px' },
            { field: 'signedplace', title: '签订地点', width: '80px' },
            { field: 'currency', title: '货币', width: '80px' },
            { field: 'pricement1', title: '价格条款1', width: '80px' },
            { field: 'pricement1per', title: '价格条款1占比', width: '80px' },
            { field: 'pricement2', title: '价格条款2', width: '80px' },
            { field: 'pricement2per', title: '价格条款2占比', width: '80px' },
            { field: 'pvalidity', title: '价格有效期', width: '80px' },
            { field: 'shipment', title: '发运条款', width: '80px' },
            { field: 'tradement', title: '贸易条款', width: '80px' },
            { field: 'harborout1', title: '出口口岸', width: '80px' },
            { field: 'harborarrive1', title: '到货口岸', width: '80px' },
            { field: 'placement', title: '产地条款', width: '80px' },
            { field: 'validity', title: '合同有效期', width: '80px' },
            { field: 'signedtime', title: '签订时间', width: '100px', formatter: formatDatebox },
            { field: 'signedplace', title: '签订地点', width: '100px' },
            { field: 'salesmanCode', title: '业务员', width: '80px' },
            { field: 'language', title: '语言', width: '100px' },
            { field: 'remark', title: '备注', width: '120px' },
            ]],
        toolbar: [{
            text: '报关单校对',
            iconCls: 'icon-add',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != undefined) {
                    if (row.cdcheck == 1) {
                        $.messager.alert('提示', '已校对合同不能重复校对！');
                        return;
                    }
                    if (row.transport === '铁路')
                        window.top.addTab('铁路报关单校对', '/Bus/StockManage/CustomsDeclarationRail.aspx?type=CustomsDeclaration&contractNo=' + row.contractNo + "&isBrowse=false");
                    else
                        window.top.addTab('海运报关单校对', '/Bus/StockManage/CustomsDeclarationForm_imp.aspx?contractNo=' + row.contractNo + "&isBrowse=false");
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }, {
            text: '报关单查看',
            iconCls: 'icon-help',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row.cdcheck != 1) {
                    $.messager.alert('提示', '已校对合同不能重复校对！');
                    return;
                }
                if (row != undefined) {
                    if (row.transport === '铁路')
                        window.top.addTab('铁路报关单查看', '/Bus/StockManage/CustomsDeclarationRail.aspx?type=CustomsDeclarationBrowse&contractNo=' + row.contractNo + "&isBrowse=true");
                    else
                        window.top.addTab('报关单校对-查看', '/Bus/StockManage/CustomsDeclarationForm_imp.aspx?contractNo=' + row.contractNo + "&isBrowse=true");
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }
        ],
        pagination: true
    });

});

//查询操作
function SearchData() {
    contractNo = $("#contract").val();
    attachmentno = $("#attachmentno").val();
    signedtime_begin = $('#signedtime_begin').datebox('getValue');
    signedtime_end = $('#signedtime_end').datebox('getValue');
    ifchecked = $("input:radio[name='partInfo']:checked").val();

    para = {};
    para.contractNo = contractNo;
    para.attachmentno = attachmentno;
    para.signedtime_begin = signedtime_begin;
    para.signedtime_end = signedtime_end;
    para.ifchecked = ifchecked;
    $("#tt").datagrid('load', para);
}