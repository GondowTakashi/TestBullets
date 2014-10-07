using UnityEngine;
using System.Collections;
using Common;

public class PointItem : MonoBehaviour {
	// Use this for initialization
	void Start () {}

	public void First(float x,float y) {
		x += -0.25f+0.5f*Random.value;
		y += 		0.5f*Random.value;
		this.transform.position = new Vector3(x,y,0);
	}
	
	// Update is called once per frame
	void Update () {
		//移動制御は重力で行っている
	}

}
