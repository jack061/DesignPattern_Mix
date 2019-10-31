$(function () {
    //隐藏panel
    hidePanel();
    //checkbox 单选
    singleCheckbox();
})
//隐藏panel
function hidePanel() {

    //隐藏独立创建的Div
    //$('#independentDiv').panel('close');
    //隐藏独立创建方式Div,选择合同创建，框架合同创建，框架合同附件创建
    //$('#independentStyleDiv').panel('close');
    //隐藏独立创建下合同创建的Div,选择模板创建或外部文本创建
    $('#newCreateContractDiv').panel('close');
    //隐藏独立创建下创建框架合同的Div,选择模板创建或外部文本创建
    $('#newCreateFrameContractDiv').panel('close');
    //隐藏独立创建下创建框架合同附件的Div
    $('#newCreateFrameAttachDiv').panel('close');
    //隐藏创建合同附件筛选合同Div
    $('#newChooseContractAttachDiv').panel('close');
    //隐藏选择简版创建附件或者原版创建
    $('#chooseSimpleOriginalDiv').panel('close');
    //隐藏关联创建的Div
    $('#associationDiv').panel('close');
    //隐藏筛选要关联合同的Div,根据时间，业务员筛选
    $('#assciationChooseDiv').panel('close');
    //隐藏选择关联合同是合同关联或者附件关联的Div
    $('#chooseAssciationStyleDiv').panel('close');
    //隐藏选择关联合同为合同关联的Div
    $('#associationByContractDiv').panel('close');
    //隐藏关联合同为合同关联的模板列表，根据业务员筛选或显示外部文本
    $('#associationContractTemplateDiv').panel('close');
    //隐藏选择关联合同为附件关联的Div
    $('#associationByAttachDiv').panel('close');
    //隐藏附件关联选择要关联附件的框架合同的Div，根据 买卖双方和业务员筛选
    $('#associationAttachChooseDiv').panel('close');
    //隐藏关联创建附件关联选择简版创建附件或者原版创建的Div
    $('#associationSimpleOriginalDiv').panel('close');

}

//===================独立创建=======================================================

