

$(function () {
    if (isFrameAttach == "true") {//加载框架子合同信息
        $("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadServiceAttachData&contractNo=' + contractNo);
    }
    else if (isContact == "true" || isCopy == "true") {
        $("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadServiceContactData&contractNo=' + contractNo);
    }
    else {
        $("#form1").form('load', '/ashx/Contract/reviewContractData.ashx?module=LoadServiceData&contractNo=' + contractNo);
    }
    bindCombobox();
    bindCostCategory();//绑定费用类别表
    bindUI();
    loadTotalName();
    selectTabs();
})
//绑定数据
function bindUI() {
    $("#outsideTr").hide();
    if (isOutside == "true") {
        $("#outsideTr").show();
        initFile(); //初始化文件路径
        checkAttachUp();//查看上传文件
    }
   
    if (isnew == "True" || isFrameAttach == "true" || isContact == "true" || isCopy == "true") {
        $('#contractNo').textbox('setValue', '自动编号');
        $('#contractNo').textbox('readonly', true);
        //$("#frameContractNo").val(frameContractNo);//为框架合同号赋值
        $("input[name='isFrame'][value='否']").attr("checked", true);
    }
    else {
        $('#contractNo').textbox('setValue', contractNo);
        $('#contractNo').textbox('readonly', true);
    }
    if (isFrame == "true") {

        $("input[name='isFrame'][value='是']").attr("checked", true);
    }
    if (isBrowse == "true") {
        $('input').attr('disabled', true);
        $("#aa123").css("display", "none");
    }
}

//保存数据
function save() {
    var rrdata = SaveDataToDB(0);
    if (rrdata != undefined && rrdata.sucess == '1') {
        window.top.selectAndRefreshTab('物流合同');
    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        $.messager.alert("提醒", rrdata.sucdata.errormsg);
    }
}
//提交
var submit = function () {

    var rrdata = SaveDataToDB(1);
    if (rrdata != undefined && rrdata.sucess == '1') {
        window.top.selectAndRefreshTab('物流合同');

    }
    if (rrdata.sucdata != undefined && rrdata.sucdata.sucess == '0') {
        //像用户提示错误
        $.messager.alert("提醒", rrdata.sucdata.errormsg);
    }
};
//取消
var undo = function () {

    //关闭当前tab
    window.top.closeTab();
}

var SaveDataToDB = function (status) {
    if (isContact == "true") {//为关联合同号赋值
        $("#associateCode").val(contractNo);
    }
    var url = '';
    $("#costCategoryTable").datagrid("acceptChanges");
    var categoryList = $("#costCategoryTable").datagrid("getRows");
    var datagridjson = JSON.stringify(categoryList);
    $('#categoryList').val(datagridjson);
    $('#htmlcontent').attr('value', editor123.html());//获取条款文本
    if (isFrameAttach == "true") {
        url = '/ashx/Contract/contractOperater.ashx?module=addServiceContract&status=' + status + '&isFrameAttach=true&isCopy=' + isCopy+'&isEdit='+isEdit;
    }
    else {
        url = '/ashx/Contract/contractOperater.ashx?module=addServiceContract&status=' + status + '&isCopy=' + isCopy + '&isEdit=' + isEdit;
    }

    //从后台提交ajax
    $.ajax({
        cache: true,
        type: "POST",
        url: url,
        data: $('#form1').serialize(), // 你的formid
        dataType: "json",
        async: false,
        error: function (data) {
            retdata.errdata = data;
        },
        success: function (data) {
            retdata = data;
        }
    });
    return retdata;
}

//tabs选择事件
function selectTabs() {
    $('#tt').tabs({
        border: false,
        onSelect: function (title) {
            $('#costCategoryTable').datagrid('acceptChanges');
            var cplist = $("#costCategoryTable").datagrid("getRows");
            var datagridjson = JSON.stringify(cplist);
            $('#htmlcontent').attr('value', editor123.html());//获取条款文本
            var signedTime = $("#signedTime").datebox('getValue');
            var signedplace = $("#signedPlace").textbox('getValue');
            var buyerCode = $("#buyerCode").val();
            var sellerCode = $("#sellerCode").val();
            var partyCCode = $("#partyCCode").val();
            var partyDCode = $("#partyDCode").val();
            var partyA = $("#buyer").combogrid('getValue');
            var partyB = $("#seller").combogrid('getValue');
            var partyC = $("#partyC").combogrid('getValue');
            var partyD = $("#partyD").combogrid('getValue');
            var contractText = $("#htmlcontent").val();
            var contractNo = $("#contractNo").textbox('getValue');
            var validity = $("#validity").datebox('getValue');
            if (title == "合同预览") {
                $.ajax({
                    type: "post",
                    url: "/ashx/Contract/contractData.ashx?module=GetServicePreview",
                    dataType: "text",
                    data: {
                        buyerCode: buyerCode, signedtime: signedTime,
                        signedplace: signedplace, language: "中文", sellerCode: sellerCode,
                        contractText: contractText, contractNo: contractNo, partyA: partyA, partyB: partyB, partyC: partyC,
                        partyD: partyD, datagridjson: datagridjson, partyCCode: partyCCode, partyDCode: partyDCode, validity: validity

                    },
                    success: function (data) {
                        $("#realTimeContractText").val(data);
                    }
                });
                $("#previewFormFrame").attr('src', '/Bus/ContractCategory/realTimePreviewLogisticsContract.aspx');
            }
            if (title == "审核日志") {
                $("#reviewTable").datagrid({
                    height: 300,
                    width: 700,
                    nowrap: true,
                    fitColumns: true,
                    striped: true,
                    collapsible: true,
                    pageList: [10, 15, 30],
                    singleSelect: true,
                    idField: 'contractNo',
                    url: '/ashx/Contract/contractData.ashx?module=GetServiceReviewList&contractNo=' + contractNo,
                    columns: [[
                    { field: 'reviewstatus', title: '审核节点', width: '100px', editor: 'text' },
                    { field: 'status', title: '状态', width: '100px', editor: 'text' },
                    { field: 'reviewdate', title: '审核时间', width: '100px', editor: 'text' },
                    { field: 'reviewlog', title: '审核日志', width: '100px', editor: 'text' },
                     { field: 'reviewman', title: '审核人', width: '100px', editor: 'text' },
                    ]],
                    pagination: false,
                })

            }

        }
    })
}


//根据卖方买方简称加载全称和地址
function loadTotalName() {
    //卖方(简称)文本框改变事件
    $("input", $("#simpleSeller").next("span")).blur(function () {
        var simpleSeller = ($("#simpleSeller").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=seller", { simpleSeller: simpleSeller }, function (data) {
            $("#simpleSeller").textbox('setValue', data.shortname);
            $("#seller").combogrid('setValue', data.name);
            $('#sellerCode').attr('value', data.code);

        }, 'json')

    })
    //买方(简称)文本框改变事件
    $("input", $("#simpleBuyer").next("span")).blur(function () {

        var simpleBuyer = ($("#simpleBuyer").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleBuyer: simpleBuyer }, function (data) {
            $("#simpleBuyer").textbox('setValue', data.shortname);
            $("#buyer").combogrid('setValue', data.name);
            $('#buyerCode').attr('value', data.code);

        }, 'json')

    })
    //丙方(简称)文本框改变事件
    $("input", $("#simplePartyC").next("span")).blur(function () {

        var simplePartyC = ($("#simplePartyC").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleSeller: simplePartyC }, function (data) {
            $("#simplePartyC").textbox('setValue', data.shortname);
            $("#partyC").combogrid('setValue', data.name);
            $('#partyCCode').attr('value', data.code);

        }, 'json')

    })
    //丁方(简称)文本框改变事件
    $("input", $("#simplePartyD").next("span")).blur(function () {
        var simplePartyD = ($("#simplePartyD").val());
        $.post("/ashx/Contract/loadPeople.ashx?module=buyer", { simpleSeller: simplePartyD }, function (data) {
            $("#simplePartyD").textbox('setValue', data.shortname);
            $("#partyD").combogrid('setValue', data.name);
            $('#partyDCode').attr('value', data.code);

        }, 'json')

    })

}

