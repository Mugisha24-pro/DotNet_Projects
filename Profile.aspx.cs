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
    public partial class Profile : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        // The user id always comes from the Session, never from the page
        int CurrentUserId { get { return Convert.ToInt32(Session["UserId"]); } }

        static string Hash(string text)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        static void Msg(Label lbl, string text, bool ok)
        {
            lbl.CssClass = ok ? "text-success" : "text-danger";
            lbl.Text = text;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null) Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadProfile();
                if (Request.QueryString["msg"] == "saved")
                    Msg(lblProfileMsg, "Profile updated successfully.", true);
            }
        }

        void LoadProfile()
        {
            using (var con = new SqlConnection(cs))
            {
                var cmd = new SqlCommand("SELECT FullName, Email, Phone, Role FROM Users WHERE UserId=@id", con);
                cmd.Parameters.AddWithValue("@id", CurrentUserId);
                con.Open();
                using (var rd = cmd.ExecuteReader())
                {
                    if (rd.Read())
                    {
                        txtName.Text = rd["FullName"].ToString();
                        txtEmail.Text = rd["Email"].ToString();
                        txtPhone.Text = rd["Phone"] == DBNull.Value ? "" : rd["Phone"].ToString();
                        lblRole.Text = rd["Role"].ToString();
                    }
                }
            }
        }

        // UPDATE personal details
        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // Email must not belong to another account
                var check = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email=@e AND UserId<>@id", con);
                check.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                check.Parameters.AddWithValue("@id", CurrentUserId);
                if ((int)check.ExecuteScalar() > 0)
                {
                    Msg(lblProfileMsg, "This email is already used by another account.", false);
                    return;
                }

                var cmd = new SqlCommand("UPDATE Users SET FullName=@n, Email=@e, Phone=@p WHERE UserId=@id", con);
                cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@e", txtEmail.Text.Trim());
                cmd.Parameters.AddWithValue("@p", string.IsNullOrWhiteSpace(txtPhone.Text)
                    ? (object)DBNull.Value : txtPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@id", CurrentUserId);
                cmd.ExecuteNonQuery();
            }

            // Update the name shown in the sidebar, then reload so the master page refreshes
            Session["FullName"] = txtName.Text.Trim();
            Response.Redirect("Profile.aspx?msg=saved");
        }

        // CHANGE password
        protected void btnPassword_Click(object sender, EventArgs e)
        {
            if (txtNew.Text.Length < 6)
            {
                Msg(lblPwdMsg, "The new password must be at least 6 characters.", false);
                return;
            }

            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // The current password must be correct
                var check = new SqlCommand("SELECT COUNT(*) FROM Users WHERE UserId=@id AND PasswordHash=@h", con);
                check.Parameters.AddWithValue("@id", CurrentUserId);
                check.Parameters.AddWithValue("@h", Hash(txtCurrent.Text));
                if ((int)check.ExecuteScalar() == 0)
                {
                    Msg(lblPwdMsg, "Your current password is incorrect.", false);
                    return;
                }

                var cmd = new SqlCommand("UPDATE Users SET PasswordHash=@h WHERE UserId=@id", con);
                cmd.Parameters.AddWithValue("@h", Hash(txtNew.Text));
                cmd.Parameters.AddWithValue("@id", CurrentUserId);
                cmd.ExecuteNonQuery();
            }
            Msg(lblPwdMsg, "Password changed successfully.", true);
        }
    }
}