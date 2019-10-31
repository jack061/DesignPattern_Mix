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
////返回
var back = function () {

    window.top.closeAddTab("合同-新增", "/Bus/Contract/ContractTest.aspx");
}
//保存
var save = function () {
    var rrdata = SaveDataToDB(0);

    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        if (flowdirection == exportFlow) {
            window.top.selectAndRefreshTab('出口合同');
        }
        else {
            window.top.selectAndRefreshTab('进口合同');
        }
        

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
            if (flowdirection == exportFlow) {
                window.top.selectAndRefreshTab('出口合同');
            }
            else {
                window.top.selectAndRefreshTab('进口合同');
            }
        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
};
var SaveDataToDB = function (status) {
  
    var retdata = {};
    //获取list数据到htcplistStr
    $("#htcplist").datagrid("acceptChanges");
    var cplist = $("#htcplist").datagrid("getRows");
    var datagridjson = JSON.stringify(cplist);
    $('#htcplistStr').val(datagridjson);
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/Contract/contractOperater.ashx?module=addOutSideContractTest&status='+status+'&isEdit='+isEdit,
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
$(document).ready(function () {
    $("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadApp&contractNo='+contractNo);
    //绑定combobox 数据
    bindCombobox();
    //弹出商品选择
    popProductSelect();
    //根据买方卖方简称加载全称
    loadTotalName();
    //绑定默认数据
    bindUI();
    //绑定模板
    var templateno = $("#templateno").val();
    bindTemplate(templateno);
    //tabs选中事件
    selectTabs();
 
});
var bindUI = function () {
    initFile(); //初始化文件路径
    checkAttachUp();//查看上传文件
   
    //说明是新建合同,或者复制合同
    if (isnew == "True") {
        $('#contractNo').textbox('setValue', '自动编号');
        $('#contractNo').attr('readonly', true);
        //默认签订地点，唛头
        $("#shippingmark").textbox('setValue', 'N/M');
        $("#signedplace").textbox('setValue', '乌鲁木齐');
    }
    else {
        $('#contractNo').textbox('setValue', contractNo);
        $('#contractNo').attr('readonly', true);
    }
    
    //说明为创建框架合同
    if (isFrame == "true") {
        $("input[name='frameContract'][value='是']").attr("checked", true);
    }
    else {
        $("input[name='frameContract'][value='否']").attr("checked", true);
    }
    //说明为查看
    if (isBrowse == "true") {
        $('input').attr('disabled', true);
        $("#aa123").css("display", "none");
    }
    //价格条款1为100时默认价格条款2不可编辑
    var v = $("#pricement1per").numberbox('getValue');
    if (parseInt(v) == 100) {

        $("#pricement2").combobox('setValue', "");
        $('#pricement2').combobox('disable', true);
        $('#pricement2per').numberbox('disable', true);
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
//获取子表数据
function getSubTable() {
    var datagrid = $("#tt_subTable").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}

//价格条款1：默认电汇，100%；默认价格条款2不可编辑，只有价格条款1小于100%时可编辑；价格条款1为必填，如不足100%，价格条款2比调
function priceTermsChange() {
    //说明为新增
    if (htdata.pricement1 == "") {
        $("#pricement1").combobox('setValue', '电汇');
        $('#pricement1per').numberbox('setValue', '100%');
        $('#pricement2').combobox('disable', true);
        $('#pricement2per').combobox('disable', true);
    }

}

function setEditing(rowIndex) {
    var editors = $('#tt').datagrid('getEditors', rowIndex);
    var priceEditor = editors[0];
    var amountEditor = editors[1];
    var costEditor = editors[2];
    priceEditor.target.bind('change', function () {
        calculate();
    });
    amountEditor.target.bind('change', function () {
        calculate();
    });
    function calculate() {
        var cost = priceEditor.target.val() * amountEditor.target.val();
        $(costEditor.target).numberbox('setValue', cost);
    }
}
//文件上传
function upload() {
    $("#form1").ajaxSubmit({
        url: "/ashx/Contract/contractOperater.ashx?module=uploadExportFile",
        type: "post",
        dataType:"text",
        success: function (data) {
            if (data == "error") {
                $.messager.alert("错误：", "上传失败");
            }
            else {
                alert("上传成功");
                   //上传成功清除文件
                   $("#file1").val("");
                    var str = data.split(':');
                    $("#upMationDetails").textbox('setText', str[1]);
                    $("#upMationDetails").textbox('setValue', str[0]);
                    $("#outsideText").val(data);
                  
            }
        }
    })

}
//点击查看文件
function checkAttachUp() {
    $('#upMationDetails').textbox({
        onClickButton: function () {
            var filepath = $("#upMationDetails").textbox('getValue');
            window.top.addNewTab("查看", filepath, '');
        }
    });
}
//判断条件过滤
function chooseCondition() {
   
    $("#flowdirection").textbox('setValue', flowdirection);
    if (isFrame == "true") {
        $("input[name='frameContract'][value='是']").attr("checked", true);
    }
    else {
        $("input[name='frameContract'][value='否']").attr("checked", true);
    }
}
//绑定combobox
function bindCombobox() {
    //溢出率限定事件
    $("#overspill").numberbox({
        min: 0,
        max: 100,
        precision: 0,
        suffix: '%',
        onChange: function () {

        }
    })
    //价格有效期限定事件
    $("#pvalidity").numberbox({
        min: 0,
        max: 100,
        precision: 0,
        suffix: '天',
        onChange: function () {

        }
    })
    ////绑定买方简称
    //$("#simpleBuyer").combobox({
    //    url: '/ashx/Contract/loadCombobox.ashx?module=simplebuyer',

    //    valueField: 'shortname',
    //    textField: 'shortname',
    //    editable: false,
    //    onSelect: function (record) {
    //        var simpleBuyer = record.shortname;
    //        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
    //            $("#buyer").combobox('setValue', data.name);
    //            $("#buyeraddress").textbox('setValue', data.address);
    //            $('#buyercode').attr('value', data.code);

    //        }, 'json')

    //    }

    //});
    ////绑定卖方简称
    //$("#simpleSeller").combobox({
    //    url: '/ashx/Contract/loadCombobox.ashx?module=simpleseller',

    //    valueField: 'shortname',
    //    textField: 'shortname',
    //    editable: false,
    //    onSelect: function (record) {
    //        var simpleSeller = record.shortname;
    //        $.post("/ashx/Contract/loadPeople.ashx?module=seller", { simpleSeller: simpleSeller }, function (data) {
    //            $("#seller").combobox('setValue', data.name);
    //            $("#selleraddress").textbox('setValue', data.address);
    //            $('#sellercode').attr('value', data.code);

    //        }, 'json')

    //    }

    //});
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
    //产品大类绑定
    $("#productCategory").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=product',
        valueField: 'productcategory',
        textField: 'productcategory',
        editable: false,
    });
    //销售组绑定
    $("#businessclass").combobox({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',
        valueField: 'Agency',
        textField: 'Agency',
        editable: false,
    });
    //发运条款绑定
    $('#shipment').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=shipment',

        valueField: 'code',
        textField: 'cname',
        editable: false,
    });
    $('#transport').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=transport',

        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    $('#currency').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=currency',

        valueField: 'code',
        textField: 'cname',
        editable: false,
        multiple: false,
        onSelect: function (record) {
            //更改产品表的货币名称
            $("#htcplist").datagrid('acceptChanges');
            var rows = $("#htcplist").datagrid('getRows');
            for (var i = 0; i < rows.length; i++) {
                $('#htcplist').datagrid('updateRow', {
                    index: i,
                    row: {
                        priceUnit: record.code,
                    }
                });


            }
        }

    });
    $('#tradement').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=tradement',

        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    $('#pricement1').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=pricement1',

        valueField: 'code',
        textField: 'cname',
        editable: false,

    });
    $('#pricement2').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=pricement2',

        valueField: 'code',
        textField: 'cname',
        editable: false,

    });

    $('#seller').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        idField: 'cname',
        textField: 'cname',
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
            $("#seller").combogrid('setValue', rowdata.name);
            $("#selleraddress").textbox('setValue', rowdata.address);
        }
    });
    $('#buyer').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=buyer',
        idField: 'code',
        textField: 'cname',
        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simpleBuyer").textbox('setValue', rowdata.shortname);
            $("#buyer").combogrid('setValue', rowdata.name);
            $("#buyercode").val(rowdata.code);
            $("#buyeraddress").textbox('setValue', rowdata.address);
        }
    });
    //模板编号列表
    $('#templatename').combogrid({
        panelWidth: 100,
        url: '/ashx/Contract/loadCombobox.ashx?module=exportTemplate',
        idField: 'templatename',
        textField: 'templatename',
        editable: false,
        columns: [[
         { field: 'templatename', title: '模板名称', width: 100 },
        ]],
        onSelect: function (index, rowdata) {
            $("#templateno").val(rowdata.templateno);
            bindTemplate(rowdata.templateno);
        }
    });

    $('#harborout').combogrid({
        panelWidth: 450,
        idField: 'name',
        textField: 'name',
        url: '/ashx/Contract/loadCombobox.ashx?module=harborout',
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
        ]],
        onSelect: function (index, rowdata) {
            $("#harboroutCountry").textbox('setValue', rowdata.country);
        }
    });
    $('#harborarrive').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=harborarrive',
        idField: 'name',
        textField: 'name',
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
        ]],
        onSelect: function (index, rowdata) {
            $("#harboroutarriveCountry").textbox('setValue', rowdata.country);
        }
    });
    $('#harborclear').combogrid({
        panelWidth: 450,
        idField: 'name',
        textField: 'name',
        url: '/ashx/Contract/loadCombobox.ashx?module=harborarrive',
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
        ]],
        onSelect: function (index, rowdata) {

        }
    });
    //价格百分比文本框改变事件
    $("#pricement1per").numberbox({

        min: 0,
        max: 100,
        precision: 0,
        suffix: '%',
        onChange: function () {
            //获取文本框中的值
            var v = $('#pricement1per').numberbox('getValue');
            //如果为100,则价格条款2和价格条款2百分比不可编辑
            if (parseInt(v) == 100) {
                $('#pricement2').combobox('disable', true);
                $('#pricement2per').numberbox('disable', true);
            }
            else {
                $("#pricement2").combobox({
                    disabled: false
                });
                $("#pricement2per").numberbox({
                    disabled: false
                });

            }
            $("#pricement2per").numberbox('setValue', parseFloat(100) - parseFloat(v));

        }
    })
    $("#pricement2per").numberbox({

        min: 0,
        max: 100,
        precision: 0,
        suffix: '%',
        onChange: function () {
            //获取文本框中的值
            var v = $('#pricement2per').numberbox('getValue');
            $("#pricement1per").numberbox('setValue', parseFloat(100) - parseFloat(v));

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
            $("#seller").combobox('setValue', data.name);
            $("#selleraddress").textbox('setValue', data.address);
            $('#sellercode').attr('value', data.code);

        }, 'json')

    })
    //买方(简称)文本框改变事件
    $("input", $("#simpleBuyer").next("span")).blur(function () {

        var simpleBuyer = ($("#simpleBuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
            $("#simpleBuyer").textbox('setValue', data.shortname);
            $("#buyer").combobox('setValue', data.name);
            $("#buyeraddress").textbox('setValue', data.address);
            $('#buyercode').attr('value', data.code);

        }, 'json')

    })

}

