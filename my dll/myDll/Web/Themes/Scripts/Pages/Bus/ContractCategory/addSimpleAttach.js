$(function () {
    //tabs选中事件
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
            var buyer = $("#buyer").combobox('getValue');
            var buyername = $("#buyername").val();
            var seller = $("#seller").combobox('getValue');
            var sellername = $("#sellername").val();
            var signedTime = $("#signedtime").datebox('getValue');
            var signedplace = $("#signedplace").textbox('getValue');
            var item1 = $("#item1").textbox('getValue');
            var item2 = $("#item2").textbox('getValue');
            var item3 = $("#item3").textbox('getValue');
            var item4 = $("#item4").textbox('getValue');
            var item5 = $("#item5").textbox('getValue');
            var language = $("#language").textbox('getValue');
            if (title == "查看合同文本") {
                $('#htcplist').datagrid('acceptChanges');
                //获取list数据到htcplistStr
                var cplist = $("#htcplist").datagrid("getRows");
                var datagridjson = JSON.stringify(cplist);

                $("#previewFormFrame").attr('src', '/Bus/Contract/priviewTemplate.aspx?buyer=' + buyer + '&buyername=' + buyername + '&seller=' + seller + '&sellername=' + sellername + '&signedTime=' + signedTime + '&signedplace=' + signedplace + '&productlist=' + datagridjson +
                '&item1=' + item1 + '&item2=' + item2 + '&item3=' + item3 + '&item4=' + item4 + '&item5=' + item5 + '&language=' + language);
                $(":checkbox").click(function () {
                    var str = "";
                    $('input[name="checkbox"]:checked').each(function () {
                        str += $(this).val() + ",";
                    });

                    $("#previewFormFrame").attr('src', '/Bus/Contract/priviewTemplate.aspx?buyer=' + buyer + '&buyername=' + buyername + '&seller=' + seller + '&sellername=' + sellername + '&signedTime=' + signedTime + '&signedplace=' + signedplace + '&productlist=' + datagridjson +
                 '&item1=' + item1 + '&item2=' + item2 + '&item3=' + item3 + '&item4=' + item4 + '&item5=' + item5 + '&language=' + language + '&moreLanguage=' + str);

                })

            }
        }
    })
    //加载合同附件已有产品列表
    loadProduct();
    //绑定数据
    bindUI();
    //选择产品
    selectProduct();
});
//绑定数据
var bindUI = function () {
    $('#maintable').form('load', htdata);
    //当新建或者复制合同附件时，编号为自动编号
    if (isnew == "true") {
        $('#contractNo').textbox('setValue', '自动编号');
    }
    //为隐藏域关联合同号，业务流向赋值
    $("#purchaseCode").val(contactCode);
    $("#flowdirection").val(flowdirection);
    //默认签订地点
    $("#signedplace").textbox('setValue', '乌鲁木齐');
    //设置语言为不可编辑
    $('#language').textbox('textbox').attr('readonly', true);
    //卖方(简称)文本框改变事件
    $("input", $("#simpleseller").next("span")).blur(function () {
        var simpleSeller = ($("#simpleseller").val());
        $.post("/ashx/Contract/loadPeople.ashx", { simpleSeller: simpleSeller }, function (data) {

            $("#seller").combobox('setValue', data.name);
            $("#selleraddress").textbox('setValue', data.address);
            $('#sellername').attr('value', data.code);

        }, 'json')

    })
    //买方(简称)文本框改变事件
    $("input", $("#simplebuyer").next("span")).blur(function () {

        var simpleBuyer = ($("#simplebuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx", { simpleBuyer: simpleBuyer }, function (data) {

            $("#buyer").combobox('setValue', data.name);
            $("#buyeraddress").textbox('setValue', data.address);
            $('#buyername').attr('value', data.code);
        }, 'json')

    })
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
            $('#sellername').attr('value', rowdata.code);
            $('#selleraddress').textbox('setValue', rowdata.address);
        }
    });
    $("#seller").combogrid("setValue", htdata.seller);
    $('#sellername').attr('value', htdata.seller);
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
            $('#buyername').attr('value', rowdata.code);
        }
    });
    $("#buyer").combogrid("setValue", htdata.buyer);
    $('#buyername').attr('value', htdata.buyer);
    //产品大类绑定
    $("#product").combobox({
        required: true,
        valueField: 'productcategory',
        textField: 'productcategory',
        editable: false,
        data: sbCategory
    });
    $("#product").combobox("setValue", htdata.product);
    //货币绑定
    $('#currency').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        multiple: false,
        data: comCurrey
    });
    $('#currency').combobox('setValue', htdata.currency);


}
var SaveDataToDB = function (status1) {
    if (endEditing() != true) {
        return;
    }

    var retdata = {};
    var status = "";
    //获取list数据到htcplistStr
    var cplist = $("#htcplist").datagrid("getRows");
    var datagridjson = JSON.stringify(cplist);
    $('#htcplistStr').attr('value', datagridjson);
    var action = "addAttachContract";
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/Contract/contractOperater.ashx?module=' + action + "&MaincontractNo=" + MainContractCode + "&status=" + status1,
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
};
//返回
var back = function () {

    window.top.closeAddTab("合同-新增", "/Bus/Contract/ContractTest.aspx");
}
//保存
var save = function () {
    var rrdata = SaveDataToDB(0);
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        $.messager.alert('系统提示', '保存成功!', 'info', function () {
            //打开指定模板页面
            window.top.selectTab('合同管理');
        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
};
//提交
var submit = function () {
    var rrdata = SaveDataToDB(1);
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        $.messager.alert('系统提示', '保存成功!', 'info', function () {
            //打开指定模板页面
            window.top.selectTab('合同管理');
        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
}
//取消
var undo = function () {
    window.top.closeTab();
}
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

//返回填写信息
function backWrite() {

    $('#tt').tabs("select", "基本信息");

}
//选择模板条款
function selectTemplate() {
    //$("#templateTable").datagrid('reload');
    var language = $("#language").textbox('setValue');
    $('#templateTable').datagrid({
        nowrap: false,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: false,
        autoRowHeight: true,
        pagination: true,
        idField: 'sortno',
        url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getdetail&no=' + templateno,
        columns: [[
          { field: 'ck', checkbox: true },
         { field: 'sortno', title: '序号', width: '50px' },

        { field: 'chncontent', title: '中文', width: '300px', editor: 'textarea' },
        { field: 'engcontent', title: '英文', width: '300px', editor: 'textarea' },
        { field: 'ruscontent', title: '俄文', width: '300px', editor: 'textarea' }
        ]],
        toolbar: [{
            text: '选择',
            iconCls: 'icon-add',
            handler: function () {

                var row = $("#templateTable").datagrid('getSelections');

                if (row.length > 5) {
                    $.messager.alert("提醒", "选择条款数超出限制");
                    return;
                }
                $('#templateDiv').dialog('close');
                $("#language").textbox('setValue', htdata.language);
                $("#product").textbox('setValue', htdata.product);
                //为条款赋值
                var splitJson = JSON.stringify(row);
                //json字符串转化为数组
                var b = eval("(" + splitJson + ")");

                //为条款赋值
                $("#item1").textbox('setValue', "");
                $("#item1foreign").textbox('setValue', "");
                $("#item2").textbox('setValue', "");
                $("#item2foreign").textbox('setValue', "");
                $("#item3").textbox('setValue', "");
                $("#item3foreign").textbox('setValue', "");
                $("#item4").textbox('setValue', "");
                $("#item4foreign").textbox('setValue', "");
                $("#item5").textbox('setValue', "");
                $("#item5foreign").textbox('setValue', "");

                if (htdata.language == "中文") {
                    $("#item1").textbox('setValue', b[0].chncontent);

                    $("#item2").textbox('setValue', b[1].chncontent);

                    $("#item3").textbox('setValue', b[2].chncontent);

                    $("#item4").textbox('setValue', b[3].chncontent);

                    $("#item5").textbox('setValue', b[4].chncontent);

                }
                if (htdata.language == "中文-英文") {
                    $("#item1").textbox('setValue', b[0].chncontent);
                    $("#item1foreign").textbox('setValue', b[0].engcontent);
                    $("#item2").textbox('setValue', b[1].chncontent);
                    $("#item2foreign").textbox('setValue', b[1].engcontent);
                    $("#item3").textbox('setValue', b[2].chncontent);
                    $("#item3foreign").textbox('setValue', b[2].engcontent);
                    $("#item4").textbox('setValue', b[3].chncontent);
                    $("#item4foreign").textbox('setValue', b[3].engcontent);
                    $("#item5").textbox('setValue', b[4].chncontent);
                    $("#item5foreign").textbox('setValue', b[4].engcontent);
                }
                if (htdata.language == "中文-俄文") {
                    $("#item1").textbox('setValue', b[0].chncontent);
                    $("#item1foreign").textbox('setValue', b[0].ruscontent);
                    $("#item2").textbox('setValue', b[1].chncontent);
                    $("#item2foreign").textbox('setValue', b[1].ruscontent);
                    $("#item3").textbox('setValue', b[2].chncontent);
                    $("#item3foreign").textbox('setValue', b[2].ruscontent);
                    $("#item4").textbox('setValue', b[3].chncontent);
                    $("#item4foreign").textbox('setValue', b[3].ruscontent);
                    $("#item5").textbox('setValue', b[4].chncontent);
                    $("#item5foreign").textbox('setValue', b[4].ruscontent);
                }



            }
        }
        ]



    });
    $("#templateDiv").dialog({
        title: "添加",
        width: 600,
        height: 500,
        closed: true,
        cache: false,
        modal: true,
        maximizable: true,
        collapsible: true,
    });
    $('#templateDiv').dialog('open');

}
//加载现有产品列表
function loadProduct() {
    $('#htcplist').datagrid({
        url: '/Bus/Xctl/ajax/AbroadLoadData.ashx?module=htcppagelist&contractNo=' + contractNo + '&isall=1',
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
         {
             field: 'priceUnit', title: '价格单位', width: 100, editor: { type: 'textbox' }, formatter: function (rowindex, data) {
                 var currency = $("#currency").combobox('getValue');
                 return currency;
             }
         },
          { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
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
        onClickCell: function (index, field, value) {
            if (editIndex != index) {
                var mygrid = $('#htcplist');
                if (endEditing()) {
                    $(this).datagrid('beginEdit', index);
                    //                            var ed = $(this).datagrid('getEditor', {index:index,field:field});
                    //                            if($(ed.target) != undefined){
                    //		                        $(ed.target).focus();
                    //                            }
                    editIndex = index;
                } else {
                    setTimeout(function () {
                        mygrid.datagrid('selectRow', editIndex);
                    }, 0);
                }
            }
        }
    });
}
//选择产品
function selectProduct() {
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
                for (var i = 0; i < rows.length; i++) {
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
}
