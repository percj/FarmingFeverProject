using I2.Loc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveLoadButton : MonoBehaviour
{
	[SerializeField] List<GameObject> Buttons;
	int selectedButtons;

	private void Awake()
	{
		LoadData();
	}

	public void OnClick()
	{
		SaveDate();
	}

	private void LoadData()
	{
		selectedButtons = PlayerPrefs.GetInt("selectedLanguageButton", selectedButtons);
		foreach (var button in Buttons) button.SetActive(false);
		Buttons[selectedButtons].SetActive(true);
	}

	private void SaveDate()
	{
		var selectedButton = Buttons.Where(x => x.activeInHierarchy).FirstOrDefault();
		if(selectedButton != null) 
		PlayerPrefs.SetInt("selectedLanguageButton", Buttons.IndexOf(selectedButton));
		else
		PlayerPrefs.SetInt("selectedLanguageButton", 0);
	}
}