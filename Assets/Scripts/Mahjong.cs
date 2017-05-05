using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Mahjong : MonoBehaviour {

	public Image selfUImj;			//UI牌面
	public Image selfUIbg;			//UI牌背


	public int clickedCount =0 ;
	void Start()
	{
		if (selfUIbg !=null) {
			selfUIbg.GetComponent<Button>().onClick.AddListener (OnClick);
		}
	}





	private int mjValue;

	public int MahjongValue {
		get {
			return mjValue;
		}
		set {
			mjValue = value;

		}
	}

	//设置麻将牌面
	public void setMahjongValueState (Sprite sp)
	{
		selfUImj.sprite = sp;
	


	}

	public string setMJImage (int val)
	{
		string imagesname = "0";
		if (val >= 0 && val < 136) {
			if (val % 4 == 0) {
				imagesname = val.ToString ();
			} else {
				imagesname = ((val / 4) * 4).ToString ();
			}		

		} else if (val >= 136 && val <= 143) {//8花  136---143
			imagesname = val.ToString ();
		}

		if (val < 0 || val == 144) {
			imagesname = "-1";
		}		
		return imagesname;
	}



	void OnClick ()
	{	
		if (ConfigView.instance.handMjList[ConfigView.instance.currentSetPlayerIndex].Count >=13) {
			return;
		}


		int objVal = int.Parse (gameObject.name);

		if (clickedCount >3 && objVal< 34) {
			return;
		}

		if (objVal>= 34 && clickedCount>0) {
			return;
		}

		int mjVal =objVal*4 + clickedCount;

		if ( objVal>= 34) {
			mjVal = objVal+ 102;
		}

		Debug.Log (mjVal);
		ConfigView.instance.CreateHandMJ (mjVal);

		clickedCount++;


	}





}
