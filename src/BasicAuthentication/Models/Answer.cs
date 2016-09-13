using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BasicAuthentication.Models
{
    [Table("Answers")]
    public class Answer
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public int QuestionId { get; set; }
        public virtual Question question { get; set; }
    }
}
