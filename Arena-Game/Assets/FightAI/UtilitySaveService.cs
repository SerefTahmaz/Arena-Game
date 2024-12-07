using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;

namespace Authentication
{
    public class UtilitySaveService
    {
        public static async UniTask<UtilitySaveData> FetchUtility(string uid)
        {
            var snapshot = await FirebaseRef.REF_UTILITYSAVES.Child(uid).GetValueAsync();
            if (!snapshot.Exists) return null;
            var user =  JsonConvert.DeserializeObject<UtilitySaveData>(snapshot.GetRawJsonValue());
            return user;
        }
        
        public static async UniTask UpdateUtility(string uid, UtilitySaveData utilitySaveData)
        {
            var serializedUtilityData = JsonConvert.SerializeObject(utilitySaveData);
            await FirebaseRef.REF_UTILITYSAVES.Child(uid).SetRawJsonValueAsync(serializedUtilityData).AsUniTask();
        }
    }
}