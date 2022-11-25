// See https://aka.ms/new-console-template for more information
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Xml.Serialization;
//To do: add comments and console.clear()
bool Dialog()//Dialog window
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
void FutureMenu (List<Event> futureEvents, List<Event> events)//Meni za izabiranje akcija za buduće eventove
{
    var idList = new List<string>();
    foreach (var item in futureEvents)
    {
        ispis(item);
        idList.Add(item.Id.ToString());
    }

    var loop = 1;
    while (loop == 1)//Trajna for petlja (dok sami ne izađemo)
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
                if (idList.Contains(idToRemove) == true)//Provjera postoji li event gdje želimo maknuti korisnika
                {
                    Console.WriteLine(futureEvents[idList.IndexOf(idToRemove)].Name + "je evnet sa kojega želite maknuti sudionika");
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
                    ispis(item);//ispis
                }
                break;
            case "1":
                Console.WriteLine("Upišite id eventa kojeg i uklonili");
                var idRemove = Console.ReadLine();
                if (idList.Contains(idRemove) == true)
                {
                    var EventToDelete = events.Find(i => i.Id == futureEvents[idList.IndexOf(idRemove)].Id);
                    Console.WriteLine(EventToDelete.Name +" je evnt kojeg želite izbrisati");
                    var confirmDewletion =Dialog();
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
                }
                break;
            case "0":
                loop = 1;
                return;

        }
    }
}
void ActiveMenu (List<Event> activeEvents)//Aktivni menu
{
    var idList = new List<string>();
    foreach (var item in activeEvents)//ista procedura kao i groe, dodajemo listu idova za lakše traženje
    {
        ispis(item);
        idList.Add(item.Id.ToString());
    }
    
    var loop = 1;
    while (loop == 1)
    {
        //izbornik
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
                    Console.WriteLine("Event kojega mijenjate "+activeEvents[idList.IndexOf(inputId)].Name);
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
List <List<Event>> filter(List<Event> events)//filter preko kojega dobijamo buduće, prošle i sadašnje evnetove i listi
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
    var fullList= new List<List<Event>>() { prosli, trenutni, buduci };//lista eventova
    return fullList;
}
//funmkcaij za ispis podataka događaja
void ispis(Event događaj)
{
    Console.WriteLine("Event info");
    Console.WriteLine($"Id: {događaj.Id}");
    Console.WriteLine($"Ime eventa: {događaj.Name}");
    Console.WriteLine($"Lokacija eventa: {događaj.Loacation}");
    //Console.WriteLine($"Datum početka {događaj.StartDate}");
    //Console.WriteLine($"Datum kraja {događaj.EndDate}");

    if (DateTime.Now>=događaj.StartDate && DateTime.Now<događaj.EndDate) {//Ispis drukciji ovisno o starosti eventa
        Console.WriteLine($"Završava za {Math.Round((decimal)(događaj.EndDate - DateTime.Now).TotalHours, 1)} sati");
        var prisutni = new List<string>();
        var neprisutni = new List<string>();
        foreach (var item in događaj.actual_attendes)//Traženje prisutnih i neprisutnih
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
        Console.WriteLine(string.Join(",", prisutni));
        //Ispis prisutnih
        if (prisutni.Count() == 0)
        {
            Console.WriteLine("Nitko nije prisutan");
        }
        Console.WriteLine("Neprisutni su ");
        Console.WriteLine(string.Join(",", neprisutni));

        if (neprisutni.Count() == 0)
        {
            Console.WriteLine("Nema neprisutnih sudionika");
        }
    }

    else {
        if (DateTime.Now > događaj.EndDate)//Slučaj da je bio prije
        {
            Console.WriteLine($"Završio prije {(int) (DateTime.Now-događaj.EndDate).TotalDays} dana");
            Console.WriteLine($"Trajao je {Math.Round((decimal)(događaj.EndDate - događaj.StartDate).TotalHours, 1)} sati");
            var prisutni = new List<string>();//Opet prisutni i neprisutni
            var neprisutni = new List<string>();
            foreach (var item in događaj.actual_attendes)//Ovo radim preko liste koju kasnije ispisujem, možda nije najefikasniji način ali čini se da radi
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
            Console.WriteLine(string.Join(",", prisutni));
            if (prisutni.Count() == 0)
            {
                Console.WriteLine("Nema prisutnih sudionika");
            }
            Console.WriteLine("Neprisutni su bili");
            Console.WriteLine(string.Join(",", neprisutni));
            if (neprisutni.Count() == 0)
            {
                Console.WriteLine("Nema neprisutnih sudionika");
            }
        }
        else//Budući događaj
        {
            Console.WriteLine($"Počinje za {(int)(događaj.StartDate - DateTime.Now).TotalDays} dana");
            Console.WriteLine($"Traje {Math.Round((decimal)(događaj.EndDate - događaj.StartDate).TotalHours, 1)} sata");
            var output = new List<string>();
            foreach (var item in događaj.actual_attendes)
            {
                output.Add($"{item.Name} {item.Prezime} ({item.email}) je sudionik");
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
Osoba FindPerson(string email, List<Osoba> possiblePeople)//Traženje osoba preko emaila
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
    new Event("Janov rođ", "Split", new DateTime(2006, 6, 28, 19, 00, 00), new DateTime(2006, 6, 28, 20, 00, 00), People, new List<Osoba>(){FindPerson("jan.modun.st@gmail.com", People)}),
    new Event("Janov 16 rođ", "Split", new DateTime(2022, 6, 28, 19, 00, 00), new DateTime(2022, 6, 28, 20, 00, 00), People, new List<Osoba>(){FindPerson("jan.modun.st@gmail.com", People) }),
    new Event("Dump 4. predavanje", "Fesb", new DateTime(2022, 11, 27, 13, 00, 00), new DateTime(2022, 11, 27, 16, 00, 00), People, new List<Osoba>{FindPerson("jan.modun.st@gmail.com", People), FindPerson("jan@gmail.com", People), FindPerson("admin@gmail.com", People), FindPerson("Luka@dump.hr", People)}),
    new Event("25.11", "Kuća", new DateTime(2022, 11, 25, 00, 00, 00), new DateTime(2022, 11, 26, 00, 00, 00), People, new List<Osoba>{FindPerson("jan.modun.st@gmail.com", People), FindPerson("Stjepan@gmail.com", People)}),
    new Event("Utakmica", "Mioc", new DateTime(2023, 11, 24, 13, 00, 00), new DateTime(2023, 11, 24, 16, 00, 00), People, new List<Osoba>{FindPerson("marko@gmail.com", People), FindPerson("Petar@gmail.com", People), FindPerson("stipe@gmail.com", People)})

};
bool checkAvability(Osoba person, DateTime startDate, DateTime endDate, List<Event> events)//Provjerava može li se event doadti s obzirom na druge evewntove koje suidonik pohađa
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
/*for(int i = 0; i < events.Count; i++)
{
   ispis(events[i]);
}
*/
while (loop == 1)//loop izbora
{//Izbornik
    Console.WriteLine("1 - Aktivni eventi");
    Console.WriteLine("2 - Nadolazeći eventi");
    Console.WriteLine("3 - Završeni eventi");
    Console.WriteLine("4 - Kreiraj event");
    Console.WriteLine("0 - izađ iz apliakcije");
    var choice = Console.ReadLine();
    
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
            break;
        case "1":
            ActiveMenu(filter(events)[1]);
            break;
        case "2":
            FutureMenu(filter(events)[2], events);
            break;
        case "3":
            var write= filter(events)[0];//Pošto su ovo stari eventovi, nije potrebno zvati drugu funkcju i nema nikakvih opcija sa stairm eventovima
            foreach (var item in write)
            {
                ispis(item);
            }
            break;
        case "0":
            Environment.Exit(0);//Izlaz iz aplikacije
            break;
        default: Console.WriteLine("Nije upisana valjana akcija"); break;
    }
}
bool AddEvent(List<Event> events, List<Osoba> people)//Funkacija za dodavanje eventa
{
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
    var lokacija = Console.ReadLine();
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
    for (var i=0; i<NumOfPeople; i++)//dodavanje sudionika
    {
        var email = PeopleToAdd[i];
        var check = 0;
        foreach(var item in people)
        {
            if (item.email == email)
            {
                if (checkAvability(item, datumStart, datumEnd, events) == true)//Provjera dostupnosti
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
            //Akop nije pronađena osoba s tim emailom, onda izlazi iz funkcije
        }
    }
    var preset = new Event(name, lokacija, datumStart, datumEnd, people, add);//Stvaranje preseta eventa
    //Ispis infomraicja o eventu
    Console.WriteLine("Informacije eventa");
    ispis(preset);
    var end = Dialog();//korinsik ušisuje jeli zadovoljan sa eventom
    if (end == true)
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
    public string Prezime { get; }//Prezime, koje se ne mijenja nakon konstrukcije
    public string email { get; }//Email osobe, nešto kao id osobe zapravo, može ga sae pristupiti svugdje ali ne i mijeanjati
    public Dictionary<Guid, bool> osobe_dict { get; private set; }//Rječnik priusutnosti
    public Osoba(string personName,string personSurname, string personEmail)//Funkcija za novu osobu
    {
        Name = personName;
        Prezime = personSurname;
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
    public List<Osoba> attendes { get; private set; }//Lista osoba, vidit mogu li mknuti 
    public List<Osoba> actual_attendes { get; private set; }//Lista sudionika
    public Event(string ime, string lokacija, DateTime datumStart, DateTime datumEnd, List<Osoba> zvanici, List<Osoba> ljudi) //Konstruktor
    {
        Id = Guid.NewGuid();
        Name = ime;
        Loacation = lokacija;
        StartDate = datumStart;
        EndDate = datumEnd;
        attendes = zvanici;
        actual_attendes = ljudi;
        foreach(var item in actual_attendes)//doadavanje eventa u prisutnosti osobe
        {
            item.osobe_dict.Add(Id, true);
        }
    }
    public void SetPoeple()//Ova se funkcija zapravo nikad ne koristi u programu pošto nikad ne moramo unijesti nove sudionike
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
                        Console.WriteLine($"Osoba {item.Name + " " + item.Prezime} maknuta");
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
        Console.WriteLine("Ova akcija trrajno mijenja podatke aplikacije, želite li je napraviti");
        Console.WriteLine("1-DA" + "\n" + "0-Ne");
        var final = Console.ReadLine();
        if (final == "1") {
            for (int i = 0; i < NumOfAbsent; i++)
            {
                /*Console.WriteLine("Upišite email neprisut osobe");
                var AbsentPerson = Console.ReadLine();
                var check = 0;
                */
                var check = 0;
                var AbsentPerson = absence[i];

                foreach (var item in actual_attendes)
                {
                    if (item.email == AbsentPerson)//Traženje preko maila
                    {

                            item.osobe_dict[Id] = false;//Promjena vrijednosti prisutnosti
                            check = 1;
                            Console.WriteLine($"Osoba {item.Name + " " + item.Prezime} je uklonjena");
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