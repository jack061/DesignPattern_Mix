<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Welcome.aspx.cs" Inherits="RM.Web.Frame.Welcome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

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
    <script type="text/javascript">
        var loadMain = function () {
            self.location = "#";
        }
        $(function () {
            initExportContract();
            initImportContract();
            initBackContract();
            initManage();
            initCashContract();
            initServiceContract();
            initInternalContract();
        });

        //出口合同
        function initExportContract() {
            $.post('/ashx/Contract/reviewContractData.ashx?module=reviewExportContract&flowdirection=出境&isDesk=true&rows=10&page=1&order=contractNo&sort=desc', function (msg) {
                var result = JSON.parse(msg);
                var rows = result.rows;
                var html = '';
                for (var i = 0; i < rows.length; i++) {
                    html = html + '<li><a href="#">业务员：<strong>' + rows[i].createmanname + '</strong>提交的合同：<strong>' + rows[i].contractNo + '</strong>待审核</a><span class="time">[出口]</span></li>';
                }
                if(html.length>0)
                    html ='<ul>'+ html + '</ul>';
                // $("#exportContract").html(html);
                $("#exportContract").append(html);
            });
        }
        //进口合同
        function initImportContract() {
            $.post('/ashx/Contract/reviewContractData.ashx?module=reviewExportContract&flowdirection=进境&isDesk=true&rows=10&page=1&order=contractNo&sort=desc', function (msg) {
                var result = JSON.parse(msg);
                var rows = result.rows;
                var html = '';
                for (var i = 0; i < rows.length; i++) {
                    html = html + '<li><a href="#">业务员：<strong>' + rows[i].createmanname + '</strong>提交的合同：<strong>' + rows[i].contractNo + '</strong>待审核</a><span class="time">[进口]</span></li>';
                }
                if(html.length>0)
                    html = '<ul>'+html + '</ul>';
                $("#exportContract").append(html);
            });
        }
        //待审批管理合同
        function initManage() {
            $.post('/ashx/Contract/reviewContractData.ashx?module=reviewManageContract&isDesk=true&rows=10&page=1&order=logisticsContractNo&sort=desc', function (msg) {
                var result = JSON.parse(msg);
                var rows = result.rows;
                var html = '';
                for (var i = 0; i < rows.length; i++) {
                    html = html + '<li><a href="#">业务员：<strong>' + rows[i].createmanname + '</strong>提交的<strong>' + rows[i].contractNo + '</strong>待审核</a><span class="time">[管理]</span></li>';
                }
                if (html.length > 0)
                    html = '<ul>' + html + '</ul>';
                $("#exportContract").append(html);
            });

        }
        //待审批物流合同
        function initServiceContract() {
            $.post('/ashx/Contract/reviewContractData.ashx?module=reviewServiceContract&isDesk=true&rows=10&page=1&order=contractNo&sort=desc', function (msg) {
                var result = JSON.parse(msg);
                var rows = result.rows;
                var html = '';
                for (var i = 0; i < rows.length; i++) {
                    html = html + '<li><a href="#">物流操作员：<strong>' + rows[i].createmanname + '</strong>提交的<strong>' + rows[i].contractNo + '</strong>待审核</a><span class="time">[物流]</span></li>';
                }
                if(html.length>0)
                    html = '<ul>'+html + '</ul>';
                $("#exportContract").append(html);
            });

        }
        //内部结算单
        function initInternalContract() {
            $.post('/ashx/Contract/reviewContractData.ashx?module=reviewInternalContract&isDesk=true&rows=10&page=1&order=contractNo&sort=desc', function (msg) {
                var result = JSON.parse(msg);
                var rows = result.rows;
                var html = '';
                for (var i = 0; i < rows.length; i++) {
                    html = html + '<li><a href="#">业务员：<strong>' + rows[i].createmanname + '</strong>提交的合同：<strong>' + rows[i].contractNo + '</strong>待审核</a><span class="time">[内部结算单]</span></li>';
                }
                if (html.length > 0)
                    html = '<ul>' + html + '</ul>';
                // $("#exportContract").html(html);
                $("#exportContract").append(html);
            });
        }
        //退回合同
        function initBackContract() {
            $.post('/ashx/Contract/reviewContractData.ashx?module=getBackContract&isDesk=true&rows=10&page=1&order=contractNo&sort=desc', function (msg) {
                var result = JSON.parse(msg);
                var rows = result.rows;
                var html = '';
                for (var i = 0; i < rows.length; i++) {
                    html = html + '<li><a href="#">业务员：<strong>' + rows[i].createmanname + '</strong>提交的合同：<strong>' + rows[i].contractNo + '</strong>退回</a><span class="time"></span></li>';
                }
                if(html.length>0)
                    html = '<ul>'+html + '</ul>';
                // $("#exportContract").html(html);
                $("#exportContract").append(html);
            });
        }
        //待匹配合同回款
        function initCashContract() {
            $.post('/ashx/ContractPayment/paymentLoadData.ashx?action=GetCashPayment', function (msg) {
                var result = JSON.parse(msg);
                var rows = result.rows;
                var html = '<ul>';
                for (var i = 0; i < rows.length; i++) {
                    html = html + '<li><a href="#"><strong>' + rows[i].ACCOUNTSIMPLYNAME + '</strong>收到<strong> ' + rows[i].PAYACCOUNT + '</strong> ￥' + rows[i].PAYAMOUNT + ' ' + rows[i].CURRENCY + '</a><span class="time">[待认领]</span></li>';
                }
                html = html + '</ul>';
                $("#importContract").append(html);
            });
        }
    </script>
