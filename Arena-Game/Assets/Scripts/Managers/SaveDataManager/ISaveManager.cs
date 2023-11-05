namespace DemoBlast.Managers.SaveManager
{
    public interface ISaveManager
    {
        public cSaveData SaveData { get; set; }
        public void Save();
        public void Load();
    }
}