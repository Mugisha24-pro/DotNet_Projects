using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MarketPriceTracker
{
    public partial class Markets : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Role protection
            if (Session["Role"] == null) Response.Redirect("Login.aspx");
            if (Session["Role"].ToString() != "Admin") Response.Redirect("Reports.aspx");

            if (!IsPostBack) LoadMarkets();
        }

        void LoadMarkets()
        {
            using (var con = new SqlConnection(cs))
            {
                var da = new SqlDataAdapter("SELECT MarketId, Name, District FROM Markets ORDER BY Name", con);
                var dt = new DataTable();
                da.Fill(dt);
                gvMarkets.DataSource = dt;
                gvMarkets.DataBind();
            }
        }

        // CREATE
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(cs))
            {
                var cmd = new SqlCommand("INSERT INTO Markets(Name,District) VALUES(@n,@d)", con);
                cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@d", txtDistrict.Text.Trim());
                con.Open();
                cmd.ExecuteNonQuery();
            }
            txtName.Text = ""; txtDistrict.Text = "";
            lblMsg.Text = "";
            LoadMarkets();
        }

        // UPDATE (edit mode)
        protected void gvMarkets_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvMarkets.EditIndex = e.NewEditIndex;
            LoadMarkets();
        }

        protected void gvMarkets_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvMarkets.EditIndex = -1;
            LoadMarkets();
        }

        protected void gvMarkets_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvMarkets.DataKeys[e.RowIndex].Value);
            string name = ((TextBox)gvMarkets.Rows[e.RowIndex].Cells[1].Controls[0]).Text.Trim();
            string district = ((TextBox)gvMarkets.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();

            using (var con = new SqlConnection(cs))
            {
                var cmd = new SqlCommand("UPDATE Markets SET Name=@n, District=@d WHERE MarketId=@id", con);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@d", district);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            gvMarkets.EditIndex = -1;
            LoadMarkets();
        }

        // DELETE
        protected void gvMarkets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvMarkets.DataKeys[e.RowIndex].Value);
            try
            {
                using (var con = new SqlConnection(cs))
                {
                    var cmd = new SqlCommand("DELETE FROM Markets WHERE MarketId=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                lblMsg.Text = "";
            }
            catch (SqlException)
            {
                lblMsg.Text = "Cannot delete: this market already has price reports.";
            }
            LoadMarkets();
        }
    }
}