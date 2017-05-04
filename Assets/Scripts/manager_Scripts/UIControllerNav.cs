using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIControllerNav : MonoBehaviour
{
	public EventSystem ES;
	private GameObject storeSelected;

	void Start()
	{
		storeSelected = ES.firstSelectedGameObject;
	}

	void Update()
	{
		if (ES.currentSelectedGameObject != storeSelected)
		{
			if (ES.SetSelectedGameObject (storeSelected) == null)
				ES.SetSelectedGameObject (storeSelected);
			else
				storeSelected = ES.currentSelectedGameObject;
		}
	}
}