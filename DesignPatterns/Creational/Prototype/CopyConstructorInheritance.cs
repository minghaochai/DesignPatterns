namespace DesignPatterns.Creational.Prototype
{
    public interface IDeepCopyable<T> where T : new()
    {
        void CopyTo(T target);

        public T DeepCopy()
        {
            T t = new T();
            CopyTo(t);
            return t;
        }
    }

    public class AddressInherit : IDeepCopyable<AddressInherit>
    {
        public string StreetName;
        public int HouseNumber;

        public AddressInherit(string streetName, int houseNumber)
        {
            StreetName = streetName;
            HouseNumber = houseNumber;
        }

        public AddressInherit()
        {
        }

        public override string ToString()
        {
            return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
        }

        public void CopyTo(AddressInherit target)
        {
            target.StreetName = StreetName;
            target.HouseNumber = HouseNumber;
        }
    }



    public class PersonInherit : IDeepCopyable<PersonInherit>
    {
        public string[] Names;
        public AddressInherit Address;

        public PersonInherit()
        {

        }

        public PersonInherit(string[] names, AddressInherit address)
        {
            Names = names;
            Address = address;
        }

        public override string ToString()
        {
            return $"{nameof(Names)}: {string.Join(",", Names)}, {nameof(Address)}: {Address}";
        }

        public virtual void CopyTo(PersonInherit target)
        {
            target.Names = (string[])Names.Clone();
            //target.Address = ((IDeepCopyable<AddressInherit>)Address).DeepCopy();
            target.Address = Address.DeepCopy();
        }
    }

    public class EmployeeInherit : PersonInherit, IDeepCopyable<EmployeeInherit>
    {
        public int Salary;

        public void CopyTo(EmployeeInherit target)
        {
            base.CopyTo(target);
            target.Salary = Salary;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Salary)}: {Salary}";
        }
    }

    // To avoid explicit casting when invoking DeepCopy() in the constructor use the extension methods below 
    public static class DeepCopyExtensions
    {
        public static T DeepCopy<T>(this IDeepCopyable<T> item)
          where T : new()
        {
            return item.DeepCopy();
        }

        public static T DeepCopy<T>(this T person)
          where T : PersonInherit, new()
        {
            return ((IDeepCopyable<T>)person).DeepCopy();
        }
    }

    public static class Demo
    {
        static void Main()
        {
            var john = new EmployeeInherit();
            john.Names = new[] { "John", "Doe" };
            john.Address = new AddressInherit { HouseNumber = 123, StreetName = "London Road" };
            john.Salary = 321000;
            var copy = ((IDeepCopyable<EmployeeInherit>)john).DeepCopy();
            //var copy = john.DeepCopy();

            copy.Names[1] = "Smith";
            copy.Address.HouseNumber++;
            copy.Salary = 123000;

            Console.WriteLine(john);
            Console.WriteLine(copy);
        }
    }
}