using BackgroundRemovalSample.App;
using OpenCvSharp;
using OpenCvSharp.Demo;
using UnityEngine;
using UnityEngine.UI;

public class RemoveBackgroundIS : MonoBehaviour
{
	public RawImage InputImage;
	public RawImage outputImage;
	private PaperScanner scanner = new PaperScanner();

	[HideInInspector]
	public double FloodFillTolerance = 0.018f;
	public const double FloodFillToleranceDefaultValue = 0.018f;
	public int MaskBlurFactor = 5;
	public Texture2D ProcessedTexture;
	public bool Processed;

	public Texture2D RemoveBackgroundOnTexture(Texture2D texture)
	{
		InputImage.texture = texture;
		//texture = Init(texture);//убрать потом

		outputImage.texture = Run(OpenCvSharp.Unity.TextureToMat(texture));
		ProcessedTexture = outputImage.texture as Texture2D;

		Processed = true;

		return ProcessedTexture;
	}

	public Texture2D ChangeValueOnFloodFillTolerance(Texture2D texture, float value)
	{
		Processed = false;
		FloodFillTolerance = value;
		outputImage.texture = Run(OpenCvSharp.Unity.TextureToMat(texture));

		ProcessedTexture = outputImage.texture as Texture2D;
		Processed = true;

		return ProcessedTexture;
	}

	public Texture2D Run(Mat mat)
	{
		var filter = new RemoveBackgroundOpenCvFilter
		{
			FloodFillTolerance = FloodFillTolerance,//0.04
			MaskBlurFactor = MaskBlurFactor//5
		};

		return OpenCvSharp.Unity.MatToTexture(filter.Apply(mat));
	}

	public void Scanner(Mat material)
	{
		scanner.Settings.NoiseReduction = 0.7;//0.7;
		scanner.Settings.EdgesTight = 0.9;//0.9;
		scanner.Settings.ExpectedArea = 0.2;// 0.2;
		scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.Grayscale;

		scanner.Input = material;

		scanner.Settings.GrayMode = PaperScanner.ScannerSettings.ColorMode.HueGrayscale;
		outputImage.texture = OpenCvSharp.Unity.MatToTexture(scanner.Output);
	}

	public Texture2D Init(Texture2D texture)
	{
		Scanner(OpenCvSharp.Unity.TextureToMat(texture));
		return outputImage.texture as Texture2D;
	}
}