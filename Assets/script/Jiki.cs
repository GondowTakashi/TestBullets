using UnityEngine;
using System.Collections;

public class Jiki : MonoBehaviour {

	public Vector3 mouse_position;
	public Camera cam;
	public GameObject jiki_bulletPrefab;
	public int cnt;
	public float before_x;
	public float start_y;
	public float start_speed;
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
			//画面内に修正
			//10<X<290,10<Y<380
    	   	Ray screen_small = this.cam.ScreenPointToRay(new Vector3(10,10,0));
       		Ray screen_large = this.cam.ScreenPointToRay(new Vector3(280,370,0));

			     if(input_mouse.x < screen_small.origin.x)  input_mouse.x = screen_small.origin.x;
			else if(input_mouse.x > screen_large.origin.x ) input_mouse.x = screen_large.origin.x;
			     if(input_mouse.y < screen_small.origin.y)  input_mouse.y = screen_small.origin.y;
			else if(input_mouse.y > screen_large.origin.y ) input_mouse.y = screen_large.origin.y;

			transform.position = input_mouse;
			//直前の座標から左右の動きを取得して画像に反映させる
			if(cnt!=0 && cnt % 15==0){
				     if(input_mouse.x < before_x)   animator.Play("jiki_left");
				else if(input_mouse.x > before_x)   animator.Play("jiki_right");
				else								animator.Play("jiki_default");
			}
		}
		before_x = input_mouse.x;
		//--------------------------------------------------------------------

		//自動射撃(5fおき)
		if(cnt > 0 && cnt % 5==0){
			for(int i=0;i<2;i++){
				var go = Instantiate( jiki_bulletPrefab ) as GameObject;
				Jiki_bullet j_bullet = go.GetComponent<Jiki_bullet>();

				j_bullet.First((float)(this.transform.position.x-0.1+0.2*i),this.transform.position.y,0.2f,(float)(3.1415926 / 2 ),0,0);
			}
		}
		cnt++;
	}
	//敵弾・敵との当たり判定
	void OnTriggerEnter2D(){
		Destroy(this.gameObject);
	}
	//クリック中に射撃
//	void OnMouseDown(){
//		for(int i=0;i<2;i++){
//			var go = Instantiate( jiki_bulletPrefab ) as GameObject;
//			Jiki_bullet j_bullet = go.GetComponent<Jiki_bullet>();
//
//			j_bullet.First((float)(this.transform.position.x-0.02+0.04*i),this.transform.position.y,0.04f,(float)(3.1415926 / 2 ),0,0);
//		}
//	}
	//被弾後の処理
	void OnDestroy(){
	}

}