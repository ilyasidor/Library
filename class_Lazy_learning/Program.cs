using System;
using System.Collections;
using System.Collections.Generic;

namespace class_Lazy_learning
{
    class Program
    {
        static void Main(string[] args)
        {
            Reader reader = new Reader();
            reader.ReadBook();
        }
    }
    interface EventInicialization
    {
        delegate void MessageHandler(string message);
        delegate int Choice();

        event MessageHandler Event;
        event Choice Event_Choice;
    }
    struct Action
    {
        public static void Get_Message(string message)
        {
            Console.WriteLine(message);
        }
    }
    class Reader : EventInicialization
    {
        
        public event EventInicialization.MessageHandler Event;
        public event EventInicialization.Choice Event_Choice;
        
        public Reader()
        {
            Event += Action.Get_Message;
        }
        
        static Lazy<Library> library = new Lazy<Library>();
        public void ReadBook()
        {
            Event?.Invoke("Вы решили звять книгу из библиотеки");
            library.Value.GetBook();
        }
        public void ReadEBook()
        {
            Event?.Invoke("Читаем электронную книгу");
        }
    }
    class Library:EventInicialization
    {
        public event EventInicialization.MessageHandler Event;
        public event EventInicialization.Choice Event_Choice;

        public Library()
        {
            Event += Action.Get_Message;
        }

        List<Book> books = new List<Book>(99)
        {
            new Book("Маша и медведь"),
            new Book("Война и мир"),
            new Book("Крысиные бега"),
            new Book("Мастер и маргарита"),
        };

        public void AddBook(string name)
        {
            books.Add(new Book(name));
        }

        public void GetBook()
        {
            
            Event?.Invoke("Выберете одну из предложенных книг: ");
            int count = 0;
            foreach (Book book in books)
            {
                count++;

                Event?.Invoke($"{count} - {book.name}");
            }
            Event_Choice += Library_Event_Choice;
            int choice = Event_Choice()-1;
            try
            {
                books.RemoveAt(choice);
                Event?.Invoke($"Вы успешно взяли книгу {books[choice].name}");
            }
            catch
            {
                Event?.Invoke("Такой книги нет!");
            }
        }
        public IEnumerator GetEnumerator()
        {
            return books.GetEnumerator();
        }


        private int Library_Event_Choice()
        {
            int i;
            string input = Console.ReadLine();
            if(Int32.TryParse(input,out i))
            {
                return i;
            }
            else
            {
                Event?.Invoke("Вы ввели не число!");
            }
            return -1;
        }
    }
    class Book
    {
        public string name;
        public Book(string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
