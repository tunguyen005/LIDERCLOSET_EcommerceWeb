<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="QuanLyCuaHangLiderCloset.Admin.Product" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        window.onload = function () {
            var secs = 5;
            setTimeout(function () {
                var lblMsg = document.getElementById("<%=lblMsg.ClientID %>");
                if (lblMsg) lblMsg.style.display = "none";
            }, secs * 1000);
        }

        function ImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var controlName = input.id.substr(input.id.indexOf("_") + 1);
                    if (controlName == 'fuFirstImage') {
                        $('#<%=imageProduct1.ClientID %>').show().prop('src', e.target.result).width(200).height(200);
                    } else if (controlName == 'fuSecondImage') {
                        $('#<%=imageProduct2.ClientID %>').show().prop('src', e.target.result).width(200).height(200);
                    } else if (controlName == 'fuThirdImage') {
                        $('#<%=imageProduct3.ClientID %>').show().prop('src', e.target.result).width(200).height(200);
                    } else if (controlName == 'fuFourthImage') {
                        $('#<%=imageProduct4.ClientID %>').show().prop('src', e.target.result).width(200).height(200);
                    }
                }
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="mb-4">
        <asp:Label ID="lblMsg" runat="server" CssClass="alert" Visible="false"></asp:Label>
    </div>
    <div class="row">
        <div class="col-sm-12 col-md-12">
            <div class="card">
                <div class="card-body">
                    <h4 class="card-title">Thêm/Sửa sản phẩm</h4>
                    <hr />
                    <div class="form-body">
                        <div class="row">
                            <div class="col-md-6">
                                <label>Tên sản phẩm</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtProductName" runat="server" CssClass="form-control" placeholder="Nhập tên sản phẩm"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtProductName"
                                        ErrorMessage="Tên sản phẩm là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Danh mục cha</label>
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" AppendDataBoundItems="true" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Chọn danh mục cha</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlCategory" InitialValue="0"
                                        ErrorMessage="Danh mục cha là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Danh mục con</label>
                                <div class="form-group">
                                    <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0">Chọn danh mục con</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvSubCategory" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="ddlSubCategory" InitialValue="0"
                                        ErrorMessage="Danh mục con là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Đơn giá</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" placeholder="Nhập đơn giá"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtPrice"
                                        ErrorMessage="Đơn giá là bắt buộc"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revPrice" runat="server" ControlToValidate="txtPrice"
                                        ValidationExpression="\d+(?:\.\d{1,2})?" ErrorMessage="Đơn giá không hợp lệ" ForeColor="Red"
                                        Display="Dynamic" SetFocusOnError="true" Font-Size="Small"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Màu sắc</label>
                                <div class="form-group">
                                    <asp:ListBox ID="lboxColor" runat="server" CssClass="form-control" SelectionMode="Multiple"
                                        ToolTip="Giữ phím CTRL để chọn nhiều mục">
                                        <asp:ListItem Value="Xanh">Xanh</asp:ListItem>
                                        <asp:ListItem Value="Đỏ">Đỏ</asp:ListItem>
                                        <asp:ListItem Value="Hồng">Hồng</asp:ListItem>
                                        <asp:ListItem Value="Tím">Tím</asp:ListItem>
                                        <asp:ListItem Value="Nâu">Nâu</asp:ListItem>
                                        <asp:ListItem Value="Xám">Xám</asp:ListItem>
                                        <asp:ListItem Value="Lục">Lục</asp:ListItem>
                                        <asp:ListItem Value="Vàng">Vàng</asp:ListItem>
                                        <asp:ListItem Value="Trắng">Trắng</asp:ListItem>
                                        <asp:ListItem Value="Đen">Đen</asp:ListItem>
                                    </asp:ListBox>
                                    <asp:RequiredFieldValidator ID="rfvColor" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="lboxColor"
                                        ErrorMessage="Màu sắc là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Kích cỡ</label>
                                <div class="form-group">
                                    <asp:ListBox ID="lboxSize" runat="server" CssClass="form-control" SelectionMode="Multiple"
                                        ToolTip="Giữ phím CTRL để chọn nhiều mục">
                                        <asp:ListItem Value="XS">XS</asp:ListItem>
                                        <asp:ListItem Value="S">S</asp:ListItem>
                                        <asp:ListItem Value="M">M</asp:ListItem>
                                        <asp:ListItem Value="L">L</asp:ListItem>
                                        <asp:ListItem Value="XL">XL</asp:ListItem>
                                        <asp:ListItem Value="XXL">XXL</asp:ListItem>
                                    </asp:ListBox>
                                    <asp:RequiredFieldValidator ID="rfvSize" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="lboxSize"
                                        ErrorMessage="Kích cỡ là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Số lượng</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" placeholder="Nhập số lượng"
                                        TextMode="Number"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtQuantity"
                                        ErrorMessage="Số lượng là bắt buộc"></asp:RequiredFieldValidator>
                                    <asp:RangeValidator ID="rvQuantity" runat="server" ControlToValidate="txtQuantity"
                                        MinimumValue="0" MaximumValue="10000" Type="Integer" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ErrorMessage="Số lượng không hợp lệ (0-10000)"></asp:RangeValidator>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <label>Nhà cung cấp</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" placeholder="Nhập tên nhà cung cấp"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtCompanyName"
                                        ErrorMessage="Nhà cung cấp là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label>Mô tả ngắn</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtShortDescription" runat="server" CssClass="form-control" placeholder="Nhập mô tả ngắn"
                                        TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvShortDescription" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtShortDescription"
                                        ErrorMessage="Mô tả ngắn là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label>Mô tả chi tiết</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtLongDescription" runat="server" CssClass="form-control" placeholder="Nhập mô tả chi tiết"
                                        TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvLongDescription" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtLongDescription"
                                        ErrorMessage="Mô tả chi tiết là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <label>Mô tả bổ sung</label>
                                <div class="form-group">
                                    <asp:TextBox ID="txtAdditionalDescription" runat="server" CssClass="form-control" placeholder="Nhập mô tả bổ sung"
                                        TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvAdditionalDescription" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="txtAdditionalDescription"
                                        ErrorMessage="Mô tả bổ sung là bắt buộc"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Ảnh sản phẩm 1</label>
                                <div class="form-group">
                                    <asp:FileUpload ID="fuFirstImage" runat="server" CssClass="form-control" ToolTip="Chỉ chấp nhận .jpg, .png, .jpeg" onchange="ImagePreview(this);" />
                                    <asp:RequiredFieldValidator ID="rfvFirstImage" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="fuFirstImage"
                                        ErrorMessage="Ảnh sản phẩm 1 là bắt buộc" Enabled="false"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <label>Ảnh sản phẩm 2</label>
                                <div class="form-group">
                                    <asp:FileUpload ID="fuSecondImage" runat="server" CssClass="form-control" ToolTip="Chỉ chấp nhận .jpg, .png, .jpeg" onchange="ImagePreview(this);" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Ảnh sản phẩm 3</label>
                                <div class="form-group">
                                    <asp:FileUpload ID="fuThirdImage" runat="server" CssClass="form-control" ToolTip="Chỉ chấp nhận .jpg, .png, .jpeg" onchange="ImagePreview(this);" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <label>Ảnh sản phẩm 4</label>
                                <div class="form-group">
                                    <asp:FileUpload ID="fuFourthImage" runat="server" CssClass="form-control" ToolTip="Chỉ chấp nhận .jpg, .png, .jpeg" onchange="ImagePreview(this);" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Ảnh mặc định</label>
                                <div class="form-group">
                                    <asp:RadioButtonList ID="rblDefaultImage" runat="server" CssClass="form-control" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1">  Ảnh 1  </asp:ListItem>
                                        <asp:ListItem Value="2">  Ảnh 2  </asp:ListItem>
                                        <asp:ListItem Value="3">  Ảnh 3  </asp:ListItem>
                                        <asp:ListItem Value="4">  Ảnh 4  </asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="rfvDefaultImage" runat="server" ForeColor="Red" Font-Size="Small"
                                        Display="Dynamic" SetFocusOnError="true" ControlToValidate="rblDefaultImage"
                                        ErrorMessage="Ảnh mặc định là bắt buộc"></asp:RequiredFieldValidator>
                                    <asp:HiddenField ID="hfProductId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hfDefImagePos" runat="server" Value="0" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Có thể tùy chỉnh</label>
                                <div class="form-group">
                                    <asp:CheckBox ID="cbIsCustomized" runat="server" Text="  Có thể tùy chỉnh" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label>Hoạt động</label>
                                <div class="form-group">
                                    <asp:CheckBox ID="cbIsActive" runat="server" Text="  Hoạt động" Checked="true" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 align-content-sm-between pl-3">
                                <span>
                                    <asp:Image ID="imageProduct1" runat="server" CssClass="img-thumbnail" AlternateText="Ảnh sản phẩm 1" Style="display:none;" />
                                </span>
                                <span>
                                    <asp:Image ID="imageProduct2" runat="server" CssClass="img-thumbnail" AlternateText="Ảnh sản phẩm 2" Style="display:none;" />
                                </span>
                                <span>
                                    <asp:Image ID="imageProduct3" runat="server" CssClass="img-thumbnail" AlternateText="Ảnh sản phẩm 3" Style="display:none;" />
                                </span>
                                <span>
                                    <asp:Image ID="imageProduct4" runat="server" CssClass="img-thumbnail" AlternateText="Ảnh sản phẩm 4" Style="display:none;" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-action pb-4">
                        <div class="text-left">
                            <asp:Button ID="btnAddOrUpdate" runat="server" CssClass="btn btn-info" Text="Thêm" OnClick="btnAddOrUpdate_Click" />
                            <asp:Button ID="btnClear" runat="server" CssClass="btn btn-dark" Text="Xóa form" CausesValidation="false"
                                OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>