using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {

	//public RenderTexture renderTexture;	
	public Rect rect1 = new Rect(0f,0f,200f,200f);
	public Rect rect2 = new Rect(0f,201f,200f,200f);

	public Material material1;
	public Material material2;

	Texture tex1, tex2;

	//void Start(){

	//	this.tex1 = Custom.Texture.Camera.Capture(512, 512, this.material1);
	//	this.tex2 = Custom.Texture.Camera.Capture(512, 512, this.material2);

	//}
		
	//void OnGUI() {
		
	//	GUI.DrawTexture(this.rect1, this.tex1);
	//	GUI.DrawTexture(this.rect2, this.tex2);
		
	//}

}
