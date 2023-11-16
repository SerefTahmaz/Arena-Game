using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cGlobalDamageManager : NetworkBehaviour
{
    private Dictionary<ulong, cCharacter> Characters = new Dictionary<ulong, cCharacter>();
    
    public void OnDeath(cCharacter reciever, cCharacter sender)
    {
        
    }
}
