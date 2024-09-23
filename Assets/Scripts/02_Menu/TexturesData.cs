using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextureData", menuName = "ScriptableObjects/TextureData", order = 3)]
public class TexturesData : ScriptableObject
{
    public List<Body> bodies;
    public List<Wing> wings;
    public List<Tyre> tyres;
}

[System.Serializable]
public class Body
{
    public Texture bodyTextures;
}

[System.Serializable]
public class Wing
{
    public Texture wingTextures;
}

[System.Serializable]
public class Tyre
{
    public Texture tyreTextures;
}