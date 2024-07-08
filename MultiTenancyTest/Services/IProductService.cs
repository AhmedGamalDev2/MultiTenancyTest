namespace MultiTenancyTest.Services;

public interface IProductService
{
    Task<Product> CreatedAsync(Product product);
    Task<Product?> GetByIdAsync(int id);
    Task<IReadOnlyList<Product>> GetAllAsync();
    Task<Product> UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    //Task<Product> DeleteAsync(int id);
}