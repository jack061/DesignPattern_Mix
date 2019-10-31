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
            url: '/ashx/Finance/BankWaterHandler.ashx?action=getList',
            columns: [[
            { field: 'DOCNO', title: '导入单号', width: '150px' },
            { field: 'BANKNAME', title: '开户行名称', width: '200px' },
            { field: 'ACCOUNTNAME', title: '户名', width: '150px' },
            { field: 'ACCOUNT', title: '账户', width: '200px' },
            { field: 'CURRENCY', title: '币种', width: '100px' },
            { field: 'BEGINDATE', title: '日期开始', width: '120px', formatter: formatDatebox },
            { field: 'ENDDATE', title: '日期结束', width: '120px', formatter: formatDatebox }
            ]],
            pagination: true
        });

    }


    if (pageTag == "form") {
        var docno = $("#docNo").val();
        var action = $.getUrlVar('action');
        //货币
        $('#currency').combobox({
            url: '/ashx/Basedata/DictronaryHandler.ashx?action=getCurrencyList',
            valueField: 'id',
            textField: 'text'
        });
        if (action == 'edit') {
            if (docno != "") {
                loadForm(docno);
            }
        }

        //--------------上传操作-------------------
        //添加界面的附件管理
        $('#file_upload').uploadify({
            'swf': '/Themes/Scripts/uploadify/uploadify.swf',  //FLash文件路径
            'buttonText': '浏  览',                                 //按钮文本
            'uploader': '/ashx/Finance/BankWaterHandler.ashx?action=load&docno=' + docno,                       //处理文件上传Action
            'queueID': 'fileQueue',                        //队列的ID
            'queueSizeLimit': 10,                          //队列最多可上传文件数量，默认为999
            'auto': false,                                 //选择文件后是否自动上传，默认为true
            'multi': false,                                 //是否为多选，默认为true
            'removeCompleted': true,                       //是否完成后移除序列，默认为true
            'fileSizeLimit': '10MB',                       //单个文件大小，0为无限制，可接受KB,MB,GB等单位的字符串值
            'fileTypeDesc': '支持格式:xls.',               //文件描述
            'fileTypeExts': '*.xls; *.xlsx;',              //上传的文件后缀过滤器
            'onQueueComplete': function (event, data) {    //所有队列完成后事件
                // $.messager.alert("提示", "加载完毕！");    //提示完成
            },
            'onUploadStart': function (file) {
                $('#uploadify').uploadify('settings', 'formData', docno);
            },
            //上传成功后执行
            'onUploadSuccess': function (file, data, response) {
                var result = JSON.parse(data);
                if ('T' == result.status) {
                    $("#tt").datagrid("loadData", result);
                    $('#' + file.id).find('.data').html(' 上传完毕');
                } else {

                    $.messager.alert("提示", "上传发生失败：" + result.msg);
                }
            },
            'onUploadError': function (event, queueId, fileObj, errorObj) {
                $.messager.alert("提示", "加载发生错误：" + errorObj.type + "：" + errorObj.info);
            }
        });

        var editRow = undefined;
        var toolbar = [{
            text: '添加',
            iconCls: 'icon-add',
            handler: function () {
                if (editRow != undefined) {
                    $("#tt").datagrid('endEdit', editRow);
                }
                if (editRow == undefined) {
                    $("#tt").datagrid('insertRow', {
                        index: 0,
                        row: {}
                    });

                    $("#tt").datagrid('beginEdit', 0);
                    editRow = 0;
                }
            }
        }, {
            text: '编辑',
            iconCls: 'icon-edit',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != null) {
                    if (editRow != undefined) {
                        $("#tt").datagrid('endEdit', editRow);
                    }

                    if (editRow == undefined) {
                        var index = $("#tt").datagrid('getRowIndex', row);
                        $("#tt").datagrid('beginEdit', index);
                        editRow = index;
                        $("#tt").datagrid('unselectAll');
                    }
                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }

            }
        }, {
            text: '删除',
            iconCls: 'icon-remove',
            handler: function () {
                var row = $("#tt").datagrid('getSelected');
                if (row != undefined) {
                    var editIndex = $("#tt").datagrid('getRowIndex', row);
                    $('#tt').datagrid('cancelEdit', editIndex)
                    .datagrid('deleteRow', editIndex);
                    editIndex = undefined;

                } else {
                    $.messager.alert('提示', '请选择一行数据！', 'info');
                }
            }
        }, {
            text: '下载模板',
            iconCls: 'icon-download',
            handler: function () {
                downloadModule();
            }
        }, {
            text: '导入',
            iconCls: 'icon-excel',
            handler: function () {
                importExcel();
            }
        }];

        //----初始化datagrid-----
        $('#tt').datagrid({
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            url: '/ashx/Finance/BankWaterHandler.ashx?action=getSubList&docno=' + docno,
            columns: [[
            //{ field: 'DOCNO', title: '导入单号', width: '100px' },
            {field: 'TRADEDATE', title: '交易时间', width: '100px', editor: 'datebox' },
            { field: 'BEGINDATE', title: '起息日期', width: '100px', editor: 'datebox' },
            { field: 'BUSINESSTYPE', title: '业务类型', width: '100px', editor: 'text' },
            { field: 'INAMOUNT', title: '收入金额', width: '100px', editor: { type: 'numberbox', options: { precision: 2}} },
            { field: 'OUTAMOUNT', title: '支出金额', width: '100px', editor: { type: 'numberbox', options: { precision: 2}} },
            { field: 'AMOUNT', title: '余额', width: '100px', editor: { type: 'numberbox', options: { precision: 2}} },
            { field: 'ACCOUNT', title: '对方账号', width: '100px', editor: 'text' },
            { field: 'ACCOUNTNAME', title: '对方户名', width: '100px', editor: 'text' },
            { field: 'TRADEPLACE', title: '交易场所', width: '100px', editor: 'text' },
            { field: 'REMARK', title: '摘要', width: '100px', editor: 'text' },
            { field: 'CONTRACTNO', title: '合同号', width: '100px', editor: 'text' },
            { field: 'PAYAMOUNT', title: '到款金额', width: '100px', editor: { type: 'numberbox', options: { precision: 2}} }
            //{ field: 'INDATE', title: '导入时间', width: '100px'},
            //{ field: 'INMAN', title: '导入人', width: '100px' }
            ]],
            pagination: true,
            toolbar: toolbar,
            onAfterEdit: function (rowIndex, rowData, changes) {
                editRow = undefined;
            },
            onDblClickRow: function (rowIndex, rowData) {
                if (editRow != undefined) {
                    $("#tt").datagrid('endEdit', editRow);
                }

                if (editRow == undefined) {
                    $("#tt").datagrid('beginEdit', rowIndex);
                    editRow = rowIndex;
                }
            },
            onClickRow: function (rowIndex, rowData) {
                if (editRow != undefined) {
                    $("#tt").datagrid('endEdit', editRow);

                }
            }

        });

        $("#uu").dialog({
            title: '上传信息',
            width: 300,
            height: 200,
            closed: true,
            cache: false,
            modal: true,
            maximizable: false,
            buttons: [{
                text: '上传',
                iconCls: 'icon-ok',
                handler: function () {
                    $('#file_upload').uploadify('upload', '*');
                }
            }, {
                text: '取消',
                iconCls: 'icon-cancel',
                handler: function () {
                    $('#file_upload').uploadify('cancel', '*');
                    $("#uu").dialog('close');
                }
            }]
        });
    }

});

