$(document).ready(function () {
    $.extend($.fn.datagrid.defaults.editors, {
        textarea: {

            init: function (container, options) {
                var input = $('<textarea class="datagrid-editable-input"></textarea>').appendTo(container);
                return input;
            },
            destroy: function (target) {
                $(target).remove();
            },
            getValue: function (target) {
                return $(target).val();
            },
            setValue: function (target, value) {
                $(target).val(value);
            },
            resize: function (target, width) {
                //alert(target);
                $(target)._outerWidth(width)._outerHeight("79");
            }
        }
    });

    var pageTag = $("#pageTag").val();
    if (pageTag == "form") {
        //子表处理
      
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
            url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=getdetail&no=' + $('#templateno').val(),
            columns: [[
             { field: 'sortno', title: '序号', width: '50px' },
            { field: 'templateno', title: '模板编号', hidden: 'true' },
            { field: 'inline', title: '排在一行', width: '65px', align: 'center',
                editor: {
                    type: 'checkbox',
                    options: {
                        on: '是',
                        off: '否'
                    }
                }
            },
            { field: 'variable', title: '变量', width: '200px', align: 'center',
                editor: {
                    type: 'combobox',
                    options: {
                        url: 'TemplateHandler.ashx?action=getvariable',
                        method: 'get',
                        valueField: 'id',
                        textField: 'text',
                        multiple: true,
                        multiline: true,
                        panelHeight: 'auto',
                        height: 79,
                        onSelect: function (item) {
                            if (editRow == undefined) {
                                return;
                            }
                            //获取多选框里面的值

                            var editors = $('#tt_subTable').datagrid('getEditors', editRow);
                            var chn = editors[2];
                            var eng = editors[3];
                            var rus = editors[4];

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
                            var chn = editors[2];
                            var eng = editors[3];
                            var rus = editors[4];

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
});

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

var contentStyle = function (value, row, index) {
    return "";
}

//提交form表单
function save() {
    getSubTable();
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=save",
        onSubmit: function () {
            //进行表单验证 
            valiForm();
        },
        success: function (data) {
            alert(data);
            //打开指定模板页面
            if (data.indexOf('保存成功') >= 0) {
                //window.top.selectTab('模板管理');
                window.top.closeTab();
            }
        }
    });

}
//提交form表单
function save1() {
 
    getSubTable();
    var form = $("#form1");
    form.form('submit', {
        url: "/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=save",
        onSubmit: function () {
            //进行表单验证 
            valiForm();
        },
        success: function (data) {
           
            if (data.indexOf('保存成功') >= 0) {
                window.parent.closeTemp();
            }
        }
    });

}

//校验form
function valiForm() {
    return $("#form1").form('validate');
}
//获取子表数据
function getSubTable() {
    var datagrid = $("#tt_subTable").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}

var preview = function () {
    var no = $('#templateno').val();
    if (no != undefined) {
        window.top.addNewTab("模板管理-预览", '/Bus/BaseData/Btemplate/BtemplateView.aspx?no=' + no);
    }
}


