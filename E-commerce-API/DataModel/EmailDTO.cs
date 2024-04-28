namespace E_commerce_API.DataModel
{
    public class EmailDTO
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailDTO(string to, string subject, string content)
        {
            To = to;
            Subject = subject;  
            Content = content;
        }
    }
}