//添加
function add() {
    //window.open('/Finance/BankWaterForm.aspx?action=add', "_blank", "");
    newPage('/Finance/BankWaterForm.aspx?action=add', "_blank");
}
//修改
function edit() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.DOCNO;
        window.open('/Finance/BankWaterForm.aspx?action=edit&docno=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//删除
function del() {
    var no = '';
    var row = $("#tt").datagrid('getSelected');
    if (row != undefined) {
        no = row.DOCNO;
        $.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
            //删除数据
            $.post('/ashx/Finance/BankWaterHandler.ashx?action=del&docno=' + no, function (msg) {
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
//导入
function importExcel() {
    $("#uu").dialog().title = "银行流水";
    $("#uu").dialog('open');
}

//下载模板
function downloadModule() {
    window.open('/ashx/Finance/BankWaterHandler.ashx?action=loadModule');
}

//查询操作
function SearchData() {
    docno = $("#docno").val();
    bankname = $("#bankname").val();
    accountname = $("#accountname").val();

    para = {};
    para.docno = docno;
    para.bankname = bankname;
    para.accountname = accountname;

    $("#tt").datagrid('load', para);
}

//加载form表单
function loadForm(docno) {
    $("#form1").form('load', '/ashx/Finance/BankWaterHandler.ashx?action=edit&docno=' + docno);

}
//提交form表单
function submitForm() {
    var action = $("#action").val();
    getSubTable(action); //获取子表信息
    var form = $("#form1");
    form.form('submit', {
        url: "/ashx/Finance/BankWaterHandler.ashx?action=add",
        onSubmit: function () {
            //进行表单验证 
            //如果返回false阻止提交 
            return form.form('validate');
        },
        success: function (data) {
            var result = JSON.parse(data);
            if ("T" == result.status) {
                //msgShow('系统提示', '操作成功', 'info');
                window.opener.refreshParent();
                $.messager.confirm('系统提示', '操作成功，是否确定关闭当前页？', function (r) {
                    //关闭当前页面
                    window.close();
                });
            } else {
                msgShow('系统提示', '操作失败', 'error');
            }
        }
    });
}

function refreshParent() {
    $("#tt").datagrid('load');
 }

//取消（关闭当前窗口）
function closeForm() {
    window.close(); 
 }

//获取子表数据
function getSubTable(action) {
    if ("add" == action) {
        var datagrid = $("#tt").datagrid("getRows");
        var datagridjson = JSON.stringify(datagrid);
        $("#datagrid").val(datagridjson);
    } else {
        var datagrid = $("#tt").datagrid("getRows");
        var datagridjson = JSON.stringify(datagrid);
        $("#datagrid").val(datagridjson);
    }
}

//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
    $.messager.alert(title, msgString, msgType);
}

/**
短暂提示
msg: 显示消息
time：停留时间ms
type：类型 4：成功，5：失败，3：警告
**/
function showTipsMsg(msg, time, type) {
    ZENG.msgbox.show(msg, type, time);
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