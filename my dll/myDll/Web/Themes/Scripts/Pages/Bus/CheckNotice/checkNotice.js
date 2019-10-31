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
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {


    }
    else if (pageTag == "form") {

        if (isBrowse == "True") {

            $('input').attr("readonly", true);

        }
        bindUI();
        //判断是否为客户订舱
        //isCustomerBooking();
        //填充数据
        $('#maintable').form('load', htdata);
        //添加产品列表
        addProductlist();


    }
});
//绑定数据
function bindUI() {


    //绑定合同号
    $('#saleContract').combobox({
        required: true,
        valueField: 'contractNo',
        textField: 'contractNo',
        editable: false,
        data: sbContractNo,
        onSelect: function (index, rowdata) {
            var contractno = $('#saleContract').combobox('getValue');
            //根据选择的合同号加载买方和卖方和货物名称
            $.post("/ashx/TrainCheckOut/trainCheckOperator.ashx?module=getPeople", { contractno: contractno }, function (data) {


                $("#seller").textbox('setValue', data[0].seller);
                $("#buyer").textbox('setValue', data[0].buyer);
                $("#productname").textbox('setValue', data[0].pname);
                $("#packages").textbox('setValue', data[0].quantity);
            }, 'json')
        }
    });
    $('#saleContract').combobox('setValue', htdata.saleContract);
    //为radio绑定单击事件
    $(":radio").click(function () {
        //选择作为发货人
        if ($("input[name='isConsignor']:checked").val() == "1") {
            $("#Consignor").combobox('setValue', $('#seller').textbox('getValue'));
        }
        if ($("input[name='isConsignor']:checked").val() == "0") {
            $("#Consignor").combobox('setValue', "");
        }
        //选择作为收货人
        if ($("input[name='isConsignee']:checked").val() == "1") {
            $("#Consignee").combobox('setValue', $('#buyer').textbox('getValue'));
        }
        if ($("input[name='isConsignee']:checked").val() == "0") {
            $("#Consignee").combobox('setValue', "");
        }

    })
    //绑定业务组
    $('#businessGroup').combobox({
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        data: sbGroup
    });
    $('#businessGroup').combobox('setValue', htdata.businessGroup);
    //绑定发货人，收货人
    $('#Consignor').combobox({
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        data: comSeller
    });
    $('#Consignor').combobox('setValue', htdata.businessGroup);

    $('#Consignee').combobox({
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        data: comBuyer
    });
    $('#Consignee').combobox('setValue', htdata.businessGroup);
    //绑定集装箱
    $('#containerCount').combobox({
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        data: sbContainer
    });

}
//查询数据
function doSearch() {

    var queryData = {};
    queryData.contractNo = $('#contractNo').textbox('getValue');
    queryData.productname = $('#productname').textbox('getValue');
    queryData.buyer = $('#buyer').textbox('getValue');
    $('#maingrid').datagrid({ queryParams: queryData });
}
//添加
function add() {
    window.top.addNewTab("海运发货出库指令-新增", '/Bus/CheckOutNotice/checkNoticeForm.aspx?action=add&isBooking=' + isBooking, '');
}
//修改
function edit() {
    var no = '';
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.checkId;
    }

    window.top.addNewTab("海运发货出库指令-修改", '/Bus/CheckOutNotice/checkNoticeForm.aspx?action=edit&checkId=' + no, '');
}

//保存
function save() {
    var rrdata = SaveDataToDB();
    if (rrdata == "ok") {
        $.messager.alert("提醒", "保存成功");
        window.top.selectTab("海运发货出库指令");
    }
    else {
        $.messager.alert("提醒", rrdata.err);
    }

}
//保存方法
var SaveDataToDB = function () {
    var shipProduct = $("#productname").datagrid("getRows");
    var shipProductJson = JSON.stringify(shipProduct);

    $('#shipProduct').attr('value', shipProductJson);
    var retdata = {};
    var action = "";
 
    


        if (isnew == "true") {
            action = "addCheckNotice";
        }
        else {
            action = "editCheckNotice";
        }




    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/CheckNotice/CheckNoticeOperator.ashx?module=' + action + '&checkId=' + checkCode,
        data: $('#form1').serialize(), // 你的formid

        async: false,
        error: function (data) {
            retdata = data;
        },
        success: function (data) {

            retdata = data;
        }
    });
    return retdata;
}

