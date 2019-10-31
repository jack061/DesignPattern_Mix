$(document).ready(function () {
	var pageTag = $("#pageTag").val();

	if (pageTag == "list") {
		//toolbar1
		var toolbar1 = [{
			text: '添加',
			iconCls: 'icon-add',
			handler: function () {
				window.open('/TrainApply/TrainApplyIForm2.aspx?action=saveList', "_blank", "");
			}
		}, {
			text: '编辑',
			iconCls: 'icon-edit',
			handler: function () {
				var row = $("#tt_list").datagrid('getSelected');
				if (row != null) {
					window.open('/TrainApply/TrainApplyIForm2.aspx?action=editList&applyNo=' + row.APPLYNO, "_blank", "");
				} else {
					$.messager.alert('提示', '请选择一行数据！', 'info');
				}

			}
		}, {
			text: '删除',
			iconCls: 'icon-remove',
			handler: function () {
				var row = $("#tt_list").datagrid('getSelected');
				if (row != undefined) {
					no = row.APPLYNO;
					$.messager.confirm('系统提示', '您确定要删除吗?', function (r) {
						//删除数据
						$.post('/ashx/TrainApply/TrainApplyIListHandler.ashx?action=delSubmitList&applyNo=' + no, function (msg) {
							var result = JSON.parse(msg);
							if ("T" == result.status) {
								msgShow('系统提示', '删除成功', 'info');
								$("#tt_list").datagrid('load');
							} else {
								msgShow('系统提示', '删除失败', 'error');
							}
						});
					});
				} else {
					$.messager.alert('提示', '请选择一行数据！', 'info');
				}
			}
		}];
		//toolbar2
		var toolbar2 = [{
			text: '添加',
			iconCls: 'icon-add',
			handler: function () {
				window.open('/TrainApply/TrainApplyIForm.aspx?action=addSum', "_blank", "");
			}
		}, {
			text: '编辑',
			iconCls: 'icon-edit',
			handler: function () {
				var row = $("#tt_sum").datagrid('getSelected');
				if (row != null) {
					var APPLYNO_ = row.APPLYNO;
					window.open('/TrainApply/TrainApplyIForm.aspx?action=editSum&applyNo=' + APPLYNO_, "_blank", "");
				} else {
					$.messager.alert('提示', '请选择一行数据！', 'info');
				}
			}
		}, {
			text: '删除',
			iconCls: 'icon-remove',
			handler: function () {
				var row = $("#tt_sum").datagrid('getSelected');
				if (row != undefined) {
					$.messager.confirm('系统提示', '删除后不可恢复，确定要删除？', function (i) {
						if (i) {
						    $.post('/ashx/TrainApply/TrainApplyIHandler.ashx?action=delSum&applyNo=' + row.APPLYNO, function (msg) {
						        var result = JSON.parse(msg);
						        if ("T" == result.status){
									msgShow('系统提示', '删除成功', 'info');
									$('#tt_sum').datagrid('reload');
								} else {
									msgShow('系统提示', '删除失败，请稍后重试！', 'info');
								}
							});
						}
					});
				} else {
					$.messager.alert('提示', '请选择一行数据！', 'info');
				}
			}
		}];
		//----初始化datagrid-----
		$('#tt_list').datagrid({
			nowrap: true,
			fitColumns: true,
			striped: true,
			collapsible: true,
			pageList: [10, 15, 30],
			singleSelect: true,
			idField: 'APPLYNO',
			url: '/ashx/TrainApply/TrainApplyIListHandler.ashx?action=getSubmitList',
			columns: [[
				{ field: 'APPLYNO', title: '申请编号', width: 200 },
				{ field: 'STATUS', title: '状态', width: 80 },
				{ field: 'CUSTOMER', title: '申请人', width: 80 },
				{ field: 'COUNTRY', title: '国家', width: 100 },
				{ field: 'PCODE', title: '产品编号', width: 200 },
				{ field: 'PNAME', title: '产品名称', width: 200 },
				{ field: 'HARBOR', title: '国境口岸', width: 150 },
				{ field: 'QUANTITY', title: '数量', width: 100 },
				{ field: 'REMARK', title: '备注', width: 200 }
				]],
			pagination: true,
			toolbar: toolbar1
		});

		$('#tt_sum').datagrid({
			nowrap: true,
			fitColumns: true,
			striped: true,
			collapsible: true,
			pageList: [10, 15, 30],
			singleSelect: true,
			idField: 'APPLYNO',
			url: '/ashx/TrainApply/TrainApplyIListHandler.ashx?action=getSumList',
			columns: [[
			//{ field: 'STATUS', title: '状态', width: 60 },
				{field: 'APPLYNO', title: '请车单号', width: 200 },
				{ field: 'APPLYNO2', title: '请车单号', width: 100 },
				{ field: 'CUSTOMER', title: '申请人', width: 80 },
				{ field: 'COUNTRY', title: '国家', width: 100 },
				{ field: 'PCODE', title: '产品编号', width: 200 },
				{ field: 'PNAME', title: '产品名称', width: 200 },
				{ field: 'HARBOR', title: '国境口岸', width: 150 },
				{ field: 'QUANTITY', title: '数量', width: 100 },
				{ field: 'REMARK', title: '备注', width: 200 },
				{ field: 'CREATEMAN', title: '汇总人', width: 200 },
				{ field: 'CREATEDATE', title: '创建日期', width: 200 }

				]],
			pagination: true,
			toolbar: toolbar2
		});
	}
	if (pageTag == "form") {
		//----初始化datagrid-----
		var applyno = $("#applyNo").val();
		var action = $("#action").val();
		if (action == 'editSum') {
			$('#tt_sub').datagrid({
				pagination: true,
				rownumbers: false,
				sortName: 'APPLYNO',
				sortOrder: 'asc',
				url: '/ashx/TrainApply/TrainApplyIHandler.ashx?action=getListSum&applyNo='+applyno,
				columns: [[
					{ field: 'APPLYNO', title: '状态', width: 80 },
					{ field: 'STATUS', title: '状态', width: 80 },
					{ field: 'CUSTOMER', title: '申请人', width: 80 },
					{ field: 'COUNTRY', title: '国家', width: 100 },
					{ field: 'PCODE', title: '产品编号', width: 200 },
					{ field: 'PNAME', title: '产品名称', width: 200 },
					{ field: 'HARBOR', title: '国境口岸', width: 150 },
					{ field: 'QUANTITY', title: '数量', width: 100 },
					{ field: 'REMARK', title: '备注', width: 200 }
				]]
			}); 
		}

		$('#tt_list').datagrid({
			pagination: true,
			rownumbers: true,
			sortName: 'APPLYNO',
			sortOrder: 'asc',
			url: '/ashx/TrainApply/TrainApplyIListHandler.ashx?action=getSubmitList&filter=true',
			columns: [[
				{ field: 'APPLYNO', title: '请车单号', width: 100 },
				{ field: 'CUSTOMER', title: '申请人', width: 80 },
				{ field: 'COUNTRY', title: '国家', width: 100 },
				{ field: 'PCODE', title: '产品编号', width: 200 },
				{ field: 'PNAME', title: '产品名称', width: 200 },
				{ field: 'HARBOR', title: '国境口岸', width: 150 },
				{ field: 'QUANTITY', title: '数量', width: 100 },
				{ field: 'REMARK', title: '备注', width: 200 }
				]]
		});

		$('#dd').dialog({
			title: '申请选择',
			width: 600,
			height: document.documentElement.clientHeight,
			closed: true,
			cache: false,
			modal: true,
			buttons: [{
				text: '选择',
				handler: function () {
					$('#dd').dialog('close');
					//把选择的产品加载到当前页面
					var rows = $('#tt_list').datagrid('getSelections');

					var oldrows = $('#tt_sub').datagrid('getRows');
					for (var i = 0; i < rows.length; i++) {
						//判断当前表格里面是否有pcode
						var isexists = false;
						for (var j = 0; j < oldrows.length; j++) {
							if (oldrows[j].APPLYNO == rows[i].APPLYNO) {
								isexists = true;
								break;
							}
						}
						if (isexists == false) {
							var row = rows[i];
							var newrow = {};

							newrow.APPLYNO = row.APPLYNO;
							newrow.STATUS = row.STATUS;
							newrow.CUSTOMER = row.CUSTOMER;
							newrow.COUNTRY = row.COUNTRY;
							newrow.PCODE = row.PCODE;
							newrow.PNAME = row.PNAME;
							newrow.HARBOR = row.HARBOR;
							newrow.QUANTITY = row.QUANTITY;
							newrow.REMARK = row.REMARK;

							$('#tt_sub').datagrid('appendRow', newrow);
						}
					}
					$('#tt_list').datagrid('clearSelections');
					if (endEditing()) {
						editIndex = $('#tt_list').datagrid('getRows').length - 1;
						$('#tt_sub').datagrid('selectRow', editIndex).datagrid('beginEdit', editIndex);
					}
				}
			}, {
				text: '取消',
				handler: function () {
					$("#dd").dialog('close');
				}
			}]
		});

		$('#tt_sub').datagrid({
			nowrap: true,
			fitColumns: true,
			striped: true,
			collapsible: true,
			pageList: [10, 15, 30],
			singleSelect: true,
			idField: 'APPLYNO',
			url: '/ashx/TrainApply/TrainApplyIListHandler.ashx?action=GetSubmitList&applyNo=' + applyno,
			columns: [[
						//{ field: 'APPLYNO', title: '申请编号', width: 100 },
						{ field: 'STATUS', title: '状态', width: 80 },
						{ field: 'CUSTOMER', title: '申请人', width: 80 },
						{ field: 'COUNTRY', title: '国家', width: 100 },
						{ field: 'PCODE', title: '产品编号', width: 200 },
						{ field: 'PNAME', title: '产品名称', width: 200 },
						{ field: 'HARBOR', title: '国境口岸', width: 150 },
						{ field: 'QUANTITY', title: '数量', width: 100 },
						{ field: 'REMARK', title: '备注', width: 200 }
					]],

			pagination: true,
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
					var rowindex = $('#tt_sub').datagrid('getRowIndex', $('#tt_sub').datagrid('getSelected'));
					$('#tt_sub').datagrid('deleteRow', rowindex);
				}
			}]
		});
	}
});
function msgShow(title, msgString, msgType) {
	$.messager.alert(title, msgString, msgType);
}

