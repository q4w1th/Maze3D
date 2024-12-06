using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MazeRect{
	public MazeRect(float x1, float y1, float x2, float y2)
	{
		this.x1 = x1;
		this.x2 = x2;
		this.y1 = y1;
		this.y2 = y2;
	}
	
	public float x1;
	public float y1;
	public float x2;
	public float y2;
}


public class CameraBehaviour : MonoBehaviour {

	GameObject[] CubeTab = new GameObject[300]; 
	Stack<MazeRect> walls = new Stack<MazeRect>();
	static int score = 0;
	static int live = 100;
	static string GameState = "Begin Game";
	static string size = "Size: large";
	static string mode = "Mode: hard";
	static int maxX = 160;
	static int maxY = 90;
	static int capsulecount = 20;
    public int enemiesCount;

    public Material floorMaterial;
    public Material ballMaterial;
	public Transform capsule;
	public Transform PlayerBall;
	GameObject player;


            void DestroyObjects()
	{
		Object[] allObjects = FindObjectsOfTypeAll(typeof(GameObject)) ;
        foreach(object thisObject in allObjects)
        if (((GameObject)thisObject).name.Equals ("Capsule(Clone)") || ((GameObject)thisObject).name.Equals ("Dynamit"))
		{   
			Destroy ((GameObject)thisObject);
		}
	}
	
	public void AddScore(int i)
	{
		score = score + i;
	}
	
	public void DecrementLive()
	{
	    live = live - 10;
		if (live <= 0)
		{
			DestroyObjects();
	        player.SendMessage("DestroyPlayer");
		    GameState = "Game Over";
		}
	}
	
	public void NextLevel()
	{
		GameState = "Next Level";
		DestroyObjects();
		player.SendMessage("DestroyPlayer");		
	}
	
	void initMaze(MazeRect r)
	{
		int wl = 0;// wall number
		//floor
		var f = CubeTab[wl++];
		f.transform.position = new Vector3((r.x1+r.x2)/2,-55,(r.y2+r.y1)/2);
		f.transform.localScale = new Vector3(r.x2-r.x1,1,r.y2-r.y1);
        if (Random.value > 0.5)
        {
            f.GetComponent<Renderer>().material = floorMaterial;
        }
        else
        {
            f.GetComponent<Renderer>().material = ballMaterial;
        }
		    
	    
		// walls
		var c1 = CubeTab[wl++];
		c1.transform.position = new Vector3(r.x1,-50,(r.y2+r.y1)/2);
		c1.transform.localScale = new Vector3(2,7,r.y2-r.y1);
				
		var c2 = CubeTab[wl++];
		c2.transform.position = new Vector3(r.x2,-50,(r.y2+r.y1)/2);
		c2.transform.localScale = new Vector3(2,7,r.y2-r.y1);
				
		var c3 = CubeTab[wl++];
		c3.transform.position = new Vector3((r.x2 + r.x1)/2,-50,r.y1);
		c3.transform.localScale = new Vector3(r.x2-r.x1,7,2);
				
		var c4 = CubeTab[wl++];
		c4.transform.position = new Vector3((r.x2 + r.x1)/2,-50,r.y2);
		c4.transform.localScale = new Vector3(r.x2-r.x1,7,2);
				
		int c = 1;
		walls.Clear();
		walls.Push (r);
		while (walls.Count > 0)
		{
			MazeRect w = walls.Pop();
			if ((w.x2 - w.x1 <= 10) || (w.y2 - w.y1 <= 10)) continue;
			 
			c = c + 1;
			if (c > 2) c = 1;
			if (c < 2)
			{
			   // vertical wall
			   float x = Random.Range ( 1, (int)(w.x2 - w.x1)/10 );
			   x = w.x1 + x * 10;
			   // add wall
			   walls.Push ( new MazeRect(w.x1,w.y1,x, w.y2));
			   walls.Push ( new MazeRect(x,w.y1, w.x2, w.y2));	
				
			   int d = (int)(w.y2-w.y1-10) / 10;
			   d = (int)w.y1 + Random.Range (1,d) * 10;
				
				
			   var cube = CubeTab[wl++];
			   cube.transform.position = new Vector3(x,-50,(d+w.y1)/2);
			   cube.transform.localScale = new Vector3(2,7,d-w.y1);
	           				
			   var cube2 = CubeTab[wl++];
			   cube2.transform.position = new Vector3(x,-50,(w.y2+d+10)/2);
			   cube2.transform.localScale = new Vector3(2,7,w.y2-d-10);
			   
			} 
			else
			{
				float y = Random.Range (1, (int)(w.y2 - w.y1)/10 );
				y = w.y1 + y * 10;
				//add horisontal wall
				walls.Push (new MazeRect(w.x1,w.y1,w.x2,y));
				walls.Push (new MazeRect(w.x1, y, w.x2, w.y2));
				
				int d = (int)(w.x2-w.x1-10) / 10;
			    d = (int)w.x1 + Random.Range (1,d) * 10;
				
				
			    var cube = CubeTab[wl++];
			    cube.transform.position = new Vector3((d+w.x1)/2,-50,y);
			    cube.transform.localScale = new Vector3(d-w.x1,7,2);
				
				
				var cube2 = CubeTab[wl++];
			    cube2.transform.position = new Vector3((w.x2+d+10)/2,-50,y);
			    cube2.transform.localScale = new Vector3(w.x2-d-10,7,2);
				
			    for (int i=wl;i<300;i++)
				{
				   CubeTab[i].transform.position = new Vector3(0,-150,0);
			       CubeTab[i].transform.localScale = new Vector3(1,1,1);
				}
			}			
		}
	}
	
