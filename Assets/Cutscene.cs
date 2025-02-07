using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour, ISavable
{

    [SerializeField] private string saveString;
    private bool activated;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Save() {
        SaveSystem.Current.SetCutscene(saveString, activated);
    }

    public void Load(SaveProfile profile) {
        if (profile.GetCutscene(saveString, false)) {
            gameObject.SetActive(false);
            activated = true;
        }
    }

    public void CutsceneFinished() {
        activated = true;
        Save();
    }


}
