namespace Project2.Models
{
    public class PublishmentModel
    {
        public string publisher_name { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string year { get; set; }
        public string ID { get; set; }

        public string publisher { get; set; }

        public string volume { get; set; }
        public int number { get; set; }
        public string series { get; set; }
        public string address { get; set; }
        public string edition { get; set; }
        public string ISBN { get; set; }
        public string ISSN { get; set; }

        public string mag_name { get; set; }
        public string pages { get; set; }
        public string author { get; set; }
        public string url { get; set; }

        public string booktitle { get; set; }
        public string editor { get; set; }
        public string chapter { get; internal set; }
    }
}