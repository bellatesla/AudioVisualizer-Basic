using UnityEngine;
using System.Collections;

public enum Shapes
{
    Wall=0,
    HalfCircle=1,
    Circle=2
   
}

public enum PillarTypes
{
    Cylinder,
    Box,
}


public enum SampleRates
{
    Hz128 = 128,
    Hz256 = 256,
    Hz512 = 512,
    Hz1024 = 1024,
    Hz2048 = 2048
}


[System.Serializable]
public struct AnalyzerSettings
{   //structs
    public PillarSettings pillar;
    public SpectrumSettings spectrum;
    public PrefabSettings Prefabs;
}



[System.Serializable]
public struct SpectrumSettings
{
    public FFTWindow FffWindowType;
    public SampleRates sampleRate;

    public void Reset()
    {
        FffWindowType = FFTWindow.BlackmanHarris;
        sampleRate = SampleRates.Hz2048;
    }
}



[System.Serializable]
public struct PillarSettings
{
    public PillarTypes type;
    public Shapes shape;
    
    public float radius;
    public float amount;
    public float sensitivity;
    public float speed;
    public void Reset()
    {
        //PillarSettings
        shape = Shapes.Circle;
        type = PillarTypes.Box;

        sensitivity = 40;
        amount = 64;
        speed = 5;
        radius = 20;
    }
}



[System.Serializable]
public struct PrefabSettings

{
    //req prefabs
    public GameObject CylPrefab;
    public GameObject BoxPrefab;

}