// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
//To do: add comments and console.clear()
bool Dialog()
{
    Console.WriteLine("Ova akcija trrajno mijenja podatke aplikacije, želite li je napraviti");
    Console.WriteLine("1-DA" + "\n" + "0-Ne");
    var final=Console.ReadLine();
    if (final == "1")
    {
        return true;
    }
    else
    {
        return false;
    }
}
void FutureMenu (List<Event> futureEvents, List<Event> events)
{
    var idList = new List<string>();
    foreach (var item in futureEvents)
    {
        ispis(item);
        idList.Add(item.Id.ToString());
    }

    var loop = 1;
    while (loop == 1)
    {
        Console.WriteLine("Upišite što želite napraviti");
        Console.WriteLine("1 - Uklonite event");
        Console.WriteLine("2 - Uklonite sudionika");
        Console.WriteLine("3 - Ispis");
        Console.WriteLine("0 - Main Menu");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "2":

                Console.WriteLine("Upišitre id eventa kojemu želite maknuti sudionika");
                var idToRemove = Console.ReadLine();
                if (idList.Contains(idToRemove) == true)
                {
                    futureEvents[idList.IndexOf(idToRemove)].RemoveAttendee();
                }
                else
                {
                    Console.WriteLine("Event ne postoji");
                }
                break;
            case "3":
                foreach (var item in futureEvents)
                {
                    ispis(item);
                }
                break;
            case "1":
                Console.WriteLine("Upišite id eventa kojeg i uklonili");
                var idRemove = Console.ReadLine();
                if (idList.Contains(idRemove) == true)
                {
                    var confirmDewletion=Dialog();
                    if (confirmDewletion == true)
                    {

                        events.Remove(events.Find(i => i.Id == futureEvents[idList.IndexOf(idRemove)].Id));
                        futureEvents.RemoveAt(idList.IndexOf(idRemove));
                        Console.WriteLine("Izbrisano");
                        idList.Remove(idRemove);
                    }                    
                    break;
                }
                break;
            case "0":
                loop = 1;
                return;

        }
    }
}
void ActiveMenu (List<Event> activeEvents)
{
    var idList = new List<string>();
    foreach (var item in activeEvents)
    {
        ispis(item);
        idList.Add(item.Id.ToString());
    }
    
    var loop = 1;
    while (loop == 1)
    {
        Console.WriteLine("Upišite što želite napraviti");
        Console.WriteLine("1 - Zabilježite izostanak");
        Console.WriteLine("2 - Ispis");
        Console.WriteLine("0 - Main Menu");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.WriteLine("Upišite id eventa kojemu želite dodati izostanke sudionika");
                var inputId=Console.ReadLine();
                if (idList.Contains(inputId)==true)
                {
                    activeEvents[idList.IndexOf(inputId)].Absent();
                }
                else
                {
                    Console.WriteLine("Event ne postoji");
                }
                break;
            case "2":
                    foreach (var item in activeEvents)
                    {
                        ispis(item);
                    }
                    break;
            case "0":
                    loop = 1;
                    return;
                default: Console.WriteLine("Nije upisana valjana opcija"); break;

        }
    }
}
List <List<Event>> filter(List<Event> events)
{
    var prosli = new List<Event>();
    var trenutni = new List<Event>();
    var buduci = new List<Event>();
    foreach (var item in events)
    {
        if (DateTime.Now > item.EndDate)
        {
            prosli.Add(item);
        }
        else if(DateTime.Now>=item.StartDate && DateTime.Now <= item.EndDate)
        {
            trenutni.Add(item);
        }
        else
        {
            buduci.Add(item);
        }
    }
    var fullList= new List<List<Event>>() { prosli, trenutni, buduci };
    return fullList;
}
void ispis(Event događaj)
{
    Console.WriteLine("Event info");
    Console.WriteLine($"Id: {događaj.Id}");
    Console.WriteLine($"Ime eventa: {događaj.Name}");
    Console.WriteLine($"Lokacija eventa: {događaj.Loacation}");
    //Console.WriteLine($"Datum početka {događaj.StartDate}");
    //Console.WriteLine($"Datum kraja {događaj.EndDate}");

    if (DateTime.Now>=događaj.StartDate && DateTime.Now<događaj.EndDate) {
        Console.WriteLine($"Završava za {Math.Round((decimal)(događaj.EndDate - DateTime.Now).TotalHours, 1)} sati");
        var prisutni = new List<string>();
        var neprisutni = new List<string>();
        foreach (var item in događaj.actual_attendes)
        {
            if (item.osobe_dict[događaj.Id] == true)
            {
                prisutni.Add(item.Name + " " + item.Prezime + " " + item.email);
            }
            else
            {
                neprisutni.Add(item.Name + " " + item.Prezime + " " + item.email);
            }
        }
        Console.WriteLine("Prisutni su ");
        foreach (var item in prisutni)
        {
            Console.WriteLine(item);
        }
        if (prisutni.Count() == 0)
        {
            Console.WriteLine("Nema prisutnih sudionika");
        }
        Console.WriteLine("Neprisutni su ");
        foreach (var item in neprisutni)
        {
            Console.WriteLine(item);
        }
        if (neprisutni.Count() == 0)
        {
            Console.WriteLine("Nema neprisutnih sudionika");
        }
    }

    else {
        if (DateTime.Now > događaj.EndDate)
        {
            Console.WriteLine($"Završio prije {(int) (DateTime.Now-događaj.EndDate).TotalDays} dana");
            Console.WriteLine($"Trajao je {Math.Round((decimal)(događaj.EndDate - događaj.StartDate).TotalHours, 1)} sati");
            var prisutni = new List<string>();
            var neprisutni = new List<string>();
            foreach (var item in događaj.actual_attendes)
            {
                if (item.osobe_dict[događaj.Id] == true)
                {
                    prisutni.Add(item.Name + " " + item.Prezime + " " + item.email );
                }
                else
                {
                    neprisutni.Add(item.Name + " " + item.Prezime + " " + item.email);
                }
            }
            Console.WriteLine("Prisutni su bili");
            foreach (var item in prisutni)
            {
                Console.WriteLine(item);
            }
            if (prisutni.Count() == 0)
            {
                Console.WriteLine("Nema prisutnih sudionika");
            }
            Console.WriteLine("Neprisutni su bili");
            foreach (var item in neprisutni)
            {
                Console.WriteLine(item);
            }
            if (neprisutni.Count() == 0)
            {
                Console.WriteLine("Nema neprisutnih sudionika");
            }
        }
        else
        {
            Console.WriteLine($"Počinje za {(int)(događaj.StartDate - DateTime.Now).TotalDays} dana");
            Console.WriteLine($"Traje {Math.Round((decimal)(događaj.EndDate - događaj.StartDate).TotalHours, 1)} sata");
            foreach(var item in događaj.actual_attendes)
            {
                Console.WriteLine($"{item.Name} {item.Prezime} ({item.email}) je sudionik");
            }
        }
    }
    Console.WriteLine(" ");


}

