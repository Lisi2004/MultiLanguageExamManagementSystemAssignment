using MultiLanguageExamManagementSystem.Data.Repository.IRepository;

namespace MultiLanguageExamManagementSystem.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IApplicationRepository<TEntity> Repository<TEntity>() where TEntity : class;

        bool Complete();
    }
}
