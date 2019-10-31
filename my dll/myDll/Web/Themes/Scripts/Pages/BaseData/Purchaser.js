var editRow = undefined;
var editRow0 = undefined;
var editRow1 = undefined;
var editRow2 = undefined;
$(document).ready(function () {
 
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //初始化弹出框        
        $("#dlg").dialog('close');
        //----初始化datagrid-----
        $('#tt').datagrid({
            pagination: true,
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'code',
            url: '/ashx/Basedata/PurchaserListHandler.ashx?action=getList',
            columns: [[
            { field: 'ck', checkbox: 'true' },
            { field: 'createdate', title: '创建日期', width: '70px',  formatter: formatDatebox },
            { field: 'createman', title: '创建人', width: '70px' },
            { field: 'classific', title: '分类', width: '50px',  align: 'center' },
            { field: 'code', title: 'SAP编码', width: '60px' },
            { field: 'shortname', title: '简称', width: '90px' },
            { field: 'name', title: '中文名称', width: '200px'},
            { field: 'address', title: '中文地址', width: '200px' },
            { field: 'egname', title: '英语名称', width: '200px'},
            { field: 'egaddress', title: '英语地址', width: '200px'},
            { field: 'rsname', title: '俄语名称', width: '200px' },
            { field: 'rsaddress', title: '俄语地址', width: '200px' },
            { field: 'currency', title: '货币', width: '50px' },
//            { field: 'property', title: '性质', width: '80px' },
            { field: 'category', title: '性质', width: '40px' },
            { field: 'salesman', title: '业务员', width: '50px' },
            { field: 'remark', title: '备注', width: '300px' }
            ]]
        });
    }
    if (pageTag == "form") {
      
        if (isBrowse=="true") {
            $('input').attr('disabled', true);
            $("#saveButtonDiv").css("display","none");//隐藏保存提交按钮
        }
      
        var code = $("input[name=code]").val();
        var toolbar = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                if (editRow != undefined) {
                    $("#tt_subTable").datagrid('endEdit', editRow);
                }
                if (editRow == undefined) {
                    $("#tt_subTable").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });
                    $("#tt_subTable").datagrid('beginEdit', 0);
                    editRow = 0;
                }
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
                var index = $("#tt_subTable").datagrid('getRowIndex', row);

                if (index != null) {
                    $('#tt_subTable').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);


                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];
        var toolbar0 = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                if (editRow0 != undefined) {
                    $("#tt_subTable0").datagrid('endEdit', editRow0);
                }
                if (editRow0 == undefined) {
                    $("#tt_subTable0").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });

                    $("#tt_subTable0").datagrid('beginEdit', 0);
                    editRow0 = 0;
                }
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#tt_subTable0").datagrid('getSelected');
                if (row != null) {
                    if (editRow0 != undefined) {
                        $("#tt_subTable0").datagrid('endEdit', editRow0);
                    }
                    if (editRow0 == undefined) {
                        var index = $("#tt_subTable0").datagrid('getRowIndex', row);
                        $("#tt_subTable0").datagrid('beginEdit', index);
                        editRow1 = index;
                        $("#tt_subTable0").datagrid('unselectAll');
                    }
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt_subTable0").datagrid('getSelected');
                var index = $("#tt_subTable0").datagrid('getRowIndex', row);
                if (index != null) {
                    $('#tt_subTable0').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);


                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];
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
                var index = $("#tt_subTable1").datagrid('getRowIndex', row);

                if (index != null) {
                    $('#tt_subTable1').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);


                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];
        var toolbar2 = [{
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
                var row = $("#tt_subTable2").datagrid('getSelected');
                var index = $("#tt_subTable2").datagrid('getRowIndex', row);

                if (index != null) {
                    $('#tt_subTable2').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];

        var columnContact = null;
        if (code == "") {
            columnContact = [[
             { field: 'LANGUAGE', title: '语言', width: '100px', editor: 'text' },
             { field: 'COUNTRY', title: '国家', width: '100px', editor: 'text' },
             { field: 'CITY', title: '城市', width: '100px', editor: 'text' },
             { field: 'ADDRESS', title: '联系地址', width: '100px', editor: 'text' },
             { field: 'LINKMAN', title: '联系人', width: '100px', editor: 'text' },
             { field: 'PHONE', title: '电话', width: '100px', editor: 'text' },
             { field: 'MAIL', title: '邮箱', width: '100px', editor: 'text' }
                ]]
        }
        else {
            columnContact = [[
            { field: 'LANGUAGE', title: '语言', width: '100px', editor: 'text' },
            { field: 'COUNTRY', title: '国家', width: '100px', editor: 'text' },
            { field: 'CITY', title: '城市', width: '100px', editor: 'text' },
            { field: 'ADDRESS', title: '联系地址', width: '100px', editor: 'text' },
            { field: 'LINKMAN', title: '联系人', width: '100px', editor: 'text' },
            { field: 'PHONE', title: '电话', width: '100px', editor: 'text' },
            { field: 'MAIL', title: '邮箱', width: '100px', editor: 'text' }
                ]]
        }

        //----初始化datagrid-----
        $('#tt_subTable').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'CODE',
            url: '/ashx/Basedata/PurchaserListHandler.ashx?action=getSubList&code=' + code,
            columns: columnContact,
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
    
        var columnDelivery = null;
        columnDelivery = [[
                    { field: 'NAME', title: '名称', width: '100px', editor: 'text' },
                    //{ field: 'FOREIGNNAME', title: '外文名称', width: '300px', editor: 'text' },
                    {field: 'ADDRESS', title: '地址', width: '300px', editor: 'text' },
                    { field: 'PHONE', title: '电话', width: '150px', editor: 'text' },
                    { field: 'EMAIL', title: '邮箱', width: '200px', editor: 'text' },
                    //{ field: 'FOREIGNADDRESS', title: '外文地址', width: '300px', editor: 'text' },
                    //{ field: 'OUTHARBOR', title: '到货口岸', width: '100px', editor: 'text' },
                    //{ field: 'CLEARANCEHARBOR', title: '清关口岸', width: '100px', editor: 'text' },
                    //{ field: 'FOREIGNOUTHARBOR', title: '到货口岸(外文)', width: '100px', editor: 'text' },
                    //{ field: 'FOREIGNCLEARANCEHARBOR', title: '清关口岸(外文)', width: '100px', editor: 'text' },
            ]];
        //----通讯信息-----
        $('#tt_subTable0').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'ID',
            url: '/ashx/Basedata/PurchaserListHandler.ashx?action=getSubList0&code=' + code,
            columns: columnDelivery,
            pagination: true,
            toolbar: toolbar0,
            onAfterEdit: function (rowIndex, rowData, changes) {
                editRow0 = undefined;
            },
            onDblClickRow: function (rowIndex, rowData) {
                if (editRow0 != undefined) {
                    $("#tt_subTable0").datagrid('endEdit', editRow0);
                }

                if (editRow0 == undefined) {
                    $("#tt_subTable0").datagrid('beginEdit', rowIndex);
                    editRow0 = rowIndex;
                }
            },
            onClickRow: function (rowIndex, rowData) {
                if (editRow0 != undefined) {
                    $("#tt_subTable0").datagrid('endEdit', editRow0);
                }
            }
        });
        //----收货人信息-----
        $('#tt_subTable1').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'ID',
            url: '/ashx/Basedata/PurchaserListHandler.ashx?action=getSubList1&code=' + code,
            columns: columnDelivery,
            pagination: true,
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
        //----通知人信息-----
        $('#tt_subTable2').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'ID',
            url: '/ashx/Basedata/PurchaserListHandler.ashx?action=getSubList2&code=' + code,
            columns: columnDelivery,
            pagination: true,
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
    
        //分类选择
        if ($("#addType").val() == '境内') {
            $("input[name=egname]").parent().parent().parent().hide();
            $("input[name=rsname]").parent().parent().parent().hide();
            //            $("#classific").textbox("setText", '境内');
            $("#classific").val('境内');
            $("input[name=classific]").val('境内');
        } else {
            //$("#classific").textbox("setText", '境外');
            $("#classific").val('境外');
            $("input[name=classific]").val('境外');
            $('#pagTab').tabs('close', 1);
        }
       
        //查看图片
        $("#dlg").dialog("close"); //init
        $("#btnInfoUpload").click(function () {
            uploadFile();
        });

        $('#informationName').textbox({
            onClickButton: function () {
                var url = $('#informationUrl').val();
                if (url != '' && url != '&nbsp;') {
                    window.open(url);
                } else {
                    alert('请先上传文件！');
                }
            }
        });
        $('#businesslicenseName').textbox({
            onClickButton: function () {
                var url = $('#businesslicenseUrl').val();
                if (url != '' && url != '&nbsp;') {
                    window.open(url);
                } else {
                    alert('请先上传文件！');
                }
            }

        });
        
    }
});

