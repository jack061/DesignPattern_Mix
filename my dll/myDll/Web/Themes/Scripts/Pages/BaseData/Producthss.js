$(document).ready(function () {
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //绑定产品类别

        bindUI();
        $('#dd').dialog({
            closed: true,
            title: '海关产品信息'
        });
        $('#tt').datagrid({
            width: document.getElementById('div1').width,

            //height: document.getElementById('div1').height,
            height: $(window).height() - 80,
            nowrap: true,
            striped: true,
            collapsible: true,
            //remoteSort: false,
            singleSelect: true,
            sortName: 'pcode',
            sortOrder: 'desc',
            idField: 'pcode',
            url: '/ashx/Basedata/ProducthssListHandler.ashx?action=getList',
            columns: [[
            { field: 'PRODUCTCATEGORY', title: '产品大类', width: 80 }, 
           { field: 'PCODE', title: '商品编码', width: '100px', editor: 'text' },
           { field: 'PNAME', title: '商品名称（中）', width: '100px', editor: 'text' },
           { field: 'IFINSPECTION', title: '是否商检', width: 60 },
           { field: 'PNAMEEN', title: '商品名称(英)', width: '100px', editor: 'text' },
           { field: 'PNAMERU', title: '商品名称（俄）', width: '100px', editor: 'text' },

           { field: 'CONDITION', title: '监管条件', width: '100px', editor: 'text' },
           { field: 'UNIT1', title: '第一计量单位', width: '80px', editor: 'text' },
           { field: 'UNIT2', title: '第二计量单位', width: '80px', editor: 'text' },
           { field: 'RATE1', title: '优惠税率', width: '60px', editor: 'text' },
           { field: 'RATE2', title: '普通税率', width: '60px', editor: 'text' },
           { field: 'RATE3', title: '出口税率', width: '60px', editor: 'text' },
           { field: 'RATE4', title: '优惠税率', width: '60px', editor: 'text' },
           { field: 'RATE5', title: '消费税率', width: '60px', editor: 'text' },
           { field: 'RATE6', title: '增值税率', width: '60px', editor: 'text' },
           { field: 'RATE7', title: '普通税率', width: '60px', editor: 'text' },
           { field: 'RATE8', title: '对台税率', width: '60px', editor: 'text' },
           { field: 'PRICEFLAG', title: '重点审价标志', width: '100px', editor: 'text' },
           { field: 'RATEFLAG', title: '征税要求', width: '100px', editor: 'text' },
           { field: 'INSPECTION', title: '检验检疫类别', width: '100px', editor: 'text' },
           //{ field: 'CREATEMAN', title: '创建人', width: '100px' },
           //{ field: 'CREATEDATE', title: '创建日期', width: '100px' },
           //{ field: 'LASTMOD', title: '最后修改人', width: '100px' },
           //{ field: 'LASTMODDATE', title: '最后修改日期', width: '100px' }
            ]],

            pagination: true
        });

        $('#tt2').tree({
            url: '/ashx/Basedata/ProducthssListHandler.ashx?action=getTree',
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
        //----初始化datagrid-----
        /**$('#tt').datagrid({
            height:550,
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'PCODE',
            url: '/ashx/Basedata/ProducthssListHandler.ashx?action=getList',
            columns: [[
           { field: 'PCODE', title: '商品编码', width: '100px', editor: 'text' },
            { field: 'PNAME', title: '商品名称', width: '100px', editor: 'text' },
            { field: 'CONDITION', title: '监管条件', width: '100px', editor: 'text' },
            { field: 'UNIT1', title: '第一计量单位', width: '80px', editor: 'text' },
            { field: 'UNIT2', title: '第二计量单位', width: '80px', editor: 'text' },
            { field: 'RATE1', title: '优惠税率', width: '60px', editor: 'text' },
            { field: 'RATE2', title: '普通税率', width: '60px', editor: 'text' },
            { field: 'RATE3', title: '出口税率', width: '60px', editor: 'text' },
            { field: 'RATE4', title: '优惠税率', width: '60px', editor: 'text' },
            { field: 'RATE5', title: '消费税率', width: '60px', editor: 'text' },
            { field: 'RATE6', title: '增值税率', width: '60px', editor: 'text' },
            { field: 'RATE7', title: '普通税率', width: '60px', editor: 'text' },
            { field: 'RATE8', title: '对台税率', width: '60px', editor: 'text' },
            { field: 'PRICEFLAG', title: '重点审价标志', width: '100px', editor: 'text' },
            { field: 'RATEFLAG', title: '征税要求', width: '100px', editor: 'text' },
            { field: 'INSPECTION', title: '检验检疫类别', width: '100px', editor: 'text' },
            { field: 'CREATEMAN', title: '创建人', width: '100px' },
            { field: 'CREATEDATE', title: '创建日期', width: '100px' },
            { field: 'LASTMOD', title: '最后修改人', width: '100px' },
            { field: 'LASTMODDATE', title: '最后修改日期', width: '100px' }
            ]],
            pagination: true
        });**/

        $("#dd").dialog({
            title: '海关产品-新增',
            //width: document.documentElement.clientWidth,
            //height: document.documentElement.clientHeight,
            closed: true,
            cache: false,
            modal: true,
            maximizable: true,
            /*buttons: [{
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
            }]*/
        });
    }

    if (pageTag == "form") {
        $("input[name='ifinspection'][value='" + ifinspection + "']").attr("checked", true);
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
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                if (editIndex != undefined) {
                    $('#dg').datagrid('cancelEdit', editIndex)
                    .datagrid('deleteRow', editIndex);
                    editIndex = undefined;

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }];

        $('#pCategory').combobox('setValue', productCategory);
    }


});



