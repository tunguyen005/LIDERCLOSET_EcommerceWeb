using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.Admin
{
    public partial class Users : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCrum"] = "Users";
                Session["breadCumbTitle"] = "Người dùng";
                Session["breadCumbPage"] = "Danh sách";
                lblMsg.Visible = false;
                GetUsers();
            }
        }

        void GetUsers()
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@Action",
                        Value = "SELECT4ADMIN",
                        SqlDbType = SqlDbType.VarChar
                    }
                };
                DataTable dt = _da.ExecuteQuery("User_Crud", parameters);
                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowMessage("Không có người dùng nào để hiển thị.", false);
                }
                else
                {
                    rUsers.DataSource = dt;
                    rUsers.DataBind();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải danh sách người dùng: " + ex.Message, false);
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