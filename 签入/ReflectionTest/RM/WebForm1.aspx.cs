using RM.WebService.ServiceClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace RM
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public string userInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            localService.ServiceSoapClient ss = new localService.ServiceSoapClient();

            #region 客户
            localService.CustomerEntity entity = new localService.CustomerEntity();
            entity.code = "vvvvv";
            entity.shortname = "s3s";
            entity.name = "sdf23";
            entity.flag = "1";
            entity.salesman = "lbja2";
            entity.property = "Z001";
            ss.AddCustomer(entity);
            #endregion

            #region 供应商
            //localService.BsupplierEntity entity = new localService.BsupplierEntity();
            //localService.bsupplier_collectionEntity collection_entity = new localService.bsupplier_collectionEntity();
            //collection_entity.icnaccount = "32";
            //collection_entity.icnaddress = "dfd";
            //collection_entity.icnbank = "sd";
            //collection_entity.icncreditcode = "sdf";
            ////localService.bsupplier_collectionEntity[] collectionArray = new localService.bsupplier_collectionEntity[50];
            ////collectionArray[0] = collection_entity;
            //entity.PROCESS_ID = "ddd";
            //entity.DATA_CODE = "dcc";
            //entity.KLART = "cs23";
            //entity.name = "df";
            //entity.code = "zaee";
            //entity.shortname = "c";
            //entity.flag = "0";
            //entity.salesman = "lbj";
            ////entity.collectionEntity = collectionArray;
            ////entity.collectionEntity = list_entity;
            // ss.AddBsupplier(entity);
            #endregion
        }
    }
}