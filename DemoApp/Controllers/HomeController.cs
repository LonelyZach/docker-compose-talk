using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DemoApp.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace DemoApp.Controllers
{
  public class HomeController : Controller
  {
    public IConfiguration Configuration { get; }
    public IMongoClient DatabaseClient { get; }

    public HomeController(IConfiguration configuration, IMongoClient databaseClient)
    {
      Configuration = configuration;
      DatabaseClient = databaseClient;
    }

    public IActionResult Index()
    {
      var database = DatabaseClient.GetDatabase("DemoApp");
      var collection = database.GetCollection<Thing>("Things");
      var things = collection.Find(x => true).ToList();
      return View(things);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost("create")]
    public IActionResult PostThing(Thing thing)
    {
      thing.Id = Guid.NewGuid();
      var database = DatabaseClient.GetDatabase("DemoApp");
      var collection = database.GetCollection<Thing>("Things");
      collection.InsertOne(thing);
      return RedirectToAction("Index");
    }
  }
}
