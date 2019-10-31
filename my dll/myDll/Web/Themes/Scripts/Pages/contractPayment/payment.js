
var suppliersJson = []; //供应json数据
var editRow = undefined;
//var type = '否';//列表中选择非关联合同的参数
$(document).ready(function () {
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //----初始化datagrid-----
        $('#tt').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'PAYNO',
            //url: '/ashx/ContractPayment/paymentLoadData.ashx?action=getList',
            columns: [[
            { field: 'ck', checkbox: true },
//            { field: 'S2STATUS', title: '状态', width: '80px', center: true, formatter: function (val) {
//                if (val == 0) return '保存';
//                if (val == 1) return '提交';
//                if (val == 2) return '作废';
//                else { return '未知'; }
//            }
//            },
//            { field: 'PARTINFO', title: '对应合同', width: '60px', center: true },
            { field: 'PARTINFO', title: '状态', width: '60px', center: true, formatter: function (val) {
                if (val == '是') return '已认领';
                if (val == '否') return '未认领';
            } },
            { field: 'BUSINESSTYPE', title: '业务类型', width: '80px' },
            { field: 'PAYNO', title: '收款号', width: '120px' },
            { field: 'ACCOUNTNAME', title: '收款户名', width: '120px' },
            { field: 'BANKNAME', title: '开户银行', width: '150px' },
            { field: 'PAYACCOUNT', title: '付款账户', width: '150px' },
            { field: 'PAYDATE', title: '汇入时间', width: '100px' },
            { field: 'PAYAMOUNT', title: '汇入金额', width: '80px',
                formatter: function (val, rowData, rowIndex) {
                    if (val != null)
                        return Number(val);
                }
            },
            { field: 'CURRENCY', title: '币种', width: '80px' },
            { field: 'CONTRACTCLIENT', title: '合同客户', width: '80px' },
            { field: 'SALEMAN', title: '业务员', width: '80px' },
            //            { field: 'PAYNO', title: '收款单号', width: '120px' },
            {field: 'CREATEMAN', title: '制单人', width: '80px' },
            { field: 'CREATEDATE', title: '制单日期', width: '80px' },
            { field: 'LASTMOD', title: '认领人', width: '80px' },
            { field: 'LASTMODDATE', title: '认领日期', width: '80px' },
            { field: 'REMARK', title: '备注', width: '150px' }
            ]],
            pagination: true
        });

        $("#dd").dialog({
            title: '境外到款-新增',
            width: document.documentElement.clientWidth,
            height: document.documentElement.clientHeight,
            closed: true,
            cache: false,
            modal: true,
            maximizable: true,
            buttons: [{
                text: '确定',
                iconCls: 'icon-ok',
                handler: function () {
                    submitForm();
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $("#dd").dialog('close');
                }
            }]
        });
        //根据日期限制搜索数据
        SearchData();
    }

    if (pageTag == "form") {
        $('#dlg').dialog({
            closed: true
        });

        //数据绑定
        loadTotalName();//`简称加载全称
        $("#maintable").find("input").attr("readonly", true);
        bindUI();

        if ($("#action").val() == 'edit') {
            $('#maintable').form('load', htdata);
        }
        //设置所有的输入项为只读
        if (isBrowse == "True" || $("#PARTINFO").val() == '是') {
            //页面展示设置
            continueEdit();
            //$('#maintable').form('load', htdata);
            //向销售合同表绑定数据
            $("#tt_subTable").datagrid({
                url: '/ashx/ContractPayment/paymentLoadData.ashx?action=getSubList&payNo=' + $("#payNo").textbox("getText")
            });
        }
        if (isBrowse == "True") {
            $("#concreteInfo").toggle();
            $('input').attr("readonly", true);
            $("#remark").attr("readonly", true);
            $("#bottombtns").toggle();
        }
        continueEdit();
    }
});
//列表查询操作
function SearchData() {
    var beginTime = $("#beginTime").datebox("getText");
    var endTime = $("#endTime").datebox("getText");
    var contractClient = $("#contractClient").val();
    var saleman = $("#saleman").val();
    var partInfo = $('#partInfo').val(); //$('input:radio[name="partInfo"]:checked').val();

    para = {};
    para.saleman = saleman;
    para.contractClient = contractClient;
    para.partInfo = partInfo;
    para.beginTime = beginTime;
    para.endTime = endTime;
    $("#tt").datagrid({ url: '/ashx/ContractPayment/paymentLoadData.ashx?action=getList' });
    $("#tt").datagrid('load', para);
}

