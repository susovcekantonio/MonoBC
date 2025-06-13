using Repository.Common;
using Repository;
using Service.Common;
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
    containerBuilder.RegisterType<DoctorRepository>().As<IDoctorRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PatientRepository>().As<IPatientRepository>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<MedicalRecordRepository>().As<IMedicalRecordRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<DoctorService>().As<IDoctorService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<PatientService>().As<IPatientService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<MedicalRecordService>().As<IMedicalRecordService>().InstancePerLifetimeScope();
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