$(function () {
    $('#maingrid').datagrid({
        border: 1,
        nowrap: false,
        fitColumns: true,
        url: '/ashx/Contract/contractData.ashx?module=getExportListByRoles',
        frozenColumns: [[
            { field: 'createmanname', title: '创建人' },
            { field: 'createdate', title: '创建日期' },
            { field: 'status1', title: '状态' },
            { field: 'contractNo', title: '合同编号'},
            { field: 'buyer', title: '买方',width:120 },
            { field: 'seller', title: '卖方', width: 120 },
            { field: 'transport', title: '运输方式'},
              {
                  field: 'UnProcessInpsectStatus', title: '待处理商检申请状态', formatter: function (value, row, index) {
                      if (value == '1') {
                          return '已处理';
                      }
                      else if (value == '2') {
                          return '已确认';
                      }
                      else {
                          return '未处理';
                      }
                  }
              },
                   {
                       field: 'bookingstatus', title: '海运订舱申请状态',  formatter: function (value, row, index) {
                           if (value == '1') {
                               return '已完成';
                           }
                           else {
                               return '未完成';
                           }
                       }
                   },
                     {
                         field: 'sendOutStatus', title: '发货状态', formatter: function (value, row, index) {
                             if (value == '1') {
                                 return '已申请';
                             }
                             else if (value == '2') {
                                 return '已确认';
                             }
                             else {
                                 return '未发货';
                             }
                         }
                     },
                     {
                         field: 'inspectionStatus', title: '商检状态', formatter: function (value, row, index) {
                             if (value == '1') {
                                 return '已申请商检';
                             }
                             else if (value == '2') {
                                 return '已商检';
                             }
                             else {
                                 return '未申请商检';
                             }
                         }
                     },
                          {
                              field: 'cdcheck', title: '报关校对状态', formatter: function (value, row, index) {
                                  if (value == '1') {
                                      return '已校对';
                                  }
                             
                                  else {
                                      return '未校对';
                                  }
                              }
                          },
                          {
                              field: 'creditFinish', title: '信用证开立状态',  formatter: function (value, row, index) {
                                  if (value == '1') {
                                      return '已完成';
                                  }

                                  else {
                                      return '未完成';
                                  }
                              }
                          }

        ]],
        columns: [
            [{ "title": "合同金额", "colspan": 3 },
            { "title": "发货量", "colspan": 2 },
            {"title":"合同收款","colspan":4}],
        [{ "field": "contractAmount", "title": "总金额", "rowspan": 1 },
         { "field": "item1Amount", "title": "条款1金额", "rowspan": 1 },
         { "field": "item2Amount", "title": "条款2金额", "rowspan": 1 },
        { "field": "quantity", "title": "合同总量", "rowspan": 1 },
        { "field": "sendQuantity", "title": "已发货量", "rowspan": 1 },
        { "field": "sendQuantity", "title": "已收金额", "rowspan": 1 },
        { "field": "sendQuantity", "title": "未收金额", "rowspan": 1 },
        { "field": "sendQuantity", "title": "本次收款", "rowspan": 1 },
        { "field": "sendQuantity", "title": "扣费金额", "rowspan": 1 }
        ]
        ],
        rownumbers: true
    });
    doSearch();
})
//获取价格有效期
function getValidityDate(row, value) {
    if (value == "" || value == null) {
        return row.pvalidity;
    }
    else {
        return value;
    }
}
//查看
function browse() {
    var isContactEdit = "";
    var no = '';
    var row = $("#maingrid").datagrid('getSelected');
    no = row.contractNo;
    if (row.purchaseCode != null && row.purchaseCode != "") {
        isContactEdit = "true";
    }
    if (row != undefined && row.outTag == outTag) {
        window.top.addNewTab("外部文本合同-查看", '/Bus/ContractCategory/exportOutsideContractForm.aspx?contractNo=' + row.contractNo + '&isBrowse=true', '');
    }
    else {
        window.top.addNewTab("出口合同-查看", '/Bus/ContractCategory/exportContractTest.aspx?contractNo=' + row.contractNo + '&isBrowse=true&isContactEdit=' + isContactEdit, '');
    }

}
//预览
function preview() {
    var no = '';
    var row = $("#maingrid").datagrid('getSelected');
    no = row.contractNo;
    if (row != undefined) {
        window.top.addNewTab("出口合同-预览", '/Bus/ContractCategory/contractTemplate.aspx?language=' + row.language + '&contractno=' + no + '&tableName=Econtract', '');
    }
}

//查找
function doSearch() {
    var queryData = {};
    queryData.contractNo = $('#contractNo').textbox('getValue');
    queryData.signedtime_begin = $('#signedtime_begin').datebox('getValue');
    queryData.signedtime_end = $('#signedtime_end').datebox('getValue');

    queryData.businessclass = $('#businessclass').textbox('getValue');
    $('#maingrid').datagrid({ queryParams: queryData });
}

//查看合同审批表
function checkContractApproval() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.contractNo;
        window.top.addTab('查看合同审批表', '/Bus/Contract/ContractApproval.aspx?type=export&contractNo=' + no, "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}