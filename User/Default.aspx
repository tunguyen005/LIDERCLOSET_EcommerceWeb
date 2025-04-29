<%@ Page Title="" Language="C#" MasterPageFile="~/User/User.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.User.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/assets/owl.carousel.min.css" />
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
        .feature-item {
            background-color: var(--bg-color);
            border: 1px solid var(--secondary-color);
            padding: 20px;
            text-align: center;
            transition: transform 0.2s;
        }
        .feature-item:hover {
            transform: scale(1.05);
        }
        .cat-item {
            background-color: var(--bg-color);
            border: 1px solid var(--secondary-color);
            padding: 20px;
            text-align: center;
            transition: transform 0.2s;
        }
        .cat-item:hover {
            transform: scale(1.05);
        }
        .cat-img img {
            width: 100%;
            height: 200px;
            object-fit: cover;
        }
        .offer-item {
            background-color: var(--secondary-color);
            color: var(--bg-color);
            padding: 30px;
            position: relative;
            overflow: hidden;
        }
        .offer-item img {
            position: absolute;
            top: 0;
            left: 0;
            opacity: 0.3;
            width: 100%;
            height: 100%;
            object-fit: cover;
        }
        .btn-primary {
            background-color: var(--primary-color);
            color: var(--bg-color);
            padding: 10px 20px;
            text-decoration: none;
            border: none;
            cursor: pointer;
            transition: opacity 0.2s, transform 0.2s;
        }
        .btn-primary:hover {
            opacity: 0.9;
            transform: scale(1.05);
        }
        .subscribe-section {
            background-color: var(--secondary-color);
            color: var(--bg-color);
            padding: 40px;
            text-align: center;
        }
        .vendor-item img {
            width: 100%;
            height: auto;
        }
        .slider-item img {
            width: 100%;
            height: 400px;
            object-fit: cover;
        }
        .quick-view, .add-to-cart {
            margin: 5px;
            font-size: 14px;
        }
        @media (max-width: 768px) {
            .row {
                flex-direction: column;
            }
            .col-lg-4, .col-md-6 {
                width: 100%;
            }
            .slider-item img {
                height: 200px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Slider Quảng cáo -->
    <div class="container-fluid pt-5">
        <div class="owl-carousel slider-carousel">
            <div class="slider-item">
                <img src="../UserTemplate/img/slider-1.jpg" alt="Khuyến mãi mùa xuân">
                <div class="carousel-caption">
                    <h2>Khuyến mãi mùa xuân</h2>
                    <p>Giảm giá 20% cho mọi đơn hàng</p>
                    <a href="Shop.aspx" class="btn btn-primary">Mua sắm ngay</a>
                </div>
            </div>
            <div class="slider-item">
                <img src="../UserTemplate/img/slider-2.jpg" alt="Bộ sưu tập mùa đông">
                <div class="carousel-caption">
                    <h2>Bộ sưu tập mùa đông</h2>
                    <p>Thời trang ấm áp, phong cách hiện đại</p>
                    <a href="Shop.aspx" class="btn btn-primary">Mua sắm ngay</a>
                </div>
            </div>
        </div>
    </div>
    <!-- Kết thúc Slider Quảng cáo -->

    <!-- Đặc điểm nổi bật -->
    <div class="container-fluid pt-5">
        <div class="row px-xl-5 pb-3">
            <div class="col-lg-3 col-md-6 col-sm-12 pb-1">
                <div class="feature-item d-flex align-items-center mb-4">
                    <i class="fa fa-check text-primary m-0 mr-3"></i>
                    <h5 class="font-weight-semi-bold m-0">Sản phẩm chất lượng</h5>
                </div>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-12 pb-1">
                <div class="feature-item d-flex align-items-center mb-4">
                    <i class="fa fa-shipping-fast text-primary m-0 mr-2"></i>
                    <h5 class="font-weight-semi-bold m-0">Miễn phí vận chuyển</h5>
                </div>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-12 pb-1">
                <div class="feature-item d-flex align-items-center mb-4">
                    <i class="fas fa-exchange-alt text-primary m-0 mr-3"></i>
                    <h5 class="font-weight-semi-bold m-0">Đổi trả trong 14 ngày</h5>
                </div>
            </div>
            <div class="col-lg-3 col-md-6 col-sm-12 pb-1">
                <div class="feature-item d-flex align-items-center mb-4">
                    <i class="fa fa-phone-volume text-primary m-0 mr-3"></i>
                    <h5 class="font-weight-semi-bold m-0">Hỗ trợ toàn thời gian 24/7</h5>
                </div>
            </div>
        </div>
    </div>
    <!-- Kết thúc Đặc điểm nổi bật -->

    <!-- Danh mục -->
    <div class="container-fluid pt-5">
        <h2 class="text-center mb-4">Danh mục</h2>
        <div class="row px-xl-5 pb-3">
            <asp:Repeater ID="rptCategories" runat="server">
                <ItemTemplate>
                    <div class="col-lg-4 col-md-6 pb-1">
                        <div class="cat-item d-flex flex-column border mb-4">
                            <p class="text-right"><%# Eval("ProductCount") %> Sản phẩm</p>
                            <a href='Shop.aspx?MaHangMuc=<%# Eval("MaHangMuc") %>' class="cat-img position-relative overflow-hidden mb-3">
                                <img class="img-fluid" src='<%# Eval("HM_ImageUrl") %>' alt='<%# Eval("TenHangMuc") %>'>
                            </a>
                            <h5 class="font-weight-semi-bold m-0"><%# Eval("TenHangMuc") %></h5>
                            <div class="mt-2">
                                <a href='ShopDetail.aspx?MaHangMuc=<%# Eval("MaHangMuc") %>' class="btn btn-primary quick-view">Xem nhanh</a>
                                <asp:Button ID="btnAddToCart" runat="server" Text="Thêm vào giỏ" CssClass="btn btn-primary add-to-cart" CommandArgument='<%# Eval("MaHangMuc") %>' OnClick="btnAddToCart_Click" />
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <!-- Kết thúc Danh mục -->

    <!-- Ưu đãi -->
    <div class="container-fluid offer pt-5">
        <div class="row px-xl-5">
            <div class="col-md-6 pb-4">
                <div class="offer-item text-center text-md-right mb-2 py-5 px-5">
                    <img src="../UserTemplate/img/offer-1.png" alt="Bộ sưu tập mùa xuân">
                    <div class="position-relative" style="z-index: 1;">
                        <h5 class="text-uppercase text-primary mb-3">Giảm giá 20% toàn bộ đơn hàng</h5>
                        <h1 class="mb-4 font-weight-semi-bold">Bộ sưu tập mùa xuân</h1>
                        <a href="Shop.aspx" class="btn btn-primary py-md-2 px-md-3">Mua sắm ngay</a>
                    </div>
                </div>
            </div>
            <div class="col-md-6 pb-4">
                <div class="offer-item text-center text-md-left mb-2 py-5 px-5">
                    <img src="../UserTemplate/img/offer-2.png" alt="Bộ sưu tập mùa đông">
                    <div class="position-relative" style="z-index: 1;">
                        <h5 class="text-uppercase text-primary mb-3">Giảm giá 20% toàn bộ đơn hàng</h5>
                        <h1 class="mb-4 font-weight-semi-bold">Bộ sưu tập mùa đông</h1>
                        <a href="Shop.aspx" class="btn btn-primary py-md-2 px-md-3">Mua sắm ngay</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Kết thúc Ưu đãi -->

    <!-- Đăng ký nhận thông tin -->
    <div class="container-fluid subscribe-section my-5">
        <div class="row justify-content-md-center py-5 px-xl-5">
            <div class="col-md-6 col-12 py-5">
                <div class="text-center mb-2 pb-2">
                    <h2 class="section-title px-5 mb-3"><span class="bg-secondary px-2">Cập nhật thông tin</span></h2>
                    <p>Đăng ký để nhận thông tin mới nhất về sản phẩm và khuyến mãi.</p>
                </div>
                <asp:Panel runat="server" DefaultButton="btnSubscribe">
                    <div class="input-group">
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control border-white p-4" placeholder="Nhập email của bạn" />
                        <div class="input-group-append">
                            <asp:Button ID="btnSubscribe" runat="server" Text="Đăng ký" CssClass="btn btn-primary px-4" OnClick="btnSubscribe_Click" />
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>
    <!-- Kết thúc Đăng ký nhận thông tin -->

    <!-- Nhà cung cấp -->
    <div class="container-fluid py-5">
        <h2 class="text-center mb-4">Nhà cung cấp của chúng tôi</h2>
        <div class="row px-xl-5">
            <div class="col">
                <div class="owl-carousel vendor-carousel">
                    <div class="vendor-item border p-4">
                        <img src="../UserTemplate/img/vendor-1.jpg" alt="Nhà cung cấp 1">
                    </div>
                    <div class="vendor-item border p-4">
                        <img src="../UserTemplate/img/vendor-2.jpg" alt="Nhà cung cấp 2">
                    </div>
                    <div class="vendor-item border p-4">
                        <img src="../UserTemplate/img/vendor-3.jpg" alt="Nhà cung cấp 3">
                    </div>
                    <div class="vendor-item border p-4">
                        <img src="../UserTemplate/img/vendor-4.jpg" alt="Nhà cung cấp 4">
                    </div>
                    <div class="vendor-item border p-4">
                        <img src="../UserTemplate/img/vendor-5.jpg" alt="Nhà cung cấp 5">
                    </div>
                    <div class="vendor-item border p-4">
                        <img src="../UserTemplate/img/vendor-6.jpg" alt="Nhà cung cấp 6">
                    </div>
                    <div class="vendor-item border p-4">
                        <img src="../UserTemplate/img/vendor-7.jpg" alt="Nhà cung cấp 7">
                    </div>
                    <div class="vendor-item border p-4">
                        <img src="../UserTemplate/img/vendor-8.jpg" alt="Nhà cung cấp 8">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- Kết thúc Nhà cung cấp -->

    <script src="https://cdnjs.cloudflare.com/ajax/libs/OwlCarousel2/2.3.4/owl.carousel.min.js"></script>
    <script>
        $(document).ready(function () {
            $('.slider-carousel').owlCarousel({
                loop: true,
                margin: 10,
                nav: true,
                autoplay: true,
                autoplayTimeout: 3000,
                responsive: {
                    0: { items: 1 },
                    600: { items: 1 },
                    1000: { items: 1 }
                }
            });
            $('.vendor-carousel').owlCarousel({
                loop: true,
                margin: 10,
                nav: true,
                responsive: {
                    0: { items: 1 },
                    600: { items: 3 },
                    1000: { items: 5 }
                }
            });
        });
    </script>
</asp:Content>