namespace TaskManagementAPI.Models
{
    public class ResponseFormat
    {
        public string Message { get; set; }
        public string Endpoint { get; set; }
        public string Status { get; set; }
        public object Body { get; set; }
    }
}