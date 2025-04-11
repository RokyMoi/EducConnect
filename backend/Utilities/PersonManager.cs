using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduConnect.Entities.Person;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace EduConnect.Utilities
{
    public class PersonManager : UserManager<Person>
    {
        public PersonManager(IUserStore<Person> store, IOptions<IdentityOptions> optionsAccessor,
       IPasswordHasher<Person> passwordHasher, IEnumerable<IUserValidator<Person>> userValidators,
       IEnumerable<IPasswordValidator<Person>> passwordValidators, ILookupNormalizer keyNormalizer,
       IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<Person>> logger)
       : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override Task<Person?> FindByNameAsync(string userName)
        {
            return Users.Include(x => x.PersonDetails).FirstOrDefaultAsync(x => x.PersonDetails.Username == userName);
        }

        public override Task<Person?> FindByEmailAsync(string email)
        {
            return Users.Include(x => x.PersonEmail).FirstOrDefaultAsync(x => x.PersonEmail.Email == email);
        }

        public override async Task<IdentityResult> AddToRoleAsync(Person user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(role));
            }

            // Example: Custom logic — maybe log role assignment or check something
            if (user.PersonDetails != null && !string.IsNullOrEmpty(user.PersonDetails.Username))
            {
                Console.WriteLine($"Assigning role '{role}' to user with username '{user.PersonDetails.Username}'");
            }
            else
            {
                Console.WriteLine($"Assigning role '{role}' to user with email '{user.Email}' (no username set)");
            }

            // Call the base implementation
            return await base.AddToRoleAsync(user, role);
        }
    }

}