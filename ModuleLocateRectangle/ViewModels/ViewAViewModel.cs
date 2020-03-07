using OpenCvSharp;
using OpenCvSharp.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ModuleLocateRectangle.ViewModels
{
    public class ViewAViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        private string[] files;
        public string[] Files
        {
            get { return files; }
            set { SetProperty(ref files, value); }
        }
        private BitmapSource bitmapSrc;
        public BitmapSource BitmapSrc
        {
            get { return bitmapSrc; }
            set { SetProperty(ref bitmapSrc, value); }
        }
        private double scale;
        public double Scale
        {
            get { return scale; }
            set { SetProperty(ref scale, value); }
        }
        private string selectedFile;
        public string SelectedFile
        {
            get { return selectedFile; }
            set
            {
                SetProperty(ref selectedFile, value);
                if (File.Exists(selectedFile))
                {
                    BitmapSrc = new BitmapImage(new Uri(selectedFile));
                    Mat bgrImage = BitmapSourceConverter.ToMat(BitmapSrc);
                    Cv2.CvtColor(bgrImage, GrayMat, ColorConversionCodes.BGR2GRAY);
                    LargestRect= Yh.OpenCV.Misc.GetLargestRectangle(grayMat);
                    Scale = BitmapSrc.DpiX / 96.0;
                    Left = LargestRect.Left;
                    Top = LargestRect.Top;
                    Width = LargestRect.Width;
                    Height = LargestRect.Height;
                }
            }
        }
        private Rect largestRect=new Rect(10,20,100,100);
        public Rect LargestRect
        {
            get { return largestRect; }
            set { SetProperty(ref largestRect, value); }
        }
        private int left;
        public int Left
        {
            get { return left; }
            set { SetProperty(ref left, value); }
        }
        private int top;
        public int Top
        {
            get { return top; }
            set { SetProperty(ref top, value); }
        }
        private int width;
        public int Width
        {
            get { return width; }
            set { SetProperty(ref width, value); }
        }
        private int height;
        public int Height
        {
            get { return height; }
            set { SetProperty(ref height, value); }
        }
        private Mat grayMat=new Mat();
        public Mat GrayMat
        {
            get { return grayMat; }
            set { SetProperty(ref grayMat, value); }
        }

        public ViewAViewModel()
        {
            Message = "View A from your Prism Module";
        }
    }
}
