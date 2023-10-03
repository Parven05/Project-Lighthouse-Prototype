using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DebugLight : MonoBehaviour
{
    [SerializeField] private GameObject lightRotate1;
    [SerializeField] private GameObject lightRotate2;
    [SerializeField] private GameObject lightOnOff;
    [SerializeField] private int toggleCount = 5;
    [SerializeField] private float toggleInterval = 1f;

    private Animator lightRotateAnim1;
    private Animator lightRotateAnim2;
    private bool isLightOn = false;
    private void Start()
    {
        StartCoroutine(ToggleLight());
        lightRotateAnim1 = lightRotate1.GetComponent<Animator>();
        lightRotateAnim2 = lightRotate2.GetComponent<Animator>();
    }

    private void Update()
    {
        lightRotateAnim1.SetBool("isPlay",true);
        lightRotateAnim2.SetBool("isPlay", true);
    }
    
    private IEnumerator ToggleLight()
    {
        for (int i = 0; i < toggleCount || toggleCount == 0; i++)
        {
            isLightOn = !isLightOn;
            lightOnOff.SetActive(isLightOn); 
            
            yield return new WaitForSeconds(toggleInterval);
        }
    }
}
