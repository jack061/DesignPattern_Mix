
$(function () {

    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {

    }
    else if (pageTag == "form") {
        if (isBrowse == "true") {

            $('input').attr("readonly", true);

        }
        //填充数据
        $('#maintable').form('load', htdata);
        //combobox 绑定数据
        bindUI();
        //加载国境口岸站子表
        loadForntiercode();
        ////加载付费代码和过境付费代码子表
        loadPayandTransit();
        //加载货物详情子表
        loadProductlist();

    }
})
//加载货物详情子表
function loadProductlist() {

    var editRow = undefined;//先定义一个变量，
    $('#productlist').datagrid({
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=trainProduct&checkId=' + checkId,
        singleSelect: true,
        pagination: false,
        sortName: 'productName',
        sortOrder: 'asc',
        columns: [[
		            { field: 'productName', title: '货物名称', width: 150 },
		            { field: 'packagesType', title: '包装种类', width: 150 },
                    { field: 'packagesNumber', title: '件数', width: 150 },
                    { field: 'weight', title: '重量', width: 150, align: 'center' },


        ]],


    });
}
//加载国境口岸站子表
function loadForntiercode() {
    var editRow = undefined;//先定义一个变量，
    $('#frontierTd').datagrid({
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=checkFrontierStation&checkId=' + checkId,
        singleSelect: true,
        pagination: false,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'frontierStationCode', title: '代码', width: 150, editor: { type: 'textbox' } },
		            { field: 'frontierStation', title: '名称', width: 150, align: 'center', editor: { type: 'textbox' } },


        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {

                if (editRow != undefined) {
                    //说明不是第一次添加
                    $("#frontierTd").datagrid('endEdit', editRow);
                    editRow = undefined;

                }
                if (editRow == undefined) {
                    //说明是第一次添加，插入一个空行，让eidtRow=0；开始第一行的编辑，当再次添加的时候，由于editrow不为undefined，第一行就会结束编辑，再把editRow赋值为undefined，这样就会再次插入一行。
                    $("#frontierTd").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });
                    $("#frontierTd").datagrid('beginEdit', 0);
                    editRow = 0;
                }

            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var row = $("#frontierTd").datagrid('getSelected');
                var index = $("#frontierTd").datagrid('getRowIndex', row);

                if (index != null) {
                    $('#frontierTd').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);


                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '保存',
            handler: function () {
                $('#frontierTd').datagrid('acceptChanges');
            }
        }],
        onDblClickRow: function (rowIndex, rowData) {

            var row = $("#frontierTd").datagrid('getSelected');

            if (row != null) {
                //如果editRow不为undefined，说明正在添加，结束添加。
                if (editRow != undefined) {
                    $("#frontierTd").datagrid('endEdit', editRow);
                    editRow = undefined
                }
                //添加完成，获取选中的index,进行编辑，取消所有的选中。
                if (editRow == undefined) {
                    var index = $("#frontierTd").datagrid('getRowIndex', row);
                    $("#frontierTd").datagrid('beginEdit', index);
                    editRow = index;
                    $("#frontierTd").datagrid('unselectAll');
                }
            }
        }

    });
}
////加载付费代码和过境付费代码子表
function loadPayandTransit() {
    var editRow = undefined;//先定义一个变量，
    var containerSize = $("#containerSize").combobox('getValue');
    $('#paycodeandtracsit').datagrid({
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=checkPayCode&checkId=' + checkId,

        singleSelect: true,
        pagination: false,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'payCode', title: '付费代码', width: 100, editor: { type: 'textbox' } },
		            { field: 'transitPayCode', title: '过境代码', width: 100, align: 'center', editor: { type: 'textbox' } },
                    {
                        field: 'containerText', title: '集装箱规格', width: 100, align: 'center', editor: {type: 'textbox' }
                    },

                       {
                           field: 'containerWeight', title: '集装箱重量', width: 100, align: 'center', formatter: function (value, row, index) {
                               return "KG";
                           }
                       },
                          {
                              field: 'carriageNumber', title: '车厢号', width: 100, align: 'center', editor: { type: 'textbox' }
                          },
                           {
                               field: 'productWeight', title: '产品重量', width: 100, align: 'center', editor: { type: 'textbox' }
                           },




        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {

                if (editRow != undefined) {
                    //说明不是第一次添加
                    $("#paycodeandtracsit").datagrid('endEdit', editRow);
                    editRow = undefined;

                }
                if (editRow == undefined) {
                    //说明是第一次添加，插入一个空行，让eidtRow=0；开始第一行的编辑，当再次添加的时候，由于editrow不为undefined，第一行就会结束编辑，再把editRow赋值为undefined，这样就会再次插入一行。
                    $("#paycodeandtracsit").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });
                    $("#paycodeandtracsit").datagrid('beginEdit', 0);
                    editRow = 0;
                }

            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var row = $("#paycodeandtracsit").datagrid('getSelected');
                var index = $("#paycodeandtracsit").datagrid('getRowIndex', row);

                if (index != null) {
                    $('#paycodeandtracsit').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);


                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '保存',
            handler: function () {
                $('#paycodeandtracsit').datagrid('acceptChanges');
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '打印',
            handler: function () {
                var row = $("#paycodeandtracsit").datagrid('getSelected');
                if (row != undefined) {
                    payCode = row.payCode;
                    window.open('/TrainApply/TrainApplyEPreview.aspx?checkId=' + checkCode+'&payCode='+ payCode, "_blank", "");
                    } else {
                        $.messager.alert('提示', '请选择一行数据！', 'info');
                    }
            }

        }, '-', {
            iconCls: 'icon-save',
            text: '复制',
            handler: function () {
            
           
                var rows = $('#paycodeandtracsit').datagrid('getRows');
                $('#paycodeandtracsit').datagrid('acceptChanges');
                if (rows[0]["payCode"] == "") {
                    $.messager.alert("提醒", "请填写第一行数据");
                    return;
                }
                var paycode = rows[0]["payCode"];
                for (var i = 0; i < rows.length; i++) {
                    $('#paycodeandtracsit').datagrid('updateRow', {
                        index: i,
                        row: {
                            payCode:paycode,
                        }
                    });

                }
            }

        }, '-', {
            iconCls: 'icon-save',
            text: '自增',
            handler: function () {
                var rows = $('#paycodeandtracsit').datagrid('getRows');
                $('#paycodeandtracsit').datagrid('acceptChanges');
                if (rows[0]["payCode"] == "") {
                    $.messager.alert("提醒", "请填写第一行数据");
                    return;
                }
                var paycode =parseInt(rows[0]["payCode"]);

                for (var i = 1; i < rows.length; i++) {
                    var code = paycode + i;
                    $('#paycodeandtracsit').datagrid('updateRow', {
                        index: i,
                        row: {
                            payCode:code,
                        }
                    });

                }
            }

        }],
        onDblClickRow: function (rowIndex, rowData) {
            var row = $("#paycodeandtracsit").datagrid('getSelected');

            if (row != null) {
                //如果editRow不为undefined，说明正在添加，结束添加。
                if (editRow != undefined) {
                    $("#paycodeandtracsit").datagrid('endEdit', editRow);
                    editRow = undefined
                }
                //添加完成，获取选中的index,进行编辑，取消所有的选中。
                if (editRow == undefined) {
                    var index = $("#paycodeandtracsit").datagrid('getRowIndex', row);
                    $("#paycodeandtracsit").datagrid('beginEdit', index);
                    editRow = index;
                    $("#paycodeandtracsit").datagrid('unselectAll');
                }
            }
        }

    });
}
//加载付费代码和过境代码子表包含子表
function laodPayandTransitContain(url) {
    $('#paycodeandtracsit').datagrid({
        url: url,

        singleSelect: true,
        pagination: false,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'pcode', title: '付费代码', width: 100, editor: { type: 'textbox' } },
		            { field: 'pname', title: '过境代码', width: 100, align: 'center', editor: { type: 'textbox' } },
                    {
                        field: 'containerSize', title: '集装箱规格', width: 100, align: 'center', editor: { type: 'textbox' }, formatter: function (value, row, index) {

                        }
                    },


        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {

                if (editRow != undefined) {
                    //说明不是第一次添加
                    $("#paycodeandtracsit").datagrid('endEdit', editRow);
                    editRow = undefined;

                }
                if (editRow == undefined) {
                    //说明是第一次添加，插入一个空行，让eidtRow=0；开始第一行的编辑，当再次添加的时候，由于editrow不为undefined，第一行就会结束编辑，再把editRow赋值为undefined，这样就会再次插入一行。
                    $("#paycodeandtracsit").datagrid('insertRow', {
                        index: 0,
                        row: {payCode:"1"}
                    });
                    $("#paycodeandtracsit").datagrid('beginEdit', 0);
                    editRow = 0;
                }

            }
        }, '-', {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#paycodeandtracsit").datagrid('getSelected');

                if (row != null) {
                    //如果editRow不为undefined，说明正在添加，结束添加。
                    if (editRow != undefined) {
                        $("#paycodeandtracsit").datagrid('endEdit', editRow);
                        editRow = undefined
                    }
                    //添加完成，获取选中的index,进行编辑，取消所有的选中。
                    if (editRow == undefined) {
                        var index = $("#paycodeandtracsit").datagrid('getRowIndex', row);
                        $("#paycodeandtracsit").datagrid('beginEdit', index);
                        editRow = index;
                        $("#paycodeandtracsit").datagrid('unselectAll');
                    }
                }

            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var row = $("#paycodeandtracsit").datagrid('getSelected');
                var index = $("#paycodeandtracsit").datagrid('getRowIndex', row);

                if (index != null) {
                    $('#paycodeandtracsit').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);


                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }, '-', {
            iconCls: 'icon-save',
            text: '保存',
            handler: function () {
                $('#paycodeandtracsit').datagrid('acceptChanges');
            }
        }]

    });
}
//国境口岸站动态添加方法
function frontiercodeAdd() {
    var frontierCode = "frontierCode";
    var frontier = "frontier";
    $("#frontiercodeAdd").click(function () {
        var t1 = "<br/><br/> <input class='easyui-textbox' style='width: 25%' type='text' name='frontier' id='frontier' data-options='required:false' value=''></input>&nbsp;<input style='width: 50%' class='easyui-textbox' name='frontierCode' id='frontierCode' value=''></input>";
        $("#frontierTd").append(t1);
        //渲染之后重新加载
        var busniessvalue = $("#businessGroup").combobox('getValue');
        var saleContract = $("#saleContract").combobox('getValue');
        $.parser.parse();
        $("#businessGroup").combobox('setValue', busniessvalue);
        $("#saleContract").combobox('setValue', saleContract);
        //$('input').attr("readonly", true);
    });
}
//付费代码动态添加方法
function payCodeAdd() {

    $("#payCodeAdd").click(function () {

        var t1 = " <br/><br/><input class='easyui-textbox' style='width: 25%' type='text' name='payCode' id='payCode' data-options='required:false' value=''></input>";
        var t2 = " <br/><br/><input class='easyui-textbox' style='width: 25%' type='text' name='transitPayCode' id='transitPayCode' data-options='required:false' value=''></input>";
        $("#payCodeTd").append(t1);
        $("#transitPayCodeTd").append(t2);

        //渲染之后重新加载
        var busniessvalue = $("#businessGroup").combobox('getValue');
        var saleContract = $("#saleContract").combobox('getValue');
        $.parser.parse();
        $("#businessGroup").combobox('setValue', busniessvalue);
        $("#saleContract").combobox('setValue', saleContract);
        //$('input').attr("readonly", true);

    });
}
//查询
var doSearch = function (data) {

    var queryData = {};
    queryData.saleContract = $('#saleContract').textbox('getValue');

    queryData.businessGroup = $('#businessGroup').textbox('getValue');
    $('#maingrid').datagrid({ queryParams: queryData });
}
//取消
var undo = function () {
    window.top.closeTab();
}

