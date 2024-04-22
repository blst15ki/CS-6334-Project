using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CustomizeMenu : MonoBehaviour
{
    private string scene = "Inside";
    private string XInput, AInput;
    private int button;
    [SerializeField] GameObject button0, button1, buttona1, buttona2;
    [SerializeField] GameObject mainMenuObj;
    [SerializeField] GameObject customizeMenuObj;
    private Image image0, image1, imagea1, imagea2;
    public MainMenu mainMenuScript;
    public bool wait;
    void Start()
    {
        image0 = button0.GetComponent<Image>();
        image1 = button1.GetComponent<Image>();
        imagea1 = buttona1.GetComponent<Image>();
        imagea2 = buttona2.GetComponent<Image>();
        XInput = "js2";
        AInput = "js10";
        button = 0;
        // imagea1.color = Color.yellow; // By default selection on avatar 1
        wait = false;
    }
    void Update()
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

    public void CycleButton()
    {
        button = (button + 1) % 4;

        if (button == 0 && !mainMenuScript.main)
        { // enter
            image0.color = Color.yellow;
            image1.color = Color.white;
            imagea1.color = Color.white;
            imagea2.color = Color.white;
        }
        else if (button == 1 && !mainMenuScript.main)
        { // back
            image0.color = Color.white;
            image1.color = Color.yellow;
            imagea1.color = Color.white;
            imagea2.color = Color.white;
        }
        else if (button == 2 && !mainMenuScript.main)
        { // 
            image0.color = Color.white;
            image1.color = Color.white;
            imagea1.color = Color.yellow;
            imagea2.color = Color.white;
        }
        else if (button == 3 && !mainMenuScript.main)
        { // avatar 2
            image0.color = Color.white;
            image1.color = Color.white;
            imagea1.color = Color.white;
            imagea2.color = Color.yellow;
        }
    }
    public void SelectButton()
    {
        if (button == 0 && !mainMenuScript.main)
        { // enter
            customizeMenuObj.SetActive(false);
            SceneManager.LoadScene(scene);
        }
        else if (button == 1 && !mainMenuScript.main)
        { // back
            Debug.Log("Going Back to Main Menu");
            customizeMenuObj.SetActive(false);
            wait = true;
            mainMenuObj.SetActive(true);
            mainMenuScript.main = true;
        }
        else if (button == 2 && !mainMenuScript.main)
        {
            Debug.Log("Selected Avatar 1");
            // TODO: add avatar;           
        }
        else if (button == 3 && !mainMenuScript.main)
        {
            Debug.Log("Selected Avatar 2");
            // TODO: add avatar;           
        }
    }
}
