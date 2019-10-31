$(function () {
    $('#printlist').datagrid({
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=checkPayCode&createDateTag=' + createDateTag + '&noticeTag=' + noticeTag,
        rownumbers: true,
        singleSelect: true,
        sortName: 'payCode',
        sortOrder: 'asc',
        columns: [[
                     {
                         field: 'printCount', title: '已打印次数', width: 100, formatter: function (value, row, index) {
                             return value == "" ? 0 : value;
                         }
                     },
		              { field: 'noticeTag', title: '通知编码', width: 100 },
                      { field: 'payCode', title: '付费代码', width: 100 },
		             { field: 'transitPayCode', title: '过境1', width: 100, align: 'center' },
                     { field: 'transitPayCode2', title: '过境2', width: 100, align: 'center' },
                     { field: 'transitPayCode3', title: '过境3', width: 100, align: 'center' },
                     { field: 'transitPayCode4', title: '过境4', width: 100, align: 'center' },
                     { field: 'containerSize', title: '集装箱规格', width: 100, align: 'center' },
                     { field: 'carriageNumber', title: '车厢号', width: 100, align: 'center' },
                     { field: 'productWeight', title: '产品重量', width: 100, align: 'center' },
                     { field: 'contractNo', title: '合同号', width: 100, align: 'center' },
        ]],
        toolbar: [{
            text: '打印',
            iconCls: 'icon-add',
                handler: function () {
                    var payRows = $('#printlist').datagrid('getSelected');
                    //window.top.addNewTab('铁路运单打印', '/TrainApply/PrintTrainApply.aspx?contractNo=' + payRows.contractNo + '&payCode=' + payRows.payCode + '&createDateTag=' + createDateTag + '&noticeTag=' + noticeTag, "_blank", "");
                    window.top.addNewTab('铁路运单打印', '/TrainApply/PrintTrainApply.aspx?contractNo=' + payRows.contractNo+'&id='+payRows.id+'&createDateTag=' + createDateTag + '&noticeTag=' + noticeTag, "_blank", "");
                }
            }
            ]

    });

})