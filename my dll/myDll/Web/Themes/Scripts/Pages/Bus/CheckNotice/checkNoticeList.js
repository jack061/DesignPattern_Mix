//发货通知
function sendNotice() {
    var row = $("#maingrid").datagrid('getSelected');
    if (row == undefined || row.applystatus == "1") {
        $.messager.alert('提示', '请选择数据！', 'info');
    }
    else {
        if ('已通知' == row.sendOutNoticeStatus) {
            $.messager.alert('提示', '该订舱申请已经发送过发货通知！', 'info');
            return false;
          }
          $('#dd').dialog({
              title: '发送发货通知',
              width: 800,
              height: document.documentElement.clientHeight - 80,
              closed: true,
              cache: false,
              //href: 'Abroad_Product_Select.aspx',
              modal: true,
              buttons: [{
                  text: '确定',
                  handler: function () {
                      var par = {};
                      par.wcode = $("#wcode").combobox('getValue')
                      par.wname = $("#wname").val();
                      par.owner = $("#sendFactory").combobox('getValue')
                      par.ownercode = $("#ownercode").val();
                      par.wkeepercode = $("#wkeepercode").val();
                      par.wkeeper = $("#wkeeper").val();
                      par.isDT = $("input[name='isDT']").val();
                      par.isTM = $("input[name='isTM']").val();
                      par.isCM = $("input[name='isCM']").val();
                      par.isTZMD = $("input[name='isTZMD']").val();
                      par.costbear = $("#costbear").val();
                      par.note = $("#note").val();
                      if (par.wcode != "" && par.wname != "") {
                          if (row.paystatus == '未收款') {
                              $.messager.confirm("确认", "合同：" + row.saleContract + "未到款，您确认提交发货吗？", function (r) {
                                  $("#wh_list").datagrid("acceptChanges");
                                  var sendDetail = $("#wh_list").datagrid("getRows");
                                  if (sendDetail.length == 0) {
                                      $.messager.alert("提醒", "没有库存信息不能提交！");
                                  } else {
                                      var sendDetailJson = JSON.stringify(sendDetail);
                                      par.sendDetail = sendDetailJson;
                                      if (r) {
                                          $.ajax({
                                              cache: true,
                                              type: "POST",
                                              url: '/ashx/CheckNotice/CheckData.ashx?module=addStockOut&checkNoticeNumber=' + row.checkNoticeNumber + '&contractNo=' + row.saleContract + '&createDateTag=' + row.contractCreateDateTag,
                                              data: par,
                                              async: false,
                                              error: function (data) {
                                                  $.messager.alert("提醒", data);
                                              },
                                              success: function (data) {

                                                  var result = JSON.parse(data);
                                                  if ("T" == result.status) {
                                                      $.messager.alert("提醒", "保存成功");
                                                      $('#dd').dialog('close');
                                                      $('#maingrid').datagrid('reload');
                                                  } else {
                                                      $.messager.alert("提醒", result.msg);
                                                  }
                                              }
                                          });
                                      }
                                   }
                              });
                          }
                      }
                  }
              }, {
                  text: '取消',
                  handler: function () {
                      $("#dd").dialog('close');
                  }
              }]
          });
          //发货工厂
          $('#sendFactory').combogrid({
              panelWidth: 200,
              url: '/ashx/Basedata/DictronaryHandler.ashx?action=getSendFactoryList',
              idField: 'text',
              textField: 'text',
              columns: [[
                    { field: 'id', title: '发货工厂编码', width: 100 },
                    { field: 'text', title: '发货工厂', width: 180 }
              ]],
              onSelect: function (index, rowdata) {
                  $("#sendFactory").combogrid('setValue', rowdata.text);
                  $("#ownercode").val(rowdata.id);
              }
          });

        //仓库
          $('#wcode').combogrid({
              panelWidth: 360,
              url: '/ashx/Basedata/DictronaryHandler.ashx?action=getWarehouseList1',
              idField: 'Person',
              textField: 'Agency',
              columns: [[
                    { field: 'PERSON', title: '仓库编码', width: 80 },
                    { field: 'AGENCY', title: '仓库名称', width: 150 },
                    { field: 'LOGINNAME', title: '库管员编码', width: 80 },
                    { field: 'USERREALNAME', title: '库管员', width: 100 }
        ]],
              onSelect: function (index, rowdata) {
                  $("#wcode").combogrid('setValue', rowdata.PERSON);
                  $("#wname").textbox('setValue', rowdata.AGENCY);
                  $("#wkeepercode").val(rowdata.LOGINNAME);
                  $("#wkeeper").textbox('setValue', rowdata.USERREALNAME);
                  var sendFactory = $("#sendFactory").combogrid('getValue');
                  if (sendFactory == "") {
                      $.messager.alert('提示', '请选择发货工厂！', 'info');
                  } else {
                      var rows = $('#checkNoticeDlist').datagrid('getRows'); //获取所有当前加载的数据行
                      var row_s = rows[0];
                      /*
                      if (row_s.sendFactory == '') {
                      //重新加载子表+仓库剩余信息（根据发货人）
                      $('#wh_list').datagrid({ url: '/ashx/CheckNotice/CheckData.ashx?module=getCheckNoticeDList_WH&contractNo=' + row_s.saleContract + "&createDateTag=" + row_s.contractCreateDateTag + "&owner_wh=" + row_s.sendMan + "&wcode=" + rowdata.PERSON,
                      queryParams: {}
                      });
                      } else {
                      //重新加载子表+仓库剩余信息（根据发货工厂）
                      $('#wh_list').datagrid({ url: '/ashx/CheckNotice/CheckData.ashx?module=getCheckNoticeDList_WH&contractNo=' + row_s.saleContract + "&createDateTag=" + row_s.contractCreateDateTag + "&owner_wh=" + row_s.sendFactory + "&wcode=" + rowdata.PERSON,
                      queryParams: {}
                      });  
                      }
                      */
                      $('#wh_list').datagrid({ url: '/ashx/CheckNotice/CheckData.ashx?module=getCheckNoticeDList_WH&contractNo=' + row_s.saleContract + "&createDateTag=" + row_s.contractCreateDateTag + "&owner_wh=" + sendFactory + "&wcode=" + rowdata.PERSON,
                          queryParams: {}
                      });
                   }
                  

              }
          });

        $('#checkNoticeDlist').datagrid({
            url: '/ashx/CheckNotice/CheckData.ashx?module=getCheckNoticeDList&contractNo=' + row.saleContract + "&createDateTag=" + row.contractCreateDateTag,
            rownumbers: true,
            singleSelect: true,
            sortName: 'saleContract',
            sortOrder: 'asc',
            columns: [[
		            { field: 'contractCreateDateTag', title: '申请单号', width: 100 },
		            { field: 'saleContract', title: '合同编号', width: 100, align: 'center' },                    
                    { field: 'seller', title: '卖方', width: 100, align: 'center' },
                    { field: 'buyer', title: '买方', width: 100, align: 'center' },
                    { field: 'relateContractNo', title: '关联合同编号', width: 100, align: 'center' },
                    { field: 'relateSeller', title: '关联卖方', width: 100, align: 'center' },
                    { field: 'relateBuyer', title: '关联买方', width: 100, align: 'center' },
                    { field: 'sendMan', title: '发货人', width: 100, align: 'center' },
                    { field: 'sendFactoryCode', title: '发货工厂编码', width: 100, align: 'center' },
                    { field: 'sendFactory', title: '发货工厂', width: 100, align: 'center',
                     formatter: function (value, row, index) {
                         if (value != '') {
                             $('#sendFactory').combogrid('setValue', value);
                             $('#ownercode').val(row.sendFactoryCode);
                             return value; 
                        }
                        else { return value; }
                    } 
                    },
                    { field: 'pcode', title: '产品编号', width: 80, align: 'center' },
                    { field: 'pname', title: '产品名称', width: 100, align: 'center' },
                    { field: 'spec', title: '规格', width: 80, align: 'center' },
                    { field: 'quantity', title: '申请数量', width: 80, align: 'center' },
                    { field: 'sendQuantity', title: '发货数量', width: 80, align: 'center' },
                    { field: 'shipcompanyname', title: '船舶公司', width: 100, align: 'center' },
                    { field: 'shipdate', title: '船期', width: 100, align: 'center' }

        ]],
            pagination: true

        });

        $('#wh_list').datagrid({
            // url: '/ashx/CheckNotice/CheckData.ashx?module=getCheckNoticeDList&contractNo=' + row.saleContract + "&createDateTag=" + row.contractCreateDateTag,
            rownumbers: true,
            singleSelect: true,
            pagination: true,
            sortName: 'createDateTag',
            sortOrder: 'asc',
            columns: [[
                    { field: 'mcode', title: '产品编号', width: 80, align: 'center' },
                    { field: 'mname', title: '产品名称', width: 100, align: 'center' },
                    { field: 'mspec', title: '规格', width: 80, align: 'center' },
                    //{ field: 'sendQuantity', title: '申请数量', width: 80, align: 'center' },
                    { field: 'stockquantity', title: '库存数量', width: 80, align: 'center' },
                    { field: 'inquantity', title: '在途数量', width: 80, align: 'center' },                    
                    { field: 'outquantity', title: '预占数量', width: 80, align: 'center' },
                    { field: 'usequantity', title: '可用数量', width: 80, align: 'center' },
                    { field: 'realnumber', title: '通知件数', width: 100, editor: { type: 'numberbox'} },
                    { field: 'realquantity', title: '通知数量', width: 100, editor: { type: 'numberbox', options: { precision: 3 } } },
                    { field: 'indocno', title: '批次', width: 100, align: 'center' },
                    { field: 'createdate', title: '批次入库时间', width: 100, align: 'center' }

        ]],
            onClickCell: function (index, row, changes) {
                var row = $("#wh_list").datagrid('getSelected');
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
            pagination: false,
            onLoadSuccess: function () {
                $('#wh_list').datagrid('selectAll');
                var rows = $('#wh_list').datagrid('getRows');
                for (var i = 0; i < rows.length; i++) {
                    $('#wh_list').datagrid('beginEdit', i);
                }
            }

        });

        $('#dd').window('open');
    }

}

function checkView() {
    var checkId = '';

    var row = $("#maingrid").datagrid('getSelected');

    if (row != undefined) {
        checkId = row.checkId;

        window.top.addNewTab("海运订舱-查看", "/Bus/CheckOutNotice/checkNoticeView.aspx?action=add&checkNoticeNumber=" + row.checkNoticeNumber + "&contractNo=" + row.saleContract + "&isBrowse=true");
    } else {
        $.messager.alert('提示', '请选择一行数据！', 'info');
    }
 }