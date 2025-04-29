using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace QuanLyCuaHangLiderCloset.User
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            LogMessage("Page_Load called");
            if (!IsPostBack)
            {
                LogMessage("Page_Load: Not a PostBack");
                // Nếu đã đăng nhập, chuyển hướng
                if (Session["UserID"] != null)
                {
                    string maCV = Session["MaCV"]?.ToString();
                    if (!string.IsNullOrEmpty(maCV))
                    {
                        LogMessage($"User already logged in, MaCV: {maCV}");
                        RedirectUser(maCV);
                    }
                    else
                    {
                        LogMessage("Session[MaCV] is null, clearing session");
                        Session.Clear();
                    }
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LogMessage("btnLogin_Click called");
            try
            {
                if (!Page.IsValid)
                {
                    LogMessage("Page is not valid, validation failed");
                    ShowMessage("Vui lòng kiểm tra lại thông tin nhập!", false);
                    return;
                }

                string username = txtUsername.Text.Trim();
                string password = HashPassword(txtPassword.Text.Trim());
                LogMessage($"Attempting login with username: {username}");

                // Check fixed credentials
                if (username == "admin" && password == "admin123")
                {
                    // Admin login
                    Session["UserID"] = "admin";
                    Session["MaCV"] = "1"; // Admin role
                    LogMessage("Admin login successful");
                    ShowMessage("Đăng nhập thành công với tài khoản admin!", true);
                    RedirectUser("1");
                    return;
                }
                else if (username == "user" && password == "user123")
                {
                    // User login
                    Session["UserID"] = "user";
                    Session["MaCV"] = "2"; // Regular user role
                    LogMessage("User login successful");
                    ShowMessage("Đăng nhập thành công với tài khoản user!", true);
                    RedirectUser("2");
                    return;
                }

                // Fallback to database for other users
                LogMessage("Checking database for credentials");
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["cs"].ConnectionString))
                {
                    conn.Open();
                    LogMessage("Database connection successful");
                }

                string query = @"SELECT UserID, MaCV 
                                FROM tblUSER 
                                WHERE UserName = @UserName AND PassWord = @PassWord AND isActive = 1";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@UserName", username),
                    new SqlParameter("@PassWord", password)
                };

                DataTable dt = _da.ExecuteQuery(query, parameters);
                LogMessage($"Query returned {dt.Rows.Count} rows");

                if (dt.Rows.Count > 0)
                {
                    // Đăng nhập thành công
                    Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                    Session["MaCV"] = dt.Rows[0]["MaCV"].ToString();
                    LogMessage($"Session set - UserID: {Session["UserID"]}, MaCV: {Session["MaCV"]}");

                    ShowMessage("Đăng nhập thành công!", true);

                    // Chuyển hướng dựa trên vai trò
                    string maCV = Session["MaCV"].ToString();
                    RedirectUser(maCV);
                }
                else
                {
                    // Sai thông tin đăng nhập
                    ShowMessage("Tên đăng nhập hoặc mật khẩu không đúng!", false);
                    LogMessage("Login failed: Invalid username or password");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Error in btnLogin_Click: {ex.Message}\nStack Trace: {ex.StackTrace}");
                ShowMessage("Lỗi khi đăng nhập: " + ex.Message, false);
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

        private void RedirectUser(string maCV)
        {
            LogMessage($"RedirectUser called with MaCV: {maCV}");
            if (maCV == "1") // Admin
            {
                LogMessage("Redirecting to Admin/Dashboard.aspx");
                Response.Redirect("~/Admin/Dashboard.aspx");
            }
            else // Khách hàng
            {
                // Chuyển hướng đến trang trước đó hoặc trang chủ
                string returnUrl = Request.QueryString["ReturnUrl"];
                if (!string.IsNullOrEmpty(returnUrl) && UrlIsValid(returnUrl))
                {
                    LogMessage($"Redirecting to ReturnUrl: {returnUrl}");
                    Response.Redirect(returnUrl);
                }
                else
                {
                    LogMessage("Redirecting to User/Default.aspx");
                    Response.Redirect("~/User/Default.aspx");
                }
            }
        }

        private bool UrlIsValid(string url)
        {
            // Kiểm tra URL để tránh chuyển hướng không an toàn
            bool isValid = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uriResult) &&
                           (uriResult.IsAbsoluteUri ? uriResult.Host == Request.Url.Host : true);
            LogMessage($"UrlIsValid: {url} -> {isValid}");
            return isValid;
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