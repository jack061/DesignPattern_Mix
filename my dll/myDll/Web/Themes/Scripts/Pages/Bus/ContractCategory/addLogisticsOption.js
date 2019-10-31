$(function () {
 
    bindUI();
    hidePanel();
})
//绑定数据
function bindUI() {
    $("#newLogisticsTemp").datagrid({
        url: '/ashx/Contract/templateContractData.ashx?module=logisticsTemplatelist',
        pagination: false,
        rownumbers: true,
        //fit: true,
        sortName: 'logisticsContractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'logisticsTemplateName', title: '模板名称', width: fixWidth(0.13) },
                    { field: 'seller', title: '卖方', width: fixWidth(0.13) },
                    { field: 'buyer', title: '买方', width: fixWidth(0.13) },

        ]],
        onClickRow: function (rowNum, record) {
         
        }

    })
    //$("#logisticsTemplate").combobox({
    //    required: true,
    //    valueField: 'logisticsTemplateno',
    //    textField: 'logisticsTemplateName',
    //    editable: false,
    //    data: sbLogisticsTemplate
    //});
}
//隐藏panel
function hidePanel() {
    
    //隐藏创建子合同筛选框架合同表
    $('#newChooseContractAttachDiv').panel('close');
    //隐藏选择物流模板Div
    $('#chooseLogTempDiv').panel('close');
    //隐藏新建或者复制创建
    $('#associationSimpleOriginalDiv').panel('close');

}
//选择类型
function independentCreateStyleClick() {
    if ($("input[name='independentCreateStyle']:checked").val() == "选择框架")
    {
        //隐藏创建子合同筛选框架合同表
        $('#newChooseContractAttachDiv').panel('close');
        //显示选择物流合同模板Div
        $('#chooseLogTempDiv').panel('open');
        //隐藏新建或者复制创建
        $('#associationSimpleOriginalDiv').panel('close');
        $("#newLogisticsTemp").datagrid({
            url: '/ashx/Contract/templateContractData.ashx?module=logisticsTemplatelist1',
            pagination: false,
            rownumbers: true,
            //fit: true,
            sortName: 'logisticsContractNo',
            singleSelect: true,
            sortOrder: 'asc',
            columns: [[
                        { field: 'ck', checkbox: true },
                        { field: 'logisticsTemplateName', title: '模板名称', width: fixWidth(0.13) },
                        { field: 'seller', title: '卖方', width: fixWidth(0.13) },
                        { field: 'buyer', title: '买方', width: fixWidth(0.13) },

            ]],
            onClickRow: function (rowNum, record) {

            }

        })
    }
    if ($("input[name='independentCreateStyle']:checked").val() == "选择子合同") {
        //填充业务员combobox选择框
        loadSalemanCombo();
        //根据时间筛选框架合同
        contractTimeClick();
        //隐藏创建子合同筛选框架合同表
        $('#newChooseContractAttachDiv').panel('open');
        //隐藏选择物流模板Div
        $('#chooseLogTempDiv').panel('close');

    }
    if ($("input[name='independentCreateStyle']:checked").val() == "选择主合同") {
        //隐藏创建子合同筛选框架合同表
        $('#newChooseContractAttachDiv').panel('close');
        //显示选择物流合同模板Div
        $('#chooseLogTempDiv').panel('open');
        //隐藏新建或者复制创建
        $('#associationSimpleOriginalDiv').panel('close');
        $("#newLogisticsTemp").datagrid({
            url: '/ashx/Contract/templateContractData.ashx?module=logisticsTemplatelist1',
            pagination: false,
            rownumbers: true,
            //fit: true,
            sortName: 'logisticsContractNo',
            singleSelect: true,
            sortOrder: 'asc',
            columns: [[
                        { field: 'ck', checkbox: true },
                        { field: 'logisticsTemplateName', title: '模板名称', width: fixWidth(0.13) },
                        { field: 'seller', title: '卖方', width: fixWidth(0.13) },
                        { field: 'buyer', title: '买方', width: fixWidth(0.13) },

            ]],
            onClickRow: function (rowNum, record) {

            }

        })
    }

}
//选择子合同创建新建或复制单击事件
function neworcopyCreate() {
    if ($("input[name='associationSimpleOriginal']:checked").val() == "新建") {

    }
    if ($("input[name='associationSimpleOriginal']:checked").val() == "复制创建") {
        //筛选出选择的框架合同下的子合同进行复制
        loadFrameConToCopy();
    }

}
//填充业务员combobox选择框，根据下拉选择业务员和时间筛选合同
function loadSalemanCombo() {
    //绑定业务员编号
    $("#attachSaleman").combobox({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',
        valueField: 'UserRealName',
        textField: 'UserRealName',
        editable: false,
        onSelect: function (record) {
            var time = $("input[name='contractTime']:checked").val();
            var attachSaleman = $('#attachSaleman').combobox('getValue');
            var  url = '/ashx/Contract/contractData.ashx?module=getLogisticsContract&attachSaleman=' + attachSaleman + '&time=' + time+'&isFrame=true';
            //根据业务员和时间筛选合同
            CreateAloneAttachGetData(time, attachSaleman,url);
        }

    });
}
//根据时间筛选框架合同
function contractTimeClick() {
    var simpleoriginal = $("input[name='simpleoriginal']:checked").val();
    var time = $("input[name='contractTime']:checked").val();
    var attachSaleman = $('#attachSaleman').combobox('getValue');
    var url = '/ashx/Contract/contractData.ashx?module=getLogisticsContract&attachSaleman=' + attachSaleman + '&time=' + time+'&isFrame=true';
    //根据业务员和时间筛选合同
    CreateAloneAttachGetData(time, attachSaleman, url);;
}
function CreateAloneAttachGetData(time, attachSaleman, url) {
    $("#newAttachContract").datagrid({
        url:url,
        pagination: false,
        rownumbers: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'logisticsContractNo', title: '合同号', width: fixWidth(0.15) },
                    { field: 'seller', title: '卖方', width: fixWidth(0.15) },
                    { field: 'buyer', title: '买方', width: fixWidth(0.15) },
        ]],
        onSelect: function (index, row) {
            //显示新建或者复制创建
            //$('#associationSimpleOriginalDiv').panel('open');
            loadFrameConToCopy();
        }
    })
}
//筛选出选择的框架合同下的子合同列表
function loadFrameConToCopy() {
    var row = $("#newAttachContract").datagrid('getSelected');
    $("#copyFrameInContract").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getLogisticsFrameInCon&logisticsContractNo=' + row.logisticsContractNo,
        pagination: false,
        rownumbers: true,
        sortName: 'logisticsContractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'logisticsContractNo', title: '合同号', width: fixWidth(0.15) },
                    { field: 'seller', title: '卖方', width: fixWidth(0.15) },
                    { field: 'buyer', title: '买方', width: fixWidth(0.15) },
        ]],
        onSelect: function (index, row) {
            //显示新建或者复制创建
            //$('#associationSimpleOriginalDiv').panel('open');
        }
    })
    
}
//下一步单击事件
function nextWay() {
    
    if ($("input[name='independentCreateStyle']:checked").val() == "选择子合同") {
       
        
        //if ($("input[name='associationSimpleOriginal']:checked").val() == "新建") {//新建子合同
        var row = $("#newAttachContract").datagrid('getSelected');
        if (row==undefined) {
            $.messager.alert("提醒", "请选择一条框架合同");
            return;
        }
        window.top.addNewTab("管理框架子合同-新增", '/Bus/ContractCategory/logisticsForm.aspx?logisticsContractNo=' + row.logisticsContractNo + '&isFrameIncon=true', '');
    }

    if ($("input[name='independentCreateStyle']:checked").val() == "选择框架") {
        var row = $('#newLogisticsTemp').datagrid('getSelected');
        var templateNo = row.logisticsTemplateno;
        if (row=="") {
            $.messager.alert("提醒", "请选择模板名称");
            return;
        }
        window.top.addNewTab("管理框架合同-新增", '/Bus/ContractCategory/logisticsForm.aspx?logisticsTemplateno=' + templateNo+'&isFrame=true', '');
    }

    if ($("input[name='independentCreateStyle']:checked").val() == "选择主合同") {
         var row = $('#newLogisticsTemp').datagrid('getSelected');
    var templateNo = row.logisticsTemplateno;
        if (templateNo == "") {
            $.messager.alert("提醒", "请选择模板名称");
            return;
        }
        window.top.addNewTab("管理主合同-新增", '/Bus/ContractCategory/logisticsForm.aspx?logisticsTemplateno=' + templateNo + '&isFrame=false', '');
    }
   
  
}
//设置百分比
function fixWidth(percent) {
    return document.body.clientWidth * percent; //这里你可以自己做调整  
}