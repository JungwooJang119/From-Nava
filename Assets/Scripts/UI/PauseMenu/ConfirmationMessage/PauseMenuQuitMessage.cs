using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuQuitMessage : MonoBehaviour {

    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private float scaleRate;
    [SerializeField] private Transform content;

    void Awake() => pauseMenu.OnGameResumed += () => gameObject.SetActive(false);

    public void ToggleMessage(bool active) {
        AudioControl.Instance.PlayVoidSFX("PMPageChange" + (active ? 2 : 1), 0.25f);
        gameObject.SetActive(active);
    }

    public async void Quit() {
        pauseMenu.QuitGame();
        Vector3 vZero = new Vector3(0, 0, content.localScale.z);
        while (content != null && !Mathf.Approximately(Vector3.Distance(content.localScale, vZero), 0)) {
            content.localScale = Vector3.MoveTowards(content.localScale, vZero, Time.unscaledDeltaTime * scaleRate);
            await Task.Yield();
        }
    }
}
