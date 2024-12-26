using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonClick : MonoBehaviour
{
	public Button Button;
	void Start()
	{
		Button btn = Button.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}
	void TaskOnClick()
	{
		GetComponent<AudioSource>().PlayOneShot(GameSingleton.Instance.Sounds.CashCollect);
	}
}
