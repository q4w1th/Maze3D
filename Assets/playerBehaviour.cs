using UnityEngine;
using System.Collections;

public class playerBehaviour : MonoBehaviour {

	public Transform fire;
	static int dynamits = 0;
	GameObject camera;
	
	// Use this for initialization
	void Start () {
	  camera = GameObject.Find("Main Camera");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void DestroyPlayer()
	{
		Instantiate(fire,gameObject.transform.position, Quaternion.identity);
		Destroy(this.gameObject); 
	}
	
	void OnCollisionEnter(Collision other)
    {
       Debug.Log ("Collision: "+other.gameObject.name);
		if(other.gameObject.name.Equals("Dynamit"))
		{
           	camera.SendMessage("AddScore",1);
			Instantiate(fire,other.gameObject.transform.position, Quaternion.identity);
			Destroy(other.gameObject); 
            dynamits = dynamits + 1;
			if (dynamits == 10 ) {
				dynamits = 0;
				// next level
				camera.SendMessage("NextLevel");
			}
		}
		
		if(other.gameObject.name.Equals("Capsule(Clone)"))
		{
            Debug.Log ("Destroy");
			Instantiate(fire,gameObject.transform.position, Quaternion.identity);
			camera.SendMessage("DecrementLive");
			//Destroy(this.gameObject); 
		}
    }
	
	
}