function bindCombobox() {
    //绑定业务员编号
    $("#salesmanCode").combobox({
        url: '/ashx/Basedata/PurchaserListHandler.ashx?action=GetJobManRole',

        valueField: 'UserRealName',
        textField: 'UserRealName',
        editable: false,
        onSelect: function (record) {
            $("#businessclass").combobox('setValue', record.Agency);
        }

    });
    //合同审核人绑定
    $("#adminReview").combobox({
        url: '/ashx/Contract/loadCombobox.ashx?module=adminReview',

        valueField: 'cname',
        textField: 'cname',
        editable: false,
        onSelect: function (record) {
            $("#adminReviewNumber").val(record.code);
        }

    });
    $('#seller').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        idField: 'name',
        textField: 'name',
        editable: false,
        columns: [[
                    { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simpleSeller").textbox('setValue', rowdata.shortname);
            $("#sellerCode").val(rowdata.code);

        }
    });
    $('#buyer').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=buyer',
        idField: 'name',
        textField: 'name',

        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simpleBuyer").textbox('setValue', rowdata.shortname);
            $("#buyerCode").val(rowdata.code);
        }
    });
    $('#partyC').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=buyer',
        idField: 'name',
        textField: 'name',

        columns: [[
                    { field: 'code', title: '客户编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '客户名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simplePartyC").textbox('setValue', rowdata.shortname);

            $("#partyCCode").val(rowdata.code);
        }
    });
    $('#partyD').combogrid({
        panelWidth: 450,
        url: '/ashx/Contract/loadCombobox.ashx?module=seller',
        idField: 'name',
        textField: 'name',

        columns: [[
                     { field: 'code', title: '供应商编码', width: 60 },
                    { field: 'shortname', title: '简称', width: 80 },
                    { field: 'name', title: '名称', width: 100 },
                    { field: 'address', title: '地址', width: 150 }
        ]],
        onSelect: function (index, rowdata) {
            $("#simplePartyD").textbox('setValue', rowdata.shortname);

            $("#partyDCode").val(rowdata.code);
        }
    });
}
//绑定费用类别表
function bindCostCategory() {
    $('#costCategoryTable').datagrid({
        url: '/ashx/Contract/contractData.ashx?module=costCategoryList&contractNo=' + contractNo,
        rownumbers: true,
        singleSelect: true,
        sortName: 'costCategory',
        sortOrder: 'asc',
        fitColumns: true,
        columns: [[
		            { field: 'costCategory', title: '费用类别', width: 100 },
		            { field: 'project', title: '开票项目', width: 100, align: 'center', editor: { type: 'textbox' } },
                     { field: 'projectDescribe', title: '项目描述', width: 100, align: 'center', editor: { type: 'textbox' } },
                    { field: 'currency', title: '币种', width: 100, editor: { type: 'textbox' } },
                    { field: 'price', title: '单价', width: 100, align: 'center', editor: { type: 'numberbox', options: { precision: 2 } }},
                    { field: 'quantity', title: '数量', width: 100, align: 'center', editor: { type: 'numberbox', options: { precision: 2 }}},
                    { field: 'amount', title: '金额', width: 100, align: 'center', editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'rate', title: '汇率', width: 100, align: 'center', editor: { type: 'numberbox', options: { precision: 2 } } },
                    { field: 'priceUnit', title: '计价单位', width: 100, editor: { type: 'textbox' } },
                    { field: 'remark', title: '备注', width: 100, editor: { type: 'textbox' } },

        ]],
        onClickRow: function (index, row) {
            $('#costCategoryTable').datagrid('beginEdit', index);
            var editors = $('#costCategoryTable').datagrid('getEditors', index);
            var price = editors[3];
            var quantity = editors[4];
            var amount = editors[5];
            //$(amount.target).numberbox('setValue', finamount);
            $(quantity.target).numberbox({
                onChange: function (n, o) {
                    var finamount = parseFloat(quantity.target.val()) * (parseFloat(price.target.val()));//计算后总金额
                    $(amount.target).numberbox('setValue', finamount);
                }
            })

        },
        toolbar: [
  {
      text: '复制',
      iconCls: 'icon-cut',
      handler: function () {
          $("#costCategoryTable").datagrid('acceptChanges');
          var row = $("#costCategoryTable").datagrid('getSelected');
          var rows = $('#costCategoryTable').datagrid('getRows');
          if (row!='undefined') {
              $('#costCategoryTable').datagrid('insertRow', {
                  index: rows.length+1,	// 
                  row: {
                      costCategory: row.costCategory,
                      project: row.project,
                      projectDescribe: row.projectDescribe,
                      currency: row.currency,
                      price: row.price,
                      quantity: row.quantity,
                      amount: row.amount,
                      rate: row.rate,
                      priceUnit: row.priceUnit,
                      remark: row.remark,
                  }

              });

          }
      }
        
  }, '-', {
      iconCls: 'icon-remove',
      text: '删除',
      handler: function () {
          var row = $("#costCategoryTable").datagrid('getSelected');
          var index = $("#costCategoryTable").datagrid('getRowIndex', row);

          if (index != null) {
              $('#costCategoryTable').datagrid('cancelEdit', index)
              .datagrid('deleteRow', index);
              //删除选中的checkbox
              $("input[name='costCategory']").each(function () {
                  var self = $(this);
                  if (self.prop('checked')) {
                      //如果删除的类别和checkbox选中的相同，去除checkbox选中
                      if (self.val()==row.costCategory) {
                          self.prop('checked', false);
                      }
                  }
              });
          } else {
              $.messager.alert('提示', '请选择一行数据！', 'info');
          }
      }
  }],
  //'-', {
  //    iconCls: 'icon-remove',
  //    text: '回滚',
  //    handler: function () {
  //        $("#costCategoryTable").datagrid('rejectChanges');
  //    }
  //}, '-', {
  //    text: '上移', iconCls: 'icon-up', handler: function () {
  //        var row = $("#costCategoryTable").datagrid('getSelected');
  //        var index = $("#costCategoryTable").datagrid('getRowIndex', row);
  //        mysort(index, 'up', 'costCategoryTable');
  //    }
  //}, '-', {
  //    text: '下移', iconCls: 'icon-down', handler: function () {
  //        var row = $("#costCategoryTable").datagrid('getSelected');
  //        var index = $("#costCategoryTable").datagrid('getRowIndex', row);
  //        mysort(index, 'down', 'costCategoryTable');
  //    }
  //}],
        onAfterEdit: function (index, row, changes) {
            row.amount = parseFloat(row.quantity) * (parseFloat(row.price));
            $('#costCategoryTable').datagrid('refreshRow', index);
        },
        onDblClickRow: function (rowIndex, rowData) {
            if (editRow == undefined) {
                $("#costCategoryTable").datagrid('beginEdit', rowIndex);
            }
        },


    });
}
//绑定checkbox点击事件
function checkClick(obj) {
    if (obj.checked) {
        var rows = $('#costCategoryTable').datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].costCategory == obj.value) {
                return;
            }
        }
        $('#costCategoryTable').datagrid('insertRow', {
            index: 0,	// 索引从0开始
            row: {
                costCategory: obj.value,
                price: 0,
                quantity:0,
                project: "",
                currency: "",
                amount: 0,
                remark: "",
            }
        });
    }
    else {
        $('#costCategoryTable').datagrid('acceptChanges');
        var rows = $('#costCategoryTable').datagrid('getRows');
        for (var i = 0; i <= rows.length; i++) {
           
            if (rows[i].costCategory == obj.value) {
                $('#costCategoryTable').datagrid('deleteRow', i);
            }
        }
    }
}

