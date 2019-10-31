
var count = 0;
var countSubmit = 0;
$(function () {
    $("#form1").form('load', '/ashx/Contract/contractData.ashx?module=LoadManageData&contractNo=' + contractNo);
    bindCombobox();
    bindUI();
    loadTotalName();
    selectTabs();
   
})
//绑定数据
function bindUI() {
    if (isnew == "True" ) {
        $('#contractNo').textbox('setValue', '自动编号');
        $('#contractNo').textbox('readonly', true);
    }
    else {
        $('#contractNo').textbox('setValue', contractNo);
        $('#contractNo').textbox('readonly', true);
    }

    if (isBrowse == "true") {
        $('input').attr('disabled', true);
        $("#aa123").css("display", "none");
    }
}
//保存数据
function save() {
    count = count + 1;
    if (count>1) {
        $.messager.alert("提醒", "不可重复保存");
        return;
    }
    var rrdata = SaveDataToDB(0);
    if (rrdata != undefined && rrdata.sucess == '1') {
        window.top.selectAndRefreshTab('物流合同');
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        $.messager.alert("提醒", rrdata.sucdata.errormsg);
    }
}
//提交
var submit = function () {
    countSubmit = countSubmit + 1;
    if (countSubmit > 1) {
        $.messager.alert("提醒", "不可重复提交");
        return;
    }
    var rrdata = SaveDataToDB(1);
    if (rrdata != undefined && rrdata.sucess == '1') {
        window.top.selectAndRefreshTab('物流合同');

    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        $.messager.alert("提醒", rrdata.sucdata.errormsg);
    }
};
//取消
var undo = function () {

    //关闭当前tab
    window.top.closeTab();
}

var SaveDataToDB = function (status) {
    var url = '';
    $('#htmlcontent').attr('value', editor123.html());//获取条款文本
    url = '/ashx/Contract/contractOperater.ashx?module=addManageContract&status=' + status+'&isEdit='+isEdit;
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: url,
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
            var buyerCode = $("#buyerCode").val();
            var sellerCode = $("#sellerCode").val();
            var buyer = $("#buyer").combogrid('getValue');
            var seller = $("#seller").combogrid('getValue');
            var contractText = $("#htmlcontent").val();
            var contractNo = $("#contractNo").textbox('getValue');
            var validity = $("#validity").datebox('getValue');
            var ItemName = $("#ItemName").textbox('getValue');
            var ItemAmount = $("#ItemAmount").numberbox('getValue');
            if (title == "合同预览") {
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/contractData.ashx?module=GetManagePreview",
                    dataType: "text",
                    data: {
                        buyerCode: buyerCode, signedtime: signedTime,
                        signedplace: signedplace, language: "中文", sellerCode: sellerCode,
                        contractText: contractText, contractNo: contractNo, buyer: buyer, seller: seller,
                        validity: validity,ItemName:ItemName,ItemAmount:ItemAmount
                    },
                    success: function (data) {
                        $("#realTimeContractText").val(data);
                    }
                });
                $("#previewFormFrame").attr('src', '/Bus/ContractCategory/realTimePreviewLogisticsContract.aspx');
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
                    url: '/ashx/Contract/contractData.ashx?module=GetServiceReviewList&contractNo=' + contractNo,
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
            $("#seller").combogrid('setValue', data.name);
            $('#sellerCode').attr('value', data.code);

        }, 'json')

    })
    //买方(简称)文本框改变事件
    $("input", $("#simpleBuyer").next("span")).blur(function () {

        var simpleBuyer = ($("#simpleBuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
            $("#simpleBuyer").textbox('setValue', data.shortname);
            $("#buyer").combogrid('setValue', data.name);
            $('#buyerCode').attr('value', data.code);

        }, 'json')

    })

    $("input", $("#simplePartyC").next("span")).blur(function () {

        var simplePartyC = ($("#simplePartyC").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleSeller: simplePartyC }, function (data) {
            $("#simplePartyC").textbox('setValue', data.shortname);
            $("#partyC").combogrid('setValue', data.name);
            $('#partyCCode').attr('value', data.code);

        }, 'json')

    })
    //丁方(简称)文本框改变事件
    $("input", $("#simplePartyD").next("span")).blur(function () {
        var simplePartyD = ($("#simplePartyD").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleSeller: simplePartyD }, function (data) {
            $("#simplePartyD").textbox('setValue', data.shortname);
            $("#partyD").combogrid('setValue', data.name);
            $('#partyDCode').attr('value', data.code);

        }, 'json')

    })

}

function bindCombobox() {
    //绑定业务员编号
    $("#salesmanCode").combobox({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',
        valueField: 'UserRealName',
        textField: 'UserRealName',
        editable: false,
        onSelect: function (record) {
            $("#businessclass").combobox('setValue', record.Agency);
        }

    });
    //合同审核人绑定
    $("#adminReview").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=adminReview',

        valueField: 'cname',
        textField: 'cname',
        editable: false,
        onSelect: function (record) {
            $("#adminReviewNumber").val(record.code);
        }

    });
    $('#seller').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        idField: 'name',
        textField: 'name',
        editable: false,
        columns: [[
                    { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simpleSeller").textbox('setValue', rowdata.shortname);
            $("#sellerCode").val(rowdata.code);

        }
    });
    $('#buyer').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=buyer',
        idField: 'name',
        textField: 'name',

        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simpleBuyer").textbox('setValue', rowdata.shortname);
            $("#buyerCode").val(rowdata.code);
        }
    });
    $('#partyC').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=buyer',
        idField: 'name',
        textField: 'name',

        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simplePartyC").textbox('setValue', rowdata.shortname);

            $("#partyCCode").val(rowdata.code);
        }
    });
    $('#partyD').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        idField: 'name',
        textField: 'name',

        columns: [[
                     { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simplePartyD").textbox('setValue', rowdata.shortname);

            $("#partyDCode").val(rowdata.code);
        }
    });
}
//绑定费用类别表
function bindCostCategory() {
    $('#costCategoryTable').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=costCategoryList&contractNo=' + contractNo,
        rownumbers: true,
        singleSelect: true,
        sortName: 'costCategory',
        sortOrder: 'asc',
        fitColumns: true,
        columns: [[
		            { field: 'costCategory', title: '费用类别', width: 100 },
		            { field: 'project', title: '项目', width: 100, align: 'center', editor: { type: 'textbox' } },
                    { field: 'currency', title: '币种', width: 100, editor: { type: 'textbox' } },
                    { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '计价单位', width: 100, editor: { type: 'textbox' } },
                    { field: 'remark', title: '备注', width: 100, editor: { type: 'textbox' } },

        ]],
        onClickRow: function (index, row) {
            $('#costCategoryTable').datagrid('beginEdit', index);
        }
    });
}
//绑定checkbox点击事件
function checkClick(obj) {
    if (obj.checked) {
        var rows = $('#costCategoryTable').datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].costCategory == obj.value) {
                return;
            }
        }
        $('#costCategoryTable').datagrid('insertRow', {
            index: 0,	// 索引从0开始
            row: {
                costCategory: obj.value,
                project: "",
                currency: "",
                amount: "",
                remark: "",
            }
        });

    }
    else {
        var rows = $('#costCategoryTable').datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].costCategory == obj.value) {
                $('#costCategoryTable').datagrid('deleteRow', i);
            }
        }
    }
}
//计算按钮点击次数
function clickButtonCount() {
    var count = 0;
    $("#button1").click(function(){
        if (count == 2) {

        }
        count=count+1;

        //执行代码

    });
}