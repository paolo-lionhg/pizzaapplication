using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Experimental.Networking;

public class ToppingsHandler : MonoBehaviour
{
	[System.Serializable]
	public class ToppingsClass
	{
		public float cheesePrince;
		public float pepperoniPrice;
		public float vegetarianPrice;

		public int pepperoniQty;
		public int onionsQty;
		public int peppersQty;
		public int sausageQty;
		public int tomatoesQty;

		public float additionalToppingsPrice;
	}

	public static ToppingsClass jsonData;

	public Text testText;

	IEnumerator GetJSONData()
	{
		UnityWebRequest www = UnityWebRequest.Get(Application.absoluteURL + "/toppings.json");
		yield return www.Send();
		
		if (www.isError)
		{
			Debug.Log(www.error);
			//testText.text = "ERROR: " + www.error;

			ToppingsClass tc = new ToppingsClass();
			tc.additionalToppingsPrice = 1.03f;
			tc.cheesePrince = 9.96f;
			tc.pepperoniPrice = 10.98f;
			tc.vegetarianPrice = 9.97f;
			tc.pepperoniQty = 1;
			tc.onionsQty = 1;
			tc.peppersQty = 1;
			tc.sausageQty = 1;
			tc.tomatoesQty = 1;

			jsonData = tc;
        }
		else
		{
			// Show results as text
			Debug.Log(www.downloadHandler.text);
			//testText.text = "RESULT: " + www.downloadHandler.text;
			jsonData = JsonUtility.FromJson<ToppingsClass>(www.downloadHandler.text);

			// Or retrieve results as binary data
			byte[] results = www.downloadHandler.data;
		}
	}
	
	// Use this for initialization
	void Start()
	{
		/*
		ToppingsClass tc = new ToppingsClass();
		tc.additionalToppingsPrice = 1.00f;
		tc.cheesePrince = 9.99f;
		tc.pepperoniPrice = 10.99f;
		tc.vegetarianPrice = 9.99f;
		tc.pepperoniQty = 10;
		tc.onionsQty = 10;
		tc.peppersQty = 10;
		tc.sausageQty = 10;
		tc.tomatoesQty = 10;


		string json = JsonUtility.ToJson(tc);
		Debug.Log(json);
		*/
		
		StartCoroutine(GetJSONData());
	}

	// Update is called once per frame
	void Update()
	{

	}
}
