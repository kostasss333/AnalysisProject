using System.ComponentModel.DataAnnotations;

namespace DAL.Entitites
{
    public class Writer
    {
        [Key] public string ID { get; set; }
        public string orchidUrl { get; set; }
        public string privateUrl { get; set; }
        public string writerRole { get; set; }
        public bool isRealUser { get; set; }
    }
}