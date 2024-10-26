using Microsoft.EntityFrameworkCore;
using Relay.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Hoster> Hosters { get; set; }
    public DbSet<PrivateServer> PrivateServers { get; set; }
}
