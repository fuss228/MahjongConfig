using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class SHQMComparerMahjongValue : IComparer
{
	int IComparer.Compare (object x, object y)
	{
		return (int)x > (int)y ? -1 : 1;
	}
}


public class ConfigView : MonoBehaviour {

	public GameObject mjPredab;
	public GameObject handmjPredab;

	private SHQMComparerMahjongValue sortValue;
	//		HandMahjongValues.Sort (sortValue);

	public static ConfigView instance;//回调使用


	public int currentSetPlayerIndex = 0;

	public ArrayList[] handMjList = new ArrayList[4];		//存的是值
	public ArrayList[] handMjObj = new ArrayList[4];		//存GameObject



	public Button[] playerBtns;

	public Button btnCreate;
	public Button btnReset;
    public Button btnExit;


	public Dictionary<string, GameObject> dicAllMj = new Dictionary<string, GameObject>();


	void Start () {
		instance = this;
		InitMahjong ();
		for (int i = 0; i < handMjList.Length; i++) {
			handMjList [i] = new ArrayList ();
			handMjObj [i] = new ArrayList ();
		}

		for (int i = 0; i < playerBtns.Length; i++) {
			EventTriggerListener.Get(playerBtns [i].gameObject).onClick = SetCurrentPlayerIndex;

		}

		playerBtns [0].gameObject.GetComponent<Image> ().color = Color.green;
		btnCreate.onClick.AddListener (OnCreateButtonClicked);
		btnReset.onClick.AddListener (OnResetButtonClicked);
        btnExit.onClick.AddListener(OnExitButtonClicked);
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void InitMahjong()
	{
		int x, y;
		for (int i = 0; i < 42; i++) {
			x = i % 18;
			y = i / 18;



			GameObject mjp = GameObject.Instantiate (mjPredab);
			mjp.transform.SetParent (transform, false);
			mjp.transform.localPosition = new Vector3 (-485+(50*x), 256-(68*y), 0);
			mjp.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
			mjp.name = i.ToString ();
	

			if (i > 33) {
				Texture2D img = Resources.Load("Mahjong/"+(102+i).ToString()) as Texture2D;
				Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));//后面Vector2就是你Anchors的Pivot的x/y属性值

				mjp.GetComponent<Mahjong> ().selfUImj.sprite= sp;

				mjp.transform.localPosition = new Vector3 (-485+(50*(i-34)), 120, 0);



				//			mjp.GetComponent<Mahjong>().setMahjongValueState(sp);
			} else {
				Texture2D img = Resources.Load("Mahjong/"+(i*4).ToString()) as Texture2D;
				Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.5f, 0.5f));//后面Vector2就是你Anchors的Pivot的x/y属性值

