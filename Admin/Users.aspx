<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.Admin.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        window.onload = function () {
            var secs = 5;
            setTimeout(function () {
                var lblMsg = document.getElementById("<%=lblMsg.ClientID %>");
                if (lblMsg) lblMsg.style.display = "none";
            }, secs * 1000);
        }

        // Hàm khởi tạo DataTable
        function initializeDataTable() {
            // Kiểm tra và hủy DataTable nếu đã tồn tại
            if ($.fn.DataTable.isDataTable('#usersTable')) {
                $('#usersTable').DataTable().destroy();
            }

            // Khởi tạo lại DataTable
            $('#usersTable').DataTable({
                "paging": true,
                "searching": true,
                "ordering": true,
                "responsive": true,
                "dom": 'Bfrtip',
                "buttons": ['copy', 'csv', 'excel', 'pdf', 'print'],
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.11.5/i18n/Vietnamese.json"
                }
            });
        }

        // Gọi hàm khởi tạo DataTable khi trang được tải
        $(document).ready(function () {
            initializeDataTable();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h2 class="mb-4">Danh sách người dùng</h2>
        <div class="mb-4">
            <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert" />
        </div>
        <asp:Repeater ID="rUsers" runat="server">
            <HeaderTemplate>
                <table class="table table-bordered table-hover" id="usersTable">
                    <thead class="thead-dark">
                        <tr>
                            <th>STT</th>
                            <th>ID</th>
                            <th>Tên người dùng</th>
                            <th>Họ tên</th>
                            <th>Email</th>
                            <th>Ngày tạo</th>
                            <th>Vai trò</th>
                            <th>Trạng thái</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("SrNo") %></td>
                    <td><%# Eval("UserID") %></td>
                    <td><%# Eval("UserName") %></td>
                    <td><%# Eval("TenUser") %></td>
                    <td><%# Eval("Email") %></td>
                    <td><%# Eval("NgayTaoUS", "{0:dd/MM/yyyy HH:mm}") %></td>
                    <td><%# Eval("TenCV") %></td>
                    <td>
                        <span class='<%# Eval("isActive") != DBNull.Value && Convert.ToBoolean(Eval("isActive")) ? "badge badge-success" : "badge badge-danger" %>'>
                            <%# Eval("isActive") != DBNull.Value && Convert.ToBoolean(Eval("isActive")) ? "Hoạt động" : "Không hoạt động" %>
                        </span>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>
</asp:Content>