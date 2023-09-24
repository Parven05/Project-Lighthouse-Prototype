using System;
using UnityEngine;

[CreateAssetMenu()]
public class GatherableSO : ScriptableObject
{
    public GameObject gatherableObjectPrefab;     // To Instantiate .
    public Sprite gatherableImageSprite;          // To Show In Ui Elements.
    public string gatherableObjectName;           // Compare The obj With Name.
    public GatherableObjectType gatherableType;   // Type that you Can Save Some Time.
    public float value;                           // like Health,And Battery Power Only For Usable
    public bool countable;
}
public enum GatherableObjectType  // This Enum Catagarising Objects.
{
    Healable,     // Like InHaller
    Collectable,  // Like Keys 
    Equipable,    // Like Guns Or Simple Weopons like Knife torch
    Usable        // Like Torch battery
}
