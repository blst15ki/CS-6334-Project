using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class EnableDisable : MonoBehaviour
{
    
    private GameObject collided;

    Ray ray;
    RaycastHit hit;
    
    private bool light;
    private bool water;

    private bool isCut;

    private GameObject cutObject;

    void Start(){
        collided = null;
        var c = GameObject.Find("Water").transform.GetChild(0);
        c.gameObject.SetActive(false);
        light = false;
        water = false;
        isCut = true;
    }
private GameObject parent;
private Vector3 childPos;
    void Update(){

        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if(Physics.Raycast(ray, out hit, 100)){
            print(hit.transform.name);
            string target = hit.transform.name;
            if(target == "Pot 1" || target == "Pot 2"){
                if(Input.GetButtonDown("js10")){
                GameObject waterCan = GameObject.Find("Water").transform.GetChild(0).gameObject;
                waterCan.GetComponent<Outline>().enabled = true;

                var canOutline = waterCan.GetComponent<Outline>();
                canOutline.OutlineColor = Color.yellow;
                canOutline.OutlineWidth = 5f;

                GameObject flowerPot = GameObject.Find(hit.transform.name);
                flowerPot.GetComponent<Outline>().enabled = true;

                var potOutline = flowerPot.GetComponent<Outline>();
                potOutline.OutlineColor = Color.blue;
                potOutline.OutlineWidth = 5f;
                }

                if(Input.GetButtonUp("js10")){
                    GameObject waterCan = GameObject.Find("Water").transform.GetChild(0).gameObject;
                    waterCan.GetComponent<Outline>().OutlineColor = Color.white;
                    waterCan.GetComponent<Outline>().enabled = false;

                    GameObject flowerPot = GameObject.Find(target);
                    flowerPot.GetComponent<Outline>().OutlineColor = Color.white;
                    flowerPot.GetComponent<Outline>().enabled = false;
                }

                if(Input.GetButton("js11")){
                if(isCut == true){
                parent = GameObject.Find(target).transform.parent.gameObject;
                childPos = GameObject.Find(target).transform.position;
                Debug.Log(parent.name);
                GameObject.Find(target).SetActive(false);
                isCut = false;
                }
                }
                
                
            

            
            }

            if(Input.GetButton("js9")){
                    Debug.Log("hhhhh");
                    var newPosition = hit.point;
                    newPosition.y = childPos.y;
                    parent.transform.GetChild(0).gameObject.transform.position = newPosition;
                    parent.transform.GetChild(0).gameObject.SetActive(true);
                    isCut = true;
                }

            if(target == "Light Controller" && Input.GetButton("js10")){
                if(light == true){
                   GameObject.Find("Directional Light").GetComponent<Light>().enabled = true;
                    GameObject.Find("Light Controller").GetComponent<Renderer>().material.color = Color.yellow;
                    light = false;
                }
                else{
                    GameObject.Find("Directional Light").GetComponent<Light>().enabled = false;
                    GameObject.Find("Light Controller").GetComponent<Renderer>().material.color = Color.white;
                    light = true;
                }
            }

            if(Input.GetButton("js1") && water  == false){
                GameObject.Find("Water").transform.GetChild(0).gameObject.SetActive(true);
                water = true;
            }
            else if(hit.transform.name == "Watering Can"){
                if(water == true && Input.GetButton("js1")){
                    GameObject.Find("Water").transform.GetChild(0).gameObject.SetActive(false);
                    water = false;
                }
            }
        }
    }


    public void OnPointerEnter(GameObject obj){
        obj.GetComponent<Outline>().enabled = true;
    }

    public void OnPointerExit(GameObject obj){
        obj.GetComponent<Outline>().enabled = false;
    }
}
