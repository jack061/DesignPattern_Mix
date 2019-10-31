var endEditing = function () {
    if (editIndex == undefined) { return true }
    if ($('#htcplist').datagrid('validateRow', editIndex)) {
        $('#htcplist').datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
$(function () {
    if (contractNo == "") {
        $("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadDataByImportTemp&templateno=' + templateno);
    }
   //如果为复制或者创建关联
    else {
        if (isContact == "true" ) {
            //填充关联创建数据
            $("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadContactApp&contractNo=' + contractNo );
        }
        else {
            //填充复制创建数据
            $("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadApp&contractNo=' + contractNo + "&isCopy=" + isCopy + "&isFrameAttach=" + isFrameAttach);
        }
        
    }
    //绑定combobox 
    bindCombobox();
 
    //绑定产品列表
    if (isContact == "true" || isContactEdit=="true") {
        popContactProductSelect();
    }
    else {
        popProductSelect();
    }
 
    if (isnew == "True") {
        bindTemplate(templateno);
    }
    else {
        bindTemplateByContractNo();
    }
    //产品编辑框绑定onchange事件
 
    //bindOnchange();
    //绑定默认数据
    bindUI();
    //根据卖方买方简称加载全称和地址
    loadTotalName();
    //根据卖方买方简称加载全称和地址
    loadTotalName();
    //tabs选中事件
    selectTabs();


});
////返回
var back = function () {

    window.top.closeAddTab("合同-新增", "/Bus/Contract/ContractTest.aspx");
}
//保存
var save = function () {

    var rrdata = SaveDataToDB(0);
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        window.top.selectAndRefreshTab('进口合同');
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        $.messager.alert("提醒", rrdata.sucdata.errormsg);
    }
}
//取消
var cancel = function () {

    //关闭当前tab
    window.top.closeTab();
}
//提交
var submit = function () {
    var buyercode = $("#buyercode").val();
    var sellercode = $("#sellercode").val();
    if (buyercode == sellercode) {
        $.messager.alert("提醒", "请确认买卖双方不相同");
        return;
    }

    getClassificValidate();//获取买卖双方为境内或境外并校验报关合同提醒项并提交
};
//修改模板
var editTemp = function () {
    var templatename = $("#templateno").combobox('getValue');

    if (templatename == "") {
        $.messager.alert("提醒", "请选择模板名称");
        return;
    }
    $("#tempIframe").attr('src', '/Bus/BaseData/Btemplate/BtemplateAdd.aspx?action=edit&no=' + templatename);
    $("#tempDiv").dialog({
        width: 800,
        height: 500,
        maxmizable: true,
        minimizable: true,
        title: '模板修改',
        modal: true,
        buttons: [{
            text: '保存',
            iconCls: 'icon-ok',
            handler: function () {
                //调用子页面方法，保存编辑模板
                document.getElementById("tempIframe").contentWindow.save1();

            }
        }, {
            text: '取消',
            iconCls: 'icon-cancel',
            handler: function () {
                $('#tempDiv').dialog('close');
            }
        }]
    })

}
//子页面调用，关闭dialog
function closeTemp() {
    $.messager.alert("提醒", "保存成功");
    $('#tempDiv').dialog('close');

}
//返回填写货物条款
function backWrite() {

    $('#tt').tabs("select", "货物条款");

}
//返回填写文本条款
function backWriteTemplate() {
    $('#tt').tabs("select", "文本条款");
}

//审核提交
function reviewSub() {
    var data = $("input[name='reviewstatus']:checked").val();
    $('input').attr("readonly", false);
    var log = $("#reviewlog").textbox('getValue');
    var factory = $("#factory").combobox('getValue');
    var stock = $("#stock").combobox('getValue');
    $.post("/ashx/Contract/contractOperater.ashx?module=reviewData" + "&status=" + data + "&log=" + log + "&contractNo=" + contractNo + "&contractStatus=" + sbStatus, { factory: factory, stock: stock }, function (data) {
        if (data.sucess == "1") {
            $.messager.alert("提醒", "审核成功");
            $("#reviewTable").datagrid("reload");
        }
        else {
            $.messager.alert("提醒", data.warnmsg);
        }
    });
}

var SaveDataToDB = function (status) {
    var retdata = {};
    getSubTable();
    //获取list数据到htcplistStr
    $("#htcplist").datagrid("acceptChanges");
    var cplist = $("#htcplist").datagrid("getRows");
    var datagridjson = JSON.stringify(cplist);
    $('#htcplistStr').val(datagridjson);
    var url = "";
    if (isContact == "true" || isContactEdit == "true") {
    
        url = '/ashx/Contract/contractOperater.ashx?module=addImContactContract&status=' + status + '&frameCotactContractNo=' + frameCotactContractNo + '&isCopyContact=' + isCopyContact + '&mainContractNo=' + mainContractNo + '&isCopy=' + isCopy + '&isEdit=' + isEdit + '&isConManage=' + isConManage+'&conManage_contractNo='+contractNo;
    }
  else  if (isFrameAttachEdit=="true"||isFrameAttach=="true") {
      url = '/ashx/Contract/contractOperater.ashx?module=addFrameAttachContractTest&status=' + status + "&mainContractNo=" + mainContractNo + '&frameCotactContractNo=' + frameCotactContractNo + '&isCopy=' + isCopy + '&isEdit=' + '&isConManage=' + isConManage + '&conManage_contractNo=' + contractNo;//添加框架合同附件
    }
    else  {
      url = '/ashx/Contract/contractOperater.ashx?module=addContractTest&status=' + status + '&isCopy=' + isCopy + '&isEdit=' + isEdit + '&isConManage=' + isConManage + '&conManage_contractNo=' + contractNo;
    }

    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: url,
        data: $('#form1').serialize(), // 你的formid
        async: false,
        error: function (data) {
            retdata.errdata = data;
        },
        success: function (data) {

            retdata.sucdata = data;
        }
    });
    return retdata;
}

var bindUI = function () {
 
    //说明是新建合同
    if (isnew == "True" || isCopy == "true" || isContact == "true" || isFrameAttach == "true") {
        if (isFrameAttach == "true") {//创建框架合同附件
            //$("input[name='frameContract'][value='否']").attr("checked", true);
            //$(':radio').attr('disabled', true);

        }
        $('#contractNo').textbox('setValue', '自动编号');
        $('#contractNo').textbox('readonly', true);
    }
    else {
        $('#contractNo').textbox('setValue', contractNo);
        $('#contractNo').textbox('readonly', true);
    }
    //说明为创建框架合同
    if (isFrame == "true") {
        $("input[name='frameContract'][value='是']").attr("checked", true);
    }
    else {
        $("input[name='frameContract'][value='否']").attr("checked", true);
    }
    //说明为查看
    if (isBrowse == "true") {
        $('input').attr('disabled', true);
        $("#aa123").css("display", "none");
    }
    //为合同管理员时可修改是否报关，是否框架
    if (isConManage == "True") {
        if (userAccount != createman) {//要修改的合同不是合同管理员自己的合同
            $('input').attr('disabled', true);
            $("input[type=radio]").attr('disabled', false);
        }
        else {
            if (contractStatus != "新建" && contractStatus != "退回") {
                $('input').attr('disabled', true);
                $("input[type=radio]").attr('disabled', false);
            }
        }
     
    }
    //价格条款1为100时默认价格条款2不可编辑
    var v = $("#pricement1per").numberbox('getValue');
    if (parseInt(v) == 100) {

        $("#pricement2").combobox('setValue', "");
        $('#pricement2').combobox('disable', true);
        $('#pricement2per').numberbox('disable', true);
    }

    //默认签订地点，默认唛头
    $("#shippingmark").textbox('setValue', 'N/M');
    $("#signedplace").textbox('setValue', '乌鲁木齐');
}

//查询产品
function SearchContract() {
    pcode = $('#pcode').textbox('getText');

    name = $('#pname').textbox('getText');
    para = {};
    para.pcode = pcode;
    para.name = name;
    $('#productlist').datagrid('load', para);
    
}

//tabs选中事件
function selectTabs() {
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
            var contractNo = $("#contractNo").textbox('getValue');
            var language = $("#language").combobox('getValue');
            var templateno = $("#templateno").val();
            var validity = $("#validity").combobox('getValue');
            var signedTime = $("#signedtime").datebox('getValue');
            var signedplace = $("#signedplace").textbox('getValue');
            var tradement = $("#tradement ").combobox('getValue');
            var transport = $("#transport").combobox('getValue');
            var harborout = $("#harborout").combobox('getValue');
            var harborarrive = $("#harborarrive ").combobox('getValue');
            var deliveryPlace = $("#deliveryPlace").textbox('getValue');
            var pricement1 = $("#pricement1 ").combobox('getValue');
            var pricement2 = $("#pricement2").combobox('getValue');
            var validity = $("#validity").combobox('getValue');
            var pvalidity = $("#pvalidity").textbox('getValue');
            var shipment = $("#shipment").combobox('getValue');
            var placement = $("#placement").textbox('getValue');
            var buyercode = $("#buyercode").val();
            var sellercode = $("#sellercode").val();
            var shippingmark = $("#shippingmark").val();
            var batchRemark = $("#batchRemark").textbox('getValue');
            var overspill = $("#overspill").numberbox('getValue');
            var paymentType = $("#paymentType").combobox('getValue');
            var shipDate = $("#shipDate").datebox('getValue');
            $('#htcplist').datagrid('acceptChanges');
            if (title == "合同预览") {
                $('input').attr('disabled', false);
                var templateno = $("#templateno").val();
                //bindTemplate(templateno);
                //隐藏语言
                hideLanguage();
                //获取模板条款列表
                var datagrid = $("#tt_subTable").datagrid("getRows");
                var templatejson = JSON.stringify(datagrid);
                //保存模板
                //saveTemplate();
                //使checkbox默认选中
                if (language == $("#checkbox").val()) {
                    $("#checkbox").prop('checked', true);
                    $("#english").prop('checked', false);
                    $("#russian").prop('checked', false);
                }
                if (language == $("#english").val()) {
                    $("#english").prop('checked', true);
                    $("#checkbox").prop('checked', false);
                    $("#russian").prop('checked', false);
                }
                if (language == $("#russian").val()) {
                    $("#russian").prop('checked', true);
                    $("#checkbox").prop('checked', false);
                    $("#english").prop('checked', false);
                }
                //使checkbox 单选
                $('#reviewContract').find('input[type=checkbox]').bind('click', function () {
                    $('#reviewContract').find('input[type=checkbox]').not(this).attr("checked", false);
                });
                //调用iframe子页面合同模板的方法,获取条款数
                //window.parent.frames["previewTemplateFrame"].alertadd();
                $('#htcplist').datagrid('acceptChanges');
                //获取list数据到htcplistStr
                var cplist = $("#htcplist").datagrid("getRows");
                var datagridjson = JSON.stringify(cplist);
                var str = "";
                var chinese = $("#chinese").val();
                var english = $("#english").val();
                var russian = $("#russian").val();
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/contractData.ashx?module=GetRealTimeContract",
                    dataType: "text",
                    data: {
                        buyer: buyer, buyercode: buyercode, seller: seller, sellercode: sellercode,
                        validity: validity, language: language, templateno: templateno, signedTime: signedTime,
                        signedplace: signedplace,
                        productlist: datagridjson, tradement: tradement, transport: transport,
                        harborout: harborout, harborarrive: harborarrive,
                        deliveryPlace: deliveryPlace, pricement1: pricement1, pricement2: pricement2, validity: validity,
                        pvalidity: pvalidity, shipment: shipment, placement: placement, shippingmark: shippingmark, contractNo: contractNo, templatejson: templatejson
                           , batchRemark: batchRemark, overspill: overspill, shipDate: shipDate, paymentType: paymentType
                    },
                    success: function (data) {
                        $("#realTimeContractText").val(data);
                        $("#previewFormFrame").attr('src', '/Bus/ContractCategory/previewContract.aspx');
                    }
                });
             
                $(":checkbox").click(function () {
                    var str = "";
                    $('input[name="checkbox"]:checked').each(function () {
                        str += $(this).val();
                    });
                    $.ajax({
                        type: "post",
                        url: "/ashx/Contract/contractData.ashx?module=GetRealTimeContract",
                        dataType: "text",
                        data: {
                            buyer: buyer, buyercode: buyercode, seller: seller, sellercode: sellercode,
                            validity: validity, language: str, templateno: templateno, signedTime: signedTime,
                            signedplace: signedplace,
                            productlist: datagridjson, tradement: tradement, transport: transport,
                            harborout: harborout, harborarrive: harborarrive,
                            deliveryPlace: deliveryPlace, pricement1: pricement1, pricement2: pricement2, validity: validity,
                            pvalidity: pvalidity, shipment: shipment, placement: placement, templatejson: templatejson, batchRemark: batchRemark
                            , overspill: overspill, contractNo: contractNo, shipDate: shipDate, paymentType: paymentType
                        },
                        success: function (data) {
                            $("#realTimeContractText").val(data);
                            $("#previewFormFrame").attr('src', '/Bus/ContractCategory/previewContract.aspx');
                        }
                    });
                 

                })
            }
            if (title == "货物条款") {
                if (isBrowse == "true") {
                    $('input').attr('disabled', true);
                }
                else {
                    $('input').attr('disabled', false);
                }
            }
            if (title == "文本条款") {
                $("#tt_subTable").datagrid('acceptChanges');

           
                controlEngRus();
            }
            if (title == "审核日志") {

                $("#reviewTable").datagrid({
                    height: 300,
                    width: 700,
                    nowrap: true,
                    fitColumns: true,
                    striped: true,
                    collapsible: true,
                    pageList: [10, 15, 30],
                    singleSelect: true,
                    idField: 'contractNo',
                    url: '/ashx/Contract/contractData.ashx?module=getReviewData&contractNo=' + contractNo,
                    columns: [[
                    { field: 'reviewstatus', title: '审核节点', width: '100px', editor: 'text' },
                    { field: 'status', title: '状态', width: '100px', editor: 'text' },
                    { field: 'reviewdate', title: '审核时间', width: '100px', editor: 'text' },
                    { field: 'reviewlog', title: '审核日志', width: '100px', editor: 'text' },
                     { field: 'reviewman', title: '审核人', width: '100px', editor: 'text' },
                    ]],
                    pagination: false,
                })

            }
            if (title == "合同箱单") {
                $('#htcplist').datagrid('acceptChanges');
                //获取list数据到htcplistStr
                var cplist = $("#htcplist").datagrid("getRows");
                var datagridjson = JSON.stringify(cplist);
                var buyer = $("#buyer").combobox('getValue');
                var buyername = $("#buyername").val();
                var seller = $("#seller").combobox('getValue');
                var sellername = $("#sellername").val();
                var language = $("#language").combobox('getValue');
                if (language == "中文") {
                    language = "zh";
                }
                if (language == "中文-英文") {
                    language = "zheg";
                }
                if (language == "中文-俄文") {
                    language = "zhrs";
                }
                var contractNo = $("#contractNo").textbox('getValue');
                var templateno = $("#templatename").combobox('getValue');
                var validity = $("#validity").combobox('getValue');
                var signedTime = $("#signedtime").datebox('getValue');
                var signedplace = $("#signedplace").textbox('getValue');
                var tradement = $("#tradement ").combobox('getValue');
                var transport = $("#transport").combobox('getValue');
                var harborout = $("#harborout").combobox('getValue');
                var harborarrive = $("#harborarrive ").combobox('getValue');
                var deliveryPlace = $("#deliveryPlace").textbox('getValue');
                var pricement1 = $("#pricement1 ").combobox('getValue');
                var pricement2 = $("#pricement2").combobox('getValue');
                var validity = $("#validity").combobox('getValue');
                var pvalidity = $("#pvalidity").textbox('getValue');
                var shipment = $("#shipment").combobox('getValue');
                var placement = $("#placement").textbox('getValue');
              
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/contractData.ashx?module=GetRealTimeInvoice",
                    dataType: "text",
                    data: {
                        buyer: buyer, buyername: buyername, seller: seller, sellername: sellername,
                        validity: validity, language: language, signedTime: signedTime,
                        signedplace: signedplace,
                        productlist: datagridjson, tradement: tradement, transport: transport,
                        harborout: harborout, harborarrive: harborarrive,
                        deliveryPlace: deliveryPlace, pricement1: pricement1, pricement2: pricement2, validity: validity,
                        pvalidity: pvalidity, shipment: shipment, placement: placement, contractNo: contractNo
                        , type: "packing"
                    },
                    success: function (data) {
                        $("#realTimeInvoiceText").val(data);

                    }
                });
                $("#packinglist").attr('src', '/Bus/ContractCategory/realTimeInvoice.aspx');

            }
            if (title == "合同发票") {

                $('#htcplist').datagrid('acceptChanges');
                //获取list数据到htcplistStr
                var cplist = $("#htcplist").datagrid("getRows");
                var datagridjson = JSON.stringify(cplist);

                var buyer = $("#buyer").combobox('getValue');
                var buyername = $("#buyername").val();
                var seller = $("#seller").combobox('getValue');
                var sellername = $("#sellername").val();
                var language = $("#language").combobox('getValue');
                if (language == "中文") {
                    language = "zh";
                }
                if (language == "中文-英文") {
                    language = "zheg";
                }
                if (language == "中文-俄文") {
                    language = "zhrs";
                }
                var contractNo = $("#contractNo").textbox('getValue');
                var templateno = $("#templatename").combobox('getValue');
                var validity = $("#validity").combobox('getValue');
                var signedTime = $("#signedtime").datebox('getValue');
                var signedplace = $("#signedplace").textbox('getValue');
                var tradement = $("#tradement ").combobox('getValue');
                var transport = $("#transport").combobox('getValue');
                var harborout = $("#harborout").combobox('getValue');

                var harborarrive = $("#harborarrive ").combobox('getValue');
                var deliveryPlace = $("#deliveryPlace").textbox('getValue');
                var pricement1 = $("#pricement1 ").combobox('getValue');
                var pricement2 = $("#pricement2").combobox('getValue');
                var validity = $("#validity").combobox('getValue');
                var pvalidity = $("#pvalidity").textbox('getValue');
                var shipment = $("#shipment").combobox('getValue');
                var placement = $("#placement").textbox('getValue');
                var currency = $("#currency").combobox('getValue');
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/contractData.ashx?module=GetRealTimeInvoice",
                    dataType: "text",
                    data: {
                        buyer: buyer, buyername: buyername, seller: seller, sellername: sellername,
                        validity: validity, language: language, signedTime: signedTime,
                        signedplace: signedplace,
                        productlist: datagridjson, tradement: tradement, transport: transport,
                        harborout: harborout, harborarrive: harborarrive,
                        deliveryPlace: deliveryPlace, pricement1: pricement1, pricement2: pricement2, validity: validity,
                        pvalidity: pvalidity, shipment: shipment, placement: placement, contractNo: contractNo,currency:currency
                        , type: "invoice"
                    },
                    success: function (data) {
                        $("#realTimeInvoiceText").val(data);
                    }
                });
                $("#Invoice").attr('src', '/Bus/ContractCategory/realTimeInvoice.aspx');
            }
        }
    })
}
//控制箱单发票显示
function showInvoice() {
    //隐藏箱单
    var packing = $('#tt').tabs('getTab', "合同箱单").panel('options').tab; //title替换成tab的title  
    packing.hide();
    //隐藏发票
    var invoice = $('#tt').tabs('getTab', "合同发票").panel('options').tab; //title替换成tab的title  
    invoice.hide();
    var flowdirection = $("#flowdirection").textbox('getValue');
    if (flowdirection == importFlow) {
        packing.show();
        invoice.show();
    }
}

