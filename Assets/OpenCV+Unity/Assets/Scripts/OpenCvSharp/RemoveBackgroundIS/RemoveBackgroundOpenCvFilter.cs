using System;
using System.Collections.Generic;
using OpenCvSharp;
using UnityEngine;
using Rect = OpenCvSharp.Rect;

namespace BackgroundRemovalSample.App
{

	///     Makes background transparent on a given image using edge detection.

	public class RemoveBackgroundOpenCvFilter : OpenCvFilter
	{
		private const Double FloodFillRelativeSeedPoint = 0.02;
		private       Double _floodFillTolerance        = 0.01;

		///     Tolerance of flood fill applied to mask.

		public Double FloodFillTolerance
		{
			get => _floodFillTolerance;
			set
			{
				if (value > 1)
					throw new ArgumentException("Flood fill tolerance should be less then 1.");
				if (value < 0)
					throw new ArgumentException("Flood fill tolerance should be greater than or equal to zero.");

				_floodFillTolerance = value;
			}
		}

		///     Amount of blur applied to the transparency mask on a final stage.

		public Int32 MaskBlurFactor { get; set; } = 5;

		public override IEnumerable<MatType> SupportedMatTypes => new[] {MatType.CV_8UC3, MatType.CV_8UC4};

		public Texture2D globalAlphaMask, globalAlphaMask2;

		public Texture2D SetMask()
		{
			return globalAlphaMask;
		}
		public Texture2D SetMask2()
		{
			return globalAlphaMask2;
		}
		protected override void ProcessFilter(Mat src, Mat dst)
		{
			FastFilter(src, dst);
			return;

			globalAlphaMask = Texture2D.whiteTexture;
			using (Mat alphaMask = GetGradient(src))
			{
				// Performs morphology operation on alpha mask with resolution-dependent element size
				void PerformMorphologyEx(MorphTypes operation, Int32 iterations)
				{
					Double elementSize = Math.Sqrt(alphaMask.Width * alphaMask.Height) / 300;
					//UnityEngine.Debug.Log("elementSize: " + elementSize);
					//UnityEngine.Debug.Log("iterations: " + iterations);
					elementSize = 3;//раскомментить нижние 4 строчки. и закоментить эту (если вернуть все так, как было раньше).
					//if (elementSize < 2)
					//	elementSize = 2;

					//if (elementSize > 30)
					//	elementSize = 30;

					using (var se = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(elementSize, elementSize)))
					{
						Cv2.MorphologyEx(alphaMask, alphaMask, operation, se, null, iterations);
					}
				}

				PerformMorphologyEx(MorphTypes.DILATE, 1); // Close small gaps in edges

				Cv2.FloodFill( // Flood fill outer space
					image: alphaMask,
					seedPoint: new Point(
						(Int32) (FloodFillRelativeSeedPoint * src.Width),
						(Int32) (FloodFillRelativeSeedPoint * src.Height)),
					newVal: new Scalar(0),
					rect: out Rect _,
					loDiff: new Scalar(FloodFillTolerance),
					upDiff: new Scalar(FloodFillTolerance),
					flags: FloodFillFlags.FixedRange | FloodFillFlags.Link4);

				PerformMorphologyEx(MorphTypes.ERODE, 1); // Compensate initial dilate
				PerformMorphologyEx(MorphTypes.Open,  2); // Remove not filled small spots (noise)
				PerformMorphologyEx(MorphTypes.ERODE, 1); // Final erode to remove white fringes/halo around objects

				Cv2.Threshold(
					src: alphaMask,
					dst: alphaMask,
					thresh: 0,
					maxval: 255,
					type: ThresholdTypes.Binary); // Everything non-filled becomes white

				alphaMask.ConvertTo(alphaMask, MatType.CV_8UC1, 255);

				// Последние поправки для закрашивания линий
				Cv2.CvtColor(src, src, ColorConversionCodes.BGR2GRAY);
				Cv2.AdaptiveThreshold(src, alphaMask, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 11, 2);
				//

				if (MaskBlurFactor > 0)
					Cv2.GaussianBlur(alphaMask, alphaMask, new Size(MaskBlurFactor, MaskBlurFactor), MaskBlurFactor);

				FillTexture(alphaMask, dst); //для обесцвечивания изобржаений
				//AddAlphaChannel(src, dst, alphaMask); //для цветных изобржаений, но закоментить FillTexture //Временно отключил, потом включить!
			}
		}

