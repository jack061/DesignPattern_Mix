var editRow = undefined;
var editRow0 = undefined;
var editRow1 = undefined;
var fileColumName = "";
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
            url: '/ashx/Basedata/BFactory.ashx?action=getList',
            columns: [[
            { field: 'ck', checkbox: 'true' },
            { field: 'createdate', title: '创建日期', width: '70px', editor: 'text', formatter: formatDatebox },
            { field: 'createman', title: '创建人', width: '70px' },
            { field: 'classific', title: '分类', width: '50px', editor: 'text', align: 'center' },
            { field: 'code', title: 'SAP编码', width: '80px', editor: 'text' },
            { field: 'shortname', title: '简称', width: '100px', editor: 'text' },
            { field: 'name', title: '中文名称', width: '200px', editor: 'text' },
            { field: 'address', title: '中文地址', width: '300px', editor: 'text' },
            { field: 'egname', title: '英语名称', width: '200px', editor: 'text' },
            { field: 'egaddress', title: '英语地址', width: '300px', editor: 'text' },
            { field: 'rsname', title: '俄语名称', width: '200px', editor: 'text' },
            { field: 'rsaddress', title: '俄语地址', width: '300px', editor: 'text' },
            { field: 'currency', title: '货币', width: '100px', editor: 'text' },
//            { field: 'property', title: '性质', width: '100px', editor: 'text' },
            { field: 'category', title: '性质', width: '100px', editor: 'text' },
            { field: 'salesman', title: '业务员', width: '100px', editor: 'text' },
            { field: 'remark', title: '备注', width: '300px', editor: 'text' }
            ]]
        });
    }

    if (pageTag == "form") {

        var code = $("#code").numberbox('getText');
        if (isBrowse == "true") {
            $('input').attr('disabled', true);
            $("#saveButtonDiv").css("display", "none");//隐藏保存提交按钮
        }
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
       

        var columnDelivery = [[
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
            url: '/ashx/Basedata/BFactory.ashx?action=getSubList0&code=' + code,
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
                text:'Ok',
                iconCls:'icon-ok',
                handler:function(){
                    //alert($('input[name=addType]:checked').val());
                    window.top.addNewTab("供应商管理-新增", '/Bus/BaseData/bfactory_Form1.aspx?action=add&addType='+$('input[name=addType]:checked').val());
                    $("#dlg").dialog("close");
                }
            },{
                text:'Cancel',
                handler:function(){
                    $("#dlg").dialog("close");
                }
            }]
        });
    $("#dlg").dialog('open');
}
//修改
function edit() {
    var code = '';
    var row = $("#tt").datagrid('getSelected');
 
    if (row != undefined) {
        code = row.code;
        window.top.addNewTab("供应商管理-修改", '/Bus/BaseData/bfactory_Form1.aspx?action=edit&code=' + code);
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
        window.top.addNewTab("供应商管理-查看", '/Bus/BaseData/bfactory_Form1.aspx?action=edit&code=' + code+'&isBrowse=true');
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
                $.post('/ashx/Basedata/BFactory.ashx?action=del&module=客户管理&tableName=bsupplier&pkName=code&pkVal=' + code, function (msg) {
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
    if($("#form1").form('enableValidation').form('validate') === false){
        $.messager.alert('系统提示', '请确认信息填写完整');
        return;
    }
    getSubTable1();
    $.ajax({
        cache: true,
        type: "POST",
        url: "/ashx/BaseData/BFactory.ashx?action=" + $("#action").val(),
        data: $("#form1").serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {

        },
        success: function (data) {
            if ("T" == data.status) {
                alert("操作成功！");
                top.selectAndRefreshTab('供应商管理');
                // msgShow('系统提示', '操作成功', 'info');
            } else {
                $.messager.alert('系统提示', result.msg);
            }
        }
    });

}
//取消
function cancelForm(){
    top.closeTab();
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
    var datagrid = $("#tt_subTable").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}

//获取收货信息子表数据
function getSubTable1() {
    if (editRow0 != undefined) {
        $("#tt_subTable0").datagrid("endEdit", editRow0); //接受更改
        editRow0 = undefined;
    }
//    if (editRow1 != undefined) {
//        $("#tt_subTable1").datagrid("endEdit", editRow1); //接受更改
//        editRow1 = undefined;
//    }

//    var datagrid = $("#tt_subTable1").datagrid("getRows");
//    var datagridjson = JSON.stringify(datagrid);
//    $("#datagrid1").val(datagridjson);

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
        url: "/ashx/Basedata/BFactory.ashx?action=upload",
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

