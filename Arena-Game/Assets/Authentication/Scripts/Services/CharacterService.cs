using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;

namespace Authentication
{
    public class CharacterService
    {
        public static async UniTask<CharacterData> FetchCharacters(string uid)
        {
            var snapshot = await FirebaseRef.REF_CHARACTERS.Child(uid).GetValueAsync();
            var json = snapshot.GetRawJsonValue();
            if (json == null) return null;
            var characterData =  JsonConvert.DeserializeObject<CharacterData>(json);
            return characterData;
        }
        
        public static async UniTask UpdateCharacters(string uid, CharacterData characterData)
        {
            var serializedUser = JsonConvert.SerializeObject(characterData);
            await FirebaseRef.REF_CHARACTERS.Child(uid).SetRawJsonValueAsync(serializedUser).AsUniTask();
        }
        
        public static DatabaseReference FetchCharacter(string uid, string guid)
        {
            return FirebaseRef.REF_CHARACTERS.Child(uid).Child(guid);
        }
    }
}