var addIndex = 0;
$(function () {
    var pageTag = $("#pageTag").val();
    if (pageTag == "list") {
        //----初始化datagrid-----

        $('#tt_product').datagrid({
            height:550,
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            pageList: [10, 15, 30],
            singleSelect: true,
            idField: 'PREVIEWCODE',
//            url: '/ashx/PreviewManage/previewLoadData.ashx?action=getProductList',
            url: '/ashx/PreviewManage/previewLoadData.ashx?action=getProductPreMList',
            columns: [[
                        { field: 'ck', checkbox: true, width: 50 },

                        { field: 'PREVIEWCODE', title: '预验单号', width: 75 },
//                        { field: 'PACKCODE', title: '包装性能检验结果单', width: 75 },
//                        { field: 'VALIDATY', title: '有效期', width: 75 },
                        { field: 'DELIVERYMAN', title: '发货人', width: 75 },
                        { field: 'REPORTAMOUNT', title: '申报总值', width: 75 },                       
                        { field: 'PRODUCTNAME', title: '产品名称', width: 75 },
                        { field: 'HSCODE', title: 'HS编号', width: 75 },
                        { field: 'TON', title: '报检数/重量', width: 75 },
                        { field: 'USEAMOUNT', title: '已冲减量', width: 75 },
                        { field: 'UNIT', title: '单位', width: 70,
                        formatter: function(value,row,index){
                                      return '吨';
                                    } },
                        { field: 'CURRENCY', title: '币种', width: 75 },
                        { field: 'UNITPRODUCTION', title: '生产单位', width: 75 },
                        { field: 'BATCHNUMBER', title: '批次号', width: 75 },
                        { field: 'PRODUCTIONDATE', title: '生产日期', width: 75,formatter: formatDatebox  },
                        { field: 'TRANSPORT', title: '运输工具', width: 75 },

            ]],
            pagination: true
        });

    }
    if (pageTag == "form") {
        bidUI();
        ////点击添加按钮显示鉴定结果单号
        $("#codeAdd1").click(function () {
          addIndex = addIndex+1;
        var tr1 = "<tr id='"+ addIndex + "' class='layout_t_r'> <td align='right' class='td1_4'>包装性能检验结果单：</td><td class='td2_4' colspan='3'><input class='easyui-textbox' name='packCode' id='packCode' data-options='multiline:true'  style='width: 20%; height: 26px'></input> 有效期：<input class='easyui-datebox' name='PackValidity1' id='PackValidity1'  style='width: 10%; height: 26px'> <a href=\"#\" class=\"easyui-linkbutton\" onclick=\"codeDel(this);\" data-options=\"iconCls:'icon-add'\">删除</a></td></tr>"
        addIndex = addIndex+1;
        var tr2 = "<tr id='"+ addIndex + "' class='layout_t_r' id='resultCode1' style='height: 180px;'> <td align='right' class='td1_4'>包装鉴定结果单号</td><td align='right' class='td2_4' colspan='3'><input class='easyui-textbox' name='conclusionCode1' id='conclusionCode1' style='width: 300px;' ></input>250吨<br /> <br /> <input class='easyui-textbox' name='conclusionCode2' id='conclusionCode2' style='width: 300px;' ></input>250吨<br /> <br /> <input class='easyui-textbox' name='conclusionCode3' id='conclusionCode3' style='width: 300px;'></input>250吨<br /><br /><input class='easyui-textbox' name='conclusionCode4' id='conclusionCode4' style='width: 300px;' ></input>250吨<br /><br />  <input class='easyui-textbox' name='conclusionCode5' id='conclusionCode5' style='width: 300px;'></input>250吨 </td></tr>"
        //var tr3 = "<tr class='layout_t_r' ><td align='right' class='td1_4'>检测单号:</td><td>< input class='easyui-textbox'></input></td></tr>"
        $("#Pretbody").append(tr1);
        $("#Pretbody").append(tr2);
      
        });

        //$("#codeAdd2").click(function () {
     
        //    $("#resultCode2").show();
        //});
        //设置所有的输入项为只读

        //绑定事件
        $('#weight').textbox({  
            onChange: function(value){  
                var weight = $.trim(value); 
                if("" == weight)
                {
                    weight = 0;
                }
                var ton =  parseFloat(weight)/1000;
                $("#ton").textbox('setValue', ton); 
           
            }  
        }); 
      
        if (isBrowse == "True") {
          
            $('input').attr("readonly", true);
        }
      
        //初始化包装
        subdata = JSON.parse(subdata);
        for( var i=0;i< subdata.length;i++)
        {
            if(i==0)
            {
                $("#packCode").textbox('setValue', subdata[i].PACKCODE); 
                $("#PackValidity1").datebox('setValue', subdata[i].VALIDITE);
                var code = subdata[i].CONCLUSIONCODE;
                for( var j=0;j< code.length;j++)
                {
                    $("#conclusionCode"+(j+1)).textbox('setValue', code[j].CONCLUSIONCODE); 
                }
            }else
            {
                addIndex = addIndex+1;
                var tr1 = "<tr id='"+ addIndex + "' class='layout_t_r'> <td align='right' class='td1_4'>包装性能检验结果单：</td><td class='td2_4' colspan='3'><input class='easyui-textbox' name='packCode' id='packCode' data-options='multiline:true' value='"+ subdata[i].PACKCODE +"'  style='width: 20%; height: 26px'></input> 有效期：<input class='easyui-datebox' name='PackValidity1' id='PackValidity1' value='"+ subdata[i].VALIDITE +"'  style='width: 10%; height: 26px'> <a href=\"#\" class=\"easyui-linkbutton\" onclick=\"codeDel(this);\" data-options=\"iconCls:'icon-add'\">删除</a></td></tr>";
                addIndex = addIndex+1;
                var tr2 = "<tr id='"+ addIndex + "' class='layout_t_r' id='resultCode1' style='height: 180px;'> <td align='right' class='td1_4'>包装鉴定结果单号</td><td align='right' class='td2_4' colspan='3'>";
                var code = subdata[i].CONCLUSIONCODE;
                for( var j=0;j< code.length;j++)
                {
                   tr2 = tr2 + " <input class='easyui-textbox' name='conclusionCode"+(j+1)+"' id='conclusionCode"+(j+1)+"' value='"+ code[j].CONCLUSIONCODE +"' style='width: 300px;' ></input>250吨<br /> <br />";
                }
                 tr2 = tr2 +"</td></tr>";
                $("#Pretbody").append(tr1);
                $("#Pretbody").append(tr2);
            }
        }
    }

    
   
});

