<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="SubCategory.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.Admin.SubCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        window.onload = function () {
            var secs = 5;
            setTimeout(function () {
                var lblMsg = document.getElementById("<%=lblMsg.ClientID %>");
                if (lblMsg) lblMsg.style.display = "none";
            }, secs * 1000);
        }

        function ConfirmDelete(productCount) {
            var message = "Bạn có chắc muốn xóa danh mục con này?";
            if (productCount > 0) {
                message += " Danh mục con này có " + productCount + " sản phẩm sẽ bị ảnh hưởng.";
            }
            return confirm(message);
        }

        // Hàm khởi tạo DataTable
        function initializeDataTable() {
            // Kiểm tra và hủy DataTable nếu đã tồn tại
            if ($.fn.DataTable.isDataTable('#subCategoryTable')) {
                $('#subCategoryTable').DataTable().destroy();
            }

            // Khởi tạo lại DataTable
            $('#subCategoryTable').DataTable({
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
    <div class="mb-4">
        <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false"></asp:Label>
    </div>
    <div class="row">
        <div class="col-sm-12 col-md-4">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Thêm/Sửa danh mục con</h4>
                    <hr />
                    <div class="form-body">
                        <label>Tên danh mục con</label>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:TextBox ID="txtSubCategoryName" runat="server" CssClass="form-control" placeholder="Nhập tên danh mục con"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSubCategoryName" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtSubCategoryName"
                                        ErrorMessage="Tên danh mục con là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <label>Danh mục cha</label>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlCategory" runat="server" AppendDataBoundItems="true" CssClass="form-control">
                                        <asp:ListItem Value="0">Chọn danh mục cha</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlCategory"
                                        ErrorMessage="Danh mục cha là bắt buộc" InitialValue="0"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hfSubCategoryId" runat="server" Value="0" />
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
                            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-dark" Text="Xóa form" OnClick="btnClear_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-12 col-md-8">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Danh sách danh mục con</h4>
                    <hr />
                    <div class="table-responsive">
                        <asp:Repeater ID="rSubCategory" runat="server" OnItemCommand="rSubCategory_ItemCommand">
                            <HeaderTemplate>
                                <table class="table table-hover nowrap" id="subCategoryTable">
                                    <thead>
                                        <tr>
                                            <th class="table-plus">Danh mục con</th>
                                            <th>Danh mục cha</th>
                                            <th>Trạng thái</th>
                                            <th>Ngày cập nhật</th>
                                            <th>Số sản phẩm</th>
                                            <th class="datatable-nosort">Hành động</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="table-plus"><%# Eval("TenHMC") %></td>
                                    <td><%# Eval("TenHangMuc") %></td>
                                    <td>
                                        <asp:Label ID="lblIsActive" runat="server" Text='<%# Eval("isActive") != DBNull.Value && (bool)Eval("isActive") ? "Hoạt động" : "Không hoạt động" %>'
                                            CssClass='<%# Eval("isActive") != DBNull.Value && (bool)Eval("isActive") ? "badge badge-success" : "badge badge-danger" %>'></asp:Label>
                                    </td>
                                    <td><%# Eval("NgayCapNhat", "{0:dd/MM/yyyy HH:mm}") %></td>
                                    <td><%# Eval("ProductCount") %></td>
                                    <td>
                                        <asp:LinkButton ID="lbEdit" Text="Sửa" runat="server" CssClass="badge badge-primary" CommandArgument='<%# Eval("MaHMC") %>' CommandName="edit" CausesValidation="false">
                                            <i class="fas fa-edit"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbDelete" Text="Xóa" runat="server" CssClass="badge badge-danger" CommandArgument='<%# Eval("MaHMC") %>' CommandName="delete" CausesValidation="false"
                                            OnClientClick='<%# "return ConfirmDelete(" + (Eval("ProductCount") != DBNull.Value ? Eval("ProductCount").ToString() : "0") + ");" %>'>
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