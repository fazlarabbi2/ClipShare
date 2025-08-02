using ClipShare.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClipShare.Core.Entities
{
    public class VideoFile : BaseEntity
    {
        [Required]
        public string ContentType { get; set; }
        [Required]
        public byte[] Contents { get; set; }
        [Required]
        public string Extension { get; set; }
        public int VideoId { get; set; }

        [ForeignKey("VideoId")]
        public Video Video { get; set; }
    }
}
