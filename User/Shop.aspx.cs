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
    public partial class Shop : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Lấy từ khóa tìm kiếm từ query string (nếu có)
                txtSearch.Text = Request.QueryString["search"]?.ToString() ?? "";
                LoadProducts();
            }
        }

        private void LoadProducts()
        {
            try
            {
                string query = @"SELECT MaSP, TenSP, DonGia, DonGia AS GiaGiam, ImageUrl AS HinhAnh, MauSac, Size 
                                FROM tblSANPHAM sp
                                LEFT JOIN ProductImages pi ON sp.MaSP = pi.MaSP AND pi.AnhHH = 1
                                WHERE sp.isActive = 1";
                SqlParameter[] parameters = null;

                // Lọc theo danh mục (nếu có MaHangMuc trong query string)
                string maHangMuc = Request.QueryString["MaHangMuc"];
                if (!string.IsNullOrEmpty(maHangMuc))
                {
                    query += " AND sp.MaHangMuc = @MaHangMuc";
                    parameters = new[] { new SqlParameter("@MaHangMuc", maHangMuc) };
                }

                // Lọc theo từ khóa tìm kiếm
                string searchText = txtSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    query += " AND sp.TenSP LIKE '%' + @SearchText + '%'";
                    parameters = parameters == null
                        ? new[] { new SqlParameter("@SearchText", searchText) }
                        : parameters.Concat(new[] { new SqlParameter("@SearchText", searchText) }).ToArray();
                }

                // Lọc theo giá
                string priceFilter = string.Join(",", cblPrice.Items.Cast<ListItem>().Where(li => li.Selected && li.Value != "all").Select(li => li.Value));
                if (!string.IsNullOrEmpty(priceFilter))
                {
                    string[] priceRanges = priceFilter.Split(',');
                    query += " AND (";
                    for (int i = 0; i < priceRanges.Length; i++)
                    {
                        var range = priceRanges[i].Split('-');
                        query += i > 0 ? " OR " : "";
                        query += $"sp.DonGia BETWEEN {range[0]}000 AND {range[1]}000";
                    }
                    query += ")";
                }

                // Lọc theo màu sắc
                string colorFilter = string.Join(",", cblColor.Items.Cast<ListItem>().Where(li => li.Selected && li.Value != "all").Select(li => li.Value));
                if (!string.IsNullOrEmpty(colorFilter))
                {
                    query += " AND (";
                    string[] colors = colorFilter.Split(',');
                    for (int i = 0; i < colors.Length; i++)
                    {
                        query += i > 0 ? " OR " : "";
                        query += $"sp.MauSac LIKE '%" + colors[i] + "%'";
                    }
                    query += ")";
                }

                // Lọc theo kích cỡ
                string sizeFilter = string.Join(",", cblSize.Items.Cast<ListItem>().Where(li => li.Selected && li.Value != "all").Select(li => li.Value));
                if (!string.IsNullOrEmpty(sizeFilter))
                {
                    query += " AND (";
                    string[] sizes = sizeFilter.Split(',');
                    for (int i = 0; i < sizes.Length; i++)
                    {
                        query += i > 0 ? " OR " : "";
                        query += $"sp.Size LIKE '%" + sizes[i] + "%'";
                    }
                    query += ")";
                }

                // Sắp xếp
                string sort = ddlSort.SelectedValue;
                switch (sort)
                {
                    case "Latest":
                        query += " ORDER BY sp.MaSP DESC";
                        break;
                    case "Popularity":
                        query += " ORDER BY sp.DonGia ASC";
                        break;
                    case "BestRating":
                        query += " ORDER BY sp.DonGia DESC";
                        break;
                    default:
                        query += " ORDER BY sp.TenSP";
                        break;
                }

                DataTable dt = _da.ExecuteQuery(query, parameters);
                lvProducts.DataSource = dt;
                lvProducts.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi tải sản phẩm: {ex.Message}');", true);
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void cblPrice_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void cblColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void cblSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void ddlSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
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
    }
}