//添加
function add() {
    $("#dlg").dialog({
        buttons: [{
            text: 'Ok',
            iconCls: 'icon-ok',
            handler: function () {
                //alert($('input[name=addType]:checked').val());
                window.top.addNewTab('客户管理-新增', '/Bus/BaseData/Purchaser_Form1.aspx?action=add&addType=' + $('input[name=addType]:checked').val(), '');
                $("#dlg").dialog("close");
            }
        }, {
            text: 'Cancel',
            handler: function () {
                $("#dlg").dialog("close");
            }
        }]
    });
    $("#dlg").dialog('open');
    //$("#ddframe").attr('src', '/Bus/BaseData/Purchaser_Form1.aspx?action=add');
    //$("#dd").dialog().title = "客户管理-新增";
    //$("#dd").dialog('open');
    //window.top.addNewTab("客户管理-新增", '/Bus/BaseData/Purchaser_Form1.aspx?action=add');

}
//修改
function edit() {

    var code = '';
    var row = $("#tt").datagrid('getSelected');

    if (row != undefined) {
        code = row.code;
        window.top.addNewTab("客户管理-修改", '/Bus/BaseData/Purchaser_Form1.aspx?action=edit&code=' + code);
        //$("#ddframe").attr('src', '/Bus/BaseData/Purchaser_Form1.aspx?action=add&code=' + code);
        //$("#dd").dialog().title = "客户管理-修改";
        //$("#dd").dialog('open');
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//查看
function browse() {
    var code = '';
    var row = $("#tt").datagrid('getSelected');

    if (row != undefined) {
        code = row.code;
        window.top.addNewTab("客户管理-查看", '/Bus/BaseData/Purchaser_Form1.aspx?action=edit&code=' + code+'&isBrowse=true');
        //$("#ddframe").attr('src', '/Bus/BaseData/Purchaser_Form1.aspx?action=add&code=' + code);
        //$("#dd").dialog().title = "客户管理-修改";
        //$("#dd").dialog('open');
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//删除
function del() {
    var code = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {

        code = row.code;

        $.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
            //删除数据
            if (r) {
                $.post('/ashx/Basedata/PurchaserListHandler.ashx?action=del&module=客户管理&tableName=bcustomer&pkName=code&pkVal=' + code, function (msg) {
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

//提交
function submitForm() {
    if ($("#form1").form('enableValidation').form('validate') === false) {
        $.messager.alert('系统提示', '请确认信息填写完整');
        return;
    }
    //ddframe.window.getSubTable();  //获取通讯信息子表信息
    getSubTable1(); //获取收货信息子表信息
    $.ajax({
        cache: true,
        type: "POST",
        url: "/ashx/BaseData/PurchaserListHandler.ashx?action=" + $("#action").val(),
        data: $("#form1").serialize(), // 你的formid
        async: false,
        cache: false,
        dataType: 'json',
        error: function (data) {

        },
        success: function (data) {
            if ("T" == data.status) {
                alert('操作成功!');
                top.selectAndRefreshTab('客户管理');
            } else {
                $.messager.alert('系统提示', result.msg);
            }
        }
    });

}
//取消
function cancelForm() {
    top.selectTab('客户管理'); 
}

function refreshParent() {
    $("#tt").datagrid('load');
}

//查询操作
function SearchData() {
    code = $("#code").val();
    name = $("#name").val();
    classific = $("#classific").val();

    para = {};
    para.code = code;
    para.name = name;
    para.classific = classific;

    $("#tt").datagrid('load', para);
}
//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

//获取通讯信息子表数据
function getSubTable() {
    var datagrid = $("#tt_subTable").datagrid("getSelected");

    var datagridjson = JSON.stringify(datagrid);

    $("#datagrid").val(datagridjson);
}

//获取收货信息子表数据
function getSubTable1() {
    if (editRow0 != undefined) {
        $("#tt_subTable0").datagrid("endEdit", editRow0); //接受更改
        editRow0 = undefined;
    }
    if (editRow1 != undefined) {
        $("#tt_subTable1").datagrid("endEdit", editRow1); //接受更改
        editRow1 = undefined;
    }
    if (editRow2 != undefined) {
        $("#tt_subTable2").datagrid("endEdit", editRow2); //接受更改
        editRow2 = undefined;
    }

    var datagrid = $("#tt_subTable1").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid1").val(datagridjson);

    var datagrid2 = $("#tt_subTable2").datagrid("getRows");
    var datagridjson2 = JSON.stringify(datagrid2);
    $("#datagrid2").val(datagridjson2);

    var datagrid0 = $("#tt_subTable0").datagrid("getRows");
    var datagridjson0 = JSON.stringify(datagrid0);
    $("#datagrid0").val(datagridjson0);
}

//打开长传界面
function uploadFile_open(name) {
    fileColumName = name;
    $("#dlg").dialog("open"); //打开
}

//文件上传
function uploadFile() {
    $("#form_up").ajaxSubmit({
        url: "/ashx/Basedata/PurchaserListHandler.ashx?action=upload",
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
                $("input[name='" + fileColumName + "Name']").val(str[1]);

            }
        }
    })
}
