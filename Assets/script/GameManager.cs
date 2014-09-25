using UnityEngine;
using System.Collections;
//back(背景)が持つ全体制御のスクリプト
public class GameManager : MonoBehaviour {

	public GameObject jiki_Prefab;
	public GameObject enemy_Prefab;
	public GameObject boss_Prefab;
	public GUIText LIFE;
	public GUIText SCORE;
	public GUIText Alert;
	public GUIText enemy_hp;
	public int game_cnt;

	static int gamelevel;
	static int highscore;

	private bool jiki_alive;
	private int jiki_life;
	private int score;
	private int menu_move_cnt;
	public const float PI = Common.Constant.PI;
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
			Alert.fontSize = (int)(24 * Common.Constant.window_rate);
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
	}
	
	// Update is called once per frame
	void Update () {
		//メインメニュー中
		if(Application.loadedLevelName=="MainMenu"){
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
					Alert.fontSize = (int)(24 * Common.Constant.window_rate);
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
		//敵の配置			-1~+1画面内(右が＋、上が＋)
		//    				Enemy_make(float x,float y,int hp,float speed,float angle        ,int move_knd,int shoot_knd)
		//1:５体降下のみ自機狙い1way3弾
		if(game_cnt==   0)	Enemy_make( -0.60f,      2,    20,      0.02f,-(float)(PI / 2   ),           0,            0);
		if(game_cnt==  60)	Enemy_make( -0.30f,      2,    20,      0.02f,-(float)(PI / 2   ),           0,            0);
		if(game_cnt== 120)	Enemy_make(  0.00f,      2,    20,      0.02f,-(float)(PI / 2   ),           0,            0);
		if(game_cnt== 180)	Enemy_make(  0.30f,      2,    20,      0.02f,-(float)(PI / 2   ),           0,            0);
		if(game_cnt== 240)	Enemy_make(  0.60f,      2,    20,      0.02f,-(float)(PI / 2   ),           0,            0);
		//2:２体渦巻き左右斜め方向転換
		if(game_cnt== 300)	Enemy_make( -0.25f,      2,    40,      0.01f,-(float)(PI / 4   ),           1,            1);
		if(game_cnt== 300)	Enemy_make(  0.25f,      2,    40,      0.01f,-(float)(PI*3/4   ),           1,            2);
		//3:120f円形弾後降りる
		if(game_cnt== 480)	Enemy_make( -0.80f,      2,    20,      0.02f,-(float)(PI / 2   ),           2,            3);
		if(game_cnt== 540)	Enemy_make( -0.40f,      2,    20,      0.02f,-(float)(PI / 2   ),           2,            3);
		if(game_cnt== 600)	Enemy_make(  0.00f,      2,    20,      0.02f,-(float)(PI / 2   ),           2,            3);
		if(game_cnt== 660)	Enemy_make(  0.40f,      2,    20,      0.02f,-(float)(PI / 2   ),           2,            3);
		if(game_cnt== 720)	Enemy_make(  0.80f,      2,    20,      0.02f,-(float)(PI / 2   ),           2,            3);
		//4:２体x3回転円形弾
		if(game_cnt== 840)	Enemy_make( -0.50f,      2,    30,      0.01f,-(float)(PI / 4   ),           0,            4);
		if(game_cnt== 840)	Enemy_make(  0.50f,      2,    30,      0.01f,-(float)(PI*3/4   ),           0,            5);
		if(game_cnt==1020)	Enemy_make( -0.30f,      2,    30,      0.01f,-(float)(PI / 4   ),           0,            5);
		if(game_cnt==1020)	Enemy_make(  0.30f,      2,    30,      0.01f,-(float)(PI*3/4   ),           0,            4);
		if(game_cnt==1200)	Enemy_make( -1.75f,   0.7f,    30,      0.01f,                 0 ,           0,            4);
		if(game_cnt==1200)	Enemy_make(  1.75f,   0.8f,    30,      0.01f,                PI ,           0,            5);
		//5:ランダム発生、下方向５弾のみ、すぐ上に戻る
		for(int i=0;i<12;i++){
			if(game_cnt==1320+30*i){
				Enemy_make( Random.value,(float)(1+Random.value),20,0.02f,-(float)(PI / 2   ),           3,            6);
				Enemy_make(-Random.value,(float)(1+Random.value),20,0.02f,-(float)(PI / 2   ),           3,            6);
			}
		}
		//Warning
		if(1680 <= game_cnt && game_cnt < 1800){
			if(game_cnt % 10 <5 )	Alert.text = "Warning!";
			else					Alert.text = "";
		}
		//ボス
		if(game_cnt==1800){
			Boss_make(800);
			Alert.text = "";
		}
		game_cnt++;
		LIFE.text  = ("LIFE : "+ jiki_life);
		SCORE.text = ("Score:"+ score);
	}
	//敵を生成する関数
	void Enemy_make(float x,float y,int hp,float speed,float angle,int move_knd,int shoot_knd){
		var go1 = Instantiate( enemy_Prefab ) as GameObject;
		Enemy enemy = go1.GetComponent<Enemy>();
		enemy.First(x,y,hp,speed,angle,move_knd,shoot_knd);
	}
	//ボスの生成
	void Boss_make(int hp){
		var go1 = Instantiate( boss_Prefab ) as GameObject;
		Enemy boss = go1.GetComponent<Enemy>();
		boss.First(0,0.50f,hp,0,0,10,10);
	}

	//画面外の処理
	//画面外に出る場合
	void OnTriggerExit2D (Collider2D collider){
            // レイヤー名を取得
            string layerName = LayerMask.LayerToName (collider.gameObject.layer);
            //画面外に出た敵弾、自機ショット、敵を消去
            //この関数は最初から画面外にいる場合は一度入らないとならないので敵にもそのまま適用
            if (layerName == "bullet_enemy" || layerName == "bullet_jiki" || layerName == "enemy"){
                Destroy (collider.gameObject);
            }
    }
    //ハイスコアセーブ
    void HighScoreSave(){
		PlayerPrefs.SetInt("save_highscore",highscore);
    }
}
