using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class RoomControlA2 : MonoBehaviour
{
    [SerializeField] GameObject firewoodController;
    private Firewood_Script[] _firewoods;
    private bool _canClear = true;

    [SerializeField] private bool isClear = false;
    //[SerializeField] private GameObject spellNotif;
    public GameObject A2Chest = null;
    public bool cheat = false;
    public bool _earlyRoom = false;

    public string virtualCameraName = "CM vcam1";	// For security reasons, the name of the virtual camera can be modified here if changed in the scene.
	private CinemachineVirtualCamera _virtualCamera;
	public GameObject _cameraTarget;				// The target the camera will move towards.
	public Transform _returnToPlayer;
    public GameObject door;
    
    // Start is called before the first frame update
    void Start() {
		_firewoods = firewoodController.GetComponentsInChildren<Firewood_Script>();
        _virtualCamera = GameObject.Find("Main Camera").transform.Find(virtualCameraName).GetComponent<CinemachineVirtualCamera>();
		_returnToPlayer = _virtualCamera.Follow;
	}

    // Update is called once per frame
    void Update() {
        if (!isClear) {
            _canClear = true;
            foreach (Firewood_Script _firewood in _firewoods) {
                if (!_firewood.isLit) { _canClear = false; break; }
            }
            if (_canClear) CompleteRoom();
        }
    }

    IEnumerator CameraTransitionIn() {
        yield return new WaitForSeconds(0.0f);
		_virtualCamera.Follow = _cameraTarget.transform;
		yield return new WaitForSeconds(2f);
        _virtualCamera.Follow = _returnToPlayer;
		Destroy(this.gameObject);
		//spellNotif.SetActive(false);
	}

	private void CompleteRoom() {
        if (!cheat) {
            StartCoroutine(CameraTransitionIn());
			door.GetComponent<Door>().OpenDoor();
            if (!_earlyRoom) A2Chest.SetActive(true);
        } else {
			if (!_earlyRoom) A2Chest.SetActive(true);
			Destroy(this.gameObject);
		}
		isClear = true;
	}
}
