using HangfireAndSendGrid.Models;
using Microsoft.EntityFrameworkCore;

namespace HangfireAndSendGrid;

public class HangfireSendGridDbContext : DbContext
{
    public HangfireSendGridDbContext(DbContextOptions<HangfireSendGridDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }
}