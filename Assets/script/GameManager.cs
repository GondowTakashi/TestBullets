using UnityEngine;
using System.Collections;
//back(背景)が持つ全体制御のスクリプト
public class GameManager : MonoBehaviour {

	public GameObject jiki_Prefab;
	public GameObject enemy_Prefab;
	public GUIText LIFE;
	public bool jiki_alive;
	public int jiki_LIFE;
	public int game_cnt;
	public int score;

	// Use this for initialization
	void Start () {
		game_cnt = 0;
		jiki_LIFE = 5 ;
		jiki_alive = false;
	}
	
	// Update is called once per frame
	void Update () {
		//自機がなくて残機が残っているなら復活
		if(jiki_alive == false && jiki_LIFE > 0 ){
			jiki_alive = true ;
			var go = Instantiate( jiki_Prefab ) as GameObject;
			Jiki jiki = go.GetComponent<Jiki>();
		}
		if(GameObject.Find("jiki")==null&&GameObject.Find("jiki(Clone)")==null) {
			jiki_alive = false;
			if(jiki_LIFE>0)	jiki_LIFE--;
		}
		//敵の配置			-1~+1画面内(右が＋、上が＋)
		//    				Enemy_make(float x,float y,int hp,float speed,float angle            ,int move_knd,int shoot_knd)
		if(game_cnt==   1)	Enemy_make(   0.30f,      2,   50,      0.01f,-(float)(3.1415926 / 4*3 ),        0,           0);
		if(game_cnt==  61)	Enemy_make(   0.50f,      2,   50,      0.01f,-(float)(3.1415926 / 2 ),          0,           1);
		if(game_cnt== 121)	Enemy_make(   0.70f,      2,   50,      0.01f,-(float)(3.1415926 / 4 ),          0,           0);


		game_cnt++;
		LIFE.text  = ("LIFE:"+ jiki_LIFE);
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
