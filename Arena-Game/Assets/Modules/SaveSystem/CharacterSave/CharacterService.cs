using ArenaGame.Managers.SaveManager;
using Cysharp.Threading.Tasks;
using Firebase.Database;
using Newtonsoft.Json;

namespace Authentication
{
    public class CharacterService
    {
        public static DatabaseReference FetchCharacter(string uid, string guid)
        {
            return FirebaseRef.REF_CHARACTERS.Child(uid).Child(guid);
        }
    }
}