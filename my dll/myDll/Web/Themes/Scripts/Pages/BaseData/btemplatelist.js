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
            idField: 'templatename',
            url: '/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=list',
            columns: [[
                { field: 'templateno', title: 'zj', width: '40px',checkbox:'true' },
                { field: 'status', title: '状态', width: '40px' },
                { field: 'templatename', title: '模板名称', width: '120px' },
                { field: 'createmanname', title: '创建人', width: '80px' },
                { field: 'createdate', title: '创建时间', width: '80px' },
                { field: 'lastmodname', title: '最后修改人 ', width: '120px' },
                { field: 'lastmod', title: '最后修改时间', width: '80px' },
                { field: 'remark', title: '备注', width: '150px' }
            ]],
            pagination: true
        });

        $("#dd").dialog({
            title: '新增入库',
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
    }
});

//查询操作
function SearchData() {
    templatename = $("#templatename").val();
    man = $("#man").val();
    beginTime = $("#beginTime").datebox('getValue');
    endTime = $("#endTime").datebox('getValue');

    console.log(templatename);
    console.log(man);
    console.log(beginTime); 
    console.log(endTime);

    para = {};
    para.templatename = templatename;
    para.man = man;
    para.beginTime = beginTime;
    para.endTime = endTime;

    $("#tt").datagrid('load', para);
}


//添加
function add() {
    window.top.addNewTab("模板管理-新增", '/Bus/BaseData/Btemplate/BtemplateAdd.aspx?action=add');
}
//修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.templateno;
        window.top.addNewTab("模板管理-修改", '/Bus/BaseData/Btemplate/BtemplateAdd.aspx?action=edit&no=' + no);
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//删除
function del() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined && row.status == '新建') {
        no = row.templateno;
        $.messager.confirm('系统提示', '确认要删除么?</br></br>',
        function (r) {
            if (r) {
                $.post('/Bus/BaseData/Btemplate/TemplateHandler.ashx?action=delete', { templateno: no },
	               function (msg) {
	                   if (msg.length > 0) {
	                       alert(msg);
	                   }
	                   else {
	                       SearchData();
	                   }
	               });
            }
        });
    }
    else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

var RefreshUI = function () {
    SearchData();
}

var submit = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.indocno;
        submitbill(row.status, '提交', no, '入库');
    }
}

var review = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.indocno;
        submitbill(row.status, '审核', no, '入库');
    }
}

var confirm = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.indocno;
        submitbill(row.status, '确认', no, '入库');
    }
}

var preview = function () {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.templateno;
        window.top.addNewTab("模板管理-预览", '/Bus/BaseData/Btemplate/BtemplateView.aspx?no='+no);
    }
}
