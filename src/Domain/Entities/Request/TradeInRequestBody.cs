using System.ComponentModel.DataAnnotations;

namespace istore_api.src.Domain.Entities.Request
{
    public class TradeInRequestBody
    {
        [Phone, Required]
        public string Phone { get; set; }

        [Required]
        public string DeviceModel { get; set; }

        [Required]
        public string CorpusState { get; set; }

        [Required]
        public string DisplayState { get; set; }

        [Required]
        public string BatteryState { get; set; }

    }
}