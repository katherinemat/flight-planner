using Nancy;
using System;
using System.Collections.Generic;

namespace Planner
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        List<City> allCities = City.GetAll();
        return View["index.cshtml", allCities];
      };

      Post["/"] = _ => {
        Flight newFlight = new Flight(Request.Form["departure"], Request.Form["departure-city"], Request.Form["arrival-city"], Request.Form["status"]);
        newFlight.Save();
        return View["add_flight_success.cshtml", newFlight];
      };

      Get["/cities"] = _ => {
        List<City> allCities = City.GetAll();
        return View["cities.cshtml", allCities];
      };

      Post["/cities"] = _ => {
        string userInput = Request.Form["name"];
        City newCity = new City(userInput);
        newCity.Save();
        List<City> allCities = City.GetAll();
        return View["cities.cshtml", allCities];
      };

      Get["/flights"] = _ => {
        List<Flight> allFlights = Flight.GetAll();
        return View["flights.cshtml", allFlights];
      };

      Post["/flights"] = _ => {
        string departure = Request.Form["departure"];
        int departureCityId = Request.Form["departure-city"];
        int arrivalCityId = Request.Form["arrival-city"];
        string status = Request.Form["status"];

        City departureCity = City.Find(departureCityId);
        City arrivalCity = City.Find(arrivalCityId);

        string departureCityString = departureCity.GetName();
        string arrivalCityString = arrivalCity.GetName();

        Flight newFlight = new Flight(departure, departureCityString, arrivalCityString, status);
        newFlight.Save();
        newFlight.AddCities(departureCity, arrivalCity);

        List<Flight> allFlights = Flight.GetAll();
        return View["flights.cshtml", allFlights];
      };
    }
  }
}
