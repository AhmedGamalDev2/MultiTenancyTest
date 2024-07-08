namespace MultiTenancyTest.Services;

public interface ICategoryService
{
    Task<Category> CreatedAsync(Category category);
    Task<Category?> GetByIdAsync(int id);
    Task<IReadOnlyList<Category>> GetAllAsync();
    Task<Category> UpdateAsync(Category category);
    Task DeleteAsync(Category category);
 }