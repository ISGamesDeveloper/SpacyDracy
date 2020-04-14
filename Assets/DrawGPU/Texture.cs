using UnityEngine;
using System.Collections;

namespace Custom{
	
	public class Texture : MonoBehaviour {
		
		public Camera textureCamera;
		public MeshRenderer quadRenderer;
		
		public static Custom.Texture Camera;
		
		void Awake(){
			
			Camera = this;
			
		}
		
		public RenderTexture Capture (int width, int height, Material material)
		{
			float scale = (float)width / (float)height;
			Debug.Log("Scale width: " + width);
			Debug.Log("Scale height: " + height);
			Debug.Log("Scale: " + scale);

			var origScale = gameObject.transform.localScale;
			gameObject.transform.localScale = new Vector3(scale,1,1);

			this.quadRenderer.material = material;

			RenderTexture origTexture = textureCamera.targetTexture;
			RenderTexture temp = RenderTexture.GetTemporary(width, height);

			this.textureCamera.targetTexture = temp;
			this.textureCamera.Render();

			textureCamera.targetTexture = origTexture;
			gameObject.transform.localScale = origScale;

			return temp;
		}
		
	}
	
}