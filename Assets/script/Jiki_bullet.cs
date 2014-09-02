using UnityEngine;
using System.Collections;

public class Jiki_bullet : MonoBehaviour {
	public float speed;
	public float angle;
	public int col;
	public int knd;
	// Use this for initialization
	void Start(){}

	public void First(float x,float y,float speed,float angle,int knd,int col) {
		this.transform.position = new Vector3(x,y,0);
		this.speed = speed;
		this.angle = angle;
		this.knd   = knd;
		this.col   = col;
	}
	// Update is called once per frame
	void Update () {
		//移動制御
		Vector3 p = this.transform.position;
		if(speed != 0){
			p.x += speed * Mathf.Cos(angle);
			p.y += speed * Mathf.Sin(angle);
			this.transform.position = p;
		}
	}
	//カメラから不可視になったら
	void OnBecameInvisible(){
	    Destroy(this.gameObject);
    }
}
