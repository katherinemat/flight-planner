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
    }
  }
}
