import { useNavigate, useParams } from "react-router-dom";
import { useExams } from "../../Providers/ExamsProvider";
import { Exam } from "../../Models/Exam";
import {
  Stack,
  Typography,
  Box,
  Divider,
  Paper,
  Fab,
  Zoom,
} from "@mui/material";
import { QuestionCreate } from "../../Models/QuestionCreate";
import { useEffect, useState } from "react";
import CreateQuestionModal from "./CreateQuestionModal";
import DashboardPaper from "../Papers/DashboardPaper";
import LoadingPage from "../Extra/LoadingPage";
import AddIcon from "@mui/icons-material/Add";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import PrimaryFab from "../Buttons/PrimaryFab";
import QuestionService from "../../Services/QuestionService";

const ExamEditPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const { examinatorExams, questions, fetchQuestions } = useExams();
  const [showQuestionModal, setShowQuestionModal] = useState(false);
  const navigate = useNavigate();
  useEffect(() => {
    if (id) {
      fetchQuestions(id);
    }
  }, []);

  const handleSaveQuestion = async (question: QuestionCreate) => {
    try {
      const result = await QuestionService.createQuestion(question);
      await fetchQuestions(question.examId);
    } catch (error) {
      console.error("Error creating question:", error);
    } finally {
      setShowQuestionModal(false);
    }
  };

  const exam: Exam | undefined = examinatorExams?.find((e) => e?.examId === id);

  if (!exam) {
    return <LoadingPage />;
  }

  return (
    <>
      <CreateQuestionModal
        open={showQuestionModal}
        onClose={() => setShowQuestionModal(false)}
        onSave={handleSaveQuestion}
        examId={exam?.examId || ""}
      />
      <DashboardPaper sx={{ height: "calc(100vh - 100px)", overflowY: "auto" }}>
        <Stack spacing={4} mt={2} px={3} pb={10}>
          <Typography variant="h6" fontWeight={700}>
            Edit Exam: {exam?.name}
          </Typography>

          <Box display="grid" gridTemplateColumns="1fr 1fr" gap={2}>
            <Typography>
              <strong>Start Date:</strong> {exam?.startDate}
            </Typography>
            <Typography>
              <strong>End Date:</strong> {exam?.endDate}
            </Typography>
            <Typography>
              <strong>Questions:</strong> {exam?.questionCount}
            </Typography>
            <Typography>
              <strong>Participants:</strong> {exam?.participantCount}
            </Typography>
            <Typography>
              <strong>Status:</strong> {exam?.status}
            </Typography>
            <Typography>
              <strong>Join Code:</strong> {exam?.joinCode}
            </Typography>
          </Box>

          <Divider />

          <Box>
            <Typography variant="subtitle1" fontWeight={600} mb={2}>
              Questions ({questions?.length})
            </Typography>
            <Stack spacing={2}>
              {questions?.map((q, i) => (
                <Paper key={i} sx={{ p: 2 }}>
                  <Typography variant="subtitle1" fontWeight={500}>
                    {i + 1}. {q.questionText}
                  </Typography>
                  <Stack component="ul" pl={2} spacing={0.5}>
                    {q.options.map((option, idx) => (
                      <li key={idx}>
                        <Typography variant="body2">
                          {option?.label}: {option?.optionText}
                        </Typography>
                      </li>
                    ))}
                  </Stack>
                </Paper>
              ))}
              {questions?.length === 0 && (
                <Typography color="text.secondary">
                  No questions created yet.
                </Typography>
              )}
            </Stack>
          </Box>
        </Stack>
      </DashboardPaper>
      <Zoom in>
        <Box
          position="fixed"
          bottom={24}
          left={"calc(var(--dashboard-navigation-width) + 24px)"}
          zIndex={1300}
        >
          <PrimaryFab
            size="small"
            variant="extended"
            onClick={() => navigate(-1)}
          >
            <ArrowBackIcon sx={{ mr: 1 }} />
            Go Back
          </PrimaryFab>
        </Box>
      </Zoom>
      <Zoom in>
        <Box position="fixed" bottom={24} right={24} zIndex={1300}>
          <PrimaryFab
            size="small"
            variant="extended"
            onClick={() => setShowQuestionModal(true)}
          >
            <AddIcon sx={{ mr: 1 }} />
            Add Question
          </PrimaryFab>
        </Box>
      </Zoom>
    </>
  );
};

export default ExamEditPage;
