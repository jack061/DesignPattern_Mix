
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
//查看
var browse = function () {
    var checkId = '';

    var row = $("#maingrid").datagrid('getSelected');
  
    if (row != undefined) {
        //checkId = row.checkId;
        if (row.saveStatus == "0" || row.saveStatus == "1") {
            window.top.addNewTab("铁路发货出库指令-查看", "/Bus/TrainCheckOut/trainCheckFormTest.aspx?contractNo=" + row.contractNo + "&isnew=false" + "&createDateTag=" + row.createDateTag + '&noticeTag=' + row.noticeTag + "&isBrowse=true");
        }
        else {
            window.top.addNewTab("铁路发货出库指令-查看", "/Bus/TrainCheckOut/trainCheckFormTest.aspx?contractNo=" + row.contractNo + "&isnew=true" + "&createDateTag=" + row.createDateTag + '&noticeTag=' + row.noticeTag + "&isBrowse=true");
        }
        //window.top.addNewTab("铁路发货出库指令-查看", "/Bus/TrainCheckOut/trainCheckFormTest.aspx?contractNo=" + row.contractNo + "&isBrowse=true" + "&createDateTag=" + row.createDateTag + "&noticeTag=" + row.noticeTag);
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
//退回,更改发货通知状态为2，已退回
function sendback() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row.noticeTag!='undefined') {
        $.post("/ashx/TrainCheckOut/trainCheckOperator.ashx?module=updateSaveStatus", { noticeTag: row.noticeTag }, function (data) {
            if (data.sucess == "1") {
                $.messager.alert("提醒", "退回成功");
                $("#maingrid").datagrid("reload");
            }
            else {
                $.messager.alert("提醒", data.warnmsg);
            }
        })
    }
}
//添加车厢号
function addCarriageCode() {
    var row = $("#maingrid").datagrid('getSelected');

   
    window.top.addNewTab("发货车厢号-添加", "/Bus/TrainCheckOut/addCarriageCode.aspx?createDateTag=" + row.createDateTag + "&noticeTag=" + row.noticeTag);
}
//保价单打印
function printInsured() {
    var row = $("#maingrid").datagrid('getSelected');
    
    if (row != undefined) {
        window.top.addNewTab("保价单打印", '/Bus/TrainCheckOut/printInsured.aspx?contractNo='+row.contractNo+'&createDateTag='+row.createDateTag+'&noticeTag='+row.noticeTag, '');
    }
}