				mjp.GetComponent<Mahjong> ().selfUImj.sprite= sp;
				//			mjp.GetComponent<Mahjong>().setMahjongValueState(sp);
			}

			dicAllMj.Add (i.ToString (), mjp);
		}
	
	}


	public void CreateHandMJ(int faceVal)
	{
//		int index = handMjList [currentSetPlayerIndex].Count;
//		GameObject mjp = GameObject.Instantiate (handmjPredab);
//		mjp.transform.SetParent (transform, false);
//		mjp.transform.localPosition = new Vector3 (-400+(50*index), 0-(currentSetPlayerIndex * 80), 0);
//		mjp.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
//		mjp.GetComponent<HandMahjong> ().SetMJFace (faceVal,currentSetPlayerIndex);
//		mjp.SetActive (false);
//		mjp.SetActive (true);
//		handMjList [currentSetPlayerIndex].Add (faceVal);
//		handMjObj [currentSetPlayerIndex].Add (mjp);


		handMjList [currentSetPlayerIndex].Add (faceVal);
		handMjList [currentSetPlayerIndex].Sort (sortValue);
		RefreshPlayerHandMJP (currentSetPlayerIndex);


	}


	void SetCurrentPlayerIndex (GameObject obj)
	{
		int currentClickedButtonName = int.Parse (obj.name);
		Debug.Log (currentClickedButtonName);
		ReseBtnBG ();
		playerBtns [currentClickedButtonName].gameObject.GetComponent<Image> ().color = Color.green;
		currentSetPlayerIndex = currentClickedButtonName;

	}



	void ReseBtnBG()
	{
		for (int i = 0; i < playerBtns.Length; i++) {
			playerBtns [i].gameObject.GetComponent<Image> ().color = Color.white;
			
		}
	}



	public void ResetMJCount(string mjKey)
	{
		GameObject mjp = dicAllMj [mjKey];
		mjp.GetComponent<Mahjong> ().clickedCount--;
	}


	public void RefreshPlayerHandMJP(int playerindex)
	{
		foreach (GameObject item in handMjObj[playerindex]) {
			Destroy (item);
		}


		ArrayList handcountVal = handMjList [playerindex];
		handcountVal.Sort (sortValue);

		for (int i = 0; i <handcountVal.Count; i++) {

			int faceVal = (int)handcountVal [i];
			int index = i;

			GameObject mjp = GameObject.Instantiate (handmjPredab);
			mjp.transform.SetParent (transform, false);
			mjp.transform.localPosition = new Vector3 (-400+(50*index), 0-(playerindex * 80), 0);
			mjp.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
			mjp.GetComponent<HandMahjong> ().SetMJFace (faceVal,playerindex);
			mjp.SetActive (false);
			mjp.SetActive (true);
			handMjObj [playerindex].Add (mjp);


			
		}


	}

	void OnCreateButtonClicked()
	{

		//生成144张顺序的麻将牌
		List<int> allmj = new List<int> ();
		for (int i = 0; i < 144; i++) {
			allmj.Add (i);
		}

		//将已配置的麻将牌 从144张中移除
		for (int i = 0; i < 4; i++) {
			if (handMjList[i].Count>0) {
				for (int x = 0; x < handMjList[i].Count; x++) {
					allmj.Remove ((int)handMjList [i][x]);
				}
			}
		}

		//将剩余的麻将牌打乱
		List<int> ranlist = ListRandom(allmj);

		//检查4家不足13张的 补齐
		for (int i = 0; i < 4; i++) {
			if (handMjList [i].Count < 13) {
				for (int j = handMjList [i].Count; j < 13; j++) {
					handMjList [i].Add (ranlist [0]);
					ranlist.RemoveAt (0);
				}			
			}
		}

		Debug.Log (ranlist.Count);

		StringBuilder MyStringBuilder = new StringBuilder ();
		for (int i = 0; i < 4; i++) {
			for (int j= 0; j < 13; j++) {
				MyStringBuilder.Append (handMjList [i] [j].ToString ()+",");
			}
		}
		for (int i = 0; i < ranlist.Count; i++) {
			MyStringBuilder.Append (ranlist [i].ToString () + ",");
		}


		string configStr = MyStringBuilder.ToString ().Substring (0, MyStringBuilder.Length - 1);

		Debug.Log(configStr);


		StreamWriter sw;
//		string iptext = Application.persistentDataPath + "/" + "ipText.txt"
		string iptext = Application.dataPath + "/" + "config.txt";;
		if (!File.Exists (iptext)) {
			//sw = File.CreateText(iptext);
			FileStream file = new FileStream (iptext, FileMode.Create, FileAccess.ReadWrite);
			sw = new StreamWriter (file);
			sw.AutoFlush = true;
			sw.WriteLine (configStr);

			sw.Close ();
			file.Close ();
		} else {
			File.Delete(iptext);
			FileStream file = new FileStream(iptext, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			sw = new StreamWriter(file);
			sw.AutoFlush = true;
			sw.WriteLine (configStr);
			sw.Close ();
			file.Close ();
		}


	}



	List<int> CreateALlMJVal()
	{
		List<int> allmj = new List<int> ();
		for (int i = 0; i < 144; i++) {
			allmj.Add (i);
		}

		return ListRandom (allmj);
	}


	/// <summary>
	/// 随机排列数组元素
	/// </summary>
	/// <param name="myList"></param>
	/// <returns></returns>
	private List<int> ListRandom(List<int> myList)
	{
		System.Random ran = new System.Random ();
		List<int> newList = new List<int>();
		int index = 0;
		int temp = 0;
		for (int i = 0; i < myList.Count; i++)
		{

			index = ran.Next(0, myList.Count-1);
			if (index != i)
			{
				temp = myList[i];
				myList[i] = myList[index];
				myList[index] = temp;
			}
		}
		return myList;
	}




	void OnResetButtonClicked()
	{
		ReseBtnBG ();
		currentSetPlayerIndex = 0;
		playerBtns [0].gameObject.GetComponent<Image> ().color = Color.green;

		for (int i = 0; i < 4; i++) {
			handMjList [i].Clear ();
			handMjList [i] = new ArrayList ();

			foreach (GameObject item in handMjObj[i]) {
				Destroy (item);
			}
			handMjObj [i].Clear ();
			handMjObj[i] = new ArrayList ();

		}


        foreach (string key  in dicAllMj.Keys)
        {
            GameObject mjp = dicAllMj [key];
            mjp.GetComponent<Mahjong>().clickedCount = 0;
        }
			


	}


    void OnExitButtonClicked()
    {
        Application.Quit();
        Debug.Log("exit");
    }
}
