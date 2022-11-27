// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Channels;
using System.Xml.Serialization;
//To do: add comments and console.clear()
bool DialogConfirmation()
{
    Console.WriteLine("Ova akcija trajno mijenja podatke aplikacije, želite li je napraviti");
    Console.WriteLine("1-Da" + "\n" + "0-Ne");
    var decsion=Console.ReadLine();
    return (decsion == "1");
}
void FutureEventsMenu (List<Event> futureEvents, List<Event> events)//Meni za izabiranje akcija za buduće eventove
{
    
    var idList = new List<string>();
    foreach (var item in futureEvents)
    {
        PrintEvent(item);
        idList.Add(item.Id.ToString());
    }

    var loop = 1;
    while (loop == 1)//Trajna for petlja (dok sami ne izađemo)
    {
        Console.Clear();
        Console.WriteLine("Upišite što želite napraviti");
        Console.WriteLine("1 - Uklonite event");
        Console.WriteLine("2 - Uklonite sudionika");
        Console.WriteLine("3 - Ispis");
        Console.WriteLine("0 - Main Menu");
        var choice = Console.ReadLine();
        Console.Clear();
        switch (choice)
        {
            case "2":

                Console.WriteLine("Upišite id eventa kojemu želite maknuti sudionika");
                var idToRemove = Console.ReadLine();
                if (idList.Contains(idToRemove) == true)//Provjera postoji li event gdje želimo maknuti korisnika
                {
                    Console.WriteLine(futureEvents[idList.IndexOf(idToRemove)].Name + " je event sa kojega želite maknuti sudionika");
                    futureEvents[idList.IndexOf(idToRemove)].RemoveAttendee();
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Event ne postoji");
                }
                break;
            case "3":
                foreach (var item in futureEvents)
                {
                    PrintEvent(item);//ispis
                }
                Console.ReadLine();
                break;
            case "1":
                Console.WriteLine("Upišite id eventa kojeg bi uklonili");
                var idRemove = Console.ReadLine();
                if (idList.Contains(idRemove) == true)
                {
                    var EventToDelete = events.Find(i => i.Id == futureEvents[idList.IndexOf(idRemove)].Id);
                    Console.WriteLine(EventToDelete.Name +" je event kojeg želite izbrisati");
                    var confirmDewletion =DialogConfirmation();
                    if (confirmDewletion == true)
                    {
                        foreach (var item in EventToDelete.actual_attendes)
                        {
                            item.osobe_dict.Remove(EventToDelete.Id);//Briasnje eventa iz rječnika osoba
                        }
                        events.Remove(events.Find(i => i.Id == futureEvents[idList.IndexOf(idRemove)].Id));//traženje evneta preko find funkcije
                        futureEvents.RemoveAt(idList.IndexOf(idRemove));//Mićemo budući event
                        Console.WriteLine("Izbrisano");
                        idList.Remove(idRemove);
                    }                    
                    break;
                }
                else
                {
                    Console.WriteLine("Event ne postoji");
                    Console.ReadLine();
                }
                break;
            case "0":
                loop = 0;
                Console.Clear();
                return;

        }
    }
}
void ActiveEventsMenu (List<Event> activeEvents)
{
    var idList = new List<string>();
    foreach (var item in activeEvents)//ista procedura kao i gore, dodajemo listu idova za lakše traženje
    {
        PrintEvent(item);
        idList.Add(item.Id.ToString()); 
    }
    
    var loop = 1;
    while (loop == 1)
    {
        //izbornik
        Console.Clear();
        Console.WriteLine("Upišite što želite napraviti");
        Console.WriteLine("1 - Zabilježite izostanak");
        Console.WriteLine("2 - Ispis");
        Console.WriteLine("0 - Main Menu");
        var choice = Console.ReadLine();
        Console.Clear ();
        switch (choice)
        {
            case "1":
                Console.WriteLine("Upišite id eventa kojemu želite dodati izostanke sudionika");
                var inputId=Console.ReadLine();
                if (idList.Contains(inputId)==true)
                {
                    Console.WriteLine("Event kojega mijenjate "+activeEvents[idList.IndexOf(inputId)].Name);
                    activeEvents[idList.IndexOf(inputId)].Absent();
                }
                else
                {
                    Console.WriteLine("Event ne postoji");
                    Console.ReadLine();
                }
                break;
            case "2":
                    foreach (var item in activeEvents)
                    {
                        PrintEvent(item);
                }
                Console.ReadLine();
                break;
            case "0":
                    loop = 1;
                    return;
                default: Console.WriteLine("Nije upisana valjana opcija"); Console.ReadLine(); break;

        }
    }
}
List <List<Event>> FilterOfEvents(List<Event> events)
{
    var pastEvents = new List<Event>();
    var activeEvents = new List<Event>();
    var futureEvents = new List<Event>();
    foreach (var item in events)
    {
        if (DateTime.Now > item.EndDate)
        {
            pastEvents.Add(item);
        }
        else if(DateTime.Now>=item.StartDate && DateTime.Now <= item.EndDate)
        {
            activeEvents.Add(item);
        }
        else
        {
            futureEvents.Add(item);
        }
    }
    var fullList= new List<List<Event>>() { pastEvents, activeEvents, futureEvents };//lista eventova
    return fullList;
}
void PrintEvent(Event eventToPrint)
{
    Console.WriteLine("Event info");
    Console.WriteLine($"Id: {eventToPrint.Id}");
    Console.WriteLine($"Ime eventa: {eventToPrint.Name}");
    Console.WriteLine($"Lokacija eventa: {eventToPrint.Loacation}");
    if (DateTime.Now>=eventToPrint.StartDate && DateTime.Now<eventToPrint.EndDate) {//Ispis drukciji ovisno o starosti eventa
        Console.WriteLine($"Završava za {Math.Round((decimal)(eventToPrint.EndDate - DateTime.Now).TotalHours, 1)} sati");
        var here = new List<string>();
        var absent = new List<string>();
        foreach (var item in eventToPrint.actual_attendes)//Traženje prisutnih i neprisutnih
        {
            if (item.osobe_dict[eventToPrint.Id] == true)
            {
                here.Add(item.Name + " " + item.Surname + " " + item.email);
            }
            else
            {
                absent.Add(item.Name + " " + item.Surname + " " + item.email);
            }
        }
        Console.WriteLine("Prisutni su ");
        Console.WriteLine(string.Join(",", here));
        //Ispis prisutnih
        if (here.Count() == 0)
        {
            Console.WriteLine("Nitko nije prisutan");
        }
        Console.WriteLine("Neprisutni su ");
        Console.WriteLine(string.Join(",", absent));

        if (absent.Count() == 0)
        {
            Console.WriteLine("Nema neprisutnih sudionika");
        }
    }

    else {
        if (DateTime.Now > eventToPrint.EndDate)//Slučaj da je bio prije
        {
            Console.WriteLine($"Završio prije {(int) (DateTime.Now-eventToPrint.EndDate).TotalDays} dana");
            Console.WriteLine($"Trajao je {Math.Round((decimal)(eventToPrint.EndDate - eventToPrint.StartDate).TotalHours, 1)} sati");
            var here = new List<string>();//Opet prisutni i neprisutni
            var absent = new List<string>();
            foreach (var item in eventToPrint.actual_attendes)//Ovo radim preko liste koju kasnije ispisujem, možda nije najefikasniji način ali čini se da radi
            {
                if (item.osobe_dict[eventToPrint.Id] == true)
                {
                    here.Add(item.Name + " " + item.Surname + " " + item.email );
                }
                else
                {
                    absent.Add(item.Name + " " + item.Surname + " " + item.email);
                }
            }
            Console.WriteLine("Prisutni su bili");
            Console.WriteLine(string.Join(",", here));
            if (here.Count() == 0)
            {
                Console.WriteLine("Nema prisutnih sudionika");
            }
            Console.WriteLine("Neprisutni su bili");
            Console.WriteLine(string.Join(",", absent));
            if (absent.Count() == 0)
            {
                Console.WriteLine("Nema neprisutnih sudionika");
            }
        }
        else//Budući eventToPrint
        {
            Console.WriteLine($"Počinje za {(int)(eventToPrint.StartDate - DateTime.Now).TotalDays} dana");
            Console.WriteLine($"Traje {Math.Round((decimal)(eventToPrint.EndDate - eventToPrint.StartDate).TotalHours, 1)} sata");
            var output = new List<string>();
            foreach (var item in eventToPrint.actual_attendes)
            {
                output.Add($"{item.Name} {item.Surname} ({item.email}) je sudionik");
            }
            Console.WriteLine(string.Join(",", output));
        }
    }
    Console.WriteLine(" ");


}

