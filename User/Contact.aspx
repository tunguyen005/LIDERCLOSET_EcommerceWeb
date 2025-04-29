<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.Contact" %>
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
        .form-control {
            border: 1px solid var(--secondary-color);
            border-radius: 5px;
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
        .contact-form {
            background-color: var(--bg-color);
            border: 1px solid var(--secondary-color);
            padding: 20px;
            border-radius: 5px;
        }
        .text-danger {
            font-size: 14px;
        }
        .alert {
            margin-top: 20px;
        }
        @media (max-width: 768px) {
            .contact-form {
                padding: 15px;
            }
            .form-group {
                margin-bottom: 15px;
            }
            .btn-primary {
                padding: 10px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Page Header Start -->
    <div class="container-fluid bg-secondary mb-5">
        <div class="d-flex flex-column align-items-center justify-content-center" style="min-height: 300px">
            <h1 class="font-weight-semi-bold text-uppercase mb-3">Liên hệ</h1>
            <div class="d-inline-flex">
                <p class="m-0"><a href="Default.aspx">Trang chủ</a></p>
                <p class="m-0 px-2">-</p>
                <p class="m-0">Liên hệ</p>
            </div>
        </div>
    </div>
    <!-- Page Header End -->

    <!-- Contact Start -->
    <div class="container-fluid pt-5">
        <div class="text-center mb-4">
            <h2 class="section-title px-5"><span class="px-2">Liên hệ với chúng tôi</span></h2>
        </div>
        <div class="row px-xl-5">
            <div class="col-lg-7 mb-5">
                <div class="contact-form">
                    <asp:Label ID="lblMessage" runat="server" Visible="false" CssClass="alert" />
                    <asp:Panel runat="server" DefaultButton="btnSendMessage">
                        <div class="form-group">
                            <label>Họ và tên *</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Nguyễn Văn A" />
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" 
                                ErrorMessage="Vui lòng nhập họ và tên!" CssClass="text-danger" Display="Dynamic" />
                        </div>
                        <div class="form-group">
                            <label>Email *</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="example@email.com" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                                ErrorMessage="Vui lòng nhập email!" CssClass="text-danger" Display="Dynamic" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" 
                                ErrorMessage="Email không hợp lệ!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                CssClass="text-danger" Display="Dynamic" />
                        </div>
                        <div class="form-group">
                            <label>Tiêu đề *</label>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="Tiêu đề" />
                            <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" 
                                ErrorMessage="Vui lòng nhập tiêu đề!" CssClass="text-danger" Display="Dynamic" />
                        </div>
                        <div class="form-group">
                            <label>Lời nhắn *</label>
                            <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" placeholder="Lời nhắn" />
                            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" 
                                ErrorMessage="Vui lòng nhập lời nhắn!" CssClass="text-danger" Display="Dynamic" />
                        </div>
                        <div>
                            <asp:Button ID="btnSendMessage" runat="server" CssClass="btn btn-primary py-2 px-4" Text="Gửi lời nhắn" OnClick="btnSendMessage_Click" />
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <div class="col-lg-5 mb-5">
                <h5 class="font-weight-semi-bold mb-3">Liên hệ với chúng tôi</h5>
                <p>Chúng tôi luôn sẵn sàng hỗ trợ bạn. Vui lòng liên hệ qua các kênh dưới đây hoặc sử dụng form bên cạnh.</p>
                <div class="d-flex flex-column mb-3">
                    <h5 class="font-weight-semi-bold mb-3">Cửa hàng chính</h5>
                    <p class="mb-2"><i class="fa fa-map-marker-alt text-primary mr-3"></i>123 Đường Lê Lợi, Quận 1, TP.HCM, Việt Nam</p>
                    <p class="mb-2"><i class="fa fa-envelope text-primary mr-3"></i>support@lidercloset.com</p>
                    <p class="mb-2"><i class="fa fa-phone-alt text-primary mr-3"></i>+84 123 456 789</p>
                </div>
                <div class="d-flex flex-column">
                    <h5 class="font-weight-semi-bold mb-3">Chi nhánh</h5>
                    <p class="mb-2"><i class="fa fa-map-marker-alt text-primary mr-3"></i>456 Đường Nguyễn Huệ, Quận 3, TP.HCM, Việt Nam</p>
                    <p class="mb-2"><i class="fa fa-envelope text-primary mr-3"></i>info@lidercloset.com</p>
                    <p class="mb-0"><i class="fa fa-phone-alt text-primary mr-3"></i>+84 987 654 321</p>
                </div>
            </div>
        </div>
    </div>
    <!-- Contact End -->
</asp:Content>