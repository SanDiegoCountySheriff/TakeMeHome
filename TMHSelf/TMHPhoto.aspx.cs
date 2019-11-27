using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace SDSD.NET.TMH
{
    // this class receives the guid for the photo in a request parameter and
    // then displays the photo. This code works for jpg, jpeg, and gif formats.

    public partial class TMHPhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Guid Guid1 = new Guid(Request["data"]);
                DataSet1TableAdapters.TMHFilesTableAdapter taTMHFiles = new DataSet1TableAdapters.TMHFilesTableAdapter();
                DataSet1.TMHFilesDataTable dtTMHFiles = taTMHFiles.GetDataBy(Guid1);

                if (dtTMHFiles.Rows.Count > 0)
                {
                    // define a memorystream object
                    MemoryStream ms = new MemoryStream();
                    // write the bits into the ms object
                    ms.Write(dtTMHFiles[0].FileImage, 0, dtTMHFiles[0].FileImage.Length);
                    // create a bitmap object from the bytes we hold, need an intermediate
                    // MemoryStream object to call the constructor
                    Bitmap bmp = new Bitmap(ms);
                    // set the page's content type to the image format
                    Response.ContentType = "image/jpeg"; // "image/gif"
                    // send the page's output stream as a graphics object
                    // convert the format-agnostic Bitmap object into the expected
                    // image format and serialize the resulting object to the page's output stream.
                    bmp.Save(Response.OutputStream, ImageFormat.Jpeg); // ImageFormat.Gif
                    bmp = null;
                    ms.Close();
                    ms.Flush();
                    taTMHFiles.Connection.Close();
                    taTMHFiles.Dispose();
                    base.Dispose();
                }
            }
            
            catch (Exception ex)
            {
                string strError = ex.Message.ToString();
            }
        }
    }
}
