using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using Model.DTO;
using Npgsql;
using System.Xml.Linq;

namespace Repository
{
    public class PatientRepository
    {
        private readonly string _connection = "Host=localhost;Port=5433;Username=postgres;Password=postgres;Database=PatientRecord";

        public void Create(Guid doctorId, Patient patient)
        {
            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO Patient values (@patient_id, @patient_name, @age, @condition, @doctor_id)", conn);
            cmd.Parameters.AddWithValue("patient_id", patient.Id);
            cmd.Parameters.AddWithValue("patient_name", patient.Name);
            cmd.Parameters.AddWithValue("age", patient.Age);
            cmd.Parameters.AddWithValue("condition", patient.Condition);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);

            cmd.ExecuteNonQuery();
        }

        public bool Update(Guid doctorId, Guid patientId, Patient patient)
        {
            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE Patient SET patient_name=@patient_name, age=@age, condition=@condition WHERE patient_id=@patient_id", conn);
            cmd.Parameters.AddWithValue("patient_id", patientId);
            cmd.Parameters.AddWithValue("patient_name", patient.Name);
            cmd.Parameters.AddWithValue("age", patient.Age);
            cmd.Parameters.AddWithValue("condition", patient.Condition);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        public Doctor GetAllPatients(Guid doctorId)
        {
            List<Patient> patients = new List<Patient>();
            Doctor doctor = new Doctor();

            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT d.doctor_id, d.doctor_name, d.age, d.specialty, p.patient_id, p.patient_name, p.age, p.condition " +
                                              "FROM Doctor d " +
                                              "INNER JOIN Patient p " +
                                              "ON d.doctor_id = p.doctor_id " +
                                              "WHERE d.doctor_id=@doctor_id", conn);

            cmd.Parameters.AddWithValue("doctor_id", doctorId);


            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    if(doctor.Id!=doctorId)
                    {
                        doctor.Id = doctorId;
                        doctor.Name = rdr.GetString(1);
                        doctor.Age = rdr.GetInt32(2);
                        doctor.Specialty = rdr.GetString(3);
                    }
                    patients.Add(new Patient
                    {
                        Id = rdr.GetGuid(4),
                        Name = rdr.GetString(5),
                        Age = rdr.GetInt32(6),
                        Condition = rdr.GetString(7),
                        DoctorId = doctorId
                    });
                }
                doctor.Patients = patients;
                return doctor;
            }
        }

        public bool Delete(Guid patientId)
        {
            using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM Patient WHERE patient_id=@patient_id", conn);
            cmd.Parameters.AddWithValue("patient_id", patientId);

            int rowsAffected = cmd.ExecuteNonQuery();

            return rowsAffected > 0;
        }

    }
}
