using ATMBank.Models;
using Microsoft.EntityFrameworkCore;

namespace ATMBank.Data{
    public class ATMContext : DbContext{
        public ATMContext(DbContextOptions<ATMContext> options):base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<User> Email { get; set; }







    }
}