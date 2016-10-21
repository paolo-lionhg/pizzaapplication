using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectPizza : MonoBehaviour
{
	public GameObject cheese;
	public GameObject pepperoni;
	public GameObject vegetarian;
	public GameObject custom;
	public Text totalPrice;

	public Toggle pepperoniToggle;
	public Toggle onionsToggle;
	public Toggle peppersToggle;
	public Toggle sausageToggle;
	public Toggle tomatoesToggle;

	public GameObject customHolder;

	public GameObject menu;

	public GameObject warningMenu;

	public Sprite cheeseTitle;
	public Sprite pepperoniTitle;
	public Sprite vegetarianTitle;
	public Image customTitleImg;

	GameObject target;
	Vector3 targetPrevPos;
	float price = 0;
	bool[] toggleChecksForSaveWarning = new bool[5];
	int[] toppingsQty = new int[5];

	enum eSelectedPizza
	{
		cheese,
		pepeperoni,
		vegetarian,
	}
	eSelectedPizza selectedPizza;

	void ProcessToggle(Toggle p_toggle)
	{
		if (p_toggle.interactable)
		{
			if (p_toggle.isOn)
				price += ToppingsHandler.jsonData.additionalToppingsPrice;
			else
				price -= ToppingsHandler.jsonData.additionalToppingsPrice;

			totalPrice.text = "$" + price.ToString("F2");
		}

	}

	public void ButtonPepperoniToggle()
	{
		//Debug.Log("TOGGLE PEPPERONI");
		ProcessToggle(pepperoniToggle);
	}

	public void ButtonOnionsToggle()
	{
		//Debug.Log("TOGGLE ONOINS");
		ProcessToggle(onionsToggle);
	}

	public void ButtonPeppersToggle()
	{
		//Debug.Log("TOGGLE PEPPERS");
		ProcessToggle(peppersToggle);
	}

	public void ButtonSausageToggle()
	{
		//Debug.Log("TOGGLE SAUSAGE");
		ProcessToggle(sausageToggle);
	}

	public void ButtonTomatoesToggle()
	{
		//Debug.Log("TOGGLE TOMATOES");
		ProcessToggle(tomatoesToggle);
	}

	void DisableToppingsThatAreOutOfStock()
	{
		if (ToppingsHandler.jsonData.pepperoniQty == 0)
			pepperoniToggle.interactable = false;
		if (ToppingsHandler.jsonData.onionsQty == 0)
			onionsToggle.interactable = false;
		if (ToppingsHandler.jsonData.peppersQty == 0)
			peppersToggle.interactable = false;
		if (ToppingsHandler.jsonData.sausageQty == 0)
			sausageToggle.interactable = false;
		if (ToppingsHandler.jsonData.tomatoesQty == 0)
			tomatoesToggle.interactable = false;
    }

	void InitCustomizingPizza()
	{
		toppingsQty = new int[5];
		toppingsQty[0] = ToppingsHandler.jsonData.pepperoniQty;
		toppingsQty[1] = ToppingsHandler.jsonData.onionsQty;
		toppingsQty[2] = ToppingsHandler.jsonData.peppersQty;
		toppingsQty[3] = ToppingsHandler.jsonData.sausageQty;
		toppingsQty[4] = ToppingsHandler.jsonData.tomatoesQty;

		pepperoniToggle.isOn = false;
		onionsToggle.isOn = false;
		peppersToggle.isOn = false;
		sausageToggle.isOn = false;
		tomatoesToggle.isOn = false;

		pepperoniToggle.interactable = true;
		onionsToggle.interactable = true;
		peppersToggle.interactable = true;
		sausageToggle.interactable = true;
		tomatoesToggle.interactable = true;

		toggleChecksForSaveWarning[0] = false;
		toggleChecksForSaveWarning[1] = false;
		toggleChecksForSaveWarning[2] = false;
		toggleChecksForSaveWarning[3] = false;
		toggleChecksForSaveWarning[4] = false;

		DisableToppingsThatAreOutOfStock();
	}

	public void SelectCheese()
	{
		StopAllCoroutines();

		InitCustomizingPizza();

		target = cheese;
		StartAnimation();
		price = ToppingsHandler.jsonData.cheesePrince;
		totalPrice.text = "$" + price.ToString("F2");
		
		selectedPizza = eSelectedPizza.cheese;
	}

	public void SelectPepperoni()
	{
		StopAllCoroutines();

		InitCustomizingPizza();

		toppingsQty[0]--;
		if (toppingsQty[0] <= 0)
			pepperoniToggle.interactable = false;

		target = pepperoni;
		StartAnimation();
		price = ToppingsHandler.jsonData.pepperoniPrice;
		totalPrice.text = "$" + price.ToString("F2");

		selectedPizza = eSelectedPizza.pepeperoni;
	}

	public void SelectVegetarian()
	{
		StopAllCoroutines();

		InitCustomizingPizza();

		toppingsQty[1]--;
		if (toppingsQty[1] <= 0)
			onionsToggle.interactable = false;
		toppingsQty[2]--;
		if (toppingsQty[2] <= 0)
			peppersToggle.interactable = false;

		target = vegetarian;
		StartAnimation();
		price = ToppingsHandler.jsonData.vegetarianPrice;
		totalPrice.text = "$" + price.ToString("F2");

		selectedPizza = eSelectedPizza.vegetarian;
	}

	public void SelectCustom()
	{
		StopAllCoroutines();

		InitCustomizingPizza();

		pepperoniToggle.isOn = (PlayerPrefs.GetInt("EXTRA_PEPPERONI", 0) != 0);
		onionsToggle.isOn = (PlayerPrefs.GetInt("EXTRA_ONIONS", 0) != 0);
		peppersToggle.isOn = (PlayerPrefs.GetInt("EXTRA_PEPPERS", 0) != 0);
		sausageToggle.isOn = (PlayerPrefs.GetInt("EXTRA_SAUSAGE", 0) != 0);
		tomatoesToggle.isOn = (PlayerPrefs.GetInt("EXTRA_TOMATO", 0) != 0);



		int customPizza = PlayerPrefs.GetInt("CUSTOM_PIZZA", -1);
		if (customPizza != -1)
		{
			switch (customPizza)
			{
			case (int)eSelectedPizza.cheese:
				price = ToppingsHandler.jsonData.cheesePrince;
				break;
			case (int)eSelectedPizza.pepeperoni:
				price = ToppingsHandler.jsonData.pepperoniPrice;
				toppingsQty[0]--;
				break;
			case (int)eSelectedPizza.vegetarian:
				price = ToppingsHandler.jsonData.vegetarianPrice;
				toppingsQty[1]--;
				toppingsQty[2]--;
				break;
			}
		}



		if (toppingsQty[0] <= 0)
		{
			pepperoniToggle.isOn = false;
			pepperoniToggle.interactable = false;
		}
		else
			price += (PlayerPrefs.GetInt("EXTRA_PEPPERONI", 0) == 1) ? ToppingsHandler.jsonData.additionalToppingsPrice : 0;
		if (toppingsQty[1] <= 0)
		{
			onionsToggle.isOn = false;
			onionsToggle.interactable = false;
		}
		else
			price += (PlayerPrefs.GetInt("EXTRA_ONIONS", 0) == 1) ? ToppingsHandler.jsonData.additionalToppingsPrice : 0;
		if (toppingsQty[2] <= 0)
		{
			peppersToggle.isOn = false;
			peppersToggle.interactable = false;
		}
		else
			price += (PlayerPrefs.GetInt("EXTRA_PEPPERS", 0) == 1) ? ToppingsHandler.jsonData.additionalToppingsPrice : 0;
		if (toppingsQty[3] <= 0)
		{
			sausageToggle.isOn = false;
			sausageToggle.interactable = false;
		}
		else
			price += (PlayerPrefs.GetInt("EXTRA_SAUSAGE", 0) == 1) ? ToppingsHandler.jsonData.additionalToppingsPrice : 0;
		if (toppingsQty[4] <= 0)
		{
			tomatoesToggle.isOn = false;
			tomatoesToggle.interactable = false;
		}
		else
			price += (PlayerPrefs.GetInt("EXTRA_TOMATO", 0) == 1) ? ToppingsHandler.jsonData.additionalToppingsPrice : 0;

		target = custom;
		StartAnimation();

		
		totalPrice.text = "$" + price.ToString("F2");

		

		totalPrice.text = "$" + price.ToString("F2");

		toggleChecksForSaveWarning[0] = pepperoniToggle.isOn;
		toggleChecksForSaveWarning[1] = onionsToggle.isOn;
		toggleChecksForSaveWarning[2] = peppersToggle.isOn;
		toggleChecksForSaveWarning[3] = sausageToggle.isOn;
		toggleChecksForSaveWarning[4] = tomatoesToggle.isOn;

		DisableToppingsThatAreOutOfStock();

		selectedPizza = (eSelectedPizza)PlayerPrefs.GetInt("CUSTOM_PIZZA", -1);
	}

	IEnumerator MoveMe(GameObject p_go, Vector3 p_dest)
	{
		RectTransform rect = p_go.GetComponent<RectTransform>();

		while (true)
		{
			yield return new WaitForFixedUpdate();
			rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, p_dest, Time.deltaTime * 10);

			if (Vector3.Distance(rect.anchoredPosition, p_dest) < 0.01f)
				break;
		}
	}

	IEnumerator CenterTarget()
	{
		Vector2 posDest = new Vector2(0, 20);
		Vector2 scaleDest = new Vector2(2100, 1100);
		RectTransform rect = target.GetComponent<RectTransform>();
		targetPrevPos = rect.anchoredPosition;

		while (true)
		{
			yield return new WaitForFixedUpdate();
			rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, posDest, Time.deltaTime * 10);
			rect.sizeDelta = Vector2.Lerp(rect.sizeDelta, scaleDest, Time.deltaTime * 5f);

			if (Vector3.Distance(rect.anchoredPosition, posDest) < 0.01f)
				break;
		}

		rect.anchoredPosition = posDest;
		rect.sizeDelta = scaleDest;
	}

	IEnumerator FadeMe(GameObject p_go)
	{
		Image img = p_go.GetComponent<Image>();

		while (true)
		{
			yield return new WaitForFixedUpdate();
			img.color = Color.Lerp(img.color, Color.clear, Time.deltaTime * 5);

			if (Mathf.Abs(img.color.a - 0) < 0.01f)
				break;
		}

		img.color = Color.clear;

	}

	IEnumerator SlideInCustom()
	{
		float speed = 10;
		RectTransform customRectTrans = customHolder.GetComponent<RectTransform>();
		Vector2 destPos = new Vector2(10, 0);
		while (true)
		{
			yield return new WaitForFixedUpdate();

			customRectTrans.anchoredPosition = Vector2.Lerp(customRectTrans.anchoredPosition, destPos, Time.deltaTime * speed);

			if (Vector2.Distance(customRectTrans.anchoredPosition, destPos) < 0.35f)
				break;
		}

		destPos = new Vector2(0, 0);
		while (true)
		{
			yield return new WaitForFixedUpdate();

			customRectTrans.anchoredPosition = Vector2.Lerp(customRectTrans.anchoredPosition, destPos, Time.deltaTime * speed);

			if (Vector2.Distance(customRectTrans.anchoredPosition, destPos) < 0.35f)
				break;
		}
		customRectTrans.anchoredPosition = destPos;
	}

	void StartAnimation()
	{
		if (target != cheese)
			StartCoroutine(MoveMe(cheese, new Vector2(-3000, cheese.GetComponent<RectTransform>().anchoredPosition.y)));
		if (target != pepperoni)
			StartCoroutine(MoveMe(pepperoni, new Vector2(3000, pepperoni.GetComponent<RectTransform>().anchoredPosition.y)));
		if (target != vegetarian)
			StartCoroutine(MoveMe(vegetarian, new Vector2(-3000, vegetarian.GetComponent<RectTransform>().anchoredPosition.y)));
		if (target != custom)
			StartCoroutine(MoveMe(custom, new Vector2(3000, custom.GetComponent<RectTransform>().anchoredPosition.y)));

		StartCoroutine(CenterTarget());
		StartCoroutine(FadeMe(menu));

		StartCoroutine(SlideInCustom());

		target.GetComponent<Button>().enabled = false;
	}

	IEnumerator SlideOutCustom()
	{
		float speed = 10;
		RectTransform customRectTrans = customHolder.GetComponent<RectTransform>();
		Vector3 destPos = new Vector2(-3000, 0);

		while (true)
		{
			yield return new WaitForFixedUpdate();

			customRectTrans.anchoredPosition = Vector3.Lerp(customRectTrans.anchoredPosition, destPos, Time.deltaTime * speed);

			if (Vector3.Distance(customRectTrans.anchoredPosition, destPos) < 0.1f)
				break;
		}
		customRectTrans.anchoredPosition = destPos;
	}

	IEnumerator FadeInMe(GameObject p_go)
	{
		Image img = p_go.GetComponent<Image>();

		while (true)
		{
			yield return new WaitForFixedUpdate();
			img.color = Color.Lerp(img.color, Color.white, Time.deltaTime * 5);

			if (Mathf.Abs(img.color.a - 1) < 0.01f)
				break;
		}

		img.color = Color.white;
	}

	IEnumerator ReturnTarget()
	{
		Vector3 posDest = targetPrevPos;
		Vector3 scaleDest = new Vector2(1538, 776);
		RectTransform rect = target.GetComponent<RectTransform>();

		while (true)
		{
			yield return new WaitForFixedUpdate();
			rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, posDest, Time.deltaTime * 10);
			rect.sizeDelta = Vector3.Lerp(rect.sizeDelta, scaleDest, Time.deltaTime * 5f);

			if (Vector3.Distance(rect.anchoredPosition, posDest) < 0.01f)
				break;
		}

		rect.anchoredPosition = posDest;
		rect.sizeDelta = scaleDest;
	}

	IEnumerator PopInWarning()
	{
		RectTransform rectTrans = warningMenu.GetComponent<RectTransform>();
		Vector3 destScale = Vector3.one * 1.02f;
		while (true)
		{
			yield return new WaitForFixedUpdate();

			rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, destScale, Time.deltaTime * 15);

			if (Vector3.Distance(rectTrans.localScale, destScale) < 0.01f)
				break;
		}
		rectTrans.localScale = destScale;


		destScale = Vector3.one * 0.95f;
		while (true)
		{
			yield return new WaitForFixedUpdate();

			rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, destScale, Time.deltaTime * 15);

			if (Vector3.Distance(rectTrans.localScale, destScale) < 0.01f)
				break;
		}
		rectTrans.localScale = destScale;

		destScale = Vector3.one * 1.0f;
		while (true)
		{
			yield return new WaitForFixedUpdate();

			rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, destScale, Time.deltaTime * 15);

			if (Vector3.Distance(rectTrans.localScale, destScale) < 0.01f)
				break;
		}
		rectTrans.localScale = destScale;
	}

	public void ButtonBackAnimation(bool p_canShowWarning)
	{
		StopAllCoroutines();

		if (p_canShowWarning)
		{
			if (toggleChecksForSaveWarning[0] != pepperoniToggle.isOn)
			{
				warningMenu.SetActive(true);
				StartCoroutine(PopInWarning());
				return;
			}
			else if (toggleChecksForSaveWarning[1] != onionsToggle.isOn)
			{
				warningMenu.SetActive(true);
				StartCoroutine(PopInWarning());
				return;
			}
			else if (toggleChecksForSaveWarning[2] != peppersToggle.isOn)
			{
				warningMenu.SetActive(true);
				StartCoroutine(PopInWarning());
				return;
			}
			else if (toggleChecksForSaveWarning[3] != sausageToggle.isOn)
			{
				warningMenu.SetActive(true);
				StartCoroutine(PopInWarning());
				return;
			}
			else if (toggleChecksForSaveWarning[4] != tomatoesToggle.isOn)
			{
				warningMenu.SetActive(true);
				StartCoroutine(PopInWarning());
				return;
			}
		}

		target.GetComponent<Button>().enabled = true;

		StartCoroutine(SlideOutCustom());
		StartCoroutine(FadeInMe(menu));

		StartCoroutine(ReturnTarget());

		if (target != cheese)
			StartCoroutine(MoveMe(cheese, new Vector2(-150, cheese.GetComponent<RectTransform>().anchoredPosition.y)));
		if (target != pepperoni)
			StartCoroutine(MoveMe(pepperoni, new Vector2(150, pepperoni.GetComponent<RectTransform>().anchoredPosition.y)));
		if (target != vegetarian)
			StartCoroutine(MoveMe(vegetarian, new Vector2(-150, vegetarian.GetComponent<RectTransform>().anchoredPosition.y)));
		if (target != custom)
			StartCoroutine(MoveMe(custom, new Vector2(150, custom.GetComponent<RectTransform>().anchoredPosition.y)));

		int customPizza = PlayerPrefs.GetInt("CUSTOM_PIZZA", -1);
		//Debug.Log("CUSTOM PIZZA STUFF: " + customPizza);
		if (customPizza != -1)
		{
			custom.GetComponent<Button>().interactable = true;

			customTitleImg.gameObject.SetActive(true);

			Sprite title = null;
			switch (customPizza)
			{
			case (int)eSelectedPizza.cheese:
				title = cheeseTitle;
				break;
			case (int)eSelectedPizza.pepeperoni:
				title = pepperoniTitle;
				break;
			case (int)eSelectedPizza.vegetarian:
				title = vegetarianTitle;
				break;
			}
			customTitleImg.sprite = title;
		}
	}

	public void ButtonSave()
	{
		PlayerPrefs.SetInt("CUSTOM_PIZZA", (int)selectedPizza);
		PlayerPrefs.SetInt("EXTRA_PEPPERONI", (pepperoniToggle.isOn) ? 1 : 0);
		PlayerPrefs.SetInt("EXTRA_ONIONS", (onionsToggle.isOn) ? 1 : 0);
		PlayerPrefs.SetInt("EXTRA_PEPPERS", (peppersToggle.isOn) ? 1 : 0);
		PlayerPrefs.SetInt("EXTRA_SAUSAGE", (sausageToggle.isOn) ? 1 : 0);
		PlayerPrefs.SetInt("EXTRA_TOMATO", (tomatoesToggle.isOn) ? 1 : 0);

		toggleChecksForSaveWarning[0] = pepperoniToggle.isOn;
		toggleChecksForSaveWarning[1] = onionsToggle.isOn;
		toggleChecksForSaveWarning[2] = peppersToggle.isOn;
		toggleChecksForSaveWarning[3] = sausageToggle.isOn;
		toggleChecksForSaveWarning[4] = tomatoesToggle.isOn;

		PlayerPrefs.Save();
	}

	IEnumerator PopOutWarning(bool p_shouldExitToMenu)
	{
		RectTransform rectTrans = warningMenu.GetComponent<RectTransform>();
		Vector3 destScale = Vector3.one * 1.1f;
		while (true)
		{
			yield return new WaitForFixedUpdate();

			rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, destScale, Time.deltaTime * 15);

			if (Vector3.Distance(rectTrans.localScale, destScale) < 0.01f)
				break;
		}
		rectTrans.localScale = destScale;


		destScale = Vector3.one * 0.01f;
		while (true)
		{
			yield return new WaitForFixedUpdate();

			rectTrans.localScale = Vector3.Lerp(rectTrans.localScale, destScale, Time.deltaTime * 15);

			if (Vector3.Distance(rectTrans.localScale, destScale) < 0.01f)
				break;
		}
		rectTrans.localScale = destScale;

		warningMenu.SetActive(false);

		if (p_shouldExitToMenu)
			ButtonBackAnimation(false);
	}

	public void ButtonCancelWarning()
	{
		StopAllCoroutines();
		StartCoroutine(PopOutWarning(false));
	}

	public void ButtonSaveWarning()
	{
		StopAllCoroutines();

		ButtonSave();

		StartCoroutine(PopOutWarning(false));
	}

	public void ButtonBackToMenuWarning()
	{
		StopAllCoroutines();

		StartCoroutine(PopOutWarning(true));


	}

	IEnumerator CheckQtyForPizza()
	{
		while(true)
		{
			yield return new WaitForFixedUpdate();

			if (ToppingsHandler.jsonData != null)
				break;
		}

		if (ToppingsHandler.jsonData.pepperoniQty <= 0)
		{
			pepperoni.GetComponent<Button>().interactable = false;
			pepperoni.transform.GetChild(1).gameObject.SetActive(true);
		}
		if (ToppingsHandler.jsonData.onionsQty <= 0 || ToppingsHandler.jsonData.peppersQty <= 0)
		{
			vegetarian.GetComponent<Button>().interactable = false;
			vegetarian.transform.GetChild(1).gameObject.SetActive(true);
		}
	}

	// Use this for initialization
	void Start()
	{
		//PlayerPrefs.DeleteAll();
		int customPizza = PlayerPrefs.GetInt("CUSTOM_PIZZA", -1);

		if (customPizza != -1)
		{
			custom.GetComponent<Button>().interactable = true;

			customTitleImg.gameObject.SetActive(true);

			Sprite title = null;
			switch (customPizza)
			{
			case (int)eSelectedPizza.cheese:
				title = cheeseTitle;
				break;
			case (int)eSelectedPizza.pepeperoni:
				title = pepperoniTitle;
				break;
			case (int)eSelectedPizza.vegetarian:
				title = vegetarianTitle;
				break;
			}
			customTitleImg.sprite = title;
		}

		StartCoroutine(CheckQtyForPizza());

	}

	// Update is called once per frame
	void Update()
	{

	}
}