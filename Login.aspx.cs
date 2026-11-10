using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MarketPriceTracker
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string hash = Convert.ToBase64String(
                 SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(txtPassword.Text)));

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                var cmd = new SqlCommand(
                    "SELECT UserId, FullName, Role FROM Users WHERE Email=@e AND PasswordHash=@p", con);
                cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@p", hash);
                con.Open();

                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        Session["UserId"] = rd["UserId"];
                        Session["FullName"] = rd["FullName"].ToString();
                        Session["Role"] = rd["Role"].ToString();

                        if (Session["Role"].ToString() == "Admin")
                            Response.Redirect("Dashboard.aspx");
                        else
                            Response.Redirect("Dashboard.aspx"); ;
                    }
                    else
                    {
                        lblMsg.Text = "Invalid email or password.";
                    }
                }

            }
        }
    }
}