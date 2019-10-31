$(document).ready(function () {

    var toolbar = [{
        text: '查看箱单',
        iconCls: 'icon-add',
        handler: function () {
            var row = $("#tt").datagrid('getSelected');
            if (row != undefined) {
                no = row.contractNo;
                newPage('/InvoiceAndPacking/InvoiceOrPacking.aspx?type=packing&no='+no, "_blank");

            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }];
    //----初始化datagrid-----
    $('#tt').datagrid({
        rownumbers: true,
        sortName:'contractNo',
        sortOrder:'asc',
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        url: '/Bus/Xctl/ajax/AbroadLoadData.ashx?module=htpagelistfj',
        columns: [[
            { field: 'status1', title: '状态', width: '150px' },
            { field: 'contractNo', title: '合同编号', width: '200px' },
            { field: 'attachmentno', title: '附件编号', width: '150px' },
            { field: 'language', title: '语言', width: '200px' },
            { field: 'seller', title: '卖方', width: '100px' },
            { field: 'signedtime', title: '签订时间', width: '120px' },
            { field: 'signedplace', title: '签订地点', width: '120px' },
            { field: 'buyer', title: '买家', width: '120px' },
            { field: 'buyeraddress', title: '买家地址', width: '120px' },
            { field: 'currency1', title: '货币', width: '120px' },
            { field: 'pricement1', title: '价格条款1', width: '120px' },
            { field: 'pricement1per', title: '价格条款1占比', width: '120px' },
            { field: 'pricement2', title: '价格条款2', width: '120px' },
            { field: 'pricement2per', title: '价格条款2占比', width: '120px' },
            { field: 'pvalidity', title: '价格有效期', width: '120px' },
            { field: 'shipment', title: '发运条款', width: '120px' },
            { field: 'transport1', title: '运输方式', width: '120px' },
            { field: 'tradement', title: '贸易条款', width: '120px' },
            { field: 'harborout', title: '贸易条款', width: '120px' },
            { field: 'harborarrive', title: '贸易条款', width: '120px' },
            { field: 'harborout1', title: '出口口岸', width: '120px' },
            { field: 'harborarrive1', title: '到货口岸', width: '120px' },
            { field: 'harborclear1', title: '清关口岸', width: '120px' },
            { field: 'placement', title: '产地条款', width: '120px' },
            { field: 'validity', title: '合同有效期', width: '120px' },
            { field: 'remark', title: '备注', width: '120px' }
            ]],
        toolbar: toolbar,
        pagination: true
    });

});

//查询操作
function SearchData() {
    contractNo = $("#contractNo").val();
    attachmentno = $("#attachmentno").val();
    signedtime_begin = $('#signedtime_begin').datebox('getValue');
    signedtime_end = $('#signedtime_end').datebox('getValue');

    para = {};
    para.contractNo = contractNo;
    para.attachmentno = attachmentno;
    para.signedtime_begin = signedtime_begin;
    para.signedtime_end = signedtime_end;

    $("#tt").datagrid('load', para);
}