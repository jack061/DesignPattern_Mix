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
    loadTotalName();
    bindUI();
    bindCombobox();//绑定买方卖方
    initContractInfo();//初始化合同信息
    selectTabs();
});
////返回
var back = function () {

    window.top.closeAddTab("合同-新增", "/Bus/Contract/ContractTest.aspx");
}
//保存
var save = function () {
    var rrdata = SaveDataToDB(0);

    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        window.top.selectAndRefreshTab('内部结算单');
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        $.messager.alert("提醒", rrdata.sucdata.errormsg);
    }
}
//取消
var cancel = function () {
    //关闭当前tab
    window.top.closeTab();
}
//提交
var submit = function () {

    var rrdata = SaveDataToDB(1);
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        $.messager.alert('系统提示', '提交成功!', 'info', function () {
            //打开指定模板页面
            window.top.selectAndRefreshTab('内部结算单');
        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
};

//返回填写信息
function backWrite() {

    $('#tt').tabs("select", "手动变量");
}
var SaveDataToDB = function (status) {
    var retdata = {};
    //获取list数据到htcplistStr
    $("#htcplist").datagrid("acceptChanges");
    var cplist = $("#htcplist").datagrid("getRows");
    var datagridjson = JSON.stringify(cplist);
    $('#htcplistStr').attr('value', datagridjson);
    $('#hideText1').attr('value', editor1.html());//获取条款文本
    $('#hideText2').attr('value', editor2.html());//获取条款文本
    $('#hideText3').attr('value', editor3.html());//获取条款文本
    $('#hideText4').attr('value', editor4.html());//获取条款文本
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/Contract/contractOperater.ashx?module=addInternalContract&status='+status+'&isEdit='+isEdit,
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
}
console.time('初始化');

var bindUI = function () {
    if (isBrowse=="true") {
        $("#aa123").hide();
        $('input').attr("disabled", true);
    }

}

//查询产品
function SearchContract() {
    pcode = $('#pcode').textbox('getText');

    name = $('#pname').textbox('getText');
    para = {};
    para.pcode = pcode;
    para.name = name;
    $('#productlist').datagrid('load', para);

}

//tabs选中事件
function selectTabs() {
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
            $('#hideText1').attr('value', editor1.html());//获取条款文本
            $('#hideText2').attr('value', editor2.html());//获取条款文本
            $('#hideText3').attr('value', editor3.html());//获取条款文本
            $('#hideText4').attr('value', editor4.html());//获取条款文本
            var contractNo = $("#contractNo").textbox('getValue');
            var signedTime = $("#signedtime").datebox('getValue');
            var signedplace = $("#signedplace").textbox('getValue');
            var itemProName = $("#itemProName").textbox('getValue');
            var createTableName = $("#createTableName").datebox('getValue');
            var Organizer = $("#Organizer").textbox('getValue');
            var startDate = $("#startDate").datebox('getValue');
            var endDate = $("#endDate").datebox('getValue');
            var signedplace = $("#signedplace").textbox('getValue');
            var buyer = $("#buyer").combobox('getValue');
            var seller = $("#seller").combobox('getValue');
            var remark = $("#htmlContent").val();
            var text1 = $("#hideText1").val();
            var text2 = $("#hideText2").val();
            var text3 = $("#hideText3").val();
            var text4 = $("#hideText4").val();
            var buyercode = $("#buyercode").val();
            var sellercode = $("#sellercode").val();
            $('#htcplist').datagrid('acceptChanges');
            //获取list数据到htcplistStr
            var cplist = $("#htcplist").datagrid("getRows");
            var datagridjson = JSON.stringify(cplist);
            if (title == "查看结算单文本") {
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/contractData.ashx?module=GetInternalPreview",
                    dataType: "text",
                    data: {
                        buyer: buyer, seller: seller,
                        signedTime: signedTime,
                        signedplace: signedplace,
                        datagridjson: datagridjson,
                        remark: remark,
                        buyercode: buyercode,
                        sellercode: sellercode,
                        contractNo: contractNo,
                        itemProName: itemProName,
                        createTableName: createTableName,
                        Organizer: Organizer,
                        startDate: startDate,
                        endDate: endDate,
                        text1: text1,
                        text2: text2,
                        text3: text3,
                        text4: text4,
                    },
                    success: function (data) {
                        $("#realTimeContractText").val(data);
                    }
                });
                $("#previewFormFrame").attr('src', '/Bus/ContractCategory/internalClearingPreviewTemplate.aspx');
            }
        }
    })
}

