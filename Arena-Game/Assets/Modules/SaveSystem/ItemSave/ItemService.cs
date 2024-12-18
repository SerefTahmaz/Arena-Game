using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;

namespace Authentication
{
    public class ItemService
    {
        public static async UniTask<ItemData> FetchItems(string uid)
        {
            var snapshot = await FirebaseRef.REF_ITEMS.Child(uid).GetValueAsync();
            var json = snapshot.GetRawJsonValue();
            if (json == null) return null;
            var characterData =  JsonConvert.DeserializeObject<ItemData>(json);
            return characterData;
        }
        
        public static async UniTask UpdateItems(string uid, ItemData itemData)
        {
            var serializedUser = JsonConvert.SerializeObject(itemData);
            await FirebaseRef.REF_ITEMS.Child(uid).SetRawJsonValueAsync(serializedUser).AsUniTask();
        }
        
        public static DatabaseReference FetchItem(string uid, string guid)
        {
            return FirebaseRef.REF_ITEMS.Child(uid).Child(guid);
        }
    }
}