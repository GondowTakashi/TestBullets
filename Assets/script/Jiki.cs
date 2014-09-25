using UnityEngine;
using System.Collections;

public class Jiki : MonoBehaviour {

	public Vector3 mouse_position;
	public Camera cam;
	public GameObject jiki_bulletPrefab;
	private int cnt;
	private float before_x;
	private float start_y;
	private float start_speed;
	public const float PI = Common.Constant.PI;
	Animator animator;
	private void Start(){
		animator = GetComponent(typeof(Animator)) as Animator;
		cnt = -60;
		start_y = -2;
		start_speed = 0.01f;
		this.transform.position = new Vector3(0,start_y,0);
	}

	private void Update()
	{
		//移動関係
		//------------------マウス座標を自機の座標に変換--------------------------
		mouse_position = Input.mousePosition;
		if( this.cam == null )
			this.cam = Camera.mainCamera;
//      Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        Ray ray = this.cam.ScreenPointToRay(mouse_position);
		Vector3 input_mouse = ray.origin;


		//被弾直後、開始時は画面外から
		if(cnt<0){
			start_y += start_speed;
			this.transform.position = new Vector3(0,start_y,0);
		}
		else{

		    // 画面左下のワールド座標をビューポートから取得
            Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0,0));
            // 画面右上のワールド座標をビューポートから取得
            Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1,0));
            // プレイヤーの座標を取得
            Vector3 pos = input_mouse;
            // プレイヤーの位置が画面内に収まるように制限をかける
            pos.x = Mathf.Clamp (pos.x, min.x, max.x);
            pos.y = Mathf.Clamp (pos.y, min.y, max.y);
            // 制限をかけた値をプレイヤーの位置とする
            transform.position = pos;
			//直前の座標から左右の動きを取得して画像に反映させる
			if(cnt % 15==0){
				     if(input_mouse.x < before_x)   animator.Play("jiki_left");
				else if(input_mouse.x > before_x)   animator.Play("jiki_right");
				else								animator.Play("jiki_default");
			}
		}
		//判定を行わない無敵時間中は点滅
		if(cnt <= 60){
			if(cnt%3==0)	renderer.enabled = true;
			else			renderer.enabled = false;
		}
		before_x = input_mouse.x;
		//--------------------------------------------------------------------

		//自動射撃(5fおき)
		if(cnt > 0 && cnt % 5==0){
			for(int i=0;i<2;i++){
				var go = Instantiate( jiki_bulletPrefab ) as GameObject;
				Jiki_bullet j_bullet = go.GetComponent<Jiki_bullet>();
				//First(float x,float y,float speed,float angle,int knd,int col)
				j_bullet.First((float)(this.transform.position.x-0.1+0.2*i),this.transform.position.y,0.2f,(float)(PI / 2 ));
			}
		}
		cnt++;
	}
	void OnTriggerEnter2D(Collider2D collider){
        // レイヤー名を取得
        string layerName = LayerMask.LayerToName (collider.gameObject.layer);
		if (layerName == "item" ){
			var go = GameObject.Find("back") as GameObject;
			GameManager game = go.GetComponent(typeof(GameManager)) as GameManager;
			game.AddScore(50);
			Destroy(collider.gameObject);
		}
		//敵弾・敵との当たり判定
		else{
			//生成後の上昇中から60fは判定を行わない
			if(this.cnt > 60){
				Destroy(this.gameObject);
			}
		}
	}
	//クリック中に射撃
//	void OnMouseDown(){
//		for(int i=0;i<2;i++){
//			var go = Instantiate( jiki_bulletPrefab ) as GameObject;
//			Jiki_bullet j_bullet = go.GetComponent<Jiki_bullet>();
//
//			j_bullet.First((float)(this.transform.position.x-0.02+0.04*i),this.transform.position.y,0.04f,(float)(PI / 2 ),0,0);
//		}
//	}
	//被弾後の処理
	void OnDestroy(){
	}

}