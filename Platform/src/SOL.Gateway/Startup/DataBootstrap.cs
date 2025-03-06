using SOL.IdP;
using SOL.IdP.Phase2;

namespace SOL.Gateway;

public class DataBootstrap : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataBootstrap> _logger;

    public DataBootstrap(IServiceProvider serviceProvider, ILogger<DataBootstrap> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Preparing to bootstrap the Demo Org -- waiting for Keycloak to be ready.");
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);

            var phase2AdminClient = _serviceProvider.GetRequiredService<IPhase2AdminClient>();
            
            var orgCount =
                await phase2AdminClient.GetOrganizationsCountAsync(KeycloakConst.RealmId, search: "demo",
                    cancellationToken: stoppingToken);

            if (orgCount > 0)
                return;

            var orgData = new OrganizationRepresentation
            {
                Name = "demo",
                DisplayName = "Demo Organization",
                Realm = KeycloakConst.RealmId
            };

            await phase2AdminClient.CreateOrganizationAsync(orgData, KeycloakConst.RealmId, stoppingToken);
            var org =
                await phase2AdminClient.GetOrganizationsAsync(KeycloakConst.RealmId, search: "demo",
                    cancellationToken: stoppingToken);

            _logger.LogInformation("Demo Organization created: {DemoOrgId}", org.First().Id);

            // Tony Stark
            await phase2AdminClient.AddOrganizationMemberAsync(KeycloakConst.RealmId, org.First().Id
                , "6a7c7d5a-e560-4172-adcf-4028c0807c2e", stoppingToken);
            _logger.LogInformation("Tony Stark added as Member to Demo Organization: {DemoOrgId}", org.First().Id);

            // James Rhodes
            await phase2AdminClient.AddOrganizationMemberAsync(KeycloakConst.RealmId, org.First().Id
                , "a024b775-13b8-4d2a-9170-dc1ea01c4d60", stoppingToken);
            _logger.LogInformation("James Rhodes added as Member to Demo Organization: {DemoOrgId}", org.First().Id);

            // Pepper Potts
            await phase2AdminClient.AddOrganizationMemberAsync(KeycloakConst.RealmId, org.First().Id
                , "0ccdf7be-c987-4f3f-b7fe-5c661028e500", stoppingToken);
            _logger.LogInformation("Pepper Potts added as Member to Demo Organization: {DemoOrgId}", org.First().Id);
            
            // Happy Hogan
            await phase2AdminClient.AddOrganizationMemberAsync(KeycloakConst.RealmId, org.First().Id
                , "7bddef79-1709-4db2-acd4-b47c8c8f4403", stoppingToken);
            _logger.LogInformation("Happy Hogan added as Member to Demo Organization: {DemoOrgId}", org.First().Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while bootstrapping the Demo Org.");
        }
    }
}