		public void FastFilter(Mat src, Mat dst)
		{
			Cv2.CvtColor(src, src, ColorConversionCodes.BGR2GRAY);
			src.ConvertTo(src, -1, 1.2); //увеличение контраста

			

			//тесты
			Mat newMask = new Mat(src.Cols, src.Rows, MatType.CV_8UC4);
			Cv2.Threshold(src, newMask, 100, 255, ThresholdTypes.BinaryInv);
			globalAlphaMask = OpenCvSharp.Unity.MatToTexture(newMask);
			Cv2.AdaptiveThreshold(src, dst, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 31, 20);
			globalAlphaMask2 = OpenCvSharp.Unity.MatToTexture(dst);
			Cv2.AddWeighted(newMask, 1, dst, 1, 0, dst); //соединение двух материалов
			//

			//Cv2.AdaptiveThreshold(src, dst, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 31, 20); //оно!!!
			//Cv2.AdaptiveThreshold(src, dst, 255, AdaptiveThresholdTypes.GaussianC, ThresholdTypes.BinaryInv, 11, 5);
			//Cv2.GaussianBlur(dst, dst, new Size(5, 5), 5);

			var elementSize = 3;
			//PerformMorphologyEx(dst, new Size(elementSize, elementSize), MorphTypes.ERODE, 1);
			//PerformMorphologyEx(dst, new Size(elementSize, elementSize), MorphTypes.Open, 2);//было 2
			//PerformMorphologyEx(dst, new Size(elementSize, elementSize), MorphTypes.ERODE, 1);
			
			FillTexture(dst, dst); //потом раскоментить

			//Cv2.Resize(dst,dst,new Size(208,208));
		}

		void PerformMorphologyEx(Mat mat, Size size, MorphTypes operation, Int32 iterations)
		{
			using (var se = Cv2.GetStructuringElement(MorphShapes.Ellipse, size))
			{
				Cv2.MorphologyEx(mat, mat, operation, se, null, iterations);
			}
		}

		private void FillTexture(Mat alphaMask, Mat dst)
		{
			Mat alpha = new Mat(alphaMask.Cols, alphaMask.Rows, MatType.CV_8UC4);
			Cv2.Threshold(alphaMask, alpha, 100, 255, ThresholdTypes.Binary);
			//Cv2.AdaptiveThreshold(alphaMask, alpha, 255, AdaptiveThresholdTypes.MeanC, ThresholdTypes.BinaryInv, 31, 20);
			var bgr = Cv2.Split(alpha);
			Mat[] bgra = { bgr[0], bgr[0], bgr[0], alpha };
			Cv2.Merge(bgra, dst);

			//globalAlphaMask = OpenCvSharp.Unity.MatToTexture(dst);
		}

		///  Adds transparency channel to source image and writes to output image.
	
		private static void AddAlphaChannel(Mat src, Mat dst, Mat alpha)
		{
			var bgr  = Cv2.Split(src);
			var bgra = new[] { bgr[0], bgr[1], bgr[2], alpha};
			Cv2.Merge(bgra, dst);
		}

		///     Performs edges detection. Result will be used as base for transparency mask.

		private Mat GetGradient(Mat src)
		{
			using (var preparedSrc = new Mat())
			{
				Cv2.CvtColor(src, preparedSrc, ColorConversionCodes.BGR2GRAY);
				preparedSrc.ConvertTo(preparedSrc, MatType.CV_32FC1, 1.0 / 255);

				// Calculate Sobel derivative with kernel size depending on image resolution
				Mat Derivative(Int32 dx, Int32 dy)
				{
					Int32 resolution = preparedSrc.Width * preparedSrc.Height;
					Int32 kernelSize =
						resolution < 1280 * 1280 ? 3 : // Larger image --> larger kernel
						resolution < 2000 * 2000 ? 5 :
						resolution < 3000 * 3000 ? 9 :
						                           15;
					Single kernelFactor = kernelSize == 3 ? 1 : 2; // Compensate lack of contrast on large images

					using (var kernelRows = new Mat())
					using (var kernelColumns = new Mat())
					{
						// Get normalized Sobel kernel of desired size
						Cv2.GetDerivKernels(kernelRows, kernelColumns, dx, dy, kernelSize, normalize: true);

						using (var multipliedKernelRows = kernelRows * kernelFactor)
						using (var multipliedKernelColumns = kernelColumns * kernelFactor)
						{
							return preparedSrc.SepFilter2D(MatType.CV_32FC1, multipliedKernelRows, multipliedKernelColumns);
						}
					}
				}

				using (var gradX = Derivative(1, 0))
				using (var gradY = Derivative(0, 1))
				{
					var result = new Mat();
					Cv2.Magnitude(gradX, gradY, result);

					result += 0.15f; //Add small constant so the flood fill will perform correctly
					return result;
				}
			}
		}
	}
}