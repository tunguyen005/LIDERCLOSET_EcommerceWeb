<%@ Page EnableViewState="true" EnableEventValidation="true" Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.Registration" %>
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
        .container {
            max-width: 1200px;
        }
        .card {
            background-color: var(--bg-color);
            border: 1px solid var(--secondary-color);
            border-radius: 5px;
        }
        .form-control {
            border: 1px solid var(--secondary-color);
            border-radius: 5px;
        }
        .btn-success {
            background-color: var(--primary-color);
            color: var(--bg-color);
            border: none;
            transition: opacity 0.2s, transform 0.2s;
        }
        .btn-success:hover {
            opacity: 0.9;
            transform: scale(1.05);
        }
        .alert {
            margin-bottom: 20px;
        }
        .img-thumbnail {
            object-fit: cover;
        }
        .text-danger {
            font-size: 14px;
        }
        @media (max-width: 768px) {
            .card {
                padding: 15px;
            }
            .form-control {
                font-size: 14px;
            }
            .btn-success {
                padding: 10px;
            }
            .img-thumbnail {
                width: 100px;
                height: 100px;
            }
        }
    </style>
    <script>
        window.onload = function () {
            var msg = document.getElementById("<%= lblMsg.ClientID %>");
            if (msg && msg.innerText !== "") {
                setTimeout(function () {
                    msg.style.display = "none";
                }, 5000);
            }
        }

        function ImagePreview(input) {
            console.log("ImagePreview called");
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%= imgUser.ClientID %>').prop('src', e.target.result)
                        .width(150)
                        .height(150);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

        function logSubmit() {
            console.log("Form submitted");
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="container py-5">
        <div class="card shadow-lg p-4">
            <div class="text-center mb-4">
                <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false" />
                <div class="text-center mb-3">
                    <asp:Label ID="lblHeaderMsg" runat="server" CssClass="fw-bold h3" Text="Đăng ký tài khoản" />
                </div>
            </div>
            <asp:Panel runat="server" DefaultButton="btnRegister">

                <div class="row g-4">
                    <div class="col-md-6">
                        <div class="mb-3 row">
                            <label class="col-sm-4 col-form-label">Họ và tên</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Nhập họ và tên" />
                                <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" 
                                    ErrorMessage="Vui lòng nhập họ và tên!" CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-sm-4 col-form-label">Tên đăng nhập</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Nhập tên đăng nhập" />
                                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" 
                                    ErrorMessage="Vui lòng nhập tên đăng nhập!" CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-sm-4 col-form-label">Email</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Nhập email" TextMode="Email" />
                                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                                    ErrorMessage="Vui lòng nhập email!" CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" 
                                    ValidationExpression="^[\w\.-]+@[\w\.-]+\.\w+$" ErrorMessage="Email không hợp lệ!" 
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-sm-4 col-form-label">Số điện thoại</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" placeholder="Nhập số điện thoại" TextMode="Phone" />
                                <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="txtMobile" 
                                    ErrorMessage="Vui lòng nhập số điện thoại!" CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revMobile" runat="server" ControlToValidate="txtMobile" 
                                    ValidationExpression="^\d{10}$" ErrorMessage="Số điện thoại phải có 10 chữ số!" 
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="mb-3 row">
                            <label class="col-sm-4 col-form-label">Địa chỉ</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Nhập địa chỉ" TextMode="MultiLine" />
                                <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" 
                                    ErrorMessage="Vui lòng nhập địa chỉ!" CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-sm-4 col-form-label">Mã bưu điện</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtPostCode" runat="server" CssClass="form-control" placeholder="Nhập mã bưu điện" />
                                <asp:RequiredFieldValidator ID="rfvPostCode" runat="server" ControlToValidate="txtPostCode" 
                                    ErrorMessage="Vui lòng nhập mã bưu điện!" CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revPostCode" runat="server" ControlToValidate="txtPostCode" 
                                    ValidationExpression="^\d{5,6}$" ErrorMessage="Mã bưu điện không hợp lệ!" 
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="mb-3 row">
                            <label class="col-sm-4 col-form-label">Mật khẩu</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Nhập mật khẩu" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" 
                                    ErrorMessage="Vui lòng nhập mật khẩu!" CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" 
                                    ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$" 
                                    ErrorMessage="Mật khẩu phải có ít nhất 8 ký tự, gồm chữ và số!" 
                                    CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        <div class="mb-3 row align-items-center">
                            <label class="col-sm-4 col-form-label">Ảnh đại diện</label>
                            <div class="col-sm-8">
                                <asp:FileUpload ID="fuUserImage" runat="server" CssClass="form-control" ToolTip="Ảnh đại diện" onchange="ImagePreview(this);" />
                                <asp:RegularExpressionValidator ID="revImage" runat="server" ControlToValidate="fuUserImage" 
                                    ValidationExpression="^.*\.(jpg|jpeg|png)$" ErrorMessage="Chỉ chấp nhận file JPG hoặc PNG!" 
                                    CssClass="text-danger" Display="Dynamic" />
                                <asp:Image ID="imgUser" runat="server" CssClass="img-thumbnail mt-3" Width="150px" Height="150px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="text-center mt-4">
                    <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-success px-5" Text="Đăng ký" OnClick="btnRegister_Click" CausesValidation="true" />
                    <p class="mt-3" id="txtAlreadyUser" runat="server">Đã có tài khoản? <a href="Login.aspx" class="text-primary">Đăng nhập tại đây...</a></p>
                </div>
            </asp:Panel>
        </div>
    </section>
</asp:Content>