using Authentication;

namespace ArenaGame.Managers.SaveManager
{
    public interface ISaveManager
    {
        public User SaveData { get; set; }
        public void Save();
        public void Load();
    }
}