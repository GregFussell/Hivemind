using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombiebehavior : MonoBehaviour {
    Stack<Vector3> Orders = new Stack<Vector3>();
    enum ZombieState
    {
        FollowingPlayer,
        FollowingOrder,
        Attacking       
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
