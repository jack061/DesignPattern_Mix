var endEditing = function () {
    if (editIndex == undefined) { return true }
    if ($('#htcplist').datagrid('validateRow', editIndex)) {
        $('#htcplist').datagrid('endEdit', editIndex);
        editIndex = undefined;
        return true;
    } else {
        return false;
    }
}
//保存
var save = function () {
   

    var rrdata = SaveDataToDB(0);
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        $.messager.alert('系统提示', '保存成功!', 'info', function () {
            //打开指定模板页面
            window.top.selectTab('境外销售合同');

        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
}
//取消
var undo = function () {
   
    //关闭当前tab
    window.top.closeTab();
}

//提交
var submit = function () {
 
    var rrdata = SaveDataToDB(1);
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '1') {
        $.messager.alert('系统提示', '提交成功!', 'info', function () {
            //打开指定模板页面
            window.top.selectTab('境外销售合同');

        });
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        alert(rrdata.sucdata.errormsg);
    }
};

var SaveDataToDB = function (status) {
    if (endEditing() != true) {
        return;
    }
    
    var retdata = {};
    //获取list数据到htcplistStr
    var cplist = $("#htcplist").datagrid("getRows");
    var datagridjson = JSON.stringify(cplist);
    //alert(datagridjson);
    $('#htcplistStr').attr('value', datagridjson);
    //alert($('#htcplistStr').val());
    var action = "";
    if (contractNo.length > 0) {
    
        if (isview=="true") {
            action = 'copycontract';
        }
        else {
            action = 'editcontract';
        }
        action = 'editcontract';
   
    
    }
    else {
        action = 'addcontract';
    }
   
    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: 'ajax/MyOperater.ashx?module=' + action+'&status='+status,
        data: $('#form1').serialize(), // 你的formid
         
        async: false,
        error: function (data) {
            retdata.errdata = data;
        },
        success: function (data) {
            
            retdata.sucdata = data;
        }
    });
    return retdata;
}
console.time('初始化');
$(function () {

    $('#language').combobox({
        required: true,
        valueField: 'cname',
        textField: 'cname',
        editable: false,
        multiple: true,
        data: combLanaguage,
        onSelect: function (index, rowdata) {
            var str = $('#language').combobox('getText');
            if (str.substr(0, 1) == ',') {
                str = str.substr(1);
            }
            //去除重复字符串
            var strArr = str.split(',');//把字符串分割成一个数组
            strArr.sort();//排序
            var result = new Array();//创建出一个结果数组
            var tempStr = "";
            for (var i in strArr) {
                if (strArr[i] != tempStr) {
                    result.push(strArr[i]);
                    tempStr = strArr[i];
                }
                else {
                    continue;
                }
            }
            str = result.join(',');
         
            $('#language').combobox('setValue', str);
          
        }

      

    });
    $('#language').combobox('setValue', htdata.language);
  
  
    console.time('bindui方法');
    bindUI();
    console.timeEnd('bindui方法');
    //绑定grid
    console.time('附件列表');
    $('#htfjlist').datagrid({
        url: 'ajax/AbroadLoadData.ashx?module=htfjpagelist&contractNo=' + htdata.contractNo,
        pagination: true,
        rownumbers: true,
        sortName: 'attachmentno',
        sortOrder: 'asc',
        columns: [[
                    { field: 'status1', title: '状态', width: 100 },
                    { field: 'templateno', title: '模板编号', width: 100 },
		            { field: 'attachmentno', title: '附件编号', width: 100 },
		            { field: 'language', title: '语言', width: 100 },
		            { field: 'seller', title: '卖方', width: 100, align: 'right' },
                    { field: 'signedtime', title: '签订时间', width: 100 },
                    { field: 'signedplace', title: '签订地点', width: 100 },
                    { field: 'buyer', title: '买家', width: 100 },
                    { field: 'buyeraddress', title: '买家地址', width: 100 },
                    { field: 'currency', title: '货币', width: 100 },
                    { field: 'pricement1', title: '价格条款1', width: 100 },
                    { field: 'pricement1per', title: '价格条款1占比', width: 100 },
                    { field: 'pricement2', title: '价格条款2', width: 100 },
                    { field: 'pricement2per', title: '价格条款2占比', width: 100 },
                    { field: 'pvalidit', title: '价格有效期', width: 100 },
                    { field: 'shipment', title: '发运条款', width: 100 },
                    { field: 'transport', title: '运输方式', width: 100 },
                    { field: 'tradement', title: '贸易条款', width: 100 },
                    { field: 'harborout', title: '出口口岸', width: 100 },
                    { field: 'harborarrive', title: '到货口岸', width: 100 },
                    { field: 'harborclear', title: '清关口岸', width: 100 },
                    { field: 'placement', title: '产地条款', width: 100 },
                    { field: 'validity', title: '合同有效期', width: 100 },
                    { field: 'remark', title: '备注', width: 100 }
                ]],
        toolbar: [/*{
                    iconCls: 'icon-add',
                    text: '新增',
                    handler: function () { alert('add') }
                }, '-', {
                    iconCls: 'icon-edit',
                    text: '修改',
                    handler: function () { alert('修改') }
                }*/]
    });
    console.timeEnd('附件列表');

    console.time('产品列表');
    $('#htcplist').datagrid({
        url: 'ajax/AbroadLoadData.ashx?module=htcppagelist&isall=1&contractNo=' + contractNo,
        rownumbers: true,
        singleSelect: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'quantity', title: '数量', width: 100, editor: { type: 'numberbox'} },
                    { field: 'qunit', title: '数量单位', width: 100 },
                    { field: 'price', title: '价格', width: 100, editor: { type: 'numberbox', options: { precision: 3}} },
                    { field: 'amount', title: '金额', width: 100, editor: { type: 'numberbox', options: { precision: 3}} },
                    { field: 'packspec', title: '包装规格', width: 100, editor: 'textbox' },
                    { field: 'packing', title: '包装描述', width: 100, editor: 'textbox' },
                    { field: 'pallet', title: '托盘要求', width: 100, editor: 'textbox' },
                    { field: 'ifcheck', title: '是否商检', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否'} }, align: 'center' },
                    { field: 'ifplace', title: '是否产地证', width: 100, editor: { type: 'checkbox', options: { on: '是', off: '否'} }, align: 'center' }
                ]],
        toolbar: [{
            iconCls: 'icon-add',
            text: '新增',
            handler: function () {
                //弹出
                $('#dd').window('open');
            }
        }, '-', {
            iconCls: 'icon-remove',
            text: '删除',
            handler: function () {
                var rowindex = $('#htcplist').datagrid('getRowIndex', $('#htcplist').datagrid('getSelected'));
                $('#htcplist').datagrid('deleteRow', rowindex);
            }
        }],
        onClickCell: function (index, field, value) {
            if (editIndex != index) {
                var mygrid = $('#htcplist');
                if (endEditing()) {
                    $(this).datagrid('beginEdit', index);
                    //                            var ed = $(this).datagrid('getEditor', {index:index,field:field});
                    //                            if($(ed.target) != undefined){
                    //		                        $(ed.target).focus();
                    //                            }
                    editIndex = index;
                } else {
                    setTimeout(function () {
                        mygrid.datagrid('selectRow', editIndex);
                    }, 0);
                }
            }
        },
        onAfterEdit: function (index, row, changes) {
            row.amount = row.quantity * row.price;
            $('#htcplist').datagrid('refreshRow', index);
        }
    });
    console.timeEnd('产品列表');
    //价格百分比文本框改变事件
    $("#pricement1per").numberbox({
    
        min: 0,
        max: 100,
        precision: 2,
        suffix: '%',
        onChange: function () {
            //获取文本框中的值
            var v = $('#pricement1per').numberbox('getValue');
            $("#pricement2per").numberbox('setValue', parseFloat(100) - parseFloat(v));
            
        }
    })
    $("#pricement2per").numberbox({

        min: 0,
        max: 100,
        precision: 2,
        suffix: '%',
        onChange: function () {
            //获取文本框中的值
            var v = $('#pricement2per').numberbox('getValue');
            $("#pricement1per").numberbox('setValue', parseFloat(100) - parseFloat(v));

        }
    })
    
});