</head>
<body onload="loadMain()">
    <form id="form1" runat="server">
    <%--<div class="easyui-calendar" style="width:250px;height:250px;"></div>--%>
    <div class="rows" style="overflow: hidden;margin-top: 5px;">
            <div style="float: left; width: 33%; margin-right: 1%;margin-left: 1%;">
                <div style="height: 500px; border: 1px solid #e6e6e6; background-color: #fff;">
                    <div class="panel panel-default">
                        <div class="panel-heading"><i class="fa fa-thumbs-o-up fa-lg" style="padding-right: 5px;"></i>待审批合同</div>
                        <div id="exportContract" class="panel-body">
                            
                        </div>
                    </div>
                </div>
            </div>
            <div style="float: left; width: 33%; margin-right: 1%;">
                <div style="height: 500px; border: 1px solid #e6e6e6; background-color: #fff;">
                    <div class="panel panel-default">
                        <div class="panel-heading"><i class="fa fa-rss fa-lg" style="padding-right: 5px;"></i>待认领收款</div>
                        <div id="importContract" class="panel-body">
                            
                        </div>
                    </div>
                </div>
            </div>
            <div style="float: left; width: 30%;">
                <div style="height: 500px; border: 1px solid #e6e6e6; background-color: #fff;">
                    <div class="panel panel-default">
                        <div class="panel-heading"><i class="fa fa-send fa-lg" style="padding-right: 5px;"></i>通知公告</div>
                        <div id="logistics" class="panel-body"> 
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <style type="text/css">
        #copyrightcontent {
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

        .dashboard-stats {
            float: left;
            width: 20%;
        }

        .dashboard-stats-item {
            position: relative;
            overflow: hidden;
            color: #fff;
            cursor: pointer;
            height: 105px;
            margin-right: 25px;
            margin-bottom: 10px;
            padding: 20px 20px;
        }

            .dashboard-stats-item .m-top-none {
                margin-top: 2px;
            }

            .dashboard-stats-item h2 {
                font-size: 35px;
                font-family: inherit;
                line-height: 1.1;
                font-weight: 500;
            }

            .dashboard-stats-item h5 {
                font-size: 14px;
                font-family: inherit;
                margin-top: 3px;
                line-height: 1.1;
            }


            .dashboard-stats-item .stat-icon {
                position: absolute;
                top: 10px;
                right: 10px;
                font-size: 30px;
                opacity: .3;
            }

        .dashboard-stats i.fa.stats-icon {
            width: 50px;
            padding: 20px;
            font-size: 50px;
            text-align: center;
            color: #fff;
            height: 50px;
            border-radius: 10px;
        }

        .panel-default {
            border: none;
            border-radius: 0px;
            margin-bottom: 0px;
            box-shadow: none;
            -webkit-box-shadow: none;
        }

            .panel-default > .panel-heading {
                color: #777;
                /*background-color: #fff;*/
                border-color: #e6e6e6;
                padding: 10px 10px;
            }

            .panel-default > .panel-body {
                padding: 10px;
                padding-bottom: 0px;
                border:0px;
            }

                .panel-default > .panel-body ul {
                    overflow: hidden;
                    padding: 0;
                    margin:0px;margin-top: -5px;
                }

                    .panel-default > .panel-body ul li {
                        line-height: 27px;
                        list-style-type: none;
                       /* white-space: nowrap;*/
                        text-overflow: ellipsis;
                    }

                        .panel-default > .panel-body ul li .time {
                            color: #a1a1a1;
                            float: right;
                            padding-right: 5px;
                        }
    </style>
</body>
</html>
