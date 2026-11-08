public class GenericRepository<T> : IRepository<T> where T : Model {
    private static readonly List<T> items = [];

    public void Add(T entity) {
        ArgumentNullException.ThrowIfNull(entity);
        if (GetById(entity.Id) is not null) throw new ArgumentException("Ідентифікатор уже існує.");
        items.Add(entity);
    }

    public T? GetById(int id) { return items.Find(item => item.Id == id); }
    public IReadOnlyList<T> GetAll() { return items.ToArray(); }

    public bool Update(T entity) {
        ArgumentNullException.ThrowIfNull(entity);
        int index = items.FindIndex(item => item.Id == entity.Id);
        if (index < 0) return false;

        items[index] = entity;
        return true;
    }

    public bool Delete(int id) { return items.RemoveAll(item => item.Id == id) > 0; }
}
