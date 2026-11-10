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
    public partial class Products : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null) Response.Redirect("Login.aspx");
            if (Session["Role"].ToString() != "Admin") Response.Redirect("Reports.aspx");

            if (!IsPostBack) LoadProducts();
        }

        void LoadProducts()
        {
            using (var con = new SqlConnection(cs))
            {
                var da = new SqlDataAdapter("SELECT ProductId, Name, Unit FROM Products ORDER BY Name", con);
                var dt = new DataTable();
                da.Fill(dt);
                gvProducts.DataSource = dt;
                gvProducts.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            using (var con = new SqlConnection(cs))
            {
                var cmd = new SqlCommand("INSERT INTO Products(Name,Unit) VALUES(@n,@u)", con);
                cmd.Parameters.AddWithValue("@n", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@u", txtUnit.Text.Trim());
                con.Open();
                cmd.ExecuteNonQuery();
            }
            txtName.Text = ""; txtUnit.Text = "";
            lblMsg.Text = "";
            LoadProducts();
        }

        protected void gvProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvProducts.EditIndex = e.NewEditIndex;
            LoadProducts();
        }

        protected void gvProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvProducts.EditIndex = -1;
            LoadProducts();
        }

        protected void gvProducts_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            string name = ((TextBox)gvProducts.Rows[e.RowIndex].Cells[1].Controls[0]).Text.Trim();
            string unit = ((TextBox)gvProducts.Rows[e.RowIndex].Cells[2].Controls[0]).Text.Trim();

            using (var con = new SqlConnection(cs))
            {
                var cmd = new SqlCommand("UPDATE Products SET Name=@n, Unit=@u WHERE ProductId=@id", con);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@u", unit);
                cmd.Parameters.AddWithValue("@id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            gvProducts.EditIndex = -1;
            LoadProducts();
        }

        protected void gvProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvProducts.DataKeys[e.RowIndex].Value);
            try
            {
                using (var con = new SqlConnection(cs))
                {
                    var cmd = new SqlCommand("DELETE FROM Products WHERE ProductId=@id", con);
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                lblMsg.Text = "";
            }
            catch (SqlException)
            {
                lblMsg.Text = "Cannot delete: this product already has price reports.";
            }
            LoadProducts();
        }
    }
}