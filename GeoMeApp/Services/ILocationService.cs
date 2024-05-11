using Metal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMeApp.Services
{
    public interface ILocationService
    {
        public Location? Location { get; }
        public double LocationUpdateSeconds { get; set; }
        void StartLocationUpdates();
        void StopLocationUpdates();
        void OnSleep();
        void OnResume();
    }
}