	int getWidth(float f)
	{
		return Mathf.RoundToInt((float)Screen.width * f);		
	}
	
	int getHeight(float f)
	{
		return Mathf.RoundToInt((float)Screen.height * f);
	}
	
	
	void OnGUI () 
	{  		
	   GUIStyle boxstyle = GUI.skin.box;
	   boxstyle.alignment = TextAnchor.UpperCenter;
	   boxstyle.fontSize = (Screen.width+Screen.height)/65;
	   boxstyle.normal.textColor = Color.white;
	   boxstyle.fontStyle = FontStyle.Bold;
	   GUI.skin.box = boxstyle;
		
	   GUIStyle buttonstyle = GUI.skin.button;
	   buttonstyle.alignment = TextAnchor.MiddleCenter;
	   buttonstyle.fontSize = (Screen.width+Screen.height)/65;
	   buttonstyle.normal.textColor = Color.yellow;
	   buttonstyle.fontStyle = FontStyle.Bold;
	   GUI.skin.button = buttonstyle;
		
	   GUIStyle labelstyle = GUI.skin.label;
	   labelstyle.alignment = TextAnchor.UpperCenter;
	   labelstyle.fontSize = (Screen.width+Screen.height)/65;
	   labelstyle.normal.textColor = Color.yellow;
	   labelstyle.fontStyle = FontStyle.Bold;
	   GUI.skin.label = labelstyle;
		
	   GUI.Label(new Rect(1, 1,getWidth(0.5f), getHeight(0.1f)),"Score: "+score,labelstyle);
	   GUI.Label(new Rect(getWidth(0.5f), 1,getWidth(0.5f), getHeight(0.1f)),"Live: "+live+" %",labelstyle);
       GUI.Label(new Rect(getWidth(0.5f), getHeight(0.1f), getWidth(0.5f), getHeight(0.1f)), "Enemies: " + (capsulecount - (100 - live)/10), labelstyle);
        if (!player)
	   {
		 
		GUI.Box (new Rect(getWidth(0.2f), getHeight(0.2f),getWidth(0.6f),getHeight(0.6f)), " ");
		if (GameState.Equals ("Begin Game"))
			{
				 if (GUI.Button (new Rect (getWidth(0.3f),getHeight(0.3f),getWidth(0.4f),getHeight(0.1f)), "Start")) 
				 {
		            NextMaze ();
					Instantiate(PlayerBall,new Vector3(5,-50,5), Quaternion.identity);
				    Time.timeScale = 1;
					player = GameObject.Find("Player(Clone)");
					GameState = "Play";
				 }
				 if (GUI.Button (new Rect (getWidth(0.3f),getHeight(0.41f),getWidth(0.4f),getHeight(0.1f)), size)) 
				 {
		            if (size.Equals("Size: large")) size = "Size: small";
					 else if (size.Equals("Size: small")) size = "Size: medium";
				      else if (size.Equals("Size: medium")) size = "Size: large";
					
					switch (size)
				    {	 
					    case "Size: small":
					      maxX = 70;
						  maxY = 40;
						  this.transform.position = new Vector3(0,25,0);
						  break;
					
					    case "Size: medium":
						  maxX = 100;
						  maxY = 60;
						  this.transform.position = new Vector3(0,50,0);
						  break;
					
						case "Size: large":
						  maxX = 150;
						  maxY = 90;
						  this.transform.position = new Vector3(0,100,0);
						  break;
					}
					initMaze(new MazeRect(-maxX,-maxY,maxX,maxY));
				 }
				
				 if (GUI.Button (new Rect (getWidth(0.3f),getHeight(0.52f),getWidth(0.4f),getHeight(0.1f)), mode)) 
				 {
		            if (mode.Equals("Mode: hard")) mode = "Mode: easy";
					 else if (mode.Equals("Mode: easy")) mode = "Mode: medium";
				      else if (mode.Equals("Mode: medium")) mode = "Mode: hard";
					
					switch (mode)
				    {	 
					    case "Mode: hard":
					      capsulecount = 20;
						  break;
					
					    case "Mode: medium":
						  capsulecount = 10;
						  break;
					
						case "Mode: easy":
						  capsulecount = 5;
						  break;
					}
					
				 }
				 if (GUI.Button (new Rect (getWidth(0.3f),getHeight(0.63f),getWidth(0.4f),getHeight(0.1f)), "Exit Game ")) 
				 {
		            Application.Quit();
				 }
				
		   }
		if (GameState.Equals ("Next Level"))
			{
				   GUIStyle biglabelstyle = GUI.skin.label;
				   biglabelstyle.alignment = TextAnchor.MiddleCenter;
				   biglabelstyle.fontSize = (Screen.width+Screen.height)/35;
				   biglabelstyle.normal.textColor = new Color(1.0f, 0.3f, 0.0f);
				   biglabelstyle.fontStyle = FontStyle.Bold;	
				
				 GUI.Label(new Rect (getWidth(0.3f),getHeight(0.13f),getWidth(0.4f),getHeight(0.4f)),"Level completed");
				 
				 if (GUI.Button (new Rect (getWidth(0.3f),getHeight(0.63f),getWidth(0.4f),getHeight(0.1f)), "Next Level")) 
				 {
		            NextMaze ();
					Instantiate(PlayerBall,new Vector3(5,-50,5), Quaternion.identity);
				    Time.timeScale = 1;
					player = GameObject.Find("Player(Clone)");
					GameState = "Play";
				 }
			}
	   }
		
		if (GameState.Equals ("Game Over"))
			{
				   GUIStyle biglabelstyle = GUI.skin.label;
				   biglabelstyle.alignment = TextAnchor.MiddleCenter;
				   biglabelstyle.fontSize = (Screen.width+Screen.height)/30;
				   biglabelstyle.normal.textColor = new Color(1.0f, 0.3f, 0.0f);
				   biglabelstyle.fontStyle = FontStyle.Bold;	
				
				 GUI.Label(new Rect (getWidth(0.3f),getHeight(0.13f),getWidth(0.4f),getHeight(0.4f)),"Game Over");
				 
				 if (GUI.Button (new Rect (getWidth(0.3f),getHeight(0.63f),getWidth(0.4f),getHeight(0.1f)), "OK")) 
				 {
		             score = 0;
				     live = 100;
				     GameState = "Begin Game";
				 }
			}
	   
   }
	
	
	
