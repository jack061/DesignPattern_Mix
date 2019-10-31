
$(function () {
    if (isnew == "true") {
        $("#form1").form('load', '/ashx/TrainCheckOut/trainCheckData.ashx?module=LoadSendOutData&contractNo=' + contractNo + '&createDateTag=' + createDateTag+ '&noticeTag=' + noticeTag);
    }
    else {
        $("#form1").form('load', '/ashx/TrainCheckOut/trainCheckData.ashx?module=LoadSendOutDataByEdit&createDateTag=' + createDateTag+ '&noticeTag=' + noticeTag);
    }
    //如果为查看状态
    if (isBrowse == "true") {
        $('input').attr('disabled', true);
        $("#aa123").hide();
    }
    //绑定Combobox数据
    bindCombobox();
    //加载货物详情子表
    loadProductlist();
    //加载国境口岸站子表
    loadForntiercode();
    //加载付费代码和过境付费代码子表
    loadPayandTransit();
    //根据车厢数改变动态加载付费代码表格
    loadPayTableByCarriage();
})
//关联买方为收货人checkbox选中事件
function contactRevClick() {

    if ($("#isContactConsignee").is(':checked') == true) {
        $("#isConsignee").attr('checked', false);
        //设置收货人为关联卖方
        $('#revMan').combobox({
            url: '/ashx/Contract/loadCombobox.ashx?module=customeDelivery&customername=' + contractInfo.revMan,
            required: false,
            required: false,
            valueField: 'name',
            textField: 'name',
            editable: false,
        });
        $("#revMan").combobox('setValue', $("#contactbuyer").val());
    }
}
//买方为收货人checkbox选中事件
function RevClick() {

    if ($("#isConsignee").is(':checked') == true) {
        $("#isContactConsignee").attr('checked', false);
        //收货人下拉框不可选
        $("#revMan").attr('readonly', true);
        //设置收货人为买方
        $('#revMan').combobox({
            url: '/ashx/Contract/loadCombobox.ashx?module=customeDelivery&customername=' + contractInfo.buyer,
            required: false,
            required: false,
            valueField: 'name',
            textField: 'name',
            editable: false,
        });
        $("#revMan").combobox('setValue', $("#buyer").val());
    }
}
//卖方为发货人checkbox选中事件
function SendClick() {

    if ($("#isConsignor").is(':checked') == true) {

        $("#isContactConsignor").attr('checked', false);

        //绑定发货人
        $('#sendMan').combobox({
            url: '/ashx/Contract/loadCombobox.ashx?module=buspplierDelivery&customername=' + $("#seller").val(),
            required: false,
            required: false,
            valueField: 'name',
            textField: 'name',
            editable: false,
        });
        $("#sendMan").combobox('setValue', $("#seller").val());
    }
}
//关联卖方为发货人checkbox选中事件
function contactSendClick() {
    if ($("#isContactConsignor").is(':checked') == true) {
        $("#isConsignor").attr('checked', false);
        $("#sendMan").textbox('setValue', $("#contactseller").val());
        //绑定发货人
        $('#sendMan').combobox({
            url: '/ashx/Contract/loadCombobox.ashx?module=buspplierDelivery&customername=' + $("#contactseller").val(),
            required: false,
            required: false,
            valueField: 'name',
            textField: 'name',
            editable: false,

        });
        $("#sendMan").combobox('setValue', $("#contactseller").val());
    }
}


