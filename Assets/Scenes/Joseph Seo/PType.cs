using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PType : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public string type;

    public string getType() {
        return type;
    }



}
