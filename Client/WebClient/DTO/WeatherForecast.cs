using System;

namespace WebClient
{
    public class WeatherForecast
    {
        public WeatherInfo weatherinfo { get; set; }
    }

    public class WeatherInfo
    {
        public string AP { get; set; }
        public string Radar { get; set; }
        public string SD { get; set; }
        public string WD { get; set; }
        public string WS { get; set; }
        public string WSE { get; set; }
        public string city { get; set; }
        public string cityid { get; set; }
        public int isRadar { get; set; }
        public string njd { get; set; }
        public float sm { get; set; }
        public float temp { get; set; }
        public string time { get; set; }
    }
}
