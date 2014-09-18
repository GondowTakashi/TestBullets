using UnityEngine;
using System.Collections;
//back(背景)が持つ全体制御のスクリプト
public class GameManager : MonoBehaviour {

	public GameObject jiki_Prefab;
	public GameObject enemy_Prefab;
	public GUIText LIFE;
	public GUIText SCORE;
	private bool jiki_alive;
	private int jiki_life;
	public int game_cnt;
//	public int score;
	public const float PI = 3.1415926f;

    //スコア加算
//    public void AddScore(int s){
 //   	score += s;
  //  }

	// Use this for initialization
	void Start () {
		game_cnt = 0;
		jiki_life = 3 ;
		jiki_alive = false;
//		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//メインメニュー中
		if(Application.loadedLevelName=="MainMenu"){
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
				game_cnt = 0;
				jiki_life = 3 ;
//				score = 0;
				Application.LoadLevel("MainMenu");
			}
		}
		if(GameObject.Find("jiki")==null&&GameObject.Find("jiki(Clone)")==null) {
			jiki_alive = false;
			if(jiki_life>=0)	jiki_life--;
		}
		//敵の配置			-1~+1画面内(右が＋、上が＋)
		//    				Enemy_make(float x,float y,int hp,float speed,float angle        ,int move_knd,int shoot_knd)
		if(game_cnt==   0)	Enemy_make( -0.50f,      2,    20,      0.02f,-(float)(PI / 2   ),           0,            2);
		if(game_cnt==   0)	Enemy_make(  0.00f,      2,    20,      0.02f,-(float)(PI / 2   ),           0,            2);
		if(game_cnt==   0)	Enemy_make(  0.50f,      2,    20,      0.02f,-(float)(PI / 2   ),           0,            2);

		if(game_cnt== 240)	Enemy_make( -0.25f,      2,    20,      0.02f,-(float)(PI / 4   ),           3,            1);
		if(game_cnt== 240)	Enemy_make(  0.25f,      2,    20,      0.02f,-(float)(PI*3/4   ),           3,            1);

		if(game_cnt== 480)	Enemy_make( -0.50f,      2,    20,      0.02f,-(float)(PI*2/3   ),           1,            0);
		if(game_cnt== 480)	Enemy_make(  0.00f,      2,    20,      0.02f,-(float)(PI / 2   ),           1,            0);
		if(game_cnt== 480)	Enemy_make(  0.50f,      2,    20,      0.02f,-(float)(PI / 3   ),           1,            0);


		game_cnt++;
		LIFE.text  = ("LIFE:"+ jiki_life);
//		SCORE.text = ("Score:"+ score);
	}
	//敵を生成する関数
	void Enemy_make(float x,float y,int hp,float speed,float angle,int move_knd,int shoot_knd){
		var go1 = Instantiate( enemy_Prefab ) as GameObject;
		Enemy enemy = go1.GetComponent<Enemy>();
		enemy.First(x,y,hp,speed,angle,move_knd,shoot_knd);
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
}
