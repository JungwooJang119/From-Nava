using System.Collections;
using UnityEngine;

public class LoadFinalRoom : LoadNextRoom {

    [SerializeField] private MusicSection targetSection;

    protected override IEnumerator SimulateLoad(GameObject player) {
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.DeactivateMovement();
        AudioControl.Instance.FadeMusic(true);
        var wait = tm.FadeOut();
        yield return new WaitForSeconds(wait);
        player.transform.position = spawn.transform.position;
        controller.ChangeSpawn(spawn);
        foreach (GameObject objList in _objects) {
            objList.GetComponent<ControllerObject>().StartReset();
        }
        yield return new WaitForSeconds(wait);
        AudioControl.Instance.SetMusicSection(targetSection);
        AudioControl.Instance.ResumeMusic();
        AudioControl.Instance.InterpolateMusicTracks(TrackMode.Exploration);
        PlayerController.Instance.ActivateMovement();
        RoomDisablerControl.Instance.ChangeRoomsState(spawn);
        tm.FadeIn();
    }
}