<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Cart.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.Cart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap" rel="stylesheet">
    <style>
        :root {
            --bg-color: #ffffff;
            --text-color: #1a1a1a;
            --primary-color: #d4af37;
            --secondary-color: #4a4a4a;
        }
        [data-theme="dark"] {
            --bg-color: #121212;
            --text-color: #e0e0e0;
            --primary-color: #d4af37;
            --secondary-color: #b0b0b0;
        }
        body {
            background-color: var(--bg-color);
            color: var(--text-color);
            font-family: 'Roboto', sans-serif;
        }
        .container-fluid {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
        }
        .table-responsive {
            overflow-x: auto;
        }
        .table {
            background-color: var(--bg-color);
        }
        .table th, .table td {
            vertical-align: middle;
        }
        .btn-primary {
            background-color: var(--primary-color);
            color: var(--bg-color);
            border: none;
            transition: opacity 0.2s, transform 0.2s;
        }
        .btn-primary:hover {
            opacity: 0.9;
            transform: scale(1.05);
        }
        .btn-danger {
            background-color: #dc3545;
            color: #fff;
            border: none;
        }
        .quantity .form-control {
            width: 60px;
            background-color: var(--secondary-color);
            color: var(--bg-color);
        }
        .card {
            background-color: var(--bg-color);
            border: 1px solid var(--secondary-color);
        }
        .card-header {
            background-color: var(--secondary-color);
            color: var(--bg-color);
        }
        .input-group .form-control {
            border: 1px solid var(--secondary-color);
        }
        .empty-cart {
            text-align: center;
            padding: 50px 0;
        }
        @media (max-width: 768px) {
            .table {
                font-size: 14px;
            }
            .quantity .form-control {
                width: 50px;
            }
            .btn-sm {
                padding: 5px 10px;
            }
        }
    </style>
    <script>
        function validateCoupon() {
            var coupon = document.getElementById('<%= txtCoupon.ClientID %>').value;
            if (!coupon.trim()) {
                alert('Vui lòng nhập mã giảm giá!');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page Header Start -->
    <div class="container-fluid bg-secondary mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center" style="min-height: 300px">
            <h1 class="font-weight-semi-bold text-uppercase mb-3">Giỏ hàng</h1>
            <div class="d-inline-flex">
                <p class="m-0"><a href="Default.aspx">Trang chủ</a></p>
                <p class="m-0 px-2">-</p>
                <p class="m-0"><a href="Shop.aspx">Cửa hàng</a></p>
                <p class="m-0 px-2">-</p>
                <p class="m-0">Giỏ hàng</p>
            </div>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Cart Start -->
    <div class="container-fluid pt-5">
        <div class="row px-xl-5">
            <div class="col-lg-8 table-responsive mb-5">
                <asp:Panel ID="pnlCart" runat="server" Visible="false">
                    <table class="table table-bordered text-center mb-0">
                        <thead class="bg-secondary text-dark">
                            <tr>
                                <th>Sản phẩm</th>
                                <th>Giá</th>
                                <th>Số lượng</th>
                                <th>Tổng</th>
                                <th>Xóa</th>
                            </tr>
                        </thead>
                        <tbody class="align-middle">
                            <asp:Repeater ID="rptCart" runat="server" OnItemCommand="rptCart_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td class="align-middle text-left">
                                            <img src='<%# Eval("HinhAnh") %>' alt='<%# Eval("TenSP") %>' style="width: 50px; float: left; margin-right: 10px;">
                                            <a href='ShopDetail.aspx?MaSP=<%# Eval("MaSP") %>'><%# Eval("TenSP") %></a>
                                            <br />
                                            <small>Kích cỡ: <%# Eval("KichCo") %></small>
                                            <br />
                                            <small>Màu sắc: <%# Eval("MauSac") %></small>
                                        </td>
                                        <td class="align-middle"><%# Eval("GiaGiam", "{0:N0} VNĐ") %></td>
                                        <td class="align-middle">
                                            <div class="input-group quantity mx-auto" style="width: 120px;">
                                                <div class="input-group-btn">
                                                    <asp:Button ID="btnMinus" runat="server" CssClass="btn btn-sm btn-primary btn-minus" CommandName="Decrease" CommandArgument='<%# Eval("CartItemId") %>' Text="-" />
                                                </div>
                                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control form-control-sm bg-secondary text-center" Text='<%# Eval("SoLuong") %>' ReadOnly="true" />
                                                <div class="input-group-btn">
                                                    <asp:Button ID="btnPlus" runat="server" CssClass="btn btn-sm btn-primary btn-plus" CommandName="Increase" CommandArgument='<%# Eval("CartItemId") %>' Text="+" />
                                                </div>
                                            </div>
                                        </td>
                                        <td class="align-middle"><%# Eval("Total", "{0:N0} VNĐ") %></td>
                                        <td class="align-middle">
                                            <asp:Button ID="btnRemove" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Remove" CommandArgument='<%# Eval("CartItemId") %>' Text="×" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pnlEmptyCart" runat="server" CssClass="empty-cart">
                    <h4>Giỏ hàng của bạn đang trống!</h4>
                    <p><a href="Shop.aspx" class="btn btn-primary">Mua sắm ngay</a></p>
                </asp:Panel>
            </div>
            <div class="col-lg-4">
                <asp:Panel ID="pnlCoupon" runat="server" CssClass="mb-5">
                    <div class="input-group">
                        <asp:TextBox ID="txtCoupon" runat="server" CssClass="form-control p-4" placeholder="Mã giảm giá" />
                        <div class="input-group-append">
                            <asp:Button ID="btnApplyCoupon" runat="server" CssClass="btn btn-primary" Text="Áp dụng" OnClick="btnApplyCoupon_Click" OnClientClick="return validateCoupon();" />
                        </div>
                    </div>
                    <asp:Label ID="lblCouponMessage" runat="server" CssClass="text-danger mt-2" Visible="false" />
                </asp:Panel>
                <div class="card border-secondary mb-5">
                    <div class="card-header bg-secondary border-0">
                        <h4 class="font-weight-semi-bold m-0">Tóm tắt giỏ hàng</h4>
                    </div>
                    <div class="card-body">
                        <div class="d-flex justify-content-between mb-3 pt-1">
                            <h6 class="font-weight-medium">Tổng phụ</h6>
                            <h6 class="font-weight-medium"><asp:Label ID="lblSubtotal" runat="server" Text="0 VNĐ" /></h6>
                        </div>
                        <div class="d-flex justify-content-between mb-3">
                            <h6 class="font-weight-medium">Phí vận chuyển</h6>
                            <h6 class="font-weight-medium"><asp:Label ID="lblShipping" runat="server" Text="30,000 VNĐ" /></h6>
                        </div>
                        <div class="d-flex justify-content-between">
                            <h6 class="font-weight-medium">Giảm giá</h6>
                            <h6 class="font-weight-medium"><asp:Label ID="lblDiscount" runat="server" Text="0 VNĐ" /></h6>
                        </div>
                    </div>
                    <div class="card-footer border-secondary bg-transparent">
                        <div class="d-flex justify-content-between mt-2">
                            <h5 class="font-weight-bold">Tổng cộng</h5>
                            <h5 class="font-weight-bold"><asp:Label ID="lblTotal" runat="server" Text="0 VNĐ" /></h5>
                        </div>
                        <asp:Button ID="btnCheckout" runat="server" CssClass="btn btn-block btn-primary my-3 py-3" Text="Tiến hành thanh toán" OnClick="btnCheckout_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Cart End -->
</asp:Content>