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
    [SerializeField] GameObject button1, buttona1, buttona2;
    [SerializeField] GameObject mainMenuObj;
    [SerializeField] GameObject customizeMenuObj;
    private Image image1, imagea1, imagea2;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject customizeMenu;
    [SerializeField] GameObject tutorialScreen;
    void Start()
    {
        image1 = button1.GetComponent<Image>();
        imagea1 = buttona1.GetComponent<Image>();
        imagea2 = buttona2.GetComponent<Image>();
        XInput = "js2";
        AInput = "js10";
        button = 1;
        imagea1.color = Color.yellow; // By default selection on avatar 1
    }
    void Update()
    {
        if (Input.GetButtonDown(XInput))
        {
            CycleButton();
        }
        else if (Input.GetButtonDown(AInput))
        {
            if(tutorialScreen.activeSelf) {
                SceneManager.LoadScene(scene);
            } else {
                SelectButton();
            }
        }
    }

    public void CycleButton()
    {
        button = (button + 1) % 3;
        if (button == 0)
        { // back
            image1.color = Color.yellow;
            imagea1.color = Color.white;
            imagea2.color = Color.white;
        }
        else if (button == 1)
        { // avatar 1
            image1.color = Color.white;
            imagea1.color = Color.yellow;
            imagea2.color = Color.white;
        }
        else if (button == 2)
        { // avatar 2
            image1.color = Color.white;
            imagea1.color = Color.white;
            imagea2.color = Color.yellow;
        }
    }
    public void SelectButton()
    {
        if (button == 0)
        { // back
            Debug.Log("Going Back to Main Menu");
            customizeMenuObj.SetActive(false);
            customizeMenu.GetComponent<CustomizeMenu>().enabled = false;
            mainMenuObj.SetActive(true);
            mainMenu.GetComponent<MainMenu>().enabled = true;
        }
        else if (button == 1)
        {
            customizeMenuObj.SetActive(false);
            tutorialScreen.SetActive(true);
            GameManager.Instance.avatarValue = 0;          
        }
        else if (button == 2)
        {
            customizeMenuObj.SetActive(false);
            tutorialScreen.SetActive(true);
            GameManager.Instance.avatarValue = 1;
        }
    }
}