var contentStyle = function (value, row, index) {
    return "";
}

//绑定模板表格
function bindTemplate(templateno) {
    var editRow = undefined;
    var toolbar = [{
        text: '添加',
        iconCls: 'icon-add',
        handler: function () {
            //新增空行
            var row = {};
            row.chncontent = "";
            row.engcontent = "";
            row.ruscontent = "";
            row.inline = '否';

            $('#tt_subTable').datagrid('appendRow', row);
        }
    }, {
        text: '编辑',
        iconCls: 'icon-edit',
        handler: function () {
            var row = $("#tt_subTable").datagrid('getSelected');
            if (row != null) {
                if (editRow != undefined) {
                    $("#tt_subTable").datagrid('endEdit', editRow);
                }

                if (editRow == undefined) {
                    var index = $("#tt_subTable").datagrid('getRowIndex', row);
                    $("#tt_subTable").datagrid('beginEdit', index);
                    editRow = index;
                    $("#tt_subTable").datagrid('unselectAll');
                }
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }

        }
    }, {
        text: '删除',
        iconCls: 'icon-remove',
        handler: function () {
            var row = $("#tt_subTable").datagrid('getSelected');
            if (row != undefined) {
                var editIndex = $("#tt_subTable").datagrid('getRowIndex', row);
                $('#tt_subTable').datagrid('cancelEdit', editIndex)
                .datagrid('deleteRow', editIndex);
                editIndex = undefined;

            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }, {
        text: '保存',
        iconCls: 'icon-remove',
        handler: function () {
            getSubTable();
            var form = $("#form1");
            $.ajax({
                cache: true,
                type: "POST",
                dataType: "json",
                url: "/ashx/Contract/templateContractOperator.ashx?module=saveTemplate",
                data: $('#form1').serialize(), // 你的formid
                async: false,
                error: function (data) {
                    retdata.errdata = data;
                },
                success: function (data) {

                    if (data != undefined && data.sucess == '0') {
                        $.messager.alert('系统提示', data.errormsg, 'info');
                    }
                    else {
                        $.messager.alert('系统提示', '保存成功', 'info');


                    }


                }
            });
        }
    }];

    //----初始化datagrid-----
    $('#tt_subTable').datagrid({
        nowrap: false,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        autoRowHeight: true,
        idField: 'mname',
        url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getdetail&no=' + templateno,
        columns: [[
         { field: 'sortno', title: '序号', width: '50px' },
        { field: 'templateno', title: '模板编号', hidden: 'true' },
        {
            field: 'inline', title: '排在一行', width: '65px', align: 'center',
            editor: {
                type: 'checkbox',
                options: {
                    on: '是',
                    off: '否'
                }
            }
        },
        {
            field: 'variable', title: '变量', width: '200px', align: 'center',
            editor: {
                type: 'combobox',
                options: {
                    url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getvariable&flowdirection=' + importFlow,
                    method: 'get',
                    valueField: 'id',
                    textField: 'text',
                    multiple: true,
                    multiline: true,
                    panelHeight: 'auto',
                    height: 79,
                    onSelect: function (item) {
                        if (editRow == undefined) {
                            return;
                        }
                        //获取多选框里面的值

                        var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                        var chn = editors[2];
                        var eng = editors[3];
                        var rus = editors[4];

                        additem(chn.target, item.id);

                        additem(eng.target, item.id);

                        additem(rus.target, item.id);
                    },
                    onUnselect: function (item) {
                        if (editRow == undefined) {
                            return;
                        }
                        //获取多选框里面的值

                        var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                        var chn = editors[2];
                        var eng = editors[3];
                        var rus = editors[4];

                        removeitem(chn.target, item.id);

                        removeitem(eng.target, item.id);

                        removeitem(rus.target, item.id);
                    }
                }
            }
        },
        { field: 'chncontent', title: '中文', width: '300px', editor: 'textarea', styler: contentStyle },
        { field: 'engcontent', title: '英文', width: '300px', editor: 'textarea', styler: contentStyle },
        { field: 'ruscontent', title: '俄文', width: '300px', editor: 'textarea', styler: contentStyle }
        ]],
        pagination: true,
        toolbar: toolbar,
        onAfterEdit: function (rowIndex, rowData, changes) {
            editRow = undefined;
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);
            }

            if (editRow == undefined) {
                $("#tt_subTable").datagrid('beginEdit', rowIndex);
                editRow = rowIndex;
            }
        },
        onClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);

            }
        }
    });
}

