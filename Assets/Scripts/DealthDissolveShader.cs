using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**!!!Make sure to assign sprite rendering to DissolveMat!!!**/
public class DealthDissolveShader : MonoBehaviour
{
    public float dissolveAmount = 1;
    public float dissolveSpeed = 1;
    public bool isDissolving;
    /*[ColorUsageAttribute(true, true)]
    public Color outColor;
    [ColorUsageAttribute(true, true)]
    public Color inColor;*/

    private Material mat;
    private Coroutine dissolveRoutine;

    void Start()
    {
        /**!!!Make sure to assign sprite rendering to DissolveMat!!!**/
        mat = GetComponent<SpriteRenderer>().material;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            DissolveOut();
        }
    }

    /*void Update()
    {
        if (isDissolving)
        {
            DissolveOut(dissolveSpeed, outColor);
        }

        if (!isDissolving)
        {
            DissolveIn(dissolveSpeed, inColor);
        }

        mat.SetFloat("_DissolveAmount", dissolveAmount);

    }*/

    
    public void DissolveOut()
    {
        // prevents multiple dissolveRoutines from running
        if (dissolveRoutine != null)
        {
            StopCoroutine(dissolveRoutine);
        }
        dissolveRoutine = StartCoroutine(DissolveOutCoroutine());
    }

    public void DissolveIn()
    {
        // prevents multiple dissolveRoutines from running
        if (dissolveRoutine != null)
        {
            StopCoroutine(dissolveRoutine);
        }
        dissolveRoutine = StartCoroutine(DissolveInCoroutine());
    }

    public IEnumerator DissolveOutCoroutine()
    {
        Debug.Log("dissolve routine start");
        while (dissolveAmount > -0.1)
        {
            dissolveAmount -= Time.deltaTime * dissolveSpeed;
            mat.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
            dissolveRoutine = null;
        }
    }
    public IEnumerator DissolveInCoroutine()
    {
        Debug.Log("dissolve routine start");
        while (dissolveAmount <= 1)
        {
            dissolveAmount += Time.deltaTime * dissolveSpeed;
            mat.SetFloat("_DissolveAmount", dissolveAmount);
            yield return null;
            dissolveRoutine = null;
        }
    }
}
