namespace DAL.Entitites
{
    public class Book
    {
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
    }
}