var methods = 0;
//绑定信息
function bindUI() {
    $("#pCategory").combobox({

    })
}
//添加
function add() {
    //$("#ddframe").attr('src', '/Bus/BaseData/bproducthss_Form1.aspx?action=add');
    //$("#dd").dialog().title = "海关产品-添加";
    //$("#dd").dialog('open');
    window.top.addNewTab("海关产品-添加", '/Bus/BaseData/bproducthss_Form1.aspx?action=add');
}
//修改
function edit() {
    var id = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        id = row.ID;
        //$("#ddframe").attr('src', '/Bus/BaseData/bproducthss_Form1.aspx?action=edit&pcode=' + id);
        //$("#dd").dialog().title = "海关产品-修改";
        //$("#dd").dialog('open');
        window.top.addNewTab("海关产品-修改", '/Bus/BaseData/bproducthss_Form1.aspx?action=edit&id=' + id);
        //$('#pCategory').combobox('setValue', productCategory);
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//删除
function del() {
    var pcode = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        pcode = row.PCODE;
        $.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
            //删除数据
            $.post('/ashx/Basedata/ProducthssListHandler.ashx?action=del&pcode=' + pcode, function (msg) {
                var result = JSON.parse(msg);
                if ("T" == result.status) {
                    msgShow('系统提示', '删除成功', 'info');
                    $("#tt").datagrid('load');
                } else {
                    msgShow('系统提示', '删除失败', 'error');
                }

            });
        });
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}


//提交form表单
function submitForm() {
    // $("#ddframe")[0].contentWindow;
    var frame = $(window.frames["ddframe"].document).find("#form1");
    frame.form('submit', {
        url: "/ashx/BaseData/ProducthssListHandler.ashx?action=add",
        onSubmit: function () {
            //进行表单验证 
            //如果返回false阻止提交 
        },
        success: function (data) {
            alert(data);
            if (data=="ok") {
                $('#dd').dialog('close');
                $('#tt').datagrid('reload');
            }
        
        }
    });

}

//查询操作
function SearchData() {
    pcode = $("#code").val();
    pname = $("#name").val();
    productCategory = $("#categoryHidden").val();

    para = {};
    para.pcode = pcode;
    para.productCategory = productCategory;

    $("#tt").datagrid('load', para);
}
//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

//确定
function saveApply() {
    $("#productCategory").val($("#pCategory").combobox('getText'));
    var url = "/ashx/BaseData/ProducthssListHandler.ashx?action=add";
    if($("#action").val()=="edit"){
        url+="&id="+$("#HdId").val();
    }

    $.ajax({
        cache: true,
        type: "POST",
        url: url,
        data: $("#form1").serialize(), // 你的formid
        async: false,
        error: function (data) {

        },
        success: function (data) {
            //var result = JSON.parse(data);
            if ("ok" == data) {
                msgShow('系统提示', '操作成功', 'info');
                window.top.selectAndRefreshTab('海关产品管理');
                top.closeTab();
            } else {
                $.messager.alert('系统提示', '操作失败');
            }
        }
    });
}
//取消
function closedd() {
    //$('#dd').dialog('close');
    top.closeTab();
}
