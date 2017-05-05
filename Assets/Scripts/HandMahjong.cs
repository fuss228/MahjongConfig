using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HandMahjong : MonoBehaviour {


	public Image face;
	public Button btn;
	// Use this for initialization
	int mjVal = -1;
	int playerIndex;


	void Start () {
		btn.onClick.AddListener (OnHandMJClicked);
	}


	public void SetMJFace(int val, int userindex)
	{
		string faceVal = setMJImage (val);
		Texture2D img = Resources.Load("Mahjong/"+faceVal) as Texture2D;
		Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));//后面Vector2就是你Anchors的Pivot的x/y属性值
		face.sprite = sp;
		playerIndex = userindex;
	}


	public string setMJImage (int val)
	{
		mjVal = val;
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



	//remove
	void OnHandMJClicked()
	{
		string mjkey = "";
		if (mjVal >= 136) {
			mjkey =  (mjVal - 102).ToString();
		} else {
			mjkey = (mjVal /4).ToString();
		}

		ConfigView.instance.ResetMJCount (mjkey);
		ConfigView.instance.handMjList [playerIndex].Remove (mjVal);
		ConfigView.instance.RefreshPlayerHandMJP (playerIndex);
//		Destroy (gameObject);
	}


}
