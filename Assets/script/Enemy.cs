using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	private float speed;
	private float angle;
	private int hp;
	private int cnt;
	private int move_knd;
	private int shoot_knd;
	private bool stop_flag;
	public GameObject e_bulletPrefab;
	public GameObject enemy_hp_Prefab;
	public const float PI = 3.1415926f;
//	GameManager game;

	// Use this for initialization
	void Start () {
//		game = GetComponent(typeof(GameManager)) as GameManager;
		cnt   = 0 ;

	}
	public void First(float x,float y,int hp,float speed,float angle,int move_knd,int shoot_knd) {
		this.transform.position = new Vector3(x,y,0);
		this.hp         = hp;
		this.speed      = speed;
		this.angle      = angle;
		this.move_knd   = move_knd;
		this.shoot_knd  = shoot_knd;
		this.stop_flag  = false;
	}
	
	// Update is called once per frame
	void Update () {
		ShootBullet(this.shoot_knd);//弾発射
		Enemy_move(this.move_knd);//移動制御
		cnt++;
	}
	//---------移動定義--------------
	void Enemy_move(int knd){
		//特別な制御を行う場合
		switch(knd){
			case 0:default:break;//特別な制御を行わない
			case 1://逆方向に戻る
				if(cnt == 120 ) this.angle -=  PI ;
				break;
			case 2://120f停止
				if(cnt == 60 ) this.stop_flag =  true;
				if(cnt ==180 ) this.stop_flag = false;
				break;
			case 3://左右方向転換
				if(cnt == 120 ) this.angle =  PI - this.angle;
				break;
		}
		//基本移動
		Vector3 p = this.transform.position;
		if(speed != 0 && this.stop_flag!=true ){
			p.x += speed * Mathf.Cos(angle);
			p.y += speed * Mathf.Sin(angle);
			this.transform.position = p;
		}
	}
	//------------------------------
	//---------弾幕定義--------------
	//弾幕定義まとめ
	void ShootBullet(int knd){
		switch(knd){
			case 0://180fおきに8方向
				if(cnt % 120== 0 )	ShootBullet_0(8,0.02f);
				break;
			case 1://5fおきに1f0.4π回転する渦巻
				if(cnt % 5== 0 )	ShootBullet_1(0.4f,0.02f);
				break;
			case 2://120fおきに自機方向3弾
				if(cnt % 120== 0 )	Shoot_Aim(3,3,0.03f);
				break;
			default:break;//何も撃たない
		}
	}
	//0:n方向
	void ShootBullet_0(int n,float spd){
		for(int i=0;i<8;i++){
			var go = Instantiate( e_bulletPrefab ) as GameObject;
			Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
			//First(float x,float y,float speed,float angle,int knd,int col) 
			e_bullet.First(this.transform.position.x,this.transform.position.y,spd,(float)(2*PI / n * i ),0,0);
		}
	}
	//1:渦巻き(1fでPI*n回転)
	void ShootBullet_1(float n,float spd){
		var go = Instantiate( e_bulletPrefab ) as GameObject;
		Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
		//First(float x,float y,float speed,float angle,int knd,int col) 
		e_bullet.First(this.transform.position.x,this.transform.position.y,spd,
			(float)(2*PI / 60 * n * this.cnt ),0,0);
	}
	//2:自機方向(way方向n弾)
	void Shoot_Aim(int way,int n,float spd){
		float aiming_angle=0.0f;
		//自機の検索
		GameObject jiki = GameObject.Find("jiki(Clone)");
		if(jiki == null) aiming_angle = -(float)(PI/2);
		else{
			aiming_angle = Mathf.Atan2(jiki.transform.position.y-this.transform.position.y,
									   jiki.transform.position.x-this.transform.position.x);
		}
		//1way
		if(way==1){
			for(int j=0;j<n;j++){
				var go = Instantiate( e_bulletPrefab ) as GameObject;
				Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
				//First(float x,float y,float speed,float angle,int knd,int col) 
				e_bullet.First(this.transform.position.x,this.transform.position.y,spd+0.001f*j,
					aiming_angle,0,0);
			}
			return;
		}
		//2way以上
		for(int i=0;i<way;i++){
			for(int j=0;j<n;j++){
				var go = Instantiate( e_bulletPrefab ) as GameObject;
				Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
				//First(float x,float y,float speed,float angle,int knd,int col) 
				e_bullet.First(this.transform.position.x,this.transform.position.y,spd+0.001f*j,
					(float)(aiming_angle - PI/2*(way*0.1f) + PI*(way*0.1f)/(way-1)*i  ),0,0);
			}
		}
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
//			game.AddScore(1);
			var go2 = Instantiate( enemy_hp_Prefab) as GameObject;
			GUIText hp_text = go2.GetComponent<GUIText>();
			hp_text.text = "" +this.hp;
			if(this.hp<=0){
				Destroy(this.gameObject);
//				game.AddScore(100);
			}
        }
	}

}
