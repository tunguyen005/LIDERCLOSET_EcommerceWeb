using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();
        public string OrderDataJson { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCumbTitle"] = "Tổng quan";
                Session["breadCumbPage"] = "Quản lý";
                LoadDashboardData();
            }
        }

        void LoadDashboardData()
        {
            try
            {
                // Số người dùng (khách hàng - MaCV = 2)
                string query = "SELECT COUNT(*) FROM tblUSER WHERE MaCV = 2";
                object result = _da.ExecuteScalar(query);
                lblUserCount.Text = result != null ? result.ToString() : "0";

                // Số danh mục cha đang hoạt động
                query = "SELECT COUNT(*) FROM tblHANGMUC WHERE isActive = 1";
                result = _da.ExecuteScalar(query);
                lblCategoryCount.Text = result != null ? result.ToString() : "0";

                // Số sản phẩm đang hoạt động
                query = "SELECT COUNT(*) FROM tblSANPHAM WHERE isActive = 1";
                result = _da.ExecuteScalar(query);
                lblProductCount.Text = result != null ? result.ToString() : "0";

                // Số đơn hàng
                query = "SELECT COUNT(*) FROM tblDONHANG";
                result = _da.ExecuteScalar(query);
                lblOrderCount.Text = result != null ? result.ToString() : "0";

                // Tổng doanh thu
                query = "SELECT SUM(TongTien) FROM tblDONHANG WHERE TrangThai = N'Đã giao'";
                result = _da.ExecuteScalar(query);
                lblTotalRevenue.Text = result != null ? Convert.ToDecimal(result).ToString("N0") + " VNĐ" : "0 VNĐ";

                // Dữ liệu đơn hàng theo ngày (7 ngày gần nhất)
                query = @"
                    SELECT CONVERT(VARCHAR(10), NgayDatHang, 103) AS Date, COUNT(*) AS Count
                    FROM tblDONHANG
                    WHERE NgayDatHang >= DATEADD(DAY, -7, GETDATE())
                    GROUP BY CONVERT(VARCHAR(10), NgayDatHang, 103)
                    ORDER BY CONVERT(VARCHAR(10), NgayDatHang, 103)";
                DataTable dt = _da.ExecuteQuery(query);

                var orderData = new System.Collections.Generic.List<object>();
                foreach (DataRow row in dt.Rows)
                {
                    orderData.Add(new { Date = row["Date"].ToString(), Count = Convert.ToInt32(row["Count"]) });
                }
                OrderDataJson = new JavaScriptSerializer().Serialize(orderData);
            }
            catch (Exception ex)
            {
                Response.Write($"<div class='alert alert-danger'>Lỗi khi tải dữ liệu: {ex.Message}</div>");
            }
        }
    }
}