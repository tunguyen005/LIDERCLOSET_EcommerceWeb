<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Checkout.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.Checkout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .cart-item img { width: 100px; height: 100px; object-fit: cover; }
        .cart-item { margin-bottom: 20px; }
        .total-price { font-weight: bold; font-size: 1.2em; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h2 class="mb-4">Thanh toán</h2>
        
        <!-- Thông báo -->
        <asp:Label ID="lblMsg" runat="server" Visible="false" CssClass="alert" />

        <!-- Giỏ hàng -->
        <div class="row mb-4">
            <div class="col-md-8">
                <h4>Giỏ hàng của bạn</h4>
                <asp:Repeater ID="rptCart" runat="server">
                    <ItemTemplate>
                        <div class="cart-item d-flex align-items-center border-bottom py-3">
                            <asp:Image ID="imgProduct" runat="server" ImageUrl='<%# Eval("HinhAnh") %>' CssClass="mr-3" />
                            <div class="flex-grow-1">
                                <h5><%# Eval("TenSP") %></h5>
                                <p>
                                    Kích cỡ: <%# Eval("Size") != null ? Eval("Size") : "Không chọn" %><br />
                                    Màu sắc: <%# Eval("MauSac") != null ? Eval("MauSac") : "Không chọn" %><br />
                                    Số lượng: <%# Eval("SoLuong") %><br />
                                    Giá: <%# string.Format("{0:N0}", Eval("GiaGiam")) %> VNĐ
                                </p>
                            </div>
                        </div>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblEmptyCart" runat="server" Visible='<%# rptCart.Items.Count == 0 %>' Text="Giỏ hàng của bạn đang trống." CssClass="text-muted" />
                    </FooterTemplate>
                </asp:Repeater>
                <div class="total-price mt-3">
                    Tổng cộng: <asp:Label ID="lblTotalPrice" runat="server" Text="0 VNĐ" />
                </div>
                <div class="coupon-section mt-3">
                    <label for="txtCoupon">Mã giảm giá</label>
                    <asp:TextBox ID="txtCoupon" runat="server" CssClass="form-control d-inline-block" style="width: 200px;" />
                    <asp:Button ID="btnApplyCoupon" runat="server" CssClass="btn btn-secondary ml-2" Text="Áp dụng" OnClick="btnApplyCoupon_Click" />
                </div>
            </div>
        </div>

        <!-- Form thông tin khách hàng -->
        <div class="row">
            <div class="col-md-6">
                <h4>Thông tin khách hàng</h4>
                <div class="form-group">
                    <label for="txtHoTen">Họ tên (*)</label>
                    <asp:TextBox ID="txtHoTen" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvHoTen" runat="server" ControlToValidate="txtHoTen"
                        ErrorMessage="Vui lòng nhập họ tên" ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="form-group">
                    <label for="txtEmail">Email (*)</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="Vui lòng nhập email" ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="form-group">
                    <label for="txtSoDienThoai">Số điện thoại (*)</label>
                    <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="form-control" TextMode="Phone" />
                    <asp:RequiredFieldValidator ID="rfvSoDienThoai" runat="server" ControlToValidate="txtSoDienThoai"
                        ErrorMessage="Vui lòng nhập số điện thoại" ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="form-group">
                    <label for="txtDiaChi">Địa chỉ (*)</label>
                    <asp:TextBox ID="txtDiaChi" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    <asp:RequiredFieldValidator ID="rfvDiaChi" runat="server" ControlToValidate="txtDiaChi"
                        ErrorMessage="Vui lòng nhập địa chỉ" ForeColor="Red" Display="Dynamic" />
                </div>
                <asp:Button ID="btnCheckout" runat="server" CssClass="btn btn-primary" Text="Đặt hàng" OnClick="btnCheckout_Click" />
            </div>
        </div>
    </div>
</asp:Content>