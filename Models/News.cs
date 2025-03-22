using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ParwatPiyushNewsPortal.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        //[Required(ErrorMessage = "Title is required.")]
        //public string Title { get; set; }
        [Required(ErrorMessage = "Content is required.")]
        public string? Content { get; set; }
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public User? Author { get; set; }
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public string? ImagePath { get; set; }
        [Required]
        public int? TopicId { get; set; }

        [ForeignKey("TopicId")]
        public Topics? Topic { get; set; }

        public string? CreatedBy { get; set; }

    }
}
