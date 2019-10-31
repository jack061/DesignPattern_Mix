$(function () {
    //绑定数据
    bindUI();
    $('#purchasesellDiv').panel('close');
    $('#othersDiv').panel('close');
    $('#chooseDiv').panel('close');
    $(":radio").click(function () {
        if ($("input[name='createType']:checked").val() == "购销") {
            //隐藏筛选条件Div
            $('#chooseDiv').panel('close');
            //显示购销选择
            $('#purchasesellDiv').panel('open');
            //隐藏其他选择
            $('#othersDiv').panel('close');
            //隐藏筛选条件Div
            $('#chooseDiv').panel('close');

        }
  
        if ($("input[name='purchasesell']:checked").val() == "进境" || $("input[name='purchasesell']:checked").val() == "出境"
            || $("input[name='purchasesell']:checked").val() == "境内") {
            //显示筛选条件Div
            $('#chooseDiv').panel('open');
            
        }
        if ($("input[name='createType']:checked").val() == "其他") {
            //显示其他选择
            $('#othersDiv').panel('open');
            //隐藏购销选择
            $('#purchasesellDiv').panel('close');
            //隐藏筛选条件Div
            $('#chooseDiv').panel('close');
        }

    })
})
//绑定数据
function bindUI() {
    $('#productCategory').combobox({
        required: true,
        valueField: 'productcategory',
        textField: 'productcategory',
        editable: false,
        data: sbporduct
    });
    $('#templateCategory').combobox({
        required: true,
        valueField: 'code',
        textField: 'code',
        editable: false,
        data: sbtemplate
    });
}
//下一步点击事件
function nextway() {
    //获取业务流向，筛选条件，模板名称
  
    var flowdirection = $("input[name='purchasesell']:checked").val();
    var productCategory = $('#productCategory').combobox('getValue');
    var templateCategory = $('#templateCategory').combobox('getValue');
    var language = $('#language').combobox('getValue');
    var templatename = $('#templateName').textbox('getValue');
    window.top.addNewTab("合同模板-新增", '/Bus/ContractTemplate/templateForContract.aspx?flowdirection=' + flowdirection + '&productCategory=' + productCategory
        + '&templateCategory=' + templateCategory + '&templatename=' + templatename + '&language=' + language, '');

   


}
