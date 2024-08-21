namespace EmailAPI.Models {
    public class EmailDTO {
        public required string App { get; set; }
        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
    }
}
