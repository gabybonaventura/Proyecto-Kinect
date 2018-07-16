using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace EmguCV_ColorTracking
{
    public static class Helper
    {
        public static VectorOfVectorOfPoint FindContours(this Image<Gray, byte> image, ChainApproxMethod method = ChainApproxMethod.ChainApproxSimple,
                Emgu.CV.CvEnum.RetrType type = RetrType.List)
        {
            // Check that all parameters are valid.
            VectorOfVectorOfPoint result = new VectorOfVectorOfPoint();

            if (method == Emgu.CV.CvEnum.ChainApproxMethod.ChainCode)
            {
                throw new ColsaNotImplementedException("Chain Code not implemented, sorry try again later");
            }

            CvInvoke.FindContours(image, result, null, type, method);
            return result;
        }
    }
}
