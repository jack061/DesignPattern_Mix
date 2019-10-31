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
            idField: 'APPLYNO',
            url: '/ashx/TrainApply/TrainApplyEListHandler.ashx?action=getList',
            columns: [[
                { field: 'STATUS', title: '状态', width: 60 },
                { field: 'APPLYNO', title: '请车单号', width: 80 },
                { field: 'CUSTOMER', title: '客户名称', width: 120 },
                { field: 'CONTRACTNO', title: '供货合同号', width: 80 },
                { field: 'SENDMAN', title: '发货人', width: 80 },
                { field: 'DEPARTURE', title: '发站', width: 80 },
                { field: 'RECEIVEMAN', title: '收货人', width: 80 },
                { field: 'ARRIVAL', title: '到站', width: 120 },
                { field: 'BATCHNO', title: '批号', width: 120 },
                { field: 'REMARK', title: '备注', width: 120 },
                { field: 'CREATEMAN', title: '申请人', width: 60 },
                { field: 'CREATEDATE', title: '申请时间', width: 80 }
                ]],

            pagination: true
        });      
    }

    if (pageTag == "form") {
        var applyNo_ = $("#applyNo").val();
        var action = $.getUrlVar('action');
        var browse = $.getUrlVar('browse');

        if (action == 'edit') {
            if (applyNo_ != "") {
                loadForm(applyNo_);
            }
        }
        if (browse == 'true') {
            setReadOnly(); //设置为只读
            $(".easyui-linkbutton").attr("style", "display:none");
        }

        var editRow1 = undefined;
        var editRow2 = undefined;
        var editRow3 = undefined;
        var editRow4 = undefined;

        //toolbar1
        var toolbar1 = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                if (editRow1 != undefined) {
                    $("#tt_subTable1").datagrid('endEdit', editRow1);
                }
                if (editRow1 == undefined) {
                    $("#tt_subTable1").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });

                    $("#tt_subTable1").datagrid('beginEdit', 0);
                    editRow1 = 0;
                }
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#tt_subTable1").datagrid('getSelected');
                if (row != null) {
                    if (editRow1 != undefined) {
                        $("#tt_subTable1").datagrid('endEdit', editRow1);
                    }

                    if (editRow1 == undefined) {
                        var index = $("#tt_subTable1").datagrid('getRowIndex', row);
                        $("#tt_subTable1").datagrid('beginEdit', index);
                        editRow1 = index;
                        $("#tt_subTable1").datagrid('unselectAll');
                    }
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }

            }
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt_subTable1").datagrid('getSelected');
                if (row != undefined) {
                    var editIndex = $("#tt_subTable1").datagrid('getRowIndex', row);
                    $("#tt_subTable1").datagrid('cancelEdit', editIndex)
                            .datagrid('deleteRow', editIndex);
                    editIndex = undefined;

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];

        //toolbar2
        var toolbar2 = [
        {
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                if (editRow2 != undefined) {
                    $("#tt_subTable2").datagrid('endEdit', editRow2);
                }
                if (editRow2 == undefined) {
                    $("#tt_subTable2").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });

                    $("#tt_subTable2").datagrid('beginEdit', 0);
                    editRow2 = 0;
                }
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#tt_subTable2").datagrid('getSelected');
                if (row != null) {
                    if (editRow2 != undefined) {
                        $("#tt_subTable2").datagrid('endEdit', editRow2);
                    }

                    if (editRow2 == undefined) {
                        var index = $("#tt_subTable2").datagrid('getRowIndex', row);
                        $("#tt_subTable2").datagrid('beginEdit', index);
                        editRow2 = index;
                        $("#tt_subTable2").datagrid('unselectAll');
                    }
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }

            }
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt_subTable2").datagrid('getSelected');
                if (row != undefined) {
                    var editIndex = $("#tt_subTable2").datagrid('getRowIndex', row);
                    $("#tt_subTable2").datagrid('cancelEdit', editIndex)
                    .datagrid('deleteRow', editIndex);
                    editIndex = undefined;

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];

        //toolbar3
        var toolbar3 = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                if (editRow3 != undefined) {
                    $("#tt_subTable3").datagrid('endEdit', editRow3);
                }
                if (editRow3 == undefined) {
                    $("#tt_subTable3").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });

                    $("#tt_subTable3").datagrid('beginEdit', 0);
                    editRow3 = 0;
                }
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#tt_subTable3").datagrid('getSelected');
                if (row != null) {
                    if (editRow3 != undefined) {
                        $("#tt_subTable3").datagrid('endEdit', editRow3);
                    }

                    if (editRow3 == undefined) {
                        var index = $("#tt_subTable3").datagrid('getRowIndex', row);
                        $("#tt_subTable3").datagrid('beginEdit', index);
                        editRow3 = index;
                        $("#tt_subTable3").datagrid('unselectAll');
                    }
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }

            }
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt_subTable3").datagrid('getSelected');
                if (row != undefined) {
                    var editIndex = $("#tt_subTable3").datagrid('getRowIndex', row);
                    $("#tt_subTable3").datagrid('cancelEdit', editIndex)
                    .datagrid('deleteRow', editIndex);
                    editIndex = undefined;

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];

        //toolbar4
        var toolbar4 = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                if (editRow4 != undefined) {
                    $("#tt_subTable4").datagrid('endEdit', editRow4);
                }
                if (editRow4 == undefined) {
                    $("#tt_subTable4").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });

                    $("#tt_subTable4").datagrid('beginEdit', 0);
                    editRow4 = 0;
                }
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#tt_subTable4").datagrid('getSelected');
                if (row != null) {
                    if (editRow4 != undefined) {
                        $("#tt_subTable4").datagrid('endEdit', editRow4);
                    }

                    if (editRow4 == undefined) {
                        var index = $("#tt_subTable4").datagrid('getRowIndex', row);
                        $("#tt_subTable4").datagrid('beginEdit', index);
                        editRow4 = index;
                        $("#tt_subTable4").datagrid('unselectAll');
                    }
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }

            }
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt_subTable4").datagrid('getSelected');
                if (row != undefined) {
                    var editIndex = $("#tt_subTable4").datagrid('getRowIndex', row);
                    $("#tt_subTable4").datagrid('cancelEdit', editIndex)
                    .datagrid('deleteRow', editIndex);
                    editIndex = undefined;

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];
        //----车辆信息-----
        $('#tt_subTable1').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'APPLYNO',
            url: '/ashx/TrainApply/TrainApplyEHandler.ashx?action=getSubList&applyNo=' + applyNo_+'&table=1',
            columns: [[
                //{ field: 'APPLYNO', title: '请车单号', width: '100px', editor: 'text' },
                //{ field: 'SORTNO', title: '请车序号', width: '100px', editor: 'text' },
                { field: 'TRAIN', title: '车辆', width: '200px', editor: 'text' },
                { field: 'PROVIDE', title: '何方提供', width: '200px', editor: 'text' },
                { field: 'LOAD', title: '载重量', width: '200px', editor: { type: 'numberbox', options: { precision: 1}} },
                { field: 'AXISNUM', title: '轴数', width: '100px', editor: { type: 'numberbox'} },
                { field: 'DEADWEIGHT', title: '自重', width: '100px', editor: { type: 'numberbox', options: { precision: 1}} },
                { field: 'TRAINTYPE', title: '罐车类型', width: '100px', editor: 'text' },
                { field: 'GOODSLOAD', title: '货物重量', width: '100px', editor: { type: 'numberbox', options: { precision: 1}} },
                { field: 'PIECE', title: '件数', width: '100px', editor: { type: 'numberbox'} }
            ]],
            pagination: false,
            toolbar: toolbar1,
            onAfterEdit: function (rowIndex, rowData, changes) {
                editRow1 = undefined;
            },
            onDblClickRow: function (rowIndex, rowData) {
                if (editRow1 != undefined) {
                    $("#tt_subTable1").datagrid('endEdit', editRow1);
                }

                if (editRow1 == undefined) {
                    $("#tt_subTable1").datagrid('beginEdit', rowIndex);
                    editRow1 = rowIndex;
                }
            },
            onClickRow: function (rowIndex, rowData) {
                if (editRow1 != undefined) {
                    $("#tt_subTable1").datagrid('endEdit', editRow1);

                }
            }
        });

        //----货物明细-----
        $('#tt_subTable2').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'APPLYNO',
            url: '/ashx/TrainApply/TrainApplyEHandler.ashx?action=getSubList&applyNo=' + applyNo_ + '&table=2',
            columns: [[
                //{ field: 'APPLYNO', title: '请车单号', width: '100px', editor: 'text' },
                { field: 'PNAME', title: '货物名称', width: '200px', editor: 'text' },
                { field: 'PACKTYPE', title: '包装种类', width: '200px', editor: 'text' },
                { field: 'PIECE', title: '件数', width: '200px', editor: { type: 'numberbox'} },
                { field: 'WEIGHT', title: '重量（公斤）', width: '200px', editor: { type: 'numberbox', options: { precision: 1}} }
            ]],
            pagination: false,
            toolbar: toolbar2,
            onAfterEdit: function (rowIndex, rowData, changes) {
                editRow2 = undefined;
            },
            onDblClickRow: function (rowIndex, rowData) {
                if (editRow2 != undefined) {
                    $("#tt_subTable2").datagrid('endEdit', editRow2);
                }

                if (editRow2 == undefined) {
                    $("#tt_subTable2").datagrid('beginEdit', rowIndex);
                    editRow2 = rowIndex;
                }
            },
            onClickRow: function (rowIndex, rowData) {
                if (editRow2 != undefined) {
                    $("#tt_subTable2").datagrid('endEdit', editRow2);
                }
            }
        });

        //----封印-----
        $('#tt_subTable3').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'APPLYNO',
            url: '/ashx/TrainApply/TrainApplyEHandler.ashx?action=getSubList&applyNo=' + applyNo_ + '&table=3',
            columns: [[
                //{ field: 'APPLYNO', title: '请车单号', width: '100px', editor: 'text' },
                { field: 'QUANTITY', title: '数量', width: '300px', editor: { type: 'numberbox'} },
                { field: 'SIGN', title: '记号', width: '300px', editor: 'text' }
            ]],
            pagination: false,
            toolbar: toolbar3,
            onAfterEdit: function (rowIndex, rowData, changes) {
                editRow3 = undefined;
            },
            onDblClickRow: function (rowIndex, rowData) {
                if (editRow3 != undefined) {
                    $("#tt_subTable3").datagrid('endEdit', editRow3);
                }
                if (editRow3 == undefined) {
                    $("#tt_subTable3").datagrid('beginEdit', rowIndex);
                    editRow3 = rowIndex;
                }
            },
            onClickRow: function (rowIndex, rowData) {
                if (editRow3 != undefined) {
                    $("#tt_subTable3").datagrid('endEdit', editRow3);
                }
            }
        });

        //----承运信息-----
        $('#tt_subTable4').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'APPLYNO',
            url: '/ashx/TrainApply/TrainApplyEHandler.ashx?action=getSubList&applyNo=' + applyNo_ + '&table=4',
            columns: [[
                //{ field: 'APPLYNO', title: '请车单号', width: '100px', editor: 'text' },
                { field: 'SECTION', title: '承运人', width: '200px', editor: 'text' },
                { field: 'SECTIONBEGIN', title: '区段自', width: '200px', editor: 'text' },
                { field: 'SECTIONEND', title: '区段至', width: '200px', editor: 'text' },
                { field: 'STATIONCODE1', title: '车站代码1', width: '100px', editor: 'text' },
                { field: 'STATIONCODE2', title: '车站代码2', width: '100px', editor: 'text' }
            ]],
            pagination: false,
            toolbar: toolbar4,
            onAfterEdit: function (rowIndex, rowData, changes) {
                editRow4 = undefined;
            },
            onDblClickRow: function (rowIndex, rowData) {
                if (editRow4 != undefined) {
                    $("#tt_subTable4").datagrid('endEdit', editRow4);
                }
                if (editRow4 == undefined) {
                    $("#tt_subTable4").datagrid('beginEdit', rowIndex);
                    editRow4 = rowIndex;
                }
            },
            onClickRow: function (rowIndex, rowData) {
                if (editRow4 != undefined) {
                    $("#tt_subTable4").datagrid('endEdit', editRow4);
                }
            }
        });
    }
    
});

