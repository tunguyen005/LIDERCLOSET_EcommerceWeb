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
    public partial class Contact : System.Web.UI.Page
    {
        private readonly DataAccess _da = new DataAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["breadCumbTitle"] = "Liên hệ";
                Session["breadCumbPage"] = "Quản lý";
                lblMsg.Visible = false;
                LoadContactMessages();
            }
        }

        void LoadContactMessages()
        {
            try
            {
                string query = "SELECT MaLienHe, Ten, Email, ChuDe, TinNhan, NgayGui FROM tblLIENHE ORDER BY NgayGui DESC";
                DataTable dt = _da.ExecuteQuery(query);
                rContact.DataSource = dt;
                rContact.DataBind();
            }
            catch (Exception ex)
            {
                ShowMessage("Lỗi khi tải tin nhắn: " + ex.Message, false);
            }
        }

        protected void rContact_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                try
                {
                    string query = "DELETE FROM tblLIENHE WHERE MaLienHe = @MaLienHe";
                    SqlParameter[] parameters = { new SqlParameter("@MaLienHe", e.CommandArgument) };
                    _da.ExecuteNonQuery(query, parameters);
                    ShowMessage("Xóa tin nhắn thành công!", true);
                    LoadContactMessages();
                }
                catch (Exception ex)
                {
                    ShowMessage("Lỗi khi xóa tin nhắn: " + ex.Message, false);
                }
            }
        }

        protected void btnSendReply_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    int maLienHe = Convert.ToInt32(hfContactId.Value);
                    string noiDungPhanHoi = txtReplyContent.Text.Trim();

                    string query = "INSERT INTO tblPHANHOI (MaLienHe, NoiDungPhanHoi, NgayPhanHoi) VALUES (@MaLienHe, @NoiDungPhanHoi, GETDATE())";
                    SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@MaLienHe", maLienHe),
                        new SqlParameter("@NoiDungPhanHoi", noiDungPhanHoi)
                    };
                    _da.ExecuteNonQuery(query, parameters);

                    ShowMessage("Trả lời tin nhắn thành công!", true);
                    LoadContactMessages();
                }
                catch (Exception ex)
                {
                    ShowMessage("Lỗi khi trả lời tin nhắn: " + ex.Message, false);
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