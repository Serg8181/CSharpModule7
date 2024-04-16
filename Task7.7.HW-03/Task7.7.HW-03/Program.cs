using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task7._7.HW_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Создаем заказчика
            Customer customer = new Customer("Глеб", "г. Городец , ул. Овражная, д.7", "567890");
            //создаем список покупок
            List<Product> products = new List<Product>()
            {
                new Milk(1.5f , 89.35f),
                new Bread(0.2f , 45.00f),
                new Juice(2 , 43.50f)
            };
            //создаем второй список продуктов - пустой
            List<Product> products2 = new List<Product>();
            //заказ - доставка на дом
            Order<HomeDelivery> order = new Order<HomeDelivery>(customer, products, new HomeDelivery(customer, products));
            order.ShowOrder();
            Console.ReadKey();
            //заказ - доставка в пункт выдачи
            Order<PickPointDelivery> order2 = new Order<PickPointDelivery>(customer, products, new PickPointDelivery(customer, products));

            order2.ShowOrder();
            Console.ReadKey();
            //заказ - доставка в магазин, но список покупок пустой
            Order<ShopDelivery> order3 = new Order<ShopDelivery>(customer, products2, new ShopDelivery(customer, products2));

            order3.ShowOrder();
            Console.ReadKey();
        }
    }
}
#region Human
internal abstract class Human
{
    public string Name { get; protected set; }
    public string Phone { get; protected set; }
    public abstract void Edit();
}
internal class Customer : Human//заказчик
{
    public string Adress { get; protected set; }
    public Customer(string name, string adress, string phone)
    {
        Name = name;
        Phone = phone;
        Adress = adress;
    }
    public override string ToString()
    {
        return $"Заказчик.\nИмя: {Name} Адрес: {Adress} Телефон: {Phone}";
    }
    public override void Edit()
    {
        Console.WriteLine("Введите имя заказчика: ");
        Name = Console.ReadLine();

        Console.WriteLine("Введите адрес заказчика: ");
        Adress = Console.ReadLine();

        Console.WriteLine("Введите номер телефона: ");
        Phone = Console.ReadLine();

        Console.WriteLine("Данные заказчика отредактированы.");
    }
}
internal class Curier : Human//курьер
{
    public string SecondName { get; protected set; }
    public Curier(string name, string secondName, string phone)
    {
        Name = name;
        SecondName = secondName;
        Phone = phone;
    }
    public override string ToString()
    {
        return $"Курьер.\nИмя: {Name} Фамилия: {SecondName} Телефон: {Phone}";
    }
    public override void Edit()
    {
        Console.WriteLine("Введите имя курьера: ");
        Name = Console.ReadLine();

        Console.WriteLine("Введите фамилию: ");
        SecondName = Console.ReadLine();

        Console.WriteLine("Введите номер телефона: ");
        Phone = Console.ReadLine();

        Console.WriteLine("Данные курьра отредактированы.");
    }
}
#endregion
#region CurierCompany
internal sealed class CurierCompany
{
    public string NameCompany { get; private set; }
    public string PhoneCompany { get; private set; }
    public Curier[] Curiers { get; }

    public CurierCompany(string name, string phone)
    {
        NameCompany = name;
        PhoneCompany = phone;
        Curiers = new Curier[]
        {
                new Curier("Иван", "Иванов", "+79996352121"),
                new Curier("Михаил", "Федоров", "+78995443973"),
                new Curier("Никита", "Сергеев", "+79632892366")
        };

    }
    public override string ToString()
    {
        return $"Курьерская служба: {NameCompany} Телефон: {PhoneCompany}";
    }
}
static class CurierCompanyExtension//расширение
{
    //назначить курьера
    public static Curier AppointCurier(this CurierCompany company)
    {
        Random random = new Random();
        int index = random.Next(0, company.Curiers.Count());
        for (int i = 0; i < company.Curiers.Count(); i++)
        {
            if (i == index) return company.Curiers[i];
        }
        return null;

    }
}
#endregion
#region Delivery
internal abstract class Delivery
{
    public Customer Customer { get; protected set; }
    public string Adress { get; protected set; }
    public List<Product> Products { get; protected set; }

    public Delivery(Customer customer, List<Product> product)
    {
        Customer = customer;
        Products = product;
        Adress = customer.Adress;
    }
    public abstract void Edit();
}
internal class HomeDelivery : Delivery
{
    public Curier Curier { get; protected set; }
    public CurierCompany CurierCompany { get; protected set; }




    public HomeDelivery(Customer customer, List<Product> product) : base(customer, product)
    {
        if (Products.Count() < 3)
        {
            Curier = new Curier("Владимир", "Гордеев", "+79871235581");
            CurierCompany = null;
        }
        else
        {
            CurierCompany = new CurierCompany("Доставляйка", "4552121");
            Curier = CurierCompany.AppointCurier();
        }
    }
    public override void Edit()
    {
        Console.WriteLine(Customer.ToString());
        Console.WriteLine("Введите новый адрес для доставки: ");
        Adress = Console.ReadLine();
        Console.WriteLine("Данные отредактированы успешно.");
    }
    public override string ToString()
    {
        if (CurierCompany != null)
            return $"{CurierCompany.ToString()} {Curier.ToString()}\nАдрес доставки: {Adress}";

        return $"{Curier.ToString()}\nАдрес доставки: {Adress}";
    }

}
internal class PickPointDelivery : Delivery
{
    public string[] PickPointAdresses { get; private set; }
    public DateTime DataForDelivery { get; private set; }