function setReadOnly() { //设置为只读
    $('input').attr("readonly", true);
}

//显示消息
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

//添加
function add() {
    window.open('/TrainApply/TrainApplyEForm.aspx?action=add', "_blank", "");
}

//浏览
function browse() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.APPLYNO;
        window.open('/TrainApply/TrainApplyEForm.aspx?action=edit&applyNo=' + no+"&browse=true", "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}


//预览
function preview() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.APPLYNO;
        window.open('/TrainApply/WebForm.aspx?applyNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}


//修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.APPLYNO;
        window.open('/TrainApply/TrainApplyEForm.aspx?action=edit&applyNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//删除
function del() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.APPLYNO;
        $.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
            if (r) {
                //删除数据
                $.post('/ashx/TrainApply/TrainApplyEHandler.ashx?action=del&applyNo=' + no, function (msg) {
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

//提交form表单
function submitForm() {
    var form = $("#form1");
    var action = $("#action").val();
    getSubTable(action); //获取子表信息
    form.form('submit', {
        url: "/ashx/TrainApply/TrainApplyEHandler.ashx?action=add",
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

//加载form表单
function loadForm(applyNo_) {
    $("#form1").form('load', '/ashx/TrainApply/TrainApplyEHandler.ashx?action=edit&applyNo=' + applyNo_);
}
//获取子表数据
function getSubTable(action) {
    if ("add" == action) {
        var datagrid1 = $("#tt_subTable1").datagrid("getRows");
        var datagridjson1 = JSON.stringify(datagrid1);
        $("#datagrid1").val(datagridjson1);

        var datagrid2 = $("#tt_subTable2").datagrid("getRows");
        var datagridjson2 = JSON.stringify(datagrid2);
        $("#datagrid2").val(datagridjson2);

        var datagrid3 = $("#tt_subTable3").datagrid("getRows");
        var datagridjson3 = JSON.stringify(datagrid3);
        $("#datagrid3").val(datagridjson3);

        var datagrid4 = $("#tt_subTable4").datagrid("getRows");
        var datagridjson4 = JSON.stringify(datagrid4);
        $("#datagrid4").val(datagridjson4);
    } else {
        var datagrid1 = $("#tt_subTable1").datagrid("getRows");
        var datagridjson1 = JSON.stringify(datagrid1);
        $("#datagrid1").val(datagridjson1);

        var datagrid2 = $("#tt_subTable2").datagrid("getRows");
        var datagridjson2 = JSON.stringify(datagrid2);
        $("#datagrid2").val(datagridjson2);

        var datagrid3 = $("#tt_subTable3").datagrid("getRows");
        var datagridjson3 = JSON.stringify(datagrid3);
        $("#datagrid3").val(datagridjson3);

        var datagrid4 = $("#tt_subTable4").datagrid("getRows");
        var datagridjson4 = JSON.stringify(datagrid4);
        $("#datagrid4").val(datagridjson4);
    }
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