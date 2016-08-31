using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace friends_circle.Models
{
    public class GoogleMapsAPI
    {

        private static string key = "AIzaSyCB8EcAme0xCZT5knWHBKEV2tzeTWa0L0k";
        public string statusCode { get; private set; }

        /*
            Status codes are standard GoogleAPI responses
            https://developers.google.com/maps/documentation/geocoding/intro#StatusCodes
        */
        public static string RESPONSE_OK = "OK",
                             RESPONSE_NOT_FOUND = "ZERO_RESULTS",
                             RESPONSE_UNKNOWN_ERROR = "UNKNOWN_ERROR";

        /* Simpleton Method - Easier control, encapasuates access and prevents inheritance */
        private static GoogleMapsAPI instance;
        private GoogleMapsAPI() { }
        public static GoogleMapsAPI getInstance()
        {
            if(instance == null)
            {
                instance = new GoogleMapsAPI();
            }
            return instance;
        }

        /* Retrives the approximate latitude and longitude from Google Maps
         * @return string[2] : 0 - Latitude
         *                     1 - Longitude
         *                     2 - Full Street Address
         * @return null : If an error occured
         */
        public string[] getAddressInfoByStreet(string street)
        {
            dynamic response = askGoogle("address=" + street);
            if(checkResponse(response))
            {
                dynamic address = response.results.First.formatted_address;
                dynamic location = response.results.First.geometry.location;
                return new string[] { location.lat, location.lng, address };
            }

            return null;
        }

        /* Retrives the approximate location using latitude and longitude from Google Maps
         * @return string : Full Street Address
         * @return null : If an error occured
         */
        public string getAddressInfoByLocation(string lat, string lng)
        {
            dynamic response = askGoogle(String.Format("latlng={0},{1}", lat, lng));
            if (checkResponse(response))
            {
                return response.results.First.formatted_address;
            }

            return null;
        }

        /* Checks the Google Response to Identify whether or not we got results
         */
        private bool checkResponse(dynamic response)
        {
            if (response == null)
            {
                this.statusCode = RESPONSE_UNKNOWN_ERROR;
                return false;
            }

            /* @TODO add other status codes for accuracy and dubugging */
            string status = response.status;
            if (status == RESPONSE_OK)
            {
                this.statusCode = RESPONSE_OK;
                return true;
            }
            else if (status == RESPONSE_NOT_FOUND)
                this.statusCode = RESPONSE_NOT_FOUND;
            else
                this.statusCode = RESPONSE_UNKNOWN_ERROR;

            return false;
        }

        /* Asks Google for a response
         */
        private dynamic askGoogle(string requestUrl)
        {
            // grab longitude and latitude from Google API
            string url = String.Format("https://maps.googleapis.com/maps/api/geocode/json?{0}&key={1}", requestUrl, key);
            string jsonResponse = new WebClient().DownloadString(url);

            // attempt to deserialize the object
            using (MemoryStream memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(jsonResponse)))
            {
                return JsonConvert.DeserializeObject(jsonResponse);
            }
        }

        public static string getIntellisenseScriptURL()
        {
            return String.Format("https://maps.googleapis.com/maps/api/js?key={0}&signed_in=true&libraries=places&callback=initMap",
                key);
        }

    }
}