//选择独立创建方式为合同创建，框架合同创建，框架附件创建的的单击事件
function independentCreateStyleClick() {
    var name = $("input[name='independentCreateStyle']:checked").val();
    switch (name) {
        case "创建合同":
            //显示创建合同的Div
            $('#newCreateContractDiv').panel('open');
            //隐藏创建合同框架Div
            $('#newCreateFrameContractDiv').panel('close');
            //隐藏创建合同框架附件的Div
            $('#newCreateFrameAttachDiv').panel('close');
            break;
        case "创建框架合同":
            //隐藏独立创建下合同创建的Div
            $('#newCreateContractDiv').panel('close');
            //显示创建合同框架Div
            $('#newCreateFrameContractDiv').panel('open');
            //隐藏创建合同框架附件的Div
            $('#newCreateFrameAttachDiv').panel('close');
            break;
        case "创建框架合同附件":
            //隐藏独立创建下合同创建的Div
            $('#newCreateContractDiv').panel('close');
            //隐藏创建合同框架Div
            $('#newCreateFrameContractDiv').panel('close');
            //显示框架合同附件Div
            $('#newCreateFrameAttachDiv').panel('open');
            $('#newChooseContractAttachDiv').panel('open');
            break;
        default:
            break;

    }
}
//独立创建下创建合同下模板创建，外部文本创建的单击事件
function newCreateContractClick() {
    var name = $("input[name='newCreateContract']:checked").val();
    switch (name) {
        case "模板创建":
            //隐藏独立创建下关联创建筛选框架合同创建关联附件的Div
            $('#contactFrameAttachDiv').panel('close');
            //独立创建下创建合同下模板创建时筛选模板
            loadTemplateByNewContract();
            break;
        case "复制创建":
            //隐藏独立创建下关联创建筛选框架合同创建关联附件的Div
            $('#contactFrameAttachDiv').panel('close');
            //独立创建下复制创建合同下筛选合同
            loadContractByCopy();
            break;
        case "关联创建":
            //独立创建关联创建合同下筛选合同
            loadContractByContact();
            break;
        case "外部文本创建":
            //隐藏独立创建下关联创建筛选框架合同创建关联附件的Div
            $('#contactFrameAttachDiv').panel('close');
            //隐藏模板创建模板表格的Div
            $('#newContactTemplateDiv').panel('close');
            break;
        default:
            break;
    }

}
//独立创建下创建框架合同下模板创建，外部文本创建的单击事件
function newCreateFrameContractClick() {
    var name = $("input[name='newCreateFrameContract']:checked").val();
    switch (name) {
        case "模板创建":
            //独立创建下创建合同下模板创建时筛选模板
            loadTemplateByNewFrameContract();
            break;
        case "复制创建":
            //独立创建下复制创建合同下筛选合同
            loadContractByCopy();
            break;
        case "外部文本创建":
            //隐藏模板创建模板表格的Div
            $('#newFrameContactTemplateDiv').panel('close');
            break;

    }

}
//独立创建下创建框架合同附件根据时间筛选的单击事件
function contractTimeClick() {
    var time = $("input[name='contractTime']:checked").val();
    //根据业务员和时间筛选合同
    CreateAloneAttachGetData(time);
}
//独立创建下创建合同附件筛选合同,只筛选出境的合同
function CreateAloneAttachGetData(time) {
    $("#newAttachContract").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getContract&saleman=' + sbSaleman + '&time=' + time + '&flowdirection=' + exportFlow + '&isFrame=true&isattachFrame=false',
        pagination: true,
        rownumbers: true,
        //fit: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'contractNo', title: '合同号', width: fixWidth(0.15) },
                    { field: 'seller', title: '卖方', width: fixWidth(0.15) },
                    { field: 'buyer', title: '买方', width: fixWidth(0.15) },

        ]],
        onSelect: function (row, index) {
            $('#chooseSimpleOriginalDiv').panel('open');
            loadFrameAttachToCopy();
        }
    })
    

}
//独立创建下创建框架合同附件复制创建筛选框架下的子合同
function loadFrameAttachToCopy() {
    var row = $("#newAttachContract").datagrid('getSelected');
    $("#newFrameContract").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getImportFrameAttachInCon&frameAttachContractNo=' + row.contractNo,
        pagination: false,
        rownumbers: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        checkOnSelect: true,
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
///创建合同下一步单击事件
function contractnextWay() {
    //获取是独立创建还是关联创建
    var createStyle = $("input[name='createType']:checked").val();
    //获取独立创建下是创建合同、创建框架合同、创建框架合同附件
    var independentCreateStyle = $("input[name='independentCreateStyle']:checked").val();
    if (independentCreateStyle == "创建合同") {
        //获取是模板创建或者外部文本创建
        var newCreateContract = $("input[name='newCreateContract']:checked").val();
        switch (newCreateContract) {
            case "模板创建":
                //获取模板编号，创建合同
                var row = $("#newContactTemplate").datagrid('getSelected');
                if (row.templatename == "") {
                    $.messager.alert("提醒", "请选择模板");
                    return;
                }
                window.top.closeAddTab("创建合同", "/Bus/ContractCategory/exportContractTest.aspx?templateno=" + row.templateno);
            case "复制创建":
                var row = $("#newContactTemplate").datagrid('getSelected');
                if (row.contractNo == "") {
                    $.messager.alert("提醒", "请选择合同");
                    return;
                }
                window.top.closeAddTab("创建合同", "/Bus/ContractCategory/exportContractTest.aspx?flowdirection=" + exportFlow + "&isCopy=true&contractNo=" + row.contractNo);
            case "外部文本创建":
                window.top.closeAddTab("创建合同", "/Bus/ContractCategory/exportOutsideContractForm.aspx?flowdirection=" + exportFlow);
            case "关联创建":
                var row = $("#newContactTemplate").datagrid('getSelected');
               
                if ($("input[name='contactFrameAttachRadio']:checked").val() == "框架合同创建") {
                    var frameCotactRow = $("#contactFrameAttachTable").datagrid('getSelected');
                    var frameContactCopyRow = $("#contactFrameAttachCopyTable").datagrid('getSelected');//获取要复制的框架合同附件
                    if (frameCotactRow == null) {
                        $.messager.alert("提醒", "请选择框架合同号");
                        return;
                    }
                    if (frameContactCopyRow == null) {
                        window.top.closeAddTab("创建附件关联合同", "/Bus/ContractCategory/exportContractTest.aspx?flowdirection=" + exportFlow + "&isContact=true&contractNo=" + row.contractNo + '&frameCotactContractNo=' + frameCotactRow.contractNo);
                    }
                    else {
                        window.top.closeAddTab("复制创建附件关联合同", "/Bus/ContractCategory/exportContractTest.aspx?flowdirection=" + exportFlow + "&isContact=true&contractNo=" + frameContactCopyRow.contractNo + '&frameCotactContractNo=' + frameCotactRow.contractNo + '&mainContractNo=' + row.contractNo);
                    }
                }
                else if ($("input[name='contactCopyRadio']:checked").val() == "复制创建") {
                    var frameCotactRow = $("#contactFrameAttachTable").datagrid('getSelected');
                    if (frameCotactRow == null) {
                        $.messager.alert("提醒", "请选择复制合同号");
                        return;
                    }
                    window.top.closeAddTab("复制创建关联合同", "/Bus/ContractCategory/exportContractTest.aspx?flowdirection=" + exportFlow + "&isContact=true&contractNo=" + frameCotactRow.contractNo + '&frameCotactContractNo=' + row.contractNo + '&isCopyContact=true');
                }
                else {
                    window.top.closeAddTab("创建关联合同", "/Bus/ContractCategory/exportContractTest.aspx?flowdirection=" + exportFlow + "&isContact=true&contractNo=" + row.contractNo + '&mainContractNo=' + row.contractNo);
                }
            default:

        }
    }
    if (independentCreateStyle == "创建框架合同") {
        //获取是模板创建或者外部文本创建
        var newCreateFrameContract = $("input[name='newCreateFrameContract']:checked").val();
        if (newCreateFrameContract == "模板创建") {
            //获取模板编号，创建合同
            var row = $("#newFrameContactTemplate").datagrid('getSelected');
            if (row.templatename == "") {
                $.messager.alert("提醒", "请选择模板");
                return;
            }
            window.top.closeAddTab("创建框架合同", "/Bus/ContractCategory/exportContractTest.aspx?templateno=" + row.templateno + '&isFrame=true');
        }

        if (newCreateFrameContract == "外部文本创建") {
            window.top.closeAddTab("创建框架合同", "/Bus/ContractCategory/exportOutsideContractForm.aspx?isFrame=true&flowdirection=" + exportFlow);
        }
    }
    if (independentCreateStyle == "创建框架合同附件") {
        var row = $("#newFrameContract").datagrid('getSelected');//获取框架合同下的所有附件
        var newAttachContract = $("#newAttachContract").datagrid('getSelected');//获取框架合同附件
        if (row == null) {//新建框架合同附件
            window.top.closeAddTab("创建框架合同附件", "/Bus/ContractCategory/exportContractTest.aspx?contractNo=" + newAttachContract.contractNo + "&isFrameAttach=true");
        }
        else {//复制创建框架合同附件
            window.top.closeAddTab("创建框架合同附件", "/Bus/ContractCategory/exportContractTest.aspx?contractNo=" + row.contractNo + "&mainContractNo=" + newAttachContract.contractNo + "&isFrameAttach=true");
        }
    }
}


//关联创建合同筛选合同
function loadContractByContact() {

    $("#newContactTemplate").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getContactContract&flowdirection=' + exportFlow,
        pagination: false,
        sortName: 'createman',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                    { field: 'contractNo', title: '合同号', width: fixWidth(0.1) },
                    { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                    { field: 'simpleBuyer', title: '买方', width: fixWidth(0.1) },
                    { field: 'simpleSeller', title: '卖方', width: fixWidth(0.1) },

        ]],
        onSelect: function (index, row) {

            $('#contactFrameAttachDiv').panel('open');
        }
    })
    $('#newContactTemplateDiv').panel('open');
}
//独立创建下关联创建筛选框架合同创建关联附件单击事件
function contactFrameAttachClick() {
    if ($("input[name='contactFrameAttachRadio']:checked").val() == "框架合同创建") {

        $("#contactFrameAttachTable").show();
        $("#contactFrameAttachTable").datagrid({
            url: '/ashx/Contract/contractData.ashx?module=getContract&saleman=' + sbSaleman + '&flowdirection=' + exportFlow + '&isFrame=true',
            pagination: false,
            rownumbers: true,
            sortName: 'contractNo',
            singleSelect: true,
            sortOrder: 'asc',
            checkOnSelect: true,
            columns: [[
        { field: 'ck', checkbox: true },
                        { field: 'contractNo', title: '合同号', width: fixWidth(0.15) },
                            { field: 'seller', title: '卖方', width: fixWidth(0.15) },
                                { field: 'buyer', title: '买方', width: fixWidth(0.15) },
            ]],
            onSelect: function (index, row) {
                loadMainFrameAttch(row.contractNo);//获取框架合同下的附件进行复制
            }
        })
    }

}
//独立创建下关联创建复制关联合同号单击事件
function contactCopyClick() {

    if ($("input[name='contactCopyRadio']:checked").val() == "复制创建") {
        $("#contactFrameAttachTable").show();
        $("#contactFrameAttachTable").datagrid({
            url: '/ashx/Contract/chosseContractData.ashx?module=getImConCopyList&flowdirection=' + exportFlow,
            pagination: false,
            rownumbers: true,
            sortName: 'contractNo',
            singleSelect: true,
            sortOrder: 'asc',
            checkOnSelect: true,
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

}
//checkbox单选
function singleCheckbox() {
    //使checkbox 单选
    $('#contactFrameAttachDiv').find('input[type=checkbox]').bind('click', function () {
        $('#contactFrameAttachDiv').find('input[type=checkbox]').not(this).attr("checked", false);
    });
}

function loadMainFrameAttch(contractNo) {
    $("#contactFrameAttachCopyTable").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getImportFrameAttachInCon&frameAttachContractNo=' + contractNo,
        pagination: false,
        rownumbers: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        checkOnSelect: true,
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


//独立创建下创建合同下模板创建时筛选模板
function loadTemplateByNewContract() {

    $("#newContactTemplate").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getExportTemplate',
        pagination: false,
        rownumbers: true,
        sortName: 'templatename',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                    { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                    { field: 'templatename', title: '模板名称', width: fixWidth(0.15) },
        ]],
    })
    $('#newContactTemplateDiv').panel('open');

}
//独立创建下创建框架合同下模板创建时筛选模板

function loadTemplateByNewFrameContract() {
    $('#newFrameContactTemplateDiv').panel('open');
    $("#newFrameContactTemplate").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getExportTemplate',
        pagination: false,
        rownumbers: true,
        sortName: 'templatename',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                    { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                    { field: 'templatename', title: '模板名称', width: fixWidth(0.15) },

        ]],
    })
}
//关联创建下合同关联时筛选模板
function loadTemplateByAssciationContract() {
    //显示模板表格
    $("#associationContractTemplateDiv").panel('open');
    //获取要关联的合同的卖方编号
    var row = $("#associationContractTable").datagrid('getSelected');
    //根据卖方编号筛选其作为买方的合同模板
    $("#associationContractTemplate").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getExportTemplate&buyercode=' + row.sellercode,
        pagination: false,
        rownumbers: true,
        sortName: 'templatename',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                    { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                    { field: 'templatename', title: '模板名称', width: fixWidth(0.15) },
        ]],
    })
}