//添加
var add = function () {

    window.top.addNewTab("铁路发货出库指令-新增", "/Bus/TrainCheckOut/trainCheckForm.aspx");
}


//保存
var save = function () {
    var rrdata = SaveDataToDB();

    if (rrdata == "ok") {
        $.messager.alert("提醒", "保存成功");
        window.top.selectTab('铁路发货出库指令');
    }
    else {
        //像用户提示错误
        $.messager.alert("提醒", rrdata);
    }

}
//修改
var edit = function () {
    var checkId = '';
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
     
        checkId = row.checkId;

        window.top.addNewTab("铁路发货出库指令-修改", "/Bus/TrainCheckOut/trainCheckForm.aspx?checkId=" + checkId);
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//查看
var browse = function () {
    var checkId = '';

    var row = $("#maingrid").datagrid('getSelected');

    if (row != undefined) {
        checkId = row.checkId;

        window.top.addNewTab("铁路发货出库指令-查看", "/Bus/TrainCheckOut/trainCheckForm.aspx?checkId=" + checkId + "&action=view");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//删除
var del = function () {
    var checkId = '';

    var row = $("#maingrid").datagrid('getSelected');

    if (row != undefined) {
        checkId = row.checkId;
        $.messager.confirm('系统提示', '确认要删除么？', function (r) {
            if (r) {
                $.post("/ashx/TrainCheckOut/trainCheckOperator.ashx?module=delCheck", { checkId: checkId }, function (data) {
                    alert(data);
                    if (data == "ok") {
                        $("#maingrid").datagrid("reload");
                    }
                    else {
                        $.messager.alert('系统提示', '删除失败!' + data, 'info');
                    }
                })
            }
        })


    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
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
//保存方法
var SaveDataToDB = function () {
    //获取产品列表并给隐藏域赋值
    var trainProduct = $("#productlist").datagrid("getRows");
    var trainProductJson = JSON.stringify(trainProduct);

    $('#trainProduct').attr('value', trainProductJson);
    //获取国境口岸站列表并给隐藏域赋值
    var trainFrontierStation = $("#frontierTd").datagrid("getRows");
    var trainFrontierStationJson = JSON.stringify(trainFrontierStation);
    $('#trainFrontierStation').attr('value', trainFrontierStationJson);
    //获取付费代码列表并给隐藏域赋值
    var trainpayCode = $("#paycodeandtracsit").datagrid("getRows");
    var trainpayCodeJson = JSON.stringify(trainpayCode);
    $('#trainpayCode').attr('value', trainpayCodeJson);
    var retdata = {};
    var action = "";

    if (isnew == "true") {
        action = "addCheck";
    }
    else {
        action = "editCheck";
    }

    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/TrainCheckOut/trainCheckOperator.ashx?module=' + action + '&checkId=' + checkCode,
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
//绑定数据
var bindUI = function () {
    //根据车厢数改变动态加载付费代码表格+
    $("input", $("#carriageCount").next("span")).blur(function () {
        //获得车厢数和集装箱规格,件数
        var carriageCount = $("#carriageCount").numberbox('getValue');
        var containerSize = $("#containerSize").combobox('getValue');
        var rows=  $('#productlist').datagrid('getRows');
        var productcount=rows[0]["packagesNumber"];
        

        if (carriageCount == "" || containerSize == "") {
            $.messager.alert("提醒", "请选择车厢数和集装箱规格");
        }
        else {
            //清空所有行
            $('#paycodeandtracsit').datagrid('loadData', { total: 0, rows: [] });
         
            carriageCount = parseInt(carriageCount);
            for (var i = 0; i < carriageCount; i++) {
                $("#paycodeandtracsit").datagrid('insertRow', {
                    index: 0,
                    row: {
                        containerText: containerSize,
                        productWeight: parseInt(productcount) / parseInt(carriageCount)
                    }
                });
             
                $("#paycodeandtracsit").datagrid('beginEdit',0);
            }



        }
    })
    //绑定合同号
    $('#saleContract').combobox({
        required: true,
        valueField: 'contractNo',
        textField: 'contractNo',
        editable: false,
        data: sbContractNo,
        onSelect: function (index, rowdata) {
            var contractno = $('#saleContract').combobox('getValue');
            //根据选择的合同号加载买方和卖方
            $.post("/ashx/TrainCheckOut/trainCheckOperator.ashx?module=getPeople", { contractno: contractno }, function (data) {


                $("#seller").textbox('setValue', data[0].seller);
                $("#buyer").textbox('setValue', data[0].buyer);
                $("#productName").textbox('setValue', data[0].pname);
            }, 'json');
            //根据合同号加载产品
            $('#productlist').datagrid({
                url: '/ashx/Contract/contractData.ashx?module=traincplist&contractNo=' + contractno,
                singleSelect: true,
                pagination: false,
                sortName: 'productName',
                sortOrder: 'asc',
                columns: [[
                            { field: 'productName', title: '货物名称', width: 150 },
                            { field: 'packagesType', title: '包装种类', width: 150 },
                            { field: 'packagesNumber', title: '件数', width: 150 },
                            { field: 'weight', title: '重量', width: 150, align: 'center' },


                ]],

            })
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
        //付费代码是否一致
        if ($("input[name='iscopyCode']:checked").val() == "0") {
            //获得车厢数和集装箱规格
            var carriageCount = $("#carriageCount").numberbox('getValue');
            var containerSize = $("#containerSize").numberbox('getValue');
            if (carriageCount == "" || containerSize == "") {
                $.messager.alert("提醒", "请选择车厢数和集装箱规格");
            }
            else {
                //清空所有行
                $('#paycodeandtracsit').datagrid('loadData', { total: 0, rows: [] });
                carriageCount = parseInt(carriageCount);
                for (var i = 0; i < carriageCount; i++) {
                    $("#paycodeandtracsit").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });
                    $("#paycodeandtracsit").datagrid('beginEdit', 0);

                }

            }
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
    //$('#Consignor').combobox('setValue', htdata.businessGroup);

    $('#Consignee').combobox({
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        data: comBuyer
    });
    //绑定集装箱规格
    $('#containerSize').combobox({
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        panelHeight: 'auto',
        data: sbContainer
    });
    //绑定到站
    $('#destination').combobox({
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        panelHeight: 'auto',
        data: sbDestination
    });
    //绑定发站
    $('#fromStation').combobox({
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        panelHeight: 'auto',
        data: sbFromStation
    });
}