using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebDating.Entities.PostEntities
{
    [Table("ImagePost")]
    public class ImagePost
    {
        public ImagePost()
        {

        }

        public ImagePost(int id, string uri, string publicId)
        {
            PostId = id;
            Path = uri;
            PublicId = publicId;
        }

        [Key]
        public int Id { get; set; }
        public string PublicId { get; set; }
        public int PostId { get; set; }
        public string Path { get; set; }
        public Post Post { get; set; }
    }
}
