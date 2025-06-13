using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using Model.DTO;
using Npgsql;
using System.Xml.Linq;
using Repository.Common;

namespace Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connection = "Host=localhost;Port=5433;Username=postgres;Password=postgres;Database=PatientRecord";

        public async Task CreateAsync(Guid doctorId, Patient patient)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("INSERT INTO Patient values (@patient_id, @patient_name, @age, @condition, @doctor_id)", conn);
            cmd.Parameters.AddWithValue("patient_id", patient.Id);
            cmd.Parameters.AddWithValue("patient_name", patient.Name);
            cmd.Parameters.AddWithValue("age", patient.Age);
            cmd.Parameters.AddWithValue("condition", patient.Condition);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateAsync(Guid doctorId, Guid patientId, Patient patient)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("UPDATE Patient " +
                                                    "SET patient_name=@patient_name, age=@age, condition=@condition " +
                                                    "WHERE patient_id=@patient_id", conn);
            cmd.Parameters.AddWithValue("patient_id", patientId);
            cmd.Parameters.AddWithValue("patient_name", patient.Name);
            cmd.Parameters.AddWithValue("age", patient.Age);
            cmd.Parameters.AddWithValue("condition", patient.Condition);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<Doctor> GetAllPatientsAsync(Guid doctorId)
        {
            List<Patient> patients = new List<Patient>();
            Doctor doctor = new Doctor();

            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("SELECT d.doctor_id, d.doctor_name, d.age, d.specialty, p.patient_id, p.patient_name, p.age, p.condition " +
                                              "FROM Doctor d " +
                                              "INNER JOIN Patient p " +
                                              "ON d.doctor_id = p.doctor_id " +
                                              "WHERE d.doctor_id=@doctor_id", conn);

            cmd.Parameters.AddWithValue("doctor_id", doctorId);


            await using (var rdr = await cmd.ExecuteReaderAsync())
            {
                while (rdr.Read())
                {
                    if (doctor.Id != doctorId)
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

        public async Task<Doctor?> GetDoctorWithPatientsPaginatedAsync(Guid doctorId, int page, int pageSize, string sort, string order)
        {
            var doctor = new Doctor();
            var patients = new List<Patient>();

            int offset = (page - 1) * pageSize;

            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();

            var countCmd = new NpgsqlCommand("SELECT COUNT(*) FROM patient WHERE doctor_id = @doctor_id", conn);
            countCmd.Parameters.AddWithValue("doctor_id", doctorId);
            int count = Convert.ToInt32(await countCmd.ExecuteScalarAsync());

            var cmd = new NpgsqlCommand("SELECT d.doctor_id, d.doctor_name, d.age, d.specialty, " +
                                        "p.patient_id, p.patient_name, p.age, p.condition, " +
                                        "r.record_id, r.treatment, r.patient_id " +
                                        "FROM Doctor d " +
                                        "INNER JOIN Patient p ON d.doctor_id = p.doctor_id " +
                                        "LEFT JOIN medical_record r ON p.patient_id = r.patient_id " +
                                        "WHERE d.doctor_id = @doctor_id " +
                                        $"ORDER BY p.{sort} {order} " +
                                        "LIMIT @limit OFFSET @offset ", conn);

            cmd.Parameters.AddWithValue("doctor_id", doctorId);
            cmd.Parameters.AddWithValue("limit", pageSize);
            cmd.Parameters.AddWithValue("offset", offset);


            await using var rdr = await cmd.ExecuteReaderAsync();

            while (await rdr.ReadAsync())
            {
                if (doctor.Id != doctorId)
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
                    DoctorId = doctorId,
                    Record = new MedicalRecord
                    {
                        Id = rdr.GetGuid(8),
                        Treatment = rdr.GetString(9),
                        PatientId = rdr.GetGuid(10)
                    }
                });
            }


            doctor.Patients = patients;
            return doctor;
        }

        public async Task<bool> DeleteAsync(Guid patientId)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("DELETE FROM Patient WHERE patient_id=@patient_id", conn);
            cmd.Parameters.AddWithValue("patient_id", patientId);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

    }
}
