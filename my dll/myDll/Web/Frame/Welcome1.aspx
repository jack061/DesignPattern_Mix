<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome1.aspx.cs" Inherits="RM.Web.Frame.Welcome1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../jquery-easyui-1.4.5/themes/default/easyui.css" rel="stylesheet" type="text/css" />
    <link href="../jquery-easyui-1.4.5/themes/icon.css" rel="stylesheet" type="text/css" />
    <script src="../jquery-easyui-1.4.5/jquery.min.js" type="text/javascript"></script>
    <script src="../jquery-easyui-1.4.5/jquery.easyui.min.js" type="text/javascript"></script>
    <script src="../jquery-easyui-1.4.5/locale/easyui-lang-zh_CN.js" type="text/javascript"></script>
    <link href="../Themes/Scripts/bootstrap/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../Themes/Styles/framework-font.css" rel="stylesheet" type="text/css" />
    <script src="../Themes/Scripts/DateFormat.js" type="text/javascript"></script>
    <script type="text/javascript">
        var loadMain = function () {
            self.location = "#";
        }
        $(function () {
            $('#exportTable').datagrid({
             
                sortName: 'contractNo',
                sortOrder: 'asc',
                nowrap: true,
                fitColumns: true,
                striped: true,
                collapsible: true,
                pageList: [10, 15, 30],
                singleSelect: true,
                url: '/ashx/Contract/reviewContractData.ashx?module=reviewExportContract&flowdirection=出境&isDesk=true',
                columns: [[ 
                { field: 'status1', title: '审核状态', width: '100px' },
                //{ field: 'businessclass', title: '销售组', width: '60px' },
                {field: 'createman', title: '创建人', width: '60px' },
                { field: 'contractNo', title: '合同编号', width: '120px' },
                  { field: 'buyer', title: '买方', width: '120px' },
                { field: 'seller', title: '卖方', width: '120px' },
                ]],
               pagination: true
            });
            $('#importTable').datagrid({
          
                sortName: 'contractNo',
                sortOrder: 'asc',
                nowrap: true,
                fitColumns: true,
                striped: true,
                collapsible: true,
                pageList: [10, 15, 30],
                singleSelect: true,
                url: '/ashx/Contract/reviewContractData.ashx?module=reviewExportContract&flowdirection=进境',
                columns: [[
                { field: 'status', title: '审核状态', width: '100px' },
                //{ field: 'businessclass', title: '销售组', width: '60px' },
                {field: 'createman', title: '创建人', width: '60px' },
                { field: 'contractNo', title: '合同编号', width: '120px' },
                  { field: 'buyer', title: '买方', width: '120px' },
                { field: 'seller', title: '卖方', width: '120px' },
                ]],
                pagination: true
            });

            $('#logisticsTable').datagrid({
               
                sortName: 'logisticsContractNo',
                sortOrder: 'asc',
                nowrap: true,
                fitColumns: true,
                striped: true,
                collapsible: true,
                pageList: [10, 15, 30],
                singleSelect: true,
                url: '/ashx/Contract/reviewContractData.ashx?module=reviewLogisticsContract',
                columns: [[
                { field: 'status', title: '审核状态', width: '100px' },
                //{ field: 'businessclass', title: '销售组', width: '60px' },
                {field: 'createman', title: '创建人', width: '60px' },
                { field: 'logisticsContractNo', title: '合同编号', width: '120px' },
                { field: 'buyer', title: '买方', width: '120px' },
                { field: 'seller', title: '卖方', width: '120px' },

                ]],
                pagination: true
            });

            //----初始化datagrid-----
            $('#tbMoney').datagrid({
                nowrap: true,
                fitColumns: true,
                striped: true,
                collapsible: true,
                pageList: [10, 15, 30],
                singleSelect: true,
                idField: 'PAYNO',
                url: '/ashx/ContractPayment/paymentLoadData.ashx?action=GetCashPayment',
                columns: [[
            { field: 'ck', checkbox: true },
//            { field: 'S2STATUS', title: '状态', width: '80px', center: true, formatter: function (val) {
//                if (val == 0) return '保存';
//                if (val == 1) return '提交';
//                if (val == 2) return '作废';
//                else { return '未知'; }
//            }
//            },
            { field: 'PARTINFO', title: '对应合同', width: '60px', align: 'center' },
            { field: 'BUSINESSTYPE', title: '业务类型', width: '60px' },
            { field: 'ACCOUNTSIMPLYNAME', title: '收款户名', width: '120px' },
            { field: 'BANKNAME', title: '开户银行', width: '150px' },
            { field: 'PAYACCOUNT', title: '付款账户', width: '130px' },
            { field: 'PAYDATE', title: '汇入时间', width: '80px', formatter: formatDatebox },
            { field: 'PAYAMOUNT', title: '汇入金额', width: '80px',
                formatter: function (val, rowData, rowIndex) {
                    if (val != null)
                        return Number(val);
                }
            },
            { field: 'CURRENCY', title: '币种', width: '40px' },
            ]],
              pagination: true
            });
        })
    </script>
