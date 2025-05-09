import React, { useEffect, useState } from "react";
import { useExamSession } from "../../Providers/ExamSessionProvider";
import LoadingPage from "../Extra/LoadingPage";
import ExamSessionQuestionBody from "./ExamSessionQuestionBody";
import { Question } from "../../Models/Question";

const ExamSession: React.FC = () => {
  const {
    studentExam,
    notCompletedQuestions,
    isQuestionsLoading,
    makeOpenAnswer,
    makeOptionAnswer,
    fetchNotCompletedQuestions,
  } = useExamSession();

  const [currentQuestion, setCurrentQuestion] = useState<Question | null>(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    if (notCompletedQuestions && notCompletedQuestions.length > 0) {
      pickRandomQuestion(notCompletedQuestions);
    }
  }, [notCompletedQuestions]);

  const pickRandomQuestion = (questions: Question[]) => {
    const randomIndex = Math.floor(Math.random() * questions.length);
    setCurrentQuestion(questions[randomIndex]);
  };

  const handleSubmitAnswer = async (answer: {
    text?: string;
    optionId?: string;
  }) => {
    if (!currentQuestion) return;
    setIsSubmitting(true);

    try {
      if (currentQuestion.type === "Text" && answer.text) {
        await makeOpenAnswer(currentQuestion.questionId, answer.text);
      } else if (answer.optionId) {
        await makeOptionAnswer(currentQuestion.questionId, answer.optionId);
      }

      await fetchNotCompletedQuestions(); // Оновлюємо список після відповіді
    } catch (err) {
      console.error("Answer submission failed:", err);
    } finally {
      setIsSubmitting(false);
    }
  };

  useEffect(() => {
    if (
      !isSubmitting &&
      notCompletedQuestions &&
      notCompletedQuestions.length > 0
    ) {
      pickRandomQuestion(notCompletedQuestions);
    }
  }, [isSubmitting, notCompletedQuestions]);

  if (!studentExam || isQuestionsLoading || !notCompletedQuestions) {
    return <LoadingPage />;
  }

  if (notCompletedQuestions.length === 0) {
    return (
      <div style={{ textAlign: "center", marginTop: "2rem" }}>
        <h2>Exam is completed ✅</h2>
      </div>
    );
  }

  return (
    <div style={{ padding: "2rem" }}>
      {currentQuestion && (
        <ExamSessionQuestionBody
          question={currentQuestion}
          onSubmit={handleSubmitAnswer}
          isSubmitting={isSubmitting}
        />
      )}
    </div>
  );
};

export default ExamSession;
