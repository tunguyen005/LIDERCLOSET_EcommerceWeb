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
    public partial class ShopDetail : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string maSP = Request.QueryString["MaSP"];
                if (string.IsNullOrEmpty(maSP))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Không tìm thấy sản phẩm.'); window.location='Shop.aspx';", true);
                    return;
                }

                LoadProductDetails(maSP);
                LoadReviews(maSP);
                LoadRelatedProducts(maSP);
            }
        }

        private void LoadProductDetails(string maSP)
        {
            try
            {
                string query = @"SELECT sp.MaSP, sp.TenSP, sp.DonGia, sp.DonGia AS GiaGiam, pi.ImageUrl AS HinhAnh, 
                                sp.MoTaDai AS MoTa, sp.MoTaBoSung, sp.Size, sp.MauSac, sp.MaHangMuc, sp.SoLuong 
                                FROM tblSANPHAM sp
                                LEFT JOIN ProductImages pi ON sp.MaSP = pi.MaSP AND pi.AnhHH = 1
                                WHERE sp.MaSP = @MaSP AND sp.isActive = 1";
                SqlParameter[] parameters = { new SqlParameter("@MaSP", maSP) };
                DataTable dt = _da.ExecuteQuery(query, parameters);

                if (dt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Sản phẩm không tồn tại hoặc không còn hoạt động.'); window.location='Shop.aspx';", true);
                    return;
                }

                DataRow row = dt.Rows[0];
                lblTenSP.Text = row["TenSP"].ToString();
                lblTenSPHeader.Text = row["TenSP"].ToString();
                lblGiaGiam.Text = string.Format("{0:N0} VNĐ", row["DonGia"]);
                lblGia.Text = string.Format("{0:N0} VNĐ", row["DonGia"]);
                lblMoTa.Text = row["MoTa"].ToString();
                lblMoTaFull.Text = row["MoTa"].ToString();
                lblSoLuongTon.Text = row["SoLuong"].ToString();
                lblSoLuongTonTab.Text = row["SoLuong"].ToString();
                lblTenSPReview.Text = row["TenSP"].ToString();

                // Hiển thị hình ảnh
                DataTable dtImages = new DataTable();
                dtImages.Columns.Add("HinhAnh", typeof(string));
                dtImages.Columns.Add("TenSP", typeof(string));
                string imageUrl = row["HinhAnh"] != DBNull.Value ? row["HinhAnh"].ToString() : "/Images/no-image.jpg";
                dtImages.Rows.Add(imageUrl, row["TenSP"]);
                rptImages.DataSource = dtImages;
                rptImages.DataBind();
                rptThumbnails.DataSource = dtImages;
                rptThumbnails.DataBind();

                // Hiển thị màu sắc và kích cỡ
                string[] colors = row["MauSac"].ToString().Split(',');
                rblColor.Items.Clear();
                foreach (string color in colors)
                {
                    if (!string.IsNullOrEmpty(color.Trim()))
                    {
                        rblColor.Items.Add(new ListItem(color.Trim(), color.Trim()));
                    }
                }
                if (rblColor.Items.Count > 0) rblColor.Items[0].Selected = true;

                string[] sizes = row["Size"].ToString().Split(',');
                rblSize.Items.Clear();
                foreach (string size in sizes)
                {
                    if (!string.IsNullOrEmpty(size.Trim()))
                    {
                        rblSize.Items.Add(new ListItem(size.Trim(), size.Trim()));
                    }
                }
                if (rblSize.Items.Count > 0) rblSize.Items[0].Selected = true;

                // Hiển thị đánh giá trung bình
                query = "SELECT AVG(Sao) AS AvgRating, COUNT(*) AS ReviewCount FROM tblREVIEWSP WHERE MaSP = @MaSP";
                dt = _da.ExecuteQuery(query, parameters);
                int avgRating = dt.Rows[0]["AvgRating"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["AvgRating"]) : 0;
                int reviewCount = Convert.ToInt32(dt.Rows[0]["ReviewCount"]);
                litRating.Text = GetRatingStars(avgRating);
                lblReviewCount.Text = reviewCount.ToString();
                lblReviewCountTab.Text = reviewCount.ToString();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi tải chi tiết sản phẩm: {ex.Message}');", true);
            }
        }

        private void LoadReviews(string maSP)
        {
            try
            {
                string query = @"SELECT u.TenUser, u.Email, r.Sao AS DiemDanhGia, r.BinhLuan AS NoiDung, r.NgayTaoRV AS NgayDanhGia 
                                FROM tblREVIEWSP r
                                JOIN tblUSER u ON r.UserID = u.UserID
                                WHERE r.MaSP = @MaSP 
                                ORDER BY r.NgayTaoRV DESC";
                SqlParameter[] parameters = { new SqlParameter("@MaSP", maSP) };
                DataTable dt = _da.ExecuteQuery(query, parameters);
                rptReviews.DataSource = dt;
                rptReviews.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi tải đánh giá: {ex.Message}');", true);
            }
        }

        private void LoadRelatedProducts(string maSP)
        {
            try
            {
                string query = @"SELECT TOP 4 sp.MaSP, sp.TenSP, sp.DonGia, sp.DonGia AS GiaGiam, pi.ImageUrl AS HinhAnh 
                                FROM tblSANPHAM sp
                                LEFT JOIN ProductImages pi ON sp.MaSP = pi.MaSP AND pi.AnhHH = 1
                                WHERE sp.MaHangMuc = (SELECT MaHangMuc FROM tblSANPHAM WHERE MaSP = @MaSP) 
                                AND sp.MaSP != @MaSP AND sp.isActive = 1";
                SqlParameter[] parameters = { new SqlParameter("@MaSP", maSP) };
                DataTable dt = _da.ExecuteQuery(query, parameters);
                lvRelatedProducts.DataSource = dt;
                lvRelatedProducts.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi tải sản phẩm liên quan: {ex.Message}');", true);
            }
        }

        protected string GetRatingStars(int rating)
        {
            string stars = "";
            for (int i = 1; i <= 5; i++)
            {
                if (i <= rating)
                    stars += "<i class=\"fas fa-star\"></i>";
                else if (i - rating < 1)
                    stars += "<i class=\"fas fa-star-half-alt\"></i>";
                else
                    stars += "<i class=\"far fa-star\"></i>";
            }
            return stars;
        }

        protected void btnMinus_Click(object sender, EventArgs e)
        {
            int quantity = int.Parse(txtQuantity.Text);
            if (quantity > 1)
            {
                txtQuantity.Text = (quantity - 1).ToString();
            }
        }

        protected void btnPlus_Click(object sender, EventArgs e)
        {
            int quantity = int.Parse(txtQuantity.Text);
            string maSP = Request.QueryString["MaSP"];
            int soLuongTon = GetSoLuongTon(maSP);
            if (quantity < soLuongTon)
            {
                txtQuantity.Text = (quantity + 1).ToString();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Số lượng vượt quá tồn kho!');", true);
            }
        }

        private int GetSoLuongTon(string maSP)
        {
            try
            {
                string query = "SELECT SoLuong FROM tblSANPHAM WHERE MaSP = @MaSP";
                SqlParameter[] parameters = { new SqlParameter("@MaSP", maSP) };
                DataTable dt = _da.ExecuteQuery(query, parameters);
                return dt.Rows.Count > 0 ? Convert.ToInt32(dt.Rows[0]["SoLuong"]) : 0;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi kiểm tra tồn kho: {ex.Message}');", true);
                return 0;
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (rblSize.SelectedItem == null || rblColor.SelectedItem == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Vui lòng chọn kích cỡ và màu sắc!');", true);
                return;
            }

            int maSP = int.Parse(Request.QueryString["MaSP"]);
            int quantity = int.Parse(txtQuantity.Text);
            string size = rblSize.SelectedValue;
            string color = rblColor.SelectedValue;

            try
            {
                // Kiểm tra sản phẩm
                string query = @"SELECT MaSP, SoLuong 
                                FROM tblSANPHAM 
                                WHERE MaSP = @MaSP AND isActive = 1";
                SqlParameter[] parameters = { new SqlParameter("@MaSP", maSP) };
                DataTable productDt = _da.ExecuteQuery(query, parameters);

                if (productDt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Sản phẩm không tồn tại hoặc không còn hoạt động!');", true);
                    return;
                }

                int soLuongTon = Convert.ToInt32(productDt.Rows[0]["SoLuong"]);
                if (quantity > soLuongTon)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Số lượng vượt quá tồn kho!');", true);
                    return;
                }

                int userId = 0; // Không yêu cầu đăng nhập, để userId = 0 (khách vãng lai)

                // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa (dựa trên MaSP, Size, MauSac)
                query = @"SELECT MaGioHang, SoLuong 
                         FROM tblGIOHANG 
                         WHERE MaSP = @MaSP AND UserID = @UserID AND Size = @Size AND MauSac = @MauSac";
                parameters = new[]
                {
                    new SqlParameter("@MaSP", maSP),
                    new SqlParameter("@UserID", userId),
                    new SqlParameter("@Size", size),
                    new SqlParameter("@MauSac", color)
                };
                DataTable cartDt = _da.ExecuteQuery(query, parameters);

                if (cartDt.Rows.Count > 0)
                {
                    // Cập nhật số lượng
                    int currentQuantity = Convert.ToInt32(cartDt.Rows[0]["SoLuong"]);
                    if (currentQuantity + quantity > soLuongTon)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Số lượng trong giỏ vượt quá tồn kho!');", true);
                        return;
                    }

                    query = @"UPDATE tblGIOHANG 
                             SET SoLuong = @SoLuong, NgayTaoGioHang = @NgayTaoGioHang 
                             WHERE MaGioHang = @MaGioHang";
                    parameters = new[]
                    {
                        new SqlParameter("@SoLuong", currentQuantity + quantity),
                        new SqlParameter("@NgayTaoGioHang", DateTime.Now),
                        new SqlParameter("@MaGioHang", cartDt.Rows[0]["MaGioHang"])
                    };
                    _da.ExecuteNonQuery(query, parameters);
                }
                else
                {
                    // Thêm mới vào giỏ hàng
                    query = @"INSERT INTO tblGIOHANG (MaSP, SoLuong, UserID, NgayTaoGioHang, Size, MauSac) 
                             VALUES (@MaSP, @SoLuong, @UserID, @NgayTaoGioHang, @Size, @MauSac)";
                    parameters = new[]
                    {
                        new SqlParameter("@MaSP", maSP),
                        new SqlParameter("@SoLuong", quantity),
                        new SqlParameter("@UserID", userId),
                        new SqlParameter("@NgayTaoGioHang", DateTime.Now),
                        new SqlParameter("@Size", size),
                        new SqlParameter("@MauSac", color)
                    };
                    _da.ExecuteNonQuery(query, parameters);
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Đã thêm sản phẩm vào giỏ hàng!');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi thêm vào giỏ: {ex.Message}');", true);
            }
        }

        protected void btnAddToCartRelated_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int maSP = int.Parse(btn.CommandArgument);

            try
            {
                // Kiểm tra sản phẩm
                string query = @"SELECT MaSP, SoLuong 
                                FROM tblSANPHAM 
                                WHERE MaSP = @MaSP AND isActive = 1";
                SqlParameter[] parameters = { new SqlParameter("@MaSP", maSP) };
                DataTable productDt = _da.ExecuteQuery(query, parameters);

                if (productDt.Rows.Count == 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Sản phẩm không tồn tại hoặc không còn hoạt động!');", true);
                    return;
                }

                int soLuongTon = Convert.ToInt32(productDt.Rows[0]["SoLuong"]);
                int userId = 0; // Không yêu cầu đăng nhập, để userId = 0 (khách vãng lai)

                // Kiểm tra xem sản phẩm đã có trong giỏ hàng chưa (dựa trên MaSP, Size, MauSac)
                query = @"SELECT MaGioHang, SoLuong 
                         FROM tblGIOHANG 
                         WHERE MaSP = @MaSP AND UserID = @UserID AND Size IS NULL AND MauSac IS NULL";
                parameters = new[]
                {
                    new SqlParameter("@MaSP", maSP),
                    new SqlParameter("@UserID", userId)
                };
                DataTable cartDt = _da.ExecuteQuery(query, parameters);

                if (cartDt.Rows.Count > 0)
                {
                    // Cập nhật số lượng
                    int currentQuantity = Convert.ToInt32(cartDt.Rows[0]["SoLuong"]);
                    if (currentQuantity + 1 > soLuongTon)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Số lượng trong giỏ vượt quá tồn kho!');", true);
                        return;
                    }

                    query = @"UPDATE tblGIOHANG 
                             SET SoLuong = @SoLuong, NgayTaoGioHang = @NgayTaoGioHang 
                             WHERE MaGioHang = @MaGioHang";
                    parameters = new[]
                    {
                        new SqlParameter("@SoLuong", currentQuantity + 1),
                        new SqlParameter("@NgayTaoGioHang", DateTime.Now),
                        new SqlParameter("@MaGioHang", cartDt.Rows[0]["MaGioHang"])
                    };
                    _da.ExecuteNonQuery(query, parameters);
                }
                else
                {
                    if (1 > soLuongTon)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Sản phẩm đã hết hàng!');", true);
                        return;
                    }

                    // Thêm mới vào giỏ hàng
                    query = @"INSERT INTO tblGIOHANG (MaSP, SoLuong, UserID, NgayTaoGioHang, Size, MauSac) 
                             VALUES (@MaSP, @SoLuong, @UserID, @NgayTaoGioHang, NULL, NULL)";
                    parameters = new[]
                    {
                        new SqlParameter("@MaSP", maSP),
                        new SqlParameter("@SoLuong", 1),
                        new SqlParameter("@UserID", userId),
                        new SqlParameter("@NgayTaoGioHang", DateTime.Now)
                    };
                    _da.ExecuteNonQuery(query, parameters);
                }

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Đã thêm sản phẩm vào giỏ hàng!');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi thêm vào giỏ: {ex.Message}');", true);
            }
        }

        protected void btnSubmitReview_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int maSP = int.Parse(Request.QueryString["MaSP"]);
                    int userId = 0; // Không yêu cầu đăng nhập, để userId = 0 (khách vãng lai)

                    // Tạo một user tạm thời trong tblUSER để lưu đánh giá
                    string query = @"INSERT INTO tblUSER (TenUser, UserName, Email, MaCV, NgayTaoUS) 
                                    OUTPUT INSERTED.UserID 
                                    VALUES (@TenUser, @UserName, @Email, 2, @NgayTaoUS)";
                    SqlParameter[] parameters = new[]
                    {
                        new SqlParameter("@TenUser", txtName.Text.Trim()),
                        new SqlParameter("@UserName", "Guest_" + Guid.NewGuid().ToString()),
                        new SqlParameter("@Email", txtEmail.Text.Trim()),
                        new SqlParameter("@NgayTaoUS", DateTime.Now)
                    };
                    object newUserId = _da.ExecuteScalar(query, parameters);
                    userId = Convert.ToInt32(newUserId);

                    // Lưu đánh giá
                    query = @"INSERT INTO tblREVIEWSP (Sao, BinhLuan, MaSP, UserID, NgayTaoRV) 
                             VALUES (@Sao, @BinhLuan, @MaSP, @UserID, @NgayTaoRV)";
                    parameters = new[]
                    {
                        new SqlParameter("@Sao", int.Parse(ddlRating.SelectedValue)),
                        new SqlParameter("@BinhLuan", txtReview.Text.Trim()),
                        new SqlParameter("@MaSP", maSP),
                        new SqlParameter("@UserID", userId),
                        new SqlParameter("@NgayTaoRV", DateTime.Now)
                    };

                    int rowsAffected = _da.ExecuteNonQuery(query, parameters);
                    if (rowsAffected > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Đánh giá đã được gửi thành công!');", true);
                        txtName.Text = "";
                        txtEmail.Text = "";
                        txtReview.Text = "";
                        ddlRating.SelectedValue = "5";
                        // Chuyển maSP thành string trước khi gọi
                        string maSPString = maSP.ToString();
                        LoadReviews(maSPString);
                        LoadProductDetails(maSPString);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Không thể lưu đánh giá. Vui lòng thử lại!');", true);
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi gửi đánh giá: {ex.Message}');", true);
                }
            }
        }
    }
}