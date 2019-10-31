$(document).ready(function () {
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //绑定产品类别

        bindUI();
        $('#dd').dialog({
            closed: true,
            title: '工厂产品信息'
        });
        $('#tt').datagrid({
            width: document.getElementById('div1').width,

            height: $(window).height() - 80,
            nowrap: true,
            striped: true,
            collapsible: true,
            //remoteSort: false,
            singleSelect: true,
            sortName: 'pcode',
            sortOrder: 'desc',
            idField: 'pcode',
            url: '/ashx/Basedata/ProductListHandler.ashx?type=getList',
            columns: [[
            { field: 'PRODUCTCATEGORY', title: '产品大类', width: 80 }, 
            { field: 'PCODE', title: 'SAP编码', width: 60 },
            { field: 'PNAME', title: 'SAP名称(中)', width: 150 },
            { field: 'PNAMEEN', title: 'SAP名称(英)', width: 150 },
            { field: 'PNAMERU', title: 'SAP名称（俄）', width: 150 },
            { field: 'SPEC', title: '规格', width: 80 },
            { field: 'UNIT', title: '计量单位', width: 80 },
            { field: 'PALLET', title: '最小包装', width: 80 },
            { field: 'PACKAGEUNIT', title: '包装单位', width: 80 },
            { field: 'PACKDES', title: '包装', width: 150 },
            { field: 'HSSCODE', title: 'HS编码', width: 100 },
//            { field: 'IFINSPECTION', title: '是否商检', width: 80 },
            { field: 'STATUS', title: '状态', width: 60 }
            ]],

            pagination: true
        });

        $('#tt2').tree({
            url: '/ashx/Basedata/ProductListHandler.ashx?type=getTree',
            onClick: function (node) {
                //$(this).tree('toggle', node.text);

                if (node.text == '产品大类') {
                    var para = {};
                    $("#tt").datagrid('load', para);
                }
                else {
                    $("#categoryHidden").val(node.text);

                    $('#tt').datagrid('reload', { 'productCategory': node.text });
                }

            },
            onContextMenu: function (e, node) {
                e.preventDefault();
                $('#tt2').tree('select', node.target);
                $('#mm').menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
            }
        });
    }

    if (pageTag == "form") {

        $("#packageUnit").combobox({
            url: '/ashx/Basedata/DictronaryHandler.ashx?action=getPackUnitList',
            valueField: 'id',
            textField: 'text'
        });
        $("#packdes").combobox({
            url: '/ashx/Basedata/DictronaryHandler.ashx?action=getPackdesList',
            valueField: 'id',
            textField: 'text'
        });
        var pcode = $("input[name=pcode]").val();
        var editRow = undefined;
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
        },{
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt_subTable").datagrid('getSelected');
                var index = $("#tt_subTable").datagrid('getRowIndex', row);
                if (index!=null) {
                    $('#tt_subTable').datagrid('cancelEdit', index)
                    .datagrid('deleteRow', index);
                

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];

        $('#pCategory').combobox('setValue', productCategory);
        $('#packageUnit').combobox('setValue', packageUnit);

        //----初始化datagrid-----
        $('#tt_subTable').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'ID',
            url: '/ashx/Basedata/ProductListHandler.ashx?type=getSubList&pcode=' + pcode,
            columns: [[
           
             {
                 field: 'buyer', title: '买方', width: '200px', editor: {
                     type: 'combobox',
                     options: {
                         url: '/ashx/Contract/loadCombobox.ashx?module=internalCompany',
                         valueField: 'name',
                         textField: 'name',
                         editable: false,
                     }
                 }
             },
             {
                 field: 'seller', title: '卖方', width: '200px', editor: {
                     type: 'combobox',
                     options: {
                         url: '/ashx/Contract/loadCombobox.ashx?module=internalCompany',
                         valueField: 'name',
                         textField: 'name',
                         editable: false,
                     }
                 }
             },
             { field: 'price', title: '价格', width: '100px', editor: 'text' },
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
             { field: 'spec', title: '产品规格', width: '100px', editor: 'text' },
             { field: 'validityStart', title: '价格有效期起日期', width: '120px', editor: 'datebox' },
             { field: 'validityEnd', title: '价格有效期止日期', width: '120px', editor: 'datebox' },
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

        //设置单选按钮
        if($("#status").val() ==='禁用'){
            $("#Radio2").attr("checked","checked");
        }else{
            $("#Radio1").attr("checked","checked");
        }

        if ($("#ifinspection").val() === '是') {
            $("#Radio3").attr("checked", "checked");
        } else {
            $("#Radio4").attr("checked", "checked");
        }
    }

});

function closedd() {
    //$('#dd').dialog('close');
    top.closeTab();
}
var methods = 0;
//绑定信息
function bindUI() {
    $("#pCategory").combobox({

    })
    
}
//添加
function add() {
    window.top.addNewTab("工厂产品新增", '/Bus/BaseData/bproduct_Form1.aspx?type=add', '');
}


//修改
function edit() {
    var pcode = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        pcode = row.PCODE;
        window.top.addNewTab("工厂产品修改", '/Bus/BaseData/bproduct_Form1.aspx?type=edit&pcode=' + pcode, '');
        $('#pCategory').combobox('setValue', productCategory);
        if (ifinspection == '是') {
            //document.getElementById("Radio3").checked = true;
            // $('input:radio:first').attr('checked', 'true');
            $("input:radio[value=是]").attr('checked', 'true');
        } else if (ifinspection == '否') {
            //document.getElementById("Radio4").checked = true;
            //$('input:radio:last').attr('checked', 'true');
            $("input:radio[value=否]").attr('checked', 'true');
        }
        if (status == '禁用') {
            document.getElementById("Radio2").checked = true;
        } else if (status == '启用') {
            document.getElementById("Radio1").checked = true;
        }
         type = 'save';
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//添加/保存申请单信息
function saveApply() {
    //var type = $("#type").val();
    var subTable = getSubTable(); //获取结算价格信息
    var pcode = $('#pcode').textbox('getValue');
    var pCategory = $('#pCategory').combobox('getValue');
    var pname = $('#pname').textbox('getValue');
    var pnameen = $('#pnameen').textbox('getValue');
    var pnameru = $('#pnameru').textbox('getValue');
    var unit = $('#unit').textbox('getValue');
    var spec = $('#spec').textbox('getValue');
    var packageUnit = $("#packageUnit").combobox("getText");
    var packdes = $('#packdes').textbox('getValue');
    var pallet = $('#pallet').textbox('getValue');
    var hsscode = $('#hsscode').textbox('getValue');
    var ifinspection = $('input:radio[name="ifinspection"]:checked').val();
    var status = $('input:radio[name="status"]:checked').val();
    if (pcode == '') {
        $.messager.alert('系统提示', '请输入产品编码', 'error');
        return false;
    } else if (pname == '') {
        $.messager.alert('系统提示', '请输入产品名称', 'error');
        return false;
    } else if (spec == '') {
        $.messager.alert('系统提示', '请输入产品规格', 'error');
        return false;
    } else if (unit == '') {
        $.messager.alert('系统提示', '请输入产品计量单位', 'error');
        return false;
    } 
//    else if (pallet == '') {
//        $.messager.alert('系统提示', '请输入最小包装', 'error');
//        return false;
//    } 
//    else if (packdes == '') {
//        $.messager.alert('系统提示', '请输入包装单位', 'error');
//        return false;
//    } else if (hsscode == '') {
//        $.messager.alert('系统提示', '请输入HS编码', 'error');
//        return false;
//    }
//    else if (ifinspection == '') {
//        $.messager.alert('系统提示', '请选择是否商检', 'error');
//        return false;
//    }
    else if (status == '') {
        $.messager.alert('系统提示', '请选择状态', 'error');
        return false;
    }
    else {
        
        $.post('/ashx/Basedata/ProductListHandler.ashx?type=save&methods=' + methods + '&pcode=' + encodeURI(pcode) + '&unit=' + encodeURI(unit) +
        '&pname=' + encodeURI(pname) + '&spec=' + encodeURI(spec) + "&packdes=" + encodeURI(packdes) + "&pallet=" + encodeURI(pallet) + "&hsscode=" +
        encodeURI(hsscode) + "&ifinspection=" + encodeURI(ifinspection) + "&status=" + encodeURI(status) + "&pCategory=" + pCategory+
          "&pnameen=" + pnameen + "&pnameru=" + pnameru + "&packageUnit=" + packageUnit + "&addStatus=" + type_, { subTable: subTable }, function (msg) {
            if (methods == 1 && msg == "1") {
                //$.messager.alert('系统提示', '添加成功', 'info');
                window.top.selectAndRefreshTab('工厂产品管理');
                //$('#dd').dialog('close');
                //$('#tt').datagrid('reload');
            } else {
                if (msg == 'false') {
                    $.messager.alert('系统提示', '产品编号已存在，请重新输入', 'error');
                    $('#code').val('');
                } else {
                    window.top.selectAndRefreshTab('工厂产品管理');
                    //$('#dd').dialog('close');
                    //$('#tt').datagrid('reload');
                    window.top.closeTab();
               
                }
            }
        },'json');

    }
}

//删除
function del() {
    var node = $('#tt').datagrid('getSelected');
    if (node != null) {
        $.messager.confirm('系统提示', '删除后不可恢复，确定要删除？', function (i) {
            if (i) {
                $.post('/ashx/Basedata/ProductListHandler.ashx?type=del&pcode=' + node.PCODE, function (msg) {
                    if (msg) {
                        msgShow('系统提示', '产品删除成功', 'info');
                        $('#tt').datagrid('reload');
                    } else {
                        msgShow('系统提示', '删除失败，请稍后重试！', 'info');
                    }
                });
            }
        })

    } else {
        msgShow('系统提示', '请选择要删除的产品', 'error');
    }
}



//提交form表单
function submitForm() {
    // $("#ddframe")[0].contentWindow;
    var frame = $(window.frames["ddframe"].document).find("#form1");
    frame.form('submit', {
        url: "/ashx/BaseData/ProductListHandler.ashx?action=add",
        onSubmit: function () {
            //进行表单验证 
            //如果返回false阻止提交 
        },
        success: function (data) {
            alert(data);
            if (data == "ok") {
                $('#dd').dialog('close');
                $('#tt').datagrid('reload');
            }

        }
    });

}


//搜索
function SearchData() {
    pcode = $("#code").val();
    pname = $("#name").val();


    para = {};
    para.code = pcode;
    para.name = pname;

    $("#tt").datagrid('load', para);
}
//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

//获取产品价格信息子表数据
function getSubTable() {
    $("#tt_subTable").datagrid("acceptChanges");
    var datagrid = $("#tt_subTable").datagrid("getRows");

    var datagridjson = JSON.stringify(datagrid);

    return datagridjson;
   // $("#datagrid").val(datagridjson);
}

//下载模板
function downloadTemp() {
    window.top.addNewTab("工厂产品模板下载", '/Bus/BaseData/file/工厂产品模板.xls', '');
}
//打开导入
function importExcel() {
    window.top.addNewTab("工厂产品上传", '/Bus/BaseData/uploadBproduct.aspx', '');
}