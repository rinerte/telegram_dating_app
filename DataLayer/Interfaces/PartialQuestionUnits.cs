using DataLayer.Models;

namespace DataLayer.Interfaces
{
    public partial interface IProfileService
    {
       public Task<QuestionUnit> GetQuestionUnit(int questionUnitId);
       public Task<List<QuestionUnit>> GetQuestionUnits();
       public Task DeleteQuestionUnit(int questionUnitId);
        public Task CreateQuestionUnit(string questionText);
    }
}
