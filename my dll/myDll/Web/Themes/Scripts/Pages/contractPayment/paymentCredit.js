/*
form:根据收款户名、合同客户、销售员、销售组确定合同范围。

*/
var supplierJson = undefined; //供应商的JSON
var ttEditRow = undefined;
var subttEditRow = undefined;

var addOredit = undefined;
var browse = undefined;

$(document).ready(function () {
    $('#dlg').dialog('close');
    buildUI();

    var formlist = $("#pageTag").val();
    addOredit = $("#addOredit").val();
    browse = $("#browse").val();
    if (browse == 'true') {
        $("div.btabs").hide();
        $("#addpart").hide();
        $("#midbtns").hide();
    }
    if (addOredit == 'edit') {
        loadData();
    }
});
//初始化界面
var buildUI = function () {

    //获取集团内管理供应商json
    $.ajax({
        type: 'GET',
        url: '/ashx/Basedata/Supplier.ashx?action=GetSKInfo',
        async: false,
        cache: false,
        dataType: 'json',
        success: function (data) {
            supplierJson = data.rows;
        }
    });
    //收款户名
    $('#ACCOUNTSIMPLYNAME').textbox({
        onChange: function (value) {
            var accountSimplyName = ($("#ACCOUNTSIMPLYNAME").val());
            if (accountSimplyName.length > 1) {
                $.each(supplierJson, function (index, item) {
                    if (supplierJson[index].CODE.indexOf(accountSimplyName) >= 0) {
                        $('#ACCOUNTNAME').combobox('select', supplierJson[index].NAME);
                    }
                });
            }
        }
    });
    $("#ACCOUNTNAME").combobox({
        valueField: 'NAME',
        textField: 'NAME',
        editable: false,
        data: supplierJson,
        onChange: function (newValue, oldValue) {
            $.each(supplierJson, function (index, item) {
                if (supplierJson[index].NAME.indexOf(newValue) >= 0) {
                    $("#ACCOUNTSIMPLYNAME").textbox('setText', supplierJson[index].CODE);
                    $("#ACCOUNTSIMPLYNAME").textbox('setValue', supplierJson[index].CODE);
                    $('#BANKNAME').combobox({
                        valueField: 'ENGLISH',
                        textField: 'ENGLISH',
                        method: 'get',
                        url: '/ashx/Basedata/Supplier.ashx?action=GetSKBank&name=' + supplierJson[index].NAME,
                        editable: false
                    });
                }
            });
        }
    });

    //销售组
    $('#BUSINESSCLASS').combobox({
        onSelect: function (record) {
            ttLoadData();
        }
    });
    //合同表
    $("#tt").datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        toolbar: '#tb',
        //        pageList: [10, 15, 30],
        singleSelect: true,
        idField: 'CONTRACTNO',
        columns: [[
                    { field: 'CONTRACTNO', title: '合同编号', width: '100px', rowspan: 2 },
                    { field: 'CONTRACTAMOUNT', title: '合同总额', width: '100px', rowspan: 2, formatter: function (val) { return Number(val); } },
                    { title: '价格条款1', align: 'center', colspan: 2 },
                    { title: '价格条款2', align: 'center', colspan: 2 },
                    { field: 'PAIDAMOUNT', title: '信用证已收金额', width: '100px', rowspan: 2, formatter: function (val) { return Number(val); } },
                    { field: 'UNPAIDAMOUNT', title: '信用证未收金额', width: '100px', rowspan: 2, formatter: function (val) { return Number(val); } },
                    { field: 'CREDITAMOUNT', title: '本次收款', width: '100px', editor: { type: 'numberbox', options: { precision: 0} }, rowspan: 2 }
                ], [
                    { field: 'PRICEMENT1', title: '条款', width: '100px' },
                    { field: 'ITEM1AMOUNT', title: '金额', width: '100px', formatter: function (val) { return Number(val); } },
                    { field: 'PRICEMENT2', title: '条款', width: '100px' },
                    { field: 'ITEM2AMOUNT', title: '金额', width: '100px', formatter: function (val) { return Number(val); } }
                ]],
        pagination: false,
        onAfterEdit: function (rowIndex, rowData, changes) {
            ttEditRow = undefined;
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (ttEditRow != undefined) {
                $("#tt").datagrid('endEdit', ttEditRow);
            }
            if (ttEditRow == undefined) {
                $("#tt").datagrid('beginEdit', rowIndex);
                ttEditRow = rowIndex;
            }
        },
        onClickRow: function (rowIndex, rowData) {
            if (ttEditRow != undefined) {
                $("#tt").datagrid('endEdit', ttEditRow);
                ttEditRow = undefined;
            } else {
                var contractNo = rowData["CONTRACTNO"];
                var payamount = rowData["CREDITAMOUNT"];
                bindSubTable(contractNo, payamount);
            }
        }
    });
    //采购合同表
    $("#subtt").datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        idField: 'contractNo',
        columns: [[
                    { field: 'PURCHASECODE', title: '销售合同号', width: '100px' },
                    { field: 'CONTRACTNO', title: '采购合同号', width: '100px' },
                    { field: 'CONTRACTAMOUNT', title: '合同额', width: '100px' },
                    { field: 'PAYAMOUNT', title: '金额', width: '100px', editor: { type: 'numberbox', options: { precision: 0}} }
                ]],
        pagination: false,
        onAfterEdit: function (rowIndex, rowData, changes) {
            subttEditRow = undefined;
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (subttEditRow != undefined) {
                $("#subtt").datagrid('endEdit', subttEditRow);
            }
            if (subttEditRow == undefined) {
                $("#subtt").datagrid('beginEdit', rowIndex);
                subttEditRow = rowIndex;
            }
        },
        onClickRow: function (rowIndex, rowData) {
            if (subttEditRow != undefined) {
                $("#subtt").datagrid('endEdit', subttEditRow);
                subttEditRow = undefined;
            }
        }
    });

    $("#ttchoose").datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: false,
        idField: 'CONTRACTNO',
        columns: [[
                    { field: 'CONTRACTNO', title: '合同编号', width: '100px', rowspan: 2 },
                    { field: 'CONTRACTAMOUNT', title: '合同总额', width: '100px', rowspan: 2 },
                    { title: '价格条款1', align: 'center', colspan: 2 },
                    { title: '价格条款2', align: 'center', colspan: 2 },
                    { field: 'PAIDAMOUNT', title: '信用证已收金额', width: '80px', rowspan: 2 },
                    { field: 'UNPAIDAMOUNT', title: '信用证未收金额', width: '100px', rowspan: 2 },
                    { field: 'CREDITAMOUNT', title: '本次收款', width: '100px', rowspan: 2 }
                ], [
                    { field: 'PRICEMENT1', title: '条款', width: '100px' },
                    { field: 'ITEM1AMOUNT', title: '金额', width: '100px' },
                    { field: 'PRICEMENT2', title: '条款', width: '100px' },
                    { field: 'ITEM2AMOUNT', title: '金额', width: '100px' },
                ]],
        pagination: true
    });

    //弹出框处理
    $('#dlg').dialog({
        //        width: 600,
        //        height: document.documentElement.clientHeight,
        closed: true,
        cache: false,
        modal: true,
        buttons: [{
            text: '选择',
            handler: function () {
                $('#dd').dialog('close');
                //把选择的产品加载到当前页面
                var rows = $('#ttchoose').datagrid('getSelections');

                var oldrows = $('#tt').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    //判断当前表格里面是否有pname,同一个pcode下可以有多个pname
                    var isexists = false;
                    for (var j = 0; j < oldrows.length; j++) {
                        if (oldrows[j].CONTRACTNO == rows[i].CONTRACTNO) {
                            isexists = true;
                            break;
                        }
                    }
                    if (isexists == false) {
                        var row = rows[i];
                        var newrow = {};
                        newrow.CONTRACTNO = row.CONTRACTNO;
                        newrow.CONTRACTAMOUNT = row.CONTRACTAMOUNT;
                        newrow.PRICEMENT1 = row.PRICEMENT1;
                        newrow.PRICEMENT2 = row.PRICEMENT2;
                        newrow.ITEM1AMOUNT = row.ITEM1AMOUNT;
                        newrow.ITEM2AMOUNT = row.ITEM2AMOUNT;
                        newrow.PAIDAMOUNT = row.PAIDAMOUNT;
                        newrow.UNPAIDAMOUNT = row.UNPAIDAMOUNT;
                        newrow.CREDITAMOUNT = row.CREDITAMOUNT;

                        $('#tt').datagrid('appendRow', newrow);
                    }
                }
                $("#dlg").dialog('close');
                $('#ttchoose').datagrid('clearSelections');
            }
        }, {
            text: '取消',
            handler: function () {
                $("#dlg").dialog('close');
            }
        }]
    });
};

