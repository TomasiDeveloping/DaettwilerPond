using Newtonsoft.Json;

namespace Application.Models;

public class TtnUplinkMessage
{
    public class ApplicationIds
    {
        [JsonProperty("application_id")]
        public string ApplicationId { get; set; }
    }

    public class DataRate
    {
        [JsonProperty("lora")]
        public Lora Lora { get; set; }
    }

    public class DecodedPayload
    {
        [JsonProperty("ADC_CH0V")]
        public int ADCCH0V { get; set; }

        [JsonProperty("BatV")]
        public double BatV { get; set; }

        [JsonProperty("Digital_IStatus")]
        public string DigitalIStatus { get; set; }

        [JsonProperty("Door_status")]
        public string DoorStatus { get; set; }

        [JsonProperty("EXTI_Trigger")]
        public string EXTITrigger { get; set; }

        [JsonProperty("TempC1")]
        public double TempC1 { get; set; }

        [JsonProperty("TempC2")]
        public double TempC2 { get; set; }

        [JsonProperty("TempC3")]
        public double TempC3 { get; set; }

        [JsonProperty("Work_mode")]
        public string WorkMode { get; set; }
    }

    public class EndDeviceIds
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("application_ids")]
        public ApplicationIds ApplicationIds { get; set; }

        [JsonProperty("dev_eui")]
        public string DevEui { get; set; }

        [JsonProperty("join_eui")]
        public string JoinEui { get; set; }

        [JsonProperty("dev_addr")]
        public string DevAddr { get; set; }
    }

    public class GatewayIds
    {
        [JsonProperty("gateway_id")]
        public string GatewayId { get; set; }

        [JsonProperty("eui")]
        public string Eui { get; set; }
    }

    public class Location
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("altitude")]
        public int Altitude { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }

    public class Locations
    {
        [JsonProperty("user")]
        public User User { get; set; }

        [JsonProperty("version_ids")]
        public VersionIds VersionIds { get; set; }

        [JsonProperty("network_ids")]
        public NetworkIds NetworkIds { get; set; }
    }

    public class Lora
    {
        [JsonProperty("bandwidth")]
        public int Bandwidth { get; set; }

        [JsonProperty("spreading_factor")]
        public int SpreadingFactor { get; set; }
    }

    public class NetworkIds
    {
        [JsonProperty("net_id")]
        public string NetId { get; set; }

        [JsonProperty("tenant_id")]
        public string TenantId { get; set; }

        [JsonProperty("cluster_id")]
        public string ClusterId { get; set; }
    }

    public class Root
    {
        [JsonProperty("end_device_ids")]
        public EndDeviceIds EndDeviceIds { get; set; }

        [JsonProperty("correlation_ids")]
        public List<string> CorrelationIds { get; set; }

        [JsonProperty("received_at")]
        public DateTime ReceivedAt { get; set; }

        [JsonProperty("uplink_message")]
        public UplinkMessage UplinkMessage { get; set; }

        [JsonProperty("rx_metadata")]
        public List<RxMetadatum> RxMetadata { get; set; }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }

        [JsonProperty("consumed_airtime")]
        public string ConsumedAirtime { get; set; }

        [JsonProperty("locations")]
        public Locations Locations { get; set; }

        [JsonProperty("simulated")]
        public bool Simulated { get; set; }
    }

    public class RxMetadatum
    {
        [JsonProperty("gateway_ids")]
        public GatewayIds GatewayIds { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("rssi")]
        public int Rssi { get; set; }

        [JsonProperty("channel_rssi")]
        public int ChannelRssi { get; set; }

        [JsonProperty("snr")]
        public double Snr { get; set; }

        [JsonProperty("uplink_token")]
        public string UplinkToken { get; set; }

        [JsonProperty("channel_index")]
        public int ChannelIndex { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }
    }

    public class Settings
    {
        [JsonProperty("data_rate")]
        public DataRate DataRate { get; set; }

        [JsonProperty("coding_rate")]
        public string CodingRate { get; set; }

        [JsonProperty("frequency")]
        public string Frequency { get; set; }
    }

    public class UplinkMessage
    {
        [JsonProperty("session_key_id")]
        public string SessionKeyId { get; set; }

        [JsonProperty("f_cnt")]
        public int FCnt { get; set; }

        [JsonProperty("f_port")]
        public int FPort { get; set; }

        [JsonProperty("frm_payload")]
        public string FrmPayload { get; set; }

        [JsonProperty("decoded_payload")]
        public DecodedPayload DecodedPayload { get; set; }
    }

    public class User
    {
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("altitude")]
        public int Altitude { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }

    public class VersionIds
    {
        [JsonProperty("brand_id")]
        public string BrandId { get; set; }

        [JsonProperty("model_id")]
        public string ModelId { get; set; }

        [JsonProperty("hardware_version")]
        public string HardwareVersion { get; set; }

        [JsonProperty("firmware_version")]
        public string FirmwareVersion { get; set; }

        [JsonProperty("band_id")]
        public string BandId { get; set; }
    }

}