using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace  CommandService.Models
{
    public class Platform{
        [Key]
        [Required]
        public int Id { get; set; }
         [Required]
        public string Name { get; set; }
         [Required]
        public int ExternalID { get; set; }
        public ICollection<Command> Commands { get; set; }=new List<Command>();
    }
}