//删除
function del() {
    var no = '';
    var row = $("#maingrid").datagrid('getSelected');
    if (row == undefined) {
        $.messager.alert("提醒", "请至少选择一条数据");
        return;
    }
    no = row.checkId;
    $.messager.confirm('系统提示', '确认要删除么？', function (r) {
        if (r) {
            $.ajax({
                cache: true,
                type: "POST",
                url: '/ashx/CheckNotice/CheckNoticeOperator.ashx?module=delCheckNotice&checkId=' + no,
                data: {},
                async: false,
                error: function (data) {
                    $.messager.alert('系统提示', '后台操作失败!', 'info');
                },
                success: function (data) {
                    if (data == "ok") {
                        //$.messager.alert('系统提示', '删除成功!', 'info');
                        $("#maingrid").datagrid("reload", {});
                    }
                    else {
                        $.messager.alert('系统提示', '删除失败!' + data.errormsg, 'info');
                    }
                }
            });
        }
    });

}
//查看
function browse() {
    var no = '';
    var row = $("#maingrid").datagrid('getSelected');
    no = row.checkId;
    if (row != undefined) {
        window.top.addNewTab("海运发货出库指令-查看", '/Bus/CheckOutNotice/checkNoticeForm.aspx?action=view&checkId=' + no, '');
    }
}

//取消
var undo = function () {

    //关闭当前tab
    window.top.closeTab();
}

//出库
var out = function () {
    var no = '';
    var row = $("#maingrid").datagrid('getSelected');

    if (row != undefined) {
        no = row.checkId;
        window.top.addNewTab("产品出库单-新增", '/Bus/StockManage/StockOutAdd.aspx?checkId=' + no, '');
    }

    else {
        $.messager.alert("提醒", "请选择一条数据");
    }
}
//判断是否为客户订舱
function isCustomerBooking() {
    if (isBooking == "true") {
        //隐藏客户信息
        $("#contractfield").css("display", "none");

    }
}

//添加产品列表
function addProductlist() {
    var editRow = undefined;//先定义一个变量，
    $('#productname').datagrid({
        url: '/ashx/CheckNotice/CheckNoticeData.ashx?module=shipproduct&checkId=' + checkCode,
        row: 10,
        page:1,
        singleSelect: true,
        pagination: false,
        sortName: 'productName',
        sortOrder: 'asc',
        columns: [[
                    { field: 'productName', title: '货物名称', width: 150, editor: { type: 'textbox' } },
                    { field: 'packages', title: '件数', width: 150, editor: { type: 'textbox' } },
                    { field: 'netWeight', title: '净重', width: 150, editor: { type: 'textbox' } },
                    { field: 'fullWeight', title: '毛重', width: 150, editor: { type: 'textbox' }, align: 'center' },
                    { field: 'mass', title: '体积', width: 150, editor: { type: 'textbox' }, align: 'center' },


        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {

                if (editRow != undefined) {
                    //说明不是第一次添加
                    $("#productname").datagrid('endEdit', editRow);
                    editRow = undefined;

                }
                if (editRow == undefined) {
                    //说明是第一次添加，插入一个空行，让eidtRow=0；开始第一行的编辑，当再次添加的时候，由于editrow不为undefined，第一行就会结束编辑，再把editRow赋值为undefined，这样就会再次插入一行。
                    $("#productname").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });
                    $("#productname").datagrid('beginEdit', 0);
                    editRow = 0;
                }

            }
        }, '-', {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#productname").datagrid('getSelected');

                if (row != null) {
                    //如果editRow不为undefined，说明正在添加，结束添加。
                    if (editRow != undefined) {
                        $("#productname").datagrid('endEdit', editRow);
                        editRow = undefined
                    }
                    //添加完成，获取选中的index,进行编辑，取消所有的选中。
                    if (editRow == undefined) {
                        var index = $("#productname").datagrid('getRowIndex', row);
                        $("#productname").datagrid('beginEdit', index);
                        editRow = index;
                        $("#productname").datagrid('unselectAll');
                    }
                }

            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var row = $("#productname").datagrid('getSelected');
                var index = $("#productname").datagrid('getRowIndex', row);

                if (index != null) {
                    $('#productname').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);


                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '保存',
            handler: function () {
                $('#productname').datagrid('acceptChanges');
            }
        }],

    });
}