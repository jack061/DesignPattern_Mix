using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RM.Web.Frame
{
    public partial class index : System.Web.UI.Page
    {
        public string username = "";
        public string sex = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                username = Session["username"].ToString();
                sex = Session["sex"].ToString();
            }
            else
            {
                Response.Redirect("login.html");
            }
        }
    }
}