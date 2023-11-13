using Newtonsoft.Json;

namespace HotelBooking.ResponsHanlder
{
    public class ResponseStatus
    {
        public int StatusCode { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public override string ToString() =>  JsonConvert.SerializeObject(this);
    }
}
