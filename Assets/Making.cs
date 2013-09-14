using UnityEngine;
using System.Collections;

public class Making : MonoBehaviour {
	static int sIZE = 8;
	// Use this for initializat ion
	GameObject ob;
	GameObject ob2;
	GameObject[] list = new GameObject[sIZE*sIZE+1];
	int[] map = new int[sIZE*sIZE+1];
	tk2dSpriteCollectionData datas;
	void Start () {
		gameObject.AddComponent<Breaking>();
		/*
		ob = new GameObject("Rock");
		tk2dSpriteCollectionData data = (tk2dSpriteCollectionData)Resources.Load("spriteC/SpriteC",typeof(tk2dSpriteCollectionData));
		tk2dSprite sp = ob.AddComponent<tk2dSprite>();
		sp.SetSprite(data,"ground");
		ob.transform.localScale = (new Vector3(10.0f,10.0f,10.0f));
		ob.transform.localPosition = (new Vector3(10.0f,10.0f,10.0f));
		*/
		datas = (tk2dSpriteCollectionData)Resources.Load("Box/BoxCollections",typeof(tk2dSpriteCollectionData));
		for(int i=0;i<sIZE;i++){
			for(int j=0;j<sIZE;j++){
				int gap = i*sIZE+j;
				list[gap] = new GameObject(gap.ToString());
				// Save Index(x,y) at Tag
				
				list[gap].AddComponent<tk2dSprite>();
				int t = (int)(Random.value * 6.0f);
				if(t == 6){
					map[gap] = t-1;
				}else{
					map[gap] = t;
				}
				SetGO (list[gap]);
				list[gap].transform.localScale = (new Vector3(1.0f,1.0f,1.0f));
				list[gap].transform.localPosition = (new Vector3((float)(j*10.0f),(float)(i*10.0f),0.1f));
				list[gap].AddComponent<BoxCollider>();
			}
		}
		StartCoroutine("BreakBlock");
	}
	
	// Update is called once per framevoid Update()
	// state of Listenable;
	bool state = false;
	GameObject target;
	
	void Update()
	{
		if(state){
			StartCoroutine("Listener");	
		}
	}
	
	bool iSnear(int f,int s){
		if(Mathf.Abs (f-s) == sIZE){
			return true;
		}
		else if(Mathf.Abs (f-s) == 1){
			if((f % sIZE == 0 && s % sIZE == sIZE-1) ||
				(s % sIZE == 0 && f % sIZE == sIZE-1)){
				return false;
			}
			return true;
		}
		return false;
	}
	
	public void SetGO(GameObject go){
		int a = int.Parse (go.name);
		tk2dSprite sprite = go.GetComponent<tk2dSprite>();
		switch(map[a]){
			case -1 : sprite.SetSprite(datas,"0"); break;                            //breaked;
			case 0: sprite.SetSprite(datas,"1"); break;
			case 1: sprite.SetSprite(datas,"2"); break;
			case 2: sprite.SetSprite(datas,"3"); break;
			case 3: sprite.SetSprite(datas,"4"); break;
			case 4: sprite.SetSprite(datas,"5"); break;
			case 5: sprite.SetSprite(datas,"6"); break;
		}
	}
	
	public GameObject GetTarget()
	{
        RaycastHit hit;
        GameObject target = null; 
    	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		Vector3 start = ray.origin;
		Vector3 direction = ray.direction*(1.0f);
        if( true == (Physics.Raycast(start, direction, out hit))) 
        {
            target = hit.collider.gameObject; 
        } 
        return target; 
	}
	
	
	public IEnumerator Listener(){
		state = false;
		if(Input.GetMouseButtonDown(0)){
			
		}
		if(Input.GetMouseButtonUp(0)){
			if(target == null){
				target = GetTarget();
				if(target != null){
					state = true;
					target.transform.localScale = (new Vector3(1.2f,1.2f,1.2f));
				}
			}
			else{
				GameObject target2;
				target2 = GetTarget();
				if(target2 != null){
					int f,s,temp;
					f = int.Parse(target.name);
					s = int.Parse(target2.name);
					if(iSnear(f,s)){
						temp = map[f];
						map[f] = map[s];
						map[s] = temp;
						
						SetGO (target);
						SetGO (target2);
					}
					else{
						// notification
						
					}
					target.transform.localScale = (new Vector3(1.0f,1.0f,1.0f));
					target = null;
					yield return StartCoroutine("BreakBlock");
				}
			}
		}
		
		
		/*
	    if (Input.GetKeyDown(KeyCode.Space))
	    {
			ob2 = GameObject.Find("Rock");
			ob2.transform.localPosition = (new Vector3(20.0f,20.0f,20.0f));
	    }
		//Mouse Move Event
		
		if(state){
			Vector3 MousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
            target.transform.position = new Vector3(MousePos.x, MousePos.y, target.transform.position.z);
       	}*/
		
		state = true;
		yield return null;
	}
	IEnumerator BreakBlock(){ //breaking all combo 
		//breaking is only breaking one time;
		Breaking b = gameObject.GetComponent<Breaking>();
		b.input(map,list,datas);
		b.Notice = true;
		while(b.Notice){
			yield return b.StartCoroutine("breaking");
		}
		state = true; // it will be trigger to work listener 
		map = b.map;
		yield return null;
	}
}