//列表添加
function add() {
    //window.top.addNewTab('合同到款-添加', '/Bus/ContractPayment/paymentForm.aspx?action=add', '');
    window.top.addNewTab('现汇到款-新增', '/Bus/ContractPayment/PayAddSwitch.aspx', '');
    //----初始化datagrid-----

}
//列表修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row.S2STATUS != 0 && row.PARTINFO === '是') {
        $.messager.alert("警告", "只有是保存状态才能修改！");
        return;
    }
    if (row != undefined) {
        no = row.PAYNO;
        //no = row.PAYACCOUNT;
        var type = row.BUSINESSTYPE;
        var pos = row.BUSINESSTYPE.indexOf('信用证');
        if (pos == -1) {
            window.top.addNewTab('认领到款', '/Bus/ContractPayment/paymentForm.aspx?action=edit&payNo=' + no + '&PARTINFO=' + row.PARTINFO, '');
        } else {
            window.top.addNewTab('信用证到款-修改', '/Bus/ContractPayment/PayCreditCash.aspx?action=edit&payNo=' + no, '');
        }

    } else {
        $.messager.alert('提示', '请选择要修改的数据！', 'info');
    }
}
//列表删除
function del() {

    //if (row.S2STATUS != 0) {
        $.messager.alert("警告", "为保证数据一致性，不能删除任何数据！");
        return;
    //}
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.PAYNO;
        $.messager.confirm("提示", "确定要删除吗？", function (r) {
            if (r) {
                $.post("/ashx/ContractPayment/paymentOperator.ashx?module=del", { payNo: no }, function (data) {
                    if (data.sucess == "1") {
                        $.messager.alert("提示", "删除成功");
                        $('#tt').datagrid("reload");
                    }
                    else {
                        $.messager.alert("提示", "删除失败");
                    }
                }, 'json');

            }
        })

    } else {
        $.messager.alert('提示', '请选择要删除的数据！', 'info');
    }
}
//列表查看
function browse() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.PAYNO;

        var pos = row.BUSINESSTYPE.indexOf('信用证');
        if (pos == -1) {
            window.top.addNewTab('合同到款-查看', '/Bus/ContractPayment/paymentForm.aspx?action=edit&payNo=' + no + "&browse=true", '');
        } else {
            window.top.addNewTab('信用证到款-查看', '/Bus/ContractPayment/PayCreditCash.aspx?action=edit&payNo=' + no + "&browse=true", '');
        }
    } else {
        $.messager.alert('提示', '请选择要查看的数据！', 'info');
    }
}

//提交form表单
function submitForm() {
    $("#ddframe")[0].contentWindow.getSubTable();
    var frame = $(window.frames["ddframe"].document).find("#form1");
    var form = window.frames["ddframe"].document.getElementById("form1");
    frame.form('submit', {
        url: "/ashx/PaymentManage/PaymentAbroadHandler.ashx?action=add",
        onSubmit: function () {
            //进行表单验证 
            return window.frames["ddframe"].valiForm();
        },
        success: function (data) {
            alert(data)
        }
    });

}
//校验form
function valiForm() {
    return $("#form1").form('validate');
}
//获取子表数据
function getSubTable() {
    $("#tt_subTable").datagrid('acceptChanges');
    var datagrid = $("#tt_subTable").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}
