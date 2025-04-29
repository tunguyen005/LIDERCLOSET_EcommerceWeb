using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyCuaHangLiderCloset.Admin
{
    public partial class Product : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();
        private List<string> _existingImages = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCumbTitle"] = "Sản phẩm";
                Session["breadCumbPage"] = "Thêm/Sửa";
                lblMsg.Visible = false;
                rfvFirstImage.Enabled = Request.QueryString["id"] == null;
                GetCategories();

                // Nếu đang thêm sản phẩm mới, tải danh mục con cho danh mục cha mặc định (nếu có)
                if (Request.QueryString["id"] == null && ddlCategory.Items.Count > 1)
                {
                    int defaultCategoryId = Convert.ToInt32(ddlCategory.Items[1].Value); // Chọn danh mục cha đầu tiên sau "Chọn danh mục cha"
                    ddlCategory.SelectedIndex = 1;
                    GetSubCategories(defaultCategoryId);
                }

                // Nếu đang chỉnh sửa sản phẩm, tải chi tiết sản phẩm
                if (Request.QueryString["id"] != null)
                {
                    GetProductDetails();
                }
            }
        }

        void GetCategories()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Action", "ACTIVECATEGORY") };
                DataTable dt = _da.ExecuteQuery("Category_Crud", parameters);
                ddlCategory.DataSource = dt;
                ddlCategory.DataTextField = "TenHangMuc";
                ddlCategory.DataValueField = "MaHangMuc";
                ddlCategory.DataBind();
                ddlCategory.Items.Insert(0, new ListItem("Chọn danh mục cha", "0"));
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh mục cha: " + ex.Message, false);
            }
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int categoryId = Convert.ToInt32(ddlCategory.SelectedValue);
            if (categoryId > 0)
            {
                GetSubCategories(categoryId);
            }
            else
            {
                ddlSubCategory.Items.Clear();
                ddlSubCategory.Items.Insert(0, new ListItem("Chọn danh mục con", "0"));
            }
        }

        void GetSubCategories(int categoryId)
        {
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@Action", "SUBCATEGORYBYID"),
                    new SqlParameter("@MaHangMuc", categoryId)
                };
                DataTable dt = _da.ExecuteQuery("SubCategory_Crud", parameters);
                ddlSubCategory.DataSource = dt;
                ddlSubCategory.DataTextField = "TenHMC";
                ddlSubCategory.DataValueField = "MaHMC";
                ddlSubCategory.DataBind();
                ddlSubCategory.Items.Insert(0, new ListItem("Chọn danh mục con", "0"));
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh mục con: " + ex.Message, false);
                ddlSubCategory.Items.Clear();
                ddlSubCategory.Items.Insert(0, new ListItem("Chọn danh mục con", "0"));
            }
        }

        void GetProductDetails()
        {
            try
            {
                int productId = Convert.ToInt32(Request.QueryString["id"]);
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Action", "GETBYID"),
                    new SqlParameter("@MaSP", productId)
                };
                DataTable dt = _da.ExecuteQuery("Product_Crud", parameters);
                if (dt.Rows.Count > 0)
                {
                    txtProductName.Text = dt.Rows[0]["TenSP"].ToString();
                    txtPrice.Text = dt.Rows[0]["DonGia"].ToString();
                    txtQuantity.Text = dt.Rows[0]["SoLuong"].ToString();
                    txtShortDescription.Text = dt.Rows[0]["MoTaNgan"].ToString();
                    txtLongDescription.Text = dt.Rows[0]["MoTaDai"].ToString();
                    txtAdditionalDescription.Text = dt.Rows[0]["MoTaBoSung"].ToString();
                    txtCompanyName.Text = dt.Rows[0]["TenNCC"].ToString();

                    // Kiểm tra và chọn danh mục cha
                    string maHangMuc = dt.Rows[0]["MaHangMuc"].ToString();
                    if (ddlCategory.Items.FindByValue(maHangMuc) != null)
                    {
                        ddlCategory.SelectedValue = maHangMuc;
                        GetSubCategories(Convert.ToInt32(maHangMuc));
                    }
                    else
                    {
                        ShowMessage("Danh mục cha của sản phẩm không còn tồn tại hoặc không hoạt động!", false);
                        ddlSubCategory.Items.Clear();
                        ddlSubCategory.Items.Insert(0, new ListItem("Chọn danh mục con", "0"));
                    }

                    // Kiểm tra và chọn danh mục con
                    string maHMC = dt.Rows[0]["MaHMC"].ToString();
                    if (ddlSubCategory.Items.FindByValue(maHMC) != null)
                    {
                        ddlSubCategory.SelectedValue = maHMC;
                    }
                    else
                    {
                        ShowMessage("Danh mục con của sản phẩm không còn tồn tại hoặc không hoạt động!", false);
                        ddlSubCategory.SelectedIndex = 0; // Chọn giá trị mặc định
                    }

                    cbIsCustomized.Checked = dt.Rows[0]["isCustomized"] != DBNull.Value && Convert.ToBoolean(dt.Rows[0]["isCustomized"]);
                    cbIsActive.Checked = dt.Rows[0]["isActive"] != DBNull.Value && Convert.ToBoolean(dt.Rows[0]["isActive"]);

                    string[] colors = dt.Rows[0]["MauSac"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] sizes = dt.Rows[0]["Size"].ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string color in colors)
                    {
                        ListItem item = lboxColor.Items.FindByValue(color.Trim());
                        if (item != null) item.Selected = true;
                    }
                    foreach (string size in sizes)
                    {
                        ListItem item = lboxSize.Items.FindByValue(size.Trim());
                        if (item != null) item.Selected = true;
                    }

                    if (!string.IsNullOrEmpty(dt.Rows[0]["Image1"].ToString()))
                    {
                        string[] images = dt.Rows[0]["Image1"].ToString().Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < images.Length && i < 4; i++)
                        {
                            string[] imageData = images[i].Split(new[] { ": " }, StringSplitOptions.None);
                            string imageUrl = imageData[0];
                            bool isDefault = imageData[1] == "1";
                            _existingImages.Add(imageUrl);
                            switch (i)
                            {
                                case 0:
                                    imageProduct1.ImageUrl = "../" + imageUrl;
                                    imageProduct1.Style.Remove("display");
                                    break;
                                case 1:
                                    imageProduct2.ImageUrl = "../" + imageUrl;
                                    imageProduct2.Style.Remove("display");
                                    break;
                                case 2:
                                    imageProduct3.ImageUrl = "../" + imageUrl;
                                    imageProduct3.Style.Remove("display");
                                    break;
                                case 3:
                                    imageProduct4.ImageUrl = "../" + imageUrl;
                                    imageProduct4.Style.Remove("display");
                                    break;
                            }
                            if (isDefault)
                            {
                                rblDefaultImage.SelectedValue = (i + 1).ToString();
                                hfDefImagePos.Value = (i + 1).ToString();
                            }
                        }
                    }

                    hfProductId.Value = productId.ToString();
                    btnAddOrUpdate.Text = "Cập nhật";
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải chi tiết sản phẩm: " + ex.Message, false);
            }
        }

        bool CheckProductNameExists(string tenSP, int maHMC, int maSP)
        {
            string query = "SELECT COUNT(*) FROM tblSANPHAM WHERE TenSP = @TenSP AND MaHMC = @MaHMC";
            if (maSP != 0)
            {
                query += " AND MaSP != @MaSP";
            }
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenSP", tenSP),
                new SqlParameter("@MaHMC", maHMC),
                new SqlParameter("@MaSP", maSP == 0 ? (object)DBNull.Value : maSP)
            };
            object result = _da.ExecuteScalar(query, parameters);
            return result != null && Convert.ToInt32(result) > 0;
        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                int productId = Convert.ToInt32(hfProductId.Value);
                string tenSP = HttpUtility.HtmlEncode(txtProductName.Text.Trim());
                int maHMC = Convert.ToInt32(ddlSubCategory.SelectedValue);
                int maHangMuc = Convert.ToInt32(ddlCategory.SelectedValue);

                // Kiểm tra trùng tên sản phẩm
                if (CheckProductNameExists(tenSP, maHMC, productId))
                {
                    ShowMessage("Tên sản phẩm đã tồn tại trong danh mục con này!", false);
                    return;
                }

                // Kiểm tra màu sắc và kích cỡ
                string selectedColor = string.Join(",", lboxColor.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value));
                string selectedSize = string.Join(",", lboxSize.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value));
                if (string.IsNullOrEmpty(selectedColor) || string.IsNullOrEmpty(selectedSize))
                {
                    ShowMessage("Vui lòng chọn ít nhất một màu sắc và kích cỡ!", false);
                    return;
                }
                if (selectedColor.Length > 30 || selectedSize.Length > 30)
                {
                    ShowMessage("Màu sắc hoặc kích cỡ vượt quá 30 ký tự!", false);
                    return;
                }

                // Xử lý ảnh
                List<string> newImagePaths = new List<string>();
                List<(string ImageUrl, bool IsDefault)> images = new List<(string, bool)>();
                bool isValid = true;

                if (productId == 0 && !fuFirstImage.HasFile)
                {
                    ShowMessage("Vui lòng tải lên ít nhất 1 ảnh sản phẩm!", false);
                    return;
                }

                FileUpload[] fileUploads = { fuFirstImage, fuSecondImage, fuThirdImage, fuFourthImage };
                for (int i = 0; i < fileUploads.Length; i++)
                {
                    if (fileUploads[i].HasFile)
                    {
                        string fileName = fileUploads[i].FileName;
                        if (!Utils.isValidExetension(fileName))
                        {
                            ShowMessage("Vui lòng chọn file .jpg, .jpeg, .png!", false);
                            isValid = false;
                            break;
                        }
                        string imagePath = $"Images/Product/{Utils.getUniqueId()}_{fileName}";
                        fileUploads[i].PostedFile.SaveAs(Server.MapPath("~/" + imagePath));
                        images.Add((imagePath, (i + 1) == Convert.ToInt32(rblDefaultImage.SelectedValue)));
                        newImagePaths.Add(imagePath);
                    }
                    else if (productId > 0 && _existingImages.Count > i)
                    {
                        images.Add((_existingImages[i], (i + 1) == Convert.ToInt32(rblDefaultImage.SelectedValue)));
                    }
                }

                if (!isValid)
                {
                    DeleteFiles(newImagePaths);
                    return;
                }

                // Kiểm tra ảnh mặc định
                int defaultImagePos = Convert.ToInt32(rblDefaultImage.SelectedValue);
                if (defaultImagePos > images.Count)
                {
                    ShowMessage("Ảnh mặc định không hợp lệ!", false);
                    DeleteFiles(newImagePaths);
                    return;
                }

                // Thêm hoặc cập nhật sản phẩm
                SqlParameter[] productParams = new SqlParameter[]
                {
                    new SqlParameter("@Action", productId == 0 ? "INSERT" : "UPDATE"),
                    new SqlParameter("@MaSP", productId),
                    new SqlParameter("@TenSP", tenSP),
                    new SqlParameter("@MoTaNgan", txtShortDescription.Text.Trim()),
                    new SqlParameter("@MoTaDai", txtLongDescription.Text.Trim()),
                    new SqlParameter("@MoTaBoSung", txtAdditionalDescription.Text.Trim()),
                    new SqlParameter("@DonGia", Convert.ToDecimal(txtPrice.Text.Trim())),
                    new SqlParameter("@SoLuong", Convert.ToInt32(txtQuantity.Text.Trim())),
                    new SqlParameter("@Size", selectedSize),
                    new SqlParameter("@MauSac", selectedColor),
                    new SqlParameter("@TenNCC", txtCompanyName.Text.Trim()),
                    new SqlParameter("@MaHangMuc", maHangMuc),
                    new SqlParameter("@MaHMC", maHMC),
                    new SqlParameter("@isCustomized", cbIsCustomized.Checked),
                    new SqlParameter("@isActive", cbIsActive.Checked)
                };

                if (productId == 0)
                {
                    // Thêm sản phẩm mới
                    _da.ExecuteNonQuery("Product_Crud", productParams);
                    SqlParameter[] recentParams = { new SqlParameter("@Action", "RECENT_PRODUCT") };
                    DataTable recentDt = _da.ExecuteQuery("Product_Crud", recentParams);
                    productId = Convert.ToInt32(recentDt.Rows[0]["MaSP"]);
                }
                else
                {
                    // Cập nhật sản phẩm
                    _da.ExecuteNonQuery("Product_Crud", productParams);

                    // Xóa ảnh cũ
                    SqlParameter[] deleteImgParams = new SqlParameter[]
                    {
                        new SqlParameter("@Action", "DELETE_PROD_IMG"),
                        new SqlParameter("@MaSP", productId)
                    };
                    _da.ExecuteNonQuery("Product_Crud", deleteImgParams);

                    // Xóa ảnh cũ khỏi hệ thống tệp nếu không còn sử dụng
                    foreach (string oldImage in _existingImages)
                    {
                        if (!images.Any(img => img.ImageUrl == oldImage) && File.Exists(Server.MapPath("~/" + oldImage)))
                        {
                            File.Delete(Server.MapPath("~/" + oldImage));
                        }
                    }
                }

                // Thêm ảnh mới
                foreach (var image in images)
                {
                    SqlParameter[] imgParams = new SqlParameter[]
                    {
                        new SqlParameter("@Action", "INSERT_PROD_IMG"),
                        new SqlParameter("@MaSP", productId),
                        new SqlParameter("@ImageUrl", image.ImageUrl),
                        new SqlParameter("@AnhHH", image.IsDefault)
                    };
                    _da.ExecuteNonQuery("Product_Crud", imgParams);
                }

                string action = productId == 0 ? "thêm" : "cập nhật";
                ShowMessage($"Đã {action} sản phẩm thành công!", true);
                Response.AddHeader("REFRESH", "2;URL=ProductList.aspx");
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message, false);
            }
        }

        void DeleteFiles(List<string> filePaths)
        {
            foreach (string path in filePaths)
            {
                if (File.Exists(Server.MapPath("~/" + path)))
                {
                    File.Delete(Server.MapPath("~/" + path));
                }
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtProductName.Text = string.Empty;
            txtShortDescription.Text = string.Empty;
            txtLongDescription.Text = string.Empty;
            txtAdditionalDescription.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            lboxColor.ClearSelection();
            lboxSize.ClearSelection();
            ddlCategory.ClearSelection();
            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Insert(0, new ListItem("Chọn danh mục con", "0"));
            rblDefaultImage.ClearSelection();
            cbIsActive.Checked = true;
            cbIsCustomized.Checked = false;
            hfProductId.Value = "0";
            hfDefImagePos.Value = "0";
            imageProduct1.ImageUrl = null;
            imageProduct2.ImageUrl = null;
            imageProduct3.ImageUrl = null;
            imageProduct4.ImageUrl = null;
            imageProduct1.Style["display"] = "none";
            imageProduct2.Style["display"] = "none";
            imageProduct3.Style["display"] = "none";
            imageProduct4.Style["display"] = "none";
            btnAddOrUpdate.Text = "Thêm";
            _existingImages.Clear();
            rfvFirstImage.Enabled = true;
        }

        void ShowMessage(string message, bool isSuccess)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMsg.Visible = true;
        }
    }
}