using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MarketPriceTracker
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool loggedIn = Session["Role"] != null;
            bool isAdmin = loggedIn && Session["Role"].ToString() == "Admin";

            liLogin.Visible = !loggedIn;
            liSignup.Visible = !loggedIn;

            liReports.Visible = loggedIn;
            liCompare.Visible = loggedIn;
            liMarkets.Visible = isAdmin;
            liProducts.Visible = isAdmin;
            liUsers.Visible = isAdmin;

            liDashboard.Visible = loggedIn;
            liProfile.Visible = loggedIn;
            liUser.Visible = loggedIn;
            liLogout.Visible = loggedIn;
            if (loggedIn) lblUser.Text = "Hello, " + Session["FullName"] + " (" + Session["Role"] + ")";
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}