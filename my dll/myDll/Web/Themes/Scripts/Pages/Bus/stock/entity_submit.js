var submitbill = function (oldstatus, newstatus, docno, doctype) {
    if (statusChanged(oldstatus, doctype) != newstatus) {
        console.log('原状态：' + oldstatus + '  变更后状态：' + newstatus);
        if (statusChanged(oldstatus, doctype).indexOf("/") > 0) {
            if (statusChanged(oldstatus, doctype).indexOf(newstatus) <= 0) {
                $.messager.alert('提示', '请选择<提交/退回>状态的数据！', 'info');
                return false;
            }
        } else {
            if (newstatus == "提交") {
                $.messager.alert('提示', '请选择<新建>状态的数据！', 'info');
//                alert("请选择<新建>状态的数据！");
            }
            if (newstatus == "收货") {
                $.messager.alert('提示', '请选择<提交/退回>状态的数据！', 'info');
            }
            if (newstatus == "退回") {
                $.messager.alert('提示', '请选择<收货>状态的数据！', 'info');
            }
            if (newstatus == "入库") {
                $.messager.alert('提示', '请选择<收货>状态的数据！', 'info');
            }
            return false;
        }

    }
    $.messager.confirm('提示', '确认要' + newstatus + '单据么?</br></br>',
        function (r) {
            if (r) {
                var pam = {};
                pam.docno = docno;
                pam.doctype = doctype;
                pam.status = newstatus;
                $.post('/Bus/StockManage/StockEntityHandler.ashx?action=submitbill', pam, function (msg) {
                    if (msg.legth > 0) {
                        alert(msg);
                    }
                    if (SearchData != undefined) {
                        SearchData();
                    }
                });
            }
        });
}

var submitbill2 = function (oldstatus, newstatus, docno, doctype, callback) {
        if (statusChanged(oldstatus, doctype) != newstatus) {
            console.log('原状态：' + oldstatus + '  变更后状态：' + newstatus);
            return false;
        }
        $.messager.confirm('提示', '确认要' + newstatus + '单据么?</br></br>',
        function (r) {
            if (r) {
                var pam = {};
                pam.docno = docno;
                pam.doctype = doctype;
                pam.status = newstatus;
                $.post('/Bus/StockManage/StockEntityHandler.ashx?action=submitbill', pam, callback);
            }
        });
    }

var statusChanged = function (status, doctype) {
        if ("入库" == doctype) {
            if (status == "新建") {
                return "提交";
            }
            if (status == "提交") {
                return "收货";
            }
            if (status == "收货") {
                return "入库/退回";
            }
            return "";
        }
        if ("出库" == doctype) {
            if (status == "新建") {
                return "提交";
            }
            if (status == "提交") {
                return "出库";
            }
            return "";
        }
    
}