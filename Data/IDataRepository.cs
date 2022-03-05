using QandA.Models;
namespace QandA.Data
{
    public interface IDataRepository
    {
        #region Read Services
        IEnumerable<QuestionGetManyResponse> GetQuestions();
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search);
        IEnumerable<QuestionGetManyResponse> GetUnansweredQuestions();
        QuestionGetSingleResponse GetQuestion(int questionId);
        bool QuestionExists(int questionId);
        AnswerGetResponse GetAnswer(int answerId);
        #endregion Read Services

        #region Write Services
        QuestionGetSingleResponse PostQuestion(QuestionPostRequest question);
        QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest
        question);
        void DeleteQuestion(int questionId);
        AnswerGetResponse PostAnswer(AnswerPostRequest answer);
        #endregion Write Services
    }
}
