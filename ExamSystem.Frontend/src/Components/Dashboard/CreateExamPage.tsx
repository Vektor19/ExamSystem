import { useState } from "react";
import { Button, Stack, Typography } from "@mui/material";
import CreateExamModal from "./CreateExamModal";
import CreateQuestionModal from "./CreateQuestionModal";
import { QuestionCreate } from "../../Models/QuestionCreate";
import { ExamCreate } from "../../Models/ExamCreate";

const CreateExamPage: React.FC = () => {
  const [showExamModal, setShowExamModal] = useState(true);
  const [isCreated, setIsCreated] = useState(false);
  const [examId, setExamId] = useState("");
  const [questions, setQuestions] = useState<QuestionCreate[]>([]);
  const [showQuestionModal, setShowQuestionModal] = useState(false);

  const handleCreateExam = (data: ExamCreate) => {
    // TODO: Replace with actual API call
    const newExamId = crypto.randomUUID(); // simulate created exam id
    setExamId(newExamId);
    setIsCreated(true);
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
        examId={examId}
      />

      {isCreated && (
        <Stack spacing={3} mt={4} px={4}>
          <Typography variant="h5">Add Questions</Typography>

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
      )}
    </>
  );
};

export default CreateExamPage;
