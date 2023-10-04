using System;
using UnityEngine;

[CreateAssetMenu()]
public class GatherableSO : ScriptableObject
{
    public GameObject gatherableObjectPrefab;     // To Instantiate .
    public Sprite gatherableImageSprite;          // To Show In Ui Elements.
    public string gatherableObjectName;           // Compare The obj With Name.
    public GatherableObjectType gatherableType;   // Type that you Can Save Some Time.
    public StoringType storingType;               // Store type That Detemines whether it Removable Or Not
    public float value;                           // like Health,And Battery Power Only For Usable
    [HideInInspector] public float quantity = 0;  // variable only Applicable For Healable And Usable
}
public enum GatherableObjectType  // This Enum Catagarising Objects.
{
    Healable,     // Like InHaller
    Collectable,  // Like Keys 
    Equipable,    // Like Guns Or Simple Weopons like Knife torch
    Usable        // Like Torch battery
}

public enum StoringType // this enum Determines is Storable Or Not
{
    Removable,
    NonRemovable
}
