using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.SessionState;
using System.IO;
using System.Web.UI.WebControls;

namespace QuanLyCuaHangLiderCloset
{
   
    public class Utils
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataTable dt;
        SqlDataReader reader;

        public static string getConnection()
        {
            return ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        }
        public static bool isValidExetension(string fileName)
        {
            bool isValid = false;
            string[] fileExtension = { ".jpg", ".png", ".jpeg" };
            foreach (string file in fileExtension)
            {
                if (fileName.Contains(file))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }
        public static string getUniqueId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public static string getImageUrl(Object url)
        {
            string url1 = string.Empty;
            if(string.IsNullOrEmpty(url.ToString()) || url == DBNull.Value)
            {
                url1 = "../Images/No_Image.png";
            }
            else
            {
                url1 = string.Format("../{0}", url);
            }
            return url1;
        }
        public static string[] getImagesPath(string[] images)
        {
            List<string> list = new List<string>();
            string fileExtension = string.Empty;
            for (int i = 0; i < images.Length - 1; i++)
            {
                fileExtension = Path.GetExtension(images[i]);
                list.Add("Images/Product/" + getUniqueId().ToString() + fileExtension);
            }
            return list.ToArray();
        }

        public static string getItemWithCommaSeperator(ListBox listBox)
        {
            string selectedItem = string.Empty;
            foreach (var item in listBox.GetSelectedIndices())
            {
                selectedItem += listBox.Items[item].Text + ",";
            }
            return selectedItem;
        }
    }
}