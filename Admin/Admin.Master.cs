using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.Admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {

        private readonly DataAccess _da = new DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kiểm tra quyền truy cập
                //if (Session["UserID"] == null || Session["MaCV"] == null || Session["MaCV"].ToString() != "1")
                //{
                //    Response.Redirect("~/User/Login.aspx?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery));
                //}

                // Hiển thị thông tin admin
                LoadAdminInfo();
            }
        }

        private void LoadAdminInfo()
        {
            try
            {
                //string userID = Session["UserID"].ToString();
                //string query = "SELECT TenUser, ImageURL FROM tblUSER WHERE UserID = @UserID";
                //SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };
                //DataTable dt = _da.ExecuteQuery(query, parameters);

                //if (dt.Rows.Count > 0)
                //{
                //    lblAdminName.Text = dt.Rows[0]["TenUser"].ToString();
                //    imgAdmin.ImageUrl = string.IsNullOrEmpty(dt.Rows[0]["ImageURL"].ToString())
                //        ? "~/AdminTemplate/assets/images/users/profile-pic.jpg"
                //        : dt.Rows[0]["ImageURL"].ToString();
                //}
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi trong debug
                Response.Write($"Lỗi khi tải thông tin admin: {ex.Message}");
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            //Response.Redirect("~/User/Login.aspx");
        }
    }
}
