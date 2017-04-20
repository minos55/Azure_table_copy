using Microsoft.WindowsAzure.Storage.Table;
namespace TableCopyTests
{
    public class CityWeatherTableEntity : TableEntity
    {
        public CityWeatherTableEntity()
        {
        }

        public CityWeatherTableEntity(string cityName, string countryName)
        {
            PartitionKey = countryName;
            RowKey = cityName;
        }


        public CityWeatherTableEntity(string cityName, string countryName, string weatherParameter, string weatherDescription, float temp)
        {
            PartitionKey = countryName;
            RowKey = cityName;
            WeatherParameter = weatherParameter;
            WeatherDescription = weatherDescription;
            Temp = temp.ToString();
        }

        public string WeatherParameter { get; set; } = string.Empty;
        public string WeatherDescription { get; set; } = string.Empty;
        public string Temp { get; set; } = string.Empty;
    }
}
