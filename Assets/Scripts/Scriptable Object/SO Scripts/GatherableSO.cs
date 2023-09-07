using UnityEngine;

[CreateAssetMenu()]
public class GatherableSO : ScriptableObject
{
    public GameObject gatherableObjectPrefab;     // To Instantiate .
    public Sprite gatherableImageSprite;                 // To Show In Ui Elements.
    public string gatherableObjectName;           // Compare The obj With Name.
    public GatherableObjectType gatherableType;   // Type that you Can Save Some Time.
}
public enum GatherableObjectType  // This Enum Catagarising Objects.
{
    Healable,     // if Picked selected Object is Healable like Inhaler
    Collectable,  // Like Keys 
    Attackable,   // Like Guns Or Simple Weopons like Knife
    Usable        // Like Torch 
}
