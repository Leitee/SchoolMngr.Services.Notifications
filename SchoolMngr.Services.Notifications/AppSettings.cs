
namespace SchoolMngr.Services.Notifications
{
    using Codeit.Enterprise.Base.Common;
    using SchoolMngr.Infrastructure.Shared.Configuration;

    public class AppSettings : BaseSettings<AppSettings>
    {
        public const string InfraSectionKey = "InfraSection";
        public INFRASettings? InfraSection { get; set; }
    }
}