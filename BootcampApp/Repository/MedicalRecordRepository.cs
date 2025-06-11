using Model.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MedicalRecordRepository
    {
        private readonly string _connection = "Host=localhost;Port=5433;Username=postgres;Password=postgres;Database=PatientRecord";

        public async Task CreateAsync(Guid patientId, MedicalRecord record)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("INSERT INTO medical_record values (@record_id, @treatment, @patient_id)", conn);
            cmd.Parameters.AddWithValue("record_id", record.Id);
            cmd.Parameters.AddWithValue("treatment", record.Treatment);
            cmd.Parameters.AddWithValue("patient_id", patientId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateAsync(Guid patientId, MedicalRecord record)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("UPDATE medical_record " +
                                                    "SET treatment=@treatment, patient_id=@patient_id " +
                                                    "WHERE record_id=@record_id", conn);
            cmd.Parameters.AddWithValue("record_id", record.Id);
            cmd.Parameters.AddWithValue("treatment", record.Treatment);
            cmd.Parameters.AddWithValue("patient_id", patientId);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<Patient?> GetPatientWithRecordAsync(Guid patientId)
        {
            await using var conn = new NpgsqlConnection(_connection);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT p.patient_id, p.patient_name, p.age, p.condition, p.doctor_id, r.record_id, r.treatment, r.patient_id " +
                                        "FROM patient p " +
                                        "LEFT JOIN medical_record r ON p.patient_id = r.patient_id " +
                                        "WHERE p.patient_id = @patientId", conn);

            cmd.Parameters.AddWithValue("patientId", patientId);

            await using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new Patient
                {
                    Id = rdr.GetGuid(0),
                    Name = rdr.GetString(1),
                    Age = rdr.GetInt32(2),
                    Condition = rdr.GetString(3),
                    DoctorId = rdr.GetGuid(4),
                    Record = new MedicalRecord
                    {
                        Id = rdr.GetGuid(5),
                        Treatment = rdr.GetString(6),
                        PatientId = rdr.GetGuid(7)
                    }
                };
            }

            return null;
        }
    }
}
