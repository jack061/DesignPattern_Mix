
$(function () {
    $('#printlist').datagrid({
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=checkPayCode&createDateTag=' + createDateTag + '&noticeTag=' + noticeTag,
        rownumbers: true,
        singleSelect: true,
        sortName: 'payCode',
        sortOrder: 'asc',
        columns: [[
                      { field: 'payCode', title: '付费代码', width: 100 },
		             { field: 'transitPayCode', title: '过境1', width: 100, align: 'center' },
                     { field: 'transitPayCode2', title: '过境2', width: 100, align: 'center' },
                     { field: 'transitPayCode3', title: '过境3', width: 100, align: 'center' },
                     { field: 'transitPayCode4', title: '过境4', width: 100, align: 'center' },
                     { field: 'containerSize', title: '集装箱规格', width: 100, align: 'center' },
                     { field: 'carriageNumber', title: '车厢号', width: 100, align: 'center', editor: { type: 'numberbox' }, options: {precision:3} },
                     { field: 'productWeight', title: '产品重量', width: 100, align: 'center' },
                     { field: 'contractNo', title: '合同号', width: 100, align: 'center' },
        ]],
        toolbar: [{
            text: '保存',
            iconCls: 'icon-add',
            handler: function () {
                $('#printlist').datagrid('acceptChanges');
                var payRows = $('#printlist').datagrid('getRows');
                var rowsJson = JSON.stringify(payRows);
                $.post("/ashx/TrainCheckOut/trainCheckOperator.ashx?module=updateCarriageCode", { rowsJson: rowsJson, noticeTag: noticeTag }, function (data) {
                    if(data.sucess=="1"){
                        $.messager.alert("提醒", "保存成功", "info");
                        $("#printlist").datagrid(reload);
                    }
                    else {
                        $.messager.alert("提醒", data.errormsg, "info");
                      
                    }
                })
                //window.top.addNewTab('铁路运单打印', '/TrainApply/PrintTrainApply.aspx?contractNo=' + payRows.contractNo + '&payCode=' + payRows.payCode + '&createDateTag=' + createDateTag + '&noticeTag=' + noticeTag, "_blank", "");
            }
        }
        ],
        onClickCell: function (index, field, value) {
            $(this).datagrid('beginEdit', index);

        },

    });
    //var rows = $("#printlist").datagrid('getRows');
    //alert(rows.length);
    //for (var i = 0; i < rows.length; i++) {
    //    $(this).datagrid('beginEdit', i);
    //}
    ////$('#printlist').datagrid('beginEdit');

})

////加载付费代码和过境付费代码子表
function loadPayandTransit() {
    var editRow = undefined;//先定义一个变量，
   
    $('#paycodeandtracsit').datagrid({
        url: '/ashx/TrainCheckOut/trainCheckData.ashx?module=checkPayCode&createDateTag=' + createDateTag,
        singleSelect: true,
        pagination: false,
        sortName: 'payCode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'payCode', title: '付费代码', width: 100},
		            { field: 'transitPayCode', title: '过境代码', width: 100, align: 'center', },
                    {
                        field: 'containerSize', title: '集装箱规格', width: 100, align: 'center'
                    },

                       {
                           field: 'containerWeight', title: '集装箱重量(吨)', width: 100, align: 'center'
                       },
                          {
                              field: 'carriageNumber', title: '车厢号', width: 100, align: 'center', editor: { type: 'numberbox' }
                          },
                           {
                               field: 'productWeight', title: '产品重量', width: 100, align: 'center'
                           },




        ]]
      ,
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
var undo = function () {
    window.top.closeTab();
}
//保存
var save = function () {
    //保存付费代码
    $('#paycodeandtracsit').datagrid('acceptChanges');
    var rrdata = SaveDataToDB();
    if (rrdata.sucess == '1') {
        $.messager.alert("提醒", "保存成功");
        window.top.selectAndRefreshTab('铁路发货出库指令');
    }
    else {
        //像用户提示错误
        $.messager.alert("提醒", rrdata);
    }
}
//保存方法
var SaveDataToDB = function () {
   
    //获取付费代码列表并给隐藏域赋值
    var trainpayCode = $("#paycodeandtracsit").datagrid("getRows");
    var trainpayCodeJson = JSON.stringify(trainpayCode);

    $('#trainpayCode').attr('value', trainpayCodeJson);
    var retdata = {};
    var dataPayCode = {
        trainpayCode: $('#trainpayCode').val(),

    }
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/TrainCheckOut/trainCheckOperator.ashx?module=addCarriageCode&createDateTag=' + createDateTag,
        data: dataPayCode, // 你的formid
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