using System.ComponentModel.DataAnnotations;

namespace ToDoListAPI.Model.Dto
{
    public class ToDoTaskDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
    }
}