function mysort(index, type, gridname) {
    if ("up" == type) {
        if (index != 0) {
            var toup = $('#' + gridname).datagrid('getData').rows[index];
            var todown = $('#' + gridname).datagrid('getData').rows[index - 1];
            $('#' + gridname).datagrid('getData').rows[index] = todown;
            $('#' + gridname).datagrid('getData').rows[index - 1] = toup;
            $('#' + gridname).datagrid('refreshRow', index);
            $('#' + gridname).datagrid('refreshRow', index - 1);
            $('#' + gridname).datagrid('selectRow', index - 1);
        }
    } else if ("down" == type) {
        var rows = $('#' + gridname).datagrid('getRows').length;
        if (index != rows - 1) {
            var todown = $('#' + gridname).datagrid('getData').rows[index];
            var toup = $('#' + gridname).datagrid('getData').rows[index + 1];
            $('#' + gridname).datagrid('getData').rows[index + 1] = todown;
            $('#' + gridname).datagrid('getData').rows[index] = toup;
            $('#' + gridname).datagrid('refreshRow', index);
            $('#' + gridname).datagrid('refreshRow', index + 1);
            $('#' + gridname).datagrid('selectRow', index + 1);
        }
    }

}

//初始化文件路径
function initFile() {
    $("#upMationDetails").textbox('setText', infoText);
    $("#upMationDetails").textbox('setValue', infoValue);
}
//文件上传
function upload() {
    $("#form1").ajaxSubmit({
        url: "/ashx/Contract/contractOperater.ashx?module=uploadServiceFile",
        type: "post",
        dataType: "text",
        success: function (data) {
            if (data == "error") {
                $.messager.alert("错误：", "上传失败");
            }
            else {
                alert("上传成功");
                //上传成功清除文件
                $("#file1").val("");
                var str = data.split(':');
                $("#upMationDetails").textbox('setText', str[1]);
                $("#upMationDetails").textbox('setValue', str[0]);
                $("#outsideText").val(data);

            }
        }
    })

}
//点击查看文件
function checkAttachUp() {
    $('#upMationDetails').textbox({
        onClickButton: function () {
            var filepath = $("#upMationDetails").textbox('getValue');
            window.top.addNewTab("查看", filepath, '');
        }
    });
}
