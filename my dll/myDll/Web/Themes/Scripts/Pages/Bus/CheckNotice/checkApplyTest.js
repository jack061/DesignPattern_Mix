var fileColumName = "";

$(function () {

    if (applytype == '中泰订舱') {
        $("#Fieldset1").hide();
    }

    if (isNew == 'true') {
        if (jQuery.isEmptyObject(applyInfo)) {
            bindCombobox();
            initAddData();
            initAddSubList();
        } else {
            initData();
            initSubList();
        }
    }
    if (isNew == 'false') {
        //bindCombobox();
        initData();
        initSubList();
       } 
    if (isBrowse == 'true') {
        initData();
        initSubList();
        //$("input").attr("readonly", true);
        $("#btabs").hide();
        $("input").attr("disabled", "disabled");
    }
    //查看图片
    $("#dlg").dialog("close"); //init
    $("#btnInfoUpload").click(function () {
        uploadFile();
    });

    $('#boxorderName').textbox({
        onClickButton: function () {
            var url = $('#boxorderUrl').val();
            if (url != '' && url != '&nbsp;') {
                window.open(url);
            } else {
                alert('请先上传文件！');
            }
        }
    });
    $('#bhorderName').textbox({
        onClickButton: function () {
            var url = $('#bhorderUrl').val();
            if (url != '' && url != '&nbsp;') {
                window.open(url);
            } else {
                alert('请先上传文件！');
            }
        }

    });
    $('#noticeorderName').textbox({
        onClickButton: function () {
            var url = $('#noticeorderUrl').val();
            if (url != '' && url != '&nbsp;') {
                window.open(url);
            } else {
                alert('请先上传文件！');
            }
        }
    });

});

//添加时初始化数据

function initAddData() {
    $("#seller").textbox('setValue', contractInfo.seller);

//    $("#consignor").textbox('setValue', contractInfo.seller);
//    $("#consignorAddress").textbox('setValue', contractInfo.selleraddress);
//    $("#consignee").textbox('setValue', contractInfo.buyer);
//    $("#consigneeAddress").textbox('setValue', contractInfo.buyeraddress);

    $.ajax({
        type: 'POST',
        url: '/ashx/Basedata/BFactory.ashx?action=getFactoryInfo&code=' + contractInfo.sellercode,
        dataType: 'json',
        success: function (result) {
            $("#consignor").textbox('setValue', result.EGNAME);
            $("#consignorAddress").textbox('setValue', result.EGADDRESS);
            $("#consignorPhone").textbox('setValue', result.PHONE);
            }    
    });

    $("#buyer").textbox('setValue', contractInfo.buyer);
    // $("#consignee").combobox('setValue', contractInfo.buyer);
    $.ajax({
        type: 'POST',
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=getCustomerInfo&code=' + contractInfo.buyercode,
        dataType: 'json',
        success: function (result) {
            $("#consignee").textbox('setValue', result.EGNAME);
            $("#consigneeAddress").textbox('setValue', result.EGADDRESS);
            $("#consigneePhone").textbox('setValue', result.PHONE);
        }
    });
    //获取港口信息
    //起运港
    $.ajax({
        type: 'POST',
        url: '/ashx/Basedata/HarborListHandler.ashx?type=getHarborInfo&name=' + contractInfo.harborout,
        dataType: 'json',
        success: function (result) {
            $("#departurePort_en").textbox('setValue', result.EGNAME);
            $("#departurePortCountry").textbox('setValue', result.COUNTRY);
            $("#departurePortCountry_en").textbox('setValue', result.COUNTRYENG);
        }
    });
    //卸货港
    $.ajax({
        type: 'POST',
        url: '/ashx/Basedata/HarborListHandler.ashx?type=getHarborInfo&name=' + contractInfo.harborarrive,
        dataType: 'json',
        success: function (result) {
            $("#unloadPort_en").textbox('setValue', result.EGNAME);
            $("#unloadPortCountry").textbox('setValue', result.COUNTRY);
            $("#unloadPortCountry_en").textbox('setValue', result.COUNTRYENG);
        }
    });
    $("#departurePort").textbox('setValue', contractInfo.harborout);
    $("#unloadPort").textbox('setValue', contractInfo.harborarrive);
    $("#saleContract").textbox('setValue', contractInfo.contractNo);
}

