using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yh.OpenCV
{
    public class Misc
    {
        public static Rect GetLargestRectangle(Mat grayMat)
        {
            Mat blur = new Mat();
            grayMat.CopyTo(blur);
            Cv2.GaussianBlur(blur, blur, new Size(5, 5), 0);
            Mat binary = new Mat();
            double otsu=Cv2.Threshold(blur, binary, 200, 255, ThresholdTypes.BinaryInv| ThresholdTypes.Otsu);
            Cv2.FindContours(binary, out Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            List<Rect> rects = new List<Rect>();
            contours=(from c in contours where Cv2.ContourArea(c,false)>100 select c).ToArray();
            Rect rect= new Rect(0, 0, 0, 0);

            if (contours.Length > 0)
            {
                double largestArea=Cv2.ContourArea(contours[0], false);
                rect = Cv2.BoundingRect(contours[0]);
                for (int i = 0; i < contours.Length; i++)
                {
                    double area = Cv2.ContourArea(contours[i], false);
                    if (area > largestArea)
                    {
                        rect= Cv2.BoundingRect(contours[i]);
                        largestArea = area;
                    }
                }
            }
            return rect;
        }
        public static Point[] GetLargestQuadrilateral(Mat grayMat,out Mat binary, out Mat contourMat)
        {
            Mat blur = new Mat();
            grayMat.CopyTo(blur);
            Cv2.GaussianBlur(blur, blur, new Size(5, 5), 0);
            binary = new Mat();
            double otsu = Cv2.Threshold(blur, binary, 200, 255, ThresholdTypes.BinaryInv | ThresholdTypes.Otsu); //ThresholdTypes.BinaryInv | ThresholdTypes.Otsu 並沒有比較好?
            //Mat kernel = new Mat(new Size(3, 3), MatType.CV_8U, 1);
            //Cv2.Dilate(binary, binary, kernel, null, 4);
            //Cv2.Erode(binary, binary, kernel, null, 4);
            Mat kernel = new Mat(new Size(9, 9), MatType.CV_8U, 1);
            Cv2.MorphologyEx(binary, binary, MorphTypes.Close, kernel);
            //Cv2.ImShow("binary", binary);
            Cv2.FindContours(binary, out Point[][] contours, out HierarchyIndex[] hierarchy, RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            List<Rect> rects = new List<Rect>();
            contours = (from c in contours where Cv2.ContourArea(c, false) > 10000 select c).ToArray();
            contourMat = new Mat(grayMat.Size(), MatType.CV_8U, 0);
            List<Point[]> qualifiedContours = new List<Point[]>();
            for (int i = 0; i < contours.Length; i++)
            {

                if (IsPseudoQuadrilateral(contours[i], out Point[] quad))
                {
                    qualifiedContours.Add(quad);
                }
            }
            Cv2.DrawContours(contourMat, qualifiedContours, -1, 255);
            //Cv2.Rotate(contourMat, contourMat, RotateFlags.Rotate90Clockwise);
            Point[] polygon = new Point[0];
            if (qualifiedContours.Count > 0)
            {
                double largestArea = Cv2.ContourArea(qualifiedContours[0], false);
                polygon = qualifiedContours[0];
                for (int i = 1; i < qualifiedContours.Count; i++)
                {
                    double area = Cv2.ContourArea(qualifiedContours[i], false);
                    if (area > largestArea)
                    {
                        polygon = qualifiedContours[i];
                        largestArea = area;
                    }
                }
            }
            return polygon;
        }
        static bool IsPseudoQuadrilateral(Point[] points, out Point[] quad)
        {
            quad = null;
            if (Cv2.ContourArea(points, false) < 100)
            {
                return false;
            }
            double peri = Cv2.ArcLength(points, true);
            Point[] fourPoints=Cv2.ApproxPolyDP(points, 0.03 * peri, true);
            if (fourPoints.Length == 4)
            {
                quad = fourPoints;
                return true;
            }
            return false;
        }
    }
}
