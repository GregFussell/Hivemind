using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerControl : MonoBehaviour {

	private float move = 20;
	private bool stop = false;	
	private float blend;
	private float delay = 0;
	public float AddRunSpeed = 1;
	public float AddWalkSpeed = 1;
	private bool hasAniComp = false;
    private List<GameObject> selectedZombies = new List<GameObject>();
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
    bool isSelecting = false;
    Vector3 mousePosition1;
    // Update is called once per frame
    void Update () 
	{
        // If we press the left mouse button, save mouse location and begin selection
        if (Input.GetMouseButtonDown(0))
        {
           // Debug.Log("working");
            isSelecting = true;
            mousePosition1 = Input.mousePosition;

        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0))
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag("Zombie"))
            {
                //Debug.Log(g.name + "working 3");
                if (IsWithinSelectionBounds(g))
                {
                    //Debug.Log(g.name + "working 2");
                    selectedZombies.Add(g);
                }
                else
                {
                    selectedZombies.Remove(g);
                }
            }
            isSelecting = false;
        }

        if (Input.GetMouseButtonDown(1))
        {

        }


        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(1))
        {

        }
        if(isSelecting)
        {
            foreach (GameObject g in selectedZombies)
            {
                //Debug.Log(g.name);
            }
        }
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
    private Vector3 FindHitPoint(Vector3 mousePos)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return Vector3.negativeInfinity;
    }
    private void HordeMove()
    {
        foreach (GameObject g in selectedZombies)
        {
            try
            {
                Debug.Log("ordering " + g.name);
                Vector3 position = FindHitPoint(FindHitPoint(Input.mousePosition));
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    position = hit.point;
                    g.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(position);
                }

            }
            catch (Exception e)
            {

            }
        }
    }
    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        //if (!isSelecting)
        //    return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position));
    }

}
