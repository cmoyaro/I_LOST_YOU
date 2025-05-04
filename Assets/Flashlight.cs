using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public GameObject ON;
    public GameObject OFF;
    private bool isOn;


    // Start is called before the first frame update
    void Start()
    {

        ON.SetActive(false);
        OFF.SetActive(true);
        isOn=false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            
            if(isOn){
                ON.SetActive(false);
                OFF.SetActive(true);
            }

              if(!isOn){
                ON.SetActive(true);
                OFF.SetActive(false);
            }
            
            isOn = !isOn;
        }
        
    }
}
