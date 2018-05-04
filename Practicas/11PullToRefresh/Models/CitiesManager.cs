using System;
using System.IO;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UIKit;
namespace PullToRefresh
{
    public class CitiesManager
    {
        #region Singleton
        static readonly Lazy<CitiesManager> lazy = new Lazy<CitiesManager>(() => new CitiesManager());
        public static CitiesManager SharedInstance { get => lazy.Value; }
        #endregion

        #region ClassVariables
        HttpClient httpClient;
        Dictionary<string, List<string>> cities;
        #endregion

        #region Events
        public event EventHandler<CitiesEventArgs> CitiesFetched;
        public event EventHandler<CitiesEventArgsFailed> FetchCitiesFailed;
        #endregion

        #region Constructors
        CitiesManager()
        {
            httpClient = new HttpClient();
        }
        #endregion

        #region PublicFunctionality
        public Dictionary<string, List<string>> GetDefaultCities(){
            var citiesJson = File.ReadAllText("cities-incomplete.json");
            return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(citiesJson);
        }

        public void FetchiCities(){
            Task.Factory.StartNew(FetchiCitiesAsync);
            async Task FetchiCitiesAsync(){
                try{
                    if (CitiesFetched == null)
                        return;

                    //var citiesJson = await httpClient.GetStringAsync("https://dl.dropboxusercontent.com/s/0adq8yw6vd5r6bj/cities.json?dl=0");
                    var citiesJson = await httpClient.GetStringAsync("https://dl.dropbo/citi=0");
                    cities = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(citiesJson);
                    //Avisar que ya: 1.- Eventos(Events/Delegate) 2.- Notificaciones(NSNotificationCenter) 3.- (Dentro de un ViewController) a traves de Unwind Segues
                    var e = new CitiesEventArgs(cities);

                    CitiesFetched(this, e);

                }catch(Exception ex){
                    //Avisar que no: 1.- Eventos(Events/Delegate) 2.- Notificaciones(NSNotificationCenter) 3.- (Dentro de un ViewController) a traves de Unwind Segues
                    if (FetchCitiesFailed == null)
                        return;
                    FetchCitiesFailed(this,new CitiesEventArgsFailed(ex.Message));
                }
            }
        }
        #endregion
    }

    public class CitiesEventArgs : EventArgs{
        public Dictionary<string, List<string>> Cities { get; private set;  }
        public CitiesEventArgs(Dictionary<string, List<string>> cities){
            Cities = cities;
        }
    }

    public class CitiesEventArgsFailed : EventArgs
    {
        public string Failed { get; set; }
        public CitiesEventArgsFailed(string failed) { Failed = failed; }
    }
}
