
//生成箱单
function packing() { //箱单
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.contractNo;
        if (no != '')
            window.top.addNewTab('报关箱单', '/Bus/TrainCheckOut/InvoiceOrPacking.aspx?type=packing&no=' + no, "");
        else {
            window.top.addNewTab('报关箱单', '/Bus/TrainCheckOut/InvoiceOrPacking.aspx?type=packing&no=' + row.contractNo, "");
        }
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//生成发票
function other() { //发票
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.contractNo;
        if (no != '') {
            window.top.addNewTab('报关发票', '/Bus/TrainCheckOut/InvoiceOrPacking.aspx?type=invoice&no=' + no, "");
        }
        else {
            window.top.addNewTab('报关发票', '/Bus/TrainCheckOut/InvoiceOrPacking.aspx?type=invoice&no=' + row.contractNo, "");
        }
    }
}
//生成价格单
function priceList() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row != undefined) {
        no = row.contractNo;
        if (no != '') {
            window.top.addNewTab('价格单查看', '/Bus/TrainCheckOut/PriceListForm.aspx?no=' + no, "");
        }
    } else {
        $.messager.alert("警告", "请选择一行数据！");
    }
}
//运单打印
function printwaybill() {
    var row = $("#maingrid").datagrid('getSelected');

    if (row.blockTrain == "1") {
        $.messager.confirm('确认对话框', '是班列，确定要打印吗？', function (r) {
            if (r) {
                window.top.addNewTab("铁路付费代码打印", "/Bus/TrainCheckOut/trainPrintPayCode.aspx?createDateTag=" + row.createDateTag + "&noticeTag=" + row.noticeTag);
            }
        });
        return;
    }


    window.top.addNewTab("铁路付费代码打印", "/Bus/TrainCheckOut/trainPrintPayCode.aspx?createDateTag=" + row.createDateTag + "&noticeTag=" + row.noticeTag);
}
//添加
var sendNotice = function () {
    var row = $("#maingrid").datagrid('getSelected');
    if (row == undefined || row.sendstatus == "1") {
        $.messager.alert('提示', '请选择发货未完成数据！', 'info');
    }
    else {
        ////如果卖方为香港公司，确保关联合同为
        //if (row.sellercoe == "160912") {
        //    if (row.purchaseCode == "") {
        //        $.messager.alert('提示', '卖方为香港必须先创建关联合同！', 'info');
        //        return;
        //    }
        //}
        window.top.addNewTab("铁路发货出库指令-新增", "/Bus/TrainCheckOut/trainCheckFormTest.aspx?contractNo=" + row.contractNo + "&isnew=true" + "&createDateTag=" + row.createDateTag);
    }
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
    var row = $("#maingrid").datagrid('getSelected');
    if (row.sendstatus != "1") {
        $.messager.alert('提示', '请选择发货完成数据！', 'info');
        return;

    } else {

        window.top.addNewTab("铁路发货出库指令-修改", "/Bus/TrainCheckOut/trainCheckFormTest.aspx?contractNo=" + row.contractNo + "&isnew=false" + "&createDateTag=" + row.createDateTag);
    }
}
//查看
var browse = function () {
    var checkId = '';

    var row = $("#maingrid").datagrid('getSelected');

    if (row != undefined) {
        //checkId = row.checkId;
        window.top.addNewTab("铁路发货出库指令-查看", "/Bus/TrainCheckOut/trainCheckFormTest.aspx?contractNo=" + row.contractNo + "&isBrowse=true" + "&createDateTag=" + row.createDateTag + "&noticeTag=" + row.noticeTag);
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//查询操作
function SearchData() {
    var row = $("#maingrid").datagrid('getSelected');
    contractNo = row.contractNo;
    buyer_sch = $('#buyer_sch').textbox('getValue');
    seller_sch = $('#seller_sch').textbox('getValue');
    para = {};
    para.contractNo = contractNo;
    //para.attachmentno = attachmentno;
    para.signedtime_begin = signedtime_begin;
    para.signedtime_end = signedtime_end;
    para.buyer_sch = buyer_sch;
    para.seller_sch = seller_sch;
    //para.ifchecked = ifchecked;
    $("#maingrid").datagrid('load', para);
}

//删除
var del = function () {
    var checkId = '';

    var row = $("#maingrid").datagrid('getSelected');

    if (row.applystatus == "1") {
        contractNo = row.contractNo;
        $.messager.confirm('系统提示', '确认要删除么？', function (r) {
            if (r) {
                $.post("/ashx/TrainCheckOut/trainCheckOperator.ashx?module=delTrainDelivery", { contractNo: contractNo }, function (data) {
                    if (data.sucess == '1') {
                        $("#maingrid").datagrid("reload");
                    }
                    else {
                        $.messager.alert('系统提示', '删除失败!' + data, 'info');
                    }
                })
            }
        })


    } else {
        $.messager.alert('提示', '请选择发货完成数据！', 'info');
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
//添加车厢号
function addCarriageCode() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row.sendstatus != "1") {
        $.messager.alert('提示', '请选择发货完成数据！', 'info');
        return;
    }
    else {
        window.top.addNewTab("发货车厢号-添加", "/Bus/TrainCheckOut/addCarriageCode.aspx?contractNo=" + row.contractNo + "&isnew=true" + "&createDateTag=" + row.createDateTag);
    }
}
