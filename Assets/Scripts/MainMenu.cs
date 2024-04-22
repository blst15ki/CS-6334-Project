using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private string XInput, AInput;
    private int button;
    [SerializeField] GameObject button0, button1;
    [SerializeField] GameObject mainMenuObj;
    [SerializeField] GameObject customizeMenuObj;
    private Image image0, image1;
    public bool main;
    public CustomizeMenu customizeMenuScript;
    private float timer;
    private float waitTime = 0.2f;
    void Start()
    {
        image0 = button0.GetComponent<Image>();
        image1 = button1.GetComponent<Image>();
        XInput = "js2";
        AInput = "js10";
        button = 0;
        // image0.color = Color.yellow;
        main = true;
    }
    void Update()
    {
        if (customizeMenuScript.wait)
        {
            //wait
            timer += Time.deltaTime;
            if (timer > waitTime)
            {
                if (Input.GetButtonDown(XInput))
                {
                    CycleButton();
                }
                else if (Input.GetButtonDown(AInput))
                {
                    SelectButton();
                }
            }
        }
        else
        {
            if (Input.GetButtonDown(XInput))
            {
                CycleButton();
            }
            else if (Input.GetButtonDown(AInput))
            {
                SelectButton();
            }
        }

    }
    public void CycleButton()
    {
        button = (button + 1) % 2;

        if (button == 0 && main)
        { // start
            image0.color = Color.yellow;
            image1.color = Color.white;
        }
        else if (button == 1 && main)
        { // quit
            image0.color = Color.white;
            image1.color = Color.yellow;
        }
    }
    public void SelectButton()
    {
        if (button == 0 && main)
        { // start
            mainMenuObj.SetActive(false);
            customizeMenuObj.SetActive(true);
            main = false;
        }
        // TODO add logic so that going back from customize menu doesnt trigger this
        else if (button == 1 && main)
        { // quit
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
