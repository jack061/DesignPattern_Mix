$(function () {
    window.onload = windowHeight;
    setInterval(windowHeight, 500)//每半秒执行一次windowHeight函数
    $('#dd2').dialog({
        closed: true,
        title: '数据字典管理',
        onResize: function () {
            $(this).dialog('center');
        }
    });
    $('#parentId').combotree({
        url: '/ashx/Basedata/DictronaryHandler.ashx?action=getList'
    });
    $('#test').treegrid({
        width: $('#content').width(),
        height: document.getElementById('content').height,
        fitColumns: false,
        nowrap: false,
        rownumbers: true,
        singleSelect: true,
        url: '/ashx/Basedata/DictronaryHandler.ashx?action=getList',
        idField: 'id',
        treeField: 'text',
        frozenColumns: [[
	                    { field: 'text', title: '名称', width: $(this).width() * 0.25
	                    }
				    ]],
        columns: [[
					    { field: 'CODE', title: '编码', width: $(this).width() * 0.15, align: 'center' },
                        { field: 'ENGLISH', title: '英文', width: $(this).width() * 0.15, align: 'center' },
                        { field: 'RUSSIAN', title: '俄文', width: $(this).width() * 0.15, align: 'center' },
					    { field: 'ISVALIDATE', title: '是否有效', width: $(this).width() * 0.15, align: 'center' },
                        { field: 'REMARK', title: '备注', width: $(this).width() * 0.4 }
                      
				
				    ]]
    });
 
});
function windowHeight() {
    var h = document.documentElement.clientHeight;
    var bodyHeight = document.getElementById("content");
    if (h < 598) {
        h = 598;
        bodyHeight.style.height = (h - 130) + "px";
    }
    else bodyHeight.style.height = (h - 130) + "px";

}
//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

function closedd2() {
    $('#dd2').dialog('close');
}
//添加
function add() {
    $('#parentId').combotree({
        url: '/ashx/Basedata/DictronaryHandler.ashx?action=getList'
    });
    $('#HdId').val('');
    $('#name').val('');
    $('#parentId').combotree('setValue', '');
    $('#code').val('');
    $('#validate').val('');
    $('#Remark').val('');

    $('#dd2').dialog('open');
}
//编辑
function edit() {
    var node = $('#test').treegrid('getSelected');
    if (node) {
        $('#HdId').val(node.id);
        //获取数据字典信息
        $('#cc').combotree({
            url: '/ashx/Basedata/DictronaryHandler.ashx?action=getList&Id=' + node.id
        });
        $.post('/ashx/Basedata/DictronaryHandler.ashx?action=edit&Id=' + node.id, function (msg) {
            var result = JSON.parse(msg);
            $('#name').val(result.NAME);
            $('#parentId').combotree('setValue', result.PARENTID);
            $('#code').val(result.CODE);
            $('#Remark').val(result.REMARK);

            if (result.ISVALIDATE === '是') {
                $('#validate').combobox('setValue', '是');
            } else {
                $('#validate').combobox('setValue', '否');
            }

            $('#english').val(result.ENGLISH);
            $('#russian').val(result.RUSSIAN);
        });
        //var scroll = $(document).scrollTop()+50;
        $('#dd2').dialog('open');
        //alert(scroll);
    } else {
        msgShow('系统提示', '请选择要编辑的数据字典', 'error');
    }
}
//删除
function del() {
    var node = $('#test').treegrid('getSelected');
    if (node) {
        var nodes = $('#test').treegrid('getChildren', node.id);
    } else {
        var nodes = $('#test').treegrid('getChildren');
    }
    var s = '';
    for (var i = 0; i < nodes.length; i++) {
        if (s != '')
            s += ',';
        s += nodes[i].id;
    }
    if (node) {
        if (s != '')
            s += ',';
        s += node.id;
        $.messager.confirm('系统提示', '删除数据字典将同时删除其下面所有数据，您确定要删除吗?', function (r) {
            if (r) {
                //删除部门信息
                $.post('/ashx/Basedata/DictronaryHandler.ashx?action=del&Id=' + s, function (msg) {
                    msgShow('系统提示', '删除成功', 'info');
                    $('#test').treegrid('reload');
                });
            }
        });
    } else {
        msgShow('系统提示', '请选择要删除的数据字典', 'error');
    }


}
//添加/保存数据字典信息
function saveDic() {
    var name = $('#name').val();
    if (name != '') {
        var remark = $('#Remark').val();
        var parentId = $('#parentId').combotree('getValue');
        var code = $('#code').val();
        var validate = $('#validate').combobox("getValue");
        var id = $('#HdId').val();
        var english = $('#english').val();
        var russian = $('#russian').val();

        $.post('/ashx/Basedata/DictronaryHandler.ashx?action=add', { "id": id, "name": name, "parentId": parentId, "code": code, "validate": validate, "remark": remark,"english":english,"russian":russian}, function (msg) {
            var result = JSON.parse(msg);
            if ("T" == result.status) {
                msgShow('系统提示', '数据字典编辑成功', 'info');
                $('#dd2').dialog('close');
                $('#test').treegrid('reload');
            } else {
                msgShow('系统提示', result.msg, 'error');
            }

        });
    } else {
        msgShow('系统提示', '请输入名称', 'error');
    }
}