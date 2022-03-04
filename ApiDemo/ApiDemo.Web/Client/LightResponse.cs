namespace ApiDemo.Web.Client
{
    public class LightResponse
    {
        public LightResponse()
        {
            Time = $"[{ DateTime.Now.ToString("T")}] ";
        }
        public int Id { get; set; }
        public string Time { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
        public string Style { get; set; }
    }
}