    void NextMaze()
	{
		Time.timeScale = 0;		
		initMaze(new MazeRect(-maxX,-maxY,maxX,maxY));
	    
		// display random dynamic boxes
		for (int i=0;i<10;i++)
		{
            
			int x = -maxX+5 + Random.Range(1,2 * maxX / 10) * 10;
			int y = -maxY+5 + Random.Range(1,2 * maxY / 10) * 10;
			
		    var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		    cube.transform.position = new Vector3(x,-50,y);
		    cube.transform.localScale = new Vector3(7,7,7);
			cube.GetComponent<Renderer>().material.color = new Color(1.0f, 0.3f, 0.0f);
			cube.name = "Dynamit";
            
			
		}
	    
		// display random capsules
		for (int i=0;i<capsulecount;i++)
		{
			int x = -maxX+5 + Random.Range(1,2 * maxX / 10) * 10;
			int y = -maxY+5 + Random.Range(1,2 * maxY / 10) * 10;
			
		    Instantiate(capsule,new Vector3(x,-50,y), Quaternion.identity);
            enemiesCount++;

        }
	}
	
	// Use this for initialization
	void Start () {
		
		for (int i=0;i<300;i++)
		{
		   CubeTab[i] = GameObject.CreatePrimitive(PrimitiveType.Cube);
		   CubeTab[i].name = "Cube "+i;	
		}		
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		// player = GameObject.Find("Player");
		
		initMaze(new MazeRect(-maxX,-maxY,maxX,maxY));
		
	}
	
