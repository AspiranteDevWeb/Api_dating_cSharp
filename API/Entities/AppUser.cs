using System.ComponentModel.DataAnnotations;
using API.Extensions;

namespace API.Entities
{
    
    public class AppUser
    {
        
        public int GetAge()
        {
            return DateOfBirth.CalculateAge();
        }
    }
}
