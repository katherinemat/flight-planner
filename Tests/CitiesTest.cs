using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Planner
{
  public class CityTest : IDisposable
  {
    public CityTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=planner_TEST;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CitiesEmptyAtFirst()
    {
      //Arrange, Act
      int result = City.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueForSameName()
    {
      //Arrange, Act
      City firstCity = new City("Seattle");
      City secondCity = new City("Seattle");

      //Assert
      Assert.Equal(firstCity, secondCity);
    }

    [Fact]
    public void Test_Save_SavesCityToDatabase()
    {
      //Arrange
      City testCity = new City("Miami");
      testCity.Save();

      //Act
      List<City> result = City.GetAll();
      List<City> testList = new List<City>{testCity};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToCityObject()
    {
      //Arrange
      City testCity = new City("Miami");
      testCity.Save();

      //Act
      City savedCity = City.GetAll()[0];

      int result = savedCity.GetId();
      int testId = testCity.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsCityInDatabase()
    {
      //Arrange
      City testCity = new City("Chicago");
      testCity.Save();

      //Act
      City foundCity = City.Find(testCity.GetId());

      //Assert
      Assert.Equal(testCity, foundCity);
    }

    [Fact]
    public void GetFlightsByDeparture_GetFlightsByCity_ConnectedFlights()
    {
      City city1 = new City("Seattle");
      city1.Save();
      City city2 = new City("Chicago");
      city2.Save();
      City city3 = new City("Orange County");
      city3.Save();

      Flight flight1 = new Flight("12:00 pm", "Seattle", "Orange County", "Delayed");
      flight1.Save();
      flight1.AddCities(city1, city3);
      Flight flight2 = new Flight("2am", "Chicago", "Seattle", "On-time");
      flight2.Save();
      flight2.AddCities(city2, city1);

      List<Flight> expected = new List<Flight>{flight1};
      List<Flight> returnedFlights = city1.GetFlightsByDepartureOrArrival(false);

      Assert.Equal(expected, returnedFlights);
    }

    [Fact]
    public void GetFlightsByArrival_GetFlightsByCity_ConnectedFlights()
    {
      City city1 = new City("Seattle");
      city1.Save();
      City city2 = new City("Chicago");
      city2.Save();
      City city3 = new City("Orange County");
      city3.Save();

      Flight flight1 = new Flight("12:00 pm", "Seattle", "Orange County", "Delayed");
      flight1.Save();
      flight1.AddCities(city1, city3);
      Flight flight2 = new Flight("2am", "Chicago", "Seattle", "On-time");
      flight2.Save();
      flight2.AddCities(city2, city1);

      List<Flight> expected = new List<Flight>{flight2};
      List<Flight> returnedFlights = city1.GetFlightsByDepartureOrArrival(true);

      Assert.Equal(expected, returnedFlights);
    }

    public void Dispose()
    {
      Flight.DeleteAll();
      City.DeleteAll();
    }
  }
}
