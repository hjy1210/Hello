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
    }
}
