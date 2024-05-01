namespace WebDating.Interfaces
{
    public interface IBaseInsertRepository<T> where T : class
    {
        Task<T> Insert(T entity);
    }
    public interface IBaseUpdateRepository<T> where T : class
    {
        void Update(T entity);
    }
    public interface IBaseDeleteRepository<T> where T : class
    {
        void Delete(T entity);
    }
    public interface IBaseGetAllRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
    }
    public interface IBaseGetByIdRepository<T> where T : class
    {
        Task<T> GetById(int id);
    }
}
