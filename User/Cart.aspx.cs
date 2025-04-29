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
    public partial class Cart : System.Web.UI.Page
    {

        private readonly DataAccess _da = new DataAccess();
        private const decimal SHIPPING_FEE = 30000;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserID"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                LoadCart();
            }
        }

        private void LoadCart()
        {
            try
            {
                string userID = Session["UserID"].ToString();
                string query = @"SELECT gh.MaGioHang, gh.MaSP, sp.TenSP, gh.Size, gh.MauSac, gh.SoLuong, sp.DonGia, 
                            (gh.SoLuong * sp.DonGia) AS Total, pi.ImageUrl
                            FROM tblGIOHANG gh
                            JOIN tblSANPHAM sp ON gh.MaSP = sp.MaSP
                            LEFT JOIN ProductImages pi ON sp.MaSP = pi.MaSP AND pi.AnhHH = 1
                            WHERE gh.UserID = @UserID";
                SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };
                DataTable cart = _da.ExecuteQuery(query, parameters);

                if (cart.Rows.Count == 0)
                {
                    pnlCart.Visible = false;
                    pnlEmptyCart.Visible = true;
                    btnCheckout.Enabled = false;
                    return;
                }

                pnlCart.Visible = true;
                pnlEmptyCart.Visible = false;
                btnCheckout.Enabled = true;

                rptCart.DataSource = cart;
                rptCart.DataBind();

                decimal subtotal = cart.AsEnumerable().Sum(row => Convert.ToDecimal(row["Total"]));
                decimal discount = Session["Coupon"] != null ? (decimal)Session["Coupon"] * subtotal : 0;
                decimal total = subtotal + SHIPPING_FEE - discount;

                lblSubtotal.Text = string.Format("{0:N0} VNĐ", subtotal);
                lblShipping.Text = string.Format("{0:N0} VNĐ", SHIPPING_FEE);
                lblDiscount.Text = string.Format("{0:N0} VNĐ", discount);
                lblTotal.Text = string.Format("{0:N0} VNĐ", total);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi tải giỏ hàng: {ex.Message}');", true);
            }
        }

        protected void rptCart_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                string userID = Session["UserID"].ToString();
                int maGioHang = int.Parse(e.CommandArgument.ToString());

                string query = "SELECT MaSP, SoLuong FROM tblGIOHANG WHERE MaGioHang = @MaGioHang AND UserID = @UserID";
                SqlParameter[] parameters = {
                new SqlParameter("@MaGioHang", maGioHang),
                new SqlParameter("@UserID", userID)
            };
                DataTable dt = _da.ExecuteQuery(query, parameters);
                if (dt.Rows.Count == 0) return;

                int maSP = Convert.ToInt32(dt.Rows[0]["MaSP"]);
                int soLuong = Convert.ToInt32(dt.Rows[0]["SoLuong"]);
                int soLuongTon = GetSoLuongTon(maSP);

                switch (e.CommandName)
                {
                    case "Increase":
                        if (soLuong < soLuongTon)
                        {
                            query = "UPDATE tblGIOHANG SET SoLuong = SoLuong + 1 WHERE MaGioHang = @MaGioHang AND UserID = @UserID";
                            _da.ExecuteNonQuery(query, parameters);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Số lượng vượt quá tồn kho!');", true);
                        }
                        break;

                    case "Decrease":
                        if (soLuong > 1)
                        {
                            query = "UPDATE tblGIOHANG SET SoLuong = SoLuong - 1 WHERE MaGioHang = @MaGioHang AND UserID = @UserID";
                            _da.ExecuteNonQuery(query, parameters);
                        }
                        break;

                    case "Remove":
                        query = "DELETE FROM tblGIOHANG WHERE MaGioHang = @MaGioHang AND UserID = @UserID";
                        _da.ExecuteNonQuery(query, parameters);
                        break;
                }

                LoadCart();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi cập nhật giỏ hàng: {ex.Message}');", true);
            }
        }

        private int GetSoLuongTon(int maSP)
        {
            string query = "SELECT SoLuong FROM tblSANPHAM WHERE MaSP = @MaSP";
            SqlParameter[] parameters = { new SqlParameter("@MaSP", maSP) };
            DataTable dt = _da.ExecuteQuery(query, parameters);
            return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["SoLuong"]) : 0;
        }

        protected void btnApplyCoupon_Click(object sender, EventArgs e)
        {
            try
            {
                string couponCode = txtCoupon.Text.Trim();
                if (string.IsNullOrEmpty(couponCode))
                {
                    lblCouponMessage.Text = "Vui lòng nhập mã giảm giá!";
                    lblCouponMessage.Visible = true;
                    return;
                }

                string query = @"SELECT DiscountPercent 
                            FROM tblCOUPON 
                            WHERE CouponCode = @CouponCode 
                            AND IsActive = 1 
                            AND ExpiryDate >= @Today";
                SqlParameter[] parameters = {
                new SqlParameter("@CouponCode", couponCode),
                new SqlParameter("@Today", DateTime.Now)
            };
                DataTable dt = _da.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    decimal discountPercent = Convert.ToDecimal(dt.Rows[0]["DiscountPercent"]) / 100;
                    Session["Coupon"] = discountPercent;
                    lblCouponMessage.Text = $"Mã giảm giá {couponCode} đã được áp dụng ({Convert.ToInt32(discountPercent * 100)}%)!";
                    lblCouponMessage.CssClass = "text-success mt-2";
                }
                else
                {
                    Session["Coupon"] = null;
                    lblCouponMessage.Text = "Mã giảm giá không hợp lệ hoặc đã hết hạn!";
                    lblCouponMessage.CssClass = "text-danger mt-2";
                }

                lblCouponMessage.Visible = true;
                LoadCart();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi áp dụng mã giảm giá: {ex.Message}');", true);
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            string userID = Session["UserID"].ToString();
            string query = "SELECT COUNT(*) FROM tblGIOHANG WHERE UserID = @UserID";
            SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };
            int count = Convert.ToInt32(_da.ExecuteScalar(query, parameters));

            if (count == 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Giỏ hàng trống! Vui lòng thêm sản phẩm trước khi thanh toán.');", true);
                return;
            }

            Response.Redirect("Checkout.aspx");
        }
    }
}
