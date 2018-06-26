public interface IAmPersisted
{
    void Save(SaveGame save);

    void Load(SaveGame save);
}