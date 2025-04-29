<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="OrderConfirmation.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.OrderConfirmation" %>
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
        .order-details {
            background-color: var(--bg-color);
            border: 1px solid var(--secondary-color);
            border-radius: 5px;
            padding: 20px;
        }
        @media (max-width: 768px) {
            .order-details {
                padding: 15px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid pt-5">
        <div class="order-details">
            <h2 class="font-weight-semi-bold mb-4">Đặt hàng thành công!</h2>
            <p>Cảm ơn bạn đã đặt hàng. Dưới đây là thông tin đơn hàng của bạn:</p>
            <h4>Mã đơn hàng: <asp:Label ID="lblMaDonHang" runat="server" /></h4>
            <h5 class="font-weight-medium mb-3">Sản phẩm</h5>
            <asp:Repeater ID="rptOrderItems" runat="server">
                <ItemTemplate>
                    <div class="d-flex justify-content-between">
                        <p><%# Eval("TenSP") %> (<%# Eval("KichCo") %>, <%# Eval("MauSac") %>, x<%# Eval("SoLuong") %>)</p>
                        <p><%# Eval("Total", "{0:N0} VNĐ") %></p>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <hr />
            <div class="d-flex justify-content-between mb-3">
                <h6 class="font-weight-medium">Tổng phụ</h6>
                <h6 class="font-weight-medium"><asp:Label ID="lblSubtotal" runat="server" /></h6>
            </div>
            <div class="d-flex justify-content-between mb-3">
                <h6 class="font-weight-medium">Phí vận chuyển</h6>
                <h6 class="font-weight-medium"><asp:Label ID="lblShipping" runat="server" /></h6>
            </div>
            <div class="d-flex justify-content-between mb-3">
                <h6 class="font-weight-medium">Giảm giá</h6>
                <h6 class="font-weight-medium"><asp:Label ID="lblDiscount" runat="server" /></h6>
            </div>
            <div class="d-flex justify-content-between">
                <h5 class="font-weight-bold">Tổng cộng</h5>
                <h5 class="font-weight-bold"><asp:Label ID="lblTotal" runat="server" /></h5>
            </div>
            <div class="mt-4">
                <p>Phương thức thanh toán: <asp:Label ID="lblPaymentMethod" runat="server" /></p>
                <p>Địa chỉ thanh toán: <asp:Label ID="lblBillingAddress" runat="server" /></p>
                <p>Địa chỉ giao hàng: <asp:Label ID="lblShippingAddress" runat="server" /></p>
            </div>
            <div class="mt-4">
                <a href="Shop.aspx" class="btn btn-primary">Tiếp tục mua sắm</a>
            </div>
        </div>
    </div>
</asp:Content>