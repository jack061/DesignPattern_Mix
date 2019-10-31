$(document).ready(function () {
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //----初始化datagrid-----
        $('#tt_product').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'INSPECTIONNO',
            url: '/ashx/Inspect/PreCheckListHandler.ashx?action=getProductList',
            columns: [[
                        { field: 'ck', checkbox: true, width: 50 },
                        { field: 'STATUS', title: '状态', width: 75 },
                        { field: 'INSPECTIONNO', title: '预验单号', width: 75 },
                        { field: 'USEABLEDATE', title: '有效期', width: 75 },
                        { field: 'INSPECMAN', title: '发货人', width: 75 },
                        { field: 'PCODE', title: '产品编号', width: 75 },
                        { field: 'PNAME', title: '产品名称', width: 75 },
                        { field: 'PCODEHS', title: 'HS编号', width: 75 },
                        { field: 'QUANTITY', title: '报检数/重量', width: 75 },
                        { field: 'QUNIT', title: '单位', width: 75 },
                        { field: 'QUANTITYA', title: '已用数量', width: 75 },
                        { field: 'QUANTITYB', title: '未用数量', width: 75 },
                        { field: 'AMOUNT', title: '申报总值', width: 75 }
            //{ field: 'CURRENCY', title: '币种', width: 75 },
            //{ field: 'PRODUCE', title: '生产单位', width: 75 },
            //{ field: 'BATCHNO', title: '批次号', width: 75 },
            //{ field: 'PRODUCEDATE', title: '生产日期', width: 75 },
            //{ field: 'TRANSPORT', title: '运输工具名称及号码', width: 75 },
            //{ field: 'CREATEMAN', title: '创建人', width: 75 },
            //{ field: 'CREATEDATE', title: '创建日期', width: 75 },
            //{ field: 'LASTMOD', title: '最后修改人', width: 75 },
            //{ field: 'LASTMODDATE', title: '最后修改日期', width: 75 }
            ]],
            pagination: true
        });
        //----初始化datagrid-----
        $('#tt_pack').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'INSPECTIONNO',
            url: '/ashx/Inspect/PreCheckListHandler.ashx?action=getPackList',
            columns: [[
                    { field: 'ck', checkbox: true, width: 50 },
                    { field: 'STATUS', title: '状态', width: 75 },
                    { field: 'INSPECTIONNO', title: '包装性能检验结果单', width: 75 },
                    { field: 'PRODUCE', title: '生产单位', width: 75 },
                    { field: 'PCODE', title: '产品编号', width: 75 },
                    { field: 'PNAME', title: '产品名称', width: 75 },
                    { field: 'USEABLEDATE', title: '有效期', width: 75 },
                    { field: 'CREATEMAN', title: '创建人', width: 75 },
                    { field: 'CREATEDATE', title: '创建日期', width: 75 },
                    { field: 'LASTMOD', title: '最后修改人', width: 75 },
                    { field: 'LASTMODDATE', title: '最后修改日期', width: 75 }
            ]],
            pagination: true
        });
    }

    if (pageTag == "form") {
        var inspectionNo_ = $("#inspectionNo").val();
        var action = $.getUrlVar('action');
        var browse = $.getUrlVar('browse');

        if (browse == 'true') {
            setReadOnly(); //设置为只读
            $(".easyui-linkbutton").attr("style", "display:none");
        }

        if (action == 'editProduct') {
            if (inspectionNo_ != "") {
                loadProductForm(inspectionNo_);
            }
        }
        if (action == 'editPack') {
            if (inspectionNo_ != "") {
                loadPackForm(inspectionNo_);
            }
        }
    }
});

function setReadOnly() { //设置为只读
    $('input').attr("readonly", true);
}

