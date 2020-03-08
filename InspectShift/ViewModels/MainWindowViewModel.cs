using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.IO;
using System.Windows.Media.Imaging;
using Yh.Core;

namespace InspectShift.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
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
            set { 
                SetProperty(ref files, value); 
                if (files.Length > 0)
                {
                    SelectedFile = files[0];
                }
            }
        }
        private BitmapSource bitmapSrc;
        public BitmapSource BitmapSrc
        {
            get { return bitmapSrc; }
            set { SetProperty(ref bitmapSrc, value); }
        }
        private string selectedFile;
        public string SelectedFile
        {
            get { return selectedFile; }
            set { 
                SetProperty(ref selectedFile, value); 
                if (File.Exists(selectedFile))
                {
                    Mat mat = Utilities.getCvImage(selectedFile, ImreadModes.Color);
                    BitmapSrc = BitmapSourceConverter.MatToBitmapSource(mat);
                    //BitmapSrc = new BitmapImage(new Uri(selectedFile)); not work in .Core
                    //Mat bgrImage = BitmapSourceConverter.BitmapSourceToMat(BitmapSrc);
                    Cv2.CvtColor(mat, GrayMat, ColorConversionCodes.BGR2GRAY);
                    var opencvPoints = Utilities.GetLargestQuadrilateral(grayMat);
                    System.Windows.Media.PointCollection points = new System.Windows.Media.PointCollection();
                    foreach (var p in opencvPoints)
                    {
                        points.Add(new System.Windows.Point(p.X, p.Y));
                    }
                    Polygon = points;
                    Scale = BitmapSrc.DpiX / 96.0;
                }
            }
        }
        private Mat grayMat=new Mat();
        public Mat GrayMat
        {
            get { return grayMat; }
            set { SetProperty(ref grayMat, value); }
        }

        private double scale;
        public double Scale
        {
            get { return scale; }
            set { SetProperty(ref scale, value); }
        }

        private System.Windows.Media.PointCollection polygon;
        public System.Windows.Media.PointCollection Polygon
        {
            get { return polygon; }
            set { SetProperty(ref polygon, value); }
        }
        public MainWindowViewModel()
        {

        }
    }
}
