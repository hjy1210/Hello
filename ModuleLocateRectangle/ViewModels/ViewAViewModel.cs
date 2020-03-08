using OpenCvSharp;
using OpenCvSharp.Extensions;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
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
            set { 
                SetProperty(ref files, value);
                if (files.Length >0)
                    SelectedFile = files[0];
            }
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
                    //LargestRect= Yh.OpenCV.Misc.GetLargestRectangle(grayMat);
                    var opencvPoints = Yh.OpenCV.Misc.GetLargestQuadrilateral(grayMat);
                    // Construct Polygon1:System.Windows.Point[] from opencvPoints
                    var tmpPolygon1 = new System.Windows.Point[opencvPoints.Length];
                    for (int i = 0; i < opencvPoints.Length; i++)
                    {
                        tmpPolygon1[i] = new System.Windows.Point(opencvPoints[i].X, opencvPoints[i].Y);
                    }
                    Polygon1 = tmpPolygon1;
                    // Construct Polygon2:PointCollection from opencvPoints
                    PointCollection points = new PointCollection();
                    foreach (var p in Polygon1)
                    {
                        //Polygon2.Add(new System.Windows.Point(p.X, p.Y));
                        points.Add(new System.Windows.Point(p.X, p.Y));
                    }
                    Polygon2 = points;
                    // Construct Polygon:ObservableCollection<new System.Windows.Point> from opencvPoints
                    Polygon.Clear();
                    foreach (var x in Polygon1)
                        Polygon.Add(x);
                    Scale = BitmapSrc.DpiX / 96.0;
                    Left = LargestRect.Left;
                    Top = LargestRect.Top;
                    Width = LargestRect.Width;  //LargestRect.Width can not use as binding source
                    Height = LargestRect.Height;//LargestRect.Height can not use as binding source
                }
            }
        }
        private Rect largestRect;
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
        private System.Windows.Point[] polygon1;
        public System.Windows.Point[] Polygon1
        {
            get { return polygon1; }
            set { SetProperty(ref polygon1, value); }
        }
        /// <summary>
        /// Polygon 的 Points 必須繫結到 PointCollection，不能繫結到 System.Windows.Point[]，也不能繫結到 ObservableCollection<System.Windows.Point>
        /// PointCollection 由 System.Windows.Point而非 OpenCvSharp.Point 所組成
        /// </summary>
        private PointCollection polygon2 = new PointCollection();
        //new PointCollection{new System.Windows.Point(200,200), new System.Windows.Point(100,500), new System.Windows.Point(400,300)};
        public PointCollection Polygon2
        {
            get { return polygon2; }
            set { SetProperty(ref polygon2, value); }
        }
        private ObservableCollection<System.Windows.Point> polygon=new ObservableCollection<System.Windows.Point>
        {
            new System.Windows.Point(1816,149), new System.Windows.Point(1817,3167), new System.Windows.Point(3167,2165), new System.Windows.Point(3167,147)
        };
        public ObservableCollection<System.Windows.Point> Polygon
        {
            get { return polygon; }
            set { SetProperty(ref polygon, value); }
        }
        public ViewAViewModel()
        {
            Message = "View A from your Prism Module";
        }
    }
}
