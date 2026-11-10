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
    public partial class Reports : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

        int CurrentUserId { get { return Convert.ToInt32(Session["UserId"]); } }
        int IsAdmin { get { return Session["Role"].ToString() == "Admin" ? 1 : 0; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null) Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadDropdowns();
                LoadReports();
            }
        }

        void LoadDropdowns()
        {
            using (var con = new SqlConnection(cs))
            {
                con.Open();

                var dtM = new DataTable();
                new SqlDataAdapter("SELECT MarketId, Name FROM Markets ORDER BY Name", con).Fill(dtM);
                ddlMarket.DataSource = dtM;
                ddlMarket.DataValueField = "MarketId";
                ddlMarket.DataTextField = "Name";
                ddlMarket.DataBind();

                var dtP = new DataTable();
                new SqlDataAdapter("SELECT ProductId, Name + ' (' + Unit + ')' AS Label FROM Products ORDER BY Name", con).Fill(dtP);
                ddlProduct.DataSource = dtP;
                ddlProduct.DataValueField = "ProductId";
                ddlProduct.DataTextField = "Label";
                ddlProduct.DataBind();
            }
        }

        // READ: Admin sees all, User sees only own
        void LoadReports()
        {
            using (var con = new SqlConnection(cs))
            {
                var da = new SqlDataAdapter(
                    "SELECT r.ReportId, u.FullName AS Reporter, m.Name AS Market, p.Name AS Product, r.Price, r.ReportDate " +
                    "FROM PriceReports r " +
                    "JOIN Users u ON r.UserId = u.UserId " +
                    "JOIN Markets m ON r.MarketId = m.MarketId " +
                    "JOIN Products p ON r.ProductId = p.ProductId " +
                    "WHERE (@admin = 1 OR r.UserId = @uid) " +
                    "ORDER BY r.ReportDate DESC, r.ReportId DESC", con);
                da.SelectCommand.Parameters.AddWithValue("@admin", IsAdmin);
                da.SelectCommand.Parameters.AddWithValue("@uid", CurrentUserId);
                var dt = new DataTable();
                da.Fill(dt);
                gvReports.DataSource = dt;
                gvReports.DataBind();
            }
        }

        // CREATE
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            decimal price;
            if (!decimal.TryParse(txtPrice.Text, out price) || price <= 0)
            {
                lblMsg.Text = "Enter a valid price.";
                return;
            }

            using (var con = new SqlConnection(cs))
            {
                var cmd = new SqlCommand(
                    "INSERT INTO PriceReports(MarketId,ProductId,UserId,Price,ReportDate) " +
                    "VALUES(@m,@p,@u,@pr,GETDATE())", con);
                cmd.Parameters.AddWithValue("@m", ddlMarket.SelectedValue);
                cmd.Parameters.AddWithValue("@p", ddlProduct.SelectedValue);
                cmd.Parameters.AddWithValue("@u", CurrentUserId);
                cmd.Parameters.AddWithValue("@pr", price);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            txtPrice.Text = "";
            ddlMarket.SelectedIndex = 0;
            ddlProduct.SelectedIndex = 0;
            lblMsg.Text = "";
            LoadReports();
        }

        // UPDATE
        protected void gvReports_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvReports.EditIndex = e.NewEditIndex;
            LoadReports();
        }

        protected void gvReports_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvReports.EditIndex = -1;
            LoadReports();
        }

        protected void gvReports_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvReports.DataKeys[e.RowIndex].Value);
            string text = ((TextBox)gvReports.Rows[e.RowIndex].Cells[4].Controls[0]).Text;

            decimal price;
            if (!decimal.TryParse(text, out price) || price <= 0)
            {
                lblMsg.Text = "Enter a valid price.";
                return;
            }

            using (var con = new SqlConnection(cs))
            {
                // Own-reports-only rule: a User can only update rows where UserId is theirs
                var cmd = new SqlCommand(
                    "UPDATE PriceReports SET Price=@pr WHERE ReportId=@id AND (@admin=1 OR UserId=@uid)", con);
                cmd.Parameters.AddWithValue("@pr", price);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@admin", IsAdmin);
                cmd.Parameters.AddWithValue("@uid", CurrentUserId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            lblMsg.Text = "";
            gvReports.EditIndex = -1;
            LoadReports();
        }

        // DELETE
        protected void gvReports_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvReports.DataKeys[e.RowIndex].Value);
            using (var con = new SqlConnection(cs))
            {
                var cmd = new SqlCommand(
                    "DELETE FROM PriceReports WHERE ReportId=@id AND (@admin=1 OR UserId=@uid)", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@admin", IsAdmin);
                cmd.Parameters.AddWithValue("@uid", CurrentUserId);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            lblMsg.Text = "";
            LoadReports();
        }
    }
}