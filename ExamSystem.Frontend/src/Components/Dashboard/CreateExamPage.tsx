import { useState } from "react";
import { Button, Stack, Typography } from "@mui/material";
import CreateExamModal from "./CreateExamModal";
import CreateQuestionModal from "./CreateQuestionModal";
import { QuestionCreate } from "../../Models/QuestionCreate";
import { ExamCreate } from "../../Models/ExamCreate";
import { Exam } from "../../Models/Exam";
import DashboardPaper from "../Papers/DashboardPaper";
import ExamService from "../../Services/ExamService";
import { useNavigate } from "react-router-dom";

const CreateExamPage: React.FC = () => {
  const [showExamModal, setShowExamModal] = useState(true);
  const [isCreated, setIsCreated] = useState(false);
  const [exam, setExam] = useState<Exam | null>(null);
  const [questions, setQuestions] = useState<QuestionCreate[]>([]);
  const [showQuestionModal, setShowQuestionModal] = useState(false);
  const navigate = useNavigate();

  const handleCreateExam = async (data: ExamCreate) => {
    const examResult = await ExamService.createExam(data);
    if (!examResult) {
      setIsCreated(false);
      setExam(null);
      navigate("/dashboard/exam-management");
      return;
    }
    setIsCreated(true);
    setExam(examResult);
  };

  const handleSaveQuestion = (question: QuestionCreate) => {
    setQuestions((prev) => [...prev, question]);
    // TODO: API call here
  };

  return (
    <>
      <CreateExamModal
        open={showExamModal}
        onClose={() => setShowExamModal(false)}
        onCreate={handleCreateExam}
      />

      <CreateQuestionModal
        open={showQuestionModal}
        onClose={() => setShowQuestionModal(false)}
        onSave={handleSaveQuestion}
        examId={exam?.examId || ""}
      />

      {isCreated && (
        <DashboardPaper>
          <Stack spacing={3} mt={4} px={4}>
            <Typography variant="subtitle1" fontWeight={600}>
              Exam: {exam?.name}
            </Typography>
            <Typography variant="subtitle1" fontWeight={600}>
              Start Date: {exam?.startDate}
            </Typography>
            <Typography variant="subtitle1" fontWeight={600}>
              End Date: {exam?.endDate}
            </Typography>
            <Typography variant="subtitle1" fontWeight={600}>
              Questions: {questions.length}
            </Typography>
            <Typography variant="subtitle1" fontWeight={600}>
              Participants: {exam?.participantCount}
            </Typography>
            <Typography variant="subtitle1" fontWeight={600}>
              Status: {exam?.status}
            </Typography>
            <Typography variant="subtitle1" fontWeight={600}>
              Join Code: {exam?.joinCode}
            </Typography>

            <Button
              variant="contained"
              onClick={() => setShowQuestionModal(true)}
            >
              Add Question
            </Button>

            <Stack spacing={2}>
              {questions.map((q, i) => (
                <div key={i}>
                  <strong>{q.questionText}</strong> ({q.type}) —{" "}
                  {q.options.length} options
                </div>
              ))}
            </Stack>
          </Stack>
        </DashboardPaper>
      )}
    </>
  );
};

export default CreateExamPage;
