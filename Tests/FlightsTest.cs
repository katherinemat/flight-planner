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
        testFlight.AddCities(arrivalCity, departureCity);

        List<City> result = testFlight.GetCities();
        List<City> testList = new List<City>{departureCity, arrivalCity};

        //Assert
        Assert.Equal(testList, result);
      }

    //   [Fact]
    //   public void Test_GetCategories_ReturnsAllFlightCategories()
    //   {
    //     //Arrange
    //     Flight testFlight = new Flight("Mow the lawn");
    //     testFlight.Save();
    //
    //     Category testCategory1 = new Category("Home stuff");
    //     testCategory1.Save();
    //
    //     Category testCategory2 = new Category("Work stuff");
    //     testCategory2.Save();
    //
    //     //Act
    //     testFlight.AddCategory(testCategory1);
    //     List<Category> result = testFlight.GetCategories();
    //     List<Category> testList = new List<Category> {testCategory1};
    //
    //     //Assert
    //     Assert.Equal(testList, result);
    //   }
    //
    //   [Fact]
    // public void Test_Delete_DeletesFlightAssociationsFromDatabase()
    // {
    //   //Arrange
    //   Category testCategory = new Category("Home stuff");
    //   testCategory.Save();
    //
    //   string testDescription = "Mow the lawn";
    //   Flight testFlight = new Flight(testDescription);
    //   testFlight.Save();
    //
    //   //Act
    //   testFlight.AddCategory(testCategory);
    //   testFlight.Delete();
    //
    //   List<Flight> resultCategoryFlights = testCategory.GetFlights();
    //   List<Flight> testCategoryFlights = new List<Flight> {};
    //
    //   //Assert
    //   Assert.Equal(testCategoryFlights, resultCategoryFlights);
    // }

      public void Dispose()
      {
        Flight.DeleteAll();
        City.DeleteAll();
      }
  }
}
