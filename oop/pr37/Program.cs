var postings = new GenericRepository<Posting>();

postings.Add(new Posting {
    Id = 1,
    TrackingNumber = "UA001",
    Weight = 2,
    From = "Київ",
    To = "Львів",
    Status = "Прийнято"
});
postings.Add(new Posting {
    Id = 2,
    TrackingNumber = "UA002",
    Weight = 3,
    From = "Рівне",
    To = "Луцьк",
    Status = "У дорозі"
});
postings.Add(new Posting {
    Id = 3,
    TrackingNumber = "UA003",
    Weight = 1,
    From = "Одеса",
    To = "Київ",
    Status = "Прийнято"
});

Console.WriteLine($"Знайдено відправлення: {postings.GetById(1)}");

postings.Update(new Posting {
    Id = 1,
    TrackingNumber = "UA001",
    Weight = 2,
    From = "Київ",
    To = "Львів",
    Status = "Доставлено"
});
postings.Delete(2);
foreach (var item in postings.GetAll()) Console.WriteLine(item);

var contacts = new GenericRepository<ContactPerson>();

contacts.Add(new ContactPerson {
    Id = 1,
    Name = "Олена",
    Phone = "+380671234567",
    Email = "olena@example.com"
});
contacts.Add(new ContactPerson {
    Id = 2,
    Name = "Іван",
    Phone = "+380501234567",
    Email = "ivan@example.com"
});
contacts.Add(new ContactPerson {
    Id = 3,
    Name = "Марія",
    Phone = "+380631234567",
    Email = "maria@example.com"
});

Console.WriteLine($"Знайдено контакт: {contacts.GetById(1)}");

contacts.Update(new ContactPerson {
    Id = 1,
    Name = "Олена Коваль",
    Phone = "+380671234567",
    Email = "olena@example.com"
});
contacts.Delete(2);
foreach (var item in contacts.GetAll()) Console.WriteLine(item);

Console.WriteLine($"Відправлення: {postings.GetAll().Count}, контакти: {contacts.GetAll().Count}");
Console.WriteLine($"Інший екземпляр репозиторію бачить {new GenericRepository<Posting>().GetAll().Count} відправлення.");
