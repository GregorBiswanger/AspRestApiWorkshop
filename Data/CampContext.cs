using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CoreCodeCamp.Data
{
    public class CampContext : DbContext
  {
    private readonly IConfiguration _config;

    public CampContext(DbContextOptions options, IConfiguration config) : base(options)
    {
      _config = config;
    }

    public DbSet<Camp> Camps { get; set; }
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Talk> Talks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_config.GetConnectionString("CodeCamp"));
    }

    protected override void OnModelCreating(ModelBuilder bldr)
    {
      bldr.Entity<Camp>()
        .HasData(new 
        {
            CampId = 1,
            Moniker = "DWX2020",
            Name = "Developer Week 20",
            EventDate = new DateTime(2020, 06, 29),
            LocationId = 1,
            Length = 1
        });

      bldr.Entity<Location>()
        .HasData(new 
        {
          LocationId = 1,
          VenueName = "Nürnberg Convention Center NCC Ost",
          Address1 = "Messezentrum",
          CityTown = "Nürnberg",
          StateProvince = "BY",
          PostalCode = "90471",
          Country = "Germany"
        });

      bldr.Entity<Talk>()
        .HasData(new 
        {
          TalkId = 1,
          CampId = 1,
          SpeakerId = 1,
          Title = "Yes, zu NoSQL mit MongoDB für .NET Entwickler!",
          Abstract = "Wachsende Daten beanspruchen schnellere und klügere Systeme um die Datenverarbeitung bewältigen zu können.",
          Level = 100
        },
        new
        {
          TalkId = 2,
          CampId = 1,
          SpeakerId = 2,
          Title = "MongoDB: Entwurfsmuster für das NoSQL-Schema Design",
          Abstract = "Diese benötigen keine festgelegten Tabellenschemata, sondern haben eine ganz eigene Herangehensweise.",
          Level = 200
        });

      bldr.Entity<Speaker>()
        .HasData(new
        {
          SpeakerId = 1,
          FirstName = "Gregor",
          LastName = "Biswanger",
          BlogUrl = "http://www.cross-platform-blog.de",
          Company = "Freier Berater, Trainer, Sprecher und Autor",
          CompanyUrl = "http://about.me/gregor.biswanger",
          GitHub = "GregorBiswanger",
          Twitter = "BFreakout"
        }, new
        {
          SpeakerId = 2,
          FirstName = "Christine",
          LastName = "Biswanger",
          BlogUrl = "http://www.cross-platform-blog.de",
          Company = "Freie Entwicklerin",
          CompanyUrl = "",
          GitHub = "",
          Twitter = ""
        });

    }

  }
}
