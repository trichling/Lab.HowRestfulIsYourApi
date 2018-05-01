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
- Demo

#RMM Level 1
- Level 1 einzeichnen (HTTP / RMM)
- Demo

#RMM Level 2
- Level 2 einzeichnen (HTTP / RMM)
- Demo

# Zwischenfazit
- Level 2 ist das "übliche" "REST"-API Niveau
- Ist das wirklich so smart?
- Ist es sinnvoll bei der Liste der Timesheets einen Liste mit kompletten Objekten zurück zu liefern?
- Wenn nicht: wie komme ich dann Detailinformationen?
- Was ist mit Paging? Woher weiß ein Client welche Links er aufrufen muss?
=> Hier wäre doch ein Standard schön!!

# Zum Warm werden: RMM Level 2.5
- Models entsprechend schneiden (TimesheetHeader - ohne Bookings)
- Dont patch like an idiot