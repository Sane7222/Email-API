namespace EmailAPI.Models {
    public class Settings {
        public string? Sentinel { get; set; }
        public required string Token { get; set; }
        public Dictionary<string, Server> Servers { get; set; } = [];
    }

    public class Server {
        public required string Host { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
