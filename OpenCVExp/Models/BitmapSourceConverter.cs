using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace OpenCVExp.Models
{
    /// <summary>
    /// 可用在 .Net Core 的程式中的 BitmapSourceConverter
    /// </summary>
    public class BitmapSourceConverter
    {
        public static BitmapSource MatToBitmapSource(Mat mat)
        {
            byte[] buffer = mat.ImEncode(".png");
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = new MemoryStream(buffer);
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.EndInit();
            return bitmapImage;
        }
        public static Mat BitmapSourceToMat(BitmapSource src)
        {
            return StreamToMat(BitmapSourceToStream(src));
        }
        public static Mat StreamToMat(Stream stream)
        {
            return Mat.FromStream(stream, ImreadModes.AnyColor);
        }
        public static MemoryStream BitmapSourceToStream(BitmapSource src)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(src));
            MemoryStream ms = new MemoryStream();
            encoder.Save(ms);
            ms.Position = 0;
            return ms;
        }
        public static void SaveBitmapSource(BitmapSource src, string filepath)
        {
            Mat mat = BitmapSourceToMat(src);
            Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2RGB);
            mat.SaveImage(filepath);
        }

    }
}
