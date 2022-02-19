namespace estoque_tek.Settings
{
    public class MongoDbSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string ConnectionString
        {
            get
            {
                return $"mongo://{Host}:{Port}";
            }
        }
    }
}
