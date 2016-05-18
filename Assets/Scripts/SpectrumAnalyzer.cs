using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Attach this script below a GameObject with an AudioSource and manually assign a clip and enable Play on Awake.
/// Since this script does not care what song is playing you can implement an Audio manager to change songs as you wish.
/// Make sure to manually assign two prefabs of your choice.
/// </summary>
public class SpectrumAnalyzer : MonoBehaviour
{
    public AnalyzerSettings settings; //All of our settings

    //private
    private float[] spectrum; //Audio Source data
    private List<GameObject> pillars; //ref pillars to scale/move with music
    private GameObject folder;
    private bool isBuilding; //Prevents multi-calls and update while building.


    void Start()
    {
        isBuilding = true;
        CreatePillarsByShapes();
    }

    private void CreatePillarsByShapes()
    {
        //get current pillar types
        GameObject currentPrefabType = settings.pillar.type == PillarTypes.Cylinder ? settings.Prefabs.CylPrefab : settings.Prefabs.BoxPrefab;
       
        
        pillars = MathB.ShapesOfGameObjects(currentPrefabType, settings.pillar.radius, (int) settings.pillar.amount,settings.pillar.shape);

        //Organize pillars nicely in this folder under this transform
        folder = new GameObject("Pillars-" + pillars.Count);
        folder.transform.SetParent(transform);

        foreach (var piller in pillars)
        {
            piller.transform.SetParent(folder.transform);
        }

        isBuilding = false;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.R)) Rebuild();
        if (isBuilding) return;

        spectrum = AudioListener.GetSpectrumData((int) settings.spectrum.sampleRate, 0, settings.spectrum.FffWindowType);


        for (int i = 0; i < pillars.Count; i++) //needs to be <= sample rate or error
        {
            float level = spectrum[i]*settings.pillar.sensitivity*Time.deltaTime*1000; //0,1 = l,r for two channels

            //Scale pillars 
            Vector3 previousScale = pillars[i].transform.localScale;
            previousScale.y = Mathf.Lerp(previousScale.y, level, settings.pillar.speed*Time.deltaTime);
                //Add delta time please
            pillars[i].transform.localScale = previousScale;

            //Move pillars up by scale/2
            Vector3 pos = pillars[i].transform.position;
            pos.y = previousScale.y*.5f;
            pillars[i].transform.position = pos;
        }
    }

    /// <summary>
    /// Called by UI slider onValue changed
    /// </summary>
    public void Rebuild()
    {
        if (isBuilding) return;

        //Destroy the pillars we had, clear the pillar list and start over
        isBuilding = true;
        pillars.Clear();
        DestroyImmediate(folder);
        CreatePillarsByShapes();
    }

    /// <summary>
    /// Resets to all settings to default in inspector drop down.
    /// </summary>
    private void Reset()
    {
        settings.pillar.Reset();
        settings.spectrum.Reset();
    }

    #region Dynamic floats and for UI sliders

    /// <summary>
    /// Convert Shapes enum to an int from a float so we can control by UI Slider
    /// </summary>
    public float PillarShape
    {
        get { return (int) settings.pillar.shape; }
        set
        {
            //set with UI Slider
            int num = (int) Mathf.Clamp(value, 0, 3);
            settings.pillar.shape = (Shapes) num;
        }
    }

    public float PillarType
    {
        get { return (int) settings.pillar.type; }
        set
        {
            //set with UI Slider
            int num = (int)Mathf.Clamp(value, 0, 2); 
            settings.pillar.type = (PillarTypes) num;
        }
    }

    public float Amount
    {
        get { return settings.pillar.amount; }
        set
        {
            settings.pillar.amount = Mathf.Clamp(value, 4, 128);
            
        }
    }

    public float Radius
    {
        get { return settings.pillar.radius; }
        set { settings.pillar.radius = Mathf.Clamp(value, 2, 256); }
    }


    public float Sensitivity
    {
        get { return settings.pillar.sensitivity; }
        set { settings.pillar.sensitivity = Mathf.Clamp(value, 1, 50); }
    }

    public float PillarSpeed
    {
        get { return settings.pillar.speed; }
        set { settings.pillar.speed = Mathf.Clamp(value, 1, 30); }
    }


    public float SampleMethod
    {
        get { return (int) settings.spectrum.FffWindowType; }
        set
        {
            //set with UI Slider
            int num = (int)Mathf.Clamp(value, 0, 6); 
            settings.spectrum.FffWindowType = (FFTWindow) num;
        }
    }

    public float SampleRate
    {
        get { return (int) settings.spectrum.sampleRate; }
        set
        {
            //set with UI Slider
            int num = (int) Mathf.Pow(2, 7 + value);//128,256,512,1024,2056
            settings.spectrum.sampleRate = (SampleRates) num;
        }
    }

    #endregion
}