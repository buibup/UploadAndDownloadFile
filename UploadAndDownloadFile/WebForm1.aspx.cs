using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UploadAndDownloadFile
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                FileUpload1.PostedFile.SaveAs(Server.MapPath($"~/Data/{FileUpload1.FileName}"));
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("File", typeof(string));
            dt.Columns.Add("Size", typeof(string));
            dt.Columns.Add("Type", typeof(string));

            foreach (var strFile in Directory.GetFiles(Server.MapPath($"~/Data")))
            {
                FileInfo fi = new FileInfo(strFile);

                dt.Rows.Add(fi.Name, fi.Length, GetFileTypeByExtension(fi.Extension));
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }

        private string GetFileTypeByExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".doc":
                case ".docx":
                    return "Microsoft Word Document";
                case ".xlsx":
                case ".xls":
                    return "Microsoft Excel Document";
                case ".txt":
                    return "Text Document";
                case ".jpg":
                case ".png":
                    return "Image";
                default:
                    return "Unknown";
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName == "Download")
            {
                Response.Clear();
                Response.ContentType = "application/octect-stream";
                Response.AppendHeader("content-disposition", $"filename={e.CommandArgument}");
                Response.TransmitFile(Server.MapPath($"~/Data/{e.CommandArgument}"));
                Response.End();
            }
        }
    }
}