console.timeEnd('初始化');
//加载产品列表
function loadProdunt() {
    console.time('产品列表');
    var buyer = $("#buyer").combobox('getValue');
    var seller = $("#seller").combobox('getValue');
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=createInterProduct&contractNo=' + contractInfo.contractNo+"&buyer="+buyer+"&seller="+seller,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                     { field: 'spec', title: '产品规格', width: 100 },
                    { field: 'quantity', title: '合同数量', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '最低销售价', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                   {
                       field: 'priceUnit', title: '单位', width: '100px', editor: {
                           type: 'combobox',
                           options: {
                               url: '/ashx/Contract/loadCombobox.ashx?module=currency',
                               valueField: 'code',
                               textField: 'code',
                               editable: false,
                           }
                       }
                   },
                    { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'rate', title: '税率', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'SAPNumber', title: 'SAP订单类型和编号', width: 100, editor: { type: 'textbox', } }
                   
        ]],
        onClickCell: function (index, row, changes) {
            $(this).datagrid('beginEdit', index);
            var editors = $(this).datagrid('getEditors', index);
            var quantity = editors[0];
            var price = editors[1];
            var amount = editors[3];
            $(price.target).numberbox({
                onChange: function (n, o) {
                    var finamount = parseFloat(quantity.target.val()) * (parseFloat(price.target.val()));//计算后总金额
                    $(amount.target).numberbox('setValue', finamount);
                }
            })
            $(quantity.target).numberbox({
                onChange: function (n, o) {
                    var finamount = parseFloat(quantity.target.val()) * (parseFloat(price.target.val()));//计算后总金额
                    $(amount.target).numberbox('setValue', finamount);
                }
            })
        },
        toolbar: [{
            iconCls: 'icon-add',
            text: '回滚',
            handler: function () {
                $("#htcplist").datagrid("rejectChanges");
            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var rowindex = $('#htcplist').datagrid('getRowIndex', $('#htcplist').datagrid('getSelected'));
                $('#htcplist').datagrid('deleteRow', rowindex);
            }
        }],
        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
    console.timeEnd('产品列表');
}
//加载内结产品表
function loadInternalProdunt() {
    console.time('产品列表');
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=GetInternalProductList&contractNo=' + contractInfo.contractNo,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		           { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'spec', title: '产品规格', width: 100 },
                    { field: 'quantity', title: '合同数量', width: 100, editor: { type: 'numberbox', options: {precision:2}} },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '最低销售价', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                   {
                       field: 'priceUnit', title: '单位', width: '100px', editor: {
                           type: 'combobox',
                           options: {
                               url: '/ashx/Contract/loadCombobox.ashx?module=currency',
                               valueField: 'code',
                               textField: 'code',
                               editable: false,
                           }
                       }
                   },
                    { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'rate', title: '税率', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'SAPNumber', title: 'SAP订单类型和编号', width: 100, editor: { type: 'textbox', } }
                  
        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '回滚',
            handler: function () {
                $("#htcplist").datagrid("rejectChanges");
            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var rowindex = $('#htcplist').datagrid('getRowIndex', $('#htcplist').datagrid('getSelected'));
                $('#htcplist').datagrid('deleteRow', rowindex);
            }
        }],
        onClickCell: function (index, row, changes) {
            $(this).datagrid('beginEdit', index);
            var editors = $(this).datagrid('getEditors', index);
            var quantity = editors[0];
            var price = editors[1];
            var amount = editors[3];
            $(price.target).numberbox({
                onChange: function (n, o) {
                    var finamount = parseFloat(quantity.target.val()) * (parseFloat(price.target.val()));//计算后总金额
                    $(amount.target).numberbox('setValue', finamount);
                }
            })
            $(quantity.target).numberbox({
                onChange: function (n, o) {
                    var finamount = parseFloat(quantity.target.val()) * (parseFloat(price.target.val()));//计算后总金额
                    $(amount.target).numberbox('setValue', finamount);
                }
            })
        },
        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
    console.timeEnd('产品列表');
}
//初始化合同信息
function initContractInfo() {

    if (isContactSeller == "true") {//卖方为关联方
        $("#contractNo").textbox('setValue', '自动编号');
        $("#signedplace").textbox('setValue', contractInfo.signedplace);
        $("#signedtime").datebox('setValue', contractInfo.signedtime);
        $("#buyer").combogrid('setValue', contractInfo.seller);
        $("#buyercode").val(contractInfo.sellercode);
        $("#simpleBuyer").textbox('setValue', contractInfo.simpleSeller);
        $("#purchaseCode").textbox('setValue', contractInfo.contractNo);
        loadProdunt();
    }
    else if (isContactBuyer=="true") {//买方为关联方
        $("#contractNo").textbox('setValue', '自动编号');
        $("#signedplace").textbox('setValue', contractInfo.signedplace);
        $("#signedtime").datebox('setValue', contractInfo.signedtime);
        $("#seller").combogrid('setValue', contractInfo.buyer);
        $("#sellercode").val(contractInfo.buyercode);
        $("#simpleSeller").textbox('setValue', contractInfo.simpleBuyer);
        $('#simpleSeller').textbox('textbox').attr('readonly', true);
        $("#purchaseCode").textbox('setValue', contractInfo.contractNo);
        loadProdunt();
    }
    else {//修改内结
        $("#contractNo").textbox('setValue', contractInfo.contractNo);
        $("#signedplace").textbox('setValue', contractInfo.signedplace);
        $("#signedtime").datebox('setValue', contractInfo.signedtime);
        $("#buyer").combogrid('setValue', contractInfo.buyer);
        $("#buyercode").val(contractInfo.buyercode);
        $("#sellercode").val(contractInfo.sellercode);
        $("#seller").combogrid('setValue', contractInfo.seller);
        $("#simpleSeller").textbox('setValue', contractInfo.simpleSeller);
        $("#simpleBuyer").textbox('setValue', contractInfo.simpleBuyer);
        $("#purchaseCode").textbox('setValue', contractInfo.purchaseCode);
        $("#adminReview").combobox('setValue', contractInfo.adminReview);
        $("#salesmanCode").combobox('setValue', contractInfo.salesmanCode);
        $("#businessclass").combobox('setValue', contractInfo.businessclass);
        $("#itemProName").textbox('setValue', contractInfo.itemProName);
        $("#createTableName").datebox('setValue', contractInfo.createTableName);
        $("#Organizer").textbox('setValue', contractInfo.Organizer);
        $("#startDate").datebox('setValue', contractInfo.startDate);
        $("#endDate").datebox('setValue', contractInfo.endDate);
        $("#status").val(contractInfo.status);
        $("#adminReviewNumber").val(contractInfo.adminReviewNumber);
        $("#createdate").val(contractInfo.createdate);
        loadInternalProdunt();
    }
}
//绑定combobox
function bindCombobox() {
  
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
            $("#sellercode").val(rowdata.code);
            loadProdunt();
            $('#htcplist').datagrid('reload');
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
            $("#buyercode").val(rowdata.code);
            loadProdunt();
            $('#htcplist').datagrid('reload');
        }
    });
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
            loadProdunt();
        }, 'json')
       
        $('#htcplist').datagrid('reload');

    })
    //买方(简称)文本框改变事件
    $("input", $("#simpleBuyer").next("span")).blur(function () {
        var simpleBuyer = ($("#simpleBuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
            $("#simpleBuyer").textbox('setValue', data.shortname);
            $("#buyer").combobox('setValue', data.name);
            $('#buyercode').attr('value', data.code);
            loadProdunt();
        }, 'json')
     
        $('#htcplist').datagrid('reload');

    })

}
