using DataLayer.Interfaces;
using DataLayer.Models;

namespace Bot
{
    internal static class Messages
    {

        internal async static Task<string> FoundUserMessage(int matchId, User user, IProfileService _db)
        {
            string stamp = (await _db.GetStampByMatchId(matchId)).Stamp;
            string formName = stamp.Split('#')[0];
            Random rand = new();
            int r = rand.Next(0, 10);

            if (formName.ToLower().Contains("знакомств"))
            {
                if (r > 4)
                {
                    return String.Format("Кажется, @{0} вам отлично подходит для «знакомства». Пора поздороваться!", user.Login);
                }
                else
                {
                    return String.Format("Ваш новый Match в анкете «Знакомства» — @{0}. Пора начинать общение!", user.Login);
                }

            }
            else if (formName.ToLower().Contains("отношения"))
            {
                if (r > 4)
                {
                    return String.Format("А вот и потенциальный партнёр для отношений @{0}. Не заставляй судьбу ждать!", user.Login);
                }
                else
                {
                    return String.Format("Кажется, @{0} вам отлично подходит! Пора на свидание?", user.Login);
                }
            }
            else if (formName.ToLower().Contains("cосед"))
            {
                if (r > 4)
                {
                    return String.Format("Вам подобран идеальный сосед! Это @{0} — напишите ему!", user.Login);
                }
                else
                {
                    return String.Format("Кажется, вы отлично уживётесь с @{0} — можно подбирать жильё!", user.Login);
                }
            }
            else
            {
                return RandomMessage(user.Login, formName);
            }
        }

        internal static string RandomMessage(string userLogin, string formName)
        {
            Random rand = new();
            int r = rand.Next(0, 4);

            switch (r)
            {
                case 0:
                    return String.Format("Это Match! Пользователь @{0} тоже прошёл анкету для поиска «{1}», и алгоритм считает, что вы подходите друг другу.", userLogin, formName);
                case 1:
                    return String.Format("Новый Match! У тебя новый матч @{0} по анкете «{1}». Поздравляю!", userLogin, formName);
                case 2:
                    return String.Format("И это Match! Помнишь анкету «{0}»? Поздравляю, у тебя новый матч, и это @{1}.", formName, userLogin);
                case 3:
                    return String.Format("@{0} подходит тебе по всем параметрам (указанным в анкете «{1}»), пора знакомиться!", userLogin, formName);
                default:
                    return String.Format("@{0} подходит тебе по всем параметрам (указанным в анкете «{1}»), пора знакомиться!", userLogin, formName);
            }
        }
    }


    internal class Message
    {
        public long UserId { get; set; }
        public string MessageText { get; set; }
        public int MatchId { get; set; }
    }
}
