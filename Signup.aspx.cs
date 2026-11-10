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
    public partial class Signup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSignup_Click(object sender, EventArgs e)
        {
            string hash = Convert.ToBase64String(
                SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(txtPassword.Text)));

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["db"].ConnectionString))
            {
                con.Open();

                // 1. Check if the email already exists
                var check = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email=@e", con);
                check.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                if ((int)check.ExecuteScalar() > 0)
                {
                    lblMsg.Text = "This email is already registered.";
                    return;
                }

                // 2. Insert the new user (role is always 'User')
                var cmd = new SqlCommand(
                    "INSERT INTO Users(FullName,Email,PasswordHash,Role) VALUES(@n,@e,@p,'User')", con);
                cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@p", hash);
                cmd.ExecuteNonQuery();
            }
            Response.Redirect("Login.aspx");
        }
    }
}