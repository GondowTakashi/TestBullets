using UnityEngine;
using System.Collections;
//back(背景)が持つ全体制御のスクリプト
public class GameManager : MonoBehaviour {

	public GameObject jiki_Prefab;
	public GameObject enemy_Prefab;
	public GameObject enemy_spin_Prefab;
	public GameObject boss0_Prefab;
	public GameObject boss1_Prefab;
	public GUIText LIFE;
	public GUIText SCORE;
	public GUIText Alert;
	public GUIText enemy_hp;
	public int game_cnt;

	static int gamelevel;
	static int stage_knd;
	static int highscore;

	private bool jiki_alive;
	private int jiki_life;
	private int score;
	private int menu_move_cnt;
	public const float PI = Common.Constant.PI;
	//ステージ設定
	public void SetStage(int knd){
		stage_knd = knd ;
	}
	//難易度設定
	public void SetLevel(int level){
		gamelevel = level;
	}
	//難易度取得
	public int GetLevel(){
		int level = gamelevel;
		//外部参照(弾発生)時level3(0Life)はlevel2と同じ
		if(level != 1 && level != 2){
			level = 2;
		}
		return level;
	}
    //スコア加算
    public void AddScore(int s){
    	//レベルそのまま倍率とする
    	score += s * gamelevel;
    }
    //ゲームクリア処理
    public void GameClear(){
		if(score > highscore){
			Alert.fontSize = (int)(21 * Common.Constant.window_rate);
			Alert.text = "Clear! (HighScore!)";
			highscore = score;
			SCORE.text = ("Score:"+ score);
			HighScoreSave();
		}
		else{
			Alert.fontSize = (int)(30 * Common.Constant.window_rate);
			Alert.text = "Clear!";
		}
		menu_move_cnt = 121;
    }

	// Use this for initialization
	void Start () {
		game_cnt = -60;
		if(gamelevel==3) 	jiki_life = 0 ;
		else				jiki_life = 3 ;
		jiki_alive = false;
		score = 0;
		menu_move_cnt = 0;
		Alert.text = "";
		enemy_hp.text   = "";
		//ロード
		if(PlayerPrefs.HasKey("save_highscore")) {
			highscore = PlayerPrefs.GetInt("save_highscore") ;
		}

		if(stage_knd==0)	stage_knd = 2;
	}
	
