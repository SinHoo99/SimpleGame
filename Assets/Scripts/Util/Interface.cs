
public interface IDataManager
{
    void Initializer();
    FriutsData GetFruitData(FriutsID id);
}

public interface ISaveManager
{
    void SaveData<T>(T data);
    bool TryLoadData<T>(out T data);
}
