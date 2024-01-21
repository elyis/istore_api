using System.ComponentModel.DataAnnotations;
using istore_api.src.Domain.Entities.Response;
using istore_api.src.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace istore_api.src.Domain.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Token), IsUnique = true)]
    public class User
    {
        public Guid Id { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string PasswordHash { get; set; }
        public string? RestoreCode { get; set; }
        public DateTime? RestoreCodeValidBefore { get; set; }
        public bool WasPasswordResetRequest { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenValidBefore { get; set; }
        public string? Image { get; set; }
        public int? ChatId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ProfileBody ToProfileBody()
        {
            return new ProfileBody
            {
                Email = Email,
                Role = Enum.Parse<UserRole>(RoleName),
                UrlIcon = string.IsNullOrEmpty(Image) ? null : $"{Constants.webPathToProfileIcons}{Image}",
            };
        }
    }
}