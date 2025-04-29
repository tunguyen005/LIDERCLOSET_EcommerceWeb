using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.Admin
{
    public partial class SubCategory : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCumbTitle"] = "Danh mục con";
                Session["breadCumbPage"] = "Quản lý";
                lblMsg.Visible = false;
                GetCategories();
                GetSubCategories();
                if (Request.QueryString["id"] != null)
                {
                    GetSubCategoryDetails();
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

        void GetSubCategories()
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Action", "GETALL") };
                DataTable dt = _da.ExecuteQuery("SubCategory_Crud", parameters);
                rSubCategory.DataSource = dt;
                rSubCategory.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh sách danh mục con: " + ex.Message, false);
            }
        }

        void GetSubCategoryDetails()
        {
            try
            {
                string maHMC = Request.QueryString["id"];
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Action", "GETBYID"),
                    new SqlParameter("@MaHMC", maHMC)
                };
                DataTable dt = _da.ExecuteQuery("SubCategory_Crud", parameters);
                if (dt.Rows.Count > 0)
                {
                    txtSubCategoryName.Text = dt.Rows[0]["TenHMC"].ToString();
                    ddlCategory.SelectedValue = dt.Rows[0]["MaHangMuc"].ToString();
                    cbIsActive.Checked = dt.Rows[0]["isActive"] != DBNull.Value && Convert.ToBoolean(dt.Rows[0]["isActive"]);
                    hfSubCategoryId.Value = maHMC;
                    btnAddOrUpdate.Text = "Cập nhật";
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải chi tiết danh mục con: " + ex.Message, false);
            }
        }

        protected void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                string maHMC = hfSubCategoryId.Value;
                string tenHMC = txtSubCategoryName.Text.Trim();
                string maHangMuc = ddlCategory.SelectedValue;
                bool isActive = cbIsActive.Checked;

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Action", maHMC == "0" ? "INSERT" : "UPDATE"),
                    new SqlParameter("@MaHMC", maHMC == "0" ? (object)DBNull.Value : maHMC),
                    new SqlParameter("@TenHMC", tenHMC),
                    new SqlParameter("@MaHangMuc", maHangMuc),
                    new SqlParameter("@isActive", isActive)
                };

                _da.ExecuteNonQuery("SubCategory_Crud", parameters);
                string action = maHMC == "0" ? "thêm" : "cập nhật";
                ShowMessage($"Đã {action} danh mục con thành công!", true);
                Clear();
                GetSubCategories();
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
            txtSubCategoryName.Text = string.Empty;
            ddlCategory.SelectedIndex = 0;
            cbIsActive.Checked = true;
            hfSubCategoryId.Value = "0";
            btnAddOrUpdate.Text = "Thêm";
        }

        protected void rSubCategory_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "edit")
                {
                    Response.Redirect("SubCategory.aspx?id=" + e.CommandArgument);
                }
                else if (e.CommandName == "delete")
                {
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@Action", "DELETE"),
                        new SqlParameter("@MaHMC", e.CommandArgument)
                    };
                    _da.ExecuteNonQuery("SubCategory_Crud", parameters);
                    ShowMessage("Xóa danh mục con thành công!", true);
                    GetSubCategories();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi: " + ex.Message, false);
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