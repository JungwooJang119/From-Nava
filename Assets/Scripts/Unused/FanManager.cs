using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanManager : MonoBehaviour
{
    private List<Fan> fans = new List<Fan>();
    private bool allBlown;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
            fans.Add(child.gameObject.GetComponent<Fan>());
        allBlown = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool check = true;
        foreach(Fan f in fans){
            if (!f.IsBlowing()) {
                check = false;
            }
        }
        if (check && !allBlown) {
            allBlown = true;
            // FansActivate();
        } else if (!check && allBlown) {
            allBlown = false;
            // FansDeactivate();
        }
    }

    // void FansActivate(){
    //     //fill in fan activated behavior here
    //     Debug.Log("all fans blowing");
    // }

    // void FansDeactivate() {
    //     //fill in fan deactivated behavior here
    //     Debug.Log("not all fans blowing");
    // }
}