//数据绑定
function bidUI() {
   
    $('#currency').combobox({
        required: true,
        valueField: 'cname',
        textField: 'cname',
        editable: false,
        multiple: false,
        data: sbCurrency,
      
    });
    $("#transport").combobox({
        required: false,
        valueField: 'cname',
        textField: 'cname',
        editable: false,
        data: sbTransport,

    });
//    $("#productName").combobox({
//        required: false,
//        valueField: 'cname',
//        textField: 'cname',
//        editable: false,
//        data: sbProduct,

//    });
        $('#productName').combogrid(
        {
            panelWidth: 300,
            idField: 'code',
            textField: 'name',
            method: 'get',
            columns: [[
				{ field: 'productcategory', title: '产品大类', width: 80 },
                { field: 'pcode', title: '产品编码', width: 80 },
				{ field: 'cname', title: '产品名称', width: 190 }
			]],
            fitColumns: false,
            onSelect: function (index, rowdata) {
                $("#productName").combogrid('setValue', rowdata.cname);
                $("#HSCode").textbox('setValue', rowdata.pcode);
            }
        }
    );
    $("#productName").combogrid('grid').datagrid('loadData', sbProduct);
    //生产单位(注册号)
    $('#unitProduction').combogrid({
        panelWidth: 300,
        url: '/ashx/Basedata/DictronaryHandler.ashx?action=getProduceUnitList',
        idField: 'code',
        textField: 'cname',
        columns: [[
                    { field: 'id', title: '注册号', width: 100 },
                    { field: 'text', title: '生产单位', width: 200 },
        ]],
        onSelect: function (index, rowdata) {
            $("#unitProduction").combogrid('setValue', rowdata.id);
            $("#unitName").textbox('setValue', rowdata.text);
        }
    });

    $("#deliveryMan").combobox({
        required: false,
        valueField: 'cname',
        textField: 'cname',
        editable: false,
        data: sbCustomer,

    });

    if (isnew == "True") {
      
        //$("#unit").textbox('setValue', "KG");
      
        $("#currency").combobox('setValue', "人民币");
       
        $("#transport").combobox('setValue', "铁路");
    }
    else {
        $("#unit").val(unit);
        $("#currency").combobox('setValue', currency);
        $("#transport").combobox('setValue', transport);
    }

}
//保存
function save(status) {
   
    var retdata = {};
    //获取list数据到htcplistStr
    var action = "";
        if (isnew == "True") {
         
            action = 'addPreview';
        }
        else {
            action = 'editPreview';
        }
   if ($("#form1").form('validate'))
    {
        //从后台提交ajax
        $.ajax({
            cache: true,
            type: "POST",
            url: '/ashx/PreviewManage/previewOperator.ashx?module=' + action,
            data: $('#form1').serialize(), // 你的formid

            async: false,
            error: function (data) {
                retdata.errdata = data;
            },
            success: function (data) {
                var result = JSON.parse(data);
                if (result.status=="T") {
                    $.messager.alert("提醒", "操作成功");
                    top.selectAndRefreshTab('预验管理');
                }
                else {
                    $.messager.alert("提醒", result.msg);
                }
       
            }
        });
    }
}
function setReadOnly() { //设置为只读
    $('input').attr("readonly", true);
}
//查看
function browse() {
    var previewCode = '';
    var packCode = '';
    var row = $("#tt_product").datagrid('getSelected');

    if (row != undefined) {
        previewCode = row.PREVIEWCODE;
        packCode = row.PACKCODE;
        window.top.addNewTab("预验管理-查看", "/Bus/previewManage/preManageForm.aspx?previewCode=" + previewCode + "&packCode=" + packCode+"&browse=true", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
//浏览
function browseProduct() {
    var no = '';
    var row = $("#tt_product").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        alert(no);
        window.open('/Inspect/PreCheckProductForm.aspx?action=editProduct&browse=true&inspectionNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}
function browsePack() {
    var no = '';
    var row = $("#tt_pack").datagrid('getSelected');
    if (row != undefined) {
        no = row.INSPECTIONNO;
        window.open('/Inspect/PreCheckProductForm.aspx?action=editPack&browse=true&inspectionNo=' + no, "_blank", "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//删除
function del() {
    var packCode = '';
    var previewCode = '';
    var row = $("#tt_product").datagrid('getSelected');
    if (row != undefined) {
        previewCode = row.PREVIEWCODE;
        packCode = row.PACKCODE;
        $.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
            if (r) {
                $.post("/ashx/PreviewManage/previewOperator.ashx?module=delPreview&previewCode=" + previewCode + "&packCode=" + packCode, function (data) {
                    if (data=="ok") {
                        $.messager.alert("提醒", "操作成功");
                        $("#tt_product").datagrid("reload");
                    }
                    else {
                        $.messager.alert("提醒", "操作失败");
                    }
                })
            }
        })
    }
    else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }

}
//取消
function undo() {
    window.top.closeTab();
}



//添加
function add() {

    window.top.addNewTab("预验管理-添加", "/Bus/previewManage/preManageForm.aspx", "");
}
//修改
function edit() {
    var previewCode = '';
    var packCode = '';
    var row = $("#tt_product").datagrid('getSelected');
  
    if (row != undefined) {
        previewCode = row.PREVIEWCODE;
        packCode = row.PACKCODE;
        window.top.addNewTab("预验管理-修改", "/Bus/previewManage/preManageForm.aspx?previewCode=" + previewCode+"&packCode="+packCode, "");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
}

//查询操作
function SearchPreviewData() {
    inspectionno_product = $("#previewCode").val();
    inspecman_product = $("#deliveryMan").val();
    pname_product = $("#productName").val();

    para = {};
    para.previewCode = inspectionno_product;
    para.deliveryMan = inspecman_product;
    para.productName = pname_product;

    $("#tt_product").datagrid('load', para);
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

function codeDel(ele)
{
    var object = ele.parentElement.parentElement.id;
    $("#"+object).remove();
    $("#"+(parseInt(object)+1)).remove();    
}