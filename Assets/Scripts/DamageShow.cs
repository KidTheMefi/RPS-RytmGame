using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageShow : MonoBehaviour
{
    private Image damageImage;

    [SerializeField] private float timeToOff;
    // Start is called before the first frame update
    void Start()
    {
        damageImage = GetComponent<Image>();
        damageImage.color = new Color32(255, 255, 255, 0);
    }

    public void DamageDeal()
    {
        StopCoroutine(ShowAndHide());
        StartCoroutine(ShowAndHide());
    }

    private IEnumerator ShowAndHide()
    {

        for (int i = 250; i >= 0; i -= 10)
        {
            damageImage.color = new Color32(255, 255, 255, (byte)i);
            yield return new WaitForSeconds(timeToOff / 25);
        }     
    }
  
}
