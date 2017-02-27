using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Planner
{
  public class City
  {
    private int _id;
    private string _name;

    public City(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherCity)
    {
        if (!(otherCity is City))
        {
          return false;
        }
        else
        {
          City newCity = (City) otherCity;
          bool idEquality = this.GetId() == newCity.GetId();
          bool nameEquality = this.GetName() == newCity.GetName();
          return (idEquality && nameEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetName()
    {
      return _name;
    }
    public void SetName(string newName)
    {
      _name = newName;
    }
  }
}
