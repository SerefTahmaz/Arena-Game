using System.Collections;
using System.Collections.Generic;
using ArenaGame.Utils;
using DefaultNamespace;
using UnityEngine;

public class FootStepHelper : cSingleton<FootStepHelper>
{
    [SerializeField] private GameSurfaceData m_DefeaultSurface;
    
    private Dictionary<PhysicMaterial, GameSurfaceData> m_CachedSurfaces = new Dictionary<PhysicMaterial, GameSurfaceData>();

    public AudioClip GetClips(Collider surfaceCollider)
    {
        var surfaceMat = surfaceCollider.material;
        if (surfaceMat != null)
        {
            if (m_CachedSurfaces.TryGetValue(surfaceMat, out var surface))
            {
                Debug.Log($"Surface Type {surface.name}");
                return surface.AudioClips.RandomItem();
            }
            else
            {
                if (surfaceCollider.attachedRigidbody &&
                    surfaceCollider.attachedRigidbody.TryGetComponent(out GameSurface gameSurface))
                {
                    m_CachedSurfaces.Add(surfaceMat, gameSurface.GameSurfaceData);
                    Debug.Log($"Surface Type {gameSurface.GameSurfaceData.name}");
                    return gameSurface.GameSurfaceData.AudioClips.RandomItem();
                }
            }
        }

        Debug.Log($"Surface Type {m_DefeaultSurface.name}");
        return m_DefeaultSurface.AudioClips.RandomItem();
    }
}
