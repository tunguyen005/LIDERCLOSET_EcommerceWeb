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
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCategories();
            }
        }

        private readonly DataAccess da = new DataAccess();

        private void LoadCategories()
        {
            try
            {
                // Kiểm tra cache
                DataTable dt = Cache["Categories"] as DataTable;
                if (dt == null)
                {
                    string query = @"SELECT hm.MaHangMuc, hm.TenHangMuc, hm.HM_ImageUrl, 
                                COUNT(sp.MaSP) AS ProductCount 
                                FROM tblHANGMUC hm 
                                LEFT JOIN tblSANPHAM sp ON hm.MaHangMuc = sp.MaHangMuc 
                                WHERE hm.isActive = 1 
                                GROUP BY hm.MaHangMuc, hm.TenHangMuc, hm.HM_ImageUrl";
                    dt = da.ExecuteQuery(query);

                    // Lưu vào cache với thời gian hết hạn 10 phút
                    Cache.Insert("Categories", dt, null, DateTime.Now.AddMinutes(10), System.Web.Caching.Cache.NoSlidingExpiration);
                }

                rptCategories.DataSource = dt;
                rptCategories.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi tải danh mục: {ex.Message}');", true);
            }
        }

        protected void btnSubscribe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEmail.Text))
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Vui lòng nhập email!');", true);
                return;
            }

            string query = @"INSERT INTO tblLIENHE (TenLH, EmailLH, TieuDe, LoiNhan, NgayTaoLH) 
                        VALUES (@TenLH, @EmailLH, @TieuDe, @LoiNhan, @NgayTaoLH)";
            SqlParameter[] parameters = {
            new SqlParameter("@TenLH", "Subscriber"),
            new SqlParameter("@EmailLH", txtEmail.Text.Trim()),
            new SqlParameter("@TieuDe", "Đăng ký nhận thông báo"),
            new SqlParameter("@LoiNhan", "Yêu cầu nhận thông báo qua email"),
            new SqlParameter("@NgayTaoLH", DateTime.Now)
        };

            try
            {
                int rowsAffected = da.ExecuteNonQuery(query, parameters);
                if (rowsAffected > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Đăng ký thành công!');", true);
                    txtEmail.Text = "";
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Không thể lưu thông tin. Vui lòng thử lại!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi đăng ký: {ex.Message}');", true);
            }
        }

        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string maHangMuc = btn.CommandArgument;

            try
            {
                // Lấy sản phẩm đầu tiên trong danh mục để thêm vào giỏ (giả định)
                string query = @"SELECT TOP 1 MaSP FROM tblSANPHAM WHERE MaHangMuc = @MaHangMuc";
                SqlParameter[] parameters = { new SqlParameter("@MaHangMuc", maHangMuc) };
                DataTable dt = da.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    string maSP = dt.Rows[0]["MaSP"].ToString();

                    // Lưu vào giỏ hàng (giả sử dùng Session)
                    var cart = Session["Cart"] as DataTable ?? CreateCartTable();
                    DataRow existingItem = cart.AsEnumerable().FirstOrDefault(row => row["MaSP"].ToString() == maSP);

                    if (existingItem != null)
                    {
                        existingItem["SoLuong"] = int.Parse(existingItem["SoLuong"].ToString()) + 1;
                    }
                    else
                    {
                        DataRow newItem = cart.NewRow();
                        newItem["MaSP"] = maSP;
                        newItem["SoLuong"] = 1;
                        cart.Rows.Add(newItem);
                    }

                    Session["Cart"] = cart;
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Đã thêm sản phẩm vào giỏ hàng!');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Không tìm thấy sản phẩm trong danh mục này!');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('Lỗi khi thêm vào giỏ: {ex.Message}');", true);
            }
        }

        private DataTable CreateCartTable()
        {
            DataTable cart = new DataTable();
            cart.Columns.Add("MaSP", typeof(string));
            cart.Columns.Add("SoLuong", typeof(int));
            return cart;
        }
    }
}
