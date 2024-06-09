using MultiLanguageExamManagementSystem.Data.Repository.IRepository;
using System.Threading.Tasks; // Add this namespace for async Task

namespace MultiLanguageExamManagementSystem.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IApplicationRepository<TEntity> Repository<TEntity>() where TEntity : class;

        Task<bool> CompleteAsync(); // Updated to return a Task<bool> for asynchronous completion
    }
}