function loadData() {
    $('#form1').form('load', '/ashx/ContractPayment/paymentCredit.ashx?action=GetPayRecieve&payNo=' + $("#payNo").val());
    $('#tt').datagrid({
        url: '/ashx/ContractPayment/paymentCredit.ashx?action=GetSubTable&payNo=' + $("#payNo").val() + "&money=CREDITAMOUNT=0"
    });
}

function select() {
    var seller = $("#ACCOUNTNAME").combobox("getText");
    var buyer = $("#BUYER").combobox("getText");
    var saleman = $("#SALEMAN").textbox('getText');
    var bclass = $("#BUSINESSCLASS").combobox("getText");
    if (seller == '' || buyer == '' || saleman == '' || bclass == '') {
        alert("请将收款户名，合同客户，销售业务员和销售组信息填写完整！");
        return;
    }
    ttLoadData();
    $('#dlg').dialog('open');
}
function ttLoadData() {
    var payAmount = $("#PAYAMOUNT").val();
    if (payAmount == '') {
        alert("请先设置信用保证金金额！");
    }
    $('#ttchoose').datagrid({
        url: '/ashx/ContractPayment/paymentCredit.ashx?action=GetContractBS',
        queryParams: {
            buyer: $('#BUYER').combobox('getText'),
            seller: $('#ACCOUNTNAME').combobox('getText'),
            businessclass: $('#BUSINESSCLASS').combobox('getText'),
            saleman: $("#SALEMAN").val(),
            money: 'CREDITAMOUNT=' + payAmount
        }
    });
}
function bindSubTable(contractNo, payAmount) {
    $('#subtt').datagrid({
        url: '/ashx/ContractPayment/paymentCredit.ashx?action=GetBuyContract',
        queryParams: {
            contractno: contractNo, //合同号
            payamount: payAmount, //金额
            collumn: 'PAYAMOUNT'//列名
        }
    });
}

