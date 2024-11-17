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
    }
}