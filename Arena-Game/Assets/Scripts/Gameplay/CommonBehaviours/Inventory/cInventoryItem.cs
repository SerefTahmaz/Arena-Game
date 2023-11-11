using UnityEngine;

public class cInventoryItem : MonoBehaviour
{
    private int m_TeamId;

    public int TeamId
    {
        get => m_TeamId;
        set => m_TeamId = value;
    }
}