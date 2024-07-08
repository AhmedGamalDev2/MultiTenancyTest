using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MultiTenancyTest.Data;

namespace MultiTenancyTest.Services;

public class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _context;

    public CategoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Category> CreatedAsync(Category category)
    {
        _context.Categories.Add(category);

        await _context.SaveChangesAsync(); // هنا هيعمل setting للكولوم اللي اسمه tenantId اتوماتيك وياخد قيمته من ال tenantId اللي جاي من ال request header

        return category;
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync(); // وهنا هيعمل فلتر اوتوماتيك على حسب ال tenantId
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories.FindAsync(id);
    }
    public async Task<Category> UpdateAsync(Category category)
    {
        //var state =ChangeTracker.Entries<Product>().Where(f => f.State == EntityState.Added);
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

}