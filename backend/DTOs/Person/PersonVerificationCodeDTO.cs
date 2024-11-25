using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs.Person
{
    public class PersonVerificationCodeDTO
    {
        public Guid PersonVerificationCodeId { get; set; }
        public Guid PersonId { get; set; }
        public string VerificationCode { get; set; }
        public long ExpiryDateTime { get; set; }
        public bool IsVerified { get; set; }
    }
}