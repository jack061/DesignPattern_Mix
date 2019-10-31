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
////返回
var back = function () {

    window.top.closeAddTab("合同-新增", "/Bus/Contract/ContractTest.aspx");
}
//保存
var save = function () {
    var rrdata = SaveDataToDB(0);

    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        window.top.selectAndRefreshTab('出口合同');

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
            window.top.selectTab('境外销售合同');

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
    if (endEditing() != true) {
        return;
    }


    var retdata = {};
    //获取list数据到htcplistStr
    $("#htcplist").datagrid("acceptChanges");
    var cplist = $("#htcplist").datagrid("getRows");

    var count = $("#splitShipment").numberbox('getValue');

    //当分批发货数量大于1时
    if (count > 1) {
        $("#splitCplist").datagrid('acceptChanges');
        var splitCplist = $("#splitCplist").datagrid("getRows");
        var splitJson = JSON.stringify(splitCplist);
        $('#splitStr').attr('value', splitJson);
    }
    else {
        $('#splitStr').attr('value', "0");
    }


    //校验分批发货产品数量
    var cplistRow = $("#htcplist").datagrid("getSelected");
    if (cplistRow != null) {
        $("#productCount").attr('value', cplistRow.quantity);
    }
    else {
        $("#productCount").attr('value', "0");
    }


    var datagridjson = JSON.stringify(cplist);

    alert(datagridjson);

    $('#htcplistStr').attr('value', datagridjson);



    var action = "";
    var Internal = "";
    //if (contractNo.length > 0) {

    //    if (isview == "true") {
    //        //表明是复制合同
    //        action = 'addcontract&isview=true';
    //    }
    //    else {
    //        action = 'editcontract&isview=false';
    //    }


    //}
    //else {
    //    action = 'addcontract&isview=false';
    //}

    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/Contract/contractOperater.ashx?module=addContractTest&status=' + status + '&contactCode=' + contactCode + '&isInternal=' + Internal,
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
console.time('初始化');
$(function () {
    //绑定数据源
    bindUI();
    //隐藏语言
    hideLanguage();
    controlEngRus();
    //设置价格条款是否可编辑
    priceTermsChange();
    //控制箱单发票显示
    showInvoice();
    //tabs选中事件
    selectTabs();
    //卖方(简称)文本框改变事件
    $("input", $("#simpleseller").next("span")).blur(function () {
        var simpleSeller = ($("#simpleseller").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=seller", { simpleSeller: simpleSeller }, function (data) {

            $("#seller").combobox('setValue', data.name);
            $("#selleraddress").textbox('setValue', data.address);
            $('#sellername').attr('value', data.code);

        }, 'json')

    })
    //买方(简称)文本框改变事件
    $("input", $("#simplebuyer").next("span")).blur(function () {

        var simpleBuyer = ($("#simplebuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {

            $("#buyer").combobox('setValue', data.name);
            $("#buyeraddress").textbox('setValue', data.address);
            $('#buyername').attr('value', data.code);

        }, 'json')

    })
    console.time('bindui方法');

    console.timeEnd('bindui方法');
    //模板修改表格
    //var templatename=$("#templateno").combobox('getValue');
    //alert(templatename);

    //预览表格弹出
    $("#previewForm").dialog({
        title: '合同-预览',
        width: 1000,
        height: 600,
        closed: true,
        cache: false,
        modal: true,
        maximizable: false,
        maximized: true,
        resizable: true,
        inline: true
    });
    //绑定grid
    console.time('附件列表');
    $('#htfjlist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=htfjpagelist&contractNo=' + htdata.contractNo,
        pagination: true,
        rownumbers: true,
        sortName: 'attachmentno',
        sortOrder: 'asc',
        columns: [[
                    { field: 'status1', title: '状态', width: 100 },
                    { field: 'templateno', title: '模板编号', width: 100 },
		            { field: 'language', title: '语言', width: 100 },
		            { field: 'seller', title: '卖方', width: 100, align: 'right' },
                    { field: 'signedtime', title: '签订时间', width: 100 },
                    { field: 'signedplace', title: '签订地点', width: 100 },
                    { field: 'buyer', title: '买家', width: 100 },
                    { field: 'buyeraddress', title: '买家地址', width: 100 },
                    { field: 'currency', title: '货币', width: 100 },
                    { field: 'pricement1', title: '价格条款1', width: 100 },
                    { field: 'pricement1per', title: '价格条款1占比', width: 100 },
                    { field: 'pricement2', title: '价格条款2', width: 100 },
                    { field: 'pricement2per', title: '价格条款2占比', width: 100 },
                    { field: 'pvalidit', title: '价格有效期', width: 100 },
                    { field: 'shipment', title: '发运条款', width: 100 },
                    { field: 'transport', title: '运输方式', width: 100 },
                    { field: 'tradement', title: '贸易条款', width: 100 },
                    { field: 'harborout', title: '出口口岸', width: 100 },
                    { field: 'harborarrive', title: '到货口岸', width: 100 },
                    { field: 'harborclear', title: '清关口岸', width: 100 },
                    { field: 'placement', title: '产地条款', width: 100 },
                    { field: 'validity', title: '合同有效期', width: 100 },
                    { field: 'remark', title: '备注', width: 100 }
        ]],
        toolbar: [/*{
                    iconCls: 'icon-add',
                    text: '新增',
                    handler: function () { alert('add') }
                }, '-', {
                    iconCls: 'icon-edit',
                    text: '修改',
                    handler: function () { alert('修改') }
                }*/]
    });
    console.timeEnd('附件列表');

    console.time('产品列表');
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
                    {field: 'qunit', title: '数量单位', width: 100},
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100 },
                    { field: 'amount', title: '金额', width: 100, },
                    { field: 'packspec', title: '包装规格', width: 100, editor: 'textbox' },
                    { field: 'packing', title: '包装描述', width: 100, editor: 'textbox' },
                    { field: 'pallet', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
                    { field: 'ifplace', title: '是否产地证', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' }
        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {
                //加载产品列表
                var category = $("#product").combobox('getValue');

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
                                { field: 'unit', title: '单位', width: 100 },
                                { field: 'spec', title: '规格', width: 100 },
                                { field: 'packdes', title: '包装描述', width: 100 },
                                { field: 'pallet  ', title: '包装规格', width: 100 },
                                { field: 'hsscode', title: 'HSS编码', width: 100 },
                                { field: 'ifinspection', title: '是否商检', width: 100 },
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
        onClickCell: function (index, row, changes) {
            var row = $("#htcplist").datagrid('getSelected');
            var splitNumber = $("#splitShipment").numberbox('getValue');
            //根据选择发货的数字加载产品子表
            var contractNo = $("#contractNo").textbox('getValue');
            loadProuductByNumber(splitNumber, row.pname, row.pcode, contractNo);
            if (editIndex != index) {
                var mygrid = $('#htcplist');
                if (endEditing()) {
                  
                    $(this).datagrid('beginEdit', index);
                    var ed = $(this).datagrid('getEditor', { index: index, field: field });
                    if ($(ed.target) != undefined) {
                        $(ed.target).focus();
                    }
                    editIndex = index;
                } else {
                    setTimeout(function () {
                        mygrid.datagrid('selectRow', editIndex);
                    }, 0);
                }
            }

        },
 
    
        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
    console.timeEnd('产品列表');
    //分批发货文本框改变事件
    $("#splitShipment").numberbox({

        min: 0,
        max: 10,
        precision: 0,

        onChange: function () {


        }
    })
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

});


var bindUI = function () {
    //为隐藏域关联合同号赋值
    $("#purchaseCode").val(contactCode);
    $("#no").val(htdata.contractNo);
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
                        if (oldrows[j].pname == rows[i].pname) {
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
                        newrow.ifcheck = row.ifinspection;
                        newrow.price = 0;
                        newrow.quantity = 0;
                        newrow.amount = 0;
                        newrow.priceUnit = $("#currency").combobox('getValue');
                        newrow.packing = 0;
                        newrow.ifplace = 0;

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
    //为table填充数据
    $('#maintable').form('load', htdata);
    //为buyercode,sellercode 赋值
    $('#buyercode').val(buyercode);
    $('#sellercode').val(sellercode);
    //为模板编号赋值
    $('#templateno').val(templateno);
    //说明是新建合同
    if (isnew == "true") {
        $('#contractNo').textbox('setValue', '自动编号');
        $('#contractNo').textbox('readonly', true);
        //$('#language').combobox('setValue', language);
    }
    //说明为创建框架合同
    if (isFrame=="true") {
        $("input[name='frameContract'][value='是']").attr("checked", true);
    }
    else {
        $("input[name='frameContract'][value='否']").attr("checked", true);
    }
    $('#language').textbox('setValue', htdata.language);
    //业务员编号绑定
    var salesCode = $("#salesSpan").text();

    $("#salesman").val(salesCode);
    //状态绑定
    $("#status").textbox('setValue', '新建');
    //默认签订地点
    $("#signedplace").textbox('setValue', '乌鲁木齐');
    //产品大类绑定
    $("#product").combobox({
        required: true,
        valueField: 'productcategory',
        textField: 'productcategory',
        editable: false,
        data: sbCategory
    });
    //发运条款绑定
    $('#shipment').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbShipment
    });
    $('#shipment').combobox('setValue', htdata.shipment);
    $('#transport').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: comTrans
    });
    $('#transport').combobox('setValue', htdata.transport);

    $('#currency').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        multiple: false,
        data: comCurrey
    });
    $('#currency').combobox('setValue', htdata.currency);



    $('#businessclass').textbox('setValue', '西出组');
    $('#tradement').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbTradement
    });
    $('#tradement').combobox('setValue', htdata.tradement);
    $('#validity').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbValidity
    });
    $('#validity').combobox('setValue', htdata.validity);



    $('#pricement1').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbJgtk1
    });
    $('#pricement1').textbox('setValue', htdata.pricement1);
    $('#pricement2').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbJgtk2
    });
    $('#pricement2').combobox('setValue', htdata.pricement2);

    $('#seller').combogrid({
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
            $('#sellercode').attr('value', rowdata.code);
            $('#selleraddress').textbox('setValue', rowdata.address);
        }
    });
    $("#seller").combogrid("setValue", htdata.seller);
    $('#sellername').attr('value', sellercode);
    $('#buyer').combogrid({
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
            $('#buyeraddress').textbox('setValue', rowdata.address);
            $('#buyercode').attr('value', rowdata.code);
        }
    });
    $("#buyer").combogrid("setValue", htdata.buyer);
    $('#buyername').attr('value', buyercode);

    console.time('fff');
    //模板编号列表
    $('#templatename').combogrid({
        panelWidth: 100,

        //value:htdata.seller,
        idField: 'templatename',
        textField: 'templatename',
        data: comMbGridData,
        url: '',
        editable: false,
        columns: [[

         { field: 'templatename', title: '模板名称', width: 100 },
        ]],
        onSelect: function (index, rowdata) {
            $('#templateno').val(rowdata.templateno);

        }
    });
    console.timeEnd('fff');
    console.time('eee');
    $('#harborout').combogrid({
        panelWidth: 450,
        idField: 'code',
        textField: 'name',
        data: comPortGrid,
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
        idField: 'code',
        textField: 'name',
        data: comPortGrid,
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
        textField: 'name',
        data: comPortGrid,
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
    console.timeEnd('eee');
    $('#harborclear').combogrid('setValue', htdata.harborclear);
    $('#harborarrive').combogrid('setValue', htdata.harborarrive);
    $('#harborclear').combogrid('setValue', htdata.harborclear);


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

//根据选择发货的数字加载产品子表
function loadProuductByNumber(number, pname, pcode, contractNo) {

    if (number == 1) {
        return;
    }

    $.post("/ashx/Contract/contractOperater.ashx?module=addReviewFirst", { number: number, pname: pname, pcode: pcode, contractNo: contractNo }, function (data) {

        if (data.sucess == "1") {
            var editRow = undefined;//先定义一个变量，

            $('#splitCplist').css('display', "block");
            $('#splitCplist').datagrid({
                url: '/ashx/Contract/contractData.ashx?module=loadReviewData&contractNo=' + contractNo + '&pname=' + pname + '&pcode=' + pcode,
                rownumbers: true,
                singleSelect: true,
                sortName: 'pcode',
                sortOrder: 'asc',
                columns: [[
                     {
                         field: 'pp', title: '批次', width: 100, formatter: function (value, row, index) {
                             var index1 = parseInt(index + 1);
                             return "第" + index1 + "批";
                         }
                     },
                            { field: 'pcode', title: 'SAP编号', width: 100 },
                            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                            { field: 'amount', title: '数量', width: 100, editor: { type: 'numberbox' } },
                            {
                                field: 'unit', title: '单位', width: 100, formatter: function (value, row, index) {

                                    return "KG";
                                }
                            },

                ]],
                onDblClickRow: function (rowIndex, rowData) {
                    $("#splitCplist").datagrid('beginEdit', rowIndex);
                },

            });

        }


    });



}
console.timeEnd('初始化');
//为radio 绑定事件

function selectRadio() {
    $('#stockDiv').panel('close')
    $(":radio").click(function () {
        if ($("input[name='selectDelivery']:checked").val() == "工厂") {
            $('#stockDiv').panel('close');
        }
        if ($("input[name='selectDelivery']:checked").val() == "仓库") {
            //$("#factoryDiv").css("display", "block");
            //$("#stockDiv").css("display", "block");
            $('#stockDiv').panel('open');
        }

    })
}

//根据userName选择是否加载选择发货地

function loadDelivery() {
    var username = sbUserName;
    if (username == "管理员") {
        $('#addDeliveryDiv').panel('open');
    }

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

// 价格条款1：默认电汇，100%；默认价格条款2不可编辑，只有价格条款1小于100%时可编辑；价格条款1为必填，如不足100%，价格条款2比调
function priceTermsChange() {
    //说明为新增
    if (htdata.pricement1 == "") {
        $("#pricement1").combobox('setValue', '电汇');
        $('#pricement1per').numberbox('setValue', '100%');
        $('#pricement2').combobox('disable', true);
        $('#pricement2per').combobox('disable', true);
    }

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

