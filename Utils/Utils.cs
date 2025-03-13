using ZooKeepers.Data;
using ZooKeepers.Models;
namespace ZooKeepers.Utils
{
    public class Age
    {
        public static int CalculateAge(Animal animal)
        {
            DateTime currentDate = DateTime.Now;
            var years = currentDate.Year-animal.DateOfBirth.Year;

            var months = currentDate.Month-animal.DateOfBirth.Month;

            var days = currentDate.Day - animal.DateOfBirth.Day;

            if (months < 0 || (months==0 && days<0))
            {
                years--;
            }

            return years;
        } 
    }
}