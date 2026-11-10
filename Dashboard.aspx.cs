using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MarketPriceTracker
{
    public partial class Dashboard : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"] == null)
            {
                Response.Redirect("Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            bool isAdmin = Session["Role"].ToString() == "Admin";
            int uid = Convert.ToInt32(Session["UserId"]);
            int admin = isAdmin ? 1 : 0;

            lblWelcome.Text = "Welcome, " + Session["FullName"] + "! " +
                (isAdmin ? "Here is an overview of the whole system." : "Here is a summary of your activity.");

            using (var con = new SqlConnection(cs))
            {
                con.Open();

                // Stat cards
                if (isAdmin)
                {
                    SetCard(1, "Users", Scalar(con, "SELECT COUNT(*) FROM Users", uid, admin));
                    SetCard(2, "Markets", Scalar(con, "SELECT COUNT(*) FROM Markets", uid, admin));
                    SetCard(3, "Products", Scalar(con, "SELECT COUNT(*) FROM Products", uid, admin));
                    SetCard(4, "Price reports", Scalar(con, "SELECT COUNT(*) FROM PriceReports", uid, admin));
                }
                else
                {
                    SetCard(1, "My reports", Scalar(con, "SELECT COUNT(*) FROM PriceReports WHERE UserId=@uid", uid, admin));
                    SetCard(2, "Markets I covered", Scalar(con, "SELECT COUNT(DISTINCT MarketId) FROM PriceReports WHERE UserId=@uid", uid, admin));
                    SetCard(3, "Products I covered", Scalar(con, "SELECT COUNT(DISTINCT ProductId) FROM PriceReports WHERE UserId=@uid", uid, admin));
                    SetCard(4, "All reports (community)", Scalar(con, "SELECT COUNT(*) FROM PriceReports", uid, admin));
                }

                // Chart 1: average price by product
                var dtAvg = Fill(con,
                    "SELECT p.Name, AVG(r.Price) AS AvgPrice FROM PriceReports r " +
                    "JOIN Products p ON r.ProductId = p.ProductId " +
                    "GROUP BY p.ProductId, p.Name ORDER BY p.Name", uid, admin);
                hfAvgLabels.Value = ToJson(
                    dtAvg.AsEnumerable()
                    .Select(r => r["Name"].ToString())
                    .ToList()
                );

                hfAvgValues.Value = ToJson(
                    dtAvg.AsEnumerable()
                    .Select(r => Math.Round(Convert.ToDecimal(r["AvgPrice"]), 0))
                    .ToList()
                ); 

                // Chart 2: Admin = reports per market, User = my reports per product
                DataTable dtSecond;
                if (isAdmin)
                {
                    lblChart2Title.Text = "Reports per market";
                    dtSecond = Fill(con,
                        "SELECT m.Name, COUNT(*) AS Total FROM PriceReports r " +
                        "JOIN Markets m ON r.MarketId = m.MarketId " +
                        "GROUP BY m.MarketId, m.Name ORDER BY Total DESC", uid, admin);
                }
                else
                {
                    lblChart2Title.Text = "My reports per product";
                    dtSecond = Fill(con,
                        "SELECT p.Name, COUNT(*) AS Total FROM PriceReports r " +
                        "JOIN Products p ON r.ProductId = p.ProductId " +
                        "WHERE r.UserId = @uid " +
                        "GROUP BY p.ProductId, p.Name ORDER BY Total DESC", uid, admin);
                }
                hfSecondLabels.Value = ToJson(
                    dtSecond.AsEnumerable()
                    .Select(r => r["Name"].ToString())
                    .ToList()
                );

                hfSecondValues.Value = ToJson(
                    dtSecond.AsEnumerable()
                    .Select(r => Convert.ToInt32(r["Total"]))
                    .ToList()
                );

                // Recent reports (Admin: everyone's, User: own)
                var dtRecent = Fill(con,
                    "SELECT TOP 5 u.FullName AS Reporter, m.Name AS Market, p.Name AS Product, r.Price, r.ReportDate " +
                    "FROM PriceReports r " +
                    "JOIN Users u ON r.UserId = u.UserId " +
                    "JOIN Markets m ON r.MarketId = m.MarketId " +
                    "JOIN Products p ON r.ProductId = p.ProductId " +
                    "WHERE (@admin = 1 OR r.UserId = @uid) " +
                    "ORDER BY r.ReportDate DESC, r.ReportId DESC", uid, admin);
                gvRecent.Columns[0].Visible = isAdmin;   // the "Reported by" column is only for Admin
                gvRecent.DataSource = dtRecent;
                gvRecent.DataBind();
            }
        }
        object Scalar(SqlConnection con, string sql, int uid, int admin)
        {
            var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@uid", uid);
            cmd.Parameters.AddWithValue("@admin", admin);
            return cmd.ExecuteScalar();
        }

        DataTable Fill(SqlConnection con, string sql, int uid, int admin)
        {
            var da = new SqlDataAdapter(sql, con);
            da.SelectCommand.Parameters.AddWithValue("@uid", uid);
            da.SelectCommand.Parameters.AddWithValue("@admin", admin);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        void SetCard(int n, string title, object value)
        {
            switch (n)
            {
                case 1:
                    lblT1.Text = title;
                    lblV1.Text = Convert.ToString(value);
                    break;

                case 2:
                    lblT2.Text = title;
                    lblV2.Text = Convert.ToString(value);
                    break;

                case 3:
                    lblT3.Text = title;
                    lblV3.Text = Convert.ToString(value);
                    break;

                case 4:
                    lblT4.Text = title;
                    lblV4.Text = Convert.ToString(value);
                    break;
            }
        }

        static string ToJson(object data)
        {
            return new JavaScriptSerializer().Serialize(data);
        }
    }
}