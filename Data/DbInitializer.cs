using Microsoft.EntityFrameworkCore;
using Relay.Models;

namespace Relay.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await context.Users.AnyAsync())
            {
                await context.Users.AddAsync(new User { Username = "admin", Password = "password" });
                await context.SaveChangesAsync();
            }
        }
    }
}
