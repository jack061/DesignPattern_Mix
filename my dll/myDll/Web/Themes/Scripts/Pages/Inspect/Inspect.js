$(document).ready(function () {
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //----初始化datagrid-----
        $('#tt').datagrid({
            height:550,
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'INSPECTIONNO',
            rownumbers: true,
            url: '/ashx/Inspect/InspectListHandler.ashx?action=getList',
            columns: [[
                        { field: 'ck', checkbox: true, width: 50 },
                        { field: 'STATUS', title: '状态', width: 100 },
                        { field: 'INSPECTIONNO', title: '申报号', width: 250 },
                        { field: 'APASSWD', title: '申报密码', width: 200 },
                        { field: 'INSPECTNUM', title: '报检号', width: 150 },
                        { field: 'REINSPECTNUM', title: '预报检号', width: 200 },
                        { field: 'REGISTERNUM', title: '注册号', width: 200 },
                        { field: 'PRODUCE', title: '企业名称', width: 200 },
                        { field: 'INSPECTMAN', title: '报检员编号', width: 200 },
                        { field: 'PROPERTY', title: '企业性质', width: 200 },
                        { field: 'INSPECTTYPE', title: '报检类别', width: 200 },
                        { field: 'INSPECTDEPT', title: '施检机构', width: 200 },
                        { field: 'PURPOSEDEPT', title: '目的机构', width: 200 },
                        { field: 'APPLYDATE', title: '申请日期', width: 200 },
            //{ field: 'LINKMAN', title: '联系人', width: 100 },
            //{ field: 'PHONE', title: '电话号码', width: 100 },
            //{ field: 'SHIPPER', title: '发货人', width: 75 },
            //{ field: 'SHIPPERCN', title: '中文名称', width: 100 },
            //{ field: 'SHIPPEREG', title: '英文名称', width: 100 },
            //{ field: 'RECEIVER', title: '收货人', width: 75 },
            //{ field: 'RECEIVERCN', title: '中文名称', width: 100 },
            //{ field: 'RECEIVEREG', title: '英文名称', width: 100 },
            //{ field: 'TRANSPORT', title: '运输工具', width: 100 },
            //{ field: 'TOCOUNTRY', title: '输往国家/地区', width: 175 },
            //{ field: 'FROMHARBOR', title: '启运口岸', width: 100 },
            //{ field: 'EXITHARBOR', title: '出境口岸', width: 100 },
            //{ field: 'LOCATION', title: '存货地点', width: 100 },
            //{ field: 'CONTRACTNO', title: '合同号', width: 75 },
            //{ field: 'ATTACHDOC', title: '随附单据', width: 100 },
            //{ field: 'NEEDDOC', title: '需要单证', width: 100 },
            //{ field: 'ATTACHMENT', title: '结果附件', width: 100 },
            //{ field: 'APPLYTYPE', title: '商检类型', width: 100 },
            //{ field: 'REMARK', title: '备注', width: 50 },
            //{ field: 'STATUS', title: '状态', width: 50 },
            //{ field: 'CREATEMAN', title: '创建人', width: 75 },
            //{ field: 'CREATEDATE', title: '创建日期', width: 100 },
            //{ field: 'LASTMOD', title: '最后修改人', width: 125},
            //{ field: 'LASTMODDATE', title: '最后修改日期', width: 150 }
            ]],
            pagination: true
        });
    }

    if (pageTag == "form") {
        var inspectionNo_ = $("#inspectionNo").val();
        var action = $.getUrlVar('action');
        var browse = $.getUrlVar('browse');
        if (action == 'edit') {
            if (inspectionNo_ != "") {
                loadForm(inspectionNo_);
            }
        }
        if (browse == 'true') {
            setReadOnly(); //设置为只读
            $(".easyui-linkbutton").attr("style", "display:none");
        }

        var editRow = undefined;
        var toolbar = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                if (editRow != undefined) {
                    $("#tt").datagrid('endEdit', editRow);
                }
                if (editRow == undefined) {
                    $("#tt").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });

                    $("#tt").datagrid('beginEdit', 0);
                    editRow = 0;
                }
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != null) {
                    if (editRow != undefined) {
                        $("#tt").datagrid('endEdit', editRow);
                    }
                    if (editRow == undefined) {
                        var index = $("#tt").datagrid('getRowIndex', row);
                        $("#tt").datagrid('beginEdit', index);
                        editRow = index;
                        $("#tt").datagrid('unselectAll');
                    }
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }

            }
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != undefined) {
                    var editIndex = $("#tt").datagrid('getRowIndex', row);
                    $('#tt').datagrid('cancelEdit', editIndex)
                    .datagrid('deleteRow', editIndex);
                    editIndex = undefined;

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];

        //----初始化datagrid-----
        $('#tt').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            url: '/ashx/Inspect/InspectHandler.ashx?action=getSubList&inspectionNo=' + inspectionNo_,
            columns: [[
            //                { field: 'INSPECTIONNO', title: '商检单号', width: '200px',editor:'text' },
                {field: 'PCODE', title: 'HS编码', width: '100px', editor: 'text' },
                { field: 'PNAME', title: '货名', width: '200px', editor: 'text' },
                { field: 'WEIGHT', title: '重量', width: '80px', editor: { type: 'numberbox', options: { precision: 2}} },
                { field: 'AMOUNT', title: '总值', width: '80px', editor: { type: 'numberbox', options: { precision: 2}} },
                { field: 'PRODUCEUNIT', title: '生产单位编码', width: '200px', editor: 'text' },
                { field: 'PACKAGE', title: '包装', width: '100px', editor: { type: 'numberbox', options: { precision: 2}} },
                { field: 'HSWEIGHT', title: 'HS标准量', width: '150px', editor: { type: 'numberbox', options: { precision: 0}} },
                { field: 'PRODUCEPLACE', title: '原产地', width: '80px', editor: 'text' },
                { field: 'REMARK', title: '备注', width: '300px', editor: 'text' }
            ]],
            pagination: true,
            toolbar: toolbar,
            onAfterEdit: function (rowIndex, rowData, changes) {
                editRow = undefined;
            },
            onDblClickRow: function (rowIndex, rowData) {
                if (editRow != undefined) {
                    $("#tt").datagrid('endEdit', editRow);
                }

                if (editRow == undefined) {
                    $("#tt").datagrid('beginEdit', rowIndex);
                    editRow = rowIndex;
                }
            },
            onClickRow: function (rowIndex, rowData) {
                if (editRow != undefined) {
                    $("#tt").datagrid('endEdit', editRow);

                }
            }

        });
    }
});

