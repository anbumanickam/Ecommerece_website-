using Microsoft.EntityFrameworkCore;
using Shop.Model;

namespace Application.Data;
public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public required DbSet<Brand> Brand{ get; set; }
  }


