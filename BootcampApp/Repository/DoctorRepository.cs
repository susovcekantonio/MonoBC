using Model.Models;
using Npgsql;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connection = "Host=localhost;Port=5433;Username=postgres;Password=postgres;Database=PatientRecord";

        public async Task CreateAsync(Doctor doctor)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("INSERT INTO Doctor values (@doctor_id, @doctor_name, @age, @specialty)", conn);
            cmd.Parameters.AddWithValue("doctor_id", doctor.Id);
            cmd.Parameters.AddWithValue("doctor_name", doctor.Name);
            cmd.Parameters.AddWithValue("age", doctor.Age);
            cmd.Parameters.AddWithValue("specialty", doctor.Specialty);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> UpdateAsync(Guid doctorId, Doctor doctor)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();


            await using var cmd = new NpgsqlCommand("UPDATE Doctor " +
                                                    "SET doctor_name=@doctor_name, age=@age, specialty=@specialty " +
                                                    "WHERE doctor_id=@doctor_id", conn);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);
            cmd.Parameters.AddWithValue("doctor_name", doctor.Name);
            cmd.Parameters.AddWithValue("age", doctor.Age);
            cmd.Parameters.AddWithValue("specialty", doctor.Specialty);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

        public async Task<Doctor?> GetAsync(Guid doctorId)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("SELECT * FROM Doctor WHERE doctor_id=@doctor_id", conn);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);


            await using (var rdr = await cmd.ExecuteReaderAsync())
            {
                while (rdr.Read())
                {
                    var doctor = new Doctor
                    {
                        Id = rdr.GetGuid(0),
                        Name = rdr.GetString(1),
                        Age = rdr.GetInt32(2),
                        Specialty = rdr.GetString(3)
                    };
                    return doctor;
                }
                return null;
            }
        }

        public async Task<bool> DeleteAsync(Guid doctorId)
        {
            await using var conn = new NpgsqlConnection(_connection);
            conn.Open();

            await using var cmd = new NpgsqlCommand("DELETE FROM Doctor WHERE doctor_id=@doctor_id", conn);
            cmd.Parameters.AddWithValue("doctor_id", doctorId);

            int rowsAffected = await cmd.ExecuteNonQueryAsync();

            return rowsAffected > 0;
        }

    }
}
