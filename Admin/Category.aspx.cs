using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.Admin
{
    public partial class Category : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCumbTitle"] = "Danh mục cha";
                Session["breadCumbPage"] = "Quản lý";
                lblMsg.Visible = false;
                GetCategories();
            }
        }

        void GetCategories()
        {
            try
            {
                // Sử dụng stored procedure để lấy tất cả danh mục
                SqlParameter[] parameters = { new SqlParameter("@Action", "GETALL") };
                DataTable dt = _da.ExecuteQuery("Category_Crud", parameters);

                // Thêm cột SubCategoryCount bằng truy vấn SQL trực tiếp
                if (!dt.Columns.Contains("SubCategoryCount"))
                {
                    dt.Columns.Add("SubCategoryCount", typeof(int));
                    foreach (DataRow row in dt.Rows)
                    {
                        int maHangMuc = Convert.ToInt32(row["MaHangMuc"]);
                        string subQuery = "SELECT COUNT(*) FROM tblHANGMUCCON WHERE MaHangMuc = @MaHangMuc";
                        SqlParameter[] subParams = { new SqlParameter("@MaHangMuc", maHangMuc) };
                        object subCount = _da.ExecuteScalar(subQuery, subParams);
                        row["SubCategoryCount"] = subCount != null ? Convert.ToInt32(subCount) : 0;
                    }
                }

                rCategory.DataSource = dt;
                rCategory.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh mục: " + ex.Message, false);
            }
        }

        bool IsCategoryNameExists(string tenHangMuc, int maHangMuc)
        {
            string query = "SELECT COUNT(*) FROM tblHANGMUC WHERE TenHangMuc = @TenHangMuc";
            if (maHangMuc != 0)
            {
                query += " AND MaHangMuc != @MaHangMuc";
            }
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenHangMuc", tenHangMuc),
                new SqlParameter("@MaHangMuc", maHangMuc == 0 ? (object)DBNull.Value : maHangMuc)
            };
            object result = _da.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string actionName = string.Empty, imagePath = string.Empty, fileExtension = string.Empty;
            int maHangMuc = Convert.ToInt32(hfCategoryId.Value);
            string tenHangMuc = txtCategoryName.Text.Trim();

            // Kiểm tra trùng tên danh mục
            if (IsCategoryNameExists(tenHangMuc, maHangMuc))
            {
                ShowMessage("Tên danh mục đã tồn tại!", false);
                return;
            }

            // Yêu cầu ảnh khi thêm mới
            if (maHangMuc == 0 && !fuCategoryImage.HasFile)
            {
                ShowMessage("Ảnh danh mục là bắt buộc khi thêm mới!", false);
                return;
            }

            // Chuẩn bị tham số cho stored procedure
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Action", maHangMuc == 0 ? "INSERT" : "UPDATE"),
                new SqlParameter("@MaHangMuc", maHangMuc),
                new SqlParameter("@TenHangMuc", tenHangMuc),
                new SqlParameter("@isActive", cbIsActive.Checked)
            };

            // Xử lý ảnh
            if (fuCategoryImage.HasFile)
            {
                if (Utils.isValidExetension(fuCategoryImage.FileName))
                {
                    string newImageName = Utils.getUniqueId();
                    fileExtension = Path.GetExtension(fuCategoryImage.FileName);
                    imagePath = "Images/Category/" + newImageName + fileExtension;
                    fuCategoryImage.PostedFile.SaveAs(Server.MapPath("~/Images/Category/") + newImageName + fileExtension);
                    parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Action", maHangMuc == 0 ? "INSERT" : "UPDATE"),
                        new SqlParameter("@MaHangMuc", maHangMuc),
                        new SqlParameter("@TenHangMuc", tenHangMuc),
                        new SqlParameter("@HM_ImageUrl", imagePath),
                        new SqlParameter("@isActive", cbIsActive.Checked)
                    };
                }
                else
                {
                    ShowMessage("Vui lòng chọn ảnh định dạng .jpg, .jpeg hoặc .png", false);
                    return;
                }
            }
            else if (maHangMuc > 0)
            {
                // Khi cập nhật mà không tải ảnh mới, không gửi tham số HM_ImageUrl
                parameters = new SqlParameter[]
                {
                    new SqlParameter("@Action", "UPDATE"),
                    new SqlParameter("@MaHangMuc", maHangMuc),
                    new SqlParameter("@TenHangMuc", tenHangMuc),
                    new SqlParameter("@HM_ImageUrl", DBNull.Value),
                    new SqlParameter("@isActive", cbIsActive.Checked)
                };
            }

            try
            {
                _da.ExecuteNonQuery("Category_Crud", parameters);
                actionName = maHangMuc == 0 ? "thêm" : "cập nhật";
                ShowMessage($"Đã {actionName} danh mục thành công!", true);
                GetCategories();
                Clear();
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message, false);
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtCategoryName.Text = string.Empty;
            cbIsActive.Checked = true;
            hfCategoryId.Value = "0";
            btnAddOrUpdate.Text = "Thêm";
            imagePreview.ImageUrl = string.Empty;
        }

        protected void rCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            lblMsg.Visible = false;
            if (e.CommandName == "edit")
            {
                try
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Action", "GETBYID"),
                        new SqlParameter("@MaHangMuc", e.CommandArgument)
                    };
                    DataTable dt = _da.ExecuteQuery("Category_Crud", parameters);
                    if (dt.Rows.Count > 0)
                    {
                        txtCategoryName.Text = dt.Rows[0]["TenHangMuc"].ToString();
                        cbIsActive.Checked = dt.Rows[0]["isActive"] != DBNull.Value && Convert.ToBoolean(dt.Rows[0]["isActive"]);
                        imagePreview.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["HM_ImageUrl"].ToString())
                            ? "../Images/No_Image.png"
                            : "../" + dt.Rows[0]["HM_ImageUrl"].ToString();
                        imagePreview.Height = 200;
                        imagePreview.Width = 200;
                        hfCategoryId.Value = dt.Rows[0]["MaHangMuc"].ToString();
                        btnAddOrUpdate.Text = "Cập nhật";
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Lỗi khi tải danh mục: " + ex.Message, false);
                }
            }
            else if (e.CommandName == "delete")
            {
                try
                {
                    // Kiểm tra số lượng danh mục con
                    string subQuery = "SELECT COUNT(*) FROM tblHANGMUCCON WHERE MaHangMuc = @MaHangMuc";
                    SqlParameter[] subParams = { new SqlParameter("@MaHangMuc", e.CommandArgument) };
                    object subCount = _da.ExecuteScalar(subQuery, subParams);
                    int subCategoryCount = subCount != null ? Convert.ToInt32(subCount) : 0;

                    // Xóa danh mục cha và các danh mục con liên quan
                    SqlParameter[] deleteParams = new SqlParameter[]
                    {
                        new SqlParameter("@Action", "DELETE"),
                        new SqlParameter("@MaHangMuc", e.CommandArgument)
                    };
                    _da.ExecuteNonQuery("Category_Crud", deleteParams);

                    ShowMessage("Xóa danh mục thành công!" + (subCategoryCount > 0 ? $" ({subCategoryCount} danh mục con cũng đã bị xóa)" : ""), true);
                    GetCategories();
                    Clear();
                }
                catch (Exception ex)
                {
                    ShowMessage("Lỗi khi xóa danh mục: " + ex.Message, false);
                }
            }
        }

        void ShowMessage(string message, bool isSuccess)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMsg.Visible = true;
        }
    }
}