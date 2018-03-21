using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularSoldierAI : MonoBehaviour {

	public UnityEngine.AI.NavMeshAgent agent;
	private float blend;
	private float delay = 0;
	private bool hasAniComp = false;
	
	public enum State {
		IDLE,
		PATROL,
		WAIT,
		PERSUE,
		ATTACK
	}

	public State state;
	public State prevState;
	public bool alive;

	// Variables for patrolling
	public GameObject[] waypoints;
	private int waypointInd = 0;
	public float walkSpeed = 1.0f;

	// Variables for waiting
	public float waitTime = 3.0f;
	private float waited = 0.0f;

	// Variables for attacking
	public float runSpeed = 1.0f;
	public GameObject target;

	// Use this for initialization
	void Start () {

		if ( null != GetComponent<Animation>() )
		{
			hasAniComp = true;
		}

		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		
		agent.updatePosition = true;
		agent.updateRotation = false;

		state = RegularSoldierAI.State.PATROL;
		prevState = RegularSoldierAI.State.PATROL;

		alive = true;

		//START FSM
		StartCoroutine("FSM");
	}

	IEnumerator FSM()
	{
		while(alive)
		{
			switch(state)
			{
				case State.IDLE:
					Idle();
					break;
				case State.PATROL:
					Patrol();
					break;
				case State.WAIT:
					Wait();
					break;
				case State.PERSUE:
					Persue();
					break;
				case State.ATTACK:
					Attack();
					break;
			}
			yield return null;
		}
	}

	void Idle()
	{
		//Idle animation
	}

	void Patrol()
	{
		agent.speed = walkSpeed;
		if(Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) >= 2)
		{
			agent.SetDestination(waypoints[waypointInd].transform.position);
			//move desired velocity
			transform.LookAt(waypoints[waypointInd].transform.position);
			//transform.Translate( 0,0,agent.speed );
		}
		else if(Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) <= 2)
		{
			agent.SetDestination(this.transform.position);
			waypointInd += 1;
			if(waypointInd >= waypoints.Length)
				waypointInd = 0;
			state = RegularSoldierAI.State.WAIT;
			prevState = RegularSoldierAI.State.PATROL;
		}
		else
		{
			//move zero velocity
		}
		animate();
	}

	void Wait()
	{
		waited += Time.deltaTime;
		if(waited >= waitTime)
		{
			waited = 0;
			state = RegularSoldierAI.State.PATROL;
			prevState = RegularSoldierAI.State.WAIT;
		}
		animate();
	}

	void Persue()
	{
		agent.speed = runSpeed;
		agent.SetDestination(target.transform.position);

		if(Vector3.Distance(this.transform.position, target.transform.position) >= 10)
		{
			//move desired velocity
			transform.LookAt(target.transform.position);
			//transform.Translate( 0,0,agent.speed );
		}
		else if(Vector3.Distance(this.transform.position, waypoints[waypointInd].transform.position) < 10)
		{
			state = RegularSoldierAI.State.ATTACK;
			prevState = RegularSoldierAI.State.PERSUE;
		}
		//move desired velocity
	}

	void Attack()
	{
		//Shoot target
		animate();
	}

	void animate()
	{
		if(hasAniComp == true)
		{
			if(state == RegularSoldierAI.State.PATROL || state == RegularSoldierAI.State.PERSUE)
			{
				GetComponent<Animation>().Play("demo_combat_run");
			}

			if(state == RegularSoldierAI.State.WAIT || state == RegularSoldierAI.State.IDLE)
			{
				GetComponent<Animation>().Play("demo_combat_idle");
			}
			
			if(state == RegularSoldierAI.State.ATTACK)
			{
				GetComponent<Animation>().Play("demo_combat_shoot");
			}

		}
	}


	void OnTriggerEnter (Collider coll)
	{
		if(coll.tag == "Player" ||coll.tag == "Undead");
		{
			state = RegularSoldierAI.State.ATTACK;
			target = coll.gameObject;
		}
	}
}
