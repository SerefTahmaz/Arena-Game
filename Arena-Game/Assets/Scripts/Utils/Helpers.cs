using ArenaGame.Utils;
using UnityEngine;

namespace STNest.Utils
{
    public static class Helpers
    {
        /// <summary>
        /// Returns -1 or 1
        /// </summary>
        /// <returns></returns>
        public static float RandomSign()
        {
            return Random.value >= .5f ? 1 : -1;
        }

        public static float RandomPentatonicPitch()
        {
            int[] pentatonicSemitones = new[] { 0, 2, 4, 7, 9 };
            var x = pentatonicSemitones.RandomItem();
            float pitch=1;
            for (int i = 0; i < x; i++)
            {
                pitch *= 1.059463f;
            }

            return pitch;
        }
    }
}