var People = new List<Osoba>() {//Lista defultnih osoba,nisu najkreatinija imena
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
Osoba FindPersonByMail(string email, List<Osoba> possiblePeople)//Traženje osoba preko emaila
{
    foreach (var item in possiblePeople)
    {
        if (item.email == email)
        {
            return item;
        }
    }
    var error = new Osoba("ERROR", "NOT FOUND", "ERROR@ERROR.com");//Vraća error osobu ako nije pronađen mail, ne koristim nigdi u programu ali bi teoretski ovo moglo poslužiti za pronaći grešku
    Console.WriteLine($"Osoba {email} nije pronađena");
    return error;

}
var events = new List<Event>() //Lista  defultnih eventova 
{
    new Event("Janov rođ", "Split", new DateTime(2006, 6, 28, 19, 00, 00), new DateTime(2006, 6, 28, 20, 00, 00), new List<Osoba>(){FindPersonByMail("jan.modun.st@gmail.com", People)}),
    new Event("Janov 16 rođ", "Split", new DateTime(2022, 6, 28, 19, 00, 00), new DateTime(2022, 6, 28, 20, 00, 00), new List<Osoba>(){FindPersonByMail("jan.modun.st@gmail.com", People) }),
    new Event("Dump 4. predavanje", "Fesb", new DateTime(2022, 11, 27, 13, 00, 00), new DateTime(2022, 11, 27, 16, 00, 00), new List<Osoba>{FindPersonByMail("jan.modun.st@gmail.com", People), FindPersonByMail("jan@gmail.com", People), FindPersonByMail("admin@gmail.com", People), FindPersonByMail("Luka@dump.hr", People)}),
    new Event("11 mjesec", "Kuća", new DateTime(2022, 11, 1, 00, 00, 00), new DateTime(2022, 12, 1, 00, 00, 00), new List<Osoba>{FindPersonByMail("jan.modun.st@gmail.com", People), FindPersonByMail("Stjepan@gmail.com", People)}),
    new Event("Utakmica", "Mioc", new DateTime(2023, 11, 24, 13, 00, 00), new DateTime(2023, 11, 24, 16, 00, 00), new List<Osoba>{FindPersonByMail("marko@gmail.com", People), FindPersonByMail("Petar@gmail.com", People), FindPersonByMail("stipe@gmail.com", People)}),
    new Event("2022 godina", "Svijet", new DateTime(2022, 1, 1, 13, 00, 00), new DateTime(2023, 1, 1, 16, 00, 00), new List<Osoba>{FindPersonByMail("marko@gmail.com", People), FindPersonByMail("Petar@gmail.com", People), FindPersonByMail("stipe@gmail.com", People)})

};
bool checkAvability(Osoba person, DateTime startDate, DateTime endDate, List<Event> events)//Provjerava može li se event doadti s obzirom na druge evewntove koje suidonik pohađa
{
    foreach (var item in person.osobe_dict)
    {
        var event1 = events.Find(i => i.Id == item.Key);
        if ((startDate>=event1.StartDate && startDate<=event1.EndDate) || (endDate >= event1.StartDate && endDate <= event1.EndDate) || (startDate<=event1.StartDate && endDate>=event1.EndDate) || startDate==event1.StartDate || endDate==event1.EndDate)
        {
            Console.WriteLine($"Nije moguće dodati osobu {person.Name + " " + person.Surname}, preklapa se sa drugim eventima");
            return false;
        }

    }
    return true;
}
var loop = 1;
while (loop == 1)//loop izbora
{//Izbornik
    Console.Clear();
    Console.WriteLine("1 - Aktivni eventi");
    Console.WriteLine("2 - Nadolazeći eventi");
    Console.WriteLine("3 - Završeni eventi");
    Console.WriteLine("4 - Kreiraj event");
    Console.WriteLine("0 - izađ iz apliakcije");
    var choice = Console.ReadLine();
    Console.Clear();
    switch (choice)
    {

        case "4":
            var check = AddEvent(events, People);//Pozivanje funkcije za dodavanje event i ovisno o supejsnosti ispis
            if (check == true)
            {
                Console.WriteLine("Uspješno dodan event");
            }
            else
            {
                Console.WriteLine("Nije uspjelo dodavanje eventa");
            }
            Console.ReadLine();
            break;
        case "1":
            ActiveEventsMenu(FilterOfEvents(events)[1]);
            break;
        case "2":
            FutureEventsMenu(FilterOfEvents(events)[2], events);
            break;
        case "3":
            var write= FilterOfEvents(events)[0];//Pošto su ovo stari eventovi, nije potrebno zvati drugu funkcju i nema nikakvih opcija sa stairm eventovima
            foreach (var item in write)
            {
                PrintEvent(item);
            }
            Console.ReadLine();
            break;
        case "0":
            Environment.Exit(0);//Izlaz iz aplikacije
            break;
        default: Console.WriteLine("Nije upisana valjana akcija"); Console.ReadLine(); break;
    } 
}
bool AddEvent(List<Event> events, List<Osoba> people)//Funkacija za dodavanje eventa
{
    Console.Clear();
    Console.WriteLine("Upišite ime eventa");
    var name = Console.ReadLine();
    foreach (var item in events)
    {
        if (item.Name  == name)//provjerava potoji li event već istoga imena, ne znam smije li se to ali bi definitivno prouzrokovalo probleme pa neću sada dopustiti
        {
            Console.WriteLine("Invalid event ime");
            return false;
        }
    }
    Console.WriteLine("Upišite lokaciju događaja");
    var location = Console.ReadLine();
    string[] formats = { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy",
                    "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy", "dd/MM/yyyy HH:mm:ss","dd/MM/yyyy HH:mm", "dd/M/yyyy HH:mm", "d/M/yyyy HH:mm", "d/MM/yyyy HH:mm",
                    "dd/MM/yy HH:mm", "dd/M/yy HH:mm", "d/M/yy HH:mm", "d/MM/yy HH:mm", "yyyy/MM/dd H:mm"}; // razni dopušteni formsti unosa eventa., lakše bi bilo da se korinsik držoi zadanoga ali je moguće ovdje izbjkeći grešku
    Console.WriteLine("Upišite datum početka (dan/mjesec/godina sat:minuta)");
    var datumStart = DateTime.MinValue;//Minvalue kao defultni value akop konverzija nije uspjela
    var datumTry = Console.ReadLine();
    DateTime.TryParseExact(datumTry, formats,System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datumStart);//KOnverija upisanoga datuma u datetime
    if (datumStart == DateTime.MinValue || DateTime.Now>datumStart)//Provjera jeli +konverzija uspjela
    {
        Console.WriteLine("Nije upisan pravilan datum pocetka");
        return false;
    }Console.WriteLine("Upišite datum kraja (dan/mjesec/godina sat:minuta)");
    var datumEnd = DateTime.MinValue;//Ista stvar za krajni datum
    DateTime.TryParseExact(Console.ReadLine(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datumEnd);
    if (datumEnd == DateTime.MinValue || datumEnd<=datumStart)
    {
        Console.WriteLine("Nije upisan pravilan datum kraja");
        return false;
    }
    //INput sudionika
    Console.WriteLine("Upišite sudionike");
    string[] PeopleToAdd = Console.ReadLine().Split(",");
    var NumOfPeople = PeopleToAdd.Length;
    if (NumOfPeople < 1)
    {
        Console.WriteLine("Invalid broj ljudi");
        return false;
    }
    var add = new List<Osoba>();
    for (var i = 0; i < NumOfPeople; i++)//dodavanje sudionika
    {
        var email = PeopleToAdd[i];
        var check = 0;
        foreach (var item in people)
        {
            if (item.email == email)
            {
                if (checkAvability(item, datumStart, datumEnd, events) == true)//Provjera dostupnosti
                {
                    Console.WriteLine($"Dodana osoba {item.Name + " " + item.Surname}");
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
            //Akop nije pronađena osoba s tim emailom, onda izlazi iz funkcije
        }
    }
    Console.Clear();
    var preset = new Event(name, location, datumStart, datumEnd, add);//Stvaranje preseta eventa
    //Ispis infomraicja o eventu
    Console.WriteLine("Informacije eventa");
    PrintEvent(preset);
    var confirmAdd = DialogConfirmation();//korinsik ušisuje jeli zadovoljan sa eventom
    if (confirmAdd == true)
    {
        events.Add(preset);//Dodavanje eventa u listu
        return true;
    }
    else
    {
        foreach (var item in add)
        {
            item.osobe_dict.Remove(preset.Id);//Brisanje nepostojećeg eventa iz liste osoba
        }
        preset = null;
        return false;
    }

}

public class Osoba//KLasa osoba
{
    public string Name;//Ime, promjenjivo svugdje
    public string Surname { get; }//Prezime, koje se ne mijenja nakon konstrukcije
    public string email { get; }//Email osobe, nešto kao id osobe zapravo, može ga sae pristupiti svugdje ali ne i mijeanjati
    public Dictionary<Guid, bool> osobe_dict { get; private set; }//Rječnik prisutnosti
    public Osoba(string personName,string personSurname, string personEmail)
    {
        Name = personName;
        Surname = personSurname;
        email=personEmail;
        osobe_dict = new Dictionary<Guid, bool>();
    }
}
public class Event//Klasa event
{
    public Guid Id { get; }//Id kojem možemo prisutpiti bilo kada ali ga ne možemo mijenjati
    public string Name;//Ime i lokacija oboje promjenjivi stringovi
    public string Loacation;
    public DateTime StartDate { get;  } //Datum početka i kraja, ne možemo ih mijanjati ali možemo ih gledati
    public DateTime EndDate { get; }
    public List<Osoba> actual_attendes { get; private set; }//Lista sudionika
    public Event(string ime, string lokacija, DateTime datumStart, DateTime datumEnd, List<Osoba> ljudi) //Konstruktor
    {
        Id = Guid.NewGuid();
        Name = ime;
        Loacation = lokacija;
        StartDate = datumStart;
        EndDate = datumEnd;
        actual_attendes = ljudi;
        foreach(var item in actual_attendes)//doadavanje eventa u prisutnosti osobe
        {
            item.osobe_dict.Add(Id, true);
        }
    }
    public void RemoveAttendee()//Funkcija za uklanjanje sudionika, uklanjanje samo 1 sudionika, ne liste njih
    {
        Console.WriteLine("Upišite mail osoba koje želite maknuti");
        string[] mail = Console.ReadLine().Split(",");
        int NumOfDelete = mail.Length;
        Console.WriteLine("Ova akcija trrajno mijenja podatke aplikacije, želite li je napraviti");
        Console.WriteLine("1-DA" + "\n" + "0-Ne");
        var final = Console.ReadLine();
        if (final == "1")
        {
           for (int i = 0; i < NumOfDelete; i++)
            {
                var check = 0;

                foreach (var item in actual_attendes)//Traženje preko maila
                {
                    if (item.email == mail[i])
                    {
                        item.osobe_dict.Remove(Id);
                        actual_attendes.Remove(item);
                        //Micanje sudionika sa liste sudionika iu eventa sda rječnika prisutnosti sudionika
                        Console.WriteLine($"Osoba {item.Name + " " + item.Surname} maknuta");
                        check = 1; 
                        break;
                        //nisam želio ovdje stavljat funkciju za dialog pa sam samo copy pasta


                    }
                }
                if (check==0)
                    Console.WriteLine("Osoba nije pronađena");
            }
        }
        else
        {   
            return;
        }
        
    }
    public void Absent()//Funkciaj za bilježenje izostanaka
    {
        Console.WriteLine("Upišite mailove neprisutnih");
        string [] absence = Console.ReadLine().Split(",");
        int NumOfAbsent = absence.Length;

        if (NumOfAbsent>actual_attendes.Count() || NumOfAbsent < 1)
        {
            Console.WriteLine("Nije upisan valjani broj ljudi");
            return;
        }
        Console.WriteLine("Ova akcija trajno mijenja podatke aplikacije, želite li je napraviti");
        Console.WriteLine("1-Da" + "\n" + "0-Ne");
        var confirmationDialog = Console.ReadLine();
        if (confirmationDialog == "1") {
            for (int i = 0; i < NumOfAbsent; i++)
            {
                var check = 0;
                var AbsentPerson = absence[i];

                foreach (var item in actual_attendes)
                {
                    if (item.email == AbsentPerson)//Traženje preko maila
                    {

                            item.osobe_dict[Id] = false;//Promjena vrijednosti prisutnosti
                            check = 1;
                            Console.WriteLine($"Osoba {item.Name + " " + item.Surname} je zapisana kao neprisutna");
                            break;
                    
                    }
                }
                if (check == 0)
                {
                    Console.WriteLine("Osoba nije pronađena");
                    return;
                }
            }
        }
        else
        {
            return;
        }
    }
}