//弹出产品选择框,根据合同号加载产品
function popProductSelect() {
    $('#dd').dialog({
        title: '请选择产品',
        width: 600,
        height: document.documentElement.clientHeight,
        closed: true,
        cache: false,
        //href: 'Abroad_Product_Select.aspx',
        modal: true,
        buttons: [{
            text: '选择',
            handler: function () {
                $('#dd').dialog('close');
                //把选择的产品加载到当前页面
                var rows = $('#productlist').datagrid('getSelections');
                var oldrows = $('#htcplist').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    //判断当前表格里面是否有pname,同一个pcode下可以有多个pname
                    var isexists = false;
                    for (var j = 0; j < oldrows.length; j++) {
                        if (oldrows[j].pname == rows[i].pname) {
                            isexists = true;
                            break;
                        }
                    }
                    if (isexists == false) {
                        var row = rows[i];
                        var newrow = {};
                        newrow.pcode = row.pcode;
                        newrow.pname = row.pname;
                        newrow.packspec = row.spec;
                        newrow.pallet = row.pallet;
                        newrow.packdes = row.packdes;
                        newrow.qunit = row.unit;
                        newrow.spec = row.spec;
                        newrow.packageUnit = row.packageUnit;
                        newrow.ifcheck = row.ifinspection;
                        newrow.price = 0;
                        newrow.quantity = 0;
                        newrow.amount = 0;
                        newrow.priceUnit = $("#currency").combobox('getValue');
                        newrow.packing = 0;
                        newrow.ifplace = 0;

                        $('#htcplist').datagrid('appendRow', newrow);
                    }
                }
                $('#productlist').datagrid('clearSelections');
                if (endEditing()) {
                    editIndex = $('#htcplist').datagrid('getRows').length - 1;
                    $('#htcplist').datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
                }
            }
        }, {
            text: '取消',
            handler: function () {
                $("#dd").dialog('close');
            }
        }]
    });
    $('#htcplist').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + contractNo,
        //width: document.documentElement.clientWidth,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        fitColumns: true,
        columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox', options: { precision: 3 } } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100 },
                    { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'spec', title: '产品规格', width: 100 },
                    { field: 'pallet', title: '最小包装', width: 100 },
                    { field: 'packageUnit', title: '包装单位', width: 100 },
                    { field: 'packdes', title: '包装', width: 100 },
                    { field: 'skinWeight', title: '皮重', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'palletrequire', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
                    { field: 'ifplace', title: '是否产地证', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' }
        ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {
                //加载产品列表
                var category = $("#productCategory").combobox('getValue');
                var currency = $("#currency").combobox('getValue');
                if (currency == "") {
                    $.messager.alert("提醒", "请选择货币");
                    return;
                }
                $('#productlist').datagrid({
                    url: '/ashx/Contract/contractData.ashx?module=cplist&category=' + category,
                    pagination: true,
                    rownumbers: true,
                    sortName: 'pcode',
                    sortOrder: 'asc',
                    columns: [[
                                { field: 'pcode', title: 'SAP编号', width: 100 },
                                { field: 'productCategory', title: '产品类别', width: 100 },
                                { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                                { field: 'spec', title: '产品规格', width: 100 },
                                { field: 'unit', title: '计量单位', width: 100 },
                                { field: 'pallet', title: '最小包装', width: 100 },
                               { field: 'packageUnit', title: '包装单位', width: 100 },
                                { field: 'packdes', title: '包装', width: 100 },
                                { field: 'hsscode', title: 'HSS编码', width: 100 },
                                { field: 'ifinspection', title: '是否商检', width: 100 },
                    ]]
                });
                //弹出
                $('#dd').window('open');
            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var rowindex = $('#htcplist').datagrid('getRowIndex', $('#htcplist').datagrid('getSelected'));
                $('#htcplist').datagrid('deleteRow', rowindex);
            }
        }],
        onClickCell: function (index, field, value) {
            $(this).datagrid('beginEdit', index);

        },
        //根据编辑
        onClickRow: function (index, row) {
            var editors = $('#htcplist').datagrid('getEditors', index);
            var quantity = editors[0];
            var price = editors[1];
            var amount = editors[2];
            $(amount.target).numberbox('setValue', quantity.target.val() * price.target.val());

            //row.amount = quantity *price;
            //alert(  parseFloat(row.quantity) * parseFloat(row.price));
        },
        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
}
//初始化文件路径
function initFile() {
    $("#upMationDetails").textbox('setText',infoText);
    $("#upMationDetails").textbox('setValue', infoValue);
}
//tabs选中事件
function selectTabs() {
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
            var contractNo = $("#contractNo").textbox('getValue');
            var language = $("#language").combobox('getValue');
            var templateno = $("#templateno").val();
            var validity = $("#validity").combobox('getValue');
            var signedTime = $("#signedtime").datebox('getValue');
            var signedplace = $("#signedplace").textbox('getValue');
            var tradement = $("#tradement ").combobox('getValue');
            var transport = $("#transport").combobox('getValue');
            var harborout = $("#harborout").combobox('getValue');
            var harborarrive = $("#harborarrive ").combobox('getValue');
            var deliveryPlace = $("#deliveryPlace").textbox('getValue');
            var pricement1 = $("#pricement1 ").combobox('getValue');
            var pricement2 = $("#pricement2").combobox('getValue');
            var validity = $("#validity").combobox('getValue');
            var pvalidity = $("#pvalidity").textbox('getValue');
            var shipment = $("#shipment").combobox('getValue');
            var placement = $("#placement").textbox('getValue');
            var buyercode = $("#buyercode").val();
            var sellercode = $("#sellercode").val();
            var shippingmark = $("#shippingmark").val();
            var batchRemark = $("#batchRemark").textbox('getValue');
            //var paymentType = $("#paymentType").combobox('getValue');
            //var shipDate = $("#shipDate").datebox('getValue');
            var overspill = $("#overspill").numberbox('getValue');
            if (title == "合同预览") {
                $('input').attr('disabled', false);
                var templateno = $("#templateno").val();
                //bindTemplate(templateno);
                //获取模板条款列表
                $("#tt_subTable").datagrid('acceptChanges');
                var datagrid = $("#tt_subTable").datagrid("getRows");
                var templatejson = JSON.stringify(datagrid);

                //隐藏语言
                hideLanguage();

                //使checkbox默认选中
                if (language == $("#checkbox").val()) {

                    $("#checkbox").prop('checked', true);
                    $("#english").prop('checked', false);
                    $("#russian").prop('checked', false);
                }
                if (language == $("#english").val()) {
                    $("#english").prop('checked', true);
                    $("#checkbox").prop('checked', false);
                    $("#russian").prop('checked', false);
                }
                if (language == $("#russian").val()) {
                    $("#russian").prop('checked', true);
                    $("#checkbox").prop('checked', false);
                    $("#english").prop('checked', false);
                }
                //使checkbox 单选
                $('#reviewContract').find('input[type=checkbox]').bind('click', function () {
                    $('#reviewContract').find('input[type=checkbox]').not(this).attr("checked", false);
                });
                //调用iframe子页面合同模板的方法,获取条款数
                //window.parent.frames["previewTemplateFrame"].alertadd();
                $('#htcplist').datagrid('acceptChanges');
                //获取list数据到htcplistStr
                var cplist = $("#htcplist").datagrid("getRows");
                var datagridjson = JSON.stringify(cplist);
                var str = "";
                var chinese = $("#chinese").val();
                var english = $("#english").val();
                var russian = $("#russian").val();
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/contractData.ashx?module=GetRealTimeContract",
                    dataType: "text",
                    data: {
                        buyer: buyer, buyercode: buyercode, seller: seller, sellercode: sellercode,
                        validity: validity, language: language, templateno: templateno, signedTime: signedTime,
                        signedplace: signedplace,
                        productlist: datagridjson, tradement: tradement, transport: transport,
                        harborout: harborout, harborarrive: harborarrive,
                        deliveryPlace: deliveryPlace, pricement1: pricement1, pricement2: pricement2, validity: validity,
                        pvalidity: pvalidity, shipment: shipment, placement: placement, shippingmark: shippingmark, contractNo: contractNo, templatejson: templatejson
                        , batchRemark: batchRemark, overspill: overspill
                    },
                    success: function (data) {
                        $("#realTimeContractText").val(data);
                        $("#previewFormFrame").attr('src', '/Bus/ContractCategory/previewContract.aspx');
                    }
                });

                $(":checkbox").click(function () {
                    var str = "";
                    $('input[name="checkbox"]:checked').each(function () {
                        str += $(this).val();
                    });
                    $.ajax({
                        type: "post",
                        url: "/ashx/Contract/contractData.ashx?module=GetRealTimeContract",
                        dataType: "text",
                        data: {
                            buyer: buyer, buyercode: buyercode, seller: seller, sellercode: sellercode,
                            validity: validity, language: str, templateno: templateno, signedTime: signedTime,
                            signedplace: signedplace,
                            productlist: datagridjson, tradement: tradement, transport: transport,
                            harborout: harborout, harborarrive: harborarrive,
                            deliveryPlace: deliveryPlace, pricement1: pricement1, pricement2: pricement2, validity: validity,
                            pvalidity: pvalidity, shipment: shipment, placement: placement, templatejson: templatejson, batchRemark: batchRemark
                            , overspill: overspill, contractNo: contractNo, shipDate: shipDate, paymentType: paymentType
                        },
                        success: function (data) {
                            $("#realTimeContractText").val(data);
                            $("#previewFormFrame").attr('src', '/Bus/ContractCategory/previewContract.aspx');
                        }
                    });


                })
            }
            if (title == "货物条款") {
                $("#tt_subTable").datagrid('acceptChanges');
                if (isBrowse == "true") {
                    $('input').attr('disabled', true);
                }
                else {
                    $('input').attr('disabled', false);
                }

            }

            if (title == "文本条款") {
                $("#tt_subTable").datagrid('acceptChanges');
                //var templateno = $("#templateno").val();

                //if (isnew=="True") {
                //    bindTemplate(templateno);
                //}
                //else {
                //    bindTemplateByContractNo();
                //}
                controlEngRus();
            }
            if (title == "审核日志") {
                $("#tt_subTable").datagrid('acceptChanges');
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
                    url: '/ashx/Contract/contractData.ashx?module=getReviewData&contractNo=' + contractNo,
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
            if (title == "合同箱单") {
                $("#tt_subTable").datagrid('acceptChanges');
                $('#htcplist').datagrid('acceptChanges');
                //获取list数据到htcplistStr
                var cplist = $("#htcplist").datagrid("getRows");
                var datagridjson = JSON.stringify(cplist);
                var buyer = $("#buyer").combobox('getValue');
                var buyername = $("#buyername").val();
                var seller = $("#seller").combobox('getValue');
                var sellername = $("#sellername").val();
                var language = "zh";
                var templateno = $("#templatename").combobox('getValue');
                var validity = $("#validity").combobox('getValue');
                var signedTime = $("#signedtime").datebox('getValue');
                var signedplace = $("#signedplace").textbox('getValue');
                var tradement = $("#tradement ").combobox('getValue');
                var transport = $("#transport").combobox('getValue');
                var harborout = $("#harborout").combobox('getValue');
                var harborarrive = $("#harborarrive ").combobox('getValue');
                var deliveryPlace = $("#deliveryPlace").textbox('getValue');
                var pricement1 = $("#pricement1 ").combobox('getValue');
                var pricement2 = $("#pricement2").combobox('getValue');
                var validity = $("#validity").combobox('getValue');
                var pvalidity = $("#pvalidity").textbox('getValue');
                var shipment = $("#shipment").combobox('getValue');
                var placement = $("#placement").textbox('getValue');
                $("#packinglist").attr('src', '/Bus/Contract/WebForm1.aspx?buyer=' + buyer + '&buyername=' + buyername + '&seller=' + seller + '&sellername=' + sellername + '&validity=' + validity + '&language=' + language
                 + '&signedTime=' + signedTime + '&signedplace=' + signedplace + '&productlist=' + datagridjson
               + '&tradement=' + tradement + '&transport=' + transport + '&harborout=' + harborout + '&harborarrive=' + harborarrive
                 + '&deliveryPlace=' + deliveryPlace + '&pricement1=' + pricement1 + '&pricement2=' + pricement2 + '&validity=' + validity
                   + '&pvalidity=' + pvalidity + '&shipment=' + shipment + '&placement=' + placement + '&currency=' + currency + '&type=packinglist');

            }
            if (title == "合同发票") {
                $("#tt_subTable").datagrid('acceptChanges');
                $('#htcplist').datagrid('acceptChanges');
                //获取list数据到htcplistStr
                var cplist = $("#htcplist").datagrid("getRows");
                var datagridjson = JSON.stringify(cplist);

                var buyer = $("#buyer").combobox('getValue');
                var buyername = $("#buyername").val();
                var seller = $("#seller").combobox('getValue');
                var sellername = $("#sellername").val();
                var language = "zh";

                var templateno = $("#templatename").combobox('getValue');
                var validity = $("#validity").combobox('getValue');
                var signedTime = $("#signedtime").datebox('getValue');
                var signedplace = $("#signedplace").textbox('getValue');
                var tradement = $("#tradement ").combobox('getValue');
                var transport = $("#transport").combobox('getValue');
                var harborout = $("#harborout").combobox('getValue');

                var harborarrive = $("#harborarrive ").combobox('getValue');
                var deliveryPlace = $("#deliveryPlace").textbox('getValue');
                var pricement1 = $("#pricement1 ").combobox('getValue');
                var pricement2 = $("#pricement2").combobox('getValue');
                var validity = $("#validity").combobox('getValue');
                var pvalidity = $("#pvalidity").textbox('getValue');
                var shipment = $("#shipment").combobox('getValue');
                var placement = $("#placement").textbox('getValue');
                var currency = $("#currency").combobox('getValue');

                $("#Invoice").attr('src', '/Bus/Contract/WebForm1.aspx?buyer=' + buyer + '&buyername=' + buyername + '&seller=' + seller + '&sellername=' + sellername + '&validity=' + validity + '&language=' + language
                     + '&signedTime=' + signedTime + '&signedplace=' + signedplace + '&productlist=' + datagridjson
                   + '&tradement=' + tradement + '&transport=' + transport + '&harborout=' + harborout + '&harborarrive=' + harborarrive
                     + '&deliveryPlace=' + deliveryPlace + '&pricement1=' + pricement1 + '&pricement2=' + pricement2 + '&validity=' + validity
                       + '&pvalidity=' + pvalidity + '&shipment=' + shipment + '&placement=' + placement + 'currency=' + currency + '&type=invoice');


            }
        }
    })
}

