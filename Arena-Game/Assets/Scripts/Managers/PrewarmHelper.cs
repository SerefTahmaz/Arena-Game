using System.Linq;
using ArenaGame.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Managers
{
    public class PrewarmHelper: MonoBehaviour
    { 
        public async UniTask PrewarmShaders(int i)
        {
            
            await UniTask.DelayFrame(5);
            if (PlayerPrefs.GetInt($"Prewarmed_{i}", 0) == 1)
            {
                Destroy(gameObject);
                return; 
            }

            var transforms = FindObjectsOfType<Transform>().ToList();
            transforms.Shuffle();
            transforms = transforms.Take(20).ToList();
            foreach (var VARIABLE in transforms)
            {
                if(VARIABLE == null) continue;
                var dir = VARIABLE.position - transform.position;
                transform.forward = dir.normalized;
                transform.position = VARIABLE.position + VARIABLE.forward * 0.01f;
                await UniTask.DelayFrame(1);
            }
            
            PlayerPrefs.SetInt($"Prewarmed_{i}",1);
            Destroy(gameObject);
        }
    }
}