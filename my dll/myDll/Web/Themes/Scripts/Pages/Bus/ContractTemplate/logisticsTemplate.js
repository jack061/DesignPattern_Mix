$(function () {
    bindUI();
    selectTabs();
    loadTotalName();
    addTemplateTable();
})
//根据item 动态生成模板表格
function addTemplateTable() {
    $("#templateItemDiv").css('display', 'block');
    var item1 = $("#item1").textbox('getValue');
    var item2 = $("#item2").textbox('getValue');
    var item3 = $("#item3").textbox('getValue');
    var item4 = $("#item4").textbox('getValue');
    var item5 = $("#item5").textbox('getValue');
    var item6 = $("#item6").textbox('getValue');
    var item7 = $("#item7").textbox('getValue');
    var item8 = $("#item8").textbox('getValue');
    var allItems = item1 + "|" + item2 + "|" + item3 + "|" + item4 + "|" + item5 + "|" + item6 + "|" + item7 + "|" + item8;
    var editRow = undefined;//先定义一个变量
    var toolbar = [{
        text: '添加',
        iconCls: 'icon-add',
        handler: function () {
            if (editRow != undefined) {

                $("#templateItem").datagrid('endEdit', editRow);
                editRow = undefined;

            }
            if (editRow == undefined) {

                $("#templateItem").datagrid('insertRow', {
                    index: 0,
                    row: {}
                });
                $("#templateItem").datagrid('beginEdit', 0);
                editRow = 0;
            }
        }
    }, {
        text: '编辑',
        iconCls: 'icon-edit',
        handler: function () {
            var row = $("#templateItem").datagrid('getSelected');
            if (row != null) {
                if (editRow != undefined) {
                    $("#templateItem").datagrid('endEdit', editRow);
                    editRow = undefined;
                }

                if (editRow == undefined) {
                    var index = $("#templateItem").datagrid('getRowIndex', row);
                    $("#templateItem").datagrid('beginEdit', index);
                    editRow = index;
                    $("#templateItem").datagrid('unselectAll');
                }
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }

        }
    }, {
        text: '删除',
        iconCls: 'icon-remove',
        handler: function () {
            var row = $("#templateItem").datagrid('getSelected');
            var index = $("#templateItem").datagrid('getRowIndex', row);

            if (index != null) {
                $('#templateItem').datagrid('cancelEdit', index)
                .datagrid('deleteRow', index);


            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }];

    $('#templateItem').datagrid({
        url: '/ashx/Contract/templateContractData.ashx?module=logisticsItems&logisticsTemplateno=' + htdata.logisticsTemplateno,
        singleSelect: true,
        pagination: false,
        sortName: 'id',
        toolbar: toolbar,
        sortOrder: 'asc',
        columns: [[
		            { field: 'item1', title: item1, width: 150, editor: 'textbox' },
		            { field: 'item2', title: item2, width: 150, editor: 'textbox' },
                    { field: 'item3', title: item3, width: 150, editor: 'textbox' },
                    { field: 'item4', title: item4, width: 150, editor: 'textbox' },
                     { field: 'item5', title: item5, width: 150, editor: 'textbox' },
		            { field: 'item6', title: item6, width: 150, editor: 'textbox' },
                    { field: 'item7', title: item7, width: 150, editor: 'textbox' },
                    { field: 'item8', title: item8, width: 150, editor: 'textbox' }
        ]],
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#templateItem").datagrid('endEdit', editRow);
                editRow = undefined;
            }

            if (editRow == undefined) {
                $("#templateItem").datagrid('beginEdit', rowIndex);
                editRow = rowIndex;
            }
        },


    });


    //根据选择的item动态隐藏列
    hideColumnByItem(item1, item2, item3, item4, item5, item6, item7, item8);

}
//根据选择的item动态隐藏列
function hideColumnByItem(item1, item2, item3, item4, item5, item6, item7, item8) {
    if (item1 == "") {
        $('#templateItem').datagrid('hideColumn', 'item1');
    }
    if (item2 == "") {
        $('#templateItem').datagrid('hideColumn', 'item2');
    }
    if (item3 == "") {
        $('#templateItem').datagrid('hideColumn', 'item3');
    }
    if (item4 == "") {
        $('#templateItem').datagrid('hideColumn', 'item4');
    }
    if (item5 == "") {
        $('#templateItem').datagrid('hideColumn', 'item5');
    }
    if (item6 == "") {
        $('#templateItem').datagrid('hideColumn', 'item6');
    }
    if (item7 == "") {
        $('#templateItem').datagrid('hideColumn', 'item7');
    }
    if (item8 == "") {
        $('#templateItem').datagrid('hideColumn', 'item8');
    }
}
//修改时填充条款表格
function loadTemplateItesByEdit() {
    $("#templateItemDiv").css('display', 'block');
    var item1 = htdata.item1;
    var item2 = htdata.item2;
    var item3 = htdata.item3;
    var item4 = htdata.item4;
    var item5 = htdata.item5;
    var item6 = htdata.item6;
    var item7 = htdata.item7;
    var item8 = htdata.item8;
    var editRow = undefined;//先定义一个变量
    var toolbar = [{
        text: '添加',
        iconCls: 'icon-add',
        handler: function () {
            if (editRow != undefined) {

                $("#templateItem").datagrid('endEdit', editRow);
                editRow = undefined;

            }
            if (editRow == undefined) {

                $("#templateItem").datagrid('insertRow', {
                    index: 0,
                    row: {}
                });
                $("#templateItem").datagrid('beginEdit', 0);
                editRow = 0;
            }
        }
    }, {
        text: '编辑',
        iconCls: 'icon-edit',
        handler: function () {
            var row = $("#templateItem").datagrid('getSelected');
            if (row != null) {
                if (editRow != undefined) {
                    $("#templateItem").datagrid('endEdit', editRow);
                    editRow = undefined;
                }

                if (editRow == undefined) {
                    var index = $("#templateItem").datagrid('getRowIndex', row);
                    $("#templateItem").datagrid('beginEdit', index);
                    editRow = index;
                    $("#templateItem").datagrid('unselectAll');
                }
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }

        }
    }, {
        text: '删除',
        iconCls: 'icon-remove',
        handler: function () {
            var row = $("#templateItem").datagrid('getSelected');
            var index = $("#templateItem").datagrid('getRowIndex', row);

            if (index != null) {
                $('#templateItem').datagrid('cancelEdit', index)
                .datagrid('deleteRow', index);


            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }];
    $('#templateItem').datagrid({
        url: '/ashx/Contract/templateContractData.ashx?module=logisticsItems&logisticsTemplateno=' + htdata.logisticsTemplateno,
        singleSelect: true,
        pagination: false,
        sortName: 'id',
        toolbar: toolbar,
        sortOrder: 'asc',
        columns: [[
		            { field: 'item1', title: item1, width: 150, editor: { type: 'textbox' } },
		            { field: 'item2', title: item2, width: 150, editor: { type: 'textbox' } },
                    { field: 'item3', title: item3, width: 150, editor: { type: 'textbox' } },
                    { field: 'item4', title: item4, width: 150, editor: { type: 'textbox' } },
                     { field: 'item5', title: item5, width: 150, editor: { type: 'textbox' } },
		            { field: 'item6', title: item6, width: 150, editor: { type: 'textbox' } },
                    { field: 'item7', title: item7, width: 150, editor: { type: 'textbox' } },
                    { field: 'item8', title: item8, width: 150, editor: { type: 'textbox' } }
        ]],
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#templateItem").datagrid('endEdit', editRow);
                editRow = undefined;
            }

            if (editRow == undefined) {
                $("#templateItem").datagrid('beginEdit', rowIndex);
                editRow = rowIndex;
            }
        },


    });

    //根据选择的item动态隐藏列
    hideColumnByItem(item1, item2, item3, item4, item5, item6, item7, item8);

}
//绑定数据
function bindUI() {

    //绑定买方卖方
    $('#seller').combogrid({
        panelWidth: 450,
        //value:htdata.seller,
        idField: 'name',
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
            $("#sellercode").val(rowdata.code);
            $("#simpleSeller").textbox('setValue',rowdata.shortname);
        }
    });
    $('#buyer').combogrid({
        panelWidth: 450,
        //value:htdata.buyer,
        idField: 'name',
        textField: 'name',
        data: comBuyer,
        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#buyercode").val(rowdata.code);
            $("#simpleBuyer").textbox('setValue', rowdata.shortname);
        }
    });
    //为table填充数据
    $('#Table1').form('load', htdata);
    $('#templateTextTable').form('load', htdata);
    $('#tableNameTable').form('load', htdata);
    $("#buyercode").val(htdata.buyerCode);
    $("#sellercode").val(htdata.sellerCode);
    //如果为修改，则填充条款表中的数据
    //说明是修改
    if (htdata.logisticsTemplateno != "") {

        loadTemplateItesByEdit();
    }
    //如果为查看，则不可编辑
    if (isBrowse == "true") {
        $('input').attr("readonly", true);

        $("#contractText").attr("readonly", true);
        //隐藏保存取消div
        $("#btabs").css('display', 'none');
    }

}

