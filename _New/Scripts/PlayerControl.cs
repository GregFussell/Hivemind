using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private float move = 20;
	private bool stop = false;	
	private float blend;
	private float delay = 0;
	public float AddRunSpeed = 1;
	public float AddWalkSpeed = 1;
	private bool hasAniComp = false;

	// Use this for initialization
	void Start () 
	{
	
		if ( null != GetComponent<Animation>() )
		{
			hasAniComp = true;
		}

	}

	void Move ()
	{ 
		float speed =0.0f;
		float add =0.0f;

		if(Input.GetKey(KeyCode.W))
		{
			transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
			if(Input.GetKey(KeyCode.A))
			{
				transform.rotation = Quaternion.Euler(0.0f, -45.0f, 0.0f);
			}
		}
		if(Input.GetKey(KeyCode.A))
		{
			transform.rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
			if(Input.GetKey(KeyCode.S))
			{
				transform.rotation = Quaternion.Euler(0.0f, -135.0f, 0.0f);
			}
		}
		if(Input.GetKey(KeyCode.S))
		{
			transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
			if(Input.GetKey(KeyCode.D))
			{
				transform.rotation = Quaternion.Euler(0.0f, 135.0f, 0.0f);
			}
		}
		if(Input.GetKey(KeyCode.D))
		{
			transform.rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
			if(Input.GetKey(KeyCode.W))
			{
				transform.rotation = Quaternion.Euler(0.0f, 45.0f, 0.0f);
			}
		}

		if ( hasAniComp == true )
		{	
			if ( Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			{  	
				move *= 1.015F;

				if ( move>250 && CheckAniClip( "move_forward_fast" )==true )
				{
					{
						GetComponent<Animation>().CrossFade("move_forward_fast");
						add = 20*AddRunSpeed;
					}
				}
				else
				{
					GetComponent<Animation>().Play("move_forward");
					add = 5*AddWalkSpeed;
				}

				speed = Time.deltaTime*add;
				transform.Translate( 0,0,speed );
			}


			if ( (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D)) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)))
			{
				if ( GetComponent<Animation>().IsPlaying("move_forward"))
				{	GetComponent<Animation>().CrossFade("idle_normal",0.3f); }
				if ( GetComponent<Animation>().IsPlaying("move_forward_fast"))
				{	
					GetComponent<Animation>().CrossFade("idle_normal",0.5f);
					stop = true;
				}	
				move = 20;
			}

			if (stop == true)
			{	
				float max = Time.deltaTime*20*AddRunSpeed;
				blend = Mathf.Lerp(max,0,delay);

				if ( blend > 0 )
				{	
					transform.Translate( 0,0,blend );
					delay += 0.025f; 
				}	
				else 
				{	
					stop = false;
					delay = 0.0f;
				}
			}
		}
		else
		{
			if ( Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
			{  	
				add = 5*AddWalkSpeed;
				speed = Time.deltaTime*add;
				transform.Translate( 0,0,speed );
			}
		}
	}

	bool CheckAniClip ( string clipname )
	{	
		if( this.GetComponent<Animation>().GetClip(clipname) == null ) 
			return false;
		else if ( this.GetComponent<Animation>().GetClip(clipname) != null ) 
			return true;

		return false;
	}

	// Update is called once per frame
	void Update () 
	{
	
		Move();

		if ( hasAniComp == true )
		{	
			if (Input.GetKey(KeyCode.V))
			{	
				if ( CheckAniClip( "dead" ) == false ) return;

				GetComponent<Animation>().CrossFade("dead",0.2f);
				//					animation.CrossFadeQueued("idle_normal");
			} 



			if (Input.GetKey(KeyCode.Q))
			{	
				if ( CheckAniClip( "attack_short_001" ) == false ) return;

				GetComponent<Animation>().CrossFade("attack_short_001",0.0f);
				GetComponent<Animation>().CrossFadeQueued("idle_combat");
			}



			if (Input.GetKey(KeyCode.Z))
			{	
				if ( CheckAniClip( "damage_001" ) == false ) return;

				GetComponent<Animation>().CrossFade("damage_001",0.0f);
				GetComponent<Animation>().CrossFadeQueued("idle_combat");
			}

		

			if (Input.GetKey(KeyCode.B))
			{	
				if ( CheckAniClip( "idle_normal" ) == false ) return;

				GetComponent<Animation>().CrossFade("idle_normal",0.0f);
				GetComponent<Animation>().CrossFadeQueued("idle_normal");			
			}			

			if (Input.GetKey(KeyCode.F))
			{	
				if ( CheckAniClip( "idle_combat" ) == false ) return;

				GetComponent<Animation>().CrossFade("idle_combat",0.0f);
				GetComponent<Animation>().CrossFadeQueued("idle_normal");			
			}			
		}

		// if ( Input.GetKey(KeyCode.LeftArrow))
		// {
		// 	transform.Rotate( 0,Time.deltaTime*-100,0);
		// }

		// if (Input.GetKey(KeyCode.RightArrow))
		// {
		// 	transform.Rotate(0,Time.deltaTime*100,0);
		// }

	}
}
