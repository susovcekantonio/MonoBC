namespace Model.Models
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Condition { get; set; }
        public Guid DoctorId { get; set; }
        public MedicalRecord Record { get; set; }
    }
}