var additem = function (control, selectitem) {
    var tmp1 = control.val();
    var v1 = selectitem;
    if (tmp1.indexOf(v1) < 0) {
        if (tmp1.length > 0) {
            tmp1 = tmp1 + ' ' + v1;
        }
        else {
            tmp1 = v1;
        }
    }
    if (tmp1.charAt(0) == ',') {
        tmp1.substring(1, tmp1.length - 1);
    }
    control.val(tmp1);
}

var removeitem = function (control, selectitem) {

    var tmp1 = control.val();
    var v1 = selectitem;
    if (tmp1.indexOf(v1) >= 0) {
        tmp1 = tmp1.replace(',' + v1, '');
        tmp1 = tmp1.replace(v1, '');
    }
    control.val(tmp1);
}
//隐藏语言
function hideLanguage() {

    var language = $("#language").combobox('getValue');

    if (language == "中文") {

        $("#englishSpan").css("display", "none");
        $("#chineseSpan").css("display", "inline-block ");
        $("#russianSpan").css("display", "none");
        $("#onlyenglishSpan").css("display", "none");
        $("#onlyrussianSpan").css("display", "none");
    }
    if (language == "中文-英文") {
        $("#englishSpan").css("display", "inline-block ");
        $("#chineseSpan").css("display", "inline-block ");
        $("#onlyenglishSpan").css("display", "inline-block");
        $("#onlyrussianSpan").css("display", "none");
        $("#russianSpan").css("display", "none");
    }
    if (language == "中文-俄文") {
        $("#englishSpan").css("display", "none");
        $("#chineseSpan").css("display", "inline-block ");
        $("#russianSpan").css("display", "inline-block ");
        $("#onlyenglishSpan").css("display", "none");
        $("#onlyrussianSpan").css("display", "inline-block");
    }
}
//绑定模板表格
function bindTemplate(templateno) {
  
    var editRow = undefined;
    var toolbar = [{
        text: '添加',
        iconCls: 'icon-add',
        handler: function () {
            //新增空行
            var row = {};
            row.chncontent = "";
            row.engcontent = "";
            row.ruscontent = "";
            row.inline = '否';
            $('#tt_subTable').datagrid('appendRow', row);
        }
    }, {
        text: '编辑',
        iconCls: 'icon-edit',
        handler: function () {
            var row = $("#tt_subTable").datagrid('getSelected');
            if (row != null) {
                if (editRow != undefined) {
                    $("#tt_subTable").datagrid('endEdit', editRow);
                }

                if (editRow == undefined) {
                    var index = $("#tt_subTable").datagrid('getRowIndex', row);
                    $("#tt_subTable").datagrid('beginEdit', index);
                    editRow = index;
                    $("#tt_subTable").datagrid('unselectAll');
                }
            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }

        }
    }, {
        text: '删除',
        iconCls: 'icon-remove',
        handler: function () {
            var row = $("#tt_subTable").datagrid('getSelected');
            if (row != undefined) {
                var editIndex = $("#tt_subTable").datagrid('getRowIndex', row);
                $('#tt_subTable').datagrid('cancelEdit', editIndex)
                .datagrid('deleteRow', editIndex);
                editIndex = undefined;

            } else {
                $.messager.alert('提示', '请选择一行数据！', 'info');
            }
        }
    }, {
        text: '保存',
        iconCls: 'icon-remove',
        handler: function () {
            $('#tt_subTable').datagrid('acceptChanges');
        }
    }];
   
    //----初始化datagrid-----
    $('#tt_subTable').datagrid({
        nowrap: false,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        autoRowHeight: true,
        idField: 'mname',
        url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getdetail&no=' + templateno,
        columns: [[
         { field: 'sortno', title: '序号', width: '50px', editor: { type: 'numberbox', options: { precision: 0 } } },
        { field: 'templateno', title: '模板编号', hidden: 'true' },
        {
            field: 'inline', title: '排在一行', width: '65px', align: 'center',
            editor: {
                type: 'checkbox',
                options: {
                    on: '是',
                    off: '否'
                }
            }
        },
        {
            field: 'variable', title: '变量', width: '200px', align: 'center',
            editor: {
                type: 'combobox',
                options: {
                    url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getvariable&flowdirection=' + exportFlow,
                    method: 'get',
                    valueField: 'id',
                    textField: 'text',
                    multiple: true,
                    multiline: true,
                    panelHeight: 150,
                    height: 79,
                    onSelect: function (item) {
                        if (editRow == undefined) {
                            return;
                        }
                        //获取多选框里面的值

                        var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                        var chn = editors[3];
                        var eng = editors[4];
                        var rus = editors[5];

                        additem(chn.target, item.id);

                        additem(eng.target, item.id);

                        additem(rus.target, item.id);
                    },
                    onUnselect: function (item) {
                        if (editRow == undefined) {
                            return;
                        }
                        //获取多选框里面的值

                        var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                        var chn = editors[3];
                        var eng = editors[4];
                        var rus = editors[5];

                        removeitem(chn.target, item.id);

                        removeitem(eng.target, item.id);

                        removeitem(rus.target, item.id);
                    }
                }
            }
        },
        { field: 'chncontent', title: '中文', width: '300px', editor: 'textarea', styler: contentStyle },
        { field: 'engcontent', title: '英文', width: '300px', editor: 'textarea', styler: contentStyle },
        { field: 'ruscontent', title: '俄文', width: '300px', editor: 'textarea', styler: contentStyle }
        ]],
        pagination: true,
        toolbar: toolbar,
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
            if (editRow != undefined) {
                $("#tt_subTable").datagrid('endEdit', editRow);

            }
        }
    });
  
}
var contentStyle = function (value, row, index) {
    return "";
}
//控制英文俄文列的隐藏与显示
function controlEngRus() {
    var language = $('#language').combobox('getValue');
    if (language == "中文-英文") {
        $('#tt_subTable').datagrid('showColumn', 'chncontent');
        $('#tt_subTable').datagrid('showColumn', 'engcontent');
        $('#tt_subTable').datagrid('hideColumn', 'ruscontent');
    }
    if (language == "中文-俄文") {
        $('#tt_subTable').datagrid('hideColumn', 'engcontent');
        $('#tt_subTable').datagrid('showColumn', 'chncontent');
        $('#tt_subTable').datagrid('showColumn', 'ruscontent');
    }
    if (language == "中文") {
        $('#tt_subTable').datagrid('hideColumn', 'engcontent');
        $('#tt_subTable').datagrid('hideColumn', 'ruscontent');
        $('#tt_subTable').datagrid('showColumn', 'chncontent');
    }
}
