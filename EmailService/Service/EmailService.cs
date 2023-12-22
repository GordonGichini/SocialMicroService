using EmailService.Data;
using EmailService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Service
{
    public class EmailService
    {
        private DbContextOptions<ApplicationDbContext> options;

        public EmailService(DbContextOptions<ApplicationDbContext> options)
        {
            this.options = options;
        }

        public async Task addDatatoDatabase(EmailLogger logger)
        {
            var _db = new ApplicationDbContext(options);
            _db.EmailLoggers.Add(logger);
            await _db.SaveChangesAsync();
        }
    }
}