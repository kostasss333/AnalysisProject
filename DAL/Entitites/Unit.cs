using System.ComponentModel.DataAnnotations;

namespace DAL.Entitites
{
    public class Unit
    {
        [Key] public string ID { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string institute { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string ownded_by { get; set; }
    }
}