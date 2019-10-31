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
    if (templateno == "") {
        //默认签订地点，唛头
        $("#shippingmark").textbox('setValue', 'N/M');
        $("#signedplace").textbox('setValue', '乌鲁木齐');
       
    } else {
        $("#form1").form('load', '/ashx/Contract/templateContractData.ashx?module=LoadTemplateData&templateno=' + templateno + '&flowdirection=' + flowdirection);
        //控制英文俄文列的隐藏与显示
        //controlEngRusBegin();
    }
    //绑定combobox数据
    bindCombobox();
    //绑定数据
    bindUI();
    //根据卖方买方简称加载全称和地址
    loadTotalName();
    //绑定模板表格
    bindTemplate();
    //控制英文俄文列的隐藏与显示
    controlEngRusBegin();
    //tabs选中事件，查看模板文本
    selectTabs();
});
//绑定模板表格
function bindTemplate() {
    var editRow = undefined;
    var toolbar = [{
        text: '添加',
        iconCls: 'icon-add',
        handler: function () {
            //新增空行
            var data = $('#tt_subTable').datagrid('getData');
            var row = {};
            row.chncontent = "";
            row.engcontent = "";
            row.ruscontent = "";
            row.inline = '否';
            //row.sortno = data.total + 1;
            row.sortno ="";
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
                    url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getvariable&flowdirection='+flowdirection,
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

var bindUI = function () {
    //为table填充数据

    //签订日期不可编辑
    $('#signedtime').datebox('disable', true);
    $('#pvalidity').datebox('disable', true);
    $('#validity').datebox('disable', true);


}

var contentStyle = function (value, row, index) {
    return "";
}
//保存
function save() {

    getSubTable();
    //校验必填项
    var templatename = $("#templatename").textbox('getValue');
    var productCategory = $("#productCategory").combobox('getValue');
    var language = $("#language").combobox('getValue');
    if (templatename == "" || productCategory == "" || language == "") {
        $.messager.alert('提示', '请填写必填项数据！', 'info');
        return;
    }
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
             
                if (flowdirection == importFlow) {
                        window.top.selectAndRefreshTab('进口模板');
                    }
                if (flowdirection == exportFlow) {

                window.top.selectAndRefreshTab('出口模板');
                      
                    }

            
            }
        }

    });
}
//取消
function cancel() {

    //关闭当前tab
    window.top.closeTab();
}
//获取子表数据
function getSubTable() {
      $("#tt_subTable").datagrid("acceptChanges");
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

//控制英文俄文列的隐藏与显示
function controlEngRus() {
    var language = $('#language').combobox('getValue');
    if (language == "中文-英文") {
        $('#tt_subTable').datagrid('hideColumn', 'ruscontent');
        $('#tt_subTable').datagrid('showColumn', 'engcontent');
        $('#tt_subTable').datagrid('showColumn', 'chncontent');
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
//控制英文俄文列的隐藏与显示
function controlEngRusBegin() {
 
    if (language == "中文-英文") {
        $('#tt_subTable').datagrid('hideColumn', 'ruscontent');
        $('#tt_subTable').datagrid('showColumn', 'engcontent');
        $('#tt_subTable').datagrid('showColumn', 'chncontent');
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
//隐藏语言
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
//tabs选中事件
function selectTabs() {
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {

            if (title == "模板预览") {
                var language = $("#language").combobox('getValue');
                var buyercode = $("#buyercode").val();
                var sellercode = $("#sellercode").val();
                var signedplace = $("#signedplace").textbox('getValue');
                var signedtime = $("#signedtime").datebox('getValue');
                var selleraddress = $("#selleraddress").textbox('getValue');
                var tradement = $("#tradement").combobox('getValue');
                var transport = $("#transport").combobox('getValue');
                var harborout = $("#harborout").combobox('getValue');
                var harborarrive = $("#harborarrive").combobox('getValue');
                var pricement1 = $("#pricement1").combobox('getValue');
                var deliveryPlace = $("#deliveryPlace").textbox('getValue');
                var pricement2 = $("#pricement2").combobox('getValue');
                var buyeraddress = $("#buyeraddress").textbox('getValue');
                var pvalidity = $("#pvalidity").textbox('getValue');
                var validity = $("#validity").combobox('getValue');
                var shipment = $("#shipment").combobox('getValue');
                var placement = $("#placement").textbox('getValue');
                var shippingmark = $("#shippingmark").val();
                var overspill = $("#overspill").numberbox('getValue');
                var paymentType = $("#paymentType").combobox('getValue');
                var shipDate = $("#shipDate").datebox('getValue');
                //获取模板条款列表
                $("#tt_subTable").datagrid("acceptChanges");
                var datagrid = $("#tt_subTable").datagrid("getRows");
                var templatejson = JSON.stringify(datagrid);
                //语言控制
                hideLanguage();
                //为隐藏域templatejson 赋值
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
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/templateContractData.ashx?module=GetTempldatePVC",
                    dataType: "text",
                    data: {
                        tempjson: templatejson, buyercode: buyercode, buyeraddress: buyeraddress, signedtime: signedtime,
                        signedplace: signedplace, language: language, sellercode: sellercode, tradement: tradement, transport: transport,
                        harborout: harborout, harborarrive: harborarrive, pricement1: pricement1, deliveryplace: deliveryPlace, pricement2: pricement2,
                        pvalidity: pvalidity, validity: validity, shipment: shipment, placement: placement, shippingmark: shippingmark
                        , overspill: overspill, shipDate: shipDate, paymentType: paymentType
                    },
                    success: function (data) {
                        $("#templatejson").val(data);
                    }
                });
                $("#previewFormFrame").attr('src', '/Bus/ContractTemplate/previewContractTemplate.aspx');
                //语言选择点击事件
                $(":checkbox").click(function () {
                    var str = "";
                    $('input[name="checkbox"]:checked').each(function () {
                        str += $(this).val() + ",";
                    });
                    $.ajax({
                        type: "post",
                        url: "/ashx/Contract/templateContractData.ashx?module=GetTempldatePVC",
                        dataType: "text",
                        data: {
                            tempjson: templatejson, buyercode: buyercode, buyeraddress: buyeraddress, signedtime: signedtime,
                            signedplace: signedplace, language: str, sellercode: sellercode, tradement: tradement, transport: transport,
                            harborout: harborout, harborarrive: harborarrive, pricement1: pricement1, deliveryplace: deliveryPlace, pricement2: pricement2,
                            pvalidity: pvalidity, validity: validity, shipment: shipment, placement: placement, shippingmark: shippingmark, overspill: overspill,
                            shipDate: shipDate, paymentType: paymentType
                        },
                        success: function (data) {
                            $("#templatejson").val(data);
                        }
                    });
                    $("#previewFormFrame").attr('src', '/Bus/ContractTemplate/previewContractTemplate.aspx');
                })
            }
        }
    })
}

//校验必填项
function valideInput() {
    var templatename = $("#templatename").textbox('getValue');
    var productCategory = $("#productCategory").combobox('getValue');
    var language = $("#language").combobox('getValue');
    if (templatename == "" || productCategory == "" || language == "") {
        $.messager.alert('提示', '请填写必填项数据！', 'info');
        return;
    }

}

//绑定combobox
function bindCombobox() {
    //溢出率限定事件
    $("#overspill").numberbox({
        min: 0,
        max: 100,
        precision: 0,
        suffix: '%',
        onChange: function () {
        }
    })
    //绑定付款方式
    $("#paymentType").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=getPaymentType',
        valueField: 'code',
        textField: 'cname',
        editable: false,
        onSelect: function (record) {

        }

    });
    //价格有效期限定事件
    $("#pvalidity").numberbox({
        min: 0,
        max: 100,
        precision: 0,
        suffix: '天',
        onChange: function () {

        }
    })
    //语言绑定,选择时改变模板预言加载内容
    $("#language").combobox({
        onSelect: function (record) {
            controlEngRus();
        }
    });

    //产品大类绑定
    $("#productCategory").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=product',
        valueField: 'productcategory',
        textField: 'productcategory',
        editable: false,
    });

    ////发运条款绑定
    //$('#shipment').combobox({
    //    url: '/ashx/Contract/loadCombobox.ashx?module=shipment',

    //    valueField: 'code',
    //    textField: 'cname',
    //    editable: false,
    //});
    //运输方式
    $('#transport').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=transport',

        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    //货币
    $('#currency').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=currency',

        valueField: 'code',
        textField: 'cname',
        editable: false,
        multiple: false,

    });
    //贸易条款
    $('#tradement').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=tradement',
        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
 
    $('#pricement1').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=pricement1',

        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    $('#pricement2').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=pricement2',

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
   
    //出口口岸
    $('#harborout').combogrid({
        panelWidth: 450,
        idField: 'name',
        textField: 'name',
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
    //到货口岸
    $('#harborarrive').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=harborarrive',
        idField: 'name',
        textField: 'name',
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
    //价格百分比文本框改变事件
    $("#pricement1per").numberbox({
        min: 0,
        max: 100,
        precision: 0,
        suffix: '%',
        value:100,
        onChange: function () {
            //获取文本框中的值
            var v = $('#pricement1per').numberbox('getValue');
            //获取价格条款2的值
            var price2 = $("#pricement2").combobox('getValue');
            //如果为100,则价格条款2和价格条款2百分比不可编辑
            if (parseInt(v) == 100) {
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

            }
            $("#pricement2per").numberbox('setValue', parseFloat(100) - parseFloat(v));
            $("#pricement2").combobox('setValue', price2);
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
