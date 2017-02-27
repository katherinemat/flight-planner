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

    public void Dispose()
    {
      // Flight.DeleteAll();
      City.DeleteAll();
    }
  }
}
