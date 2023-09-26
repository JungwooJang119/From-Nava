using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPushable
{
    void Push(Vector2 dir, float dist, float spd);

    void PushTranslate();

    bool GetPushed();
}
