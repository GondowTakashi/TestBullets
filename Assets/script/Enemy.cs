using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float speed;
	public float angle;
	public int hp;
	public int cnt;
	public int move_knd;
	public int shoot_knd;	
	public GameObject e_bulletPrefab;
	// Use this for initialization
	void Start () {
		cnt   = 0 ;
	}
	public void First(float x,float y,int hp,float speed,float angle,int move_knd,int shoot_knd) {
		this.transform.position = new Vector3(x,y,0);
		this.hp         = hp;
		this.speed      = speed;
		this.angle      = angle;
		this.move_knd   = move_knd;
		this.shoot_knd  = shoot_knd;
	}
	
	// Update is called once per frame
	void Update () {
		ShootBullet(this.shoot_knd);//弾発射
		Enemy_move(this.move_knd);//移動制御
		cnt++;
	}
	//---------移動定義--------------
	void Enemy_move(int knd){
		Vector3 p = this.transform.position;
		if(speed != 0){
			p.x += speed * Mathf.Cos(angle);
			p.y += speed * Mathf.Sin(angle);
			this.transform.position = p;
		}
		//特別な制御を行う場合
		switch(knd){
			case 0:default:break;//特別な制御を行わない
			case 1://120fおきに反対方向へ方向転換
				if(cnt % 120== 60 ) this.angle -=  3.1415926f ;
				break;
		}
	}
	//------------------------------
	//---------弾幕定義--------------
	//弾幕定義まとめ
	void ShootBullet(int knd){
		switch(knd){
			case 0://180fおきに8方向
				if(cnt % 180== 0 )	ShootBullet_0(8);
				break;
			case 1://5fおきに1f0.4π回転する渦巻
				if(cnt % 5== 0 )	ShootBullet_1(0.4f);
				break;

			default:break;//何も撃たない
		}
	}
	//0:n方向
	void ShootBullet_0(int n){
		for(int i=0;i<8;i++){
			var go = Instantiate( e_bulletPrefab ) as GameObject;
			Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
			//First(float x,float y,float speed,float angle,int knd,int col) 
			e_bullet.First(this.transform.position.x,this.transform.position.y,0.01f,(float)(2*3.1415926 / n * i ),0,0);
		}
	}
	//1:渦巻き(1fでPI*n回転)
	void ShootBullet_1(float n){
		var go = Instantiate( e_bulletPrefab ) as GameObject;
		Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
		//First(float x,float y,float speed,float angle,int knd,int col) 
		e_bullet.First(this.transform.position.x,this.transform.position.y,0.01f,
			(float)(2*3.1415926 / 60 * n * this.cnt ),0,0);
	}
	//------------------------------

	//当たり判定
	void OnTriggerEnter2D(Collider2D collider){
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName (collider.gameObject.layer);
        //自機弾との当たり判定：ダメージを受けて弾を消去
		if (layerName == "bullet_jiki" ){
			Destroy(collider.gameObject);
			this.hp--;
			if(this.hp<=0){
				Destroy(this.gameObject);
			}
        }
	}

}