//浏览
function browseProduct() {
    var no = '';
    var row = $("#tt_product").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        alert(no);
        window.open('/Inspect/PreCheckProductForm.aspx?action=editProduct&browse=true&inspectionNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
function browsePack() {
    var no = '';
    var row = $("#tt_pack").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        window.open('/Inspect/PreCheckProductForm.aspx?action=editPack&browse=true&inspectionNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//添加
function addProduct() {
    window.open('/Inspect/PreCheckProductForm.aspx?action=addProduct', "_blank", "");
}
function addPack() {
    window.open('/Inspect/PreCheckPackForm.aspx?action=addPack', "_blank", "");
}
//修改
function editProduct() {
    var no = '';
    var row = $("#tt_product").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        window.open('/Inspect/PreCheckProductForm.aspx?action=editProduct&inspectionNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
function editPack() {
    var no = '';
    var row = $("#tt_pack").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        window.open('/Inspect/PreCheckPackForm.aspx?action=editPack&inspectionNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//删除
function delProduct() {
    var no = '';
    var row = $("#tt_product").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        $.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
            //删除数据
            if (r) {
                $.post('/ashx/Inspect/PreCheckListHandler.ashx?action=delProduct&inspectionNo=' + no, function (msg) {
                    var result = JSON.parse(msg);
                    if ("T" == result.status) {
                        msgShow('系统提示', '删除成功', 'info');
                        $("#tt_product").datagrid('load');
                    } else {
                        msgShow('系统提示', '删除失败', 'error');
                    }
                });
            }
           
        });
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
function delPack() {
    var no = '';
    var row = $("#tt_pack").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        $.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
            if (r) {
                //删除数据
                $.post('/ashx/Inspect/PreCheckListHandler.ashx?action=delPack&inspectionNo=' + no, function (msg) {
                    var result = JSON.parse(msg);
                    if ("T" == result.status) {
                        msgShow('系统提示', '删除成功', 'info');
                        $("#tt_pack").datagrid('load');
                    } else {
                        msgShow('系统提示', '删除失败', 'error');
                    }
                });
            }
     
        });
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//查询操作
function SearchProductData() {
    inspectionno_product = $("#inspectionno_product").val();
    inspecman_product = $("#inspecman_product").val();
    pname_product = $("#pname_product").val();

    para = {};
    para.INSPECTIONNO = inspectionno_product;
    para.INSPECMAN = inspecman_product;
    para.PNAME = pname_product;

    $("#tt_product").datagrid('load', para);
}

function SearchPackData() {
    inspectionno_pack = $("#inspectionno_pack").val();
    pname_pack = $("#pname_pack").val();

    para = {};
    para.INSPECTIONNO = inspectionno_pack;
    para.PNAME = pname_pack;

    $("#tt_pack").datagrid('load', para);
}

//加载form表单
function loadProductForm(inspectionno) {
    $("#form1").form('load', '/ashx/Inspect/PreCheckHandler.ashx?action=editProduct&inspectionNo=' + inspectionno);
}

function loadPackForm(inspectionno) {
    $("#form1").form('load', '/ashx/Inspect/PreCheckHandler.ashx?action=editPack&inspectionNo=' + inspectionno);
}
//提交form表单
function submitProductForm() {
    var action = $("#action").val();
    var form = $("#form1");
    form.form('submit', {
        url: "/ashx/Inspect/PreCheckHandler.ashx?action=addProduct",
        onSubmit: function () {
            //return $(this).form('enableValidation').form('validate');
            //进行表单验证 
            //如果返回false阻止提交 
        },
        success: function (data) {
            var result = JSON.parse(data);
            if ("T" == result.status) {
                msgShow('系统提示', '操作成功', 'info');
            } else {
                msgShow('系统提示', '操作失败', 'error');
            }
        }
    });
}
function submitPackForm() {
    var action = $("#action").val();
    //getSubTable(action); //获取子表信息
    var form = $("#form1");
    form.form('submit', {
        url: "/ashx/Inspect/PreCheckHandler.ashx?action=addPack",
        onSubmit: function () {
            //return $(this).form('enableValidation').form('validate');
            //进行表单验证 
            //如果返回false阻止提交 
        },
        success: function (data) {
            var result = JSON.parse(data);
            if ("T" == result.status) {
                msgShow('系统提示', '操作成功', 'info');
            } else {
                msgShow('系统提示', '操作失败', 'error');
            }
        }
    });
}

//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

/**
短暂提示
msg: 显示消息
time：停留时间ms
type：类型 4：成功，5：失败，3：警告
**/
function showTipsMsg(msg, time, type) {
    ZENG.msgbox.show(msg, type, time);
}
//jquery 扩展 （获取url参数）
$.extend({
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1];
        }
        return vars;
    },
    getUrlVar: function (name) {
        return $.getUrlVars()[name];
    }
});