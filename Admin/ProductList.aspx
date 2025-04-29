<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.Admin.ProductList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h2 class="mb-4">Danh sách sản phẩm</h2>
        <div class="card mb-4">
            <div class="card-header">Lọc sản phẩm</div>
            <div class="card-body">
                <div class="row">
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Danh mục</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                <asp:ListItem Text="Tất cả" Value="0" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>Trạng thái</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                <asp:ListItem Text="Tất cả" Value="2" />
                                <asp:ListItem Text="Hoạt động" Value="1" />
                                <asp:ListItem Text="Không hoạt động" Value="0" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label>&nbsp;</label><br />
                            <asp:Button ID="btnExport" runat="server" Text="Xuất CSV" CssClass="btn btn-success" OnClick="btnExport_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:Repeater ID="rProductList" runat="server">
            <HeaderTemplate>
                <table class="table table-bordered table-hover" id="productTable">
                    <thead class="thead-dark">
                        <tr>
                            <th>ID</th>
                            <th>Tên sản phẩm</th>
                            <th>Hình ảnh</th>
                            <th>Giá</th>
                            <th>Danh mục</th>
                            <th>Danh mục con</th>
                            <th>Trạng thái</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("MaSP") %></td>
                    <td><%# Eval("TenSP") %></td>
                    <td>
                        <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("ImageUrl") != DBNull.Value ? "../" + Eval("ImageUrl") : "../Images/No_Image.png" %>' Width="100px" Height="100px" />
                    </td>
                    <td><%# Eval("DonGia", "{0:N0}") %> VNĐ</td>
                    <td><%# Eval("TenHangMuc") != DBNull.Value ? Eval("TenHangMuc") : "Không có" %></td>
                    <td><%# Eval("TenHMC") != DBNull.Value ? Eval("TenHMC") : "Không có" %></td>
                    <td>
                        <span class="badge <%# Convert.ToBoolean(Eval("isActive")) ? "badge-success" : "badge-danger" %>">
                            <%# Convert.ToBoolean(Eval("isActive")) ? "Hoạt động" : "Không hoạt động" %>
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

    <script type="text/javascript">
        $(document).ready(function () {
            if ($.fn.DataTable.isDataTable('#productTable')) {
                $('#productTable').DataTable().destroy();
            }

            $('#productTable').DataTable({
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
    </script>
</asp:Content>