    public PickPointDelivery(Customer customer, List<Product> product) : base(customer, product)
    {
        PickPointAdresses = new string[] { "г. Москва, ул. Октябрьская, д.10", "г. Казань, ул. Баумана, д.25", "г. Н.Новгород, пр-т Гагарина, д.17 " };
        Random random = new Random();
        Adress = PickPointAdresses[random.Next(0, 3)];
        DataForDelivery = DateTime.Now.AddDays(10);
    }



    public override string ToString() => $"Доставка в пункт выдачи по адресу: {Adress}\nПолучатель: {Customer.ToString()}\nДата получения: {DataForDelivery.ToShortDateString()}\n";

    public override void Edit()
    {
        Console.WriteLine(Customer.ToString());
        Console.WriteLine("Введите новую дату получения: ");
        DataForDelivery = System.Convert.ToDateTime(Console.ReadLine());
        Console.WriteLine("Данные отредактированы успешно.");
    }
}
internal class ShopDelivery : Delivery
{
    public string[] ShopAdresses { get; private set; }
    protected List<Product> products;

    public ShopDelivery(Customer customer, List<Product> product) : base(customer, product)
    {

        ShopAdresses = new string[] { "г. Москва, ул. Фрунзе, д.18а", "г. Казань, ул. Мира, д.5", "г. Н.Новгород, ул. Ванеева, д.39 " };
        Random random = new Random();
        Adress = ShopAdresses[random.Next(0, 3)];

    }

    public override string ToString() => $"Доставка в магазин по адресу: {Adress}\nПолучатель: {Customer.ToString()}\n";

    public override void Edit()
    {
        Console.WriteLine("Данные на доставку в магазин изменить нельзя.");
    }
}

#endregion
#region Product
abstract class Product
{
    public string Name { get; protected set; }
    public float Price { get; protected set; }
}
internal class Milk : Product
{
    public float Volume { get; protected set; }
    public Milk(float volume, float price)
    {
        Name = "Молоко";
        Volume = volume;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} {Volume} л.";
    }
}
internal class Bread : Product
{
    public float Weight { get; protected set; }
    public Bread(float weight, float price)
    {
        Name = "Хлеб";
        Weight = weight;
        Price = price;
    }
    public override string ToString()
    {
        return $"{Name} {Weight} кг.";
    }
}
internal class Juice : Product
{
    public int Quantity { get; protected set; }

    public Juice(int quantity, float price)
    {
        Name = "Сок";
        Quantity = quantity;
        Price = price;
    }
    public override string ToString()
    {
        return $"{Name} {Quantity} шт.";
    }
}
#endregion
#region Order
internal class Order<TDelivery>
{
    public Customer Customer { get; protected set; }
    public string Adress { get; protected set; }
    public int Number { get; private set; }
    public TDelivery DeliveryForCustomer { get; protected set; }
    public float Price { get; protected set; }
    private List<Product> products;
    public List<Product> ProductForCustomer
    {
        get
        {
            return products;
        }
        protected set
        {
            if (value.Count != 0)
            {
                products = value;
            }
            else
            {
                Console.WriteLine($"Номер заказа: {Number}\nОшибка. Заказ не может быть создан.Список товаров для покупки пуст.");
                Console.ReadKey();
                System.Environment.Exit(0);
            }

        }


    }
    public Order(Customer customer, List<Product> prod, TDelivery del)
    {
        Console.Clear();

        Console.WriteLine("Создание заказа.\nЗаказчик:");
        Customer = customer;
        Adress = Customer.Adress;
        Random random = new Random();
        Number = random.Next(1, 1000);
        ProductForCustomer = prod;
        DeliveryForCustomer = del;


        Console.WriteLine(DeliveryForCustomer.GetType());



        this.Price = TotalPrice(ProductForCustomer);
    }
    private float TotalPrice(List<Product> products)
    {
        float total = 0;
        foreach (Product prod in ProductForCustomer)
        {
            total += prod.Price;
        }
        return total;
    }

    public void ShowOrder()
    {
        Console.Clear();
        string products = "";
        foreach (Product prod in ProductForCustomer)
        {
            products += $"{prod.ToString()}" + "\n\t";
        }
        Console.WriteLine("_________________________________________________________");
        Console.WriteLine($"Номер заказа: {Number}\n{Customer.ToString()}" +
            $"\nАдрес доставки: {Adress}\nТовар: {products}\nДоставка: {DeliveryForCustomer.ToString()}\nСтоимость заказа: {Price} руб.");
        Console.WriteLine("_________________________________________________________");
    }


}

#endregion
