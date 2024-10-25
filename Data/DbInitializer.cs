public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();

        // Инициализация данных
        if (!context.Users.Any())
        {
            context.Users.Add(new User { Username = "admin", Password = "password" });
            context.SaveChanges();
        }
    }
}