//获取子表数据
function getSubTable() {
    $("#tt_subTable").datagrid('acceptChanges');
    var datagrid = $("#tt_subTable").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}
//校验form
function valiForm() {
    return $("#form1").form('validate');
}
var additem = function (control, selectitem) {
    var tmp1 = control.val();
    var v1 = selectitem;
    if (tmp1.indexOf(v1) < 0) {
        if (tmp1.length > 0) {
            tmp1 = tmp1 + ' ' + v1;
        }
        else {
            tmp1 = v1;
        }
    }
    if (tmp1.charAt(0) == ',') {
        tmp1.substring(1, tmp1.length - 1);
    }
    control.val(tmp1);
}

var removeitem = function (control, selectitem) {

    var tmp1 = control.val();
    var v1 = selectitem;
    if (tmp1.indexOf(v1) >= 0) {
        tmp1 = tmp1.replace(',' + v1, '');
        tmp1 = tmp1.replace(v1, '');
    }
    control.val(tmp1);
}

function setEditing(rowIndex) {
    var editors = $('#tt').datagrid('getEditors', rowIndex);
    var priceEditor = editors[0];
    var amountEditor = editors[1];
    var costEditor = editors[2];
    priceEditor.target.bind('change', function () {
        calculate();
    });
    amountEditor.target.bind('change', function () {
        calculate();
    });
    function calculate() {
        var cost = priceEditor.target.val() * amountEditor.target.val();
        $(costEditor.target).numberbox('setValue', cost);
    }
}
//控制语言的显示与隐藏
function hideLanguage() {

    var language = $("#language").combobox('getValue');

    if (language == "中文") {

        $("#englishSpan").css("display", "none");
        $("#chineseSpan").css("display", "inline-block ");
        $("#russianSpan").css("display", "none");
        $("#onlyenglishSpan").css("display", "none");
        $("#onlyrussianSpan").css("display", "none");
    }
    if (language == "中文-英文") {
        $("#englishSpan").css("display", "inline-block ");
        $("#chineseSpan").css("display", "inline-block ");
        $("#onlyenglishSpan").css("display", "inline-block");
        $("#onlyrussianSpan").css("display", "none");
        $("#russianSpan").css("display", "none");
    }
    if (language == "中文-俄文") {
        $("#englishSpan").css("display", "none");
        $("#chineseSpan").css("display", "inline-block ");
        $("#russianSpan").css("display", "inline-block ");
        $("#onlyenglishSpan").css("display", "none");
        $("#onlyrussianSpan").css("display", "inline-block");
    }
}
//控制英文俄文列的隐藏与显示
function controlEngRus() {
    var language = $('#language').combobox('getValue');
    if (language == "中文-英文") {

        $('#tt_subTable').datagrid('showColumn', 'chncontent');
        $('#tt_subTable').datagrid('showColumn', 'engcontent');
        $('#tt_subTable').datagrid('hideColumn', 'ruscontent');
    }
    if (language == "中文-俄文") {
        $('#tt_subTable').datagrid('hideColumn', 'engcontent');
        $('#tt_subTable').datagrid('showColumn', 'chncontent');
        $('#tt_subTable').datagrid('showColumn', 'ruscontent');
    }
    if (language == "中文") {
        $('#tt_subTable').datagrid('hideColumn', 'engcontent');
        $('#tt_subTable').datagrid('hideColumn', 'ruscontent');
        $('#tt_subTable').datagrid('showColumn', 'chncontent');
    }
}

