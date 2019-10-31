using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        public int i;
        protected void Page_Load(object sender, EventArgs e)
        {
            localhost.ServiceSoap ss = new localhost.ServiceSoapClient();
            i = ss.GetSum(3, 2);
        }
    }
}