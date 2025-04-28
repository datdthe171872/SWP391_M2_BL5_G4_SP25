using Microsoft.EntityFrameworkCore;
using SWP391_M2_BL5_G4_SP25.Models;

namespace SWP391_M2_BL5_G4_SP25.Pages.Resumes
{
    public class ResumeService : IResumeService
    {
        private readonly MyDBContext _context;

        public ResumeService(MyDBContext context)
        {
            _context = context;
        }

        public async Task<IList<Resume>> GetResumesAsync(int id, string searchDescription, DateTime? searchDob)
        {
            var query = _context.Resumes
                .Include(r => r.JobSeekerProfile)
                .Where(r => !r.IsDelete && r.JobSeekerProfile.UserID.ToString() == id.ToString() && !r.JobSeekerProfile.isDelete);

            if (!string.IsNullOrEmpty(searchDescription))
            {
                query = query.Where(r => r.JobSeekerProfile.Description != null && 
                                       r.JobSeekerProfile.Description.Contains(searchDescription));
            }

            if (searchDob.HasValue)
            {
                query = query.Where(r => r.JobSeekerProfile.Dob.Date == searchDob.Value.Date);
            }

            return await query.OrderByDescending(r => r.ResumeID).ToListAsync();
        }

        public async Task<Resume> GetResumeByIdAsync(int resumeID, int id)
        {
            return await _context.Resumes
                .Include(r => r.JobSeekerProfile)
                .FirstOrDefaultAsync(r => r.ResumeID == resumeID && 
                                        !r.IsDelete && 
                                        r.JobSeekerProfile.UserID.ToString() == id.ToString() && 
                                        !r.JobSeekerProfile.isDelete);
        }

        public async Task AddResumeAsync(Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));

            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateResumeAsync(Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));

            _context.Resumes.Update(resume);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteResumeAsync(int id1, int id2)
        {
            var resume = await GetResumeByIdAsync(id1, id2);
            if (resume != null)
            {
                resume.IsDelete = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
