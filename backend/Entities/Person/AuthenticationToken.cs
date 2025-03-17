using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EduConnect.Entities.Person
{
    [Table("AuthenticationToken", Schema = "Person")]
    public class AuthenticationToken
    {

        [Key]
        public Guid AuthenticationTokenId { get; set; }

        public Guid PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public Person? Person { get; set; } = null;

        public required string Token { get; set; }

        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        public long? UpdatedAt { get; set; } = null;


    }
}