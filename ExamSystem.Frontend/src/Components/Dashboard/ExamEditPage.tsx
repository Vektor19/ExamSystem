import { useNavigate, useParams } from "react-router-dom";
import { useExams } from "../../Providers/ExamsProvider";
import {
  Stack,
  Typography,
  Box,
  Divider,
  Paper,
  Zoom,
  IconButton,
  TextField,
  Select,
  MenuItem,
} from "@mui/material";
import { useEffect, useState } from "react";
import CreateQuestionModal from "./CreateQuestionModal";
import DashboardPaper from "../Papers/DashboardPaper";
import LoadingPage from "../Extra/LoadingPage";
import AddIcon from "@mui/icons-material/Add";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import EditIcon from "@mui/icons-material/Edit";
import CheckIcon from "@mui/icons-material/Check";
import CloseIcon from "@mui/icons-material/Close";
import DeleteIcon from "@mui/icons-material/Delete";
import PrimaryFab from "../Buttons/PrimaryFab";
import QuestionService from "../../Services/QuestionService";
import ExamService from "../../Services/ExamService";
import AddParticipantModal from "./AddParticipantModal";
import { ExaminatorExam } from "../../Models/ExaminatorExam";
import { Participant } from "../../Models/Participant";

const statusOptions = ["NotStarted", "Started", "Finished"];

const ExamEditPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const { examinatorExams, questions, fetchQuestions, fetchExaminatorExams } =
    useExams();
  const [showQuestionModal, setShowQuestionModal] = useState(false);
  const [showParticipantModal, setShowParticipantModal] = useState(false);
  const [editingField, setEditingField] = useState<string | null>(null);
  const [formValues, setFormValues] = useState({
    name: "",
    startDate: "",
    endDate: "",
    status: "",
  });

  const [participants, setParticipants] = useState<Participant[]>([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (id) {
      fetchQuestions(id);
    }
  }, []);

  const exam: ExaminatorExam | undefined = examinatorExams?.find((e) => e?.examId === id);

  useEffect(() => {
    if (exam) {
      setFormValues({
        name: exam.name,
        startDate: exam.startDate,
        endDate: exam.endDate,
        status: exam.status,
      });
      setParticipants(exam.participants || []);
    }
  }, [exam]);

  const handleSaveField = async (field: keyof typeof formValues) => {
    if (!exam) return;
    try {
      await ExamService.updateExam(exam.examId, {
        name: formValues.name,
        startDate: new Date(formValues.startDate).toISOString(),
        endDate: new Date(formValues.endDate).toISOString(),
        status: formValues.status,
      });
      await fetchExaminatorExams();
    } catch (error) {
      console.error("Failed to update exam:", error);
    } finally {
      setFormValues(exam);
      setEditingField(null);
    }
  };

  const renderField = (
    label: string,
    field: keyof typeof formValues,
    isDate = false,
    isSelect = false
  ) => {
    const value = formValues[field];
    const isEditing = editingField === field;
    return (
      <Box display="flex" alignItems="center" gap={1}>
        <strong>{label}:</strong>
        {isEditing ? (
          isSelect ? (
            <Select
              size="small"
              value={value}
              onChange={(e) =>
                setFormValues((prev) => ({ ...prev, [field]: e.target.value }))
              }
            >
              {statusOptions.map((status) => (
                <MenuItem key={status} value={status}>
                  {status}
                </MenuItem>
              ))}
            </Select>
          ) : (
            <TextField
              size="small"
              type={isDate ? "date" : "text"}
              value={isDate ? value.slice(0, 10) : value}
              onChange={(e) =>
                setFormValues((prev) => ({ ...prev, [field]: e.target.value }))
              }
            />
          )
        ) : (
          <Typography>{exam?.[field]}</Typography>
        )}
        {isEditing ? (
          <>
            <IconButton
              onClick={() => handleSaveField(field)}
              size="small"
              color="success"
            >
              <CheckIcon />
            </IconButton>
            <IconButton
              onClick={() => {
                setEditingField(null);
                setFormValues(
                  exam ?? { name: "", startDate: "", endDate: "", status: "" }
                );
              }}
              size="small"
              color="error"
            >
              <CloseIcon />
            </IconButton>
          </>
        ) : (
          <IconButton onClick={() => setEditingField(field)} size="small">
            <EditIcon />
          </IconButton>
        )}
      </Box>
    );
  };

  if (!exam) {
    return <LoadingPage />;
  }

  return (
    <>
      <CreateQuestionModal
        open={showQuestionModal}
        onClose={() => setShowQuestionModal(false)}
        onSave={async (question) => {
          await QuestionService.createQuestion(question);
          await fetchQuestions(question.examId);
          setShowQuestionModal(false);
        }}
        examId={exam?.examId || ""}
      />

      <AddParticipantModal
        open={showParticipantModal}
        onClose={() => setShowParticipantModal(false)}
        onSave={async (participantEmail: string) => {
          if (!id) return;
          try {
            await ExamService.addParticipantToExamByEmail(id, participantEmail);
            await fetchExaminatorExams();
          } catch (error) {
            console.error("Failed to add participant:", error);
          }
        }}
      />

      <DashboardPaper sx={{ height: "calc(100vh - 100px)", overflowY: "auto" }}>
        <Stack spacing={4} mt={2} px={3} pb={10}>
          <Typography variant="h6" fontWeight={700}>
            Edit Exam: {exam?.name}
          </Typography>

          <Box display="grid" gridTemplateColumns="1fr 1fr" gap={2}>
            {renderField("Name", "name")}
            {renderField("Start Date", "startDate", true)}
            {renderField("End Date", "endDate", true)}
            <Typography>
              <strong>Questions:</strong> {exam?.questionCount}
            </Typography>
            <Typography>
              <strong>Participants:</strong> {exam?.participantCount}
            </Typography>
            {renderField("Status", "status", false, true)}
            <Typography>
              <strong>Join Code:</strong> {exam?.joinCode}
            </Typography>
          </Box>

          <Divider />

          <Box display="grid" gridTemplateColumns="1fr 1fr" gap={4}>
            {/* Questions Section (Left) */}
            <Box>
              <Box
                display="flex"
                justifyContent="space-between"
                alignItems="center"
                mb={2}
              >
                <Typography variant="subtitle1" fontWeight={600}>
                  Questions ({questions?.length})
                </Typography>
                <PrimaryFab
                  size="small"
                  variant="extended"
                  onClick={() => setShowQuestionModal(true)}
                >
                  <AddIcon sx={{ mr: 1 }} />
                  Add Question
                </PrimaryFab>
              </Box>
              <Stack spacing={2}>
                {questions?.map((q, i) => (
                  <Paper
                    key={i}
                    sx={{
                      p: 2,
                      display: "flex",
                      justifyContent: "space-between",
                      alignItems: "flex-start",
                    }}
                  >
                    <Box>
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
                    </Box>
                    <IconButton
                      color="error"
                      onClick={async () => {
                        if (!id) return;
                        try {
                          await QuestionService.deleteQuestion(q.questionId);
                          await fetchQuestions(id);
                        } catch (err) {
                          console.error("Failed to delete question:", err);
                        }
                      }}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </Paper>
                ))}
                {(questions?.length === 0 || !questions) && (
                  <Typography color="text.secondary">
                    No questions created yet.
                  </Typography>
                )}
              </Stack>
            </Box>

            {/* Participants Section (Right) */}
            <Box>
              <Box
                display="flex"
                justifyContent="space-between"
                alignItems="center"
                mb={2}
              >
                <Typography variant="subtitle1" fontWeight={600}>
                  Participants ({participants.length})
                </Typography>
                <PrimaryFab
                  size="small"
                  variant="extended"
                  onClick={() => {
                    setShowParticipantModal(true);
                  }}
                >
                  <AddIcon />
                  Add Participant
                </PrimaryFab>
              </Box>
              <Stack spacing={2}>
                {participants.map((p, i) => (
                  <Paper
                    key={p.userId}
                    sx={{
                      p: 2,
                      display: "flex",
                      justifyContent: "space-between",
                      alignItems: "center",
                    }}
                  >
                    <Box>
                      <Typography variant="subtitle1" fontWeight={500}>
                        {i + 1}. {p.firstName} ({p.email})
                      </Typography>
                    </Box>
                    <IconButton color="error">
                      <DeleteIcon />
                    </IconButton>
                  </Paper>
                ))}
                {participants.length === 0 && (
                  <Typography color="text.secondary">
                    No participants added yet.
                  </Typography>
                )}
              </Stack>
            </Box>
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
