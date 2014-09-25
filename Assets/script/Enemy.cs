using UnityEngine;
using System.Collections;
using Common;

public class Enemy : MonoBehaviour {
	private float speed;
	private float angle;
	private int hp;
	private int item_num;
	private int cnt;
	private Common.Enemy knd;
	private int move_knd;
	private int shoot_knd;
	private bool stop_flag;
	public GameObject e_bullet0Prefab;
	public GameObject e_bullet1Prefab;
	public GameObject pointPrefab;
	public const float PI = Common.Constant.PI;

	// Use this for initialization
	void Start () {
		cnt   = 0 ;
	}
	public void First(float x,float y,int hp,float speed,float angle,int move_knd,int shoot_knd,
		Common.Enemy knd,int item) {
		this.transform.position = new Vector3(x,y,0);
		this.hp         = hp;
		this.speed      = speed;
		this.angle      = angle;
		this.move_knd   = move_knd;
		this.shoot_knd  = shoot_knd;
		this.stop_flag  = false;
		this.knd 		= knd;
		this.item_num   = item;
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
			case 1://左右方向転換
				if(cnt == 120 ) this.angle =  PI - this.angle;
				break;
			case 2://120f停止
				if(cnt == 60 ) this.stop_flag =  true;
				if(cnt ==180 ) this.stop_flag = false;
				break;
			case 3://逆方向に戻る
				if(cnt == 60 ) this.angle -=  PI ;
				break;
			//-----ボス制御-----
			case 10:
				int t = cnt % 1200;
				if(t<300){//1:ランダム移動しながら円形弾
					if(this.hp < 200){
						if(t%20==  5 ){
							float rx,ry;
							rx = -0.8f + 1.0f * Random.value;
							ry =  0.4f + 0.5f * Random.value;
							this.transform.position = new Vector3(rx,ry,0);
						}
					}
					else{
						if(t%60== 15 ){
							float rx,ry;
							rx = -0.8f + 1.0f * Random.value;
							ry =  0.4f + 0.5f * Random.value;
							this.transform.position = new Vector3(rx,ry,0);
						}
					}
				}
				if(t==599||t==899){
					float rx,ry;
					rx = 0.2f + 0.6f * Random.value;
					ry = 0.4f + 0.5f * Random.value;
					this.transform.position = new Vector3(rx,ry,0);
				}
				break;
			//-----------------
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
		var go = GameObject.Find("back") as GameObject;
		GameManager game = go.GetComponent(typeof(GameManager)) as GameManager;
		switch(knd){
			case 0://自機方向1way3弾
				if(cnt == 60)	Shoot_Aim(1,1+game.GetLevel(),0.03f);
				break;
			case 1://6fおきに1f0.3π回転する渦巻
				if(cnt % (12/game.GetLevel()) == 0 )	ShootBullet_1(0.3f,0.02f);
				break;
			case 2://１の逆回転版
				if(cnt % (12/game.GetLevel()) == 0 )	ShootBullet_1(-0.3f,0.02f);
				break;
			case 3://120f中30fおきに8方向
				if( (60<=cnt&&cnt<=180)&&(cnt % 30== 0 )){
					ShootBullet_0(4*game.GetLevel(),0.02f);
				}
				break;
			case 4://回転円形弾10fおきに4方向1f0.5π回転
				if(cnt % 10== 0 ){
					ShootBullet_0_spin(2*game.GetLevel(),0.6f,0.02f);
				}
				break;
			case 5://４の逆回転版
				if(cnt % 10== 0 ){
					ShootBullet_0_spin(2*game.GetLevel(),-0.6f,0.02f);
				}
				break;
			case 6://下方向５弾
				if(cnt == 60)	Shoot_under(1+2*game.GetLevel(),0.02f);
				break;
			//-----ボス制御-----
			case 10:
				int t = cnt % 1200;
				if(t<300){//1:ランダム移動しながら円形弾
					if(this.hp < 200){
						if(t % 20 == 0 )	ShootBullet_0(6*game.GetLevel(),0.02f);
					}
					else{
						if(t % 60 == 0 )	ShootBullet_0(4*game.GetLevel(),0.03f);
					}
				}
				else if(t<600){//2:自機方向5way5弾
					if(t % 60 == 0 ){
						int x;
						if(this.hp < 200) x = 1+4*game.GetLevel();
						else			  x = 1+2*game.GetLevel();
						Shoot_Aim(x,2*game.GetLevel(),0.04f);
					}
				}
				else if(t<900){//3:回転円形両方向同時
					if(t % 15== 0 ){
						int x;
						if(this.hp < 200) x = 3*game.GetLevel();
						else			  x = 2*game.GetLevel();
						ShootBullet_0_spin(x, 0.3f,0.02f);
						ShootBullet_0_spin(x,-0.5f,0.02f);
					}
				}
				else{//4:ランダムばらまき	
					if(t % (6/game.GetLevel()) == 0){
						Shoot_random();
						//追加
						if(this.hp < 200)	Shoot_random();
					}
				}
				break;
			//-----------------
			default:break;//何も撃たない
		}
	}
	//0:n方向(ランダム角開始)
	void ShootBullet_0(int n,float spd){
		float angle = PI * Random.value;
		for(int i=0;i<n;i++){
			var go = Instantiate( e_bullet0Prefab ) as GameObject;
			Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
			//First(float x,float y,float speed,float angle,int knd,int col) 
			e_bullet.First(this.transform.position.x,this.transform.position.y,spd,
				(float)(2*PI / n * i + angle ));
		}
	}
	//０の回転する版(1fでPI*n回転)
	void ShootBullet_0_spin(int n,float npi,float spd){
		float angle = 2*PI / 60 * npi * this.cnt;
		GameObject go=null;
		for(int i=0;i<n;i++){
			if(npi < 0)	go = Instantiate( e_bullet0Prefab ) as GameObject;
			else		go = Instantiate( e_bullet1Prefab ) as GameObject;
			Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
			//First(float x,float y,float speed,float angle,int knd,int col) 
			e_bullet.First(this.transform.position.x,this.transform.position.y,spd,
				(float)(2*PI / n * i + angle ));
		}
	}
	//1:渦巻き(1fでPI*n回転)
	void ShootBullet_1(float n,float spd){
		GameObject go=null;
		if(n < 0)	go = Instantiate( e_bullet0Prefab ) as GameObject;
		else		go = Instantiate( e_bullet1Prefab ) as GameObject;
		Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
		//First(float x,float y,float speed,float angle,int knd,int col) 
		e_bullet.First(this.transform.position.x,this.transform.position.y,spd,
			(float)(2*PI / 60 * n * this.cnt ));
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
				var go = Instantiate( e_bullet0Prefab ) as GameObject;
				Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
				//First(float x,float y,float speed,float angle,int knd,int col) 
				e_bullet.First(this.transform.position.x,this.transform.position.y,spd+0.004f*j,
					aiming_angle);
			}
			return;
		}
		//2way以上
		for(int i=0;i<way;i++){
			for(int j=0;j<n;j++){
				var go = Instantiate( e_bullet0Prefab ) as GameObject;
				Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
				//First(float x,float y,float speed,float angle,int knd,int col) 
				e_bullet.First(this.transform.position.x,this.transform.position.y,spd+0.004f*j,
					(float)(aiming_angle - PI/2*(way*0.1f) + PI*(way*0.1f)/(way-1)*i  ));
			}
		}
	}
	//3:下方向直線弾
	void Shoot_under(int n,float spd){
			for(int j=0;j<n;j++){
				var go = Instantiate( e_bullet0Prefab ) as GameObject;
				Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
				//First(float x,float y,float speed,float angle,int knd,int col) 
				e_bullet.First(this.transform.position.x,this.transform.position.y,spd+0.004f*j,
					-(float)(PI/2));
			}
	}
	//4:ランダムばらまき
	void Shoot_random(){
		var go = Instantiate( e_bullet0Prefab ) as GameObject;
		Enemy_bullet e_bullet = go.GetComponent<Enemy_bullet>();
		//First(float x,float y,float speed,float angle,int knd,int col) 
		e_bullet.First(this.transform.position.x,this.transform.position.y,
			0.005f+0.02f*Random.value,2*PI*Random.value);

		var go1 = Instantiate( e_bullet1Prefab ) as GameObject;
		Enemy_bullet e_bullet1 = go1.GetComponent<Enemy_bullet>();
		//First(float x,float y,float speed,float angle,int knd,int col) 
		e_bullet1.First(this.transform.position.x,this.transform.position.y,
			0.01f+0.03f*Random.value,2*PI*Random.value);

	}
	//------------------------------
	void OnTriggerEnter2D(Collider2D collider){
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName (collider.gameObject.layer);
        //自機弾との当たり判定：ダメージを受けて弾を消去
		if (layerName == "bullet_jiki" ){
			var go3 = GameObject.Find("back") as GameObject;
			GameManager game = go3.GetComponent(typeof(GameManager)) as GameManager;

			Destroy(collider.gameObject);
			this.hp--;
			game.AddScore(1);
			//ヒットした敵のHP表示
			game.enemy_hp.text = "" +this.hp;
			if(this.knd == Common.Enemy.boss){
				game.enemy_hp.fontSize = (int)(50 * Common.Constant.window_rate);
			}
			if(this.hp<=0){
				if(this.knd == Common.Enemy.boss){
					game.AddScore(2000);
					game.GameClear();
				}
				else{
					game.AddScore(100);
				}
				ItemMake(this.item_num);
				Destroy(this.gameObject);
			}
        }
	}
	//アイテム生成
	void ItemMake(int num){
		for(int i=0;i<num;i++){
			var go = Instantiate( pointPrefab ) as GameObject;
			PointItem item = go.GetComponent<PointItem>();
			item.First(this.transform.position.x,this.transform.position.y);
		}

	}
}
