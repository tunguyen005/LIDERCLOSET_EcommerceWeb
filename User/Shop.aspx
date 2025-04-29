<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Shop.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.Shop" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .product-item { margin-bottom: 20px; text-align: center; }
        .product-item img { width: 100%; height: 200px; object-fit: cover; }
        .product-item h5 { font-size: 16px; margin: 10px 0; }
        .product-price { color: #dc3545; font-weight: bold; }
        .product-price-old { color: #999; text-decoration: line-through; }
        .filter-section { margin-bottom: 20px; }
        .filter-section h6 { margin-top: 10px; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <!-- Thanh tìm kiếm -->
        <div class="row mb-4">
            <div class="col-md-6">
                <div class="input-group">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Tìm kiếm sản phẩm..."></asp:TextBox>
                    <div class="input-group-append">
                        <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Tìm kiếm" OnClick="btnSearch_Click" />
                    </div>
                </div>
            </div>
            <div class="col-md-6 text-right">
                <asp:DropDownList ID="ddlSort" runat="server" CssClass="form-control w-auto d-inline" AutoPostBack="true" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                    <asp:ListItem Value="Default">Sắp xếp mặc định</asp:ListItem>
                    <asp:ListItem Value="Latest">Mới nhất</asp:ListItem>
                    <asp:ListItem Value="Popularity">Phổ biến</asp:ListItem>
                    <asp:ListItem Value="BestRating">Đánh giá tốt nhất</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <!-- Bộ lọc -->
        <div class="row filter-section">
            <div class="col-md-3">
                <h6>Lọc theo giá</h6>
                <asp:CheckBoxList ID="cblPrice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblPrice_SelectedIndexChanged">
                    <asp:ListItem Value="all" Selected="True">Tất cả</asp:ListItem>
                    <asp:ListItem Value="0-100">0 - 100,000 VNĐ</asp:ListItem>
                    <asp:ListItem Value="100-300">100,000 - 300,000 VNĐ</asp:ListItem>
                    <asp:ListItem Value="300-500">300,000 - 500,000 VNĐ</asp:ListItem>
                    <asp:ListItem Value="500-1000">500,000 - 1,000,000 VNĐ</asp:ListItem>
                </asp:CheckBoxList>
            </div>
            <div class="col-md-3">
                <h6>Lọc theo màu sắc</h6>
                <asp:CheckBoxList ID="cblColor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblColor_SelectedIndexChanged">
                    <asp:ListItem Value="all" Selected="True">Tất cả</asp:ListItem>
                    <asp:ListItem Value="Đỏ">Đỏ</asp:ListItem>
                    <asp:ListItem Value="Xanh">Xanh</asp:ListItem>
                    <asp:ListItem Value="Đen">Đen</asp:ListItem>
                    <asp:ListItem Value="Trắng">Trắng</asp:ListItem>
                </asp:CheckBoxList>
            </div>
            <div class="col-md-3">
                <h6>Lọc theo kích cỡ</h6>
                <asp:CheckBoxList ID="cblSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblSize_SelectedIndexChanged">
                    <asp:ListItem Value="all" Selected="True">Tất cả</asp:ListItem>
                    <asp:ListItem Value="S">S</asp:ListItem>
                    <asp:ListItem Value="M">M</asp:ListItem>
                    <asp:ListItem Value="L">L</asp:ListItem>
                    <asp:ListItem Value="XL">XL</asp:ListItem>
                </asp:CheckBoxList>
            </div>
        </div>

        <!-- Danh sách sản phẩm -->
        <div class="row">
            <asp:ListView ID="lvProducts" runat="server" ItemPlaceholderID="itemPlaceholder">
                <LayoutTemplate>
                    <div class="row">
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="col-md-3 product-item">
                        <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("HinhAnh") %>' AlternateText='<%# Eval("TenSP") %>' />
                        <h5><%# Eval("TenSP") %></h5>
                        <p>
                            <span class="product-price"><%# string.Format("{0:N0}", Eval("GiaGiam")) %> VNĐ</span><br />
                            <span class="product-price-old"><%# string.Format("{0:N0}", Eval("Gia")) %> VNĐ</span>
                        </p>
                        <p>
                            Màu: <%# Eval("MauSac") %><br />
                            Kích cỡ: <%# Eval("KichCo") %>
                        </p>
                        <asp:Button ID="btnAddToCart" runat="server" CssClass="btn btn-primary" Text="Thêm vào giỏ hàng" 
                            CommandArgument='<%# Eval("MaSP") %>' OnClick="btnAddToCart_Click" />
                    </div>
                </ItemTemplate>
                <EmptyDataTemplate>
                    <div class="col-12 text-center">
                        <p>Không tìm thấy sản phẩm nào.</p>
                    </div>
                </EmptyDataTemplate>
            </asp:ListView>
        </div>
    </div>
</asp:Content>