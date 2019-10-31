var submitbill = function (oldstatus, newstatus, docno, doctype) {
    if (statusChanged(oldstatus) != newstatus) {
        console.log('原状态：' + oldstatus + '  变更后状态：' + newstatus);
        return false;
    }
    $.messager.confirm('系统提示', '确认要' + newstatus + '单据么?</br></br>',
        function (r) {
            if (r) {
                var pam = {};
                pam.docno = docno;
                pam.doctype = doctype;
                pam.status = newstatus;
                $.post('/Bus/StockManage/StockHandler.ashx?action=submitbill', pam, function (msg) {
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
        if (statusChanged(oldstatus) != newstatus) {
            console.log('原状态：' + oldstatus + '  变更后状态：' + newstatus);
            return false;
        }
        $.messager.confirm('系统提示', '确认要' + newstatus + '单据么?</br></br>',
        function (r) {
            if (r) {
                var pam = {};
                pam.docno = docno;
                pam.doctype = doctype;
                pam.status = newstatus;
                $.post('/Bus/StockManage/StockHandler.ashx?action=submitbill', pam, callback);
            }
        });
    }




var statusChanged = function (status) {
    if (status == "新建") {
        return "提交";
    }
    if (status == "提交") {
        return "审核";
    }
    if (status == "审核") {
        return "确认";
    }
    return "";
}