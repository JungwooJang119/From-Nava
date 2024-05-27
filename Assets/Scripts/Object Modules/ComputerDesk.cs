using UnityEngine;

public class ComputerDesk : SpriteSwapper {

    [SerializeField] private Sprite defaultUnlit, defaultLit,
                                    frozenUnlit, frozenLit;
    [SerializeField] private LabReport report;

    void Start() {
        if (report == null) {
            Default = defaultUnlit;
            Frozen = frozenUnlit;
        } else {
            Default = defaultLit;
            Frozen = frozenLit;
            report.OnReportRead += ComputerDesk_OnReportRead;
        }
    }

    private void ComputerDesk_OnReportRead() {
        Default = defaultUnlit;
        Frozen = frozenUnlit;
        spriteRenderer.sprite = defaultUnlit;
    }
}