//卖方买方简称带出全称以及comobox下拉选择卖方买方
function gettotalnameBySimple() {

    //卖方(简称)文本框改变事件
    $("input", $("#associationAttachSimpleSeller").next("span")).blur(function () {

        var simpleSeller = ($("#associationAttachSimpleSeller").val());
        $.post("/ashx/Contract/loadPeople.ashx", { simpleSeller: simpleSeller }, function (data) {

            $("#associationAttachSeller").combobox('setValue', data.name);
        }, 'json')

    })
    //买方(简称)文本框改变事件
    $("input", $("#associationAttachSimpleBuyer").next("span")).blur(function () {
        var simpleBuyer = ($("#associationAttachSimpleBuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx", { simpleBuyer: simpleBuyer }, function (data) {
            $("#associationAttachBuyer").combobox('setValue', data.name);
            var seller = $("#associationAttachSeller").combobox('getValue');
            var buyer = $("#associationAttachBuyer").combobox('getValue');
            //筛选所有的框架合同加载表格
            getFrameContract(seller, buyer);
        }, 'json')

    })

    //绑定卖方combogrid
    $('#associationAttachSeller').combogrid({
        panelWidth: 450,
        //value:htdata.seller,
        idField: 'code',
        textField: 'name',
        data: comSeller,
        editable: false,
        columns: [[
                    { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {

        }
    });
    //绑定买方combogrid
    $('#associationAttachBuyer').combogrid({
        panelWidth: 450,
        //value:htdata.buyer,
        idField: 'code',
        textField: 'name',
        data: comBuyer,
        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            var seller = $("#associationAttachSeller").combobox('getValue');
            var buyer = $("#associationAttachBuyer").combobox('getValue');
            //筛选所有的框架合同加载表格
            getFrameContract(seller, buyer);
        }
    });


}

//筛选所有的框架合同加载表格
function getFrameContract(seller, buyer) {

    $("#associationAttachTable").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getContract&seller=' + seller + '&buyer=' + buyer + '&checkAttachTime=' + '&isFrame=true',
        pagination: false,
        rownumbers: true,
        //fit: true,
        sortName: 'contractNo',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'contractNo', title: '合同号', width: fixWidth(0.2) },
                    { field: 'seller', title: '卖方', width: fixWidth(0.2) },
                    { field: 'buyer', title: '买方', width: fixWidth(0.2) },

        ]],
        onClickRow: function (rowNum, record) {
            //为隐藏域卖方买方赋值
            $("#hiddenseller").val(record.seller);
            $("#hiddenbuyer").val(record.buyer);
            //显示选择简版创建附件或者原版创建的Div
            $('#associationSimpleOriginalDiv').panel('open');
        }

    })

}
//复制创建合同筛选合同
function loadContractByCopy() {
    $("#newContactTemplate").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getCopyContract&flowdirection=' + exportFlow,
        pagination: false,
        sortName: 'createman',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                    { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                    { field: 'contractNo', title: '合同号', width: fixWidth(0.1) },
                    { field: 'simpleBuyer', title: '买方', width: fixWidth(0.1) },
                    { field: 'simpleSeller', title: '卖方', width: fixWidth(0.1) },
        ]],
    })
    $('#newContactTemplateDiv').panel('open');
}
//复制创建框架合同筛选合同
function loadContractByCopy() {
    $("#newContactTemplate").datagrid({
        url: '/ashx/Contract/contractData.ashx?module=getCopyContract&flowdirection=' + exportFlow,
        pagination: false,
        sortName: 'createman',
        singleSelect: true,
        sortOrder: 'asc',
        columns: [[
                    { field: 'ck', checkbox: true },
                    { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                     { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                    { field: 'contractNo', title: '合同号', width: fixWidth(0.1) },
                    { field: 'simpleBuyer', title: '买方', width: fixWidth(0.1) },
                    { field: 'simpleSeller', title: '卖方', width: fixWidth(0.1) },

        ]],
    })
    $('#newContactTemplateDiv').panel('open');
}
//设置百分比
function fixWidth(percent) {
    return document.body.clientWidth * percent; //这里你可以自己做调整  
}
//dosearch筛选
function doSearch() {
    var checkval = $("input[name='newCreateContract']:checked").val();
    var signedtime_begin = $("#signedtime_begin").datebox('getValue');
    var signedtime_end = $("#signedtime_end").datebox('getValue');
    switch (checkval) {
        case "关联创建":
            $("#newContactTemplate").datagrid({
                url: '/ashx/Contract/contractData.ashx?module=getContactContract&flowdirection=' + exportFlow + '&signedtime_begin=' +
                    signedtime_begin + '&signedtime_end=' + signedtime_end,
                pagination: false,
                sortName: 'createman',
                singleSelect: true,
                sortOrder: 'asc',
                columns: [[
                            { field: 'ck', checkbox: true },
                            { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                            { field: 'contractNo', title: '合同号', width: fixWidth(0.1) },
                            { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                            { field: 'simpleBuyer', title: '买方', width: fixWidth(0.1) },
                            { field: 'simpleSeller', title: '卖方', width: fixWidth(0.1) },

                ]],
                onSelect: function (index, row) {

                    $('#contactFrameAttachDiv').panel('open');
                }
            })
            $('#newContactTemplateDiv').panel('open');
            break;
        case "复制创建":
            $("#newContactTemplate").datagrid({
                url: '/ashx/Contract/contractData.ashx?module=getCopyContract&flowdirection=' + exportFlow + '&signedtime_begin=' +
                    signedtime_begin + '&signedtime_end=' + signedtime_end,
                pagination: false,
                sortName: 'createman',
                singleSelect: true,
                sortOrder: 'asc',
                columns: [[
                            { field: 'ck', checkbox: true },
                            { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                            { field: 'contractNo', title: '合同号', width: fixWidth(0.1) },
                            { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                            { field: 'simpleBuyer', title: '买方', width: fixWidth(0.1) },
                            { field: 'simpleSeller', title: '卖方', width: fixWidth(0.1) },

                ]],
                onSelect: function (index, row) {
                    //隐藏独立创建下关联创建筛选框架合同创建关联附件的Div
                    $('#contactFrameAttachDiv').panel('close');
                }
            })
            $('#newContactTemplateDiv').panel('open');
            break;
        case "模板创建":
            $("#newContactTemplate").datagrid({
                url: '/ashx/Contract/contractData.ashx?module=GetExportTemplate&signedtime_begin=' +
                    signedtime_begin + '&signedtime_end=' + signedtime_end,
                pagination: false,
                rownumbers: true,
                sortName: 'templatename',
                singleSelect: true,
                sortOrder: 'asc',
                columns: [[
                            { field: 'ck', checkbox: true },
                            { field: 'createmanname', title: '创建人', width: fixWidth(0.1) },
                            { field: 'createdate', title: '创建时间', width: fixWidth(0.1) },
                            { field: 'templatename', title: '模板名称', width: fixWidth(0.15) },
                ]],
            })
            $('#newContactTemplateDiv').panel('open');
            break;
        default:

    }
}
//doBodySearch筛选
function doBodySearch() {
    var signedtime_begin = $("#signedtime_begin1").datebox('getValue');
    var signedtime_end = $("#signedtime_end1").datebox('getValue');
    if ($("input[name='contactFrameAttachRadio']:checked").val() == "框架合同创建") {
        $("#contactFrameAttachTable").show();
        $("#contactFrameAttachTable").datagrid({
            url: '/ashx/Contract/contractData.ashx?module=getContract&saleman=' + sbSaleman + '&flowdirection=' + exportFlow + '&isFrame=true&signedtime_begin=' + signedtime_begin + '&signedtime_end=' + signedtime_end,
            pagination: false,
            rownumbers: true,
            sortName: 'contractNo',
            singleSelect: true,
            sortOrder: 'asc',
            checkOnSelect: true,
            columns: [[
        { field: 'ck', checkbox: true },
                        { field: 'contractNo', title: '合同号', width: fixWidth(0.15) },
                            { field: 'seller', title: '卖方', width: fixWidth(0.15) },
                                { field: 'buyer', title: '买方', width: fixWidth(0.15) },
            ]],
            onSelect: function (index, row) {
                loadMainFrameAttch(row.contractNo);//获取框架合同下的附件进行复制
            }
        })
    }
    else if ($("input[name='contactCopyRadio']:checked").val() == "复制创建") {
        $("#contactFrameAttachTable").show();
        $("#contactFrameAttachTable").datagrid({
            url: '/ashx/Contract/chosseContractData.ashx?module=getImConCopyList&flowdirection=' + exportFlow + '&signedtime_begin=' +
                    signedtime_begin + '&signedtime_end=' + signedtime_end,
            pagination: false,
            rownumbers: true,
            sortName: 'contractNo',
            singleSelect: true,
            sortOrder: 'asc',
            checkOnSelect: true,
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
}








