using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace QuanLyCuaHangLiderCloset
{
    public class ProductObj { 
        public int ProductId { get; set; } 
        public string ProductName { get; set; } 
        public string ShortDescription { get; set; } 
        public string LongDescription { get; set; } 
        public string AdditionalDescription { get; set; } 
        public decimal Price { get; set; } 
        public int Quantity { get; set; } 
        public string Size { get; set; } 
        public string Color { get; set; } 
        public string CompanyName { get; set; } 
        public int CategoryId { get; set; } 
        public int SubCategoryId { get; set; } 
        public bool IsCustomized { get; set; } 
        public bool IsActive { get; set; } 
        public int Sold { get; set; } 
        public DateTime NgayCapNhat { get; set; }
        public List<ProductImageObj> ProductImages { get; set; } = new List<ProductImageObj>();
        
        public int DefaultImagePosition { get; set; } }

    public class ProductImageObj
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public int ProductId { get; set; }
        public bool DefaultImage { get; set; }
    }

    public class ProductDAL
    {
        private readonly DataAccess _da = new DataAccess();

        public int AddUpdateProduct(ProductObj product)
        {
            string imageUrls = product.ProductImages != null && product.ProductImages.Any()
                ? string.Join(",", product.ProductImages.Select(img => img.ImageUrl))
                : null;
            int anhMacDinh = product.DefaultImagePosition > 0
                ? product.DefaultImagePosition
                : product.ProductImages?.FindIndex(img => img.DefaultImage) + 1 ?? 1;

            SqlParameter[] parameters =
            {
            new SqlParameter("@Action", product.ProductId == 0 ? "INSERT" : "UPDATE"),
            new SqlParameter("@MaSP", product.ProductId),
            new SqlParameter("@TenSP", product.ProductName ?? (object)DBNull.Value),
            new SqlParameter("@MaHMC", product.SubCategoryId),
            new SqlParameter("@MaHangMuc", product.CategoryId),
            new SqlParameter("@DonGia", product.Price),
            new SqlParameter("@SoLuong", product.Quantity),
            new SqlParameter("@MauSac", product.Color ?? (object)DBNull.Value),
            new SqlParameter("@Size", product.Size ?? (object)DBNull.Value),
            new SqlParameter("@TenNCC", product.CompanyName ?? (object)DBNull.Value),
            new SqlParameter("@MoTaNgan", product.ShortDescription ?? (object)DBNull.Value),
            new SqlParameter("@MoTaDai", product.LongDescription ?? (object)DBNull.Value),
            new SqlParameter("@MoTaBoSung", product.AdditionalDescription ?? (object)DBNull.Value),
            new SqlParameter("@isActive", product.IsActive),
            new SqlParameter("@isCustomized", product.IsCustomized),
            new SqlParameter("@DaBan", product.Sold),
            new SqlParameter("@ImageUrls", (object)imageUrls ?? DBNull.Value),
            new SqlParameter("@AnhMacDinh", anhMacDinh)
        };

            return _da.ExecuteNonQuery("Product_Crud", parameters);
        }

        public DataTable ProductByIdWithImages(int productId)
        {
            SqlParameter[] parameters =
            {
            new SqlParameter("@Action", "GETBYID"),
            new SqlParameter("@MaSP", productId)
        };
            DataTable dt = _da.ExecuteQuery("Product_Crud", parameters);

            dt.Columns.Add("Image1");
            dt.Columns.Add("Image2");
            dt.Columns.Add("Image3");
            dt.Columns.Add("Image4");
            dt.Columns.Add("DefaultImage");

            if (dt.Rows.Count > 0)
            {
                string imageUrls = dt.Rows[0]["ImageUrls"].ToString();
                if (!string.IsNullOrEmpty(imageUrls))
                {
                    string[] images = imageUrls.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (images.Length > 0) row["Image1"] = images[0];
                        if (images.Length > 1) row["Image2"] = images[1];
                        if (images.Length > 2) row["Image3"] = images[2];
                        if (images.Length > 3) row["Image4"] = images[3];
                        row["DefaultImage"] = dt.Rows[0]["AnhMacDinh"].ToString();
                    }
                }
            }

            return dt;
        }

        public bool CheckProductNameExists(string tenSP, int maHMC, int maSP)
        {
            SqlParameter[] parameters =
            {
            new SqlParameter("@Action", "CHECKNAME"),
            new SqlParameter("@TenSP", tenSP),
            new SqlParameter("@MaHMC", maHMC),
            new SqlParameter("@MaSP", maSP == 0 ? (object)DBNull.Value : maSP)
        };
            DataTable dt = _da.ExecuteQuery("Product_Crud", parameters);
            return Convert.ToInt32(dt.Rows[0]["Count"]) > 0;
        }
    }
}