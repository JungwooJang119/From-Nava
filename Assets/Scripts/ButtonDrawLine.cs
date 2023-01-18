using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDrawLine : MonoBehaviour
{

    public GameObject nodeStart;

    [SerializeField] private LogicScript logicManager;
    [SerializeField] private PlayerMagicSystem playerMagicCastScript;

    private LineRenderer line;

    public Material material;
    
    // Start is called before the first frame update
    void Start() {
        //line = GetComponent<LineRenderer>();
        
    }
/*
    public void DrawUILine() {
        var currNumLines = logicManager.getCurrLines();
        if (line == null) {
            createLine(currNumLines);
            logicManager.increaseLines(1);
        }
        Vector3 buttonPos = transform.position;
        nodeStart = GameObject.Find(logicManager.getPrevNode());
        Vector3 nodeStartPos = nodeStart.transform.position;
        line.positionCount = 2;
        line.SetPosition(0, nodeStartPos);
        line.SetPosition(1, buttonPos);
        var name = EventSystem.current.currentSelectedGameObject.name;
        logicManager.setString(name, line);
        Debug.Log(logicManager.getCurrLines() + " this is currLines NOw");
        Debug.Log(logicManager.getList().Count + " the length of list");
        if (name == "9") {
            while (logicManager.getCurrLines() > 0) {
                //Destroy(logicManager.getList()[logicManager.getCurrLines() - 1]);
                logicManager.destroyLine(logicManager.getCurrLines() - 1);
                Debug.Log("Destroying line " + logicManager.getCurrLines());
                logicManager.setCurrLines(1);
            }
        }
    }
    */

    void createLine(int currLines) {
        line = new GameObject("Line" + currLines).AddComponent<LineRenderer>();
        line.material = material;
        line.positionCount = 2;
        line.startWidth = 0.15f;
        line.endWidth = 0.15f;
        line.useWorldSpace = true;
        line.numCapVertices = 50;
    }
}