//查找按钮筛选合同
function SearchContract() {
    var queryData = {};
    queryData.contractCode = $('#contractCode').textbox('getValue');
    queryData.payerCode = $('#payerCode').textbox('getValue');
    $('#abroadlist').datagrid({ queryParams: queryData });

}


//数据绑定
var bindUI = function () {
    //绑定业务员编号
    $("#saleman").combobox({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',
        valueField: 'UserRealName',
        textField: 'UserRealName',
        editable: false,
        onSelect: function (record) {
        }

    });
    //货币
    $('#currency').combobox({
        url: '/ashx/Basedata/DictronaryHandler.ashx?action=GetDicByParentID&pid=22',
        valueField: 'id',
        textField: 'text',
        panelHeight: 'auto'
    });
    $('#currency').combobox('setValue', htdata.currency);

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
    ////收款户名
    //$('#accountSimplyName').textbox({
    //    onChange: function (value) {
    //        var accountSimplyName = ($("#accountSimplyName").val());
    //        if (accountSimplyName.length > 1) {
    //            $.each(supplierJson, function (index, item) {
    //                if (supplierJson[index].CODE.indexOf(accountSimplyName) >= 0) {
    //                    $('#accountName').combobox('select', supplierJson[index].NAME);
    //                }
    //            });
    //        }
    //    }
    //});
    //$("#accountName").combobox({
    //    valueField: 'NAME',
    //    textField: 'NAME',
    //    editable: false,
    //    panelHeight:'auto',
    //    data: supplierJson,
    //    onChange: function (newValue, oldValue) {
    //        $.each(supplierJson, function (index, item) {
    //            if (supplierJson[index].NAME.indexOf(newValue) >= 0) {
    //                $("#accountSimplyName").textbox('setText', supplierJson[index].CODE);
    //                $("#accountSimplyName").textbox('setValue', supplierJson[index].CODE);
    //                $('#bankName').combobox({
    //                    valueField: 'ENGLISH',
    //                    textField: 'ENGLISH',
    //                    panelHeight: 'auto',
    //                    method: 'get',
    //                    url: '/ashx/Basedata/Supplier.ashx?action=GetSKBank&name=' + supplierJson[index].NAME,
    //                    editable: false
    //                });
    //            }
    //        });
    //    }
    //});
    $("#accountName").combobox({
        valueField: 'NAME',
        textField: 'NAME',
        editable: false,
        panelHeight: 'auto',
        data: supplierJson,
        onChange: function (newValue, oldValue) {
            $.each(supplierJson, function (index, item) {
                if (supplierJson[index].NAME.indexOf(newValue) >= 0) {
                    //$("#accountSimplyName").textbox('setText', supplierJson[index].CODE);
                    //$("#accountSimplyName").textbox('setValue', supplierJson[index].CODE);
                    $('#bankName').combobox({
                        valueField: 'ENGLISH',
                        textField: 'ENGLISH',
                        panelHeight: 'auto',
                        method: 'get',
                        url: '/ashx/Basedata/Supplier.ashx?action=GetSKBank&name=' + supplierJson[index].NAME,
                        editable: false,
                        onSelect: function (record) {//根据所选银行加载收款银行账户
                            $('#revBankAccount').combobox({
                                valueField: 'CODE',
                                textField: 'CODE',
                                panelHeight: 'auto',
                                method: 'get',
                                url: '/ashx/Contract/loadCombobox.ashx?module=getBankAccount&bankName=' + record.ENGLISH,
                                editable: false,
                                onSelect: function (record) {
                                    $("#revAccountType").textbox('setValue', record.RUSSIAN);
                                }
                            })
                        }
                    });
                }
            });
        }
    });

    //合同客户
    $('#contractClient').combobox({
        required: true,
        valueField: 'shortname',
        textField: 'name',
        editable: false,
        data: sbCustomer,
        onSelect: function (index, rowdata) {
            //加载销售合同
            saleLoad();
        }

    });
    $('#contractClient').combobox('setValue', htdata.contractClient);
//    //销售业务员
//    $('#saleman').combogrid({
//        required: true,
//        valueField: 'name',
//        textField: 'name',
//        editable: false,
//        data: sbSaleman
//    });
//    $('#saleman').combobox('setValue', htdata.saleman);
    //付款金额限制
    $("#payamount").numberbox({
        precision: 0,
        onChange: function () {


        }
    })
}
var purchaseLoad = function (saleContractNo, saleAmount) {

    var accountName = $('#accountName').combobox('getValue');

    $('#tt_purchase').datagrid({
        url: '/ashx/ContractPayment/paymentLoadData.ashx?action=getPurchaseCode&saleContractNo=' + saleContractNo
    });
}
var saleLoad = function () {
    //获得选择的客户名和收款户名
    var accountName = $('#accountName').combobox('getValue');
    var contractClient = $('#contractClient').combobox('getValue');
    //根据选择的合同客户加载合同详细信息
    var indexRow = undefined;

    $('#tt_subTable').datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        toolbar: '#tb',
        singleSelect: true,
        idField: 'contractNo',
        columns: [[
                    { field: 'CONTRACTNO', title: '合同编号', width: '120px', rowspan: 2 },
                    { field: 'CONTRACTAMOUNT', title: '合同总额', width: '100px', rowspan: 2 },
                    { title: '价格条款1', align: 'center', colspan: 2 },
                    { title: '价格条款2', align: 'center', colspan: 2 },
                    { field: 'PAIDAMOUNT', title: '已收金额', width: '100px', rowspan: 2 },
                    { field: 'UNPAIDAMOUNT', title: '未收金额', width: '100px', rowspan: 2 },
                    { field: 'PAYINGAMOUNT', title: '本次收款', width: '100px', editor: { type: 'numberbox', options: { precision: 2 } }, rowspan: 2 },
                    { field: 'CHARGINGAMOUNT', title: '扣费金额', width: '100px', editor: { type: 'numberbox', options: { precision: 2 } }, rowspan: 2 }
                ], [
                    { field: 'PRICEMENT1', title: '条款', width: '100px' },
                    { field: 'ITEM1AMOUNT', title: '金额', width: '100px' },
                    { field: 'PRICEMENT2', title: '条款', width: '100px' },
                    { field: 'ITEM2AMOUNT', title: '金额', width: '100px' }
                ]],
        pagination: false,
        onAfterEdit: function (rowIndex, rowData, changes) {
            editRow = undefined;
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);
            }
            if (editRow == undefined) {
                $("#tt_subTable").datagrid('beginEdit', rowIndex);
                editRow = rowIndex;
            }
        },
        onClickRow: function (rowIndex, rowData) {
            //选中的时候加载这个合同的卖方的采购合同号

            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', rowIndex);
                editRow = undefined;
            } else {
                var contractNo = rowData.contractNo;
                var saleAmount = rowData.contractAmount;
                //purchaseLoad(contractNo, saleAmount);
            }
        }
    });
}