//添加
//function add() {
//	window.open('/TrainApply/TrainApplyIForm.aspx?action=add', "_blank", "");
//}
////修改
//function edit() {
//	var no = '';
//	var row = $("#tt").datagrid('getSelected');
//	if (row != null) {
//		window.open('/TrainApply/TrainApplyIForm.aspx?action=edit&applyNo=' + row.APPLYNO, "_blank", "");      $("#dd").dialog('open');
//	}
//	else {
//		msgShow('系统提示', '请选择要编辑的申请单', 'error');
//	}
//}
////删除
//function del() {
//	var row = $('#tt').datagrid('getSelected');
//	if (row != null) {
//		$.messager.confirm('系统提示', '删除后不可恢复，确定要删除？', function (i) {
//			if (i) {
//				$.post('/ashx/TrainApply/TrainApplyIHandler.ashx?type=del&applyNo=' + row.APPLYNO, function (msg) {
//					if (msg) {
//						msgShow('系统提示', '申请单删除成功', 'info');
//						$('#tt').datagrid('reload');
//					} else {
//						msgShow('系统提示', '删除失败，请稍后重试！', 'info');
//					}
//				});
//			}
//		})

//	} else {
//		msgShow('系统提示', '请选择要删除的申请单1', 'error');
//	}
//}

//提交form表单
function saveApply() {
	getSubTable();
	$("#form1").form('submit', {
		url: "/ashx/TrainApply/TrainApplyIHandler.ashx?action=saveSum",
		onSubmit: function () {
			//进行表单验证 
		    //如果返回false阻止提交 
		    return $(this).form('enableValidation').form('validate');
		},
		success: function (data) {
			var result = JSON.parse(data);
			if ("T" == result.status) {
				msgShow('系统提示', '操作成功', 'info');
				$("#tt").datagrid('load');
			} else {
				msgShow('系统提示', '操作失败', 'error');
			}
		}
	});

}
//获取子表数据
function getSubTable() {
	var datagrid = $("#tt_sub").datagrid("getRows");
	var datagridjson = JSON.stringify(datagrid);
	$("#datagrid").val(datagridjson);
}