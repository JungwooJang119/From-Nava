using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auditor : MonoBehaviour
{
    
    //An array for each of the quests as follows:
    //[0] = Leave the room you wake up in
    //[1] = Find all 12 Lab Reports
    //[2] = Complete the S1 Room
    //[3] = Complete the S2 Room
    //[4] = Complete the S3 Room
    //[5] = Complete the S4 Room
    //[6] = Access S5
    //[7] = Complete the game in 9 minutes

    //consider: Set everything on fire (try with adding a separate check in every object? if collider trigger on auditor object (only trees) then do smth?)
    //consider: Complete B3 in 3 pressure plates (int count of pressure plates when pressed, when unpressed -1, once chest is opened check if true)
    //consider: 6 obelisks - (check logic for obelisks; if every laser hit counts, maybe can work out a solution (add to list of previous gameobjects if successful, at the end if < 6 give points))
    //escape the lab = mandatory lmao
    private bool[] quests = new bool[8];
    //Track the total nubmer of lab reports for [1]
    private int labReportCount = 0;
    private float time = 0;
    private bool runTimer = true;

    public void updateSideRoom(string name) {
        int yes = int.Parse(name.Substring(1));
        print(yes);
        quests[yes + 1] = true;
    }
    public void updateLabReport() {
        labReportCount++;
        if (labReportCount == 12) {
            //quests[1] = complete 12 lab reports
            quests[1] = true;
        }
    }

    public void updateEnterRoom(string name) {
        if (string.Compare(name, "B1South") == 0) {
            quests[0] = true;
        } else if (string.Compare(name, "S5South") == 0) {
            quests[6] = true;
        }
    }

    public void updateTimer() {
        runTimer = false;
        if (time <= 540) {
            quests[7] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (runTimer) {
            time += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            string res = "";
            for (int i = 0; i < quests.Length; i++) {
                if (quests[i]) {
                    res += "D ";
                } else {
                    res += "N ";
                }
            }
            res += Mathf.Round(time);
            Debug.Log(res);
        }   
    }
}