var People = new List<Osoba>() {
    new Osoba("Jan", "Modun", "jan.modun.st@gmail.com"),
    new Osoba("Stipe", "Stišić", "stipe@gmail.com"),
    new Osoba("Marko", "Markić", "marko@gmail.com"),
    new Osoba("Admin", "Adminić", "admin@gmail.com"),
    new Osoba("Niko", "Nikić", "Nikić@gmail.com"),
    new Osoba("Jan", "Janić", "jan@gmail.com"),
    new Osoba("Patar", "Petrić", "Petar@gmail.com"),
    new Osoba("Stjepan", "Stjepanić", "Stjepan@gmail.com"),
    new Osoba("Frane", "Franić ", "Frane@gmail.com"),
    new Osoba("Luka", "Lukić", "Luka@dump.hr"),
};
Osoba FindPerson(string email, List<Osoba> possiblePeople)
{
    foreach (var item in possiblePeople)
    {
        if (item.email == email)
        {
            return item;
        }
    }
    var error = new Osoba("ERROR", "NOT FOUND", "ERROR@ERROR.com");
    Console.WriteLine($"Osoba {email} nije pronađena");
    return error;

}
var events = new List<Event>()
{
    new Event("Janov rođ", "Split", new DateTime(2006, 6, 28, 19, 00, 00), new DateTime(2006, 6, 28, 20, 00, 00), People, new List<Osoba>(){FindPerson("jan.modun.st@gmail.com", People)}),
    new Event("Janov 16 rođ", "Split", new DateTime(2022, 6, 28, 19, 00, 00), new DateTime(2022, 6, 28, 20, 00, 00), People, new List<Osoba>(){FindPerson("jan.modun.st@gmail.com", People) }),
    new Event("Dump 4. predavanje", "Fesb", new DateTime(2022, 11, 27, 13, 00, 00), new DateTime(2022, 11, 27, 16, 00, 00), People, new List<Osoba>{FindPerson("jan.modun.st@gmail.com", People), FindPerson("jan@gmail.com", People), FindPerson("admin@gmail.com", People), FindPerson("Luka@dump.hr", People)}),
    new Event("24.11", "Kuća", new DateTime(2022, 11, 24, 00, 00, 00), new DateTime(2022, 11, 25, 00, 00, 00), People, new List<Osoba>{FindPerson("jan.modun.st@gmail.com", People), FindPerson("Stjepan@gmail.com", People)}),
    new Event("Utakmica", "Mioc", new DateTime(2023, 11, 24, 13, 00, 00), new DateTime(2023, 11, 24, 16, 00, 00), People, new List<Osoba>{FindPerson("marko@gmail.com", People), FindPerson("Petar@gmail.com", People), FindPerson("stipe@gmail.com", People)})

};
bool checkAvability(Osoba person, DateTime startDate, DateTime endDate, List<Event> events)
{
    foreach (var item in person.osobe_dict)
    {
        var event1 = events.Find(i => i.Id == item.Key);
        if ((startDate>=event1.StartDate && startDate<=event1.EndDate) || (endDate >= event1.StartDate && endDate <= event1.EndDate) || (startDate<=event1.StartDate && endDate>=event1.EndDate) || startDate==event1.StartDate || endDate==event1.EndDate)
        {
            Console.WriteLine($"Nije moguće dodati osobu {person.Name + " " + person.Prezime}, preklapa se sa drugim eventima");
            return false;
        }

    }
    return true;
}
var loop = 1;
for(int i = 0; i < events.Count; i++)
{
    ispis(events[i]);
}
while (loop == 1)
{
    Console.WriteLine("1 - Aktivni eventi");
    Console.WriteLine("2 - Nadolazeći eventi");
    Console.WriteLine("3 - Završeni eventi");
    Console.WriteLine("4 - Kreiraj event");
    Console.WriteLine("0 - izađ iz apliakcije");
    var choice = Console.ReadLine();
    
    switch (choice)
    {

        case "4":
            var check = AddEvent(events, People);
            if (check == true)
            {
                Console.WriteLine("Uspješno dodan event");
            }
            else
            {
                Console.WriteLine("Nije uspjelo dodavanje eventa");
            }
            break;
        case "1":
            ActiveMenu(filter(events)[1]);
            break;
        case "2":
            FutureMenu(filter(events)[2], events);
            break;
        case "3":
            var write= filter(events)[0];
            foreach (var item in write)
            {
                ispis(item);
            }
            break;
        case "0":
            Environment.Exit(0);
            break;
        default: Console.WriteLine("Nije upisana valjana akcija"); break;
    }
}
bool AddEvent(List<Event> events, List<Osoba> people)
{
    Console.WriteLine("Upišite ime eventa");
    var name = Console.ReadLine();
    foreach (var item in events)
    {
        if (item.Name  == name)
        {
            Console.WriteLine("Invalid event ime");
            return false;
        }
    }
    Console.WriteLine("Upišite lokaciju događaja");
    var lokacija = Console.ReadLine();
    string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy",
                    "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy", "dd/MM/yyyy HH:mm:ss","dd/MM/yyyy HH:mm", "dd/M/yyyy HH:mm", "d/M/yyyy HH:mm", "d/MM/yyyy HH:mm",
                    "dd/MM/yy HH:mm", "dd/M/yy HH:mm", "d/M/yy HH:mm", "d/MM/yy HH:mm", "yyyy/MM H:mm"};
    Console.WriteLine("Upišite datum početka (dan/mjesec/godina sat:minuta)");
    var datumStart = DateTime.MinValue;
    var datumTry = Console.ReadLine();
    DateTime.TryParseExact(datumTry, formats,System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datumStart);
    Console.WriteLine(datumStart.ToString());
    if (datumStart == DateTime.MinValue || DateTime.Now>datumStart)
    {
        Console.WriteLine("Nije upisan pravilan datum pocetka");
        return false;
    }Console.WriteLine("Upišite datum kraja (dan/mjesec/godina sat:minuta)");
    var datumEnd = DateTime.MinValue;
    DateTime.TryParseExact(Console.ReadLine(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datumEnd);
    Console.WriteLine(datumEnd.ToString());
    if (datumEnd == DateTime.MinValue || datumEnd<=datumStart)
    {
        Console.WriteLine("Nije upisan pravilan datum kraja");
        return false;
    }
    Console.WriteLine("Upišite broj ljudi");
    var NumOfPeople = 0;
    int.TryParse(Console.ReadLine(), out NumOfPeople);
    if (NumOfPeople < 1)
    {
        Console.WriteLine("Invalid broj ljudi");
        return false;
    }
    var add = new List<Osoba>();
    for (var i=0; i<NumOfPeople; i++)
    {
        Console.WriteLine("Upišite mail osobe koja pristuje eventu");
        var email=Console.ReadLine();
        var check = 0;
        foreach(var item in people)
        {
            if (item.email == email)
            {
                if (checkAvability(item, datumStart, datumEnd, events) == true)
                {
                    Console.WriteLine($"Dodana osoba {item.Name + " " + item.Prezime}");
                    add.Add(item);
                    check++;
                }
                else
                {
                    return false;
                }

            }
        }
        if (check == 0)
        {
            Console.WriteLine("Nije pronađena ni jedna osoba s tim emailom");
            return false;
        }
    }
    var preset = new Event(name, lokacija, datumStart, datumEnd, people, add);
    Console.WriteLine("Informacije eventa");
    ispis(preset);
    var end = Dialog();
    if (end == true)
    {
        events.Add(preset);
        return true;
    }
    else
    {
        preset = null;
        return false;
    }

    //Pogle onima jkel im se krsi sa datetimeom
}

