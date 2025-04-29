<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.Admin.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <h2 class="mb-4">Tổng quan</h2>
        <div class="row">
            <div class="col-md-3">
                <div class="card text-white bg-primary mb-3">
                    <div class="card-body">
                        <h5 class="card-title">Người dùng</h5>
                        <p class="card-text">
                            <asp:Label ID="lblUserCount" runat="server" Text="0" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card text-white bg-success mb-3">
                    <div class="card-body">
                        <h5 class="card-title">Danh mục</h5>
                        <p class="card-text">
                            <asp:Label ID="lblCategoryCount" runat="server" Text="0" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card text-white bg-info mb-3">
                    <div class="card-body">
                        <h5 class="card-title">Sản phẩm</h5>
                        <p class="card-text">
                            <asp:Label ID="lblProductCount" runat="server" Text="0" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card text-white bg-warning mb-3">
                    <div class="card-body">
                        <h5 class="card-title">Đơn hàng</h5>
                        <p class="card-text">
                            <asp:Label ID="lblOrderCount" runat="server" Text="0" />
                        </p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <div class="card mb-3">
                    <div class="card-body">
                        <h5 class="card-title">Tổng doanh thu</h5>
                        <p class="card-text">
                            <asp:Label ID="lblTotalRevenue" runat="server" Text="0 VNĐ" />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card mb-3">
                    <div class="card-body">
                        <h5 class="card-title">Đơn hàng theo ngày</h5>
                        <canvas id="orderChart" width="400" height="200"></canvas>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Thêm Chart.js -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            // Dữ liệu cho biểu đồ (lấy từ code-behind qua biến ẩn)
            var orderData = JSON.parse('<%= OrderDataJson %>');
            var labels = orderData.map(function (item) { return item.Date; });
            var data = orderData.map(function (item) { return item.Count; });

            // Khởi tạo biểu đồ
            var ctx = document.getElementById('orderChart').getContext('2d');
            new Chart(ctx, {
                type: 'line',
                data: {
                    labels: labels,
                    datasets: [{
                        label: 'Số đơn hàng',
                        data: data,
                        borderColor: 'rgba(75, 192, 192, 1)',
                        backgroundColor: 'rgba(75, 192, 192, 0.2)',
                        fill: true
                    }]
                },
                options: {
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'Số đơn hàng'
                            }
                        },
                        x: {
                            title: {
                                display: true,
                                text: 'Ngày'
                            }
                        }
                    }
                }
            });
        });
    </script>
</asp:Content>