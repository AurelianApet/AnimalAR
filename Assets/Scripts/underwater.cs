using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class underwater : MonoBehaviour
{
    private bool defaultFog;
    private Color defaultFogColor;
    private float defaultFogDensity;
    private Material defaultSkybox;
    private Material noSkybox;

    private bool flag = false;
    private bool isChanged = false;
    void Start()
    {
        defaultFog = RenderSettings.fog;
        defaultFogColor = RenderSettings.fogColor;
        defaultFogDensity = RenderSettings.fogDensity;
        defaultSkybox = RenderSettings.skybox;
        //Set the background color
        StartCoroutine("changeBack");
    }

    void Update()
    {
        if (Global.curMarkerId == 11 || Global.curMarkerId == 9 || Global.curMarkerId == 16 || Global.curMarkerId == 3)
        {
            if (!flag)
                isChanged = true;
            flag = true;
        }
        else
        {
            if (flag)
                isChanged = true;
            flag = false;
        }
    }

    IEnumerator changeBack()
    {
        while (true)
        {
            if (!isChanged)
                yield return new WaitForSeconds(0.1f);
            if (flag)
            {
                GetComponent<Camera>().backgroundColor = new Color(0, 0.4f, 0.7f, 1);
                RenderSettings.fog = true;
                RenderSettings.fogColor = new Color(0, 0.4f, 0.7f, 0.6f);
                RenderSettings.fogDensity = 0.04f;
                RenderSettings.skybox = noSkybox;
            }
            else
            {
                GetComponent<Camera>().backgroundColor = new Color(0, 0, 0, 1);
                RenderSettings.fog = defaultFog;
                RenderSettings.fogColor = defaultFogColor;
                RenderSettings.fogDensity = defaultFogDensity;
                RenderSettings.skybox = defaultSkybox;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
