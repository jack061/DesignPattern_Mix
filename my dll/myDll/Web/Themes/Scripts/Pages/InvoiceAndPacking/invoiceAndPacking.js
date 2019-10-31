$(document).ready(function () {
    var type = $('#type').val();
    var no = $('#no').val();
    var languages = $('#language').val();
    if (languages.indexOf("中文") < 0) {
        $(".CHN").remove();
    }
    if (languages.indexOf("英文") < 0) {
        $(".ENG").remove();
    }
    if (languages.indexOf("俄文") < 0) {
        $(".RUS").remove();
    }


    if ('invoice' == type) {
        $("#packing").remove();
        //----初始化datagrid-----
        $('#tt').datagrid({
            sortName: 'contractNo',
            sortOrder: 'asc',
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            singleSelect: true,
            url: '/ashx/InvoiceAndPacking/InvoiceAndPackingHandler.ashx?action=getSubInvoice&no=' + no,
            columns: [[
            { field: 'MARK', title: '唛头<br />', width: '15%' },
            { field: 'PCODE', title: 'HS编码', width: '15%' },
            { field: 'PNAME', title: '货物名称', width: '20%' },
            { field: 'QUNIT', title: '单位', width: '12%' },
            { field: 'QUANTITY', title: '数量', width: '12%' },
            { field: 'PRICE', title: '单价', width: '12%' },
            { field: 'AMOUNT', title: '金额', width: '12%' }
            ]]
        });
    }
    if ('packing' == type) {
        $("#invoice").remove();
        //----初始化datagrid-----
        $('#tt').datagrid({
            sortName: 'contractNo',
            sortOrder: 'asc',
            nowrap: true,
            fitColumns: true,
            striped: true,
            collapsible: true,
            singleSelect: true,
            url: '/ashx/InvoiceAndPacking/InvoiceAndPackingHandler.ashx?action=getSubPacking&no=' + no,
            columns: [[
            { field: 'MARK', title: '唛头<br />', width: '11%' },
            { field: 'PCODE', title: 'HS编码', width: '11%' },
            { field: 'PNAME', title: '品名', width: '11%' },
            { field: 'XXXX', title: '规格型号', width: '11%' },
            { field: 'QUNIT', title: '单位', width: '11%' },
            { field: 'QUANTITY', title: '数量', width: '11%' },
            { field: 'XXXXX', title: '件数', width: '11%' },
            { field: 'XXXXX', title: '毛重(KG)', width: '11%' },
            { field: 'XXXXX', title: '净重（KG）', width: '11%' }
            ]]
        });
    }
});