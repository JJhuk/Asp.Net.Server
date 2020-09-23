using Domain;
using Microsoft.EntityFrameworkCore;

namespace Tests.Domain
{
    public class TestUserContext
    {
        public UserContext Db;
        public TestUserContext()
        {
            var option = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase("TestDB")
                .Options;
            
            Db = new UserContext(option);
            Db.Database.EnsureDeletedAsync();
            Db.Database.EnsureCreated();
        }
    }
}