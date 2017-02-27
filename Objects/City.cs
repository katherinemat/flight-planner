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

    public static List<City> GetAll()
    {
      List<City> allCities = new List<City>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        string cityName = rdr.GetString(1);
        City newCity = new City(cityName, cityId);
        allCities.Add(newCity);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allCities;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO cities (name) OUTPUT INSERTED.id VALUES (@CityName);", conn);

      SqlParameter nameParameter = new SqlParameter();
      nameParameter.ParameterName = "@CityName";
      nameParameter.Value = this.GetName();
      cmd.Parameters.Add(nameParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if(conn != null)
      {
        conn.Close();
      }
    }

    public static City Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", conn);
      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@CityId";
      flightIdParameter.Value = id.ToString();
      cmd.Parameters.Add(flightIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundCityId = 0;
      string foundCityName = null;

      while(rdr.Read())
      {
        foundCityId = rdr.GetInt32(0);
        foundCityName = rdr.GetString(1);
      }
      City foundCity = new City(foundCityName, foundCityId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundCity;
    }

    public List<Flight> GetFlightsByDepartureOrArrival(bool Arrival)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT flight_id FROM flights_cities WHERE city_id = @CityId;", conn);

      SqlParameter cityIdParameter = new SqlParameter("@CityId", this.GetId());
      cmd.Parameters.Add(cityIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> flightIds = new List<int>{};

      while (rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        flightIds.Add(flightId);
      }
      if(rdr != null)
      {
        rdr.Close();
      }

      List<Flight> flights = new List<Flight>{};

      if (!Arrival)
      {

        foreach(int flightId in flightIds)
        {
          SqlCommand flightQuery = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId and departure_city = @CityName;", conn);

          SqlParameter flightIdParameter = new SqlParameter("@FlightId", flightId);
          flightQuery.Parameters.Add(flightIdParameter);

          SqlParameter cityNameParameter = new SqlParameter("@CityName", this.GetName());
          flightQuery.Parameters.Add(cityNameParameter);

          SqlDataReader queryReader = flightQuery.ExecuteReader();
          while(queryReader.Read())
          {
            int thisFlightId = queryReader.GetInt32(0);
            string thisDeparture = queryReader.GetString(1);
            string thisDepartureCity = queryReader.GetString(2);
            string thisArrivalCity = queryReader.GetString(3);
            string thisStatus = queryReader.GetString(4);
            Flight thisFlight = new Flight(thisDeparture, thisDepartureCity, thisArrivalCity, thisStatus, thisFlightId);
            flights.Add(thisFlight);
          }
          if(queryReader != null)
          {
            queryReader.Close();
          }
        }
      }
      else
      {
        foreach(int flightId in flightIds)
        {
          SqlCommand flightQuery = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId and arrival_city = @CityName;", conn);

          SqlParameter flightIdParameter = new SqlParameter("@FlightId", flightId);
          flightQuery.Parameters.Add(flightIdParameter);

          SqlParameter cityNameParameter = new SqlParameter("@CityName", this.GetName());
          flightQuery.Parameters.Add(cityNameParameter);

          SqlDataReader queryReader = flightQuery.ExecuteReader();
          while(queryReader.Read())
          {
            int thisFlightId = queryReader.GetInt32(0);
            string thisDeparture = queryReader.GetString(1);
            string thisDepartureCity = queryReader.GetString(2);
            string thisArrivalCity = queryReader.GetString(3);
            string thisStatus = queryReader.GetString(4);
            Flight thisFlight = new Flight(thisDeparture, thisDepartureCity, thisArrivalCity, thisStatus, thisFlightId);
            flights.Add(thisFlight);
          }
          if(queryReader != null)
          {
            queryReader.Close();
          }
        }
      }
      if(conn != null)
      {
        conn.Close();
      }
      return flights;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM cities;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }
  }
}
