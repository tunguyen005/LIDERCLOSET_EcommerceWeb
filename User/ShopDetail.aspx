<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="ShopDetail.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.ShopDetail" %>
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
        .carousel-item img {
            width: 100%;
            height: 450px;
            object-fit: cover;
        }
        .carousel-item img.zoom-img:hover {
            transform: scale(1.1);
        }
        .carousel-inner {
            overflow: hidden;
        }
        .product-item {
            transition: transform 0.2s;
        }
        .product-item:hover {
            transform: scale(1.05);
        }
        .btn-primary {
            background-color: var(--primary-color);
            color: var(--bg-color);
            border: none;
            padding: 8px 16px;
            transition: opacity 0.2s, transform 0.2s;
        }
        .btn-primary:hover {
            opacity: 0.9;
            transform: scale(1.05);
        }
        .quantity .form-control {
            width: 60px;
        }
        .nav-tabs .nav-link {
            color: var(--bg-color);
            margin: 0 5px;
        }
        .rblSize label, .rblColor label {
            margin-right: 15px;
            padding: 5px 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
            transition: all 0.2s;
        }
        .rblSize input:checked + label, .rblColor input:checked + label {
            background-color: var(--primary-color);
            color: var(--bg-color);
            border-color: var(--primary-color);
        }
        @media (max-width: 768px) {
            .carousel-item img {
                height: 300px;
            }
            .col-lg-5, .col-lg-7 {
                width: 100%;
            }
            .nav-tabs .nav-link {
                font-size: 14px;
                padding: 10px;
            }
        }
    </style>
    <script>
        function validateAddToCart() {
            var size = document.querySelector('input[name="<%= rblSize.UniqueID %>"]:checked');
            var color = document.querySelector('input[name="<%= rblColor.UniqueID %>"]:checked');
            if (!size || !color) {
                alert('Vui lòng chọn kích cỡ và màu sắc!');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Tiêu đề trang -->
    <div class="container-fluid mb-5" style="background: linear-gradient(rgba(0,0,0,0.5), rgba(0,0,0,0.5)), url('../UserTemplate/img/product-bg.jpg'); background-size: cover; background-position: center;">
        <div class="d-flex flex-column align-items-center justify-content-center text-white" style="min-height: 300px">
            <h1 class="font-weight-semi-bold text-uppercase mb-3"><asp:Label ID="lblTenSPHeader" runat="server" /></h1>
            <div class="d-inline-flex">
                <p class="m-0"><a href="Default.aspx" class="text-white">Trang chủ</a></p>
                <p class="m-0 px-2">-</p>
                <p class="m-0"><a href="Shop.aspx" class="text-white">Cửa hàng</a></p>
                <p class="m-0 px-2">-</p>
                <p class="m-0">Chi tiết sản phẩm</p>
            </div>
        </div>
    </div>
    <!-- Kết thúc Tiêu đề trang -->

    <!-- Chi tiết sản phẩm -->
    <div class="container-fluid py-5">
        <div class="row px-xl-5">
            <div class="col-lg-5 pb-5">
                <div id="product-carousel" class="carousel slide" data-ride="carousel">
                    <div class="carousel-inner border">
                        <asp:Repeater ID="rptImages" runat="server">
                            <ItemTemplate>
                                <div class="carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>">
                                    <img class="w-100 h-100 zoom-img" src='<%# Eval("HinhAnh") %>' alt='<%# Eval("TenSP") %>' style="transition: transform 0.3s;">
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <a class="carousel-control-prev" href="#product-carousel" data-slide="prev">
                        <i class="fa fa-2x fa-angle-left text-dark"></i>
                    </a>
                    <a class="carousel-control-next" href="#product-carousel" data-slide="next">
                        <i class="fa fa-2x fa-angle-right text-dark"></i>
                    </a>
                </div>
                <div class="d-flex justify-content-center mt-3">
                    <asp:Repeater ID="rptThumbnails" runat="server">
                        <ItemTemplate>
                            <img class="img-thumbnail m-1" src='<%# Eval("HinhAnh") %>' alt='<%# Eval("TenSP") %>' style="width: 60px; height: 60px; cursor: pointer;" data-slide-to='<%# Container.ItemIndex %>' onclick="jQuery('#product-carousel').carousel(<%# Container.ItemIndex %>);">
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <div class="col-lg-7 pb-5">
                <h3 class="font-weight-semi-bold"><asp:Label ID="lblTenSP" runat="server" /></h3>
                <div class="d-flex mb-3">
                    <div class="text-primary mr-2">
                        <asp:Literal ID="litRating" runat="server" />
                    </div>
                    <small class="pt-1">(<asp:Label ID="lblReviewCount" runat="server" /> Đánh giá)</small>
                </div>
                <div class="d-flex align-items-center mb-4">
                    <h3 class="font-weight-semi-bold text-primary mr-3"><asp:Label ID="lblGiaGiam" runat="server" /></h3>
                    <h5 class="text-muted"><del><asp:Label ID="lblGia" runat="server" /></del></h5>
                </div>
                <p class="mb-2"><i class="fas fa-info-circle mr-2"></i>Chất liệu: <asp:Label ID="lblChatLieu" runat="server" /></p>
                <p class="mb-2"><i class="fas fa-globe mr-2"></i>Xuất xứ: <asp:Label ID="lblXuatXu" runat="server" /></p>
                <p class="mb-4"><i class="fas fa-box-open mr-2"></i>Tồn kho: <asp:Label ID="lblSoLuongTon" runat="server" /></p>
                <p class="mb-4 border-left pl-3"><asp:Label ID="lblMoTa" runat="server" /></p>
                <div class="d-flex mb-3 align-items-center">
                    <p class="text-dark font-weight-medium mb-0 mr-3"><i class="fas fa-ruler mr-2"></i>Kích cỡ:</p>
                    <asp:RadioButtonList ID="rblSize" runat="server" CssClass="d-flex flex-wrap" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
                <div class="d-flex mb-4 align-items-center">
                    <p class="text-dark font-weight-medium mb-0 mr-3"><i class="fas fa-palette mr-2"></i>Màu sắc:</p>
                    <asp:RadioButtonList ID="rblColor" runat="server" CssClass="d-flex flex-wrap" RepeatDirection="Horizontal" RepeatLayout="Flow">
                    </asp:RadioButtonList>
                </div>
                <div class="d-flex align-items-center mb-4 pt-2">
                    <div class="input-group quantity mr-3" style="width: 130px;">
                        <div class="input-group-btn">
                            <asp:Button ID="btnMinus" runat="server" CssClass="btn btn-primary btn-minus" Text="-" OnClick="btnMinus_Click" />
                        </div>
                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control bg-secondary text-center" Text="1" />
                        <div class="input-group-btn">
                            <asp:Button ID="btnPlus" runat="server" CssClass="btn btn-primary btn-plus" Text="+" OnClick="btnPlus_Click" />
                        </div>
                    </div>
                    <asp:Button ID="btnAddToCart" runat="server" CssClass="btn btn-primary px-4 py-2" Text="Thêm vào giỏ <i class='fas fa-shopping-cart ml-2'></i>" OnClick="btnAddToCart_Click" OnClientClick="return validateAddToCart();" />
                </div>
                <div class="d-flex pt-2">
                    <p class="text-dark font-weight-medium mb-0 mr-2"><i class="fas fa-share-alt mr-2"></i>Chia sẻ:</p>
                    <div class="d-inline-flex">
                        <a class="text-dark px-2" href="https://www.facebook.com/sharer/sharer.php?u=<%= Request.Url.AbsoluteUri %>">
                            <i class="fab fa-facebook-f"></i>
                        </a>
                        <a class="text-dark px-2" href="https://twitter.com/intent/tweet?url=<%= Request.Url.AbsoluteUri %>">
                            <i class="fab fa-twitter"></i>
                        </a>
                        <a class="text-dark px-2" href="https://www.linkedin.com/shareArticle?url=<%= Request.Url.AbsoluteUri %>">
                            <i class="fab fa-linkedin-in"></i>
                        </a>
                        <a class="text-dark px-2" href="https://pinterest.com/pin/create/button/?url=<%= Request.Url.AbsoluteUri %>">
                            <i class="fab fa-pinterest"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <div class="row px-xl-5">
            <div class="col">
                <div class="nav nav-tabs justify-content-center border-0 mb-4">
                    <a class="nav-item nav-link active text-uppercase" data-toggle="tab" href="#tab-pane-1" style="background: var(--primary-color); color: var(--bg-color); border-radius: 5px 5px 0 0;">Mô tả</a>
                    <a class="nav-item nav-link text-uppercase" data-toggle="tab" href="#tab-pane-2" style="background: var(--secondary-color); color: var(--bg-color); border-radius: 5px 5px 0 0;">Thông tin</a>
                    <a class="nav-item nav-link text-uppercase" data-toggle="tab" href="#tab-pane-3" style="background: var(--secondary-color); color: var(--bg-color); border-radius: 5px 5px 0 0;">Đánh giá (<asp:Label ID="lblReviewCountTab" runat="server" />)</a>
                </div>
                <div class="tab-content shadow p-4" style="background: var(--bg-color); border-radius: 5px;">
                    <div class="tab-pane fade show active" id="tab-pane-1">
                        <h4 class="mb-3 text-primary">Mô tả sản phẩm</h4>
                        <p class="lead"><asp:Label ID="lblMoTaFull" runat="server" /></p>
                    </div>
                    <div class="tab-pane fade" id="tab-pane-2">
                        <h4 class="mb-3 text-primary">Thông tin bổ sung</h4>
                        <ul class="list-unstyled">
                            <li><strong>Chất liệu:</strong> <asp:Label ID="lblChatLieuInfo" runat="server" /></li>
                            <li><strong>Xuất xứ:</strong> <asp:Label ID="lblXuatXuInfo" runat="server" /></li>
                            <li><strong>Số lượng tồn:</strong> <asp:Label ID="lblSoLuongTonTab" runat="server" /></li>
                        </ul>
                    </div>
                    <div class="tab-pane fade" id="tab-pane-3">
                        <div class="row">
                            <div class="col-md-6">
                                <h4 class="mb-4 text-primary">Đánh giá cho "<asp:Label ID="lblTenSPReview" runat="server" />"</h4>
                                <asp:Repeater ID="rptReviews" runat="server">
                                    <ItemTemplate>
                                        <div class="media mb-4 p-3 border rounded">
                                            <img src="../UserTemplate/img/user.jpg" alt="User" class="img-fluid mr-3 mt-1" style="width: 45px;">
                                            <div class="media-body">
                                                <h6><%# Eval("TenNguoiDung") %><small> - <i><%# Eval("NgayDanhGia", "{0:dd/MM/yyyy}") %></i></small></h6>
                                                <div class="text-primary mb-2">
                                                    <%# GetRatingStars(Convert.ToInt32(Eval("DiemDanhGia"))) %>
                                                </div>
                                                <p><%# Eval("NoiDung") %></p>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="col-md-6">
                                <h4 class="mb-4 text-primary">Để lại đánh giá</h4>
                                <small>Email của bạn sẽ không được công khai. Các trường bắt buộc được đánh dấu *</small>
                                <div class="d-flex my-3">
                                    <p class="mb-0 mr-2">Đánh giá của bạn * :</p>
                                    <asp:DropDownList ID="ddlRating" runat="server" CssClass="form-control" Width="100px">
                                        <asp:ListItem Text="1 sao" Value="1" />
                                        <asp:ListItem Text="2 sao" Value="2" />
                                        <asp:ListItem Text="3 sao" Value="3" />
                                        <asp:ListItem Text="4 sao" Value="4" />
                                        <asp:ListItem Text="5 sao" Value="5" Selected="True" />
                                    </asp:DropDownList>
                                </div>
                                <asp:Panel runat="server" DefaultButton="btnSubmitReview">
                                    <div class="form-group">
                                        <label for="txtReview">Đánh giá của bạn *</label>
                                        <asp:TextBox ID="txtReview" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
                                        <asp:RequiredFieldValidator ID="rfvReview" runat="server" ControlToValidate="txtReview" ErrorMessage="Vui lòng nhập nội dung đánh giá!" CssClass="text-danger" Display="Dynamic" />
                                    </div>
                                    <div class="form-group">
                                        <label for="txtName">Tên của bạn *</label>
                                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
                                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Vui lòng nhập tên!" CssClass="text-danger" Display="Dynamic" />
                                    </div>
                                    <div class="form-group">
                                        <label for="txtEmail">Email của bạn *</label>
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Vui lòng nhập email!" CssClass="text-danger" Display="Dynamic" />
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email không hợp lệ!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="text-danger" Display="Dynamic" />
                                    </div>
                                    <div class="form-group mb-0">
                                        <asp:Button ID="btnSubmitReview" runat="server" CssClass="btn btn-primary px-4" Text="Gửi đánh giá" OnClick="btnSubmitReview_Click" />
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Kết thúc Chi tiết sản phẩm -->

    <!-- Sản phẩm liên quan -->
    <div class="container-fluid py-5">
        <div class="text-center mb-4">
            <h2 class="section-title px-5"><span class="px-2">Sản phẩm liên quan</span></h2>
        </div>
        <div class="row px-xl-5">
            <div class="col">
                <div class="carousel slide" data-ride="carousel" id="related-carousel">
                    <div class="carousel-inner">
                        <asp:ListView ID="lvRelatedProducts" runat="server">
                            <LayoutTemplate>
                                <div class="carousel-item active">
                                    <div class="row">
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                                    </div>
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div class="col-lg-3 col-md-6 col-sm-12 pb-1">
                                    <div class="card product-item border-0 mb-4 shadow-sm">
                                        <div class="card-header product-img position-relative overflow-hidden bg-transparent border p-0">
                                            <img class="img-fluid w-100" src='<%# Eval("HinhAnh") %>' alt='<%# Eval("TenSP") %>' style="height: 200px; object-fit: cover;">
                                        </div>
                                        <div class="card-body border-left border-right text-center p-0 pt-4 pb-3">
                                            <h6 class="text-truncate mb-3"><%# Eval("TenSP") %></h6>
                                            <div class="d-flex justify-content-center">
                                                <h6><%# Eval("GiaGiam", "{0:N0} VNĐ") %></h6>
                                                <%# Convert.ToDecimal(Eval("GiaGiam")) < Convert.ToDecimal(Eval("Gia")) ? "<h6 class=\"text-muted ml-2\"><del>" + String.Format("{0:N0} VNĐ", Eval("Gia")) + "</del></h6>" : "" %>
                                            </div>
                                        </div>
                                        <div class="card-footer d-flex justify-content-between bg-light border">
                                            <a href='ShopDetail.aspx?MaSP=<%# Eval("MaSP") %>' class="btn btn-sm text-dark p-0"><i class="fas fa-eye text-primary mr-1"></i>Xem chi tiết</a>
                                            <asp:Button ID="btnAddToCartRelated" runat="server" Text="Thêm vào giỏ" CssClass="btn btn-sm text-dark p-0" CommandArgument='<%# Eval("MaSP") %>' OnClick="btnAddToCartRelated_Click" />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                            <EmptyDataTemplate>
                                <div class="col-12 text-center">
                                    <p>Không có sản phẩm liên quan.</p>
                                </div>
                            </EmptyDataTemplate>
                        </asp:ListView>
                    </div>
                    <a class="carousel-control-prev" href="#related-carousel" data-slide="prev">
                        <i class="fa fa-2x fa-angle-left text-dark"></i>
                    </a>
                    <a class="carousel-control-next" href="#related-carousel" data-slide="next">
                        <i class="fa fa-2x fa-angle-right text-dark"></i>
                    </a>
                </div>
            </div>
        </div>
    </div>
    <!-- Kết thúc Sản phẩm liên quan -->
</asp:Content>