//绑定combobox 
function bindCombobox() {

    /*
    //发货人
    $('#consignor').combogrid({
    panelWidth: 450,
    url: '/ashx/Contract/loadCombobox.ashx?module=seller',
    idField: 'code',
    textField: 'cname',
    columns: [[
    { field: 'code', title: '客户编码', width: 60 },
    { field: 'shortname', title: '简称', width: 80 },
    { field: 'name', title: '客户名称', width: 100 },
    { field: 'address', title: '地址', width: 150 }
    ]],
    onSelect: function (index, rowdata) {
    $("#consignor").combogrid('setValue', rowdata.name);
    $("#consignorAddress").textbox('setValue', rowdata.address);
    $("#consignorPhone").textbox('setValue', '');
    }
    });

    //收货人
    $('#consignee').combogrid({
    panelWidth: 450,
    url: '/ashx/Contract/loadCombobox.ashx?module=customeDelivery&customername=' + contractInfo.buyer,
    idField: 'code',
    textField: 'cname',
    columns: [[
    //                    { field: 'code', title: '客户编码', width: 60 },
    //                    { field: 'shortname', title: '简称', width: 80 },
    //                    { field: 'name', title: '客户名称', width: 100 },
    //                    { field: 'address', title: '地址', width: 150 }

    { field: 'name', title: '收货人', width: 80 },
    { field: 'address', title: '地址', width: 150 },
    { field: 'phone', title: '电话', width: 100 }
    ]],
    onSelect: function (index, rowdata) {
    $("#consignee").combogrid('setValue', rowdata.name);
    $("#consigneeAddress").textbox('setValue', rowdata.address);
    $("#consigneePhone").textbox('setValue', rowdata.phone);
    }
    });
    
    //通知人
    $('#noticeMan1').combogrid({
    panelWidth: 450,
    url: '/ashx/Contract/loadCombobox.ashx?module=customeNotice&customername=' + contractInfo.buyer,
    idField: 'code',
    textField: 'cname',
    columns: [[
    { field: 'name', title: '通知人', width: 80 },
    { field: 'address', title: '地址', width: 150 },
    { field: 'phone', title: '电话', width: 100 }
    ]],
    onSelect: function (index, rowdata) {
    $("#noticeMan1").combogrid('setValue', rowdata.name);
    $("#noticeManAddress1").textbox('setValue', rowdata.address);
    $("#noticeManPhone1").textbox('setValue', rowdata.phone);
    }
    });
    //通知人
    $('#noticeMan2').combogrid({
    panelWidth: 450,
    url: '/ashx/Contract/loadCombobox.ashx?module=customeNotice&customername=' + contractInfo.buyer,
    idField: 'code',
    textField: 'cname',
    columns: [[
    { field: 'name', title: '通知人', width: 80 },
    { field: 'address', title: '地址', width: 150 },
    { field: 'phone', title: '电话', width: 100 }
    ]],
    onSelect: function (index, rowdata) {
    $("#noticeMan2").combogrid('setValue', rowdata.name);
    $("#noticeManAddress2").textbox('setValue', rowdata.address);
    $("#noticeManPhone2").textbox('setValue', rowdata.phone);
    }
    });
    */
    //运费条款
    $('#shippngcostItem').combobox({
        url: '/ashx/Basedata/DictronaryHandler.ashx?action=getShippngcostItemList',
        valueField: 'id',
        textField: 'text'
    });
 }
