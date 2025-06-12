using Repository.Interface;
using Repository;
using Service.Interface;
using Service.ServiceImpl;
using Autofac;
using WebAPI.Controllers;
using Autofac.Extensions.DependencyInjection;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
//builder.Services.AddScoped<IPatientRepository, PatientRepository>();
//builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();

//builder.Services.AddScoped<IDoctorService, DoctorService>();
//builder.Services.AddScoped<IPatientService, PatientService>();
//builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();

//var app = builder.Build();


//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();



var containerBuilder = new ContainerBuilder();

    containerBuilder.RegisterType<DoctorRepository>().As<IDoctorRepository>();
    containerBuilder.RegisterType<PatientRepository>().As<IPatientRepository>();
    containerBuilder.RegisterType<MedicalRecordRepository>().As<IMedicalRecordRepository>();

    containerBuilder.RegisterType<DoctorService>().As<IDoctorService>();
    containerBuilder.RegisterType<PatientService>().As<IPatientService>();
    containerBuilder.RegisterType<MedicalRecordService>().As<IMedicalRecordService>();

var app = containerBuilder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//var builder = new ContainerBuilder();
//builder.RegisterType<DoctorService>().As<IDoctorService>();
//builder.RegisterType<DoctorRepository>().As<IDoctorRepository>();
//builder.RegisterType<DoctorController>();

//var app = builder.Build();