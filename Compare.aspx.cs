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
    public partial class Compare : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null) Response.Redirect("Login.aspx");

            if (!IsPostBack)
            {
                LoadProducts();
                LoadSummary();
            }
        }

        void LoadProducts()
        {
            using (var con = new SqlConnection(cs))
            {
                var dt = new DataTable();
                new SqlDataAdapter("SELECT ProductId, Name FROM Products ORDER BY Name", con).Fill(dt);
                ddlProduct.DataSource = dt;
                ddlProduct.DataValueField = "ProductId";
                ddlProduct.DataTextField = "Name";
                ddlProduct.DataBind();
            }
        }

        // Statistics per product: count, min, average, max
        void LoadSummary()
        {
            using (var con = new SqlConnection(cs))
            {
                var da = new SqlDataAdapter(
                    "SELECT p.Name AS Product, p.Unit, COUNT(r.ReportId) AS Reports, " +
                    "MIN(r.Price) AS Lowest, AVG(r.Price) AS Average, MAX(r.Price) AS Highest " +
                    "FROM Products p " +
                    "LEFT JOIN PriceReports r ON r.ProductId = p.ProductId " +
                    "GROUP BY p.ProductId, p.Name, p.Unit " +
                    "ORDER BY p.Name", con);
                var dt = new DataTable();
                da.Fill(dt);
                gvSummary.DataSource = dt;
                gvSummary.DataBind();
            }
        }

        protected void ddlProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProduct.SelectedValue == "")
            {
                gvMarkets.Visible = false;
                lblCheapest.Visible = false;
                return;
            }
            LoadMarkets(Convert.ToInt32(ddlProduct.SelectedValue));
        }

        // Average price per market for the chosen product, cheapest first
        void LoadMarkets(int productId)
        {
            using (var con = new SqlConnection(cs))
            {
                var da = new SqlDataAdapter(
                    "SELECT m.Name AS Market, m.District, COUNT(*) AS Reports, " +
                    "MIN(r.Price) AS Lowest, AVG(r.Price) AS Average, MAX(r.ReportDate) AS LastReported " +
                    "FROM PriceReports r " +
                    "JOIN Markets m ON r.MarketId = m.MarketId " +
                    "WHERE r.ProductId = @pid " +
                    "GROUP BY m.MarketId, m.Name, m.District " +
                    "ORDER BY AVG(r.Price) ASC", con);
                da.SelectCommand.Parameters.AddWithValue("@pid", productId);
                var dt = new DataTable();
                da.Fill(dt);

                gvMarkets.Visible = true;
                gvMarkets.DataSource = dt;
                gvMarkets.DataBind();

                if (dt.Rows.Count > 0)
                {
                    lblCheapest.Text = "Cheapest market for " + ddlProduct.SelectedItem.Text + ": " +
                        dt.Rows[0]["Market"] + " (average " +
                        Convert.ToDecimal(dt.Rows[0]["Average"]).ToString("N0") + " RWF)";
                    lblCheapest.Visible = true;
                }
                else
                {
                    lblCheapest.Visible = false;
                }
            }
        }

        // Highlight the cheapest (first) row
        protected void gvMarkets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == 0)
            {
                e.Row.CssClass = "table-success";
                e.Row.Cells[0].Text += " <span class=\"badge bg-success\">Cheapest</span>";
            }
        }
    }
}