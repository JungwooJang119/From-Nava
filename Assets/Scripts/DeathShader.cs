using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**!!!Make sure to assign sprite rendering to DissolveMat!!!**/
public class DealthShader : MonoBehaviour
{
    public float dissolveAmount;
    public float dissolveSpeed;
    public bool isDissolving;
    [ColorUsageAttribute(true, true)]
    public Color outColor;
    [ColorUsageAttribute(true, true)]
    public Color inColor;

    private Material mat;

    void Start()
    {
        /**!!!Make sure to assign sprite rendering to DissolveMat!!!**/
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
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

    }
    /*public void setIsDissolve(bool isDissolving)
    {
        this.isDissolving = isDissolving;
    }*/

    public void DissolveOut(float speed, Color color)
    {
        mat.SetColor("_DissolveColor", color);
        if (dissolveAmount > -0.1)
            dissolveAmount -= Time.deltaTime * speed;
    }

    public void DissolveIn(float speed, Color color)
    {
        mat.SetColor("_DissolveColor", color);
        if (dissolveAmount < 1)
            dissolveAmount += Time.deltaTime * dissolveSpeed;
    }
}
