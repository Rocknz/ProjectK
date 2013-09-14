using UnityEngine;
using System.Collections;

public class Breaking : MonoBehaviour{
	static int sIZE = 8;
	public bool Notice = true; // is breaking box ?
	public int[] map = new int[sIZE*sIZE+1];
	public int[] Breakmap = new int[sIZE*sIZE+1];
	public GameObject[] list = new GameObject[sIZE*sIZE+1];
	tk2dSpriteCollectionData datas;
	
	public void input(int[] m,GameObject[] l,tk2dSpriteCollectionData data){
		this.map = m;	
		this.list = l;
		this.datas = data;
	}
	public IEnumerator breaking(){
		
		Notice = false;
		Breakis();
		Breakblock();
		
		if(Notice){
			yield return StartCoroutine("BreakingAni"); // animation;
			yield return StartCoroutine("DownAni");
		}
		yield return null;
	}
	public void go(int x,int y,int px,int py){
		int cnt = 0;
		int xx,yy;
		int now;
		xx = x;
		yy = y;
		now = map[x*sIZE+y];
		while(map[xx*sIZE+yy] == now){
			cnt ++;
			xx += px;
			yy += py;
			if( xx < 0 || xx >= sIZE || yy < 0 || yy >=sIZE)
				break;
		}
		if(cnt >= 3){
			xx = x;
			yy = y;
			while(map[xx*sIZE+yy] == now){
				Breakmap[xx*sIZE+yy] = 1;
				xx += px;
				yy += py;
				if( xx < 0 || xx >= sIZE || yy < 0 || yy >=sIZE)
					break;
			}
		}
	}
	public void Breakis(){
		int i,j;
		for(i=0;i<sIZE;i++){
			for(j=0;j<sIZE;j++){
				Breakmap[i*sIZE+j] = 0;
			}
		}
		for(i=0;i<sIZE;i++){
			for(j=0;j<sIZE;j++){
				go(i,j,0,1);
				go(i,j,0,-1);
				go(i,j,1,0);
				go(i,j,-1,0);
			}
		}
	}
	public void Breakblock(){
		int i;
		for(i=0;i<sIZE*sIZE;i++){
			if(Breakmap[i] == 1){
				map[i] = -1;
				Notice = true;
			}
		}
	}
	public IEnumerator DownAni(){
		int i;
		bool check = true;
		while(check){
			check = false;
			for(i=0;i<sIZE*sIZE;i++){
				if(map[i] == -1){
					check = true;
					if(i >= (sIZE*sIZE-sIZE)){
						int t = (int)(Random.value * 6.0f);
						if(t == 6){
							map[i] = t-1;
						}else{
							map[i] = t;
						}
					}
					else{
						map[i] = map[i+sIZE];
						map[i+sIZE] = -1;
					}
				}
			}
			if(check){
				yield return StartCoroutine("showMap");
			}
		}
	}
	public IEnumerator showMap(){
		for(int i=0;i<sIZE*sIZE;i++){
			SetGO (list[i]);	
		}
		yield return new WaitForSeconds(1.0f);
	}
	public IEnumerator BreakingAni(){
		
		yield return StartCoroutine("showMap");
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
}
