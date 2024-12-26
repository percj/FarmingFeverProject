using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DebugPanelController : MonoBehaviour
{
    float time;
    [SerializeField] CameraFollower cameraFollower;
    [SerializeField] TextMeshProUGUI TimeText;
    [SerializeField] TextMeshProUGUI OS;
    [SerializeField] TextMeshProUGUI Language;

    [SerializeField] TextMeshProUGUI camXText;
    [SerializeField] TextMeshProUGUI camYText;
    [SerializeField] TextMeshProUGUI camZText;
    [SerializeField] TextMeshProUGUI FOVText;


    [Header("=== Camera Logic  ===")]
    [SerializeField] float MaxOfset;
    [SerializeField] Slider camX;
    [SerializeField] Slider camY;
    [SerializeField] Slider camZ;
    [SerializeField] Slider FOV;


    void Start()
    {
        time = PlayerPrefs.GetFloat("GameTime", time);
        OS.text = SystemInfo.operatingSystemFamily.ToString();
        Language.text = Application.systemLanguage.ToString();
        camX.value = Mathf.InverseLerp(-MaxOfset, MaxOfset, cameraFollower.offset.x);
        camY.value = Mathf.InverseLerp(-MaxOfset, MaxOfset, cameraFollower.offset.y);
        camZ.value = Mathf.InverseLerp(-MaxOfset, MaxOfset, cameraFollower.offset.z);
        FOV.value =  Mathf.InverseLerp(60,120,Camera.main.fieldOfView);


        camXText.text = ((int)(camX.value * (2 * MaxOfset)) - MaxOfset).ToString();
        camYText.text = ((int)(camY.value * (2 * MaxOfset)) - MaxOfset).ToString();
        camZText.text = ((int)(camZ.value * (2 * MaxOfset)) - MaxOfset).ToString();
        Camera.main.fieldOfView = (FOV.value * 60) + 60;
    }


    void Update()
    {
        Timer();
    }

    public void FOVChange()
    {
        Camera.main.fieldOfView = (FOV.value * 60) + 60;
        FOVText.text = ((int)(FOV.value * 60) + 60).ToString();
    }
    public void camXChange()
    {
        cameraFollower.offset.x = (int)((camX.value * (2 * MaxOfset)) - MaxOfset);
        camXText.text = ((int)(camX.value * (2 * MaxOfset)) - MaxOfset).ToString();
    }

    public void camYChange()
    {
        cameraFollower.offset.y = (int)((camY.value * (2 * MaxOfset)) - MaxOfset);
        camYText.text = ((int)(camY.value * (2 * MaxOfset)) - MaxOfset).ToString();
    }

    public void camZChange()
    {
        cameraFollower.offset.z = (int)((camZ.value * (2 * MaxOfset)) - MaxOfset);
        camZText.text = ((int)(camZ.value * (2 * MaxOfset)) - MaxOfset).ToString();
    }


    private void Timer()
    {
        time += Time.deltaTime;
        PlayerPrefs.SetFloat("GameTime", time);
        TimeSpan timerCalculated = TimeSpan.FromSeconds(time);
        TimeText.text = timerCalculated.ToString(@"mm\:ss");
    }

    public void AddMoney()
    {
        GameSingleton.Instance.SetMoney(100000);
    }
    public void ClearData()
    {
        FindObjectOfType<PlayerLogic>().enabled = false;
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
