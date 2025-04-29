<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.Login" %>
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
        .book_section {
            padding: 50px 0;
        }
        .container {
            max-width: 1200px;
        }
        .form_container {
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
        .img-fluid {
            max-height: 300px;
            object-fit: cover;
        }
        .text-danger {
            font-size: 14px;
        }
        @media (max-width: 768px) {
            .form_container {
                padding: 15px;
            }
            .img-fluid {
                max-height: 200px;
            }
            .btn-success {
                padding: 10px;
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

        function logSubmit() {
            console.log("Form submitted");
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container text-center">
                <asp:Label runat="server" ID="lblMsg" CssClass="alert" Visible="false" />
                <h2 class="mb-4">Đăng nhập</h2>
            </div>

            <div class="row justify-content-center">
                <div class="col-md-5">
                    <div class="text-center">
                        <img id="userLogin" src="../Images/Login.jpg" alt="Login Image" class="img-fluid rounded" />
                    </div>
                </div>

                <div class="col-md-5">
                    <div class="form_container p-4 shadow rounded bg-light">
                        <asp:Panel runat="server" DefaultButton="btnLogin">
                            <div class="mb-3">
                                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Tên đăng nhập" />
                                <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                                    ErrorMessage="Vui lòng nhập tên đăng nhập!" ForeColor="Red" Display="Dynamic" Font-Size="Small" />
                            </div>
                            <div class="mb-3">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Mật khẩu" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                    ErrorMessage="Vui lòng nhập mật khẩu!" ForeColor="Red" Display="Dynamic" Font-Size="Small" />
                            </div>
                            <div class="text-center">
                                <asp:Button ID="btnLogin" runat="server" Text="Đăng nhập" CssClass="btn btn-success text-white w-100"
                                    OnClick="btnLogin_Click" CausesValidation="true" OnClientClick="logSubmit();" />
                            </div>
                        </asp:Panel>
                        <div class="text-center mt-3">
                            <span class="text-info">Chưa có tài khoản? <a href="Registration.aspx" class="badge badge-info">Đăng ký tại đây...</a></span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>