var bindUI = function () {
    $("#no").val(htdata.contractNo);
    $('#dd').dialog({
        title: '请选择产品',
        width: 600,
        height: document.documentElement.clientHeight,
        closed: true,
        cache: false,
        //href: 'Abroad_Product_Select.aspx',
        modal: true,
        buttons: [{
            text: '选择',
            handler: function () {
                $('#dd').dialog('close');
                //把选择的产品加载到当前页面
                var rows = $('#productlist').datagrid('getSelections');

                var oldrows = $('#htcplist').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    //判断当前表格里面是否有pcode
                    var isexists = false;
                    for (var j = 0; j < oldrows.length; j++) {
                        if (oldrows[j].pcode == rows[i].pcode) {
                            isexists = true;
                            break;
                        }
                    }
                    if (isexists == false) {
                        var row = rows[i];
                        var newrow = {};
                        newrow.pcode = row.pcode;
                        newrow.pname = row.pname;
                        newrow.packspec = row.spec;
                        newrow.pallet = row.pallet;
                        newrow.packdes = row.packdes;
                        newrow.qunit = row.unit;
                        newrow.ifcheck = row.ifinspection;
                        newrow.price = 0;
                        newrow.quantity = 0;
                        newrow.amount = 0;

                        $('#htcplist').datagrid('appendRow', newrow);
                    }
                }
                $('#productlist').datagrid('clearSelections');
                if (endEditing()) {
                    editIndex = $('#htcplist').datagrid('getRows').length - 1;
                    $('#htcplist').datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
                }
            }
        }, {
            text: '取消',
            handler: function () {
                $("#dd").dialog('close');
            }
        }]
    });
    //加载销售合同编号
    $('#purchaseCode').combobox({
        required: false,
        valueField: 'contractNo',
        textField: 'contractNo',
        editable: false,
        data: sbContractNo,
        onSelect: function (rowdata) {

            var purchaseCode = $('#purchaseCode').combobox("getText");

            $.post("/Bus/Xctl/ajax/AbroadLoadData.ashx", { module: "attach", contractNo: purchaseCode }, function (data) {

                $("#seller").combogrid("setValue", data.rows[0].buyer);

             
                $('#language').combobox('setValue', data.rows[0].language);
              
                $('#transport').combobox('setValue', data.rows[0].transport);
                $('#currency').combobox('setValue', data.rows[0].currency);


            });


        }
    });
    $('#purchaseCode').combobox('setValue', htdata.purchaseCode);
    $('#productlist').datagrid({
        url: 'ajax/AbroadLoadData.ashx?module=cplist',
        pagination: true,
        rownumbers: true,
        sortName: 'pcode',
        sortOrder: 'asc',
        columns: [[
		            { field: 'pcode', title: 'SAP编号', width: 100 },
		            { field: 'pname', title: 'SAP名称', width: 100, align: 'center' },
                    { field: 'unit', title: '单位', width: 100 },
                    { field: 'spec', title: '规格', width: 100 },
                    { field: 'packdes', title: '包装描述', width: 100 },
                    { field: 'pallet  ', title: '包装规格', width: 100 },
                    { field: 'hsscode', title: 'HSS编码', width: 100 },
                    { field: 'ifinspection', title: '是否商检', width: 100 },
        ]]
    });
   
    $('#maintable').form('load', htdata);


    if (isnew == "true") {
    
        $('#contractNo').textbox('setValue', '自动编号');
        $('#contractNo').textbox('readonly', true);
        $("#status").textbox('setValue', '新建');
        $('#status').textbox('readonly', true);


    }
    else {
        //如果是复制合同
        if (isview == "true") {
        
            $('#contractNo').textbox('setValue', '自动编号');
            $("#status").textbox('setValue', '新建');
            $('#status').textbox('readonly', true);

        }
    }
   

    var type2_1 = [{ id: "直销", text: "直销" }, { id: "转口", text: "转口"}];
    var type2_2 = [{ id: "代理出口", text: "代理出口" }, { id: "国内销售", text: "国内销售"}];


    $('#contracttype').combobox({
        required: true,
        valueField: 'id',
        textField: 'text',
        editable: false,
        data: [{ id: "外销", text: "外销" }, { id: "内销", text: "内销"}],
        onSelect: function (item) {
            if (item.id == '外销') {
                $('#contracttype2').combobox('loadData', type2_1);
                $('#contracttype2').combobox('setValue', '直销');
            }
            else if (item.id == '内销') {
                $('#contracttype2').combobox('loadData', type2_2);
                $('#contracttype2').combobox('setValue', '代理出口');
            }
        }
    });
    //为价格条款占比添加文本框改变事件
    $("#pricement1per").onblur = function () {

    }
    $('#contracttype2').combobox({
        required: true,
        valueField: 'id',
        textField: 'text',
        editable: false,
        data:function () {
                if (htdata.contracttype == '内销') {
                    return type2_2;
                }
                else {
                    return type2_1;
                }
            }
});
    $('#contracttype2').combobox('setValues', htdata.contracttype2);



  
 


    $('#transport').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: comTrans
    });
    $('#transport').combobox('setValue', htdata.transport);

    $('#currency').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        multiple: false,
        data: comCurrey
    });
    $('#currency').combobox('setValue', htdata.currency);


    $('#businessclass').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbXsz
    });
    $('#businessclass').combobox('setValue', htdata.businessclass);
    $('#tradement').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbTradement
    });
    $('#tradement').combobox('setValue', htdata.tradement);
    $('#validity').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbValidity
    });
    $('#validity').combobox('setValue', htdata.validity);

    $('#shipment').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbFytk
    });
    $('#shipment').combobox('setValue', htdata.shipment);

    $('#pricement1').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbJgtk1
    });
    $('#pricement1').combobox('setValue', htdata.pricement1);


    $('#pricement2').combobox({
        required: true,
        valueField: 'code',
        textField: 'cname',
        editable: false,
        data: sbJgtk2
    });
    $('#pricement2').combobox('setValue', htdata.pricement2);

    $('#seller').combogrid({
        panelWidth: 450,
        //value:htdata.seller,
        idField: 'code',
        textField: 'name',
        data: comSeller,
        editable: false,
        columns: [[
                    { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
                ]],
        onSelect: function (index, rowdata) {
            $('#sellername').attr('value',rowdata.name);
        }
    });
    $("#seller").combogrid("setValue", htdata.seller);
    $('#buyer').combogrid({
        panelWidth: 450,
        //value:htdata.buyer,
        idField: 'code',
        textField: 'name',
        data: comBuyer,
        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
                ]],
        onSelect: function (index, rowdata) {
            $('#buyeraddress').textbox('setValue', rowdata.address);
            $('#buyername').attr('value', rowdata.name);
        }
    });
    $("#buyer").combogrid("setValue", htdata.buyer);
    console.time('fff');
    //模板编号列表
    $('#templateno').combogrid({
        panelWidth: 200,
        //value:htdata.seller,
        idField: 'templateno',
        textField: 'templateno',
        data: comMbGridData,
        url: '',
        editable: false,
        columns: [[
                    { field: 'templatetype', title: '模板类型', width: 80 },
                    { field: 'templateno', title: '模板编号', width: 100 },
                ]]
    });
    console.timeEnd('fff');
    $("#templateno").combogrid("setValue", htdata.templateno);

    console.time('eee');
    $('#harborout').combogrid({
        panelWidth: 450,
        idField: 'code',
        textField: 'name',
        data: comPortGrid,
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
                ]],
        onSelect: function (index, rowdata) {

        }
    });
    $('#harborarrive').combogrid({
        panelWidth: 450,
        idField: 'code',
        textField: 'name',
        data: comPortGrid,
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
                ]],
        onSelect: function (index, rowdata) {

        }
    });
    $('#harborclear').combogrid({
        panelWidth: 450,
        idField: 'code',
        textField: 'name',
        data: comPortGrid,
        columns: [[
                    { field: 'code', title: '编码', width: 60 },
                    { field: 'country', title: '国家', width: 80 },
                    { field: 'name', title: '中文名', width: 100 },
                    { field: 'egname', title: '英文名', width: 100 },
                    { field: 'runame', title: '俄文名', width: 100 }
                ]],
        onSelect: function (index, rowdata) {

        }
    });
    console.timeEnd('eee');
    $('#harborclear').combogrid('setValue', htdata.harborclear);
    $('#harborarrive').combogrid('setValue', htdata.harborarrive);
    $('#harborclear').combogrid('setValue', htdata.harborclear);
    if (isnew == true) {
        $('#createman').textbox('setValue', userid);
        $('#status1').textbox('setValue', '新建');
    }
    else {
        $('#status1').textbox('setValue', htdata.status);
    }
    //alert(htdata.signedtime);
    if (isview == 'true') {
        $('#aa123').hide();
    }
}
console.timeEnd('初始化');

