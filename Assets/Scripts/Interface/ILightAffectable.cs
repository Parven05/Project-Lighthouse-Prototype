using UnityEngine;

public interface ILightAffectable 
{
    bool IsAffectedByLight { get; set; }

    bool IsOnPlayerFov { get; set; }

    bool IsOnPlayerRadius { get; set; }

    void SetLightAffected(bool isAffectedByLight);

    void SetOnPlayerFOV(bool isOnPlayerFOV);

    void SetOnPlayerRadious(bool isOnPlayerRadious);


}
