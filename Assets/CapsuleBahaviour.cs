using UnityEngine;
using System.Collections;
using System;

public class CapsuleBahaviour : MonoBehaviour {

	
	public Transform fire;
	// Use this for initialization
	DateTime dt = DateTime.Now;
	System.Random rnd = new System.Random();
	Vector3 MovDir = new Vector3(10,0,0);
	Vector3 TargetPos;
	
	void Start () {
		TargetPos = this.transform.position;
		
		    bool h = Physics.Raycast (transform.position, MovDir, 11);
		    if (h)
		    {
		        bool r = Physics.Raycast (transform.position, Quaternion.Euler (0,90,0) * MovDir, 11);
			    if (!r) MovDir = Quaternion.Euler (0,90,0) * MovDir;
			    else
			    {
			       bool l = Physics.Raycast (transform.position, Quaternion.Euler (0,-90,0) * MovDir,11); 
				   if (!l) MovDir = Quaternion.Euler (0,-90,0) * MovDir;
				   else
					{
					    bool b = Physics.Raycast (transform.position, Quaternion.Euler (0,180,0) * MovDir,11);
		    			if (!b) MovDir = Quaternion.Euler (0,180,0) * MovDir;
			    	}
		        }
		    }
			
	}
	
	
	bool eq(Vector3 v1,Vector3 v2)
	{
	   return ( Math.Abs(v1.x-v2.x) < 2f) && (Math.Abs(v1.y-v2.y) < 2f) && (Math.Abs(v1.z-v2.z)< 2f);			
	}
	
	// Update is called once per frame
	void Update () {
		//this.transform.Rotate(new Vector3(0, 100 * Time.deltaTime, 0));
		this.GetComponent<Rigidbody>().velocity = MovDir;
		
		
		if (Time.timeScale == 0) dt = DateTime.Now;
		TimeSpan ts = DateTime.Now - dt;
		if (ts.TotalMilliseconds > 1500) {
			System.Random rand = new System.Random(DateTime.Now.Millisecond);
			//Instantiate(fire,this.gameObject.transform.position, Quaternion.identity);
			if (rnd.Next (1,2) > 1) MovDir = Quaternion.Euler (0,90 * rand.Next(1,3) ,0) * MovDir;
			else MovDir = Quaternion.Euler (0,-90 * rand.Next(1,3) ,0) * MovDir;
			TargetPos = transform.position + MovDir;
			dt = DateTime.Now;
			//Destroy(this.gameObject); 
			
		}
		
		if (eq (this.transform.position, TargetPos))
		{
			dt = DateTime.Now;
			Debug.Log (this.transform.position + " " + TargetPos + " " +eq(this.transform.position, TargetPos));
			// check if wall are near player
			bool h = Physics.Raycast (transform.position, MovDir, 10);
			bool r = Physics.Raycast (transform.position, Quaternion.Euler (0,90,0) * MovDir, 10);
			bool l = Physics.Raycast (transform.position, Quaternion.Euler (0,-90,0) * MovDir,10);
			bool b = Physics.Raycast (transform.position, Quaternion.Euler (0,180,0) * MovDir,10);
		
		    // wolne na prawo
			if (!r)
			{
				MovDir = Quaternion.Euler (0,90,0) * MovDir;
				TargetPos = TargetPos + MovDir;
			} else
				if (h) // zajete prosto
				{
				   if (!l)
				   {
				   		MovDir = Quaternion.Euler (0,-90,0) * MovDir;
						TargetPos = TargetPos + MovDir;
				   } else
					{
					    MovDir = Quaternion.Euler (0,180,0) * MovDir;
						TargetPos = TargetPos + MovDir;
					}
			    } else TargetPos = TargetPos + MovDir;    				
		} 
		
		
		
	}

    void OnCollisionEnter(Collision other)
    {
       
		if(other.gameObject.name.IndexOf("Capsule") > -1)
		{
           	Instantiate(fire,this.gameObject.transform.position, Quaternion.identity);
			Destroy(this.gameObject); 
			
		}
		if(other.gameObject.name.IndexOf("Player") > -1)
		{
           	Instantiate(fire,this.gameObject.transform.position, Quaternion.identity);
			Destroy(this.gameObject); 
			
		}
    }
}