function setReadOnly() { //设置为只读
    $('input').attr("readonly", true);
}

//浏览
function browse() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        window.open('/Inspect/InspectForm.aspx?action=edit&browse=true&inspectionNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//添加
function add() {
    window.open('/Inspect/InspectForm.aspx?action=add', "_blank", "");
}
//修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        window.open('/Inspect/InspectForm.aspx?action=edit&inspectionNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//删除
function del() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        $.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
            //删除数据
            if (r) {
                $.post('/ashx/Inspect/InspectListHandler.ashx?action=del&inspectionNo=' + no, function (msg) {
                    var result = JSON.parse(msg);
                    if ("T" == result.status) {
                        msgShow('系统提示', '删除成功', 'info');
                        $("#tt").datagrid('load');
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
function SearchData() {
    inspectionno = $("#inspectionnos").val();
    inspectnum = $("#inspectnums").val();
    reinspectnum = $("#reinspectnums").val();

    para = {};
    para.INSPECTIONNO = inspectionno;
    para.INSPECTNUM = inspectnum;
    para.REINSPECTNUM = reinspectnum;

    $("#tt").datagrid('load', para);
}

//加载form表单
function loadForm(inspectionno) {
    $("#form1").form('load', '/ashx/Inspect/InspectHandler.ashx?action=edit&inspectionNo=' + inspectionno);
}
//提交form表单
function submitForm() {
    var action = $("#action").val();
    getSubTable(action); //获取子表信息
    var form = $("#form1");
    form.form('submit', {
        url: "/ashx/Inspect/InspectHandler.ashx?action=add",
        onSubmit: function () {
            return $(this).form('enableValidation').form('validate');
            //进行表单验证 
            //如果返回false阻止提交 
        },
        success: function (data) {
            var result = JSON.parse(data);
            if ("T" == result.status) {
                msgShow('系统提示', '操作成功', 'info');
                $("#tt").datagrid('load');
            } else {
                msgShow('系统提示', '操作失败', 'error');
            }
        }
    });
}

//获取子表数据
function getSubTable(action) {
    if ("add" == action) {
        var datagrid = $("#tt").datagrid("getRows");
        var datagridjson = JSON.stringify(datagrid);
        $("#datagrid").val(datagridjson);
    } else {
        var datagrid = $("#tt").datagrid("getRows");
        var datagridjson = JSON.stringify(datagrid);
        $("#datagrid").val(datagridjson);
    }
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