//绑定combobox
function bindCombobox() {
    //溢出率限定事件
    $("#overspill").numberbox({

        min: 0,
        max: 100,
        precision: 0,
        //suffix: '%',
        onChange: function () {

        }
    })
    //价格有效期限定事件
    $("#pvalidity").numberbox({

        precision: 0,
        //suffix: '天',
    });
    //绑定付款方式
    $("#paymentType").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=getPaymentType',
        valueField: 'code',
        textField: 'cname',
        editable: false,
        onSelect: function (record) {

        }

    });
    ////绑定买方简称
    //$("#simpleBuyer").combobox({
    //    url: '/ashx/Contract/loadCombobox.ashx?module=simplebuyer',
    //    required: false,
    //    valueField: 'shortname',
    //    textField: 'shortname',
    //    editable: false,
    //    onSelect: function (record) {
    //        var simpleBuyer = record.shortname;
    //        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
    //            $("#buyer").combobox('setValue', data.name);
    //            $("#buyeraddress").textbox('setValue', data.address);
    //            $('#buyercode').attr('value', data.code);

    //        }, 'json')

    //    }

    //});
    ////绑定卖方简称
    //$("#simpleSeller").combobox({
    //    url: '/ashx/Contract/loadCombobox.ashx?module=simpleseller',
    //    required: false,
    //    valueField: 'shortname',
    //    textField: 'shortname',
    //    editable: false,
    //    onSelect: function (record) {
    //        var simpleSeller = record.shortname;
    //        $.post("/ashx/Contract/loadPeople.ashx?module=seller", { simpleSeller: simpleSeller }, function (data) {
    //            $("#seller").combobox('setValue', data.name);
    //            $("#selleraddress").textbox('setValue', data.address);
    //            $('#sellercode').attr('value', data.code);

    //        }, 'json')

    //    }

    //});
    //绑定业务员编号
    $("#salesmanCode").combobox({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',
        required: false,
        valueField: 'UserRealName',
        textField: 'UserRealName',
        editable: false,
        onSelect: function (record) {
            $("#businessclass").combobox('setValue', record.Agency);
        }

    });
    //合同审核人绑定
    $("#adminReview").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=adminReview',
        required: false,
        valueField: 'cname',
        textField: 'cname',
        editable: false,
        onSelect: function (record) {

            $("#adminReviewNumber").val(record.code);
         
        }

    });
    //产品大类绑定
    $("#productCategory").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=product',
        required: false,
        valueField: 'productcategory',
        textField: 'productcategory',
        editable: false,
    });
    //销售组绑定
    $("#businessclass").combobox({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',
        required: false,
        valueField: 'Agency',
        textField: 'Agency',
        editable: false,
    });
    ////发运条款绑定
    //$('#shipment').combobox({
    //    url: '/ashx/Contract/loadCombobox.ashx?module=shipment',
    //    required: true,
    //    valueField: 'code',
    //    textField: 'cname',
    //    editable: false,
    //});
    $('#transport').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=transport',
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    $('#currency').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=currency',
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        multiple: false,
        onSelect: function (record) {
            //更改产品表的货币名称
            $("#htcplist").datagrid('acceptChanges');
            var rows = $("#htcplist").datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                $('#htcplist').datagrid('updateRow', {
                    index: i,
                    row: {
                        priceUnit: record.code,
                    }
                });


            }
        }
        
    });
    $('#tradement').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=tradement',
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    //$('#validity').combobox({
    //    url: '/ashx/Contract/loadCombobox.ashx?module=validity',
    //    required: true,
    //    valueField: 'code',
    //    textField: 'cname',
    //    editable: false,

    //});
    $('#pricement1').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=pricement1',
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    $('#pricement2').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=pricement2',
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,

    });

    $('#seller').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        idField: 'cname',
        textField: 'cname',
        editable: false,
        columns: [[
                    { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simpleSeller").textbox('setValue', rowdata.shortname);
            $("#sellercode").val(rowdata.code);
            $("#seller").combogrid('setValue', rowdata.name);
            $("#selleraddress").textbox('setValue', rowdata.address);
        }
    });
    $('#buyer').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=buyer',
        idField: 'cname',
        textField: 'cname',

        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simpleBuyer").textbox('setValue', rowdata.shortname);
            $("#buyer").combogrid('setValue', rowdata.name);
            $("#buyercode").val(rowdata.code);
            $("#buyeraddress").textbox('setValue', rowdata.address);
        }
    });
    //模板编号列表
    $('#templatename').combogrid({
        panelWidth: 100,
        url: '/ashx/Contract/loadCombobox.ashx?module=importTemplate',
        idField: 'templatename',
        textField: 'templatename',
        editable: false,
        columns: [[
         { field: 'templatename', title: '模板名称', width: 100 },
        ]],
        onSelect: function (index, rowdata) {
            $("#templateno").val(rowdata.templateno);
        }
    });

    //根据国家加载港口
    $('#harboroutCountry').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=harborout',
        valueField: 'country',
        textField: 'country',
        editable: false,
        onSelect: function (record) {
            $('#harborout').combogrid({
                panelWidth: 450,
                idField: 'name',
                textField: 'name',
                editable: false,
                url: '/ashx/Contract/loadCombobox.ashx?module=harborout&country=' + record.country,
                columns: [[
                            { field: 'code', title: '编码', width: 60 },
                            { field: 'country', title: '国家', width: 80 },
                            { field: 'name', title: '中文名', width: 100 },
                            { field: 'egname', title: '英文名', width: 100 },
                            { field: 'runame', title: '俄文名', width: 100 }
                ]]
            });
        }

    });
    $('#harboroutarriveCountry').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=harborout',
        valueField: 'country',
        textField: 'country',
        editable: false,
        onSelect: function (record) {
            $('#harborarrive').combogrid({
                panelWidth: 450,
                url: '/ashx/Contract/loadCombobox.ashx?module=harborout&country=' + record.country,
                idField: 'name',
                textField: 'name',
                editable: false,
                columns: [[
                            { field: 'code', title: '编码', width: 60 },
                            { field: 'country', title: '国家', width: 80 },
                            { field: 'name', title: '中文名', width: 100 },
                            { field: 'egname', title: '英文名', width: 100 },
                            { field: 'runame', title: '俄文名', width: 100 }
                ]]
            });
        }

    });
    $('#harborclear').combogrid({
        panelWidth: 450,
        idField: 'name',
        textField: 'name',
        url: '/ashx/Contract/loadCombobox.ashx?module=harborarrive',
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
        ]],
        onSelect: function (index, rowdata) {

        }
    });
    //价格百分比文本框改变事件
    $("#pricement1per").numberbox({
        min: 0,
        max: 100,
        precision: 0,
        suffix: '%',
        value: 100,
        onChange: function () {
            //获取文本框中的值
            var v = $('#pricement1per').numberbox('getValue');
            //获取价格条款2的值
            var price2 = $("#pricement2").combobox('getValue');
            //如果为100,则价格条款2和价格条款2百分比不可编辑
            if (parseInt(v) == 100) {
                $("#pricement2").combobox('setValue', "");
                $('#pricement2').combobox('disable', true);
                $('#pricement2per').numberbox('disable', true);
            }
            else {
                $("#pricement2").combobox({
                    disabled: false
                });
                $("#pricement2per").numberbox({
                    disabled: false
                });
                $("#pricement2").combobox('setValue', price2);
            }
            $("#pricement2per").numberbox('setValue', parseFloat(100) - parseFloat(v));

        }
    })
    $("#pricement2per").numberbox({

        min: 0,
        max: 100,
        precision: 0,
        suffix: '%',
        onChange: function () {
            //获取文本框中的值
            var v = $('#pricement2per').numberbox('getValue');
            $("#pricement1per").numberbox('setValue', parseFloat(100) - parseFloat(v));

        }
    })
}
//根据卖方买方简称加载全称和地址
function loadTotalName() {
    //卖方(简称)文本框改变事件
    $("input", $("#simpleSeller").next("span")).blur(function () {
        var simpleSeller = ($("#simpleSeller").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=seller", { simpleSeller: simpleSeller }, function (data) {
            $("#simpleSeller").textbox('setValue', data.shortname);
            $("#seller").combobox('setValue', data.name);
            $("#selleraddress").textbox('setValue', data.address);
            $('#sellercode').attr('value', data.code);

        }, 'json')

    })
    //买方(简称)文本框改变事件
    $("input", $("#simpleBuyer").next("span")).blur(function () {

        var simpleBuyer = ($("#simpleBuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
            $("#simpleBuyer").textbox('setValue', data.shortname);
            $("#buyer").combobox('setValue', data.name);
            $("#buyeraddress").textbox('setValue', data.address);
            $('#buyercode').attr('value', data.code);

        }, 'json')

    })

}