//合同选择表
function select() {
    //验证信息
    var error = "";
    if ($("#accountName").combobox("getText").length == 0) {
        error += "收款户名";
    }
    if ($("#saleman").combobox("getText").length == 0) {
        error += " 发货人";
    }
    if ($("#contractClient").combobox("getText").length == 0) {
        error += " 合同客户";
    }
    if (error.length > 0) {
        $.messager.alert("警告", "请确认已选择" + error);
        return;
    }

    ttLoadData();
    $('#dlg').dialog('open');
}
function ttLoadData() {
    var accountName = $('#accountName').combobox('getText');
    var contractClient = $('#contractClient').combobox('getText');
    var saleman = $("#saleman").combobox("getText");

    $("#ttchoose").datagrid({
        url: '/ashx/ContractPayment/paymentLoadData.ashx?action=getContactCode&accountName=' + accountName + '&contractClient=' + contractClient + '&saleman=' + saleman
    });
}


//验证必填项
function validTheForm() {
    var error = "";
    if ($("#accountName").combobox("getText").length == 0) { error += "收款户名 "; }
    if ($("#bankName").textbox("getText").length == 0) { error += "开户银行 "; }
    if ($("#payAccount").textbox("getText").length == 0) { error += "对方账户 "; }
    if ($("#paydate").datebox("getText").length == 0) { error += "汇入时间 "; }
    if ($("#currency").combobox("getText").length == 0) { error += "币种 "; }
    if ($("#payamount").textbox("getText").length == 0) { error += "汇入金额 "; }

    if (error.length != 0) {
        alert(error + "为必填项！");
        return false;
    } else {
        return true;
    }
}
//部分保存
function savepart() {
    //getSubTable();
    if (validTheForm() == false) {
        return;
    }
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/ContractPayment/paymentOperator.ashx?module=AddPart&payNo=' + payNo + '&action=' + action + "&part=part",
        data: $('#form1').serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {

        },
        success: function (data) {
            if (data.sucess == "1") {
                $.messager.alert('系统提示', '保存成功!', 'info', function () {
                    //打开指定模板页面
                    window.top.selectAndRefreshTab('现汇收款');
                });
            }
            else {
                $.messager.alert("提醒", data.errormsg);
            }
        }
    });
}
//部分提交
function commitpart() {
    //getSubTable();
    if (validTheForm() == false) {
        return;
    }
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/ContractPayment/paymentOperator.ashx?module=AddPart&payNo=' + payNo + '&action=' + action + "&part=whole",
        data: $('#form1').serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {

        },
        success: function (data) {
            if (data.sucess == "1") {
                $.messager.alert('系统提示', '保存成功!', 'info', function () {
                    window.top.selectAndRefreshTab('现汇收款');
                });
            }
            else {
                $.messager.alert("提醒", data.errormsg);
            }
        }
    });
}
//全部保存
function saveall() {
    if (editRow != undefined) {
        $("#tt_subTable").datagrid('endEdit', editRow);
        editRow = undefined;
    }
    if (validTheForm() == false) {
        return;
    }
    getSubTable();
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/ContractPayment/paymentOperator.ashx?module=add&payNo=' + payNo + '&action=' + action + "&part=part",
        data: $('#form1').serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {

        },
        success: function (data) {
            if (data.sucess == "1") {
                $.messager.alert('提示', '保存成功!', 'info', function () {
                    //打开指定模板页面
                    window.top.selectAndRefreshTab('现汇收款');
                });
            }
            else {
                $.messager.alert("提示", data.errormsg);
            }

        }
    });
}
//全部提交
function commitall() {
    if (editRow != undefined) {
        $("#tt_subTable").datagrid('endEdit', editRow);
    }
    if (validTheForm() == false) {
        return;
    }
    var options = $('#tt_subTable').datagrid('getRows');
    if (options == 0) {
        $.messager.alert("警告", "收款没有对应相应的合同");
        return;
    }
    getSubTable();
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/ContractPayment/paymentOperator.ashx?module=add&payNo=' + payNo + '&action=' + action + "&part=whole",
        data: $('#form1').serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {

        },
        success: function (data) {

            if (data.sucess == "1") {
                $.messager.alert('提示', '保存成功!', 'info', function () {
                    //打开指定模板页面
                    window.top.selectAndRefreshTab('现汇收款');
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

    //加载销售合同
    saleLoad();
    $('#tt_purchase').datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        idField: 'contractNo',
        columns: [[
                 { field: 'saleContractNo', title: '销售合同号', width: '100px', formatter: function (value, row, index) {
                     return saleContractNo;
                 }
                 },
                { field: 'contractNo', title: '采购合同号', width: '100px' },
                { field: 'contractAmount', title: '合同额', width: '100px' },
                { field: 'saleAmount', title: '金额', width: '100px', formatter: function (value, row, index) {
                    return saleAmount;
                }
                }
            ]],
        pagination: true,
        toolbar: toolbar,
        onAfterEdit: function (rowIndex, rowData, changes) {
            editRow = undefined;
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_purchase").datagrid('endEdit', editRow);
            }

            if (editRow == undefined) {
                $("#tt_purchase").datagrid('beginEdit', rowIndex);
                editRow = rowIndex;
            }
        },
        onClickRow: function (rowIndex, rowData) {
            if (editRow != undefined) {
                $("#tt_purchase").datagrid('endEdit', editRow);

            }
        }
    });
    $("#ttchoose").datagrid({
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        idField: 'contractNo',
        columns: [[
                    { field: 'contractNo', title: '合同编号', width: '120px', rowspan: 2 },
                    { field: 'contractAmount', title: '合同总额', width: '100px', rowspan: 2 },
                    { title: '价格条款1', align: 'center', colspan: 2 },
                    { title: '价格条款2', align: 'center', colspan: 2 },
                    { field: 'paidAmount', title: '已收金额', width: '100px', rowspan: 2 },
                    { field: 'unpaidAmount', title: '未收金额', width: '100px', rowspan: 2 },
                    { field: 'payingAmount', title: '本次付款', width: '100px', rowspan: 2 },
                ], [
                    { field: 'pricement1', title: '条款', width: '100px' },
                    { field: 'item1Amount', title: '金额', width: '100px' },
                    { field: 'pricement2', title: '条款', width: '100px' },
                    { field: 'item2Amount', title: '金额', width: '100px' }
                ]],
        pagination: true
    });
    //弹出框处理
    $('#dlg').dialog({
        closed: true,
        cache: false,
        modal: true,
        buttons: [{
            text: '选择',
            handler: function () {
                $('#dd').dialog('close');
                //把选择的产品加载到当前页面
                var rows = $('#ttchoose').datagrid('getSelections');
                var oldrows = $('#tt_subTable').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    //判断当前表格里面是否有pname,同一个pcode下可以有多个pname
                    var isexists = false;
                    for (var j = 0; j < oldrows.length; j++) {
                        if (oldrows[j].CONTRACTNO == rows[i].contractNo) {
                            isexists = true;
                            break;
                        }
                    }
                    if (isexists == false) {
                        var row = rows[i];
                        var newrow = {};
                        newrow.CONTRACTNO = row.contractNo;
                        newrow.CONTRACTAMOUNT = row.contractAmount;
                        newrow.ITEM1AMOUNT = row.item1Amount;
                        newrow.ITEM2AMOUNT = row.item2Amount;
                        newrow.PAIDAMOUNT = row.paidAmount;
                        newrow.UNPAIDAMOUNT = row.unpaidAmount;
                        newrow.PAYINGAMOUNT = row.payingAmount;
                        newrow.PRICEMENT1 = row.pricement1;
                        newrow.PRICEMENT2 = row.pricement2;
                        newrow.PAYINGAMOUNT = row.payingAmount;
                        newrow.CHARGINGAMOUNT = 0;
                        $('#tt_subTable').datagrid('appendRow', newrow);
                    }
                }
                $('#ttchoose').datagrid('clearSelections');

                $("#dlg").dialog('close');
            }
        }, {
            text: '取消',
            handler: function () {
                $("#dlg").dialog('close');
            }
        }]
    });
}
function undo() {
    //window.top.closeTab();//关闭标签
    $("#concreteInfo").toggle();
    $("#midbtns").toggle();
}
function closetab() {
    window.top.closeTab(); //关闭标签
}
//根据卖方买方简称加载全称和地址
function loadTotalName() {
    $("input", $("#simpleContractClient").next("span")).blur(function () {
        var simpleBuyer = ($("#simpleContractClient").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
            $("#contractClient").combobox('setValue', data.name);
        }, 'json')

    })
}