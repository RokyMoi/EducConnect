using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EduConnect.Entities;


namespace EduConnect.Entities.Student
{
    [Table("Student", Schema = "Student")]
    public class Student
    {
        //        StudentId(uuid), PRIMARY KEY

        //PersonId(uuid), FOREIGN KEY TO „Person“.“Person“ 

        //CreatedAt – (bigint) – UNIX millis

        //ModifiedAt – (bigint) – UNIX millis – NULLABLE
        [Key]
        public Guid StudentId { get; set; }
        public  Guid PersonId { get; set; }
        [ForeignKey(nameof(PersonId))]
        public Person.Person Person { get; set; }

        public long CreatedAt { get; set; }
        public long? ModifiedAt {  get; set; }

       //Pristupi
     public StudentDetails StudentDetails { get; set; }


    }
}
