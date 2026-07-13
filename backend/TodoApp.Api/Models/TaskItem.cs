using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Api.Models
{
	public class TaskItem
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(200)]
		public string Title { get; set; } = string.Empty;

		[MaxLength(100)]
		public string? Description { get; set; }

		public bool IsCompleted { get; set; }= false;

		public DateTime? DueDate {  get ; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

		public int UserId { get; set; }
		[ForeignKey("UserId")]
		public User User { get; set; }= null!;

		public int? CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public Category? Category { get; set; }



    }
}
