$(document).ready(function () {

    //----初始化datagrid-----
    var applyno = $("#applyNo").val();
    var action = $("#action").val();
    if (action == 'editList') {
        loadForm(applyno);

    }   
});

function loadForm(applyno) {
    $('#form1').form("load", "/ashx/TrainApply/TrainApplyIHandler.ashx?action=loadList&applyNo=" + applyno);
}

function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

//保存申请单
function saveApply() {
    if ($("input[name='STATUS']").val() == '已汇总') {
        msgShow('系统提示', '已汇总提交单无法修改！', 'error');
    } else {
        var form = $("#form1");
        form.form('submit', {
            url: "/ashx/TrainApply/TrainApplyIHandler.ashx?action=saveList",
            onSubmit: function () {
                return $(this).form('enableValidation').form('validate');
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
}

//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

