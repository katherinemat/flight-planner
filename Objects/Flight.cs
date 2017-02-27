using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Planner
{
  public class Flight
  {
    private int _id;
    private string _departure;
    private string _departureCity;
    private string _arrivalCity;
    private string _status;

    public Flight(string departure, string departureCity, string arrivalCity, string status, int Id = 0)
    {
      _id = Id;
      _departure = departure;
      _departureCity = departureCity;
      _arrivalCity = arrivalCity;
      _status = status;
    }

    public override bool Equals(System.Object otherFlight)
    {
        if (!(otherFlight is Flight))
        {
          return false;
        }
        else {
          Flight newFlight = (Flight) otherFlight;
          bool idEquality = this.GetId() == newFlight.GetId();
          bool departureEquality = this.GetDeparture() == newFlight.GetDeparture();
          bool departureCityEquality = this.GetDepartureCity() == newFlight.GetDepartureCity();
          bool arrivalCityEquality = this.GetArrivalCity() == newFlight.GetArrivalCity();
          bool statusEquality = this.GetStatus() == newFlight.GetStatus();
          return (idEquality && departureEquality && departureCityEquality && arrivalCityEquality && statusEquality);
        }
    }

    public int GetId()
    {
      return _id;
    }
    public string GetDeparture()
    {
      return _departure;
    }

    public string GetDepartureCity()
    {
      return _departureCity;
    }

    public string GetArrivalCity()
    {
      return _arrivalCity;
    }

    public string GetStatus()
    {
      return _status;
    }

    public static List<Flight> GetAll()
    {
      List<Flight> allFlights = new List<Flight>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int flightId = rdr.GetInt32(0);
        string departure = rdr.GetString(1);
        string departureCity = rdr.GetString(2);
        string arrivalCity = rdr.GetString(3);
        string status = rdr.GetString(4);
        Flight newFlight = new Flight(departure, departureCity, arrivalCity, status, flightId);
        allFlights.Add(newFlight);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allFlights;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM flights;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

  }
}
