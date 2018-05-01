# Einführung
- Wer hat ein (echtes) REST Api?
- Was bedeutet das überhaupt?
- REST != HTTP - wenn aber HTTP genutzt wird, dann nutze es richtig!
- [REST Apis must be hypertext driven](http://roy.gbiv.com/untangled/2008/rest-apis-must-be-hypertext-driven)

# HTTP und RMM und eine Fallstudie
- Struktur eines HTTP-Requests (Folie)
- RMM hat Level 0 - 3
- Fallstudie: Zeitbuchung (kurz per Swagger UI zeigen)

#RMM Level 0
- Level 0 einzeichnen (HTTP / RMM)
- Demo (nur fertigen Stand zeigen)

#RMM Level 1
- Level 1 einzeichnen (HTTP / RMM)
- Demo (nur fertigen Stand zeigen)

#RMM Level 2
- Level 2 einzeichnen (HTTP / RMM)
- Demo (nur fertigen Stand zeigen, ausführlicher durchgehen)

# Zwischenfazit
- Level 2 ist das "übliche" "REST"-API Niveau
- Ist das wirklich so smart?
- Ist es sinnvoll bei der Liste der Timesheets einen Liste mit kompletten Objekten zurück zu liefern?
- Wenn nicht: wie komme ich dann Detailinformationen?
- Was ist mit Paging? Woher weiß ein Client welche Links er aufrufen muss?
=> Hier wäre doch ein Standard schön!!

# Der Hypermedia-Zoo
- Was ist eigentlich ein Hypermedia-Format? Content-Type, Spezifikation
- Wer kennt Hypermedia-Formate?
- Beispiele: JsonPatch, HAL, Collection+Json, Siren

# Zum Warm werden: RMM Level 2.5
- Models entsprechend schneiden (TimesheetHeader - ohne Bookings) + Automapper
- Dont patch like an idiot: use [JsonPatch](http://jsonpatch.com/)
- Demo (nur zeigen)

#RMM Level 3
- Level 3 einzeichnen (HTTP / RMM)
- "Volle" Nutzung von HTTP

# RMM Level 3 - HAL
- [HAL Specification](http://stateless.co/hal_specification.html)
- [Halcyon](https://github.com/visualeyes/halcyon)

## Demo
- Startup.cs: auf Media Type Formatter hinweisen
- GetAll - Timesheets
```csharp
            [Produces("application/hal+json")]

            var timesheetModel = timesheets.Select(t => new { t.Id, t.Name });
            var response = new HALResponse(new { Count = timesheetModel.Count() })
                                .AddLinks(new Link("self", "/timesheets"))
                                // Paging links
                                .AddEmbeddedCollection("timesheets", timesheetModel, new Link[] { new Link("self", "/timesheets/{Name}") });
```

- GetByName - Timesheet
```csharp
            [Produces("application/hal+json")]

            var timesheet = repostitory.GetByName(name);

            var timesheetModel = new {
                timesheet.Id,
                timesheet.Name                
            };

            var bookingsModel = timesheet.Bookings.Select(b => new { b.Date, b.Duration });

            var response = new HALResponse(timesheetModel)
                                .AddLinks(new Link("self", "/timesheets/{Name}"))
                                .AddLinks(new Link("bookings", "/timesheets/{Name}/bookings"))
                                .AddEmbeddedCollection("bookings", bookingsModel, new Link[] { new Link("self", $"/timesheets/{timesheet.Name}/bookings/{{Date}}") });
```
- Zur fertigen Demo wechseln und den Rest zeigen

## Fazit HAL

### Pro
- Einfaches Format, leicht zu implementieren
- Gute Möglichkeit um Links standrdisiert einzubauen

### Contra
- Keine Semantik (z. B. Collection)
- [Collections in HAL](https://knpuniversity.com/screencast/rest-ep2/hal-collection) 
- Nur GET

# RMM Level 3 - Collection+json
- "Sematisches" Format, speziell für Collections
- Kann, für Collections, (fast) alle Requests beschreiben (außer PATCH)
- [collection+json Specification](https://github.com/collection-json/spec)
- [Gach.CollectionJson](https://github.com/gach87/collectionjson)

## Demo
- GetAll - Timesheets
```csharp
            var template = new Template();
            template.Data.Add(new DataElement("name") { Prompt = "Name" });

            //var timesheetItems = timesheets.Select(t => new Item<Timesheet, Link>() {  })
            var response = new Collection(new Uri("/timesheets", UriKind.Relative))
            {
                Template = template,
                Items = timesheets.Select(t => new Item(new Uri($"/timesheets/{t.Name}", UriKind.Relative)) {
                    Data = new List<DataElement>() { new DataElement("Id") { Value = t.Id.ToString() }, new DataElement("Name") { Value = t.Name } }
                }).ToList()
            };
```

- Create - Timesheet
```csharp
            var tempalteName = newTimesheetTemplate.Template.Data.Single(d => d.Name == "name").Value;
            if (repostitory.Exists(tempalteName))
                return this.ConflictWithRoute("GetByName", new { name = tempalteName });
```

- Zur fertigen Demo wechseln und den Rest zeigen

## Fazit collection+json

### Pro
- Angelehnt an RSS
- Kann alle Requests beschreiben die für eine Collection von intersse sind

### Contra
- Schlecht geeignet um einzelen Elemente einer Colleciton als Hypermedia darzustellen
- Dafür wäre wieder ein weiterer Media-Type notwendig, z. B. HAL
- Für POST / PUT muss die "Signatur" der Methoden speziell auf collection+json abgestimmt sein

# RMM Level 3 - Siren
- [Siren Specification](https://github.com/kevinswiber/siren)
- [FluentSiren](https://github.com/agabani/FluentSiren)

## Demo
- Startup.cs: Siren Output Formatter

- GetAll - Timesheets
```csharp
            var sirenResultBuilder = new EntityBuilder()
                .WithClass("timesheet")
                .WithClass("collection")
                .WithLink(new LinkBuilder()
                    .WithRel("self")
                    .WithHref(this.Url.Link("GetAll", new {}))
                );

            foreach (var timesheetEntry in result)
            {
                sirenResultBuilder
                    .WithSubEntity(new EmbeddedLinkBuilder()
                        .WithClass("timesheet")
                        .WithTitle(timesheetEntry.Name)
                        .WithRel("timesheet")
                        .WithHref(this.Url.Link("GetByName", new { name = timesheetEntry.Name }))
                    );
            }

            sirenResultBuilder.WithAction(new ActionBuilder()
                .WithName("addTimesheet")
                .WithTitle("Add a new Timesheet")
                .WithMethod("POST")
                .WithHref(this.Url.Link("GetAll", new {}))
                .WithType("application/json")
                .WithField(new FieldBuilder().WithName("Name").WithType("text"))
            );

            var response = sirenResultBuilder.Build();
```

- GetByName - Timesheet 
```csharp
            var sirenResultBuilder = new EntityBuilder()
                .WithClass("timesheet")
                .WithProperty("Name", timesheet.Name)
                .WithLink(new LinkBuilder()
                    .WithRel("self")
                    .WithHref(this.Url.Link("GetByName", new { name = timesheet.Name}))
                );

            foreach (var timebooking in timesheet.Bookings)
            {
                sirenResultBuilder
                    .WithSubEntity(new EmbeddedRepresentationBuilder()
                        .WithClass("timebooking")
                        .WithProperty("Date", timebooking.Date)
                        .WithProperty("Duration", timebooking.Duration)
                        .WithRel("timebooking")
                        .WithLink(new LinkBuilder() 
                            .WithRel("timebooking")
                            .WithHref(this.Url.Action("GetByDate", "TimeBooking", new { name = timesheet.Name, date = timebooking.Date }))
                        )
                    );
            }

            var href = this.Url.Action("Create", "TimeBooking", new { name = timesheet.Name});
            var href1 = this.Url.Action("GetByDate", "TimeBooking", new { name = "Test", date = System.DateTime.Today });

            sirenResultBuilder.WithAction(new ActionBuilder()
                .WithName("addTimebooking")
                .WithTitle("Add a new Timebooking")
                .WithMethod("POST")
                .WithHref(this.Url.Action("Create", "TimeBooking", new { name = timesheet.Name}))
                .WithType("application/json")
                .WithField(new FieldBuilder().WithName("Date").WithType("text"))
                .WithField(new FieldBuilder().WithName("Start").WithType("text"))
                .WithField(new FieldBuilder().WithName("Pause").WithType("text"))
                .WithField(new FieldBuilder().WithName("End").WithType("text"))
            );

            var result = sirenResultBuilder.Build();
```

- Zur fertigen Demo wechseln und zeigen

## Fazit siren

### Pro
- Kann alle Requests beschreiben
- POST / PUT / PATCH - Interface bleibt stabil

### Contra
- keine eigene Semantik
- Viel Schreibarbeit
- Maintainer ist "nur" eine Einzelperson

# Fazit

## Pro
- Standards nutzen wann immer möglich!
- "Mindestens" HAL schon mal für Paging / Detailabfragen

## Contra
- Alle NuGet-Pakete haben Nachteile
- Kein Format wirkt wirklich "weit verbreitet"

## Also?
- Swagger as Hypermedia?