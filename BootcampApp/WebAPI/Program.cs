using Repository.Interface;
using Repository;
using Service.Interface;
using Service.ServiceImpl;
using Autofac;
using WebAPI.Controllers;
using Autofac.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<DoctorRepository>().As<IDoctorRepository>().InstancePerRequest();
    containerBuilder.RegisterType<PatientRepository>().As<IPatientRepository>().InstancePerRequest();
    containerBuilder.RegisterType<MedicalRecordRepository>().As<IMedicalRecordRepository>().InstancePerRequest();

    containerBuilder.RegisterType<DoctorService>().As<IDoctorService>().InstancePerRequest();
    containerBuilder.RegisterType<PatientService>().As<IPatientService>().InstancePerRequest();
    containerBuilder.RegisterType<MedicalRecordService>().As<IMedicalRecordService>().InstancePerRequest();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();