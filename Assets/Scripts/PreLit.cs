using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreLit : MonoBehaviour
{
    public Firewood_Script f;
    // Start is called before the first frame update
    void Start()
    {
        f.isLit = true;
    }
}
