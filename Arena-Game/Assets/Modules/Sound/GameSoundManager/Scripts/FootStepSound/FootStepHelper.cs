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
                return surface.AudioClips.RandomItem();
            }
            else
            {
                if (surfaceCollider.attachedRigidbody &&
                    surfaceCollider.attachedRigidbody.TryGetComponent(out GameSurface gameSurface))
                {
                    m_CachedSurfaces.Add(surfaceMat, gameSurface.GameSurfaceData);
                    return gameSurface.GameSurfaceData.AudioClips.RandomItem();
                }
            }
        }
        return m_DefeaultSurface.AudioClips.RandomItem();
    }
}
