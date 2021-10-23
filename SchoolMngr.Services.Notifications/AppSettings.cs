
namespace SchoolMngr.Services.Backoffice
{
    using Codeit.NetStdLibrary.Base.Common;
    using SchoolMngr.Infrastructure.Shared.Configuration;

    public class AppSettings : BaseSettings<AppSettings>
    {
        public INFRASettings InfraSection { get; set; }
    }
}
