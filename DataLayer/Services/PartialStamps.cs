using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DataLayer.Interfaces;
using DataLayer.Models;

namespace DataLayer.Services
{
    public partial class ProfileService : IProfileService
    {
        public async Task<UsersStamp> GetStampByMatchId(int matchId)
        {
            Match m = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);
            return await _context.UsersStamps.FirstOrDefaultAsync(s => s.Id == m.UsersStampId);
        }
        public async Task<List<UsersStamp>> FindMatching(string pattern)
        {
            return await _context.UsersStamps.Where(s=>EF.Functions.Like(s.Stamp, pattern)).ToListAsync();
        }
        public async Task<UsersStamp> GetStamp(int userId, int formId)
        {
            return await _context.UsersStamps.FirstOrDefaultAsync(u=>u.UserId == userId && u.FormId == formId);
        }
        public async Task DeleteStampAndMatches(int userId, int formId)
        {
            UsersStamp stamp = await _context.UsersStamps.FirstOrDefaultAsync(s=>s.UserId == userId && s.FormId == formId);

            if(stamp!=null)
            {
                await DeleteMatches(stamp.Id);
                _context.UsersStamps.Remove(stamp);
                await _context.SaveChangesAsync();
            }

            await ChangeUserStatus(userId, formId, Enums.UserStatuses.NoAnswers);
        }

        public async Task CreateStamp(List<UserAnswer> answers, int formId)
        {
            Form form = await _context.Forms.FirstOrDefaultAsync(f=>f.Id == formId);

            UsersStamp newStamp = new()
            {
                UserId = answers.First().UserId,
                FormId = form.Id,
                Stamp = form.Name+"#",
                Pattern = form.Name+"#"             
            };

            for(int i =0; i<answers.Count(); i++)
            {
                
                QuestionModule module = await GetQuestionModule(answers[i].FormModuleConnection.QuestionModuleId);
                Dictionary<string, string[]> answerMatrix = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(module.JSONAnswerMatrix);

                bool firstMulti = module.FirstMultiselectable;
                bool secondMulti = module.SecondMultiselectable;


                string[] firstDatas = await GetDataFromAnswerVariantIds(JsonConvert.DeserializeObject<int[]>(answers[i].FirstQuestionAnswerVariantsJSON));
                string[] secondDatas = null;

                if (answers[i].SecondQuestionAnswerVariantsJSON != null)
                {
                    secondDatas = await GetDataFromAnswerVariantIds(JsonConvert.DeserializeObject<int[]>(answers[i].SecondQuestionAnswerVariantsJSON));
                }

                newStamp.Stamp += CreateStamp(firstDatas, secondDatas) + "#";
                newStamp.Pattern+= CreatePattern(firstDatas, secondDatas, firstMulti, secondMulti, answerMatrix) + "#";
            }

            await _context.UsersStamps.AddAsync(newStamp);
            await _context.SaveChangesAsync();
        }
        private async Task<string[]> GetDataFromAnswerVariantIds(int[] answerVariantId)
        {
            List<string> datas = new();

            foreach(int id in answerVariantId)
            {
               var variant =  await GetAnswerVariant(id);
                datas.Add(variant.Data);
            }
            return datas.ToArray();
        }
        private string CreateStamp(string[] firstDatas, string[] secondDatas)
        {
            string result = "";
            foreach (string data in firstDatas)
            {
                result += data;
            }
            if (secondDatas != null && secondDatas.Length > 0)
            {
                result += "#";
                foreach (string data in secondDatas)
                {
                    result += data;
                }
            }
            return result;
        }
        private string FomatElementsForPattern(List<string> elements, bool multiselectable)
        {
            string result = String.Join(",", elements);
            if (elements.Count > 1) result = "[" + result + "]";
            if (multiselectable) result = "%" + result + "%";
            return result;
        }
        private string CreatePattern(string[] firstDatas, string[] secondDatas, bool firstMulti, bool secondMulti, Dictionary<string, string[]> matrix)
        {
            string firstPart = "";
            string secondPart = "";
            List<string> elements = new();

            if (secondDatas == null || secondDatas.Length == 0)
            {
                foreach (string data in firstDatas)
                {
                    elements.AddRange(matrix[data]);
                }
                firstPart = FomatElementsForPattern(elements.Distinct().ToList(), firstMulti);
            }
            else
            {
                foreach (string data in firstDatas)
                {
                    elements.AddRange(matrix[data]);
                }
                secondPart = "#" + FomatElementsForPattern(elements.Distinct().ToList(), secondMulti);

                elements.Clear();

                foreach (KeyValuePair<string, string[]> entry in matrix)
                {
                    foreach (string data in secondDatas)
                    {
                        if (entry.Value.Contains(data))
                        {
                            elements.Add(entry.Key);
                        }
                    }
                }
                firstPart = FomatElementsForPattern(elements.Distinct().ToList(), firstMulti);
            }

            return firstPart + secondPart;
        }
    }
}
