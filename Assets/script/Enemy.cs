using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float speed;
	public float angle;
	public int hp;
	public int cnt;
	public GameObject e_bulletPrefab;
	// Use this for initialization
	void Start () {
		hp    = 100 ;
		speed = 0.01f ;
		angle = 3.1415926f ;
		cnt   = 0 ;
	}
	
	// Update is called once per frame
	void Update () {
		//--------------弾発射--------------
//		if(cnt % 180== 0 )	ShootBullet0(8);
		if(cnt % 5== 0 )	ShootBullet1(0.4f,cnt);
		
		//---------------------------------
		//------------基本移動--------------
		Vector3 p = this.transform.position;
		if(speed != 0){
			p.x += speed * Mathf.Cos(angle);
			p.y += speed * Mathf.Sin(angle);
			this.transform.position = p;
		}
		//---------------------------------
		//移動制御
		if(cnt % 120== 60 ) this.angle -=  3.1415926f ;
		cnt++;
	}
	//---------弾幕定義--------------
	//0:n方向
	void ShootBullet0(int n){
		for(int i=0;i<8;i++){
			var go = Instantiate( e_bulletPrefab ) as GameObject;
			Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();

			e_bullet.First(this.transform.position.x,this.transform.position.y,0.01f,(float)(2*3.1415926 / n * i ),0,0);
		}
	}
	//1:渦巻き(1fでPI*n回転)
	void ShootBullet1(float n,int cnt){
		var go = Instantiate( e_bulletPrefab ) as GameObject;
		Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();

		e_bullet.First(this.transform.position.x,this.transform.position.y,0.01f,
			(float)(2*3.1415926 / 60 * n * cnt ),0,0);
	}
	//-------------------------------


	//自機弾との当たり判定
	void OnTriggerEnter2D(Collider2D collider){
		Destroy(collider.gameObject);
		this.hp--;
		if(this.hp<=0)	
			Destroy(this.gameObject);
	}

}