//弹出产品选择框,根据合同号加载产品
function popProductSelect() {
    $('#dd').dialog({
        title: '请选择产品',
        width: 600,
        height: document.documentElement.clientHeight,
        closed: true,
        cache: false,
        //href: 'Abroad_Product_Select.aspx',
        modal: true,
        buttons: [{
            text: '选择',
            handler: function () {
                $('#dd').dialog('close');
                //把选择的产品加载到当前页面
                var rows = $('#productlist').datagrid('getSelections');
                var oldrows = $('#htcplist').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    //判断当前表格里面是否有pname,同一个pcode下可以有多个pname
                    var isexists = false;
                    for (var j = 0; j < oldrows.length; j++) {
                        //if (oldrows[j].pname == rows[i].pname) {
                        if (oldrows[j].id == rows[i].id) {
                            isexists = true;
                            break;
                        }
                    }
                    if (isexists == false) {
                        var row = rows[i];
                        var newrow = {};
                        newrow.pcode = row.pcode;
                        newrow.pname = row.pname;
                        newrow.packspec = row.spec;
                        newrow.pallet = row.pallet;
                        newrow.packdes = row.packdes;
                        newrow.qunit = row.unit;
                        newrow.spec = row.spec;
                        newrow.unit = row.packageUnit;
                        newrow.ifcheck = row.customsInspec;
                        newrow.price = 0;
                        newrow.hsCode = row.hsscode;
                        newrow.quantity = 0;
                        newrow.amount = 0;
                        newrow.priceUnit = $("#currency").combobox('getValue');
                        newrow.packing = 0;
                        newrow.ifplace = 0;
                        newrow.skinWeight = 0;
                        newrow.packagesNumber = 1;
                        newrow.pnameen = row.pnameen;
                        newrow.pnameru = row.pnameru;


                        $('#htcplist').datagrid('appendRow', newrow);
                    }
                }
                $('#productlist').datagrid('clearSelections');
                if (endEditing()) {
                    editIndex = $('#htcplist').datagrid('getRows').length - 1;
                    $('#htcplist').datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
                }
            }
        }, {
            text: '取消',
            handler: function () {
                $("#dd").dialog('close');
            }
        }]
    });
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + contractNo,
        //width: document.documentElement.clientWidth,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        fitColumns: true,
        columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox', options: {precision:3} } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100 },
                          {
                              field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } }, formatter: function (value, row, index) {
                                  if (value == '' || value == null) {
                                      return 0;
                                  }
                                  else {
                                      return value;
                                  }
                              }
                          },
                    { field: 'spec', title: '产品规格', width: 100 },
                    { field: 'pallet', title: '最小包装', width: 100 },
                    { field: 'unit', title: '包装单位', width: 100 },
                    { field: 'packdes', title: '包装', width: 100 },
                    {
                        field: 'skinWeight', title: '皮重', width: 100, editor: {
                            type: 'numberbox', options: {
                                precision: 3
                            }
                        }
                    },
                    { field: 'packagesNumber', title: '件数', width: 100, editor: { type: 'numberbox', options: { precision: 0 } } },
                    { field: 'palletrequire', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
                    { field: 'ifplace', title: '是否产地证', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
                  
     
        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {
                //加载产品列表
                var category = $("#productCategory").combobox('getValue');
                var currency = $("#currency").combobox('getValue');
                if (currency == "") {
                    $.messager.alert("提醒", "请选择货币");
                    return;
                }
                $('#productlist').datagrid({
                    url: '/ashx/Contract/contractData.ashx?module=cplist&category=' + category,
                    pagination: true,
                    rownumbers: true,
                    sortName: 'pcode',
                    sortOrder: 'asc',
                    columns: [[
                                { field: 'pcode', title: 'SAP编号', width: 100 },
                                { field: 'productCategory', title: '产品类别', width: 100 },
                                { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                                { field: 'spec', title: '产品规格', width: 100 },
                                { field: 'unit', title: '计量单位', width: 100 },
                                { field: 'pallet', title: '最小包装', width: 100 },
                               { field: 'packageUnit', title: '包装单位', width: 100 },
                                { field: 'packdes', title: '包装', width: 100 },
                                { field: 'hsscode', title: 'HSS编码', width: 100 },
                                { field: 'customsInspec', title: '是否商检', width: 100 },
                                { field: 'pnameen', title: 'SAP名称(英文)', width: 100 },
                                { field: 'pnameru', title: 'SAP名称(俄文)', width: 100 },
                    ]]
                });
                //弹出
                $('#dd').window('open');
            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var rowindex = $('#htcplist').datagrid('getRowIndex', $('#htcplist').datagrid('getSelected'));
                $('#htcplist').datagrid('deleteRow', rowindex);
            }
        }],
        onClickCell: function (index, field, value) {
            $(this).datagrid('beginEdit', index);
            //setInterval(bindOnchange, 1000);
        },
        //根据编辑
        onClickRow: function (index, row) {
          
            var editors = $('#htcplist').datagrid('getEditors', index);
            var quantity = editors[0];
            var price = editors[1];
            var amount = editors[2];
            $(price.target).numberbox({
                onChange: function () {
                    $(amount.target).numberbox('setValue', quantity.target.val() * price.target.val());
                    $(amount.target).attr("disabled", true);
            }
            })
            //quantity.target.bind('onchange', function () {
            //    alert("aa");

            //});
           
        
            //row.amount = quantity *price;
            //alert(  parseFloat(row.quantity) * parseFloat(row.price));
        },
        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
}
//弹出关联产品选择框,根据合同号加载产品
function popContactProductSelect() {
    //$('.datagrid-editable .textbox,.datagrid-editable .datagrid-editable-input,.datagrid-editable .textbox-text').bind('keydown', function (e) {
    //    var code = e.keyCode || e.which;
    //    if (code == 13) {
    //        //保存更改 第一次编辑可能不会改变值
         
    //        //$('#eDatagrid').datagrid('acceptChanges');
    //        //$('#eDatagrid').datagrid('endEdit', editIndex);
    //        //do something
    //    }
    //});
    $('#dd').dialog({
        title: '请选择产品',
        width: 600,
        height: document.documentElement.clientHeight,
        closed: true,
        cache: false,
        //href: 'Abroad_Product_Select.aspx',
        modal: true,
        buttons: [{
            text: '选择',
            handler: function () {
                $('#dd').dialog('close');
                //把选择的产品加载到当前页面
                var rows = $('#productlist').datagrid('getSelections');
                var oldrows = $('#htcplist').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    //判断当前表格里面是否有pname,同一个pcode下可以有多个pname
                    var isexists = false;
                    for (var j = 0; j < oldrows.length; j++) {
                        if (oldrows[j].id == rows[i].id) {
                            isexists = true;
                            break;
                        }
                    }
                    if (isexists == false) {
                        var row = rows[i];
                        var newrow = {};
                        newrow.pcode = row.pcode;
                        newrow.pname = row.pname;
                        newrow.packspec = row.spec;
                        newrow.pallet = row.pallet;
                        newrow.packdes = row.packdes;
                        newrow.qunit = row.unit;
                        newrow.spec = row.spec;
                        newrow.unit = row.packageUnit;
                        newrow.ifcheck = row.customsInspec;
                        newrow.price = 0;
                        newrow.hsCode = row.hsscode;
                        newrow.quantity = 0;
                        newrow.amount = 0;
                        newrow.priceUnit = $("#currency").combobox('getValue');
                        newrow.packing = 0;
                        newrow.ifplace = 0;
                        newrow.pnameen = row.pnameen;
                        newrow.pnameru = row.pnameru;


                        $('#htcplist').datagrid('appendRow', newrow);
                    }
                }
                $('#productlist').datagrid('clearSelections');
                if (endEditing()) {
                    editIndex = $('#htcplist').datagrid('getRows').length - 1;
                    $('#htcplist').datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
                }
            }
        }, {
            text: '取消',
            handler: function () {
                $("#dd").dialog('close');
            }
        }]
    });
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + contractNo,
        //width: document.documentElement.clientWidth,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        fitColumns: true,
        columns: [[

		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox' ,options: {precision:3} } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    {
                        field: 'amountfloat', title: '价格增减', width: 100, editor: { type: 'numberbox', options: { precision: 2 } }, formatter: function (value,row,index) {
                            if (value == '' || value == null) {
                                return 0;
                            }
                            else {
                                return value;
                            }
                        }
                    },
                    { field: 'priceAdd', title: '增减后价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } }, formatter: function (value,row,index) {
                        if (value == '' || value == null) {
                            return row.price;
                        }
                        else {
                            return value;
                        }
                    } },
                    { field: 'priceUnit', title: '价格单位', width: 100 },
                    {
                        field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } }, formatter: function (value,row,index) {
                            if (value == '' || value == null) {
                                return 0;
                            }
                            else {
                                return value;
                            }
                        }
                    },
                    { field: 'spec', title: '产品规格', width: 100 },
                    { field: 'pallet', title: '最小包装', width: 100 },
                    { field: 'unit', title: '包装单位', width: 100 },
                    { field: 'packdes', title: '包装', width: 100 },
                    { field: 'skinWeight', title: '皮重', width: 100, editor: { type: 'numberbox', options: { precision: 3} } },
                    { field: 'packagesNumber', title: '件数', width: 100, editor: { type: 'numberbox', options: { precision: 0 } } },
                    { field: 'palletrequire', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
                    { field: 'ifplace', title: '是否产地证', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },


        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {
                //加载产品列表
                var category = $("#productCategory").combobox('getValue');
                var currency = $("#currency").combobox('getValue');
                if (currency == "") {
                    $.messager.alert("提醒", "请选择货币");
                    return;
                }
                $('#productlist').datagrid({
                    url: '/ashx/Contract/contractData.ashx?module=cplist&category=' + category,
                    pagination: true,
                    rownumbers: true,
                    sortName: 'pcode',
                    sortOrder: 'asc',
                    columns: [[
                                { field: 'pcode', title: 'SAP编号', width: 100 },
                                { field: 'productCategory', title: '产品类别', width: 100 },
                                { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                                { field: 'spec', title: '产品规格', width: 100 },
                                { field: 'unit', title: '计量单位', width: 100 },
                                { field: 'pallet', title: '最小包装', width: 100 },
                               { field: 'packageUnit', title: '包装单位', width: 100 },
                                { field: 'packdes', title: '包装', width: 100 },
                                { field: 'hsscode', title: 'HSS编码', width: 100 },
                                { field: 'customsInspec', title: '是否商检', width: 100 },
                                { field: 'pnameen', title: 'SAP名称(英文)', width: 100 },
                                { field: 'pnameru', title: 'SAP名称(俄文)', width: 100 },
                    ]]
                });
                //弹出
                $('#dd').window('open');
            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var rowindex = $('#htcplist').datagrid('getRowIndex', $('#htcplist').datagrid('getSelected'));
                $('#htcplist').datagrid('deleteRow', rowindex);
            }
        }],
        onClickCell: function (index, field, value) {
            $(this).datagrid('beginEdit', index);
            //setInterval(bindOnchange, 1000);
   
        },
        //根据编辑
        onClickRow: function (index, row) {
            var editors = $('#htcplist').datagrid('getEditors', index);
            var quantity = editors[0];
            var price = editors[1];
            var amountfloat = editors[2];
            var priceAdd = editors[3];
            var amount = editors[4];
         
            $(amountfloat.target).numberbox({
                onChange: function () {
                    var finalPrice = parseFloat(price.target.val()) + parseFloat(amountfloat.target.val());//增减后价格
                    $(priceAdd.target).numberbox('setValue', finalPrice);
                    var finamount = parseFloat(quantity.target.val()) * (parseFloat(priceAdd.target.val()));//计算后总金额
                    $(amount.target).numberbox('setValue', finamount);
                 
                }
            })
            //$(priceAdd.target).numberbox({
            //    onChange: function () {
                 
            //    }
            //})
          
            
           
        },
        onAfterEdit: function (index, row, changes) {
            //row.amount = row.quantity * row.price;
            row.amount = parseFloat(row.quantity) * (parseFloat(row.priceAdd));
            row.price = parseFloat(price.target.val()) + parseFloat(amountfloat.target.val());
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
}
//保存模板
function saveTemplate() {
    getSubTable();
    var form = $("#form1");
    $.ajax({
        cache: true,
        type: "POST",
        dataType: "json",
        url: "/ashx/Contract/templateContractOperator.ashx?module=saveTemplate",
        data: $('#form1').serialize(), // 你的formid
        async: false,
        error: function (data) {
            retdata.errdata = data;
        },
        success: function (data) {

            if (data != undefined && data.sucess == '0') {
                $.messager.alert('系统提示', data.errormsg, 'info');
            }

        }
    });
}
//绑定模板表格
function bindTemplateByContractNo() {
    var editRow = undefined;
    var toolbar = [{
        text: '添加',
        iconCls: 'icon-add',
        handler: function () {
            //新增空行
            var row = {};
            row.chncontent = "";
            row.engcontent = "";
            row.ruscontent = "";
            row.inline = '否';

            $('#tt_subTable').datagrid('appendRow', row);
        }
    }, {
        text: '编辑',
        iconCls: 'icon-edit',
        handler: function () {
            var row = $("#tt_subTable").datagrid('getSelected');
            if (row != null) {
                if (editRow != undefined) {
                    $("#tt_subTable").datagrid('endEdit', editRow);
                }

                if (editRow == undefined) {
                    var index = $("#tt_subTable").datagrid('getRowIndex', row);
                    $("#tt_subTable").datagrid('beginEdit', index);
                    editRow = index;
                    $("#tt_subTable").datagrid('unselectAll');
                }
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }

        }
    }, {
        text: '删除',
        iconCls: 'icon-remove',
        handler: function () {
            var row = $("#tt_subTable").datagrid('getSelected');
            if (row != undefined) {
                var editIndex = $("#tt_subTable").datagrid('getRowIndex', row);
                $('#tt_subTable').datagrid('cancelEdit', editIndex)
                .datagrid('deleteRow', editIndex);
                editIndex = undefined;

            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }, {
        text: '保存',
        iconCls: 'icon-remove',
        handler: function () {
            $('#tt_subTable').datagrid('acceptChanges');
        }
    }];

    //----初始化datagrid-----
    $('#tt_subTable').datagrid({
        nowrap: false,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        autoRowHeight: true,
        idField: 'sortno',
        url: '/ashx/Contract/templateContractData.ashx?module=contractTempList&no=' + contractNo,
        columns: [[
         { field: 'sortno', title: '序号', width: '50px', editor: { type: 'numberbox', options: { precision: 0 } } },
        { field: 'templateno', title: '模板编号', hidden: 'true' },
        {
            field: 'inline', title: '排在一行', width: '65px', align: 'center',
            editor: {
                type: 'checkbox',
                options: {
                    on: '是',
                    off: '否'
                }
            }
        },
        {
            field: 'variable', title: '变量', width: '200px', align: 'center',
            editor: {
                type: 'combobox',
                options: {
                    url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getvariable&flowdirection=' + importFlow,
                    method: 'get',
                    valueField: 'id',
                    textField: 'text',
                    multiple: true,
                    multiline: true,
                    panelHeight: 150,
                    height: 79,
                    onSelect: function (item) {
                        if (editRow == undefined) {
                            return;
                        }
                        //获取多选框里面的值

                        var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                        var chn = editors[3];
                        var eng = editors[4];
                        var rus = editors[5];

                        additem(chn.target, item.id);

                        additem(eng.target, item.id);

                        additem(rus.target, item.id);
                    },
                    onUnselect: function (item) {
                        if (editRow == undefined) {
                            return;
                        }
                        //获取多选框里面的值

                        var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                        var chn = editors[3];
                        var eng = editors[4];
                        var rus = editors[5];

                        removeitem(chn.target, item.id);

                        removeitem(eng.target, item.id);

                        removeitem(rus.target, item.id);
                    }
                }
            }
        },
        { field: 'chncontent', title: '中文', width: '300px', editor: 'textarea', styler: contentStyle },
        { field: 'engcontent', title: '英文', width: '300px', editor: 'textarea', styler: contentStyle },
        { field: 'ruscontent', title: '俄文', width: '300px', editor: 'textarea', styler: contentStyle }
        ]],
        pagination: true,
        toolbar: toolbar,
        onAfterEdit: function (rowIndex, rowData, changes) {
            editRow = undefined;
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);
            }
            if (editRow == undefined) {
                $("#tt_subTable").datagrid('beginEdit', rowIndex);
                editRow = rowIndex;
            }
        },
        onClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);

            }
        }
    });
}
//绑定模板表格
function bindTemplate(templateno) {
    var editRow = undefined;
    var toolbar = [{
        text: '添加',
        iconCls: 'icon-add',
        handler: function () {
            //新增空行
            var row = {};
            row.chncontent = "";
            row.engcontent = "";
            row.ruscontent = "";
            row.inline = '否';

            $('#tt_subTable').datagrid('appendRow', row);
        }
    }, {
        text: '编辑',
        iconCls: 'icon-edit',
        handler: function () {
            var row = $("#tt_subTable").datagrid('getSelected');
            if (row != null) {
                if (editRow != undefined) {
                    $("#tt_subTable").datagrid('endEdit', editRow);
                }

                if (editRow == undefined) {
                    var index = $("#tt_subTable").datagrid('getRowIndex', row);
                    $("#tt_subTable").datagrid('beginEdit', index);
                    editRow = index;
                    $("#tt_subTable").datagrid('unselectAll');
                }
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }

        }
    }, {
        text: '删除',
        iconCls: 'icon-remove',
        handler: function () {
            var row = $("#tt_subTable").datagrid('getSelected');
            if (row != undefined) {
                var editIndex = $("#tt_subTable").datagrid('getRowIndex', row);
                $('#tt_subTable').datagrid('cancelEdit', editIndex)
                .datagrid('deleteRow', editIndex);
                editIndex = undefined;

            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }, {
        text: '保存',
        iconCls: 'icon-remove',
        handler: function () {
            $('#tt_subTable').datagrid('acceptChanges');
        }
    }];

    //----初始化datagrid-----
    $('#tt_subTable').datagrid({
        nowrap: false,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        autoRowHeight: true,
        idField: 'mname',
        url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getdetail&no=' + templateno,
        columns: [[
         { field: 'sortno', title: '序号', width: '50px', editor: { type: 'numberbox', options: { precision: 0 } } },
        { field: 'templateno', title: '模板编号', hidden: 'true' },
        {
            field: 'inline', title: '排在一行', width: '65px', align: 'center',
            editor: {
                type: 'checkbox',
                options: {
                    on: '是',
                    off: '否'
                }
            }
        },
        {
            field: 'variable', title: '变量', width: '200px', align: 'center',
            editor: {
                type: 'combobox',
                options: {
                    url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getvariable&flowdirection=' + importFlow,
                    method: 'get',
                    valueField: 'id',
                    textField: 'text',
                    multiple: true,
                    multiline: true,
                    panelHeight:150,
                    height: 79,
                    onSelect: function (item) {
                        if (editRow == undefined) {
                            return;
                        }
                        //获取多选框里面的值

                        var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                        var chn = editors[3];
                        var eng = editors[4];
                        var rus = editors[5];

                        additem(chn.target, item.id);

                        additem(eng.target, item.id);

                        additem(rus.target, item.id);
                    },
                    onUnselect: function (item) {
                        if (editRow == undefined) {
                            return;
                        }
                        //获取多选框里面的值

                        var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                        var chn = editors[3];
                        var eng = editors[4];
                        var rus = editors[5];

                        removeitem(chn.target, item.id);

                        removeitem(eng.target, item.id);

                        removeitem(rus.target, item.id);
                    }
                }
            }
        },
        { field: 'chncontent', title: '中文', width: '300px', editor: 'textarea', styler: contentStyle },
        { field: 'engcontent', title: '英文', width: '300px', editor: 'textarea', styler: contentStyle },
        { field: 'ruscontent', title: '俄文', width: '300px', editor: 'textarea', styler: contentStyle }
        ]],
        pagination: true,
        toolbar: toolbar,
        onAfterEdit: function (rowIndex, rowData, changes) {
            editRow = undefined;
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);
            }

            if (editRow == undefined) {
                $("#tt_subTable").datagrid('beginEdit', rowIndex);
                editRow = rowIndex;
            }
        },
        onClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);

            }
        }
    });
}
//获取是否报关的买卖双方为境内境外
function getClassificValidate() {
    var validata = {};
    var bool = false;
    //获取买卖双方为境内境外
    var buyercode = $("#buyercode").val();
    var sellercode = $("#sellercode").val();
    var user = {
        buyercode: $("#buyercode").val(),
        sellercode: $("#sellercode").val(),
    };
    $.ajax({
        cache: true,
        type: "POST",
        url: "/ashx/Contract/loadOther.ashx?module=loadClassific",
        data: user, // 你的formid
        async: false,
        error: function (data) {
            retdata.errdata = data;
        },
        success: function (data) {
            validata = data;
            var isCustom = $("input[name='iscustoms']:checked").val();//是否报关
            if (isCustom == "是") {//是报关
                if (validata.customFic == validata.supplierFic) {//都为境内或都为境外
                    $.messager.confirm('提醒', '为报关合同，买卖方为同一流向，确定要继续吗？', function (r) {
                        if (r) {
                            submitValidate();
                        }
                    });
                }
                else {
                    submitValidate();
                }
            }
            else {
                if (validata.customFic != validata.supplierFic) {//不是报关合同
                    $.messager.confirm('提醒', '不是报关合同，买卖方为不同流向，确定要继续吗？', function (r) {
                        if (r) {
                            submitValidate();
                        }
                    });
                }
                else {
                    submitValidate();
                }
            }

        }
    }, 'json');

}
//提交验证
function submitValidate() {
    var rrdata = SaveDataToDB(1);//提交
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        $.messager.alert('系统提示', '提交成功!', 'info', function () {
            //打开指定模板页面
            window.top.selectAndRefreshTab('进口合同');

        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
}

//为编辑框绑定onchange事件
function bindOnchange() {
    var row = $('#htcplist').datagrid('getSelected');
    var index = $('#htcplist').datagrid('getRowIndex',row);
    var editors = $('#htcplist').datagrid('getEditors', index);
    var quantity = editors[0];
    var price = editors[1];
    var amountfloat = editors[2];
    var priceAdd = editors[3];
    var amount = editors[4];
    quantity.target.bind('blur', function () {
        alert("aa");

    });
}
