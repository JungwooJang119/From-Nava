using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FakeChestScript : IInteractable
{
    public string virtualCameraName = "CM vcam1";	// For security reasons, the name of the virtual camera can be modified here if changed in the scene.
    private CinemachineVirtualCamera _virtualCamera;
    public GameObject _cameraTarget;				// The target the camera will move towards.
    public Transform _returnToPlayer;
    public Animator animator;

    //for fake Chest
    public Transform enemySpawnPoint;
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        //_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _virtualCamera = GameObject.Find("Main Camera").transform.Find(virtualCameraName).GetComponent<CinemachineVirtualCamera>();
        _returnToPlayer = _virtualCamera.Follow;
        if (_cameraTarget != null) StartCoroutine(CameraTransitionIn());
        animator = GetComponent<Animator>();
    }

    protected override void InteractBehavior() {
        StartCoroutine(EnemySpawn());
        canTrigger = false;
    }

    IEnumerator CameraTransitionIn() {
      yield return new WaitForSeconds(0.0f);
      _virtualCamera.Follow = _cameraTarget.transform;
      yield return new WaitForSeconds(2f);
      _virtualCamera.Follow = _returnToPlayer;
    }

    IEnumerator EnemySpawn() {
        animator.SetBool("OpeningChest", true);
        AudioControl.Instance.PlaySFX("Chest Open", gameObject);
        yield return new WaitForSeconds(1.0f);
        //spawn enemy
        var newEnemy = Instantiate(enemy, enemySpawnPoint.position, enemySpawnPoint.rotation);
        newEnemy.name = newEnemy.name.Replace("(Clone)","").Trim();
        FadeButton();
        Destroy(this);
	}
}
