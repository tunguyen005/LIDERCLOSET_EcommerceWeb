using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset.User
{
    public partial class Contact : System.Web.UI.Page
    {

        private readonly DataAccess _da = new DataAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Tải thông tin người dùng nếu đã đăng nhập
                if (Session["UserID"] != null)
                {
                    LoadUserInfo();
                }
            }
        }

        private void LoadUserInfo()
        {
            try
            {
                string userID = Session["UserID"].ToString();
                string query = "SELECT TenUser, Email FROM tblUSER WHERE UserID = @UserID";
                SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };
                var dt = _da.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["TenUser"].ToString();
                    txtEmail.Text = dt.Rows[0]["Email"].ToString();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải thông tin người dùng: " + ex.Message, false);
            }
        }

        protected void btnSendMessage_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                string name = txtName.Text.Trim();
                string email = txtEmail.Text.Trim();
                string subject = txtSubject.Text.Trim();
                string message = txtMessage.Text.Trim();

                // Lưu vào tblLIENHE
                string query = @"INSERT INTO tblLIENHE (TenLH, EmailLH, TieuDe, LoiNhan, NgayTaoLH)
                            VALUES (@TenLH, @EmailLH, @TieuDe, @LoiNhan, @NgayTaoLH)";
                SqlParameter[] parameters = new SqlParameter[]
                {
                new SqlParameter("@TenLH", name),
                new SqlParameter("@EmailLH", email),
                new SqlParameter("@TieuDe", subject),
                new SqlParameter("@LoiNhan", message),
                new SqlParameter("@NgayTaoLH", DateTime.Now)
                };

                _da.ExecuteNonQuery(query, parameters);

                // Xóa form và hiển thị thông báo thành công
                txtName.Text = "";
                txtEmail.Text = "";
                txtSubject.Text = "";
                txtMessage.Text = "";
                ShowMessage("Tin nhắn của bạn đã được gửi thành công! Chúng tôi sẽ phản hồi sớm nhất có thể.", true);
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi gửi tin nhắn: " + ex.Message, false);
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }
    }
}
