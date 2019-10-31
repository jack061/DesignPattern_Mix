var LISTNAME = "已发送报关合同";
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
        text: '形式发票',
        iconCls: 'icon-add',
        handler: function () {
            var row = $("#tt").datagrid('getSelected');
            if (row != undefined) {
                no = row.contractNo;
                window.top.closeAddTab('形式发票', '/InvoiceAndPacking/InvoiceOrPacking.aspx?type=other&no=' + no, "");
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }];
    //----初始化datagrid-----
    $('#tt').datagrid({

        sortName: 'contractNo',
        sortOrder: 'asc',
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        url: '/InvoiceAndPacking/ashx/GenerateInvoicePacking.ashx?action=GetContractList',
        columns: [[
            { field: 'ck', checkbox: 'true' },
            { field: 'customAccept', title: '接收状态', width: '100px', formatter: function (val) { if (val == 1) return "已接收"; else return "未接收"; } },
            { field: 'customMan', title: '报关人', width: '100px' },
            { field: 'businessclass', title: '业务组', width: '60px' },
            { field: 'iscustoms', title: '是否为报关合同', width: '100px' },
            { field: 'createmanname', title: '创建人', width: '60px' },
            { field: 'createdate', title: '创建日期', width: '100px', formatter: formatDatebox },
            { field: 'contractNo', title: '合同编号', width: '120px' },
            { field: 'purchaseCode', title: '关联合同号', width: '120px' },
            { field: 'seller', title: '卖方', width: '150px' },
            { field: 'buyer', title: '买方', width: '150px' },
            { field: 'transport', title: '运输方式', width: '80px' },
            { field: 'signedplace', title: '签订地点', width: '80px' },
            { field: 'currency', title: '货币', width: '50px' },
            { field: 'pricement1', title: '价格条款1', width: '80px' },
            { field: 'pricement1per', title: '价格条款1占比', width: '90px' },
            { field: 'pricement2', title: '价格条款2', width: '80px' },
            { field: 'pricement2per', title: '价格条款2占比', width: '90px' },
            { field: 'pvalidity', title: '价格有效期', width: '80px' },
            { field: 'shipment', title: '发运条款', width: '90px' },
            { field: 'tradement', title: '贸易条款', width: '80px' },
            { field: 'harborout1', title: '出口口岸', width: '80px' },
            { field: 'harborarrive1', title: '到货口岸', width: '80px' },
            { field: 'placement', title: '产地条款', width: '80px' },
            { field: 'validity', title: '合同有效期', width: '80px' },
            { field: 'signedtime', title: '签订时间', width: '100px', formatter: formatDatebox },
            { field: 'signedplace', title: '签订地点', width: '100px' },
            { field: 'salesmanCode', title: '业务员', width: '80px' },
            { field: 'language', title: '语言', width: '100px' },
            { field: 'remark', title: '备注', width: '120px' }
            ]],
        toolbar: [{
            text: '报关合同箱单发票',
            iconCls: 'icon-add',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != undefined) {
                    var language = "";
                    if (row.language === '中文') {
                        language = 'zh';
                    }
                    if (row.language.indexOf('俄文') > 0) {
                        language = 'zhrs';
                    }
                    if (row.language.indexOf('英文') > 0) {
                        language = 'zheg';
                    }
                    no = row.contractNo;
                    if (row.transport == '铁路') {
                        window.top.addNewTab('铁路' + LISTNAME, '/InvoiceAndPacking/InvoiceOrPackingRailList.aspx?type=all&no=' + no + "&language=" + language + "&choice=" + language, "");
                        return;
                    }
                    var customMan = row.customMan;
                    if (row.customMan == '') customMan = 'null';
                    window.top.addNewTab('非铁路' + LISTNAME, "/Bus/StockManage/CustomsDeclarationSendForm.aspx?action=send&type=非铁路&no=" + no + "&language=" + language + "&choice=" + language + "&customMan=" + (customMan), "");
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        },
        {
            text: '接收',
            iconCls: 'icon-add',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != undefined) {
                    var no = row.contractNo;
                    $.ajax({
                        cache: true,
                        type: "POST",
                        url: '/InvoiceAndPacking/ashx/GenerateInvoicePacking.ashx?action=CustomAccept&contractNo=' + no,
                        async: false,
                        error: function (data) {
                        },
                        success: function (data) {
                            if (data == 'yes') {
                                alert('操作成功');
                                $('#tt').datagrid("reload");
                            }
                            else {
                                //像用户提示错误
                                $.messager.alert("error", "操作失败");
                            }
                        }
                    });
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }
        //        , {
        //            text: '报关箱单',
        //            iconCls: 'icon-add',
        //            handler: function () {
        //                var row = $("#tt").datagrid('getSelected');
        //                if (row != undefined) {
        //                    var language = "";
        //                    if (row.language === '中文') {
        //                        language = 'zh';
        //                    }
        //                    if (row.language.indexOf('俄文') > 0) {
        //                        language = 'zhrs';
        //                    }
        //                    if (row.language.indexOf('英文') > 0) {
        //                        language = 'zheg';
        //                    }

        //                    no = row.contractNo;
        //                    if (row.transport == '铁路') {
        //                        window.top.addNewTab('铁路报关箱单', '/InvoiceAndPacking/InvoiceOrPackingRailList.aspx?type=packing&no=' + no + "&language=" + language + "&choice=" + language, "");
        //                        return;
        //                    }
        //                    window.top.addNewTab('报关箱单', '/InvoiceAndPacking/InvoiceOrPacking.aspx?type=packing&no=' + no + "&language=" + language + "&choice=" + language, "");
        //                } else {
        //                    $.messager.alert('提示', '请选择一行数据！', 'info');
        //                }
        //            }
        //        }
        ],
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