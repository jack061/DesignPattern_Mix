
//创建内部清算单
$(function () {
    $("#internalDiv").css("display", "block");
    //显示筛选条件
    $("#historyAttachContract").css("display", "block");
    getInternalCheckbox();
})


//创建内部清算单下一步单击事件
function interalnextWay() {
    //获取卖方或买方为关联方
    //var internalpeoplecheckbox = $("input[name='internalpeoplecheckbox']:checked").val();
    var row = $("#internaldataBase").datagrid('getSelected');
    window.top.closeAddTab("创建内部清算单", "/Bus/ContractCategory/internalClearingForm.aspx?contactContractNo=" + row.contractNo + '&isContactBuyer=true');
    //if (internalpeoplecheckbox == "seller") {
    //    //确定卖方为关联方
    //    window.top.closeAddTab("创建内部清算单", "/Bus/ContractCategory/internalClearingForm.aspx?contactContractNo=" + row.contractNo+'&isContactSeller=true');
    //}
    //if (internalpeoplecheckbox == "buyer") {
    //    //确定买方为关联方
    //    window.top.closeAddTab("创建内部清算单", "/Bus/ContractCategory/internalClearingForm.aspx?contactContractNo=" + row.contractNo + '&isContactBuyer=true');
    //}
}
//触发内部清算单下产品大类，业务员是、时间的选择事件
function getInternalCheckbox() {
    $('#internalProduct').combobox({
        required: true,
        valueField: 'productCategory',
        textField: 'productCategory',
        editable: true,
        multiple: false,
        data: sbProduct,
        onSelect: function (index, rowdata) {
        }
    });
    $('#internalSaleman').combogrid({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',
        editable: false,
        panelWidth: 200,
        textField: 'UserRealName',
        idField: 'UserRealName',
        columns:[[
           { field: 'UserRealName', title: '业务员',width:80 },
          { field: 'Agency', title: '部门',width:120 },
        ]],
        onSelect: function (index, rowdata) {
            var saleman = $("#internalSaleman").combogrid('getValue');
            var product = $("#internalProduct").combobox('getValue');
            getInternalContactData(saleman, product);
        }
    });
    //$('#internalTime').combobox({

    //    onSelect: function (index, rowdata) {
    //        var saleman = $("#internalSaleman").combobox('getValue');
    //        var time = $("#internalTime").combobox('getValue');
    //        var product = $("#internalProduct").combobox('getValue');
    //        getInternalContactData(saleman, time, product);
    //    }
    //});
}
//加载进境内部清算单数据表格
function getInternalContactData(saleman, product) {

    $("#internaldataBase").css("display", "block");
    $("#internaldataBase").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getInternalContract&saleman=' + saleman + '&product=' + product + '&flowdirection=' + importFlow,
        pagination: false,
        rownumbers: true,
        //fit: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'contractNo', title: '合同号', width: 100 },
                    { field: 'seller', title: '卖方', width: 100 },
                    { field: 'buyer', title: '买方', width: 100 },
        ]],
        onClickRow: function (rowNum, record) {
            //显示是否作为关联方Div
            //$("#internalContactDiv").css("display", "block");


        }

    })
}

