using System.ComponentModel.DataAnnotations;

namespace TaskManagerAPI.Models
{
    public enum TaskFrequency  // Enum Tanımla
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }

    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        public string Description { get; set; } = "";

        public bool IsCompleted { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // String yerine integer kullanarak daha performanslı bir kod elde etmek istedim.
        [Required]
        public TaskFrequency Frequency { get; set; } = TaskFrequency.Daily;
    }
}
