using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    //[7] = Light up all objects in room
    //[8] = Pressure Plates
    //[9] = 6 Obelisks
    //[10] = Complete the game in 9 minutes

    //consider: Set everything on fire (try with adding a separate check in every object? if collider trigger on auditor object (only trees) then do smth?)
    //consider: Complete B3 in 3 pressure plates (int count of pressure plates when pressed, when unpressed -1, once chest is opened check if true)
    //consider: 6 obelisks - (check logic for obelisks; if every laser hit counts, maybe can work out a solution (add to list of previous gameobjects if successful, at the end if < 6 give points))
    //escape the lab = mandatory lmao
    private bool[] quests = new bool[11];
    //Track the total nubmer of lab reports for [1]
    private int labReportCount = 0;
    private float time = 0;
    private bool runTimer = true;
    private int litObjectCount = 0;
    private int plateCount = 1;
    [SerializeField] private GameObject bigGuy;

    private bool isOpen;
    public GameObject AuditPage;
    public Text timerText;
    public GameObject labReportCounter;
    public Text labReportCounterText;

    public GameObject quest1Complete;
    private bool isQuest1Complete;

    public GameObject quest2Complete;
    private bool isQuest2Complete;

    public GameObject quest3Complete;
    private bool isQuest3Complete;

    public GameObject quest4Complete;
    private bool isQuest4Complete;

    public GameObject quest5Complete;
    private bool isQuest5Complete;

    public GameObject quest6Complete;
    private bool isQuest6Complete;

    public GameObject quest7Complete;
    private bool isQuest7Complete;

    public GameObject quest8Complete;
    private bool isQuest8Complete;

    public GameObject quest9Complete;
    private bool isQuest9Complete;

    public GameObject quest10Complete;
    private bool isQuest10Complete;
    public GameObject quest12Complete;
    private bool isQuest12Complete;

    public void updateSideRoom(string name) {
        int yes = int.Parse(name.Substring(1));
        print(yes);
        quests[yes + 1] = true;
        switch (yes) {
            case 1:
                isQuest3Complete = true;
                break;
            case 2:
                isQuest4Complete = true;
                break;
            case 3:
                isQuest5Complete = true;
                break;
            case 4:
                isQuest6Complete = true;
                break;
        }
    }
    public void updateLabReport() {
        labReportCount++;
        if (labReportCount == 12) {
            //quests[1] = complete 12 lab reports
            quests[1] = true;
            isQuest2Complete = true;
        }
    }

    public void updateEnterRoom(string name) {
        if (string.Compare(name, "B1South") == 0) {
            quests[0] = true;
            isQuest1Complete = true;

        } else if (string.Compare(name, "S5South") == 0) {
            quests[6] = true;
            isQuest7Complete = true;
        }
    }

    public void updateTimer() {
        runTimer = false;
        if (time <= 540) {
            quests[10] = true;
            isQuest12Complete = true;
        }
    }

    public void updateLightUp(int i) {
        litObjectCount += i;
        print(litObjectCount);
        if (litObjectCount == 14) {
            quests[7] = true;
            isQuest8Complete = true;
        }
    } 

    public void updatePressurePlate(string name) {
        if (string.Compare(name, "Pressure Plate (First)") == 0) {
            plateCount = 2;
            return;
        }
        if (string.Compare(name, "B3") == 0) {
            if (plateCount <= 6) {
                quests[8] = true;
                isQuest9Complete = true;
            }
        } else {
            plateCount++;
            print(plateCount);
        }
    }

    int currMirrCount = 0;
    int finalMirrCount = 0;
    public void updateMirror(string name) {
        if (name == "Mirror") {
            currMirrCount++;
        }
        if (name == "Receiver" || name == "ReceiverFinal") {
            finalMirrCount += currMirrCount;
            if (name == "ReceiverFinal" && finalMirrCount <= 6) {
                quests[9] = true;
                isQuest10Complete = true;
            }
        }
        if (name == "Reset") {
            currMirrCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bigGuy == null) {
            updateTimer();
        }
        if (runTimer) {
            time += Time.deltaTime;
            int minutes = (int)time / 60;
            int seconds = (int)time % 60;
            timerText.text = "Time: " + minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString();
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
            res += finalMirrCount;
            //res += Mathf.Round(time);
            res += labReportCount;
            Debug.Log(res);
        }   
    }

    public void ToggleOpen() {
        if (isOpen) {
            AuditPage.SetActive(false);
            labReportCounterText.text = "Reports Read: " + labReportCount;
            labReportCounter.SetActive(false);
            isOpen = false;
            quest1Complete.SetActive(false);
            quest2Complete.SetActive(false);
            quest3Complete.SetActive(false);
            quest4Complete.SetActive(false);
            quest5Complete.SetActive(false);
            quest6Complete.SetActive(false);
            quest7Complete.SetActive(false);
            quest8Complete.SetActive(false);
            quest9Complete.SetActive(false);
            quest10Complete.SetActive(false);
            quest12Complete.SetActive(false);
        } else {
            AuditPage.SetActive(true);
            labReportCounterText.text = "Reports Read: " + labReportCount;
            labReportCounter.SetActive(true);
            isOpen = true;
            if (isQuest1Complete) {
                quest1Complete.SetActive(true);
                
            }
            if (isQuest2Complete) {
                quest2Complete.SetActive(true);
                
            }
            if (isQuest3Complete) {
                quest3Complete.SetActive(true);
                
            }
            if (isQuest4Complete) {
                quest4Complete.SetActive(true);
                
            }
            if (isQuest5Complete) {
                quest5Complete.SetActive(true);
                
            }
            if (isQuest6Complete) {
                quest6Complete.SetActive(true);
                
            }
            if (isQuest7Complete) {
                quest7Complete.SetActive(true);
                
            }

            if (isQuest8Complete) {
                quest8Complete.SetActive(true);
                
            }
            if (isQuest9Complete) {
                quest9Complete.SetActive(true);
                
            }
            if (isQuest10Complete) {
                quest10Complete.SetActive(true);
                
            }
            if (isQuest12Complete) {
                quest12Complete.SetActive(true);
                
            }
        }
    }
}
