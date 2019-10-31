$(document).ready(function () {
    $('#preUploadFile').datagrid({
        sortName: 'contractNo',
        sortOrder: 'asc',
        nowrap: true,
        fitColumns: true,
        striped: true,
        collapsible: true,
        pageList: [10, 15, 30],
        singleSelect: true,
        columns: [[
            { field: 'productcategory', title: '产品大类', width: 80 }, 
            { field: 'pcode', title: 'SAP编码', width: 60 },
            { field: 'pname', title: 'SAP名称(中)', width: 150 },
            { field: 'pnameen', title: 'SAP名称(英)', width: 150 },
            { field: 'pnameru', title: 'SAP名称（俄）', width: 150 },
            { field: 'spec', title: '规格', width: 80 },
            { field: 'unit', title: '计量单位', width: 80 },
            { field: 'pallet', title: '最小包装', width: 80 },
            { field: 'packageunit', title: '包装单位', width: 80 },
            { field: 'packdes', title: '包装', width: 150 },
            { field: 'hsscode', title: 'HS编码', width: 100 },
            { field: 'status', title: '状态', width: 60 }
        ]]
    });
    $("#btnFilePreview").click(function () {
        uploadFile();
    });
});
//文件预览
function uploadFile() {
    $("#form1").ajaxSubmit({
        url: "/ashx/Contract/chosseContractData.ashx?module=acceptUploadFile",
        type: "post",
        dataType: 'json',
        success: function (data) {
            if (data == "error") {
                $.messager.alert("错误：", "上传失败");
            }
            else {
                $('#preUploadFile').datagrid('loadData', data.rows);
                $.messager.alert("提示", "加载成功！");
            }
        }
    });
}
//保存
function save() {
    var rows = $("#preUploadFile").datagrid('getRows');
    var datagridJson = JSON.stringify(rows);
    $.post("/ashx/Basedata/ProductListHandler.ashx?type=saveImportData", { datagridJson: datagridJson }, function (data) {
        var result = JSON.parse(data);
        if (result.sucess == "1") {
            $.messager.alert("提醒", "保存成功");
            window.top.selectAndRefreshTab('工厂产品管理');

        }
        else {
            $.messager.alert("提醒", result.errormsg);
        }
    })
}
