using SOL.IdP;
using SOL.IdP.Phase2;

namespace SOL.Gateway;

public class DataBootstrap : BackgroundService
{
    private readonly IPhase2AdminClient _phase2AdminClient;
    private readonly ILogger<DataBootstrap> _logger;

    public DataBootstrap(IPhase2AdminClient phase2AdminClient, ILogger<DataBootstrap> logger)
    {
        _phase2AdminClient = phase2AdminClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Preparing to bootstrap the Demo Org.");
        await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
        
        try
        {
            var orgCount =
                await _phase2AdminClient.GetOrganizationsCountAsync(KeycloakConst.RealmId, search: "demo",
                    cancellationToken: stoppingToken);

            if (orgCount > 0)
                return;

            var orgData = new OrganizationRepresentation
            {
                Name = "demo",
                DisplayName = "Demo Organization",
                Realm = KeycloakConst.RealmId
            };

            await _phase2AdminClient.CreateOrganizationAsync(orgData, KeycloakConst.RealmId, stoppingToken);
            var org =
                await _phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId, search: "demo",
                    cancellationToken: stoppingToken);

            _logger.LogInformation("Demo Organization created: {DemoOrgId}", org.First().Id);

            // Tony Stark
            await _phase2AdminClient.AddOrganizationMemberAsync(KeycloakConst.RealmId, org.First().Id
                , "30400edb-a083-4efc-a823-86b6599e8811", stoppingToken);
            _logger.LogInformation("Tony Stark added as Member to Demo Organization: {DemoOrgId}", org.First().Id);

            // James Rhodes
            await _phase2AdminClient.AddOrganizationMemberAsync(KeycloakConst.RealmId, org.First().Id
                , "896b1095-a603-4f3d-8ed4-76b46d1be0a6", stoppingToken);
            _logger.LogInformation("James Rhodes added as Member to Demo Organization: {DemoOrgId}", org.First().Id);

            // Pepper Potts
            await _phase2AdminClient.AddOrganizationMemberAsync(KeycloakConst.RealmId, org.First().Id
                , "fa455caf-fd17-4c97-be45-64b856783e88", stoppingToken);
            _logger.LogInformation("Pepper Potts added as Member to Demo Organization: {DemoOrgId}", org.First().Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while bootstrapping the Demo Org.");
        }
    }
}