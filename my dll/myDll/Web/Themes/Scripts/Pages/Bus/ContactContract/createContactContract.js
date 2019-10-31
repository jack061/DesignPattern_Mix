var editIndex = undefined;
$(document).ready(function () {
    $("#form").form('load', '/Inspect/Inspection12.ashx?action=LoadApp&contractNo=' + $("#contractNo").val());
        //申请关联合同表
        $('#sub').datagrid({
            rownumbers: true,
            singleSelect: true,
            sortName: 'pcode',
            sortOrder: 'asc',
            url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + $("#contractNo").val(),
            columns: [[
		    { field: 'pcode', title: 'SAP编号', width: 100 },
		    { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
            { field: 'packspec', title: '包装规格', width: 100 },
            { field: 'qunit', title: '单位', width: 70 },
            { field: 'quantity', title: '合同数量', width: 100 },
            { field: 'insQuantity', title: '关联产品数量', width: 100, editor: { type: 'numberbox' } },
            ]],
            onClickCell: function (index, row, changes) {
                var row = $("#sub").datagrid('getSelected');
                if (editIndex != index) {
                    if (endEditing()) {
                        $(this).datagrid('beginEdit', index);
                    } else {
                        $(this).datagrid('endEdit', editIndex);
                        $(this).datagrid('beginEdit', index);
                        editIndex = index;
                    }
                }
            },
            onAfterEdit: function (index, row, changes) {

            },
            onDblClickRow: function (index, row) {
                $(this).datagrid('endEdit', index);
                editIndex = undefined;
            },
            pagination: false
        });
        //已关联合同列表
        $('#tt').datagrid({
            rownumbers: true,
            singleSelect: true,
            sortName: 'pcode',
            sortOrder: 'asc',
            url: '/Inspect/Inspection12.ashx?action=LoadttApp&contractNo=' + $("#contractNo").val(),
            columns: [[
		    { field: 'BUYER', title: '买方', width: 200 },
		    { field: 'SELLER', title: '卖方', width: 200 },
		    { field: 'SIGNEDTIME', title: '签订时间', width: 100, formatter: formatDatebox },
		    { field: 'PCODE', title: 'SAP编号', width: 100 },
		    { field: 'PNAME', title: 'SAP名称', width: 100, align: 'center' },
            { field: 'PACKSPEC', title: '包装规格', width: 100 },
            { field: 'QUNIT', title: '单位', width: 50 },
            { field: 'INSQUANTITY', title: '商检数量', width: 70 },
         
            ]],
        });
        //合同产品子表
        $('#htcplist').datagrid({
            url: '/ashx/Contract/contractData.ashx?module=htcppagelist&isall=1&contractNo=' + $("#contractNo").val(),
            rownumbers: true,
            singleSelect: true,
            sortName: 'pcode',
            sortOrder: 'asc',
            columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox' } },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '价格单位', width: 100, editor: { type: 'textbox' } },
                    { field: 'packspec', title: '包装规格', width: 100, editor: 'textbox' },
                    { field: 'packing', title: '包装描述', width: 100, editor: 'textbox' },
                    { field: 'pallet', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否' } }, align: 'center' },
          
            ]]
          
        });
    //加载combobox及简称带出全称
        loadCombobox();
});
//加载combobox及简称带出全称
function loadCombobox() {
    $("input", $("#simpleSendMan").next("span")).blur(function () {
        var simpleSeller = ($("#simpleSendMan").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=seller", { simpleSeller: simpleSeller }, function (data) {
            $("#sendMan").combobox('setValue', data.name);
            $("#sellercode").val(data.code);

        }, 'json')
    })
    $('#sendMan').combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        required: true,
        valueField: 'name',
        textField: 'name',
        editable: false,
        onSelect: function ( record) {
            $("#simpleSendMan").textbox('setValue', record.shortname);
            $("#sellercode").val(record.code);
        }
    });
}
var endEditing = function () {
    if (editIndex == undefined) { return true }
    else { return false; }
}
//查询操作
function SearchData() {
    contractNo = $("#contract").val();
    //attachmentno = $("#attachmentno").val();
    signedtime_begin = $('#signedtime_begin').datebox('getValue');
    signedtime_end = $('#signedtime_end').datebox('getValue');
    buyer_sch = $('#buyer_sch').textbox('getValue');
    seller_sch = $('#seller_sch').textbox('getValue');
    //ifchecked = $("input:radio[name='partInfo']:checked").val();

    para = {};
    //para.contractNo = contractNo;
    //para.attachmentno = attachmentno;
    para.signedtime_begin = signedtime_begin;
    para.signedtime_end = signedtime_end;
    para.buyer_sch = buyer_sch;
    para.seller_sch = seller_sch;
    //para.ifchecked = ifchecked;
    $("#tt").datagrid('load', para);
}
//提交操作
function submitApply() {
    var str = $("#sendMan").combobox("getText");
    if (str == '') {
        $.messager.alert("警告", "请选择发货人！");
    } else {
        getSubTable();
        $.ajax({
            cache: true,
            type: "POST",
            url: '/ashx/Contract/contractOperater.ashx?module=saveCotactContract&flowdirection=出境',
            data: $('#form').serialize(), // 你的formid
            async: false,
            error: function (data) {
                alert("提交操作错误");
            },
            success: function (data) {
                if (data === 'ok') {
                    $.messager.alert('返回信息', '提交成功');
                    //closedd();//关闭窗口
                } else {
                    alert("操作失败");
                }
            }
        });
    }
}
//将列表加入隐藏域
function getSubTable() {
    var datagrid = $("#sub").datagrid("getRows");
    var datagridjson = JSON.stringify(datagrid);
    $("#datagrid").val(datagridjson);
}
//关闭tab
function closedd() {
  
    top.closeTab();
}