	void Update () 
	{
		if (Input.GetKey(KeyCode.Menu))
    	{
			if (Time.timeScale == 1 ) Time.timeScale = 0;
			else Time.timeScale = 1;
    	}		
		if (Input.GetKey(KeyCode.Escape))
    	{
        	Application.Quit();
    	}
		
		if (player == null) return;
		
		Vector3 pos = this.player.transform.position;
		Rigidbody rp = player.GetComponent<Rigidbody>();
		if (Input.GetKey(KeyCode.Space))
		{
			//Rigidbody instance = Instantiate(missile, obj.transform.position, this.transform.rotation) as Rigidbody;
			//instance.AddForce( v * 50000);
			
			//irockets--;
		}
		
		if (Input.GetKey(KeyCode.UpArrow))
		{
            pos.z += 1;
		}
		
		if (Input.GetKey(KeyCode.DownArrow))
		{
            pos.z -= 1;
			
		}
        
		if (Input.GetKey(KeyCode.RightArrow))
		{
            pos.x += 1;
			
		}
		
		if (Input.GetKey(KeyCode.LeftArrow))
		{
            pos.x -= 1;
				
		}
		
		
		
		Vector3 dir = Vector3.zero;
		dir.x = Input.acceleration.x;
		dir.z = Input.acceleration.y;
		//if (dir.sqrMagnitude > 1) dir.Normalize();
		//dir *= Time.deltaTime;
		
		player.GetComponent<Rigidbody>().AddForce(dir * 10000*Time.deltaTime);
		//if (pos.x > 1065) pos.x = 1065;
		//else if (pos.x < 930) pos.x = 930;
		//if (pos.y > 100) pos.y = 100;
		//else if (pos.y < 40) pos.y = 40;
		
		//transform.rotation = Quaternion.Euler(270 - 90 * dx,270,0);
		player.transform.position = pos;
		
	}
}