public class Osoba
{
    public string Name;
    public string Prezime { get; }
    public string email { get; }
    public Dictionary<Guid, bool> osobe_dict { get; private set; }
    public Osoba(string personName,string personSurname, string personEmail)
    {
        Name = personName;
        Prezime = personSurname;
        email=personEmail;
        osobe_dict = new Dictionary<Guid, bool>();
    }
}
public class Event
{
    public Guid Id;
    public string Name;
    public string Loacation;
    public DateTime StartDate { get; set;  }
    public DateTime EndDate { get; set; }
    public List<Osoba> attendes { get; private set; }
    public List<Osoba> actual_attendes { get; private set; }
    public Event(string ime, string lokacija, DateTime datumStart, DateTime datumEnd, List<Osoba> zvanici, List<Osoba> ljudi)
    {
        Id = Guid.NewGuid();
        Name = ime;
        Loacation = lokacija;
        StartDate = datumStart;
        EndDate = datumEnd;
        attendes = zvanici;
        actual_attendes = ljudi;
        foreach(var item in actual_attendes)
        {
            item.osobe_dict.Add(Id, true);
        }
    }
    public void SetPoeple()
    {
        var NumOfPeople = 0;

        int.TryParse(Console.ReadLine(), out NumOfPeople);
        for (int i = 0; i < NumOfPeople; i++)
        {
            Console.WriteLine("Upišite ime osobe za zvati na event");
            var newPerson = Console.ReadLine();
            var check = 0;
            foreach (var item in attendes)
            {
                if (item.email == newPerson)
                {
                    Console.WriteLine($"Osoba {item.Name + " " + item.Prezime} dodana");
                    item.osobe_dict.Add(Id, true);
                    actual_attendes.Add(item);
                    check = 1;
                    break;

                }
            }
            if (check == 0)
            {
                Console.WriteLine("Osoba nije pronađena");
            }
        }

    }
    public void RemoveAttendee()
    {
        Console.WriteLine("Upišite mail osobe koju želite maknuti");
        var mail = Console.ReadLine();
        foreach (var item in actual_attendes)
        {
            if (item.email == mail)
            {
                Console.WriteLine("Ova akcija trrajno mijenja podatke aplikacije, želite li je napraviti");
                Console.WriteLine("1-DA" + "\n" + "0-Ne");
                var final = Console.ReadLine();
                if (final == "1")
                {
                    item.osobe_dict.Remove(Id);
                    actual_attendes.Remove(item);

                    Console.WriteLine($"Osoba {item.Name + " " + item.Prezime} maknuta");
                    return;
                }
                else
                {
                    return;
                }
                
            }
        }
        Console.WriteLine("Osoba nije pronađena");
    }
    public void Absent()
    {
        Console.WriteLine("Upišite koliko je ljudi neprisutno");
        var NumOfAbsent = 0;
        if (NumOfAbsent>actual_attendes.Count() || NumOfAbsent < 1)
        {
            Console.WriteLine("Nije upisan valjani broj ljudi");
            return;
        }
        int.TryParse(Console.ReadLine(), out NumOfAbsent);
        for (int i = 0; i < NumOfAbsent; i++)
        {
            Console.WriteLine("Upišite email neprisutne osobe");
            var AbsentPerson = Console.ReadLine();
            var check = 0;
            foreach (var item in actual_attendes)
            {
                if (item.email == AbsentPerson)
                {
                    Console.WriteLine("Ova akcija trrajno mijenja podatke aplikacije, želite li je napraviti");
                    Console.WriteLine("1-DA" + "\n" + "0-Ne");
                    var final = Console.ReadLine();
                    if (final == "1")
                    {
                        item.osobe_dict[Id] = false;
                        check= 1;    
                        Console.WriteLine($"Osoba {item.Name + " " + item.Prezime} je uklonjena");
                    }
                    else
                    {
                        return;
                    }
                    }
                break;
            }
            if (check == 0)
            {
                Console.WriteLine("Osoba nije pronađena");
                return;
            }
        }
    }
}