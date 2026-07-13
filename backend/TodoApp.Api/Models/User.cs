using System.ComponentModel.DataAnnotations;

namespace TodoApp.Api.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(50)]
		public string Username { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Email { get; set; } = string.Empty;

		[Required]
		public string PasswordHash { get; set; } = string.Empty;

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


		public ICollection<TaskItem> Tasks { get; set; }= new List<TaskItem>();
		public ICollection<Category> Categories { get; set; }= new List<Category>();

    }																		   
}																			   
