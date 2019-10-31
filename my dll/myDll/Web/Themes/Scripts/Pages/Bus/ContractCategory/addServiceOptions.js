var contactCopyStaus = "";//定义选择关联创建或复制创建
$(function () {
    chooseCreateStyle();
    bindUI();
})
function bindUI() {
    //填充业务员combobox选择框，根据下拉选择业务员和时间筛选合同
    $("#attachSaleman").combobox({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',
        valueField: 'UserRealName',
        textField: 'UserRealName',
        editable: false,
        onSelect: function (record) {
            var time = $("input[name='contractTime']:checked").val();
            var attachSaleman = $('#attachSaleman').combobox('getValue');
            var url = '/ashx/Contract/contractData.ashx?module=getServiceContract&attachSaleman=' + attachSaleman + '&time=' + time + '&isFrame=true';
            //根据业务员和时间筛选合同
            //#region Main 
            CreateAloneAttachGetData(url);
        }
    });

}
////下一步单击事件
function nextWay() {
    switch ($("input[name=independentCreateStyle]:checked").attr("id"))
    {
        case "main"://创建主合同
            switch (contactCopyStaus) {
                case "contact"://创建关联合同
                    var row = $("#mainContactCopyTab").datagrid('getSelected');
                    window.top.closeAddTab("物流关联合同-新增", '/Bus/ContractCategory/ServiceForm.aspx?isFrame=false&contractNo='+row.contractNo+'&isContact=true', '');
                    break;
            
                case "copy"://复制创建合同
                    var row = $("#mainContactCopyTab").datagrid('getSelected');
                    window.top.closeAddTab("物流主合同-新增", '/Bus/ContractCategory/ServiceForm.aspx?isFrame=false&contractNo=' + row.contractNo + '&isCopy=true', '');
                    break;
                case "new"://新建主合同
                    window.top.closeAddTab("物流主合同-新增", '/Bus/ContractCategory/ServiceForm.aspx?isFrame=false', '');
                    break;
                default:
                    break;
            }
            break;
        case "frame"://创建框架合同
            window.top.closeAddTab("物流框架合同-新增", '/Bus/ContractCategory/ServiceForm.aspx?isFrame=true', '');
            break;
        case "body"://创建子合同
            var row = $("#newAttachContract").datagrid('getSelected');
            if (row == undefined) {
                $.messager.alert("提醒", "请选择一条框架合同");
                return;
            }
            window.top.closeAddTab("物流框架子合同-新增", '/Bus/ContractCategory/ServiceForm.aspx?contractNo=' + row.contractNo + '&isFrameAttach=true', '');
            break;
        case "outText"://创建外部文本合同
            window.top.closeAddTab("物流外部文本合同-新增", '/Bus/ContractCategory/ServiceForm.aspx?isOutside=true', '');
            break;
        default:
            break;
    }
}
//设置百分比
function fixWidth(percent) {
    return document.body.clientWidth * percent; //这里你可以自己做调整  
}
//选择创建方式
function chooseCreateStyle() {
    $("#newChooseContractAttachDiv").panel('close');//隐藏创建子合同Div
    $("#mainCotactCopyDiv").panel('close');//隐藏主合同关联复制创建div
    $("input[name=independentCreateStyle]").click(function () {
        showCont();
    });
};
function showCont() {
    switch ($("input[name=independentCreateStyle]:checked").attr("id")) {
        case "main":
            $("#newChooseContractAttachDiv").panel('close');
            $("#mainCotactCopyDiv").panel('open');
            break;
        case "frame":
            $("#newChooseContractAttachDiv").panel('close');
            $("#mainCotactCopyDiv").panel('close');//隐藏主合同关联复制创建div
            break;
        case "body":
            $("#newChooseContractAttachDiv").panel('open');
            $("#mainCotactCopyDiv").panel('close');//隐藏主合同关联复制创建div
            var url = '/ashx/Contract/contractData.ashx?module=getServiceContract';
            CreateAloneAttachGetData(url);//显示框架合同筛选
            break;
        case "outText":
            $("#newChooseContractAttachDiv").panel('close');
            $("#mainCotactCopyDiv").panel('close');//隐藏主合同关联复制创建div
            break;
        default:
            break;
    }
}
//根据时间，业务员筛选框架合同
function CreateAloneAttachGetData(url) {
    $("#newAttachContract").datagrid({
        url: url,
        pagination: false,
        rownumbers: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'contractNo', title: '合同号', width: fixWidth(0.15) },
                    { field: 'seller', title: '卖方', width: fixWidth(0.15) },
                    { field: 'buyer', title: '买方', width: fixWidth(0.15) },
        ]],
        onSelect: function (index, row) {
        }
    })
}
//创建关联合同复制合同单击事件
function contactCopyCreate(obj) {
    contactCopyStaus = obj.value;
    switch (obj.value) {
        case "contact"://关联
            var url = "/ashx/Contract/chosseContractData.ashx?module=serviceContactContract";
            chooseContactCopy(url);
            break;
        case "copy"://复制
            var url = "/ashx/Contract/chosseContractData.ashx?module=serviceContactContract";
            chooseContactCopy(url);
            break;
        default:
            break;

    }
}
//筛选主合同关联复制数据
function chooseContactCopy(url) {
    $("#mainContactCopyTab").datagrid({
        url: url,
        pagination: false,
        rownumbers: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'contractNo', title: '合同号', width: fixWidth(0.15) },
                    { field: 'seller', title: '卖方', width: fixWidth(0.15) },
                    { field: 'buyer', title: '买方', width: fixWidth(0.15) },
        ]],
        onSelect: function (index, row) {

        }
    })
}

//dosearch筛选
function doSearch() {
   var checkval = $("input[name='newCreateContract']:checked").val();
    var signedtime_begin = $("#signedtime_begin").datebox('getValue');
    var signedtime_end = $("#signedtime_end").datebox('getValue');
    $("#mainContactCopyTab").datagrid({
        url: '/ashx/Contract/chosseContractData.ashx?module=serviceContactContract&signedtime_begin=' + signedtime_begin + '&signedtime_end=' + signedtime_end,
        pagination: false,
        rownumbers: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'contractNo', title: '合同号', width: fixWidth(0.15) },
                    { field: 'seller', title: '卖方', width: fixWidth(0.15) },
                    { field: 'buyer', title: '买方', width: fixWidth(0.15) },
        ]],
        onSelect: function (index, row) {

        }
    })
 
}




