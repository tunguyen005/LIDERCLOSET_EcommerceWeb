using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.User
{
    public partial class Checkout : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCart();
            }
        }

        private void LoadCart()
        {
            try
            {
                int userId = 0; // Không yêu cầu đăng nhập, để userId = 0 (khách vãng lai)
                string query = @"SELECT g.MaGioHang, g.MaSP, sp.TenSP, sp.DonGia AS GiaGiam, pi.ImageUrl AS HinhAnh, g.Size, g.MauSac, g.SoLuong 
                                FROM tblGIOHANG g
                                JOIN tblSANPHAM sp ON g.MaSP = sp.MaSP
                                LEFT JOIN ProductImages pi ON sp.MaSP = pi.MaSP AND pi.AnhHH = 1
                                WHERE g.UserID = @UserID";
                SqlParameter[] parameters = { new SqlParameter("@UserID", userId) };
                DataTable cart = _da.ExecuteQuery(query, parameters);

                if (cart == null || cart.Rows.Count == 0)
                {
                    lblMsg.Text = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm trước khi thanh toán.";
                    lblMsg.CssClass = "alert alert-warning";
                    lblMsg.Visible = true;
                    btnCheckout.Enabled = false;
                    return;
                }

                rptCart.DataSource = cart;
                rptCart.DataBind();

                decimal totalPrice = 0;
                foreach (DataRow row in cart.Rows)
                {
                    totalPrice += Convert.ToDecimal(row["GiaGiam"]) * Convert.ToInt32(row["SoLuong"]);
                }
                lblTotalPrice.Text = string.Format("{0:N0} VNĐ", totalPrice);
                ViewState["TotalPrice"] = totalPrice; // Lưu tổng tiền để sử dụng khi áp dụng mã giảm giá
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Lỗi khi tải giỏ hàng: {ex.Message}";
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void btnApplyCoupon_Click(object sender, EventArgs e)
        {
            try
            {
                string couponCode = txtCoupon.Text.Trim();
                if (string.IsNullOrEmpty(couponCode))
                {
                    lblMsg.Text = "Vui lòng nhập mã giảm giá.";
                    lblMsg.CssClass = "alert alert-warning";
                    lblMsg.Visible = true;
                    return;
                }

                string query = @"SELECT DiscountPercent 
                                FROM tblCOUPON 
                                WHERE CouponCode = @CouponCode AND IsActive = 1 AND ExpiryDate > @CurrentDate";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@CouponCode", couponCode),
                    new SqlParameter("@CurrentDate", DateTime.Now)
                };
                DataTable dt = _da.ExecuteQuery(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    lblMsg.Text = "Mã giảm giá không hợp lệ hoặc đã hết hạn.";
                    lblMsg.CssClass = "alert alert-danger";
                    lblMsg.Visible = true;
                    return;
                }

                decimal discountPercent = Convert.ToDecimal(dt.Rows[0]["DiscountPercent"]);
                decimal totalPrice = Convert.ToDecimal(ViewState["TotalPrice"]);
                decimal discountAmount = totalPrice * (discountPercent / 100);
                decimal finalPrice = totalPrice - discountAmount;

                lblTotalPrice.Text = string.Format("{0:N0} VNĐ", finalPrice);
                ViewState["FinalPrice"] = finalPrice;
                lblMsg.Text = $"Áp dụng mã giảm giá thành công! Bạn được giảm {discountPercent}%.";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Lỗi khi áp dụng mã giảm giá: {ex.Message}";
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int userId = 0; // Không yêu cầu đăng nhập, để userId = 0 (khách vãng lai)

                // Lấy giỏ hàng
                string query = @"SELECT g.MaGioHang, g.MaSP, sp.TenSP, sp.DonGia AS GiaGiam, g.Size, g.MauSac, g.SoLuong, sp.SoLuong AS SoLuongTon 
                                FROM tblGIOHANG g
                                JOIN tblSANPHAM sp ON g.MaSP = sp.MaSP
                                WHERE g.UserID = @UserID";
                SqlParameter[] parameters = { new SqlParameter("@UserID", userId) };
                DataTable cart = _da.ExecuteQuery(query, parameters);

                if (cart == null || cart.Rows.Count == 0)
                {
                    lblMsg.Text = "Giỏ hàng của bạn đang trống. Vui lòng thêm sản phẩm trước khi thanh toán.";
                    lblMsg.CssClass = "alert alert-warning";
                    lblMsg.Visible = true;
                    return;
                }

                // Kiểm tra tồn kho
                foreach (DataRow row in cart.Rows)
                {
                    int soLuongTon = Convert.ToInt32(row["SoLuongTon"]);
                    int soLuong = Convert.ToInt32(row["SoLuong"]);
                    if (soLuong > soLuongTon)
                    {
                        lblMsg.Text = $"Sản phẩm {row["TenSP"]} chỉ còn {soLuongTon} sản phẩm trong kho.";
                        lblMsg.CssClass = "alert alert-danger";
                        lblMsg.Visible = true;
                        return;
                    }
                }

                // Tính tổng tiền
                decimal totalPrice = ViewState["FinalPrice"] != null
                    ? Convert.ToDecimal(ViewState["FinalPrice"])
                    : Convert.ToDecimal(ViewState["TotalPrice"]);

                // Tạo user tạm thời trong tblUSER để lưu đơn hàng
                query = @"INSERT INTO tblUSER (TenUser, UserName, Email, MaCV, NgayTaoUS) 
                         OUTPUT INSERTED.UserID 
                         VALUES (@TenUser, @UserName, @Email, 2, @NgayTaoUS)";
                parameters = new[]
                {
                    new SqlParameter("@TenUser", txtHoTen.Text.Trim()),
                    new SqlParameter("@UserName", "Guest_" + Guid.NewGuid().ToString()),
                    new SqlParameter("@Email", txtEmail.Text.Trim()),
                    new SqlParameter("@NgayTaoUS", DateTime.Now)
                };
                object newUserId = _da.ExecuteScalar(query, parameters);
                userId = Convert.ToInt32(newUserId);

                // Lưu đơn hàng vào tblDONHANG
                query = @"INSERT INTO tblDONHANG (STT_DH, MaSP, SoLuong, UserID, TinhTrangDH, MaTT, NgayTaoDH, IsCancel, TongTien) 
                         OUTPUT INSERTED.MaDH 
                         VALUES (@STT_DH, @MaSP, @SoLuong, @UserID, @TinhTrangDH, @MaTT, @NgayTaoDH, 0, @TongTien)";
                string sttDH = "DH" + DateTime.Now.ToString("yyyyMMddHHmmss");
                parameters = new[]
                {
                    new SqlParameter("@STT_DH", sttDH),
                    new SqlParameter("@MaSP", cart.Rows[0]["MaSP"]), // Lấy MaSP đầu tiên (do bảng yêu cầu, nhưng sẽ lưu chi tiết đầy đủ ở tblCHITIETDONHANG)
                    new SqlParameter("@SoLuong", cart.Rows[0]["SoLuong"]),
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@TinhTrangDH", "Chờ xử lý"),
                    new SqlParameter("@MaTT", 1), // Thanh toán khi nhận hàng
                    new SqlParameter("@NgayTaoDH", DateTime.Now),
                    new SqlParameter("@TongTien", totalPrice)
                };
                object newOrderId = _da.ExecuteScalar(query, parameters);
                int maDH = Convert.ToInt32(newOrderId);

                // Lưu chi tiết đơn hàng vào tblCHITIETDONHANG
                foreach (DataRow row in cart.Rows)
                {
                    query = @"INSERT INTO tblCHITIETDONHANG (MaDH, MaSP, Size, MauSac, SoLuong, Gia) 
                             VALUES (@MaDH, @MaSP, @Size, @MauSac, @SoLuong, @Gia)";
                    parameters = new[]
                    {
                        new SqlParameter("@MaDH", maDH),
                        new SqlParameter("@MaSP", row["MaSP"]),
                        new SqlParameter("@Size", row["Size"] != DBNull.Value ? row["Size"] : (object)DBNull.Value),
                        new SqlParameter("@MauSac", row["MauSac"] != DBNull.Value ? row["MauSac"] : (object)DBNull.Value),
                        new SqlParameter("@SoLuong", row["SoLuong"]),
                        new SqlParameter("@Gia", row["GiaGiam"])
                    };
                    _da.ExecuteNonQuery(query, parameters);

                    // Cập nhật số lượng tồn kho
                    query = @"UPDATE tblSANPHAM 
                             SET SoLuong = SoLuong - @SoLuong 
                             WHERE MaSP = @MaSP";
                    parameters = new[]
                    {
                        new SqlParameter("@SoLuong", row["SoLuong"]),
                        new SqlParameter("@MaSP", row["MaSP"])
                    };
                    _da.ExecuteNonQuery(query, parameters);
                }

                // Xóa giỏ hàng
                query = "DELETE FROM tblGIOHANG WHERE UserID = @UserID";
                parameters = new[] { new SqlParameter("@UserID", 0) };
                _da.ExecuteNonQuery(query, parameters);

                lblMsg.Text = $"Đặt hàng thành công! Mã đơn hàng của bạn là {sttDH}.";
                lblMsg.CssClass = "alert alert-success";
                lblMsg.Visible = true;
                btnCheckout.Enabled = false;
                rptCart.DataSource = null;
                rptCart.DataBind();
                lblTotalPrice.Text = "0 VNĐ";
            }
            catch (Exception ex)
            {
                lblMsg.Text = $"Lỗi khi đặt hàng: {ex.Message}";
                lblMsg.CssClass = "alert alert-danger";
                lblMsg.Visible = true;
            }
        }
    }
}