	// Update is called once per frame
	void Update () {
		//メインメニュー中
		if(Application.loadedLevelName!="TestBullet"){
			SCORE.text = ("HighScore:"+ highscore);
			return;
		}
		//メインメニュー以降直前
		if(menu_move_cnt>0){
			if(menu_move_cnt==1){
				game_cnt = 0;
				jiki_life = 3 ;
				score = 0;
				//
				Application.LoadLevel("MainMenu");
			}
			menu_move_cnt--;
			return;
		}
		
		//自機がなくて残機が残っているなら復活
		if(jiki_alive == false ){
			if( jiki_life >= 0 ){
				jiki_alive = true ;
				var go = Instantiate( jiki_Prefab ) as GameObject;
				Jiki jiki = go.GetComponent<Jiki>();
			}
			//残機なければゲームオーバー
			else{
				menu_move_cnt = 121;
				jiki_life = 0 ;
				LIFE.text  = ("LIFE : X");
				if(score > highscore){
					Alert.fontSize = (int)(21 * Common.Constant.window_rate);
					Alert.text = "Game Over (HighScore!)";
					highscore = score;
					SCORE.text = ("Score:"+ score);
					HighScoreSave();
				}
				else{
					Alert.fontSize = (int)(30 * Common.Constant.window_rate);
					Alert.text = "Game Over";
				}
				return;
			}	
		}
		if(GameObject.Find("jiki")==null&&GameObject.Find("jiki(Clone)")==null) {
			jiki_alive = false;
			if(jiki_life>=0)	jiki_life--;
		}
		//敵の配置
		switch(stage_knd){
			case 1:default:		Stage_pattern_1();break;
			case 2:				Stage_pattern_2();break;
		}

		game_cnt++;
		LIFE.text  = ("LIFE : "+ jiki_life);
		SCORE.text = ("Score:"+ score);
	}
	//敵の配置１
	void Stage_pattern_1(){
		//      			-1~+1画面内(右が＋、上が＋)
		//    				Enemy_make(float x,float y,hp,speed,float angle        ,move,shoot,             knd,item)
		//1:５体降下のみ自機狙い1way3弾
		if(game_cnt==   0)	Enemy_make( -0.60f,      2,20,0.02f,-(float)(PI / 2   ),   0,    0,Common.Enemy.normal,2);
		if(game_cnt==  60)	Enemy_make( -0.30f,      2,20,0.02f,-(float)(PI / 2   ),   0,    0,Common.Enemy.normal,2);
		if(game_cnt== 120)	Enemy_make(  0.00f,      2,20,0.02f,-(float)(PI / 2   ),   0,    0,Common.Enemy.normal,2);
		if(game_cnt== 180)	Enemy_make(  0.30f,      2,20,0.02f,-(float)(PI / 2   ),   0,    0,Common.Enemy.normal,2);
		if(game_cnt== 240)	Enemy_make(  0.60f,      2,20,0.02f,-(float)(PI / 2   ),   0,    0,Common.Enemy.normal,2);
		//2:２体渦巻き左右斜め方向転換
		if(game_cnt== 300)	Enemy_make( -0.25f,      2,30,0.01f,-(float)(PI / 4   ),   1,    1,Common.Enemy.normal,3);
		if(game_cnt== 300)	Enemy_make(  0.25f,      2,30,0.01f,-(float)(PI*3/4   ),   1,    2,Common.Enemy.normal,3);
		//3:120f円形弾後降りる
		if(game_cnt== 480)	Enemy_make( -0.80f,      2,20,0.02f,-(float)(PI / 2   ),   2,    3,Common.Enemy.normal,2);
		if(game_cnt== 540)	Enemy_make( -0.40f,      2,20,0.02f,-(float)(PI / 2   ),   2,    3,Common.Enemy.normal,2);
		if(game_cnt== 600)	Enemy_make(  0.00f,      2,20,0.02f,-(float)(PI / 2   ),   2,    3,Common.Enemy.normal,2);
		if(game_cnt== 660)	Enemy_make(  0.40f,      2,20,0.02f,-(float)(PI / 2   ),   2,    3,Common.Enemy.normal,2);
		if(game_cnt== 720)	Enemy_make(  0.80f,      2,20,0.02f,-(float)(PI / 2   ),   2,    3,Common.Enemy.normal,2);
		//4:２体x3回転円形弾
		if(game_cnt== 840)	Enemy_make( -0.50f,      2,30,0.01f,-(float)(PI / 4   ),   0,    4,Common.Enemy.normal,4);
		if(game_cnt== 840)	Enemy_make(  0.50f,      2,30,0.01f,-(float)(PI*3/4   ),   0,    5,Common.Enemy.normal,4);
		if(game_cnt==1020)	Enemy_make( -0.30f,      2,30,0.01f,-(float)(PI / 4   ),   0,    5,Common.Enemy.normal,4);
		if(game_cnt==1020)	Enemy_make(  0.30f,      2,30,0.01f,-(float)(PI*3/4   ),   0,    4,Common.Enemy.normal,4);
		if(game_cnt==1200)	Enemy_make( -1.75f,   0.7f,30,0.01f,                 0 ,   0,    4,Common.Enemy.normal,4);
		if(game_cnt==1200)	Enemy_make(  1.75f,   0.8f,30,0.01f,                PI ,   0,    5,Common.Enemy.normal,4);
		//5:ランダム発生、下方向５弾のみ、すぐ上に戻る
		for(int i=0;i<12;i++){
			if(game_cnt==1320+30*i){
				Enemy_make( Random.value,(float)(1+Random.value),20,0.02f,-(float)(PI  /2),3,6,Common.Enemy.normal,2);
				Enemy_make(-Random.value,(float)(1+Random.value),20,0.02f,-(float)(PI  /2),3,6,Common.Enemy.normal,2);
			}
		}
		//Warning
		if(1680 <= game_cnt && game_cnt < 1800){
			if(game_cnt % 10 <5 )	Alert.text = "Warning!";
			else					Alert.text = "";
		}
		//ボス
		if(game_cnt==1800){
			Boss_make(0,800);
			Alert.text = "";
		}
	}
	//敵の配置２
	void Stage_pattern_2(){
		//      			-1~+1画面内(右が＋、上が＋)
		//    				Enemy_make(float x,float y,hp,speed,float angle        ,move,shoot,             knd,item)
		//1:ランダム発生、回転敵下方向
		for(int i=0;i<12;i++){
			if(game_cnt==30*i){
							Enemy_make( Random.value,2,10,0.06f,-(float)(PI/2+PI/3*Random.value),
							   															0,   10,Common.Enemy.spin,2);
							Enemy_make(-Random.value,2,10,0.06f,-(float)(PI/2-PI/3*Random.value),
							   															0,   10,Common.Enemy.spin,2);
			}
		}
		//2:ランダム発生、回転敵下方向左右反射
		for(int i=0;i<6;i++){
			if(game_cnt==480+120*i){
							Enemy_make(-Random.value,2,20,0.03f,-(float)(PI/2+PI/3*Random.value),
							   														   10,   11,Common.Enemy.spin,6);
			}
		}
		//3:左右順に高速降下敵
		for(int i=0;i<5;i++){
			if(game_cnt==1200+30*i){
							Enemy_make(-1+0.4f*i,2.2f,10,0.08f,-(float)(PI/2),       0,   12,Common.Enemy.spin,3);
			}
			if(game_cnt==1440+30*i){
							Enemy_make( 1-0.4f*i,2.2f,10,0.08f,-(float)(PI/2),       0,   12,Common.Enemy.spin,3);
			}
		}
		//4:斜め同時
		for(int i=0;i<4;i++){
			if(game_cnt==1710+180*i){
							Enemy_make(+0.2f*i,2.0f,10,0.06f,-(float)(PI  /6),       10,   13,Common.Enemy.spin,4);
							Enemy_make(-0.2f*i,2.0f,10,0.06f,-(float)(PI*5/6),       10,   13,Common.Enemy.spin,4);
			}
		}
		//Warning
		if(2340 <= game_cnt && game_cnt < 2400){
			if(game_cnt % 10 <5 )	Alert.text = "Warning!";
			else					Alert.text = "";
		}
		//ボス
		if(game_cnt==2400){
			Boss_make(1,600);
			Alert.text = "";
		}
		if(game_cnt>=2400){
			var go = GameObject.Find("enemy_boss_1(Clone)") as GameObject;
			Enemy boss = go.GetComponent(typeof(Enemy)) as Enemy;
			//HP減少時
			if(boss.hp < 300){
				if(GameObject.Find("enemy_animation_0(Clone)")==null){
					for(int i=0;i<8;i++){
						int knd,hp;
						if(i%2==0){ knd = 15;	hp = 40;}
						else	  { knd = 14;  	hp = 80;}
						Enemy_make(boss.transform.position.x,boss.transform.position.y,hp,
										0.03f,-(float)(2*PI*i/8.0f),11,knd,Common.Enemy.spin,8);
					}
				}
			}
		}
	}
	//敵を生成する関数
	void Enemy_make(float x,float y,int hp,float speed,float angle,int move_knd,int shoot_knd,
		Common.Enemy knd,int item){
		//スピンする敵
		if(knd==Common.Enemy.spin){
			var go = Instantiate( enemy_spin_Prefab ) as GameObject;
			Enemy enemy = go.GetComponent<Enemy>();
			enemy.First(x,y,hp,speed,angle,move_knd,shoot_knd,knd,item);
		}
		//普通の敵
		else{
			var go1 = Instantiate( enemy_Prefab ) as GameObject;
			Enemy enemy = go1.GetComponent<Enemy>();
			enemy.First(x,y,hp,speed,angle,move_knd,shoot_knd,knd,item);
		}

	}
	//ボスの生成
	void Boss_make(int knd,int hp){
		if(knd==0){
			var go = Instantiate( boss0_Prefab ) as GameObject;
			Enemy boss = go.GetComponent<Enemy>();
			boss.First(0,0.25f,hp,0,0,100,100,Common.Enemy.boss,20);
		}
		else{
			var go1 = Instantiate( boss1_Prefab ) as GameObject;
			Enemy boss = go1.GetComponent<Enemy>();
			boss.First(0,0.50f,hp,0,0,200,200,Common.Enemy.boss,30);
		}

	}

	//画面外の処理
	//画面外に出る場合
	void OnTriggerExit2D (Collider2D collider){
            // レイヤー名を取得
            string layerName = LayerMask.LayerToName (collider.gameObject.layer);
            //画面外に出た敵弾、自機ショット、敵、アイテムを消去
            //この関数は最初から画面外にいる場合は一度入らないとならないので敵にもそのまま適用
            if (layerName == "bullet_enemy" || layerName == "bullet_jiki" ||
            	layerName == "enemy" || layerName == "item"){
                Destroy (collider.gameObject);
            }
    }
    //ハイスコアセーブ
    void HighScoreSave(){
		PlayerPrefs.SetInt("save_highscore",highscore);
    }
}
