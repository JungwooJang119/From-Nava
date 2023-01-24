using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestLineRenderer : MonoBehaviour
{
    public UnityEngine.UI.Extensions.UILineRenderer LineRenderer; // Assign Line Renderer in editor
    [SerializeField] private LogicScript logicManager;

    // Use this for initialization
    /*
    @desc When UI button clicked, adds to currently building string holding spell pattern
    @params 
    @return 
    */
    public void AddNewPoint () {
        var name = EventSystem.current.currentSelectedGameObject.name;
        var point = new Vector2();
        if (name == "1") {
            //point = new Vector2() { x = 500, y = -125};
        }
        else if (name == "2") {
            point = new Vector2() { x = 660, y = -125};
            logicManager.setString("2");
        }
        else if (name == "3") {
            point = new Vector2() { x = 840, y = -125};
            logicManager.setString("3");
        }
        else if (name == "4") {
            point = new Vector2() { x = 500, y = -285};
            logicManager.setString("4");
        }
        else if (name == "5") {
            point = new Vector2() { x = 660, y = -285};
            logicManager.setString("5");
        }
        else if (name == "6") {
            point = new Vector2() { x = 840, y = -285};
            logicManager.setString("6");
        }
        else if (name == "7") {
            point = new Vector2() { x = 500, y = -445};
            logicManager.setString("7");
        }
        else if (name == "8") {
            point = new Vector2() { x = 660, y = -445};
            logicManager.setString("8");
        }
        else if (name == "9") {
            point = new Vector2() { x = 840, y = -445};
            logicManager.setString("9");
        }
        var pointlist = new List<Vector2>(LineRenderer.Points);
        pointlist.Add(point);
        LineRenderer.Points = pointlist.ToArray();
        logicManager.setCurrLines(1);
    }

    /*
    @desc When UI button clicked, clears spell pattern string and clears UI of lines
    @params 
    @return 
    */
    public void RemoveNewPoint() {
        //StartCoroutine(ExampleCoroutine());
        var totalLines = logicManager.getCurrLines();
        var pointlist = new List<Vector2>(LineRenderer.Points);
        while (totalLines > 0) {
            pointlist.RemoveAt(totalLines);
            totalLines--;
            logicManager.setCurrLines(-1);
        }
        LineRenderer.Points = pointlist.ToArray();
        //LineRenderer.Points.Remove(0);
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSecondsRealtime(3);
    }
}
