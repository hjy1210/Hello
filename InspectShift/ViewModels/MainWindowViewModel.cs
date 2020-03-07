using OpenCvSharp;
using Prism.Mvvm;
using System;
using System.IO;
using System.Windows.Media.Imaging;

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
            set { SetProperty(ref files, value); }
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
                    BitmapSrc = new BitmapImage(new Uri(selectedFile));
                }
            }
        }
        private Mat grayMat;
        public Mat GrayMat
        {
            get { return grayMat; }
            set { SetProperty(ref grayMat, value); }
        }
        public MainWindowViewModel()
        {

        }
    }
}
