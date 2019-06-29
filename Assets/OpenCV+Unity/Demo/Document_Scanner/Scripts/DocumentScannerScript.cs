namespace OpenCvSharp.Demo
{
	using UnityEngine;

	public class DocumentScannerScript : MonoBehaviour
	{
		//public Texture2D inputTexture;
		//public Texture2D outputTexture;

		//private Mat CombineMats(Mat original, Mat processed, Point[] detectedContour)
		//{
		//	Size inputSize = new Size(original.Width, original.Height);

		//	var matCombined = new Mat(new Size(inputSize.Width * 2, inputSize.Height), original.Type(), Scalar.FromRgb(64, 64, 64));

		//	original.CopyTo(matCombined.SubMat(0, inputSize.Height, 0, inputSize.Width));
		//	if (null != detectedContour && detectedContour.Length > 2)
		//		matCombined.DrawContours(new Point[][] { detectedContour }, 0, Scalar.FromRgb(255, 255, 0), 3);

		//	if (null != processed)
		//	{
		//		double hw = processed.Width * 0.5, hh = processed.Height * 0.5;
		//		Point2d center = new Point2d(inputSize.Width + inputSize.Width * 0.5, inputSize.Height * 0.5);
		//		Mat roi = matCombined.SubMat(
		//			(int)(center.Y - hh), (int)(center.Y + hh),
		//			(int)(center.X - hw), (int)(center.X + hw)
		//		);
		//		processed.CopyTo(roi);
		//	}

		//	return matCombined;
		//}

	}
}