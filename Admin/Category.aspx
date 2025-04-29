<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.Admin.Category" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        window.onload = function () {
            var secs = 5;
            setTimeout(function () {
                var lblMsg = document.getElementById("<%=lblMsg.ClientID %>");
                if (lblMsg) lblMsg.style.display = "none";
            }, secs * 1000);
        }

        function ImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imagePreview.ClientID %>').prop('src', e.target.result).width(200).height(200);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function ConfirmDelete(subCategoryCount) {
            var message = "Bạn có chắc muốn xóa danh mục này?";
            if (subCategoryCount > 0) {
                message += " Danh mục này có " + subCategoryCount + " danh mục con sẽ bị xóa.";
            }
            return confirm(message);
        }
    </script>
    <script>
        $(document).ready(function () {
            $('.data-table-export').DataTable({
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mb-4">
        <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false"></asp:Label>
    </div>
    <div class="row">
        <div class="col-sm-12 col-md-4">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Thêm/Sửa danh mục cha</h4>
                    <hr />
                    <div class="form-body">
                        <label>Tên danh mục cha</label>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" placeholder="Nhập tên danh mục cha"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCategoryName" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCategoryName"
                                        ErrorMessage="Tên danh mục cha là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <label>Ảnh danh mục</label>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:FileUpload ID="fuCategoryImage" runat="server" CssClass="form-control" onchange="ImagePreview(this);" />
                                    <asp:RequiredFieldValidator ID="rfvCategoryImage" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="fuCategoryImage"
                                        ErrorMessage="Ảnh danh mục là bắt buộc" Enabled='<%# Convert.ToInt32(hfCategoryId.Value) == 0 %>'></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hfCategoryId" runat="server" Value="0" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:CheckBox ID="cbIsActive" runat="server" Text="  Hoạt động" Checked="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-action pb-5">
                        <div class="text-left">
                            <asp:Button ID="btnAddOrUpdate" runat="server" CssClass="btn btn-info" Text="Thêm" OnClick="btnAddOrUpdate_Click" />
                            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-dark" Text="Xóa form" OnClick="btnClear_Click" />
                        </div>
                    </div>
                    <div>
                        <asp:Image ID="imagePreview" runat="server" CssClass="img-thumbnail" AlternateText="Ảnh danh mục" Style="max-width: 200px; max-height: 200px;" />
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-12 col-md-8">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Danh sách danh mục cha</h4>
                    <hr />
                    <div class="table-responsive">
                        <asp:Repeater ID="rCategory" runat="server" OnItemCommand="rCategory_ItemCommand">
                            <HeaderTemplate>
                                <table class="table data-table-export table-hover nowrap">
                                    <thead>
                                        <tr>
                                            <th class="table-plus">Tên</th>
                                            <th>Ảnh</th>
                                            <th>Trạng thái</th>
                                            <th>Ngày cập nhật</th>
                                            <th class="dataTable-nosort">Hành động</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="table-plus"><%# Eval("TenHangMuc") %></td>
                                    <td>
                                        <img width="40" src="<%# QuanLyCuaHangLiderCloset.Utils.getImageUrl(Eval("HM_ImageUrl")) %>" alt="image" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblIsActive" runat="server" Text='<%# Eval("isActive") != DBNull.Value && (bool)Eval("isActive") ? "Hoạt động" : "Không hoạt động" %>'
                                            CssClass='<%# Eval("isActive") != DBNull.Value && (bool)Eval("isActive") ? "badge badge-success" : "badge badge-danger" %>'></asp:Label>
                                    </td>
                                    <td><%# Eval("NgayCapNhat", "{0:dd/MM/yyyy HH:mm}") %></td>
                                    <td>
                                        <asp:LinkButton ID="lbEdit" Text="Sửa" runat="server" CssClass="badge badge-primary" CommandArgument='<%# Eval("MaHangMuc") %>' CommandName="edit" CausesValidation="false">
                                            <i class="fas fa-edit"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbDelete" Text="Xóa" runat="server" CssClass="badge badge-danger" CommandArgument='<%# Eval("MaHangMuc") %>' CommandName="delete" CausesValidation="false"
                                            OnClientClick='<%# "return ConfirmDelete(" + (Eval("SubCategoryCount") != DBNull.Value ? Eval("SubCategoryCount").ToString() : "0") + ");" %>'>
                                            <i class="fas fa-trash-alt"></i>
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
                </div>
            </div>
        </div>
    </div>
</asp:Content>