using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] private ItemData[] wantedData;

    private void Start() {
        SaveSystem.Current.Load();
    }

    public void SaveCurrentGame() {
        CollectibleController controller = ReferenceSingleton.Instance.collectibleController;
        HashSet<ItemData> polaroidInventory = controller.GetItems<PolaroidData>();
        Debug.Log(polaroidInventory.Count);
        SaveSystem.Current.SetNumberOfPolaroids(polaroidInventory.Count);

        SaveSystem.SaveGame();
    
    }

    void OnApplicationQuit() {
        SaveCurrentGame();
    }
}