
namespace SchoolMngr.Services.Notifications.Api;

public class AppSettings : BaseSettings<AppSettings>
{
    public static string InfraSectionKey => INFRASettings.SectionKey;
    public INFRASettings InfraSection { get; set; }
}
