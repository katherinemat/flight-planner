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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flights (departure, departure_city, arrival_city, flight_status) OUTPUT INSERTED.id VALUES (@Departure, @DepartureCity, @ArrivalCity, @Status);", conn);

      SqlParameter departureParameter = new SqlParameter("@Departure", this.GetDeparture());
      cmd.Parameters.Add(departureParameter);

      SqlParameter departureCityParameter = new SqlParameter("@DepartureCity", this.GetDepartureCity());
      cmd.Parameters.Add(departureCityParameter);

      SqlParameter arrivalCityParameter = new SqlParameter("@ArrivalCity", this.GetArrivalCity());
      cmd.Parameters.Add(arrivalCityParameter);

      SqlParameter statusParameter = new SqlParameter("@Status", this.GetStatus());
      cmd.Parameters.Add(statusParameter);

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

    public static Flight Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM flights WHERE id = @FlightId;", conn);
      SqlParameter flightIdParameter = new SqlParameter();
      flightIdParameter.ParameterName = "@FlightId";
      flightIdParameter.Value = id.ToString();
      cmd.Parameters.Add(flightIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundFlightId = 0;
      string foundDeparture = null;
      string foundDepartureCity = null;
      string foundArrivalCity = null;
      string foundStatus = null;

      while(rdr.Read())
      {
        foundFlightId = rdr.GetInt32(0);
        foundDeparture = rdr.GetString(1);
        foundDepartureCity = rdr.GetString(2);
        foundArrivalCity = rdr.GetString(3);
        foundStatus = rdr.GetString(4);
      }
      Flight foundFlight = new Flight(foundDeparture, foundDepartureCity, foundArrivalCity, foundStatus, foundFlightId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundFlight;
    }

    public void AddCities(City departureCity, City arrivalCity)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO flights_cities (flight_id, city_id) VALUES (@FlightId, @DepartureCity); INSERT INTO flights_cities (flight_id, city_id) VALUES (@FlightId, @ArrivalCity);", conn);

      SqlParameter flightIdParameter = new SqlParameter("@FlightId", this.GetId());
      SqlParameter DepartureCityParameter = new SqlParameter("@DepartureCity", departureCity.GetId());
      SqlParameter ArrivalCityParameter = new SqlParameter("@ArrivalCity", arrivalCity.GetId());

      cmd.Parameters.Add(flightIdParameter);
      cmd.Parameters.Add(DepartureCityParameter);
      cmd.Parameters.Add(ArrivalCityParameter);

      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<City> GetCities()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT city_id FROM flights_cities WHERE flight_id = @FlightId;", conn);

      SqlParameter flightIdParameter = new SqlParameter("@FlightId", this.GetId());
      cmd.Parameters.Add(flightIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      List<int> cityIds = new List<int>{};

      while (rdr.Read())
      {
        int cityId = rdr.GetInt32(0);
        cityIds.Add(cityId);
      }
      if(rdr != null)
      {
        rdr.Close();
      }

      List<City> cities = new List<City>{};

      foreach(int cityId in cityIds)
      {
        SqlCommand cityQuery = new SqlCommand("SELECT * FROM cities WHERE id = @CityId;", conn);

        SqlParameter cityIdParameter = new SqlParameter("@CityId", cityId);
        cityQuery.Parameters.Add(cityIdParameter);

        SqlDataReader queryReader = cityQuery.ExecuteReader();
        while(queryReader.Read())
        {
          int thisCityId = queryReader.GetInt32(0);
          string thisCityName = queryReader.GetString(1);
          City thisCity = new City(thisCityName, thisCityId);
          cities.Add(thisCity);
        }
        if(queryReader != null)
        {
          queryReader.Close();
        }
      }
      if(conn != null)
      {
        conn.Close();
      }
      return cities;
    }

    public void UpdateDeparture(string newDeparture)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE flights SET departure = @NewDeparture OUTPUT INSERTED.departure WHERE id=@FlightId;", conn);

      SqlParameter newDepartureParameter = new SqlParameter("@NewDeparture", newDeparture);
      cmd.Parameters.Add(newDepartureParameter);

      SqlParameter clientIdParameter = new SqlParameter("@FlightId", this.GetId());
      cmd.Parameters.Add(clientIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._departure = rdr.GetString(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void UpdateStatus(string newStatus)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE flights SET flight_status = @NewStatus OUTPUT INSERTED.flight_status WHERE id=@FlightId;", conn);

      SqlParameter newStatusParameter = new SqlParameter("@NewStatus", newStatus);
      cmd.Parameters.Add(newStatusParameter);

      SqlParameter flightIdParameter = new SqlParameter("@FlightId", this.GetId());
      cmd.Parameters.Add(flightIdParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._status = rdr.GetString(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void UpdateDepartureCity(City newCity)
    {
      City OldDepartureCity = City.FindByName(this.GetDepartureCity());
      int OldDepartureCityId = OldDepartureCity.GetId();

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE flights SET departure_city = @DepartureCity OUTPUT INSERTED.departure_city WHERE id=@FlightId; UPDATE flights_cities SET city_id = @NewDepartureCityId WHERE flight_id = @FlightId AND city_id = @OldDepartureCityId;", conn);

      SqlParameter departureCityParameter = new SqlParameter("@DepartureCity", newCity.GetName());
      SqlParameter flightIdParameter = new SqlParameter("@FlightId", this.GetId());
      SqlParameter oldDepartureCity = new SqlParameter("@OldDepartureCityId", OldDepartureCityId);
      SqlParameter newDepartureCity = new SqlParameter("@NewDepartureCityId", newCity.GetId());
      cmd.Parameters.Add(departureCityParameter);
      cmd.Parameters.Add(flightIdParameter);
      cmd.Parameters.Add(oldDepartureCity);
      cmd.Parameters.Add(newDepartureCity);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._departureCity = rdr.GetString(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void UpdateArrivalCity(City newCity)
    {
      City OldArrivalCity = City.FindByName(this.GetArrivalCity());
      int OldArrivalCityId = OldArrivalCity.GetId();

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE flights SET arrival_city = @ArrivalCity OUTPUT INSERTED.arrival_city WHERE id=@FlightId; UPDATE flights_cities SET city_id = @NewArrivalCityId WHERE flight_id = @FlightId AND city_id = @OldArrivalCityId;", conn);

      SqlParameter arrivalCityParameter = new SqlParameter("@ArrivalCity", newCity.GetName());
      SqlParameter flightIdParameter = new SqlParameter("@FlightId", this.GetId());
      SqlParameter oldArrivalCity = new SqlParameter("@OldArrivalCityId", OldArrivalCityId);
      SqlParameter newArrivalCity = new SqlParameter("@NewArrivalCityId", newCity.GetId());
      cmd.Parameters.Add(arrivalCityParameter);
      cmd.Parameters.Add(flightIdParameter);
      cmd.Parameters.Add(oldArrivalCity);
      cmd.Parameters.Add(newArrivalCity);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._arrivalCity = rdr.GetString(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM flights WHERE id = @FlightId; DELETE FROM flights_cities WHERE flight_id = @FlightId;", conn);
      SqlParameter flightIdParameter = new SqlParameter("@FlightId", this.GetId());

      cmd.Parameters.Add(flightIdParameter);
      cmd.ExecuteNonQuery();

      if (conn != null)
      {
        conn.Close();
      }
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
