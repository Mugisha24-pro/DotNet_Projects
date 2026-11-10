using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MarketPriceTracker
{
    public partial class Users : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        static string Hash(string text)
        {
            return Convert.ToBase64String(SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(text)));
        }

        void Msg(string text, bool ok)
        {
            lblMsg.CssClass = ok ? "text-success" : "text-danger";
            lblMsg.Text = text;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Role protection: Admin only
            if (Session["Role"] == null) Response.Redirect("Login.aspx");
            if (Session["Role"].ToString() != "Admin") Response.Redirect("Reports.aspx");

            if (!IsPostBack) LoadUsers();
        }

        void LoadUsers()
        {
            using (var con = new SqlConnection(cs))
            {
                var da = new SqlDataAdapter("SELECT UserId, FullName, Email, Phone, Role FROM Users ORDER BY FullName", con);
                var dt = new DataTable();
                da.Fill(dt);
                gvUsers.DataSource = dt;
                gvUsers.DataBind();
            }
        }

        // UPDATE
        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers();
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
            var row = gvUsers.Rows[e.RowIndex];
            string name = ((TextBox)row.Cells[1].Controls[0]).Text.Trim();
            string email = ((TextBox)row.Cells[2].Controls[0]).Text.Trim();
            string phone = ((TextBox)row.Cells[3].Controls[0]).Text.Trim();
            string role = ((DropDownList)row.FindControl("ddlRole")).SelectedValue;

            if (name == "" || email == "")
            {
                Msg("Name and email cannot be empty.", false);
                return;
            }

            // An Admin cannot remove their own Admin role
            if (id == Convert.ToInt32(Session["UserId"]) && role != "Admin")
            {
                Msg("You cannot remove your own Admin role.", false);
                return;
            }

            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // Email must be unique
                var check = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Email=@e AND UserId<>@id", con);
                check.Parameters.AddWithValue("@e", email);
                check.Parameters.AddWithValue("@id", id);
                if ((int)check.ExecuteScalar() > 0)
                {
                    Msg("This email is already used by another account.", false);
                    return;
                }

                var cmd = new SqlCommand(
                    "UPDATE Users SET FullName=@n, Email=@e, Phone=@p, Role=@r WHERE UserId=@id", con);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@e", email);
                cmd.Parameters.AddWithValue("@p", phone == "" ? (object)DBNull.Value : phone);
                cmd.Parameters.AddWithValue("@r", role);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            Msg("User updated.", true);
            gvUsers.EditIndex = -1;
            LoadUsers();
        }

        // DELETE
        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

            if (id == Convert.ToInt32(Session["UserId"]))
            {
                Msg("You cannot delete your own account.", false);
                return;
            }

            try
            {
                using (var con = new SqlConnection(cs))
                {
                    var cmd = new SqlCommand("DELETE FROM Users WHERE UserId=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                Msg("User deleted.", true);
            }
            catch (SqlException)
            {
                Msg("Cannot delete: this user has price reports. Delete their reports first.", false);
            }
            LoadUsers();
        }

        // RESET PASSWORD (temporary password: Welcome123)
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "ResetPwd") return;

            int id = Convert.ToInt32(e.CommandArgument);
            using (var con = new SqlConnection(cs))
            {
                var cmd = new SqlCommand(
                    "UPDATE Users SET PasswordHash=@h WHERE UserId=@id; SELECT FullName FROM Users WHERE UserId=@id", con);
                cmd.Parameters.AddWithValue("@h", Hash("Welcome123"));
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                string name = Convert.ToString(cmd.ExecuteScalar());
                Msg("Password for " + name + " was reset to Welcome123. Ask them to change it in My Profile.", true);
            }
        }
    }
}