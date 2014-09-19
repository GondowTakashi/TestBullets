using UnityEngine;
using System.Collections;

public class FontResize : MonoBehaviour {
	//デフォルト縦サイズ
	float d_height = 400.0f;

	//最初に固定
	void Start () {
		//現在の縦サイズ
		float s_height = Screen.height;
		//比率・大きい方に合わせる
		float resize_rate = s_height / d_height;
		guiText.fontSize =(int)( guiText.fontSize * resize_rate );  
	}
	// Update is called once per frame
	void Update () {}
}