</head>
<body onload="loadMain()">
    <form id="form1" runat="server">
    <%--<div class="easyui-calendar" style="width:250px;height:250px;"></div>--%>
    <div class="rows" style="overflow: hidden; margin-top: 5px;">
        <div style="float: left; width: 48%; margin-right: 1%; margin-left: 1%;">
            <div style="height: 300px; /*!border: 1px solid #e6e6e6; */ background-color: #fff;">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-thumbs-o-up fa-lg" style="padding-right: 5px;"></i>待审批出口合同</div>
                    <div class="panel-body">
                        <table id="exportTable">
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div style="float: left; width: 48%; margin-right: 1%;">
            <div style="height: 300px; /*!border: 1px solid #e6e6e6; */ background-color: #fff;">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-thumbs-o-up fa-lg" style="padding-right: 5px;"></i>待审批进口合同</div>
                    <div class="panel-body">
                        <table id="importTable">
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="rows" style="overflow: hidden; margin-top: 5px;">
        <div style="float: left; width: 48%; margin-right: 1%; margin-left: 1%;">
            <div style="height: 300px; /*!border: 1px solid #e6e6e6; */ background-color: #fff;">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-thumbs-o-up fa-lg" style="padding-right: 5px;"></i>待审批物流合同</div>
                    <div class="panel-body">
                        <table id="logisticsTable">
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div style="float: left; width: 48%; margin-right: 1%;">
            <div style="height: 300px; /*!border: 1px solid #e6e6e6; */ background-color: #fff;">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <i class="fa fa-rss fa-lg" style="padding-right: 5px;"></i>待匹配合同现汇收款</div>
                    <div class="panel-body">
                        <table id="tbMoney">
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <style>
        #copyrightcontent
        {
            height: 30px;
            line-height: 29px;
            overflow: hidden;
            position: absolute;
            top: 100%;
            margin-top: -30px;
            width: 100%;
            background-color: #fff;
            border: 1px solid #e6e6e6;
            padding-left: 10px;
            padding-right: 10px;
        }
        
        .dashboard-stats
        {
            float: left;
            width: 20%;
        }
        
        .dashboard-stats-item
        {
            position: relative;
            overflow: hidden;
            color: #fff;
            cursor: pointer;
            height: 105px;
            margin-right: 25px;
            margin-bottom: 10px;
            padding: 20px 20px;
        }
        
        .dashboard-stats-item .m-top-none
        {
            margin-top: 2px;
        }
        
        .dashboard-stats-item h2
        {
            font-size: 35px;
            font-family: inherit;
            line-height: 1.1;
            font-weight: 500;
        }
        
        .dashboard-stats-item h5
        {
            font-size: 14px;
            font-family: inherit;
            margin-top: 3px;
            line-height: 1.1;
        }
        
        
        .dashboard-stats-item .stat-icon
        {
            position: absolute;
            top: 10px;
            right: 10px;
            font-size: 30px;
            opacity: .3;
        }
        
        .dashboard-stats i.fa.stats-icon
        {
            width: 50px;
            padding: 20px;
            font-size: 50px;
            text-align: center;
            color: #fff;
            height: 50px;
            border-radius: 10px;
        }
        
        .panel-default
        {
            border: none;
            border-radius: 0px;
            margin-bottom: 0px;
            box-shadow: none;
            -webkit-box-shadow: none;
        }
        
        .panel-default > .panel-heading
        {
            color: #777; /*background-color: #fff;*/
            border-color: #e6e6e6;
            padding: 10px 10px;
        }
        
        <%--.panel-default > .panel-body
        {
            padding: 10px;
            padding-bottom: 0px;
        }
        
        .panel-default > .panel-body ul
        {
            overflow: hidden;
            padding: 0;
            margin: 0px;
            margin-top: -5px;
        }
        
        .panel-default > .panel-body ul li
        {
            line-height: 27px;
            list-style-type: none;
            white-space: nowrap;
            text-overflow: ellipsis;
        }
        
        .panel-default > .panel-body ul li .time
        {
            color: #a1a1a1;
            float: right;
            padding-right: 5px;
        }--%>
    </style>
</body>
</html>
