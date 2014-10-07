using UnityEngine;
using System.Collections;

public class Enemy_bullet : MonoBehaviour {
	private float speed;
	private float speed_2;
	private float speed_b;
	private float angle;
	private float angle_2;
	private float angle_b;
	private bool  second_move;
	private int cnt;
	private int second_move_cnt;
	public const float PI = Common.Constant.PI;
	// Use this for initialization
	void Start(){
		this.speed_b = 0;
		this.angle_b = 0;
		this.cnt     = 0;
	}

	public void First(float x,float y,float speed,float angle) {
		this.transform.position = new Vector3(x,y,0);
		this.speed = speed;
		this.angle = angle;
		this.second_move = false;
		this.second_move_cnt = 0;
		this.speed_2         = 0;
		this.angle_2         = 0;
	}
	//途中で動き出すタイプ
	public void First_mover(float x,float y,float speed,float speed2,float angle,float angle2,int move_cnt) {
		this.transform.position = new Vector3(x,y,0);
		this.speed = speed;
		this.angle = angle;
		this.second_move = true;
		this.second_move_cnt = move_cnt;
		this.speed_2         = speed2;
		this.angle_2         = angle2;
	}

	// Update is called once per frame
	void Update () {
		//移動制御
		Vector3 p = this.transform.position;
		if(speed != 0){
			p.x += speed * Mathf.Cos(angle);
			p.y += speed * Mathf.Sin(angle);
			this.transform.position = p;
		}
		if(second_move == true &&cnt >= second_move_cnt){
			if(cnt == second_move_cnt){
				this.speed_b = this.speed;
				this.angle_b = this.angle;
			}
			else if(cnt <= second_move_cnt+60){
				//速度修正(60f)
				if(this.speed_b < this.speed_2)	this.speed += (this.speed_2-this.speed_b)/60.0f;
				if(this.speed_b > this.speed_2)	this.speed -= (this.speed_2-this.speed_b)/60.0f;
				//角度修正(60f)
				if((this.angle_2      < this.angle_b)&&(this.angle_b < this.angle_2 + PI)||
				   (this.angle_2 -PI*2< this.angle_b)&&(this.angle_b < this.angle_2 - PI))
				{
					this.angle += (this.angle_2-this.angle_b)/60.0f;
				}
				else if(this.angle_b != this.angle_2){
					this.angle -= (this.angle_2-this.angle_b)/60.0f;
				}
			}
			else{
				this.speed = this.speed_2;
				this.angle = this.angle_2;
				this.second_move = false;
			}
		}
		cnt++;
	}
}
