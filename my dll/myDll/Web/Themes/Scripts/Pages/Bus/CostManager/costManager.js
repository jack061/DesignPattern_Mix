$(function () {
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
    
        $('#tt').datagrid({
            height:550,
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'contractNo',
            url: '/ashx/CostManager/costList.ashx?action=getList',
            columns: [[
            { field: 'status', title: '状态', width: '100px', editor: 'text' },
            { field: 'contractNo', title: '合同编号', width: '100px', editor: 'text' },
            { field: 'attachmentNo', title: '附件编号', width: '100px', editor: 'text' },
            { field: 'department', title: '申请部门', width: '100px', editor: 'text' },
            { field: 'purseApplication', title: '款项用途', width: '100px', editor: 'text' },
            { field: 'amountName', title: '收款单位名称', width: '100px', editor: 'text' },
            { field: 'contractAmount', title: '合同金额', width: '100px', editor: 'text' },
            { field: 'amountPaid', title: '已付金额', width: '100px', editor: 'text' },
            { field: 'foreignAmount', title: '外币金额', width: '100px', editor: 'text' },
            { field: 'amount', title: '金额', width: '100px', editor: 'text' },
            { field: 'payingBank', title: '汇入银行', width: '100px', editor: 'text' },
            { field: 'account', title: '账号', width: '100px', editor: 'text' },
            { field: 'bankCode', title: '联行号', width: '100px', editor: 'text' },
            { field: 'bankAddress', title: '开户行地址', width: '100px', editor: 'text' },
            { field: 'payment', title: '支付方式', width: '100px', editor: 'text' },
            { field: 'applicationDate', title: '申请日期', width: '100px', editor: 'text', formatter: formatDatebox },
            { field: 'applicant', title: '申请人', width: '100px', editor: 'text' },
            { field: 'departmentCharge', title: '部门负责人', width: '100px', editor: 'text' },
            { field: 'departmentLeader', title: '部门领导', width: '100px', editor: 'text' },
            { field: 'financialManager', title: '财务负责人', width: '100px', editor: 'text' },
            { field: 'financialLeader', title: '财务领导', width: '100px', editor: 'text' },
            { field: 'chairMan', title: '董事长', width: '100px', editor: 'text' },
            ]],
            pagination: true
        });
    }
    else if (pageTag == "form") {
        var contractNo = "";
     
        $('#contractNo').combobox({
            required: true,
            valueField: 'contractNo',
            textField: 'contractNo',
            editable: false,
            multiple: false,
            data: sbContractNo,
            onSelect: function (index, rowdata) {
                var contractNo = $(this).combobox('getText');
                //根据合同编号加载附件编号
                $.post("/ashx/")
            }
        });
        $('#contractNo').combobox('setValue', "");

        $('#payment').combobox({
            required: true,
            valueField: 'name',
            textField: 'name',
            editable: false,
            multiple: false,
            data: sbPayment,
            onSelect: function (index, rowdata) {

            }
        });
        $('#payment').combobox('setValue', "");
        
        //填充数据
      
        $('#maintable').form('load', htdata);
        //设置所有的输入项为只读
        if (isBrowse == "True") {

            $('input').attr("readonly", true);
          
        }
    }
    });

//添加
function add() {
 
    window.top.addNewTab("费用管理-新增", "/Bus/CostManagement/CostManagementForm.aspx");

}
//修改
function edit() {
  
    var no = "";
    var row = $("#tt").datagrid("getSelected");
    if (row != undefined) {


        no = row.contractNo;

        window.top.addNewTab("费用管理-修改", "/Bus/CostManagement/CostManagementForm.aspx?contractNo=" + no);
    }
    else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//搜索
function SearchData() {
    contractNo = $("#contractNo").val();
    applicant = $("#applicant").val();
    department = $("#department").val();

    para = {};
    para.contractNo = contractNo;
    para.applicant = applicant;
    para.department = department;

    $("#tt").datagrid('load', para);
}
//保存
function save() {
    var action = "";
    if (isNew == "true") {
      
        action = "adddManager";
    }
    else {
        action = "editManager";
    }
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: '/ashx/CostManager/costChange.ashx?module=' + action,
        data: $('#form1').serialize(), // 你的formid
        async: false,
        error: function (data) {
            retdata.errdata = data;
        },
        success: function (data) {
           
            if (data == "True") {
               
                $.messager.alert("提醒", "操作成功");
                window.top.selectTab("费用管理");
            }
            else {
                $.messager.alert("提醒", "添加失败");
            }
        }
    });
}
//取消
function undo() {
    window.top.closeTab();

}
//删除
function del() {
    var row = $("#tt").datagrid('getSelected');
    no = row.contractNo;
    if (row == undefined) {
        $.messager.alert("提醒", "请至少选择一条数据");
        return;
    }
   
    $.messager.confirm('系统提示', '确认要删除么？', function (r) {
        if (r) {
            $.ajax({
                cache: true,
                type: "POST",
                url: '/ashx/CostManager/costChange.ashx?module=deletecontract&contractNo=' + no,
                data: {},
                async: false,
                error: function (data) {
                    $.messager.alert('系统提示', '后台操作失败!', 'info');
                },
                success: function (data) {
                    if (data=="True") {
                        $.messager.alert('系统提示', '删除成功!', 'info');
                        $("#tt").datagrid("reload", {});
                    }
                    else {
                      
                        $.messager.alert('系统提示', '删除失败!' + data.errormsg, 'info');
                    }
                }
            });
        }
    });
}
//查看
function browse() {
    var no = "";
    var row = $("#tt").datagrid("getSelected");
    if (row != undefined) {


        no = row.contractNo;

        window.top.addNewTab("费用管理-查看", "/Bus/CostManagement/CostManagementForm.aspx?contractNo=" + no+"&browse=true");
    }
    else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

