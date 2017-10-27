namespace Survey.Business.Services.Contracts
{
    using Entities;
 
    using System.Threading.Tasks;

    public interface IStudentService 
    {
        #region Public Methods
        Task<UserDto> GetUserProfile(string userName);
        Task<UserDto> GetUserProfile(long studentId);
        #endregion
    }
}