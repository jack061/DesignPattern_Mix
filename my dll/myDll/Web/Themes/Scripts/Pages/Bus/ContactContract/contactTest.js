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
    //$("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadContactApp&contractNo=' + contractNo);
    //绑定combobox 
    bindCombobox();
    //绑定产品列表
    popProductSelect();
    //绑定默认数据
   bindUI();
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
        window.top.selectAndRefreshTab('海运关联合同');
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        $.messager.alert("提醒", rrdata.sucdata.errormsg);
    }
}
//取消
var undo = function () {

    //关闭当前tab
    window.top.closeTab();
}
//提交
var submit = function () {

    var rrdata = SaveDataToDB(1);
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        $.messager.alert('系统提示', '提交成功!', 'info', function () {
            //打开指定模板页面
            window.top.selectTab('海运关联合同');

        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
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
    //获取list数据到htcplistStr
    $("#htcplist").datagrid("acceptChanges");
    var cplist = $("#htcplist").datagrid("getRows");
    var datagridjson = JSON.stringify(cplist);
    $('#htcplistStr').val(datagridjson);
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/Contract/contractOperater.ashx?module=saveCotactTrainContract&status=' + status,
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
    //为table填充数据
    $('#maintable').form('load', htdata);
    //为buyercode,sellercode 赋值
    $('#buyercode').val(htdata.buyercode);
    $("#purchasecode").val(htdata.purchasecode);
    //为模板编号赋值
    $('#templateno').val(templateno);
    $('#contractNo').textbox('setValue', '自动编号');
    $('#contractNo').textbox('readonly', true);
    //默认签订地点
    $("#signedplace").textbox('setValue', '乌鲁木齐');
    //说明为创建框架合同
    if (isFrame == "true") {
        $("input[name='frameContract'][value='是']").attr("checked", true);
    }
    else {
        $("input[name='frameContract'][value='否']").attr("checked", true);
    }


  
}

//查询产品
function SearchContract() {
    pcode = $('#pcode').textbox('getText');

    name = $('#pname').textbox('getText');
    para = {};
    para.pcode = pcode;
    para.name = name;
    $('#productlist').datagrid('load', para);
    //alert(para.name);
    ////queryData = {};
    ////queryData.pcode = $('#pcode').textbox('getValue');
    ////queryData.name = $('#name').textbox('getValue');
    ////alert($('#pcode').textbox('getValue'));

    ////$('#productlist').datagrid('options').queryParams = queryData;
    ////$('#productlist').datagrid('reload');
    ////$('#productlist').datagrid({queryParams:queryData});


}

//tabs选中事件
function selectTabs() {
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
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
            if (title == "合同预览") {
                var templateno = $("#templateno").val();
                bindTemplate(templateno);
                //获取模板条款列表
                var datagrid = $("#tt_subTable").datagrid("getRows");
                var templatejson = JSON.stringify(datagrid);
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
                        pvalidity: pvalidity, shipment: shipment, placement: placement
                    },
                    success: function (data) {
                        $("#realTimeContractText").val(data);
                    }
                });
                $("#previewFormFrame").attr('src', '/Bus/ContractCategory/previewContract.aspx');
                $(":checkbox").click(function () {
                    var str = "";
                    $('input[name="checkbox"]:checked').each(function () {
                        str += $(this).val() + ",";
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
                            pvalidity: pvalidity, shipment: shipment, placement: placement
                        },
                        success: function (data) {
                            $("#realTimeContractText").val(data);
                        }
                    });
                    $("#previewFormFrame").attr('src', '/Bus/ContractCategory/previewContract.aspx');

                })



            }
            if (title == "文本条款") {

                var templateno = $("#templateno").val();
                bindTemplate(templateno);
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
                var language = "zh";
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
                $("#packinglist").attr('src', '/Bus/Contract/WebForm1.aspx?buyer=' + buyer + '&buyername=' + buyername + '&seller=' + seller + '&sellername=' + sellername + '&validity=' + validity + '&language=' + language
                 + '&signedTime=' + signedTime + '&signedplace=' + signedplace + '&productlist=' + datagridjson
               + '&tradement=' + tradement + '&transport=' + transport + '&harborout=' + harborout + '&harborarrive=' + harborarrive
                 + '&deliveryPlace=' + deliveryPlace + '&pricement1=' + pricement1 + '&pricement2=' + pricement2 + '&validity=' + validity
                   + '&pvalidity=' + pvalidity + '&shipment=' + shipment + '&placement=' + placement + '&currency=' + currency + '&type=packinglist');

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
                var language = "zh";

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

                $("#Invoice").attr('src', '/Bus/Contract/WebForm1.aspx?buyer=' + buyer + '&buyername=' + buyername + '&seller=' + seller + '&sellername=' + sellername + '&validity=' + validity + '&language=' + language
                     + '&signedTime=' + signedTime + '&signedplace=' + signedplace + '&productlist=' + datagridjson
                   + '&tradement=' + tradement + '&transport=' + transport + '&harborout=' + harborout + '&harborarrive=' + harborarrive
                     + '&deliveryPlace=' + deliveryPlace + '&pricement1=' + pricement1 + '&pricement2=' + pricement2 + '&validity=' + validity
                       + '&pvalidity=' + pvalidity + '&shipment=' + shipment + '&placement=' + placement + 'currency=' + currency + '&type=invoice');


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
    if (flowdirection == "进境") {
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
                    url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getvariable',
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

function hideLanguage() {

    var language = $("#language").combobox('getText');

    if (language == "中文") {

        $("#englishSpan").css("display", "none");
        $("#chineseSpan").css("display", "inline-block ");
        $("#russianSpan").css("display", "none");
    }
    if (language == "中文-英文") {
        $("#englishSpan").css("display", "inline-block ");
        $("#chineseSpan").css("display", "inline-block ");
        $("#russianSpan").css("display", "none");
    }
    if (language == "中文-俄文") {
        $("#englishSpan").css("display", "none");
        $("#chineseSpan").css("display", "inline-block ");
        $("#russianSpan").css("display", "inline-block ");
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
    //产品大类绑定
    $("#product").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=product',
        required: true,
        valueField: 'productcategory',
        textField: 'productcategory',
        editable: false,
    });
    //发运条款绑定
    $('#shipment').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=shipment',
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
    });
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

    });
    $('#tradement').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=tradement',
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    $('#validity').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=validity',
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
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
            $("#simpleseller").textbox('setValue', rowdata.shortname);
            $("#sellercode").val(rowdata.code);
            $("#seller").combogrid('setValue', rowdata.name);
            $("#selleraddress").textbox('setValue', rowdata.address);
        }
    });
    //模板编号列表
    $('#templatename').combogrid({
        panelWidth: 100,
        url: '/ashx/Contract/loadCombobox.ashx?module=exportTemplate',
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

    $('#harborout').combogrid({
        panelWidth: 450,
        idField: 'code',
        textField: 'cname',
        url: '/ashx/Contract/loadCombobox.ashx?module=harborout',
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
        ]],
        onSelect: function (index, rowdata) {
            $("#harboroutCountry").textbox('setValue', rowdata.country);
        }
    });
    $('#harborarrive').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=harborarrive',
        idField: 'code',
        textField: 'cname',
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
        ]],
        onSelect: function (index, rowdata) {
            $("#harboroutarriveCountry").textbox('setValue', rowdata.country);
        }
    });
    $('#harborclear').combogrid({
        panelWidth: 450,
        idField: 'code',
        textField: 'cname',
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
        onChange: function () {
            //获取文本框中的值
            var v = $('#pricement1per').numberbox('getValue');
            //如果为100,则价格条款2和价格条款2百分比不可编辑
            if (parseInt(v) == 100) {
                $('#pricement2').combobox('disable', true);
                $('#pricement2per').combobox('disable', true);
            }
            else {
                $("#pricement2").combobox({
                    disabled: false
                });
                $("#pricement2per").combobox({
                    disabled: false
                });

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
    $("input", $("#simpleseller").next("span")).blur(function () {

        var simpleseller = ($("#simpleseller").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=seller", { simpleseller: simpleseller }, function (data) {
            $("#seller").combobox('setValue', data.name);
            $("#selleraddress").textbox('setValue', data.address);
            $('#sellercode').attr('value', data.code);

        }, 'json')

    })
    //买方(简称)文本框改变事件
    $("input", $("#simplebuyer").next("span")).blur(function () {

        var simplebuyer = ($("#simplebuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simplebuyer: simplebuyer }, function (data) {

            $("#buyer").combobox('setValue', data.name);
            $("#buyeraddress").textbox('setValue', data.address);
            $('#buyercode').attr('value', data.code);

        }, 'json')

    })

}
//弹出产品选择框,根据合同号加载产品
function popProductSelect() {
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + contractNo,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox' } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100 },
                    { field: 'amount', title: '金额', width: 100, },
                    { field: 'packspec', title: '包装规格', width: 100, editor: 'textbox' },
                    { field: 'packing', title: '包装描述', width: 100, editor: 'textbox' },
                    { field: 'pallet', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
                    { field: 'ifplace', title: '是否产地证', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' }
        ]],
        onClickCell: function (index, row, changes) {
            $(this).datagrid('beginEdit', index);
        },


        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
}

