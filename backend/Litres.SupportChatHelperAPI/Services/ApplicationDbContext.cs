using Litres.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Litres.SupportChatHelperAPI.Services;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
    ) : DbContext(options)
{
    public DbSet<Message> Message { get; set; }
    public DbSet<Chat> Chat { get; set; }
}