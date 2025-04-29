<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.Admin.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h2 class="mb-4">Quản lý liên hệ</h2>
        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert" />
        <asp:Repeater ID="rContact" runat="server" OnItemCommand="rContact_ItemCommand">
            <HeaderTemplate>
                <table class="table table-bordered table-hover" id="contactTable">
                    <thead class="thead-dark">
                        <tr>
                            <th>ID</th>
                            <th>Tên</th>
                            <th>Email</th>
                            <th>Chủ đề</th>
                            <th>Tin nhắn</th>
                            <th>Ngày gửi</th>
                            <th>Hành động</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("MaLienHe") %></td>
                    <td><%# Eval("Ten") %></td>
                    <td><%# Eval("Email") %></td>
                    <td><%# Eval("ChuDe") %></td>
                    <td><%# Eval("TinNhan") %></td>
                    <td><%# Eval("NgayGui", "{0:dd/MM/yyyy HH:mm}") %></td>
                    <td>
                        <button type="button" class="btn btn-info btn-sm" data-toggle="modal" data-target="#replyModal" 
                                onclick="showReplyModal('<%# Eval("MaLienHe") %>', '<%# Eval("Email") %>', '<%# Eval("ChuDe") %>')">
                            Trả lời
                        </button>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="delete" CommandArgument='<%# Eval("MaLienHe") %>' CssClass="btn btn-danger btn-sm" OnClientClick="return confirm('Bạn có chắc muốn xóa tin nhắn này?');">
                            Xóa
                        </asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </div>

    <!-- Modal trả lời -->
    <div class="modal fade" id="replyModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Trả lời tin nhắn</h5>
                    <button type="button" class="close" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <label>Chủ đề</label>
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" ReadOnly="true" />
                    </div>
                    <div class="form-group">
                        <label>Nội dung trả lời</label>
                        <asp:TextBox ID="txtReplyContent" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
                        <asp:RequiredFieldValidator ID="rfvReplyContent" runat="server" ControlToValidate="txtReplyContent" 
                            ErrorMessage="Vui lòng nhập nội dung trả lời!" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <asp:HiddenField ID="hfContactId" runat="server" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Đóng</button>
                    <asp:Button ID="btnSendReply" runat="server" Text="Gửi" CssClass="btn btn-primary" OnClick="btnSendReply_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Script DataTable -->
    <script type="text/javascript">
        $(document).ready(function () {
            if ($.fn.DataTable.isDataTable('#contactTable')) {
                $('#contactTable').DataTable().destroy();
            }

            $('#contactTable').DataTable({
                "paging": true,
                "searching": true,
                "ordering": true,
                "info": true,
                "lengthMenu": [5, 10, 25, 50],
                "pageLength": 10,
                "language": {
                    "url": "//cdn.datatables.net/plug-ins/1.11.5/i18n/Vietnamese.json"
                }
            });
        });

        function showReplyModal(contactId, email, subject) {
            document.getElementById('<%= hfContactId.ClientID %>').value = contactId;
            document.getElementById('<%= txtEmail.ClientID %>').value = email;
            document.getElementById('<%= txtSubject.ClientID %>').value = subject;
            document.getElementById('<%= txtReplyContent.ClientID %>').value = '';
        }
    </script>

    <!-- Thêm Bootstrap JS cho modal -->
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</asp:Content>