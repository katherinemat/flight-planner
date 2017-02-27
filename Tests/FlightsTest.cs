using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Planner
{
  public class FlightTest : IDisposable
  {
    public FlightTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=planner_TEST;Integrated Security=SSPI;";
    }

    [Fact]
      public void Test_EmptyAtFirst()
      {
        //Arrange, Act
        int result = Flight.GetAll().Count;

        //Assert
        Assert.Equal(0, result);
      }

      [Fact]
      public void Test_EqualOverrideTrueForSameDescription()
      {
        //Arrange, Act
        Flight firstFlight = new Flight("12:00 pm", "Seattle", "Orange County", "Delayed");
        Flight secondFlight = new Flight("12:00 pm", "Seattle", "Orange County", "Delayed");

        //Assert
        Assert.Equal(firstFlight, secondFlight);
      }

      [Fact]
      public void Test_Save()
      {
        //Arrange
        Flight testFlight = new Flight("12:00 pm", "Seattle", "Orange County", "Delayed");
        testFlight.Save();

        //Act
        List<Flight> result = Flight.GetAll();
        List<Flight> testList = new List<Flight>{testFlight};

        //Assert
        Assert.Equal(testList, result);
      }

      [Fact]
      public void Test_SaveAssignsIdToObject()
      {
        //Arrange
        Flight testFlight = new Flight("12:00 pm", "Seattle", "Orange County", "Delayed");
        testFlight.Save();

        //Act
        Flight savedFlight = Flight.GetAll()[0];

        int result = savedFlight.GetId();
        int testId = testFlight.GetId();

        //Assert
        Assert.Equal(testId, result);
      }

      [Fact]
      public void Test_FindFindsFlightInDatabase()
      {
        //Arrange
        Flight testFlight = new Flight("12:00 pm", "Seattle", "Orange County", "Delayed");
        testFlight.Save();

        //Act
        Flight result = Flight.Find(testFlight.GetId());

        //Assert
        Assert.Equal(testFlight, result);
      }

      [Fact]
      public void Test_AddCities_AddsCityToFlight()
      {
        //Arrange
        City departureCity = new City("Chicago");
        departureCity.Save();
        City arrivalCity = new City("Seattle");
        arrivalCity.Save();

        Flight testFlight = new Flight("12:00 pm", departureCity.GetName(), arrivalCity.GetName(), "Delayed");
        testFlight.Save();
        testFlight.AddCities(departureCity, arrivalCity);

        List<City> result = testFlight.GetCities();
        List<City> testList = new List<City>{departureCity, arrivalCity};

        //Assert
        Assert.Equal(testList, result);
      }

      [Fact]
      public void Delete_AllFlights_AllButOneFlight()
      {
        City city1 = new City("Barcelona");
        city1.Save();
        City city2 = new City("Honolulu");
        city2.Save();
        Flight flight1 = new Flight("12:00 pm", city1.GetName(), city2.GetName(), "Delayed");
        flight1.Save();
        flight1.AddCities(city1, city2);

        City city3 = new City("Barcelona");
        city3.Save();
        City city4 = new City("Honolulu");
        city4.Save();
        Flight flight2 = new Flight("14:00 pm", city3.GetName(), city4.GetName(), "Delayed");
        flight2.Save();
        flight2.AddCities(city3, city4);

        flight1.Delete();

        List<Flight> returnedFlights = Flight.GetAll();
        List<Flight> expected = new List<Flight>{flight2};

        Assert.Equal(returnedFlights, expected);
      }

      [Fact]
      public void UpdateDeparture_OneFlight_NewDeparture()
      {
        City city1 = new City("Barcelona");
        city1.Save();
        City city2 = new City("Honolulu");
        city2.Save();
        Flight flight1 = new Flight("12:00 pm", city1.GetName(), city2.GetName(), "Delayed");
        flight1.Save();
        flight1.AddCities(city1, city2);

        flight1.UpdateDeparture("12:00 am");

        string newDeparture = flight1.GetDeparture();

        Assert.Equal(newDeparture, "12:00 am");
      }

      [Fact]
      public void UpdateStatus_OneFlight_NewStatus()
      {
        City city1 = new City("Barcelona");
        city1.Save();
        City city2 = new City("Honolulu");
        city2.Save();
        Flight flight1 = new Flight("12:00 pm", city1.GetName(), city2.GetName(), "Delayed");
        flight1.Save();
        flight1.AddCities(city1, city2);

        flight1.UpdateStatus("On Time");

        string newStatus = flight1.GetStatus();

        Assert.Equal(newStatus, "On Time");
      }

      [Fact]
      public void UpdateDepartureCity_OneFlight_NewDepartureCity()
      {
        City city1 = new City("Barcelona");
        city1.Save();
        City city2 = new City("Honolulu");
        city2.Save();
        Flight flight1 = new Flight("12:00 pm", city1.GetName(), city2.GetName(), "Delayed");
        flight1.Save();
        flight1.AddCities(city1, city2);

        City city3 = new City("Austin");
        city3.Save();

        flight1.UpdateDepartureCity(city3);

        string newDepartureCity = flight1.GetDepartureCity();

        Assert.Equal(newDepartureCity, city3.GetName());
      }

      [Fact]
      public void UpdateArrivalCity_OneFlight_NewArrivalCity()
      {
        City city1 = new City("Barcelona");
        city1.Save();
        City city2 = new City("Honolulu");
        city2.Save();
        Flight flight1 = new Flight("12:00 pm", city1.GetName(), city2.GetName(), "Delayed");
        flight1.Save();
        flight1.AddCities(city1, city2);

        City city3 = new City("Austin");
        city3.Save();

        flight1.UpdateArrivalCity(city3);

        string newArrivalCity = flight1.GetArrivalCity();

        Assert.Equal(newArrivalCity, city3.GetName());
      }

      public void Dispose()
      {
        Flight.DeleteAll();
        City.DeleteAll();
      }
  }
}
