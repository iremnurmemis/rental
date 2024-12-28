using Business;
using DataAccess;
namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("HELLo");
   

           



            ////CarManager carManager = new CarManager(new EfCarDal());
            ////foreach(var car in carManager.GetAll())
            ////{
            ////    Console.WriteLine(car.Description);
            ////}

            ////Console.WriteLine("***********************************");

            ////foreach (var car in carManager.GetCarsByBrandId(2))
            ////{
            ////    Console.WriteLine(car.Description);
            ////}

            ////Brand brand = new Brand { Id=13,Name="MiniCooper"};
            ////BrandManager brandManager= new BrandManager(new EfBrandDal());
            ////brandManager.Add(brand);

            ////Car deneme = new Car {Id=11,BrandId=13, ColorId=5,DailyPrice=2,ModelYear=2010,Description="MiniCooper" };

            ////carManager.Add(deneme);

        }
    }
}