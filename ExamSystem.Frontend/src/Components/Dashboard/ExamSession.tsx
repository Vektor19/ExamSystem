import React, { useEffect, useState } from "react";
import { useExamSession } from "../../Providers/ExamSessionProvider";
import LoadingPage from "../Extra/LoadingPage";
import ExamSessionQuestionBody from "./ExamSessionQuestionBody";
import { Question } from "../../Models/Question";
import { useNavigate } from "react-router";
import examsStyles from "../../Styles/Exams.module.css";
import { useNotification } from "../../Providers/NotificationProvider";
import { useAntiCheating } from "../../Providers/AntiCheatingProvider";
import BlockingModal from "./BlockingModal";

const ExamSession: React.FC = () => {
  const {
    studentExam,
    notCompletedQuestions,
    isQuestionsLoading,
    makeOpenAnswer,
    makeOptionAnswer,
    fetchNotCompletedQuestions,
  } = useExamSession();
  const { violations } = useAntiCheating();
  const [currentQuestion, setCurrentQuestion] = useState<Question | null>(null);
  const [showBlockingModal, setShowBlockingModal] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const navigate = useNavigate();
  const { showNotification } = useNotification();

  useEffect(() => {
    if (studentExam?.examUser.isBlocked) {
      showNotification(
        "You are blocked from this exam due to suspicious activity.",
        "error"
      );
      setShowBlockingModal(true);
    }
  }, [studentExam]);

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
    optionIds?: string[];
  }) => {
    if (!currentQuestion) return;
    setIsSubmitting(true);

    try {
      if (currentQuestion.type === "Text" && answer.text) {
        await makeOpenAnswer(currentQuestion.questionId, answer.text);
      } else if (answer.optionIds) {
        for (const optionId of answer.optionIds) {
          await makeOptionAnswer(currentQuestion.questionId, optionId);
        }
      }
      await fetchNotCompletedQuestions();
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
    showNotification("You finished the exam!", "success");
    navigate("/dashboard/exam-management");
  }

  return (
    <>
      <BlockingModal open={showBlockingModal} />
      <h1 className={`${examsStyles["exams-title"]}`}>{studentExam.name}</h1>
      {currentQuestion && (
        <ExamSessionQuestionBody
          question={currentQuestion}
          onSubmit={handleSubmitAnswer}
          isSubmitting={isSubmitting}
          isLastQuestion={notCompletedQuestions.length === 1}
        />
      )}
    </>
  );
};

export default ExamSession;