//保存数据
function save() {
    //var obj = $('#templateItem').datagrid('options').columns[0];
    ////alert(obj[0].width);

    var rrdata = SaveDataToDB(0);
    if (rrdata != undefined && rrdata.sucess == '1') {

        window.top.selectAndRefreshTab('管理合同模板');

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

var SaveDataToDB = function (status) {

    //为动态表格变量转换json
    $("#templateItem").datagrid('acceptChanges');
    var Item = $("#templateItem").datagrid('getRows');
    var logisticsItem = JSON.stringify(Item);
    $('#logisticsItem').attr('value', logisticsItem);
    var redata = "";
    $('#htmlcontent').attr('value', editor123.html());//获取条款文本
   
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/Contract/templateContractOperator.ashx?module=addLogisticsTemplate&logisticsTemplateno=' + htdata.logisticsTemplateno,
        data: $('#form1').serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {
            retdata.errdata = data;
        },
        success: function (data) {
            retdata = data;

        }
    });
    return retdata;
}

//tabs选择事件
function selectTabs() {
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
            $('#htmlcontent').attr('value', editor123.html());//获取条款文本
            var signedTime = $("#signedTime").datebox('getValue');
            var signedplace = $("#signedPlace").textbox('getValue');
            var buyercode = $("#buyercode").val();
            var sellercode = $("#sellercode").val();
            var contractText = $("#htmlcontent").val();
            var item1 = $("#item1").textbox('getValue');
            var item2 = $("#item2").textbox('getValue');
            var item3 = $("#item3").textbox('getValue');
            var item4 = $("#item4").textbox('getValue');
            var item5 = $("#item5").textbox('getValue');
            var item6 = $("#item6").textbox('getValue');
            var item7 = $("#item7").textbox('getValue');
            var item8 = $("#item8").textbox('getValue');
            var item1width = $("#item1width").numberbox('getValue');
            var item2width = $("#item2width").numberbox('getValue');
            var item3width = $("#item3width").numberbox('getValue');
            var item4width = $("#item4width").numberbox('getValue');
            var item5width = $("#item5width").numberbox('getValue');
            var item6width = $("#item6width").numberbox('getValue');
            var item7width = $("#item7width").numberbox('getValue');
            var item8width = $("#item8width").numberbox('getValue');
            var allItems = item1 + "|" + item2 + "|" + item3 + "|" + item4 + "|" + item5 + "|" + item6 + "|" + item7 + "|" + item8;
            var allItemswidth = item1width + "|" + item2width + "|" + item3width + "|" + item4width + "|" + item5width + "|" + item6width + "|" + item7width + "|" + item8width;
            if (title == "模板预览") {
                //获取模板条款列表
                $('#htmlcontent').attr('value', editor123.html());//获取条款文本
                $('#templateItem').datagrid('acceptChanges');
                var datagrid = $("#templateItem").datagrid("getRows");
                var templatejson = JSON.stringify(datagrid);
                var firstparty = $("#buyer").combobox('getValue');
                var secondparty = $("#seller").combobox('getValue');
                var templateName = $("#logisticsTemplateName").textbox('getValue');
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/templateContractData.ashx?module=GetLogisiticsTempldatePVC",
                    dataType: "text",
                    data: {
                        tempjson: templatejson, buyercode: buyercode, signedtime: signedTime,
                        signedplace: signedplace, language: "中文", sellercode: sellercode,
                        contractText: contractText, allItems: allItems, allItemswidth: allItemswidth, firstparty: firstparty, secondparty: secondparty,
                        templatename: templateName
                    },
                    success: function (data) {
                        $("#templatejson").val(data);
                    }
                });
                $("#previewFormFrame").attr('src', '/Bus/ContractTemplate/realTimePreviewLogisticsTemplate.aspx');
            }


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
            $('#sellercode').attr('value', data.code);

        }, 'json')
    })
    //买方(简称)文本框改变事件
    $("input", $("#simpleBuyer").next("span")).blur(function () {

        var simpleBuyer = ($("#simpleBuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
            $("#simpleBuyer").textbox('setValue', data.shortname);
            $("#buyer").combobox('setValue', data.name);

            $('#buyercode').attr('value', data.code);

        }, 'json')

    })

}

