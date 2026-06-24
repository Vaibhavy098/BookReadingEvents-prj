using Microsoft.AspNetCore.Identity;

namespace BookReadingEvents.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
