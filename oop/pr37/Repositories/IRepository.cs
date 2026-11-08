public interface IRepository<T> where T : Model {
    void Add(T entity);
    T? GetById(int id);
    IReadOnlyList<T> GetAll();
    bool Update(T entity);
    bool Delete(int id);
}
