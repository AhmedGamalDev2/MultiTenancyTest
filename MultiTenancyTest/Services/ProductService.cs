using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MultiTenancyTest.Data;

namespace MultiTenancyTest.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> CreatedAsync(Product product)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync(); // هنا هيعمل setting للكولوم اللي اسمه tenantId اتوماتيك وياخد قيمته من ال tenantId اللي جاي من ال request header

        return product;
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync(); // وهنا هيعمل فلتر اوتوماتيك على حسب ال tenantId
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task<Product> UpdateAsync(Product product)
    {
        //var aa = EntityState.Modified;
    //var aa =     ChangeTracker.Entries<Product>().Where(f => f.State == EntityState.Added);
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
    //public async Task<Product> DeleteAsync(int id)
    //{
    //   var product = await _context.Products.FindAsync(id);
    //    if(product is not null)
    //    {
    //        _context.Remove(product);
    //        return product;
    //    }
    //}
}