//获取子表数据
function getSubTable() {
    var ttdatagrid = $("#tt").datagrid("getRows");
    var ttdatagridjson = JSON.stringify(ttdatagrid);
    $("#ttdatagrid").val(ttdatagridjson);

    var subttdatagrid = $("#subtt").datagrid("getRows");
    var subttdatagridjson = JSON.stringify(subttdatagrid);
    $("#subttdatagrid").val(subttdatagridjson);
}

//验证合同收款和信用证金额的关系
function checkCreditAmount(type) {
    var rows = $("#tt").datagrid("getRows");
    var amount = 0;
    var creditamount = parseFloat($("#PAYAMOUNT").numberbox("getValue"));

    var flag = false; //检查本次付款金额大于未付款金额
    var rowindex = 0;
    $.each(rows, function (index, row) {
        amount += parseFloat(row.CREDITAMOUNT);
        if (row.UNPAIDAMOUNT < row.CREDITAMOUNT) {
            flag = true;
            rowindex = index;
        }
    });
    if (flag == true) {
        $.messager.confirm('警告', (rowindex + 1) + '行本次付款金额大于未付款金额，同意该操作吗？', function (r) {
            if (r) flag = false;
            else flag = true ;
        });
    }
    if (flag == true) {return;}

    if (amount > creditamount) {
        $.messager.confirm('警告', '付款金额比中信保金额多' + (amount - creditamount) + ',同意该操作吗？', function (r) {
            if (r && type == 'save') saveItem();
            if (r && type == 'commit') commitItem();
        });

    } else if (amount < creditamount) {
        return $.messager.confirm('警告', '付款金额比中信保金额少' + (creditamount - amount) + ',同意该操作吗？', function (r) {
            if (r && type == 'save') saveItem();
            if (r && type == 'commit') commitItem();
        });
    }
    else {
        if (type == 'save') {
            saveItem();
        }
        else {
            commitItem();
        }
    }
}
//form全部保存
function save() {
    //接受更改
    if (ttEditRow != undefined) {
        $("#tt").datagrid('endEdit', ttEditRow);
        ttEditRow = undefined;
    }
    checkCreditAmount('save');
}
function saveItem() {
    getSubTable();
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/ContractPayment/paymentCredit.ashx?action=add&status=新建&type=' + addOredit,
        data: $('#form1').serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {

        },
        success: function (data) {
            if (data.sucess == "1") {
                $.messager.alert('系统提示', '保存成功!', 'info', function () {
                    //打开指定模板页面
                    window.top.selectTab('信用证开立');
                });
            }
            else {
                $.messager.alert("提醒", data.errormsg);
            }
        }
    });
}
//全部提交
function commit() {
    //接受更改
    if (ttEditRow != undefined) {
        $("#tt").datagrid('endEdit', ttEditRow);
        ttEditRow = undefined;
    }
    checkCreditAmount('commit');
}
function commitItem() {
    getSubTable();
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/ContractPayment/paymentCredit.ashx?action=add&status=提交&type=' + addOredit,
        data: $('#form1').serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {

        },
        success: function (data) {
            if (data.sucess == "1") {
                $.messager.alert('提示', '提交成功!', 'info', function () {
                    //打开指定模板页面
                    window.top.selectTab('信用证开立');
                });
            }
            else {
                $.messager.alert("提示", data.errormsg);
            }
        }
    });
}


//显示隐藏控制
function continueEdit() {
    $("#midbtns").toggle();
    $("#concreteInfo").toggle();
}
function undo() {
    //window.top.closeTab();//关闭标签
    $("#concreteInfo").toggle();
    $("#midbtns").toggle();
}
function closetab() {
    window.top.closeTab(); //关闭标签
}
