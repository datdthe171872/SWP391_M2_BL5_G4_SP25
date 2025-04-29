using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Resumes
{
    public interface IResumeService
    {
        Task<IList<Resume>> GetResumesAsync(int id, string searchDescription, DateTime? searchDob);
        Task<Resume> GetResumeByIdAsync(int resumeID, int id);
        Task AddResumeAsync(Resume resume);
        Task UpdateResumeAsync(Resume resume);
        Task DeleteResumeAsync(int id1, int id2);
    }
}
