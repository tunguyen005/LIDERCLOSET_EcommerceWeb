using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.User
{

    public partial class OrderConfirmation : System.Web.UI.Page
    {

        private readonly DataAccess _da = new DataAccess();
        private const decimal SHIPPING_FEE = 30000;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string maDonHang = Request.QueryString["MaDonHang"];
                if (string.IsNullOrEmpty(maDonHang))
                {
                    Response.Redirect("Shop.aspx");
                }

                LoadOrderDetails(maDonHang);
            }
        }

        private void LoadOrderDetails(string maDonHang)
        {
            try
            {
                // Lấy thông tin đơn hàng
                string query = @"SELECT MaDonHang, NgayDatHang, TongTien, PhuongThucThanhToan, DiaChiThanhToan, DiaChiGiaoHang
                            FROM tblDONHANG 
                            WHERE MaDonHang = @MaDonHang";
                SqlParameter[] parameters = { new SqlParameter("@MaDonHang", maDonHang) };
                DataTable dtOrder = _da.ExecuteQuery(query, parameters);

                if (dtOrder.Rows.Count == 0)
                {
                    Response.Redirect("Shop.aspx");
                }

                DataRow order = dtOrder.Rows[0];
                lblMaDonHang.Text = order["MaDonHang"].ToString();
                lblPaymentMethod.Text = order["PhuongThucThanhToan"].ToString();
                lblBillingAddress.Text = order["DiaChiThanhToan"].ToString();
                lblShippingAddress.Text = order["DiaChiGiaoHang"] != DBNull.Value ? order["DiaChiGiaoHang"].ToString() : "Giống địa chỉ thanh toán";

                // Lấy chi tiết đơn hàng
                query = @"SELECT ct.MaSP, sp.TenSP, ct.KichCo, ct.MauSac, ct.SoLuong, ct.Gia, (ct.SoLuong * ct.Gia) AS Total
                      FROM tblCHITIETDONHANG ct
                      JOIN tblSANPHAM sp ON ct.MaSP = sp.MaSP
                      WHERE ct.MaDonHang = @MaDonHang";
                DataTable dtItems = _da.ExecuteQuery(query, parameters);

                rptOrderItems.DataSource = dtItems;
                rptOrderItems.DataBind();

                // Tính tổng tiền
                decimal subtotal = dtItems.AsEnumerable().Sum(row => Convert.ToDecimal(row["Total"]));
                decimal discount = Convert.ToDecimal(order["TongTien"]) - subtotal - SHIPPING_FEE; // Tổng tiền đã trừ phí vận chuyển
                decimal total = Convert.ToDecimal(order["TongTien"]);

                lblSubtotal.Text = string.Format("{0:N0} VNĐ", subtotal);
                lblShipping.Text = string.Format("{0:N0} VNĐ", SHIPPING_FEE);
                lblDiscount.Text = string.Format("{0:N0} VNĐ", discount);
                lblTotal.Text = string.Format("{0:N0} VNĐ", total);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi tải thông tin đơn hàng: {ex.Message}');", true);
            }
        }
    }
}
