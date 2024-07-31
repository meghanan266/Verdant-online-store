using System.Text.Json.Serialization;

namespace E_commerce_API.DataModel
{
    public class ShipmentTrack
    {
        [JsonPropertyName("current_status")]
        public string CurrentStatus { get; set; }
    }

    public class ShipmentTrackActivity
    {
        [JsonPropertyName("date")]
        public string DateString { get; set; }

        [JsonIgnore] // Ignore this property during serialization
        public DateTime Date => DateTime.ParseExact(DateString, "yyyy-MM-dd HH:mm:ss", null);

        [JsonPropertyName("activity")]
        public string Activity { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }
    }

    public class TrackingData
    {
        [JsonPropertyName("shipment_track")]
        public List<ShipmentTrack> ShipmentTrack { get; set; }

        [JsonPropertyName("shipment_track_activities")]
        public List<ShipmentTrackActivity> ShipmentTrackActivities { get; set; }

        [JsonPropertyName("track_url")]
        public string TrackUrl { get; set; }
    }

    public class DeliveryTrackingDetails
    {
        [JsonPropertyName("tracking_data")]
        public TrackingData TrackingData { get; set; }
    }

    public class DeliveryTracking
    {
        public string TrackUrl { get; set; }
        public string CurrentStatus { get; set; }
    }
}