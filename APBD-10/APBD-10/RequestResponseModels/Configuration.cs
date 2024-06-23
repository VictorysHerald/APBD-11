using System.Data;
using APBD_10.Models;

namespace APBD_10.RequestResponseModels;

public static class Configuration
{
    public static void RegisterEndpointsForHospital(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/prescriptions", async (IHospitalService service, Prescription request) =>
        {
            try
            {
                await service.AddPrescription(request);
                return Results.Created();
            }
            catch (DataException e)
            {
                return Results.NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });

        app.MapGet("api/prescription/patient/{id:int}", async (IHospitalService service, int id) =>
        {
            try
            {
                var result = await service.GetPatientPrescriptions(id);
                return Results.Ok(result);
            }
            catch (DataException e)
            {
                return Results.NotFound(e.Message);
            }
            catch (Exception e)
            {
                return Results.Problem(e.Message);
            }
        });
    }
}