//加载货物详情子表
function loadProductlist() {
    $('#productlist').datagrid({
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=trainProductByContractNo&createDateTag=' + createDateTag,
        singleSelect: true,
        pagination: false,
        sortName: 'productName',
        sortOrder: 'asc',
        columns: [[
		            { field: 'pname', title: '货物名称', width: 150 },
                     { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox' } },
		            { field: 'sendQuantity', title: '发货数量', width: 150 },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100 },
                    { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'spec', title: '产品规格', width: 100 },
                    { field: 'pacNumber', title: '件数', width: 100, },
                    { field: 'packdes', title: '最小包装', width: 100 },
                    { field: 'unit', title: '包装单位', width: 100 },
                    { field: 'packing', title: '包装', width: 100 },
                    { field: 'pallet', title: '托盘要求', width: 100, editor: 'textbox' },

        ]],


    });
}
//加载国境口岸站子表
function loadForntiercode() {
    var editRow = undefined;//先定义一个变量，
  
    $('#frontierTd').datagrid({
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=checkFrontierStation&createDateTag=' + createDateTag+'&noticeTag='+noticeTag,
        singleSelect: true,
        pagination: false,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            {
		                field: 'frontierStationCode', title: '代码', width: 150, editor: {
		                    type: 'combogrid',
		                    options: {
		                        panelWidth: 300,
		                        url: '/ashx/Contract/loadCombobox.ashx?module=harborout',
		                        idField: 'code',
		                        textField: 'code',
		                        columns: [[
                                            { field: 'code', title: '编码', width: 60 },
                                            { field: 'name', title: '口岸', width: 80 },
                                            { field: 'country', title: '国家', width: 100 },
                                          
		                        ]],
		                        onSelect: function (index, rowdata) {
		                            var data = $('#frontierTd').datagrid('getData');
		                            var length = data.rows.length;
		                            var thisTarget = $('#frontierTd').datagrid('getEditor', { 'index': length-1, 'field': 'frontierStation' }).target;
		                            thisTarget.textbox('setValue', rowdata.name);
		                        }
		                    }
		                }
		            },
		            { field: 'frontierStation', title: '名称', width: 150, align: 'center', editor: { type: 'textbox' } },
                      { field: 'carrier', title: '承运人', width: 150, align: 'center', editor: { type: 'textbox' } },
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
                    var data = $('#frontierTd').datagrid('getData');
                    var length = data.rows.length;
                    $("#frontierTd").datagrid('insertRow', {
                        index: length,
                        row: {}
                    });
                    $("#frontierTd").datagrid('beginEdit', length);
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
                ////追加发站到站
                //$('#frontierTd').datagrid('insertRow', {
                //    index: 0,	// 索引从0开始
                //    row: {
                //        frontierStationCode: $("#harboroutcode").val(),
                //        frontierStation: $('#harborout').textbox('getValue'),
                //    }
                //});
                //$('#frontierTd').datagrid('appendRow', {
                //    frontierStationCode: $("#harborarrivecode").val(),
                //    frontierStation: $('#harborarrive').textbox('getValue'),
                //});
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
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=checkPayCode&createDateTag=' + createDateTag+'&noticeTag=' + noticeTag,
        singleSelect: true,
        pagination: false,
        sortName: 'payCode',
        height:300,
        sortOrder: 'asc',
        columns: [[
		            { field: 'payCode', title: '付费代码', width: 400, editor: { type: 'textbox' } },
		            //{ field: 'transitPayCode', title: '过境1', width: 150, align: 'center', editor: { type: 'textbox' } },
                    //{ field: 'transitPayCode2', title: '过境2', width: 150, align: 'center', editor: { type: 'textbox' } },
                    //{ field: 'transitPayCode3', title: '过境3', width: 150, align: 'center', editor: { type: 'textbox' } },
                    //{ field: 'transitPayCode4', title: '过境4', width: 150, align: 'center', editor: { type: 'textbox' } },
                    {
                        field: 'containerSize', title: '集装箱规格', width: 150, align: 'center'
                    },

                       {
                           field: 'containerWeight', title: '集装箱重量(吨)', width: 150, align: 'center', editor:{type:'numberbox',options:{precision:2}} }
                       ,
                          {
                              field: 'carriageNumber', title: '车厢号', width: 150, align: 'center', editor: { type: 'textbox' }
                          },
                           {
                               field: 'productWeight', title: '产品净重', width: 150, align: 'center',editor:{type:'numberbox',options:{
                                   precision:2
                               }}
                           },
                           {
                               field: 'grossWeight', title: '毛重', width: 150, align: 'center', editor: {
                                   type: 'numberbox', options: {
                                       precision: 2
                                   }
                               }
                           },




        ]],
        toolbar: [{
            //    iconCls: 'icon-add',
            //    text: '新增',
            //    handler: function () {

            //        if (editRow != undefined) {
            //            //说明不是第一次添加
            //            $("#paycodeandtracsit").datagrid('endEdit', editRow);
            //            editRow = undefined;

            //        }
            //        if (editRow == undefined) {
            //            //说明是第一次添加，插入一个空行，让eidtRow=0；开始第一行的编辑，当再次添加的时候，由于editrow不为undefined，第一行就会结束编辑，再把editRow赋值为undefined，这样就会再次插入一行。
            //            $("#paycodeandtracsit").datagrid('insertRow', {
            //                index: 0,
            //                row: {}
            //            });
            //            $("#paycodeandtracsit").datagrid('beginEdit', 0);
            //            editRow = 0;
            //        }

            //    }
            //}, '-', {
            //    iconCls: 'icon-remove',
            //    text: '删除',
            //    handler: function () {
            //        var row = $("#paycodeandtracsit").datagrid('getSelected');
            //        var index = $("#paycodeandtracsit").datagrid('getRowIndex', row);

            //        if (index != null) {
            //            $('#paycodeandtracsit').datagrid('cancelEdit', index)
            //            .datagrid('deleteRow', index);


            //        } else {
            //            $.messager.alert('提示', '请选择一行数据！', 'info');
            //        }
            //    }
            //}, '-', {
            iconCls: 'icon-save',
            text: '保存',
            handler: function () {
                $('#paycodeandtracsit').datagrid('acceptChanges');
            }
        }, '-', {
            iconCls: 'icon-cut',
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
                            payCode: paycode,
                        }
                    });

                }
            }

        }, '-', {
            iconCls: 'icon-add',
            text: '自增',
            handler: function () {
                var rows = $('#paycodeandtracsit').datagrid('getRows');
                $('#paycodeandtracsit').datagrid('acceptChanges');
                if (rows[0]["payCode"] == "") {
                    $.messager.alert("提醒", "请填写第一行数据");
                    return;
                }
                var paycode = parseInt(rows[0]["payCode"]);

                for (var i = 1; i < rows.length; i++) {
                    var code = paycode + i;
                    $('#paycodeandtracsit').datagrid('updateRow', {
                        index: i,
                        row: {
                            payCode: code,
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
//查询
var doSearch = function (data) {

    var queryData = {};
    queryData.saleContract = $('#saleContract').textbox('getValue');

    queryData.businessGroup = $('#businessGroup').textbox('getValue');
    $('#maingrid').datagrid({ queryParams: queryData });
}
//取消
var cancel = function () {
    window.top.closeTab();
}
//保存
var save = function () {
    //保存付费代码
    $('#paycodeandtracsit').datagrid('acceptChanges');
    var rrdata = SaveDataToDB(0);
    if (rrdata.sucess == '1') {
        $.messager.alert("提醒", "保存成功");
        window.top.selectAndRefreshTab('铁路发货出库指令');
    }
    else {
        //像用户提示错误
        $.messager.alert("提醒", rrdata);
    }
}
//提交
var submit = function () {
    //保存付费代码
    $('#paycodeandtracsit').datagrid('acceptChanges');
    var rrdata = SaveDataToDB(1);
    if (rrdata.sucess == '1') {
        $.messager.alert("提醒", "提交成功");
        window.top.selectAndRefreshTab('铁路发货出库指令');
    }
    else {
        //像用户提示错误
        $.messager.alert("提醒", rrdata);
    }
}
//保存方法
var SaveDataToDB = function (status) {
    //获取产品列表并给隐藏域赋值
    var trainProduct = $("#productlist").datagrid("getRows");
    var trainProductJson = JSON.stringify(trainProduct);
    $('#trainProduct').attr('value', trainProductJson);
    //获取国境口岸站列表并给隐藏域赋值
    $("#frontierTd").datagrid("acceptChanges");
    var trainFrontierStation = $("#frontierTd").datagrid("getRows");
    var trainFrontierStationJson = JSON.stringify(trainFrontierStation);
    $('#trainFrontierStation').attr('value', trainFrontierStationJson);
    //获取付费代码列表并给隐藏域赋值
    $("#paycodeandtracsit").datagrid("acceptChanges");
    var trainpayCode = $("#paycodeandtracsit").datagrid("getRows");
    var trainpayCodeJson = JSON.stringify(trainpayCode);
    $('#trainpayCode').attr('value', trainpayCodeJson);

    var retdata = {};
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/TrainCheckOut/trainCheckOperator.ashx?module=addTrainDelivery&createDateTag=' + createDateTag+'&status='+status+'&noticeTag='+noticeTag,
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
//绑定Combobox数据
var bindCombobox = function () {
    //绑定托盘要求
    $('#palletRequire').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=palletRequire',
        required: false,
        valueField: 'CODE',
        textField: 'CODE',
        editable: false,
        onSelect: function (record) {
            $("#palletRequireRus").val(record.RUSSIAN);
        }
    });
    //绑定集装箱规格
    $('#containerSize').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=containerSize',
        required: false,
        valueField: 'code',
        textField: 'cname',
        editable: false,
    });

    //绑定收货人
    $('#revMan').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=customeDelivery&customername=' + contractInfo.revMan,
        required: false,
        required: false,
        valueField: 'name',
        textField: 'name',
        //editable: false,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) == 0;
        }
    });
    //绑定发货人
    $('#sendMan').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=buspplierDelivery&customername=' + contractInfo.sendMan,
        required: false,
        required: false,
        valueField: 'name',
        textField: 'name',
        //editable: false,
        filter: function (q, row) {
            var opts = $(this).combobox('options');
            return row[opts.textField].indexOf(q) == 0;
        }
    });
    //绑定发货人
    $('#harborout').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=harborout',
        required: false,
        required: false,
        valueField: 'name',
        textField: 'name',
        editable: false,
        onSelect: function (record) {
            $("#harboroutcode").val(record.code);
        }
    });
    //绑定发货人
    $('#harborarrive').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=harborout',
        required: false,
        required: false,
        valueField: 'name',
        textField: 'name',
        editable: false,
        onSelect: function (record) {
            $("#harborarrivecode").val(record.code);
        }
    });

}
//根据车厢数改变动态加载付费代码表格
function loadPayTableByCarriage() {
    //根据车厢数改变动态加载付费代码表格+
    $("input", $("#carriageCount").next("span")).blur(function () {
        //获得车厢数和集装箱规格,件数
        var carriageCount = $("#carriageCount").numberbox('getValue');
        var containerSize = $("#containerSize").combobox('getValue');
        var rows = $('#productlist').datagrid('getRows');
        var productcount = rows[0]["sendQuantity"];

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
                        containerSize: containerSize,
                        productWeight: parseInt(productcount) / parseInt(carriageCount)
                    }
                });

                $("#paycodeandtracsit").datagrid('beginEdit', 0);
            }
        }
    })

}