using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace QuanLyCuaHangLiderCloset.User
{
    public partial class Registration : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            LogMessage("Page_Load called");
            if (!IsPostBack)
            {
                LogMessage("Page_Load: Not a PostBack");
                // Kiểm tra nếu đã đăng nhập, chuyển hướng
                if (Session["UserID"] != null)
                {
                    LogMessage("User already logged in, redirecting to Default.aspx");
                    Response.Redirect("~/User/Default.aspx");
                }
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            LogMessage("btnRegister_Click called");
            try
            {
                if (!Page.IsValid)
                {
                    LogMessage("Page is not valid, validation failed");
                    ShowMessage("Vui lòng kiểm tra lại thông tin nhập!", false);
                    return;
                }

                string name = txtName.Text.Trim();
                string username = txtUsername.Text.Trim();
                string email = txtEmail.Text.Trim();
                string mobile = txtMobile.Text.Trim();
                string address = txtAddress.Text.Trim();
                string postCode = txtPostCode.Text.Trim();
                string password = HashPassword(txtPassword.Text.Trim());
                string imageUrl = null;

                LogMessage($"Registering user: Name={name}, Username={username}, Email={email}");

                try
                {
                    string checkQuery = @"SELECT COUNT(*) FROM tblUSER WHERE UserName = @UserName OR Email = @Email";
                    SqlParameter[] checkParams = new SqlParameter[]
                    {
                new SqlParameter("@UserName", username),
                new SqlParameter("@Email", email)
                    };
                    object result = _da.ExecuteScalar(checkQuery, checkParams);
                    LogMessage($"Check uniqueness result: {result}");
                    if (Convert.ToInt32(result) > 0)
                    {
                        ShowMessage("Tên đăng nhập hoặc email đã tồn tại!", false);
                        LogMessage("Registration failed: Username or email already exists");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Error when checking uniqueness: {ex.Message}\n{ex.StackTrace}");
                    ShowMessage("Lỗi kiểm tra tên đăng nhập/email!", false);
                    return;
                }

                try
                {
                    if (fuUserImage.HasFile)
                    {
                        string fileExtension = Path.GetExtension(fuUserImage.FileName).ToLower();
                        LogMessage($"File uploaded, extension: {fileExtension}");

                        if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png")
                        {
                            string fileName = Guid.NewGuid().ToString() + fileExtension;
                            string savePath = Server.MapPath("~/Images/Users/") + fileName;
                            string relativePath = "~/Images/Users/" + fileName;

                            Directory.CreateDirectory(Server.MapPath("~/Images/Users/"));
                            fuUserImage.SaveAs(savePath);
                            imageUrl = relativePath;

                            LogMessage($"Image saved: {relativePath}");
                        }
                        else
                        {
                            ShowMessage("Chỉ chấp nhận file JPG hoặc PNG!", false);
                            LogMessage("Invalid file format");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Error uploading image: {ex.Message}\n{ex.StackTrace}");
                    ShowMessage("Lỗi khi tải ảnh đại diện!", false);
                    return;
                }

                try
                {
                    string query = @"INSERT INTO tblUSER (TenUser, UserName, Mobile, Email, DiaChiUser, PostCode, PassWord, ImageURL, MaCV, NgayTaoUS)
                             VALUES (@TenUser, @UserName, @Mobile, @Email, @DiaChiUser, @PostCode, @PassWord, @ImageURL, @MaCV, @NgayTaoUS)";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                new SqlParameter("@TenUser", name),
                new SqlParameter("@UserName", username),
                new SqlParameter("@Mobile", mobile),
                new SqlParameter("@Email", email),
                new SqlParameter("@DiaChiUser", address),
                new SqlParameter("@PostCode", postCode),
                new SqlParameter("@PassWord", password),
                new SqlParameter("@ImageURL", (object)imageUrl ?? DBNull.Value),
                new SqlParameter("@MaCV", 2),
                new SqlParameter("@NgayTaoUS", DateTime.Now)
                    };

                    int rowsAffected = _da.ExecuteNonQuery(query, parameters);
                    LogMessage($"Rows affected: {rowsAffected}");

                    if (rowsAffected > 0)
                    {
                        ShowMessage("Đăng ký thành công! Vui lòng đăng nhập để tiếp tục.", true);
                        LogMessage("Registration successful");
                        Response.AddHeader("REFRESH", "5;URL=Login.aspx");
                    }
                    else
                    {
                        ShowMessage("Đăng ký thất bại! Vui lòng thử lại.", false);
                        LogMessage("Registration failed: No rows affected");
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"Error inserting into database: {ex.Message}\n{ex.StackTrace}");
                    ShowMessage("Lỗi khi lưu thông tin người dùng!", false);
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Unhandled error in btnRegister_Click: {ex.Message}\n{ex.StackTrace}");
                ShowMessage("Lỗi không xác định khi đăng ký!", false);
            }
        }


        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMsg.Text = message;
            lblMsg.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMsg.Visible = true;
        }

        private void LogMessage(string message)
        {
            try
            {
                string logPath = Server.MapPath("~/error.log");
                File.AppendAllText(logPath, $"{DateTime.Now}: {message}\n");
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi ghi log: " + ex.Message, false);
            }
        }
    }
}