function initAddSubList() {
    $('#htcplist').datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        sortName:'contractNo',
        sortOrder:'desc',
        url: '/ashx/CheckNotice/CheckData.ashx?module=getAddSubList&contractNo=' + contractNo + "&createDateTag=" + createDateTag,
        columns: [[
            { field: 'mass', title: '箱唛', width: '100px' },
            { field: 'pcode', title: '货物编码', width: '100px' },
            { field: 'pname', title: '货物名称', width: '200px' },
            { field: 'spec', title: '规格', width: '100px' },
            { field: 'quantity', title: '数量', width: '150px' },
            { field: 'qunit', title: '单位', width: '150px' },
            { field: 'packing', title: '包装', width: '200px' },
            { field: 'weight', title: '重量', width: '100px' },
            { field: 'volume', title: '体积', width: '120px' }
            ]],
        pagination: true
    });
}
//保存
function save() {
    var shipProduct = $("#htcplist").datagrid("getRows");
    var shipProductJson = JSON.stringify(shipProduct);
    $('#shipProduct').attr('value', shipProductJson);

    var retdata = {};
    var action = "saveNotice";
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/CheckNotice/CheckData.ashx?module=' + action + "&createDateTag=" + createDateTag,
        data: $('#form1').serialize(),

        async: false,
        error: function (data) {
            $.messager.alert("提醒", "保存失败");
        },
        success: function (data) {
            var result = JSON.parse(data);
            if ("T" == result.status) {
                $.messager.alert("提醒", "保存成功");

            } else {
                $.messager.alert("提醒", result.msg);
            }
        }
    });
}
//保存提交
function save_submit() {
    $.messager.confirm("确认", "请检查所填信息正确无误，提交后不可修改！", function (r) {
        if (r) {
            if (contractInfo.paystatus == '未收款') {
                $.messager.confirm("确认", "合同："+contractInfo.contractNo+"未到款，您确认提交吗？", function (r) {
                    if (r) {
                        if ($("#form1").form('validate')) {
                            var rrdata = SaveDataToDB();

                            var result = JSON.parse(rrdata);
                            if ("T" == result.status) {
                                $.messager.alert("提醒", "保存成功");
                                //window.top.closeTab();
                                top.selectAndRefreshTab('海运订舱申请');

                            } else {
                                $.messager.alert("提醒", result.msg);
                            }
                        } else {
                            return false;
                        }
                    }
                });
            }
        }
    }); 

}
//保存方法
var SaveDataToDB = function () {
    var shipProduct = $("#htcplist").datagrid("getRows");
    var shipProductJson = JSON.stringify(shipProduct);
    $('#shipProduct').attr('value', shipProductJson);

    var retdata = {};
    var action = "submitNotice";
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/CheckNotice/CheckData.ashx?module=' + action + "&createDateTag=" + createDateTag,
        data: $('#form1').serialize(),

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

//取消
function cancel() {

    //关闭当前tab
    window.top.closeTab();
}

//查看时调用函数
function initData() {
    $('#form1').form('load', applyInfo);
}
function initSubList() {
    $('#htcplist').datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        sortName: 'checkNoticeNumber',
        sortOrder: 'desc',
        url: '/ashx/CheckNotice/CheckData.ashx?module=getCheckNoticeSubList&checkNoticeNumber=' + applyInfo.checkNoticeNumber,
        columns: [[
            { field: 'mass', title: '箱唛', width: '150px' },
            { field: 'pname', title: '货物名称', width: '200px' },
            { field: 'quantity', title: '数量', width: '150px' },
            { field: 'qunit', title: '单位', width: '150px' },
            { field: 'packing', title: '包装', width: '200px' },
            { field: 'packspec', title: '重量', width: '100px' },
            { field: 'volume', title: '体积', width: '120px' }
            ]],
        pagination: true
    });
}

//打开长传界面
function uploadFile_open(name)
{
    fileColumName = name;
    $("#dlg").dialog("open"); //打开
}

//文件上传
function uploadFile() {
    $("#form_up").ajaxSubmit({
        url: "/ashx/CheckNotice/CheckData.ashx?module=upload",
        type: "post",
        success: function (data) {
            if (data == "error") {
                $.messager.alert("错误：", "上传失败");
            }
            else {
                $.messager.alert("提醒：", "上传成功");
                var str = data.split(':'); 
                $("#" + fileColumName + "Url").val(str[0]);//str[0]-文件路径
                $("#" + fileColumName + "Name").textbox('setText', str[1]); //str[1]-文件名称
                $("#" + fileColumName + "Name").val(str[1]);
                
            }
        }
    })
}