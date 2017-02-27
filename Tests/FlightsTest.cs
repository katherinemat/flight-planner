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

    //   [Fact]
    //   public void Test_Save()
    //   {
    //     //Arrange
    //     Task testTask = new Task("Mow the lawn");
    //     testTask.Save();
    //
    //     //Act
    //     List<Task> result = Task.GetAll();
    //     List<Task> testList = new List<Task>{testTask};
    //
    //     //Assert
    //     Assert.Equal(testList, result);
    //   }
    //
    //   [Fact]
    //   public void Test_SaveAssignsIdToObject()
    //   {
    //     //Arrange
    //     Task testTask = new Task("Mow the lawn");
    //     testTask.Save();
    //
    //     //Act
    //     Task savedTask = Task.GetAll()[0];
    //
    //     int result = savedTask.GetId();
    //     int testId = testTask.GetId();
    //
    //     //Assert
    //     Assert.Equal(testId, result);
    //   }
    //
    //   [Fact]
    //   public void Test_FindFindsTaskInDatabase()
    //   {
    //     //Arrange
    //     Task testTask = new Task("Mow the lawn");
    //     testTask.Save();
    //
    //     //Act
    //     Task result = Task.Find(testTask.GetId());
    //
    //     //Assert
    //     Assert.Equal(testTask, result);
    //   }
    //
    //   [Fact]
    //   public void Test_AddCategory_AddsCategoryToTask()
    //   {
    //     //Arrange
    //     Task testTask = new Task("Mow the lawn");
    //     testTask.Save();
    //
    //     Category testCategory = new Category("Home stuff");
    //     testCategory.Save();
    //
    //     //Act
    //     testTask.AddCategory(testCategory);
    //
    //     List<Category> result = testTask.GetCategories();
    //     List<Category> testList = new List<Category>{testCategory};
    //
    //     //Assert
    //     Assert.Equal(testList, result);
    //   }
    //
    //   [Fact]
    //   public void Test_GetCategories_ReturnsAllTaskCategories()
    //   {
    //     //Arrange
    //     Task testTask = new Task("Mow the lawn");
    //     testTask.Save();
    //
    //     Category testCategory1 = new Category("Home stuff");
    //     testCategory1.Save();
    //
    //     Category testCategory2 = new Category("Work stuff");
    //     testCategory2.Save();
    //
    //     //Act
    //     testTask.AddCategory(testCategory1);
    //     List<Category> result = testTask.GetCategories();
    //     List<Category> testList = new List<Category> {testCategory1};
    //
    //     //Assert
    //     Assert.Equal(testList, result);
    //   }
    //
    //   [Fact]
    // public void Test_Delete_DeletesTaskAssociationsFromDatabase()
    // {
    //   //Arrange
    //   Category testCategory = new Category("Home stuff");
    //   testCategory.Save();
    //
    //   string testDescription = "Mow the lawn";
    //   Task testTask = new Task(testDescription);
    //   testTask.Save();
    //
    //   //Act
    //   testTask.AddCategory(testCategory);
    //   testTask.Delete();
    //
    //   List<Task> resultCategoryTasks = testCategory.GetTasks();
    //   List<Task> testCategoryTasks = new List<Task> {};
    //
    //   //Assert
    //   Assert.Equal(testCategoryTasks, resultCategoryTasks);
    // }

      public void Dispose()
      {
        Flight.DeleteAll();
        // Category.DeleteAll();
      }
  }
}
