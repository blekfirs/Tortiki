using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ConsoleSharpProb
{
    //Тип данных - подпункт меню
    struct SubItem
    {
        public readonly string name;
        public readonly int price;

        public SubItem(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }
    //Класс - заказ
    class Order
    {
        //Путь куда сохранять заказы
        private static string _path = Environment.CurrentDirectory + @"\Заказы.txt";
        //Элементы меню
        private static (string, SubItem[])[] _items =
        {
            ("Форма", new SubItem[]{ 
                new SubItem("Круг", 500), 
                new SubItem("Квадрат", 500),
                new SubItem("Прямоугольник", 500),
                new SubItem("Сердечко", 700)
            }),
            ("Размер", new SubItem[]{
                new SubItem("Маленький", 1000),
                new SubItem("Обычный", 1200),
                new SubItem("Большой", 2000)
            }),
            ("Вкус коржей", new SubItem[]{
                new SubItem("Ванильный", 100),
                new SubItem("Шоколадный", 100),
                new SubItem("Карамельный", 150)
            }),
            ("Кол-во коржей", new SubItem[]{
                new SubItem("1", 200),
                new SubItem("2", 300),
                new SubItem("3", 400)
            }),
            ("Кол-во коржей", new SubItem[]{
                new SubItem("1", 200),
                new SubItem("2", 300),
                new SubItem("3", 400)
            }),
            ("Глазурь", new SubItem[]{
                new SubItem("Крем", 100),
                new SubItem("Драже", 150),
                new SubItem("Ягоды", 300)
            }),
            ("Декор", new SubItem[]{
                new SubItem("Крем", 100),
                new SubItem("Шоколад", 150),
                new SubItem("Ягоды", 100)
            })
        };
        //Выбранные элементы меню
        private Dictionary<string,SubItem> _selectedItems = new Dictionary<string, SubItem>(_items.Length);
        //Цена заказа
        private int TotalPrice { get => _selectedItems.Sum(x=>x.Value.price); }

        //Запись заказа в файл
        public void ToFile() =>
            File.WriteAllText(_path, ToString());

        //Основной метод с выбором пунктов
        public static Order Make()
        {
            Order order = new Order();

            int arrow_pos = 0, itemsLenght = _items.Count();
            while(true)
            {
                ShowItems(arrow_pos);
                order.ShowOrder();

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (arrow_pos - 1 < 0)
                            arrow_pos = 0;
                        else
                            arrow_pos--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (arrow_pos >= itemsLenght)
                            arrow_pos = itemsLenght;
                        else
                            arrow_pos++;
                        break;
                    case ConsoleKey.Enter:
                        if (arrow_pos >= itemsLenght)
                            return order;

                        SubItem? selectedItem = SelectSubItem(_items[arrow_pos].Item2);
                        if (selectedItem.HasValue)
                            order._selectedItems.Add(_items[arrow_pos].Item1, selectedItem.Value);
                        break;
                    case ConsoleKey.Escape:
                        return null;
                }
            }
        }

        //Выбор одного из подпунктов
        private static SubItem? SelectSubItem(SubItem[] subItems)
        {
            int arrow_pos = 0;
            while (true)
            {
                ShowSubItems(subItems, arrow_pos);
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                        if (arrow_pos - 1 < 0)
                            arrow_pos = 0;
                        else
                            arrow_pos--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (arrow_pos + 1 >= subItems.Length)
                            arrow_pos = subItems.Length - 1;
                        else
                            arrow_pos++;
                        break;
                    case ConsoleKey.Enter:
                        return subItems[arrow_pos];
                    case ConsoleKey.Escape:
                        return null;
                }
            }
        }

        //Вывод подпунктов в консоль
        private static void ShowSubItems(SubItem[] subItems, int arrow_pos)
        {
            Console.Clear();
            Console.WriteLine(
                "Для выхода нажмите Esc\n" +
                "Выберете пункт из меню:\n" +
                "==========================");

            for (int i = 0; i < subItems.Length; i++)
            {
                if (i == arrow_pos)
                    Console.Write("->");
                else
                    Console.Write("  ");
                Console.WriteLine(subItems[i].name + " - " + subItems[i].price);
            }
        }

        //Вывод пунктов меню в консоль
        private static void ShowItems(int arrowPos)
        {
            Console.Clear();
            Console.WriteLine(
                "Заказ тортов в ГЛАЗУРЬКА, торты на выбор!\n" +
                "Выберете параметр торта\n" +
                "==========================");
            for (int i = 0; i < _items.Length; i++)
                if (i == arrowPos)
                    Console.WriteLine("->" + _items[i].Item1);
                else
                    Console.WriteLine("  " + _items[i].Item1);
            if (arrowPos == _items.Length)
                Console.Write("->");
            else
                Console.Write("  ");
            Console.WriteLine("Конец заказа");
        }

        //Вывод заказа на экран
        private void ShowOrder() =>
            Console.WriteLine(ToString());

        //Преобразует весь заказ в строку
        private new string ToString()
        {
            string str_order = "Цена: " + TotalPrice +
                "\nВаш торт: ";
            foreach (SubItem item in _selectedItems.Select(x => x.Value))
                str_order += item.name + " - " + item.price + "; ";
            return str_order;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //Создаем заказ
                Order order = Order.Make();
                //Если нулл - выход из проги
                if (order == null)
                    return;
                //записываем в файл
                order.ToFile();

                Console.Clear();
                Console.WriteLine(
                    "Спасибо за заказ! " + 
                    "Если хотите ещё, нажмите Enter " +
                    "Чтобы выйти - Escape");
                select:
                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.Enter:
                        continue;
                    case ConsoleKey.Escape:
                        return;
                    default:
                        goto select;
                }

            }
        }
    }
}